using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenGames.SheetMachine
{
    public enum CellType
    {
        // Undefined is not extract.
        Undefined,
        String,
        Short,
        Int,
        Long,
        Float,
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

    public class MemberFieldData
    {
        public CellType type = CellType.Undefined;
        public string Type => TypeEnumToString(type);
        public string name;

        public static string TypeEnumToString(CellType cellType)
        {
            switch (cellType)
            {
                case CellType.String:
                    return "string";
                case CellType.Short:
                    return "short";
                case CellType.Int:
                    return "int";
                case CellType.Long:
                    return "long";
                case CellType.Float:
                    return "float";
                case CellType.Double:
                    return "double";
                case CellType.Enum:
                    return "enum";
                case CellType.Bool:
                    return "bool";
                default:
                    return "string";
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
                    string type = header.type.ToString().ToLower();

                    // Property 제외
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
