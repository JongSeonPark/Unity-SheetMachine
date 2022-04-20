using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChickenGames.SheetMachine.GoogleSheet
{
    public abstract class GoogleSheetClassEditor : Editor
    {
        public abstract string SheetName { get; }
        public abstract string WorksheetName { get; }
        public abstract int TypeRowIndex { get; }

        public void Load()
        {
            //SheetClassBase targetData = target as SheetClassBase;

            //var req = GoogleDataSettings.Instance.Service.Spreadsheets.Values.Get(targetData.SheetName, targetData.WorksheetName);
            //var res = req.Execute();
            //var rows = res.Values;
            ////targetData.dataArray = GoogleDataSerializer.Deserialize<DataClassT>(rows, TypeRowIndex);

            //var method = typeof(GoogleDataSerializer).GetMethod(nameof(GoogleDataSerializer.Deserialize));
            //var generic = method.MakeGenericMethod(t);
            //targetData.dataArray = generic.Invoke(null, new object[] { rows, TypeRowIndex });

            //EditorUtility.SetDirty(this);
            //AssetDatabase.SaveAssets();
        }
    }
}
