using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ChickenGames.SheetMachine.Utils
{
    /// <summary>
    /// Default Path Value 설정하는 곳입니다.
    /// 프로그램 이동시 ProgramPath를 변경해주세요.
    /// </summary>
    public static class PathMethods
    {
        static readonly string ProgramPath = "SheetMachine";
        static readonly string TemplatePath = Combine(ProgramPath, "Templates");
        static readonly string CredientalPath = Combine(ProgramPath, "GoogleCredientals");
        static readonly string TokenPath = Combine(ProgramPath, "GoogleTokens");
        static readonly string RuntimeClassPath = Combine(ProgramPath, "CreatedScrpits", "Runtime");
        static readonly string EditorClassPath = Combine(ProgramPath, "CreatedScrpits", "Editor");

        public static string GetDefaultProgramPath() => ProgramPath;
        public static string GetDefaultTemplatePath() => TemplatePath;
        public static string GetDefaultCredientalPath() => CredientalPath;
        public static string GetDefaultTokenPath() => TokenPath;
        public static string GetDefaultRuntimeClassPath() => RuntimeClassPath;
        public static string GetDefaultEditorClassPath() => EditorClassPath;

        /// <summary>
        /// \형태의 Path.Combine을 유니티가 친숙한 /형태로 Combine
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Combine(string path1, string path2)
        {
            if (path1 == null || path2 == null)
            {
                throw new ArgumentNullException((path1 == null) ? "path1" : "path2");
            }

            path1.Replace('\\', '/');
            path2.Replace('\\', '/');

            return CombineNoChecks(path1, path2);
        }

        public static string Combine(string path1, string path2, string path3)
        {
            if (path1 == null || path2 == null || path3 == null)
            {
                throw new ArgumentNullException((path1 == null) ? "path1" : ((path2 == null) ? "path2" : "path3"));
            }

            path1.Replace('\\', '/');
            path2.Replace('\\', '/');
            path3.Replace('\\', '/');

            return CombineNoChecks(CombineNoChecks(path1, path2), path3);
        }

        public static string Combine(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
            {
                throw new ArgumentNullException("paths");
            }

            var result = paths[0];
            for (int i = 1; i < paths.Length; i++)
            {
                result = CombineNoChecks(result, paths[i]);
            }
            return result;
        }

        static string CombineNoChecks(string path1, string path2)
        {
            if (path2.Length == 0)
            {
                return path1;
            }

            if (path1.Length == 0)
            {
                return path2;
            }

            if (Path.IsPathRooted(path2))
            {
                return path2;
            }

            char c = path1[path1.Length - 1];
            if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar && c != Path.VolumeSeparatorChar)
            {
                return path1 + "/" + path2;
            }

            return path1 + path2;
        }

    }
}

