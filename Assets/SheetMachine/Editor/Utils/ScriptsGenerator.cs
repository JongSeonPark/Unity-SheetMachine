using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace ChickenGames.SheetMachine.Utils
{
    public class ScriptsGenerator
    {
        //public Dictionary<string, string> replaceTexts;

        public static string Generate(string targetFilePath, ScriptPrescription sp)
        {
            if (!File.Exists(targetFilePath))
                throw new ArgumentNullException("paths");

            string template = File.ReadAllText(targetFilePath);

            var type = sp.GetType();

            
            foreach (var f in type.GetFields())
            {
                template = template.Replace($"${f.Name}", f.GetValue(sp).ToString());
            }


            //foreach(var keyValue in replaceTexts)
            //{
            //    result = result.Replace(keyValue.Key, keyValue.Value);
            //}

            return template;
        }
    }
}
