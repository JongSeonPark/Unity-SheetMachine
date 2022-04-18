using ChickenGames.SheetMachine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ChickenGames.SheetMachine.GoogleSheet
{
    [CustomEditor(typeof(GoogleMachine))]
    public class GoogleMachineEditor : BaseMachineEditor
    {

        private void OnEnable()
        {
            machine = target as GoogleMachine;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Script Path Settings:", new GUIStyle(GUI.skin.label));
            EditorGUILayout.HelpBox(
                "Sheet�� Id �κ��Դϴ�.\n" +
                "ex) https://docs.google.com/spreadsheets/d/(�̺κ�)/edit#gid=0\n" +
                "(�̺κ�)�� �����ؼ� �־��ֽø� �˴ϴ�.",
                MessageType.Info, true);
            
            GUIHelper.DrawTextField(ref machine.spreadSheetName, "SpreadSheet Id: ", labelWidth);
            GUIHelper.DrawTextField(ref machine.sheetName, "Sheet Name: ", labelWidth);

            OnAfterDraw();
        }
    }
}
