using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ChickenGames.SheetMachine.Utils
{
    /// <summary>
    /// Default Path Value �����ϴ� ���Դϴ�.
    /// ���α׷� �̵��� ProgramPath�� �������ּ���.
    /// </summary>
    public static class PathMethods
    {
        static readonly string ProgramPath = "SheetMachine";
        static readonly string TemplatePath = Path.Combine(ProgramPath, "Templates");
        static readonly string CredientalPath = Path.Combine(ProgramPath, "GoogleCredientals");
        static readonly string TokenPath = Path.Combine(ProgramPath, "GoogleTokens");
        static readonly string RuntimePath = Path.Combine(ProgramPath, "CreatedScrpits", "Runtime");
        static readonly string EditorPath = Path.Combine(ProgramPath, "CreatedScrpits", "Editor");

        static public string GetDefaultTemplatePath() => TemplatePath;
        static public string GetDefaultCredientalPath() => CredientalPath;
        static public string GetDefaultTokenPath() => TokenPath;
        static public string GetDefaultRuntimePath() => RuntimePath;
        static public string GetDefaultEditorPath() => EditorPath;


    }
}

