using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace ChickenGames.SheetMachine.Utils
{
    public static class GUIHelper
    {
        public static void DrawTextField(ref string targetText, string title, int labelWidth)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, GUILayout.Width(labelWidth));
            targetText = GUILayout.TextField(targetText);
            GUILayout.EndHorizontal();
        }
        public static void DrawOpenFilePathLayout(ref string pathText, string defaultPath, string title, int labelWidth, string filePanelExtension, string filePanelTitle = "Open file")
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(title, GUILayout.Width(labelWidth));

            string path = string.IsNullOrEmpty(pathText) ? defaultPath : pathText;
            pathText = EditorGUILayout.TextField(path);

            if (GUILayout.Button("...", GUILayout.Width(20)))
            {
                string folder = Path.GetDirectoryName(PathMethods.Combine(Application.dataPath, path));
                path = EditorUtility.OpenFilePanel(filePanelTitle, folder, filePanelExtension);
                if (path.Length != 0)
                {
                    var assetsFolderName = "Assets/";
                    var idx = path.IndexOf(assetsFolderName) + assetsFolderName.Length;
                    if (idx >= 0)
                    {
                        var pathSubstring = path.Substring(idx);
                        pathText = pathSubstring;
                    }
                    else
                    {
                        Debug.LogError("err");
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        public static void DrawOpenFolderPathLayout(ref string pathText, string defaultPath, string title, int labelWidth, string filePanelTitle = "Open file")
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(title, GUILayout.Width(labelWidth));

            string path = string.IsNullOrEmpty(pathText) ? defaultPath : pathText;
            pathText = EditorGUILayout.TextField(path);

            if (GUILayout.Button("...", GUILayout.Width(20)))
            {
                string folder = Path.GetDirectoryName(PathMethods.Combine(Application.dataPath, path));
                string defaultFolder = PathMethods.Combine(Application.dataPath, defaultPath);
                path = EditorUtility.OpenFolderPanel(filePanelTitle, folder, defaultFolder);
                if (path.Length != 0)
                {
                    var assetsFolderName = "Assets/";
                    var idx = path.IndexOf(assetsFolderName) + assetsFolderName.Length;
                    if (idx >= 0)
                    {
                        var pathSubstring = path.Substring(idx);
                        pathText = pathSubstring;
                    }
                    else
                    {
                        Debug.LogError("err");
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
