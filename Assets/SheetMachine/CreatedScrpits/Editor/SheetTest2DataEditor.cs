using UnityEngine;
using UnityEditor;
using System.IO;
using ChickenGames.SheetMachine.ExcelSheet;
using ChickenGames.SheetMachine.Utils;

[CustomEditor(typeof(SheetTest2))]
public class SheetTest2Editor : BaseExcelClassEditor
{	    
    public override bool OnDataLoad()
    {
        SheetTest2 targetData = target as SheetTest2;

        string path = PathMethods.Combine(Application.dataPath, targetData.SpreadSheetName);
        if (!File.Exists(path))
            return false;

        string sheet = targetData.WorksheetName;

        targetData.dataArray = new ExcelDataSerializer(path, sheet).Deserialize<SheetTest2Data>(targetData.typeRowIndex);
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();

        return true;
    }
}
