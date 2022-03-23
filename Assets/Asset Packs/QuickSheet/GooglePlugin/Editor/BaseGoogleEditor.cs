///////////////////////////////////////////////////////////////////////////////
///
/// BaseGoogleEditor.cs
/// 
/// (c)2013 Kim, Hyoun Woo
///
///////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

// to resolve TlsException error.
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace UnityQuickSheet
{
    /// <summary>
    /// Base class of .asset ScriptableObject class created from google spreadsheet.
    /// </summary>
    public class BaseGoogleEditor<T> : BaseEditor<T> where T : ScriptableObject
    {
        public override void OnEnable()
        {
            base.OnEnable();
        }

        /// <summary>
        /// Draw Inspector view.
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (target == null)
                return;

            // Update SerializedObject
            targetObject.Update();

            if (GUILayout.Button("Download"))
            {
                if (!Load())
                    Debug.LogError("Failed to Load data from Google.");
            }

            EditorGUILayout.Separator();

            DrawInspector();

            // Be sure to call [your serialized object].ApplyModifiedProperties()to save any changes.  
            targetObject.ApplyModifiedProperties();
        }


        protected List<int> SetArrayValue(string from)
        {
            List<int> tmp = new List<int>();

            CsvParser parser = new CsvParser(from);

            foreach (string s in parser)
            {
                Debug.Log("parsed value: " + s);
                tmp.Add(int.Parse(s));
            }

            return tmp;
        }

        public static string SplitCamelCase(string inputCamelCaseString)
        {
            string sTemp = Regex.Replace(inputCamelCaseString, "([A-Z][a-z])", " $1", RegexOptions.Compiled).Trim();
            return Regex.Replace(sTemp, "([A-Z][A-Z])", " $1", RegexOptions.Compiled).Trim();
        }
    }
}