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

    [System.Serializable]
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

        public ColumnHeader(string name, bool isArray, string typeStr = null)
        {
            this.name = name.Replace(" ", string.Empty);
            this.isArray = isArray;

            type = CellType.Undefined;
            if (typeStr != null)
                Enum.TryParse(typeStr, true, out type);
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

    public class BaseMachine : ScriptableObject
    {
        /// <summary>
        /// SpreadSheetName is SpreadSheetId in Google
        /// </summary>
        public string spreadSheetName;
        public string sheetName;
        public int dataStartRowIndex;
        public bool includeTypeRow;
        public int typeRowIndex;
        public bool includeIsArrayRow;
        public int arrayRowIndex;
        public List<ColumnHeader> columnHeaderList;
        public string templatePath;
        public string dataClassTemplatePath;
        public string scriptableObjectClassPath;
        public string runtimeClassPath;
        public string editorClassPath;
        public string dataClassName;
    }
}
