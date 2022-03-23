using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ChickenGames.SheetMachine.Utils
{
    public static class PathMethods
    {
        /// <summary>
        /// 프로그램 이동시 ProgramPath를 변경해주세요.
        /// </summary>
        static readonly string ProgramPath = "SheetMachine";
        static readonly string TemplatePath = Path.Combine(ProgramPath, "Templates");
        static readonly string CredientalPath = Path.Combine(ProgramPath, "GoogleCredientals");
        static readonly string TokenPath = Path.Combine(ProgramPath, "GoogleTokens");
        static readonly string RuntimePath = Path.Combine(ProgramPath, "CreatedScrpits", "Runtime");
        static readonly string EditorPath = Path.Combine(ProgramPath, "CreatedScrpits", "Editor");

        static public string GetDefaultTemplatePath() => Path.Combine(Application.dataPath, TemplatePath);
        static public string GetDefaultCredientalPath() => Path.Combine(Application.dataPath, CredientalPath);
        static public string GetDefaultTokenPath() => Path.Combine(Application.dataPath, TokenPath);
        static public string GetDefaultRuntimePath() => Path.Combine(Application.dataPath, RuntimePath);
        static public string GetDefaultEditorPath() => Path.Combine(Application.dataPath, EditorPath);


    }
}

