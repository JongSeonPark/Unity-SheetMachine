using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SheetClassBase<TDataClass> : ScriptableObject
{
    public abstract string SheetName { get; }
    public abstract string WorksheetName { get; }
    public abstract int TypeRowIndex { get; }

    public TDataClass dataArray;
}
