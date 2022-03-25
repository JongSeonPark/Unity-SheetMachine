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
