using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace UnityQuickSheet
{
    public class Serializer
    {

        static public T[] Deserialize<T>(IList<IList<object>> rows, int dataRowStartIndex)
        {
            Type t = typeof(T);
            List<T> r = new List<T>();

            List<HeaderRowInfo> headerRowInfo = new List<HeaderRowInfo>();
            var headerRow = rows[0];

            for (int i = 0; i < headerRow.Count; i++)
            {
                var header = (string)headerRow[i];
                var property = t.GetProperty(header, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (property != null && property.CanRead)
                {
                    headerRowInfo.Add(new HeaderRowInfo
                    {
                        index = i,
                        property = property,
                    });
                }
            }

            PropertyInfo[] properties = t.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                if (property.CanWrite)
                {

                }
            }


            for (int j = dataRowStartIndex; j < rows.Count; j++)
            {
                T inst = (T)Activator.CreateInstance(t);
                var row = rows[j];

                for (int i = 0; i < headerRowInfo.Count; i++)
                {
                    var headerInfo = headerRowInfo[i];
                    var cell = (string)row[headerInfo.index];
                    var type = headerInfo.property.PropertyType;
                    object value = new object();


                    if (type.IsArray)
                    {
                        const char DELIMETER = ','; // '\n'

                        if (type.GetElementType().IsEnum)
                        {
                            var values = cell.Split(DELIMETER)
                                .Select(s => s.Replace(" ", string.Empty))
                                .Select(i => Enum.Parse(type.GetElementType(), i))
                                .ToArray();

                            Array array = (Array)Activator.CreateInstance(type, values.Length);

                            for (int k = 0; k < values.Length; k++)
                            {
                                array.SetValue(values[k], k);
                            }

                            value = array;
                        }
                        else
                        {
                            var values = cell.Split(DELIMETER)
                                .Select(s => s.Replace(" ", string.Empty))
                                .ToArray();

                            Array array = (Array)Activator.CreateInstance(type, values.Length);

                            for (int k = 0; k < values.Length; k++)
                            {
                                var temp = Convert.ChangeType(values[k], type.GetElementType());
                                array.SetValue(temp, k);
                            }

                            value = array;
                        }
                    }
                    else
                    {
                        if (type.IsEnum)
                        {
                            value = Enum.Parse(type, cell.Replace(" ", string.Empty));
                        }
                        else
                        {
                            value = Convert.ChangeType(cell, type);
                        }

                    }

                    headerInfo.property.SetValue(inst, value);


                }
                r.Add(inst);
            }
            return r.ToArray();
        }

        class HeaderRowInfo
        {
            public int index;
            public PropertyInfo property;
        }
    }
}