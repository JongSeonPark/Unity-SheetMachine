using ChickenGames.SheetMachine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenGames.SheetMachine.GoogleSheet
{
    [CreateAssetMenu(fileName = "GoogleMachine ", menuName = "SheetMachine/GoogleMachine", order = 0)]
    public class GoogleMachine : BaseMachine
    {
        private void Awake()
        {
            Debug.Log("Awake");

            templatePath = GoogleDataSettings.Instance.templatePath;
            editorClassPath = GoogleDataSettings.Instance.editorClassPath;
            runtimeClassPath = GoogleDataSettings.Instance.runtimeClassPath;
        }

        public override void Import()
        {
            Debug.Log("Import");
            var sheetRanges = new List<string>()
            {
                $"{sheetName}!{1}:{1}" // header,
            };

            int typeRangeIndex = 0, arrayRangeIndex = 0;

            if (includeTypeRow)
            {
                typeRangeIndex = sheetRanges.Count;
                sheetRanges.Add($"{sheetName}!{typeRowIndex + 1}:{typeRowIndex + 1}"); // typeRow
            }
            if (includeIsArrayRow)
            {
                arrayRangeIndex = sheetRanges.Count;
                sheetRanges.Add($"{sheetName}!{arrayRowIndex + 1}:{arrayRowIndex + 1}"); // ArrayRow
            }


            var batchGetReq = GoogleDataSettings.Instance.Service.Spreadsheets.Values.BatchGet(spreadSheetName);
            batchGetReq.Ranges = sheetRanges;
            var batchGetRes = batchGetReq.Execute();

            ColumnHeaderList tmpColumnList = new ColumnHeaderList();

            var headerRow = batchGetRes.ValueRanges[0].Values[0];
            for (int i = 0; i < headerRow.Count; i++)
            {
                string headerName = (string)headerRow[i];
                string typeStr = null;
                bool isArray = false;
                if (includeTypeRow)
                {
                    var typeRow = batchGetRes.ValueRanges[typeRangeIndex].Values[0];
                    typeStr = typeRow.Count > i ? (string)typeRow[i] : null;
                }
                if (includeIsArrayRow)
                {
                    var isArrayRow = batchGetRes.ValueRanges[arrayRangeIndex].Values[0];
                    isArray = isArrayRow.Count > i ? (bool)Convert.ChangeType((string)isArrayRow[i], TypeCode.Boolean) : false;
                }
                ColumnHeader column = new ColumnHeader(headerName, isArray, typeStr);

                tmpColumnList.Add(column);
            }
            columnHeaderList = tmpColumnList;
            if (string.IsNullOrEmpty(className)) className = sheetName;
        }


        public override void Generate()
        {
            Debug.Log("Generate");

            //var sp = new ScriptPrescription
            //{
            //    ClassName = className,
            //    DataClassName = className + "Data",
            //    SpreadsheetName = spreadSheetName,
            //};

            //ScriptsGenerator.Generate(
            //    PathMethods.Combine(Application.dataPath, templatePath, "ScriptableObjectClass.txt"),
            //    PathMethods.Combine(Application.dataPath, runtimeClassPath, "ClassName.cs"),
            //    sp);


            // 포함된 특수한 Row 중에서 가장 큰 값을 DataStart의 시작 값으로 표현
            int dataStartRowIndex = Mathf.Max(
                1,
                includeTypeRow ? typeRowIndex + 1 : 0,
                includeIsArrayRow ? arrayRowIndex + 1 : 0);

            Dictionary<string, string> scriptPrescription = new Dictionary<string, string>()
            {
                { "ClassName", className },
                { "DataClassName", className + "Data" },
                { "SpreadsheetName", spreadSheetName },
                { "WorksheetName", sheetName },
                { "DataStartRowIndex", dataStartRowIndex.ToString() },
                { "MemberFields", columnHeaderList.ToString() },
            };

            ScriptsGenerator.Generate(
                txtFilePath: PathMethods.Combine(Application.dataPath, templatePath, "ScriptableObjectClass.txt"),
                createPath: PathMethods.Combine(Application.dataPath, runtimeClassPath, $"{scriptPrescription["ClassName"]}.cs"),
                sp: scriptPrescription);

            ScriptsGenerator.Generate(
                txtFilePath: PathMethods.Combine(Application.dataPath, templatePath, "DataClass.txt"),
                createPath: PathMethods.Combine(Application.dataPath, runtimeClassPath, $"{scriptPrescription["DataClassName"]}.cs"),
                sp: scriptPrescription);

            ScriptsGenerator.Generate(
                txtFilePath: PathMethods.Combine(Application.dataPath, templatePath, "ScriptableObjectEditorClass.txt"),
                createPath: PathMethods.Combine(Application.dataPath, editorClassPath, $"{scriptPrescription["DataClassName"]}Editor.cs"),
                sp: scriptPrescription);

            //ScriptsGenerator.Generate(
            //    PathMethods.Combine(Application.dataPath, templatePath, "ScriptableObjectClass.txt"),
            //    PathMethods.Combine(Application.dataPath, runtimeClassPath, $"{scriptPrescription["DataClassName"]}.cs"),
            //    scriptPrescription);

            //ScriptsGenerator.Generate(
            //    PathMethods.Combine(Application.dataPath, templatePath, "ScriptableObjectClass.txt"),
            //    PathMethods.Combine(Application.dataPath, runtimeClassPath, $"{scriptPrescription["DataClassName"]}.cs"),
            //    scriptPrescription);
        }
    }
}
