using ChickenGames.SheetMachine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ChickenGames.SheetMachine.ExcelSheet
{
    [CustomEditor(typeof(ExcelMachine))]
    public class ExcelMachineEditor : BaseMachineEditor
    {
        string excelDataPath = "";

        private void OnEnable()
        {
            machine = target as ExcelMachine;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Script Path Settings:", new GUIStyle(GUI.skin.label));

            GUIHelper.DrawOpenFilePathLayout(
                pathText: ref excelDataPath,
                defaultPath: PathMethods.GetDefaultProgramPath(),
                title: "Excel file path:",
                labelWidth: labelWidth,
                filePanelExtension: "xls",
                filePanelTitle: "Open excel file"
                );

            GUIHelper.DrawTextField(ref machine.sheetName, "Sheet Name: ", labelWidth);

            OnAfterDraw();
        }
    }
}
