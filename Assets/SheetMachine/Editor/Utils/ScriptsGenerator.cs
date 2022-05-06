using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace ChickenGames.SheetMachine.Utils
{
    public class ScriptsGenerator
    {
        public static void Generate(string txtFilePath, string createPath, Dictionary<string, string> sp)
        {
            if (!File.Exists(txtFilePath))
                throw new ArgumentNullException("paths");

            string template = File.ReadAllText(txtFilePath);

            foreach (var keyValue in sp)
            {
                var key = keyValue.Key;
                var value = keyValue.Value;
                template = template.Replace($"${key}", value);
            }

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
