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

namespace ChickenGames.SheetMachine.GoogleSheet
{
    [CustomEditor(typeof(GoogleDataSettings))]
    public class GoogleDataSettingsEditor : Editor
    {
        const int labelWidth = 90;

        void DrawOpenFilePathLayout(ref string pathText, string title, int labelWidth, string filePanelExtension, string filePanelTitle = "Open file")
        {
            EditorGUILayout.BeginHorizontal(); // Begin json file setting

            GUILayout.Label(title, GUILayout.Width(labelWidth));

            string path = string.IsNullOrEmpty(pathText) ? Application.dataPath : pathText;
            pathText = EditorGUILayout.TextField(path);
            
            if (GUILayout.Button("...", GUILayout.Width(20)))
            {
                string folder = Path.GetDirectoryName(path);
                path = EditorUtility.OpenFilePanel(filePanelTitle, folder, filePanelExtension);
                if (path.Length != 0)
                {
                    var assetsFolderName = "Assets/";
                    var idx = path.IndexOf(assetsFolderName) + assetsFolderName.Length;
                    var pathSubstring = path.Substring(idx);
                    pathText = pathSubstring;

                    // force to save the setting.
                    EditorUtility.SetDirty(GoogleDataSettings.Instance);
                    AssetDatabase.SaveAssets();
                }
            }

            EditorGUILayout.EndHorizontal(); // End json file setting.
        }

        public override void OnInspectorGUI()
        {
            GUI.changed = false;
            EditorGUILayout.HelpBox("Credentials를 다운받으셔서 json 파일의 경로를 넣어주세요.", MessageType.Info, true);

            //GUILayout.BeginHorizontal(); // Begin json file setting

            //GUILayout.Label("Cred json Path:", GUILayout.Width(labelWidth));
            //string path = string.IsNullOrEmpty(GoogleDataSettings.Instance.credentialsPath) ? Application.dataPath : GoogleDataSettings.Instance.credentialsPath;
            //GoogleDataSettings.Instance.credentialsPath = EditorGUILayout.TextField(path);
            //if (GUILayout.Button("...", GUILayout.Width(20)))
            //{
            //    string folder = Path.GetDirectoryName(path);
            //    path = EditorUtility.OpenFilePanel("Open JSON file", folder, "json");
            //    if (path.Length != 0)
            //    {
            //        var assetsFolderName = "Assets/";
            //        var idx = path.IndexOf(assetsFolderName) + assetsFolderName.Length;
            //        var pathSubstring = path.Substring(idx);
            //        GoogleDataSettings.Instance.credentialsPath = pathSubstring;

            //        // force to save the setting.
            //        EditorUtility.SetDirty(GoogleDataSettings.Instance);
            //        AssetDatabase.SaveAssets();
            //    }
            //}
            //GUILayout.EndHorizontal(); // End json file setting.

            DrawOpenFilePathLayout(
                pathText: ref GoogleDataSettings.Instance.credentialsPath,
                title: "Cred json Path:", 
                labelWidth: labelWidth,
                filePanelExtension: "json",
                filePanelTitle: "Open JSON file");
            
            EditorGUILayout.HelpBox("Google Cloud Platform에서의 프로젝트(어플리케이션)이름을 입력하세요.", MessageType.Info, true);
            GUILayout.BeginHorizontal();
            GUILayout.Label("App Name: ", GUILayout.Width(labelWidth));
            GoogleDataSettings.Instance.applicationName = GUILayout.TextField(GoogleDataSettings.Instance.applicationName);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Initialize Authenticate"))
            {
                GoogleDataSettings.Instance.InitAuthenticate();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Token Path: ", GUILayout.Width(labelWidth));
            GoogleDataSettings.Instance.tokenPath = GUILayout.TextField(GoogleDataSettings.Instance.tokenPath);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Template Path: ", GUILayout.Width(labelWidth));
            GoogleDataSettings.Instance.templatePath = GUILayout.TextField(GoogleDataSettings.Instance.templatePath);
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Label("Runtime Path: ", GUILayout.Width(labelWidth));
            GoogleDataSettings.Instance.runtimePath = GUILayout.TextField(GoogleDataSettings.Instance.runtimePath);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Editor Path: ", GUILayout.Width(labelWidth));
            GoogleDataSettings.Instance.editorPath = GUILayout.TextField(GoogleDataSettings.Instance.editorPath);
            GUILayout.EndHorizontal();

            ////
            //GUILayout.BeginHorizontal();
            //GUILayout.Label("spreadsheetId: ", GUILayout.Width(LabelWidth));
            //GoogleDataSettings.Instance.spreadsheetId = GUILayout.TextField(GoogleDataSettings.Instance.spreadsheetId);
            //GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();
            //GUILayout.Label("range: ", GUILayout.Width(LabelWidth));
            //GoogleDataSettings.Instance.range = GUILayout.TextField(GoogleDataSettings.Instance.range);
            //GUILayout.EndHorizontal();

            //if (GUILayout.Button("GetRange"))
            //{
            //    GoogleDataSettings.Instance.GetRange();
            //}
            //if (GUILayout.Button("CreateSheet"))
            //{
            //    GoogleDataSettings.Instance.CreateSheet();
            //}
            //if (GUILayout.Button("JustGetRanges"))
            //{
            //    GoogleDataSettings.Instance.JustGetRanges();
            //}
            //if (GUILayout.Button("ActDataFilter"))
            //{
            //    GoogleDataSettings.Instance.ActDataFilter();
            //}
            //if (GUILayout.Button("MetaGet"))
            //{
            //    GoogleDataSettings.Instance.MetaGet();
            //}
            //if (GUILayout.Button("Append"))
            //{
            //    GoogleDataSettings.Instance.Append();
            //}
            //if (GUILayout.Button("BatchClear"))
            //{
            //    GoogleDataSettings.Instance.BatchClear();
            //}
            //if (GUILayout.Button("BatchGet"))
            //{
            //    GoogleDataSettings.Instance.BatchGet();
            //}
            //if (GUILayout.Button("BatchUpdate"))
            //{
            //    GoogleDataSettings.Instance.BatchUpdate();
            //}
        }
    }
}