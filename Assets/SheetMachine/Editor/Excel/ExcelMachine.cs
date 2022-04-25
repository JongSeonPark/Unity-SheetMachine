using ChickenGames.SheetMachine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
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
        }


        public override void Generate()
        {
            Debug.Log("Generate");

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
                { "MemberFields", CreateMemberFieldsString() },
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
