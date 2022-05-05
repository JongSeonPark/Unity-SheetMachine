using UnityEngine;
using UnityEditor;
using ChickenGames.SheetMachine;
using ChickenGames.SheetMachine.GoogleSheet;

[CustomEditor(typeof(SheetTest))]
public class SheetTestEditor : BaseGoogleClassEditor
{	    
    public override bool OnDataLoad()
    {        
        SheetTest targetData = target as SheetTest;
	
        var req = GoogleDataSettings.Instance.Service.Spreadsheets.Values.Get(targetData.SpreadSheetName, targetData.WorksheetName);
        var res = req.Execute();
        var rows = res.Values;
        targetData.dataArray = new GoogleDataSerializer().Deserialize<SheetTestData>(rows, targetData.typeRowIndex);
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
