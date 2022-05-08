using ChickenGames.SheetMachine.Utils;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChickenGames.SheetMachine.GoogleSheet
{
    public class GoogleMachine : BaseMachine
    {
        [MenuItem("Assets/Create/SheetMachine/GoogleMachine")]
        public static void Create()
        {
            GoogleMachine asset = CreateInstance<GoogleMachine>();

            asset.templatePath = GoogleDataSettings.Instance.templatePath;
            asset.editorClassPath = GoogleDataSettings.Instance.editorClassPath;
            asset.runtimeClassPath = GoogleDataSettings.Instance.runtimeClassPath;

            var path = PathMethods.Combine(AssetDatabase.GetAssetPath(Selection.activeObject), "GoogleMachine.asset");
            AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(path));
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(asset);
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }


        public override void Import()
        {
            Debug.Log("Import");

            if (string.IsNullOrEmpty(spreadSheetName) || string.IsNullOrEmpty(sheetName))
            {
                string msg = "";
                if (string.IsNullOrEmpty(spreadSheetName))
                    msg += $"sheetName is empty. {spreadSheetName}\n";
                if (string.IsNullOrEmpty(sheetName))
                    msg += $"sheetName is empty. {sheetName}\n";

                EditorUtility.DisplayDialog("Error", msg, "OK");
                return;
            }

            var sheetRanges = new List<string>()
            {
                $"{sheetName}!{1}:{1}" // header,
            };

            int typeRangeIndex = 0;

            if (includeTypeRow)
            {
                typeRangeIndex = sheetRanges.Count;
                sheetRanges.Add($"{sheetName}!{typeRowIndex + 1}:{typeRowIndex + 1}"); // typeRow
            }

            var batchGetReq = GoogleDataSettings.Instance.Service.Spreadsheets.Values.BatchGet(spreadSheetName);
            batchGetReq.Ranges = sheetRanges;
            var batchGetRes = batchGetReq.Execute();

            List<ColumnHeader> tmpColumnList = new List<ColumnHeader>();

            var headerRow = batchGetRes.ValueRanges[0].Values[0];
            var typeRow = includeTypeRow ? batchGetRes.ValueRanges[typeRangeIndex].Values[0] : null;
            for (int i = 0; i < headerRow.Count; i++)
            {
                string headerName = (string)headerRow[i];
                string typeStr = includeTypeRow && typeRow.Count > i ? (string)typeRow[i] : null;
                ColumnHeader column = new ColumnHeader(headerName, typeStr);
                tmpColumnList.Add(column);
            }
            columnHeaderList = tmpColumnList;
            if (string.IsNullOrEmpty(className)) className = sheetName;
        }

        public override void Generate()
        {
            Debug.Log("Generate");

            Dictionary<string, string> scriptPrescription = new Dictionary<string, string>()
            {
                { "ClassName", className },
                { "DataClassName", className + "Data" },
                { "SpreadsheetName", spreadSheetName },
                { "WorksheetName", sheetName },
                { "DataStartRowIndex", DataStartRowIndex.ToString() },
                { "MemberFields", MemberFieldsString },
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
        }
    }
}
