using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace ChickenGames.SheetMachine.Utils
{
    public class ScriptsGenerator
    {
        public Dictionary<string, string> replaceTexts;

        public string Generate(string targetFilePath)
        {
            if (!File.Exists(targetFilePath))
                throw new ArgumentNullException("paths");

            string result = File.ReadAllText(targetFilePath);

            foreach(var keyValue in replaceTexts)
            {
                result = result.Replace(keyValue.Key, keyValue.Value);
            }

            return result;
        }
    }
}
