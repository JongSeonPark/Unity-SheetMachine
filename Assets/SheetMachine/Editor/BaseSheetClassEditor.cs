using UnityEditor;
using UnityEngine;


public abstract class BaseSheetClassEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Update"))
        {
            OnDataLoad();
        }
        base.OnInspectorGUI();
    }

    public abstract bool OnDataLoad();
}

