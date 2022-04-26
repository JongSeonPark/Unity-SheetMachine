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

        private void OnEnable()
        {
            machine = target as ExcelMachine;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Script Path Settings:", new GUIStyle(GUI.skin.label));

#if UNITY_EDITOR_WIN
            var filePanelExtension = "excel files;*.xls;*.xlsx";
#else // for UNITY_EDITOR_OSX
            var filePanelExtension = "xls";
#endif
            GUIHelper.DrawOpenFilePathLayout(
                pathText: ref machine.spreadSheetName,
                defaultPath: PathMethods.GetDefaultProgramPath(),
                title: "Excel file path:",
                labelWidth: labelWidth,
                filePanelExtension: filePanelExtension,
                filePanelTitle: "Open excel file"
                );

            GUIHelper.DrawTextField(ref machine.sheetName, "Sheet Name: ", labelWidth);

            OnAfterDraw();
        }
    }
}
