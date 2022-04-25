using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "SheetTest", menuName = "SheetMachine/Data/Google/SheetTest")]
public class SheetTest : ScriptableObject 
{
    [HideInInspector] [SerializeField] 
    public readonly string SpreadSheetName = "1BpKOeJMIIls1ThQ1rklhqycpxqHvTs8I2ozCHqzMC9o";
    
    [HideInInspector] [SerializeField] 
    public readonly string WorksheetName = "SheetTest";
    
    // Note: initialize in OnEnable() not here.
    public SheetTestData[] dataArray;

    public readonly int typeRowIndex = 3;    

    void OnEnable()
    {
        if (dataArray == null)
            dataArray = new SheetTestData[0];
    }
}
