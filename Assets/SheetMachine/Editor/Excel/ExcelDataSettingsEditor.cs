using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using UnityEditor;
using System.Text;
using Newtonsoft.Json.Linq;
using ChickenGames.SheetMachine.Utils;

namespace ChickenGames.SheetMachine.ExcelSheet
{
    [CustomEditor(typeof(ExcelDataSettings))]
    public class ExcelDataSettingsEditor : Editor
    {
        const int labelWidth = 90;

        public override void OnInspectorGUI()
        {
            GUIHelper.DrawOpenFolderPathLayout(
                pathText: ref ExcelDataSettings.Instance.templatePath,
                defaultPath: PathMethods.GetDefaultTemplatePath(),
                title: "Template Path: ",
                labelWidth: labelWidth,
                filePanelTitle: "Open folder"
                );

            GUIHelper.DrawOpenFolderPathLayout(
                pathText: ref ExcelDataSettings.Instance.runtimeClassPath,
                defaultPath: PathMethods.GetDefaultRuntimeClassPath(),
                title: "Runtime Path: ",
                labelWidth: labelWidth,
                filePanelTitle: "Open folder"
                );

            GUIHelper.DrawOpenFolderPathLayout(
                pathText: ref ExcelDataSettings.Instance.editorClassPath,
                defaultPath: PathMethods.GetDefaultEditorClassPath(),
                title: "Editor Path: ",
                labelWidth: labelWidth,
                filePanelTitle: "Open folder"
                );
        }
    }
}