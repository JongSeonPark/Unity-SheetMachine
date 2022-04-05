using ChickenGames.SheetMachine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ChickenGames.SheetMachine.GoogleSheet
{
    [CustomEditor(typeof(GoogleMachine))]
    public class GoogleMachineEditor : BaseMachineEditor
    {

        private void OnEnable()
        {
            machine = target as GoogleMachine;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Script Path Settings:", new GUIStyle(GUI.skin.label));
            EditorGUILayout.HelpBox(
                "Sheet의 Id 부분입니다.\n" +
                "ex) https://docs.google.com/spreadsheets/d/(이부분)/edit#gid=0\n" +
                "(이부분)을 복사해서 넣어주시면 됩니다.",
                MessageType.Info, true);

            
            GUIHelper.DrawTextField(ref machine.spreadSheetName, "SpreadSheet Id: ", labelWidth);
            GUIHelper.DrawTextField(ref machine.sheetName, "Sheet Name: ", labelWidth);

            OnAfterDraw();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(GoogleDataSettings.Instance);
                EditorUtility.SetDirty(machine);
            }
        }

        protected override void Import()
        {
            var sheetRanges = new List<string>()
            {
                $"{machine.sheetName}!{1}:{1}" // header,
            };

            int typeRangeIndex = 0, arrayRangeIndex = 0;

            if (machine.includeTypeRow)
            {
                typeRangeIndex = sheetRanges.Count;
                sheetRanges.Add($"{machine.sheetName}!{machine.typeRowIndex + 1}:{machine.typeRowIndex + 1}"); // typeRow
            }
            if (machine.includeIsArrayRow)
            {
                arrayRangeIndex = sheetRanges.Count;
                sheetRanges.Add($"{machine.sheetName}!{machine.arrayRowIndex + 1}:{machine.arrayRowIndex + 1}"); // ArrayRow
            }


            var batchGetReq = GoogleDataSettings.Instance.Service.Spreadsheets.Values.BatchGet(machine.spreadSheetName);
            batchGetReq.Ranges = sheetRanges;
            var batchGetRes = batchGetReq.Execute();

            List<ColumnHeader> tmpColumnList = new List<ColumnHeader>();

            var headerRow = batchGetRes.ValueRanges[0].Values[0];
            for (int i = 0; i < headerRow.Count; i++)
            {
                string headerName = (string)headerRow[i];
                string typeStr = null;
                bool isArray = false;
                if (machine.includeTypeRow)
                {
                    var typeRow = batchGetRes.ValueRanges[typeRangeIndex].Values[0];
                    typeStr = typeRow.Count > i ? (string)typeRow[i] : null;
                }
                if (machine.includeIsArrayRow)
                {
                    var isArrayRow = batchGetRes.ValueRanges[arrayRangeIndex].Values[0];
                    isArray = isArrayRow.Count > i ? (bool)Convert.ChangeType((string)isArrayRow[i], TypeCode.Boolean) : false;
                }
                ColumnHeader column = new ColumnHeader(headerName, isArray, typeStr);

                tmpColumnList.Add(column);
            }

            machine.columnHeaderList = tmpColumnList;


            if (string.IsNullOrEmpty(machine.dataClassName)) machine.dataClassName = machine.sheetName;

            EditorUtility.SetDirty(machine);
            AssetDatabase.SaveAssets();
        }

        protected override void Generate()
        {
            ScriptsGenerator.Generate(PathMethods.Combine(Application.dataPath, machine.templatePath, "ScriptableObjectClass.txt"),
                new ScriptPrescription
                {
                    className = "classNama",
                    dataClassName = "dataClassName",
                    spreadsheetName = "spreadsheetName",
                });
            //using (var writer = new StreamWriter("Path"))
            //{
            //    writer.Write("value");
            //    writer.Close();
            //}
        }
    }
}
