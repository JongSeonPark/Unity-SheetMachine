using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace ChickenGames.SheetMachine
{
    public abstract class BaseDataSerializer
    {
        protected object ParseCellData<T>(HeaderRowInfo headerRowInfo, string cellData)
        {
            var type = headerRowInfo.field.FieldType;
            object value = null;

            if (type != typeof(string) && type != typeof(string[]))
                cellData = cellData.Replace(" ", string.Empty);

            if (type.IsArray)
            {
                if (string.IsNullOrWhiteSpace(cellData))
                    return value;

                const char DELIMETER = ','; // '\n'
                var values = cellData.Split(DELIMETER).Where(v => !string.IsNullOrWhiteSpace(v)).ToArray();
                Array array = (Array)Activator.CreateInstance(type, values.Length);

                for (int k = 0; k < values.Length; k++)
                {
                    var v = values[k];
                    object data;
                    if (type.GetElementType().IsEnum)
                        data = Enum.Parse(type.GetElementType(), v);
                    else
                        data = Convert.ChangeType(v, type.GetElementType());
                    array.SetValue(data, k);
                }
                value = array;
            }
            else
            {
                if (type.IsEnum)
                {
                    if (!string.IsNullOrEmpty(cellData))
                    {
                        value = Enum.Parse(type, cellData);
                    }
                }
                else
                {
                    if (type == typeof(int))
                        cellData = cellData.Split('.')[0];

                    if (type == typeof(bool))
                        cellData = "false";

                    if (string.IsNullOrEmpty(cellData) && type != typeof(string))
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