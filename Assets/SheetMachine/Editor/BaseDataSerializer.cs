using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace ChickenGames.SheetMachine
{
    public abstract class BaseDataSerializer
    {
        protected object ParseCellData<T>(HeaderRowInfo headerRowInfo, string cellData)
        {
            var type = headerRowInfo.field.FieldType;
            object value = new object();

            if (type != typeof(string) && type != typeof(string[]))
                cellData = cellData.Replace(" ", string.Empty);

            if (type.IsArray)
            {
                const char DELIMETER = ','; // '\n'

                if (type.GetElementType().IsEnum)
                {
                    var values = cellData.Split(DELIMETER)
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
                    if (string.IsNullOrEmpty(cellData)) 
                        value = null;
                    else
                    {
                        var values = cellData.Split(DELIMETER);

                        Array array = (Array)Activator.CreateInstance(type, values.Length);

                        for (int k = 0; k < values.Length; k++)
                        {
                            var v = values[k];
                            if (v == "" && type != typeof(string))
                                v = "0";
                            var temp = Convert.ChangeType(v, type.GetElementType());
                            array.SetValue(temp, k);
                        }

                        value = array;
                    }
                }
            }
            else
            {
                if (type.IsEnum)
                {
                    value = Enum.Parse(type, cellData);
                }
                else
                {
                    if (type == typeof(int))
                        cellData = cellData.Split('.')[0];

                    if (cellData == "" && type != typeof(string))
                        cellData = "0";
                    
                    value = Convert.ChangeType(cellData, type);
                }
            }

            return value;
        }

        protected List<HeaderRowInfo> GetHeaderRowInfos<T>(List<string> headerNames)
        {
            Type t = typeof(T);

            List<HeaderRowInfo> headerRowInfo = new List<HeaderRowInfo>();
            var headerRow = headerNames;

            for (int i = 0; i < headerRow.Count; i++)
            {
                var header = headerRow[i];

                var field = t.GetField(header, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (field != null)
                {
                    headerRowInfo.Add(new HeaderRowInfo
                    {
                        index = i,
                        field = field,
                    });
                }
            }

            return headerRowInfo;
        }

        protected class HeaderRowInfo
        {
            public int index;
            public FieldInfo field;
        }
    }
}