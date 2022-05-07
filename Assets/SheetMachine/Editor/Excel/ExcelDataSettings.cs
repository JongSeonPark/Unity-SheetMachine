using UnityEngine;
using ChickenGames.SheetMachine.Utils;

namespace ChickenGames.SheetMachine.ExcelSheet
{

    [CreateAssetMenu(menuName = "SheetMachine/Setting/ExcelData Setting")]
    public class ExcelDataSettings : SingletonScriptableObject<ExcelDataSettings>
    {
        
        /// <summary>
        /// A default path where .txt template files are.
        /// </summary>
        public string templatePath = PathMethods.GetDefaultTemplatePath();

        /// <summary>
        /// A path where generated ScriptableObject derived class and its data class script files are to be put.
        /// </summary>
        public string runtimeClassPath = PathMethods.GetDefaultRuntimeClassPath();

        /// <summary>
        /// A path where generated editor script files are to be put.
        /// </summary>
        public string editorClassPath = PathMethods.GetDefaultEditorClassPath();


    }
}