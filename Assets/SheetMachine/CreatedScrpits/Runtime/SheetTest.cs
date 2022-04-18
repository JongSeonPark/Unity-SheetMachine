using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

///
/// !!! Machine generated code !!!
///
/// A class which deriveds ScritableObject class so all its data 
/// can be serialized onto an asset data file.
/// 
[System.Serializable]
public class SheetTest : ScriptableObject 
{
    [HideInInspector] [SerializeField] 
    public string SheetName = "";
    
    [HideInInspector] [SerializeField] 
    public string WorksheetName = "";
    
    // Note: initialize in OnEnable() not here.
    public SheetTestData[] dataArray;

    public const int typeRowIndex = 1;    

    void OnEnable()
    {
        if (dataArray == null)
            dataArray = new SheetTestData[0];
    }
}
