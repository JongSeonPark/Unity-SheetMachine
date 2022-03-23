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

namespace UnityQuickSheet
{
    [CustomEditor(typeof(GoogleDataSettings))]
    public class GoogleDataSettingsEditor : Editor
    {
        const int LabelWidth = 90;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUI.changed = false;
            EditorGUILayout.HelpBox("Credentials를 다운받으셔서 json 파일의 경로를 넣어주세요.", MessageType.Info, true);
            GUILayout.BeginHorizontal(); // Begin json file setting
            GUILayout.Label("Cred json Path:", GUILayout.Width(LabelWidth));
            string path = "";
            if (string.IsNullOrEmpty(GoogleDataSettings.Instance.credentialsPath))
                path = Application.dataPath;
            else
                path = GoogleDataSettings.Instance.credentialsPath;

            GoogleDataSettings.Instance.credentialsPath = GUILayout.TextField(path, GUILayout.Width(250));
            if (GUILayout.Button("...", GUILayout.Width(20)))
            {
                string folder = Path.GetDirectoryName(path);
                path = EditorUtility.OpenFilePanel("Open JSON file", folder, "json");
                if (path.Length != 0)
                {
                    GoogleDataSettings.Instance.credentialsPath = path;

                    // force to save the setting.
                    EditorUtility.SetDirty(GoogleDataSettings.Instance);
                    AssetDatabase.SaveAssets();
                }
            }
            GUILayout.EndHorizontal(); // End json file setting.

            EditorGUILayout.HelpBox("Google Cloud Platform에서의 프로젝트(어플리케이션)이름을 입력하세요.", MessageType.Info, true);
            GUILayout.BeginHorizontal();
            GUILayout.Label("App Name: ", GUILayout.Width(LabelWidth));
            GoogleDataSettings.Instance.applicationName = GUILayout.TextField(GoogleDataSettings.Instance.applicationName);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Initialize Authenticate"))
            {
                GoogleDataSettings.Instance.InitAuthenticate();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Template Path: ", GUILayout.Width(LabelWidth));
            GoogleDataSettings.Instance.TemplatePath = GUILayout.TextField(GoogleDataSettings.Instance.TemplatePath);
            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Runtime Path: ", GUILayout.Width(LabelWidth));
            GoogleDataSettings.Instance.RuntimePath = GUILayout.TextField(GoogleDataSettings.Instance.RuntimePath);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Editor Path: ", GUILayout.Width(LabelWidth));
            GoogleDataSettings.Instance.EditorPath = GUILayout.TextField(GoogleDataSettings.Instance.EditorPath);
            GUILayout.EndHorizontal();

            ////
            GUILayout.BeginHorizontal();
            GUILayout.Label("spreadsheetId: ", GUILayout.Width(LabelWidth));
            GoogleDataSettings.Instance.spreadsheetId = GUILayout.TextField(GoogleDataSettings.Instance.spreadsheetId);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("range: ", GUILayout.Width(LabelWidth));
            GoogleDataSettings.Instance.range = GUILayout.TextField(GoogleDataSettings.Instance.range);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("GetRange"))
            {
                GoogleDataSettings.Instance.GetRange();
            }
            if (GUILayout.Button("CreateSheet"))
            {
                GoogleDataSettings.Instance.CreateSheet();
            }
            if (GUILayout.Button("JustGetRanges"))
            {
                GoogleDataSettings.Instance.JustGetRanges();
            }
            if (GUILayout.Button("ActDataFilter"))
            {
                GoogleDataSettings.Instance.ActDataFilter();
            }
            if (GUILayout.Button("MetaGet"))
            {
                GoogleDataSettings.Instance.MetaGet();
            }
            if (GUILayout.Button("Append"))
            {
                GoogleDataSettings.Instance.Append();
            } 
            if (GUILayout.Button("BatchClear"))
            {
                GoogleDataSettings.Instance.BatchClear();
            }
            if (GUILayout.Button("BatchGet"))
            {
                GoogleDataSettings.Instance.BatchGet();
            }
            if (GUILayout.Button("BatchUpdate"))
            {
                GoogleDataSettings.Instance.BatchUpdate();
            }
        }
    }
}