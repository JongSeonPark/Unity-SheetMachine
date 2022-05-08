using ChickenGames.SheetMachine.Utils;
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
                "Sheet의 Id 부분입니다.\n" +
                "ex) https://docs.google.com/spreadsheets/d/(이부분)/edit#gid=0\n" +
                "(이부분)을 복사해서 넣어주시면 됩니다.",
                MessageType.Info, true);
            
            GUIHelper.DrawTextField(ref machine.spreadSheetName, "SpreadSheet Id: ", labelWidth);
            GUIHelper.DrawTextField(ref machine.sheetName, "Sheet Name: ", labelWidth);

            OnAfterDraw();
        }
    }
}
