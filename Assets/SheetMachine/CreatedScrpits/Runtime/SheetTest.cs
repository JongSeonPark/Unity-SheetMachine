using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "SheetTest", menuName = "SheetMachine/Data/Google/SheetTest")]
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
