using UnityEngine;
using ChickenGames.SheetMachine.Utils;

namespace ChickenGames.SheetMachine.ExcelSheet
{

    [CreateAssetMenu(menuName = "SheetMachine/Setting/ExcelData Setting")]
    public class ExcelDataSettings : SingletonScriptableObject<ExcelDataSettings>
    {
        // A default path where .txt template files are.
        public string templatePath = PathMethods.GetDefaultTemplatePath();

        // A path where generated ScriptableObject derived class and its data class script files are to be put.
        public string runtimeClassPath = PathMethods.GetDefaultRuntimeClassPath();

        // A path where generated editor script files are to be put.
        public string editorClassPath = PathMethods.GetDefaultEditorClassPath();
    }
}