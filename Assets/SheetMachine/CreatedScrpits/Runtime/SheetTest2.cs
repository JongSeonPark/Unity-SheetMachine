using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "SheetTest2", menuName = "SheetMachine/Data/Excel/SheetTest2")]
public class SheetTest2 : ScriptableObject 
{
    [HideInInspector] [SerializeField] 
    public readonly string SpreadSheetName = "Resources/TEST.xlsx";
    
    [HideInInspector] [SerializeField] 
    public readonly string WorksheetName = "SheetTest";
    
    // Note: initialize in OnEnable() not here.
    public SheetTest2Data[] dataArray;

    public readonly int typeRowIndex = 2;    

    void OnEnable()
    {
        if (dataArray == null)
            dataArray = new SheetTest2Data[0];
    }
}
