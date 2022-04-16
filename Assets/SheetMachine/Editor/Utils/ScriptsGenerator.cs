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

        public static void Generate(string txtFilePath, string createPath, ScriptPrescription sp)
        {
            if (!File.Exists(txtFilePath))
                throw new ArgumentNullException("paths");

            string template = File.ReadAllText(txtFilePath);

            var type = sp.GetType();

            foreach (var f in type.GetFields())
            {
                var name = f.Name;
                var v = f.GetValue(sp).ToString();
                template = template.Replace($"${f.Name}", v);
            }


            //foreach(var keyValue in replaceTexts)
            //{
            //    result = result.Replace(keyValue.Key, keyValue.Value);
            //}

            CreateCSFile(createPath, template);
        }



        static void CreateCSFile(string path, string content)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write(content);
                writer.Close();
            }
        }
    }
}
