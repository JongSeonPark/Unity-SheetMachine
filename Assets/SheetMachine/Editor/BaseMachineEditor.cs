using ChickenGames.SheetMachine.GoogleSheet;
using ChickenGames.SheetMachine.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChickenGames.SheetMachine
{
    [CustomEditor(typeof(BaseMachine))]
    public abstract class BaseMachineEditor : Editor
    {
        protected const int labelWidth = 100;

        protected BaseMachine machine;

        protected void OnAfterDraw()
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                machine.includeTypeRow = EditorGUILayout.Toggle("Include Type Row: ", machine.includeTypeRow);
                if (machine.includeTypeRow)
                    machine.typeRowIndex = EditorGUILayout.IntField("Type Row: ", machine.typeRowIndex);
            }

            if (GUILayout.Button("Import"))
            {
                machine.Import();
                EditorUtility.SetDirty(machine);
                AssetDatabase.SaveAssets();
            }

            bool hasColum = machine.columnHeaderList != null && machine.columnHeaderList.Count != 0;
            if (hasColum) DrawSheetHeaders();

            EditorGUILayout.Separator();

            GUIHelper.DrawOpenFolderPathLayout(
                pathText: ref machine.templatePath,
                defaultPath: PathMethods.GetDefaultTemplatePath(),
                title: "Template Path: ",
                labelWidth: labelWidth,
                filePanelTitle: "Open folder"
                );

            GUIHelper.DrawOpenFolderPathLayout(
                pathText: ref machine.runtimeClassPath,
                defaultPath: PathMethods.GetDefaultRuntimeClassPath(),
                title: "Runtime Path: ",
                labelWidth: labelWidth,
                filePanelTitle: "Open folder"
                );

            GUIHelper.DrawOpenFolderPathLayout(
                pathText: ref machine.editorClassPath,
                defaultPath: PathMethods.GetDefaultEditorClassPath(),
                title: "Editor Path: ",
                labelWidth: labelWidth,
                filePanelTitle: "Open folder"
                );
            GUIHelper.DrawTextField(ref machine.className, "DataClass name: ", labelWidth);


            EditorGUILayout.Separator();

            if (GUILayout.Button("Generate"))
            {
                machine.Generate();
                AssetDatabase.Refresh();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(GoogleDataSettings.Instance);
                EditorUtility.SetDirty(machine);
            }
        }

        protected void DrawSheetHeaders()
        {
            EditorGUILayout.Separator();

            GUILayout.Label("Type Settings:");

            const int MEMBER_WIDTH = 100;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("Member", GUILayout.MinWidth(MEMBER_WIDTH));
                GUILayout.FlexibleSpace();
                string[] names = { "Type", "Array" };
                int[] widths = { 55, 40 };
                for (int i = 0; i < names.Length; i++)
                {
                    GUILayout.Label(new GUIContent(names[i]), GUILayout.Width(widths[i]));
                }
            }

            // Each cells
            using (new EditorGUILayout.VerticalScope("box"))
            {
                foreach (ColumnHeader header in machine.columnHeaderList)
                {
                    GUILayout.BeginHorizontal();

                    // show member field with label, read-only
                    EditorGUILayout.LabelField(header.name, GUILayout.MinWidth(MEMBER_WIDTH));
                    GUILayout.FlexibleSpace();

                    // specify type with enum-popup
                    header.type = (CellType)EditorGUILayout.EnumPopup(header.type, GUILayout.Width(60));
                    GUILayout.Space(20);

                    // array toggle
                    header.isArray = EditorGUILayout.Toggle(header.isArray, GUILayout.Width(20));
                    GUILayout.Space(10);
                    GUILayout.EndHorizontal();
                }
            }
        }


        //protected abstract void Import();
        //protected abstract void Generate();
    }
}
