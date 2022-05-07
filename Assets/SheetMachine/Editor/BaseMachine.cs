using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChickenGames.SheetMachine
{
    public enum CellType
    {
        // Undefined is not extract.
        Undefined,
        String,
        Int,
        Float,
        Short,
        Long,
        Double,
        Enum,
        Bool,
    }

    [Serializable]
    public class ColumnHeader
    {
        public string name;
        public bool isArray;
        public CellType type;

        ColumnHeader(string name, bool isArray, CellType type)
        {
            this.name = name;
            this.isArray = isArray;
            this.type = type;
        }

        public ColumnHeader(string name, string typeStr = null)
        {
            this.name = name.Replace(" ", string.Empty);
            
            if (string.IsNullOrEmpty(typeStr))
            {
                type = CellType.Undefined;
            }
            else
            {
                isArray = typeStr.Contains("[]");
                var temp = isArray ? typeStr.Split(new string[] { "[]" }, StringSplitOptions.RemoveEmptyEntries)[0] : typeStr;
                Enum.TryParse(temp, true, out type);
            }
        }
    }

    public abstract class BaseMachine : ScriptableObject
    {
        /// <summary>
        /// SpreadSheetName is SpreadSheetId in Google
        /// SpreadSheetName is excelFilePath in Excel
        /// </summary>
        public string spreadSheetName;
        public string sheetName;
        public bool includeTypeRow;
        public int typeRowIndex;
        public List<ColumnHeader> columnHeaderList;
        public string templatePath;
        public string dataClassTemplatePath;
        public string scriptableObjectClassPath;
        public string runtimeClassPath;
        public string editorClassPath;
        public string className;

        // 포함된 특수한 Row 중에서 가장 큰 값을 DataStart의 시작 값으로 표현
        protected int DataStartRowIndex => Mathf.Max(
                1,
                includeTypeRow ? typeRowIndex + 1 : 0);

        protected string MemberFieldsString
        {
            get
            {
                string result = "\n";
                columnHeaderList.ForEach(header =>
                {
                    if (header.type == CellType.Undefined) return;

                    string type;

                    if (header.type == CellType.Enum)
                    {
                        type = header.name.First().ToString().ToUpper() + String.Join("", header.name.Skip(1));
                    }
                    else
                        type = header.type.ToString().ToLower();
                    string name = header.name;
                    string arr = header.isArray ? "[]" : "";
                    result +=
                    $"    public {type}{arr} {name};\n" +
                    $"\n";
                });
                return result;
            }
        }

        public abstract void Import();
        public abstract void Generate();
        
    }
}
