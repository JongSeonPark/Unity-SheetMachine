using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

// to resolve TlsException error.
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;

//using GDataDB;
//using GDataDB.Linq;

//using GDataDB.Impl;
//using Google.GData.Client;
//using Google.GData.Spreadsheets;

namespace UnityQuickSheet
{
    /// <summary>
    /// Resolve TlsException error.
    /// </summary>
    public class UnsafeSecurityPolicy
    {
        public static bool Validator(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            //Debug.Log("Validation successful!");
            return true;
        }

        public static void Instate()
        {
            ServicePointManager.ServerCertificateValidationCallback = Validator;
        }
    }

    /// <summary>
    /// An editor script class of GoogleMachine class.
    /// Google API V4가 적용 되었음.
    /// </summary>
    [CustomEditor(typeof(GoogleMachine))]
    public class GoogleMachineEditor : BaseMachineEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();

            // resolve TlsException error
            UnsafeSecurityPolicy.Instate();

            machine = target as GoogleMachine;
            if (machine != null)
            {
                machine.ReInitialize();

                // Specify paths with one on the GoogleDataSettings.asset file.
                if (string.IsNullOrEmpty(GoogleDataSettings.Instance.RuntimePath) == false)
                    machine.RuntimeClassPath = GoogleDataSettings.Instance.RuntimePath;
                if (string.IsNullOrEmpty(GoogleDataSettings.Instance.EditorPath) == false)
                    machine.EditorClassPath = GoogleDataSettings.Instance.EditorPath;
            }
        }

        /// <summary>
        /// Draw custom UI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GoogleDataSettings.Instance == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Toggle(true, "", "CN EntryError", GUILayout.Width(20));
                GUILayout.BeginVertical();
                GUILayout.Label("", GUILayout.Height(12));
                GUILayout.Label("Check the GoogleDataSetting.asset file exists or its path is correct.", GUILayout.Height(20));
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Label("Google Spreadsheet Settings:", headerStyle);

            EditorGUILayout.Separator();


            GUILayout.Label("Script Path Settings:", headerStyle);
            EditorGUILayout.HelpBox(
                "Sheet의 Id 부분입니다.\n" +
                "ex) https://docs.google.com/spreadsheets/d/(이부분)/edit#gid=0\n" +
                "(이부분)을 복사해서 넣어주시면 됩니다.",
                MessageType.Info, true);
            machine.SpreadSheetName = EditorGUILayout.TextField("SpreadSheet Id: ", machine.SpreadSheetName);
            machine.WorkSheetName = EditorGUILayout.TextField("WorkSheet Name: ", machine.WorkSheetName);

            EditorGUILayout.Separator();

            machine.prefixString = EditorGUILayout.TextField("Prefix String: ", machine.prefixString);
            machine.dataRowStartIndex = EditorGUILayout.IntField("DataRow Start Index: ", machine.dataRowStartIndex);
            machine.isDataTypeIndex = EditorGUILayout.Toggle(machine.isDataTypeIndex);
            if (machine.isDataTypeIndex)
            {
                machine.dataTypeIndex = EditorGUILayout.IntField("DataType Index: ", machine.dataTypeIndex);
            }


            GUILayout.BeginHorizontal();

            if (machine.HasColumnHeader())
            {
                if (GUILayout.Button("Update"))
                    Import();

                if (GUILayout.Button("Reimport"))
                    Import(true);
            }
            else
            {
                if (GUILayout.Button("Import"))
                {
                    //Import();
                    Test();
                }
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            DrawHeaderSetting(machine);

            EditorGUILayout.Separator();

            GUILayout.Label("Path Settings:", headerStyle);

            machine.TemplatePath = EditorGUILayout.TextField("Template: ", machine.TemplatePath);
            machine.RuntimeClassPath = EditorGUILayout.TextField("Runtime: ", machine.RuntimeClassPath);
            machine.EditorClassPath = EditorGUILayout.TextField("Editor:", machine.EditorClassPath);

            machine.onlyCreateDataClass = EditorGUILayout.Toggle("Only DataClass", machine.onlyCreateDataClass);

            EditorGUILayout.Separator();

            if (GUILayout.Button("Generate"))
            {
                if (string.IsNullOrEmpty(machine.SpreadSheetName) || string.IsNullOrEmpty(machine.WorkSheetName))
                {
                    Debug.LogWarning("No spreadsheet or worksheet is specified.");
                    return;
                }

                if (Generate(this.machine) != null)
                    Debug.Log("Successfully generated!");
                else
                    Debug.LogError("Failed to create a script from Google Spreadsheet.");
            }

            // force save changed type.
            if (GUI.changed)
            {
                EditorUtility.SetDirty(GoogleDataSettings.Instance);
                EditorUtility.SetDirty(machine);
            }
        }

        /// <summary>
        /// A delegate called on each of a cell query.
        /// </summary>
        //delegate void OnEachCell(CellEntry cell);

        /// <summary>
        /// Connect to google-spreadsheet with the specified account and password 
        /// then query cells and call the given callback.
        /// </summary>
        private void DoCellQuery()
        {
            // first we need to connect to the google-spreadsheet to get all the first row of the cells
            // which are used for the properties of data class.
            //var client = new DatabaseClient("", "");

            //if (string.IsNullOrEmpty(machine.SpreadSheetName))
            //    return;
            //if (string.IsNullOrEmpty(machine.WorkSheetName))
            //    return;

            //string error = string.Empty;
            //var db = client.GetDatabase(machine.SpreadSheetName, ref error);
            //if (db == null)
            //{
            //    string message = string.Empty;
            //    if (string.IsNullOrEmpty(error))
            //        message = @"Unknown error.";
            //    else
            //        message = string.Format(@"{0}", error);

            //    message += "\n\nOn the other hand, see 'GoogleDataSettings.asset' file and check the oAuth2 setting is correctly done.";
            //    EditorUtility.DisplayDialog("Error", message, "OK");
            //    return;
            //}

            //// retrieves all cells
            //var worksheet = ((Database)db).GetWorksheetEntry(machine.WorkSheetName);

            //// Fetch the cell feed of the worksheet.
            //CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            //var cellFeed = client.SpreadsheetService.Query(cellQuery);

            //// Iterate through each cell, printing its value.
            //foreach (CellEntry cell in cellFeed.Entries)
            //{
            //    if (onCell != null)
            //        onCell(cell);
            //}
        }

        /// <summary>
        /// Connect to the google spreadsheet and retrieves its header columns.
        /// </summary>
        protected override void Import(bool reimport = false)
        {
            Regex re = new Regex(@"\d+");

            Dictionary<string, ColumnHeader> headerDic = null;
            if (reimport)
                machine.ColumnHeaderList.Clear();
            else
                headerDic = machine.ColumnHeaderList.ToDictionary(k => k.name);

            List<ColumnHeader> tmpColumnList = new List<ColumnHeader>();


            DoCellQuery();

            //int order = 0;
            // query the first columns only.
            //DoCellQuery((cell) =>
            //{

            //    // get numerical value from a cell's address in A1 notation
            //    // only retrieves first column of the worksheet 
            //    // which is used for member fields of the created data class.
            //    Match m = re.Match(cell.Title.Text);
            //    if (int.Parse(m.Value) > 1)
            //        return;

            //    // check the column header is valid
            //    if (!IsValidHeader(cell.Value))
            //    {
            //        string error = string.Format(@"Invalid column header name {0}. Any c# keyword should not be used for column header. Note it is not case sensitive.", cell.Value);
            //        EditorUtility.DisplayDialog("Error", error, "OK");
            //        return;
            //    }

            //    ColumnHeader column = ParseColumnHeader(cell.Value, order++);
            //    if (headerDic != null && headerDic.ContainsKey(cell.Value))
            //    {
            //        // if the column is already exist, copy its name and type from the exist one.
            //        ColumnHeader h = machine.ColumnHeaderList.Find(x => x.name == column.name);
            //        if (h != null)
            //        {
            //            column.type = h.type;
            //            column.isArray = h.isArray;
            //        }
            //    }

            //    tmpColumnList.Add(column);
            //});

            // update (all of settings are reset when it reimports)
            machine.ColumnHeaderList = tmpColumnList;

            EditorUtility.SetDirty(machine);
            AssetDatabase.SaveAssets();
        }

        void Test()
        {
            var sheetRanges = new List<string>()
            {
                $"{machine.WorkSheetName}!{1}:{1}" // header,
            };


            if (machine.isDataTypeIndex)
                sheetRanges.Add($"{machine.WorkSheetName}!{machine.dataTypeIndex + 1}:{machine.dataTypeIndex + 1}"); // type

           
            var batchGetReq = GoogleDataSettings.Instance.Service.Spreadsheets.Values.BatchGet(machine.SpreadSheetName);
            batchGetReq.Ranges = sheetRanges;
            var batchGetRes = batchGetReq.Execute();

            List<ColumnHeader> tmpColumnList = new List<ColumnHeader>();

            var headerRow = batchGetRes.ValueRanges[0].Values[0];
            for (int i = 0; i < headerRow.Count; i++)
            {
                string headerName = (string)headerRow[i];
                string typeStr = null;
                if (machine.isDataTypeIndex)
                {
                    var typeRow = batchGetRes.ValueRanges[1].Values[0];
                    typeStr = typeRow.Count() > i ? (string)typeRow[i] : null;
                }
                ColumnHeader column = ParseColumnHeader(headerName, i, typeStr);

                tmpColumnList.Add(column);
            }


            // Test
            var valueGetReq = GoogleDataSettings.Instance.Service.Spreadsheets.Values.Get(machine.SpreadSheetName, $"{machine.WorkSheetName}!A{machine.dataRowStartIndex + 1}:{(char)('A' + tmpColumnList.Count)}");
            var valueGetRes = valueGetReq.Execute();
            var dataValues = valueGetRes.Values;

            for (int i = 0; i < dataValues.Count; i++)
            {
                var dataRow = dataValues[i];
                for (int j = 0; j < dataRow.Count; j++)
                {
                    var data = dataRow[j];
                    Debug.Log($"{(char)('A' + j)}{machine.dataRowStartIndex + 1 + i}:{data}");
                }
            }
            
            //


            machine.ColumnHeaderList = tmpColumnList;

            EditorUtility.SetDirty(machine);
            AssetDatabase.SaveAssets();
        }

        /// 
        /// Create utility class which has menu item function to create an asset file.
        /// 
        protected override void CreateAssetCreationScript(BaseMachine m, ScriptPrescription sp)
        {
            sp.className = machine.WorkSheetName;
            sp.spreadsheetName = machine.SpreadSheetName;
            sp.worksheetClassName = machine.WorkSheetName;
            sp.assetFileCreateFuncName = "Create" + machine.prefixString + machine.WorkSheetName + "AssetFile";
            sp.template = GetTemplate("AssetFileClass");

            sp.prefixStrLen = machine.prefixString.Length;
            sp.prefix = machine.prefixString;
            // write a script to the given folder.		
            using (var writer = new StreamWriter(TargetPathForAssetFileCreateFunc(machine.WorkSheetName)))
            {
                writer.Write(new ScriptGenerator(sp).ToString());
                writer.Close();
            }
        }

    }
}