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

namespace ChickenGames.SheetMachine.GoogleSheet
{
    [CustomEditor(typeof(GoogleSheetTest))]
    public class GoogleSheetTestEditor : Editor
    {
        const int labelWidth = 90;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targetInst =  target as GoogleSheetTest;

            GUILayout.BeginHorizontal();
            GUILayout.Label("spreadsheetId: ", GUILayout.Width(labelWidth));
            targetInst.spreadsheetId = GUILayout.TextField(targetInst.spreadsheetId);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("range: ", GUILayout.Width(labelWidth));
            targetInst.range = GUILayout.TextField(targetInst.range);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("GetRange"))
            {
                targetInst.GetRange();
            }
            if (GUILayout.Button("CreateSheet"))
            {
                targetInst.CreateSheet();
            }
            if (GUILayout.Button("JustGetRanges"))
            {
                targetInst.JustGetRanges();
            }
            if (GUILayout.Button("ActDataFilter"))
            {
                targetInst.ActDataFilter();
            }
            if (GUILayout.Button("MetaGet"))
            {
                targetInst.MetaGet();
            }
            if (GUILayout.Button("Append"))
            {
                targetInst.Append();
            }
            if (GUILayout.Button("BatchClear"))
            {
                targetInst.BatchClear();
            }
            if (GUILayout.Button("BatchGet"))
            {
                targetInst.BatchGet();
            }
            if (GUILayout.Button("BatchUpdate"))
            {
                targetInst.BatchUpdate();
            }
        }
    }
}