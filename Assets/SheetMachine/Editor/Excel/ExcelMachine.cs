using ChickenGames.SheetMachine.Utils;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ChickenGames.SheetMachine.ExcelSheet
{
    public class ExcelMachine : BaseMachine
    {


        [MenuItem("Assets/Create/SheetMachine/ExcelMachine")]
        public static void Create()
        {
            ExcelMachine asset = CreateInstance<ExcelMachine>();

            asset.templatePath = ExcelDataSettings.Instance.templatePath;
            asset.editorClassPath = ExcelDataSettings.Instance.editorClassPath;
            asset.runtimeClassPath = ExcelDataSettings.Instance.runtimeClassPath;

            var path = PathMethods.Combine(AssetDatabase.GetAssetPath(Selection.activeObject), "ExcelMachine.asset");
            AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(path));
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(asset);
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }


        public override void Import()
        {
            Debug.Log("Import");
        
            string path = PathMethods.Combine(Application.dataPath, spreadSheetName);
            string sheet = sheetName;

            if (!File.Exists(path) || string.IsNullOrEmpty(sheet))
            {
                string msg = "";
                if (!File.Exists(path)) 
                    msg += $"Can't read data. Path: {path}\n";
                if (string.IsNullOrEmpty(sheet))
                    msg += $"sheetName is empty. {sheet}\n";

                EditorUtility.DisplayDialog("Error", msg, "OK");
                return;
            }
            
            var serializer = new ExcelDataSerializer(path, sheet);

            int? typeRow = null;
            int? arrayRow = null;
            if (includeTypeRow) typeRow = typeRowIndex;
            columnHeaderList = serializer.GetColumnHeaders(typeRow, arrayRow);

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
