using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace ChickenGames.SheetMachine.GoogleSheet
{
    public class GoogleDataSerializer : BaseDataSerializer
    {
        public T[] Deserialize<T>(IList<IList<object>> rows, int dataRowStartIndex)
        {
            Type t = typeof(T);
            List<T> r = new List<T>();

            var headerRow = rows[0];
            List<HeaderRowInfo> headerRowInfo = GetHeaderRowInfos<T>(rows[0].Select(cell => (string)cell).ToList());

            for (int j = dataRowStartIndex; j < rows.Count; j++)
            {
                T inst = (T)Activator.CreateInstance(t);
                var row = rows[j];

                for (int i = 0; i < headerRowInfo.Count; i++)
                {
                    var headerInfo = headerRowInfo[i];
                    var cell = row.Count > headerInfo.index ? (string)row[headerInfo.index] : "";
                    object value = ParseCellData<T>(headerInfo, cell);
                    
                    headerInfo.field.SetValue(inst, value);
                }
                r.Add(inst);
            }
            return r.ToArray();
        }
    }
}