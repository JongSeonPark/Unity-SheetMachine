using UnityEngine;
using UnityEditor;
using ChickenGames.SheetMachine.Utils;

namespace ChickenGames.SheetMachine.GoogleSheet
{
    [CustomEditor(typeof(GoogleDataSettings))]
    public class GoogleDataSettingsEditor : Editor
    {
        const int labelWidth = 90;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Credentials를 다운받으셔서 json 파일의 경로를 넣어주세요.", MessageType.Info, true);

            GUIHelper.DrawOpenFilePathLayout(
                pathText: ref GoogleDataSettings.Instance.credentialsPath,
                defaultPath: PathMethods.GetDefaultCredientalPath(),
                title: "Cred json Path:", 
                labelWidth: labelWidth,
                filePanelExtension: "json",
                filePanelTitle: "Open JSON file"
                );
            
            EditorGUILayout.HelpBox("Google Cloud Platform에서의 프로젝트(어플리케이션)이름을 입력하세요.", MessageType.Info, true);
            GUIHelper.DrawTextField(ref GoogleDataSettings.Instance.applicationName, "App Name: ", labelWidth);

            GUIHelper.DrawOpenFolderPathLayout(
                pathText: ref GoogleDataSettings.Instance.tokenPath,
                defaultPath: PathMethods.GetDefaultTokenPath(),
                title: "Token Path: ",
                labelWidth: labelWidth,
                filePanelTitle: "Open folder"
                );

            if (GUILayout.Button("Initialize Authenticate"))
                GoogleDataSettings.Instance.InitAuthenticate();

            GUIHelper.DrawOpenFolderPathLayout(
                pathText: ref GoogleDataSettings.Instance.templatePath,
                defaultPath: PathMethods.GetDefaultGoogleTemplatePath(),
                title: "Template Path: ",
                labelWidth: labelWidth,
                filePanelTitle: "Open folder"
                );

            GUIHelper.DrawOpenFolderPathLayout(
                pathText: ref GoogleDataSettings.Instance.runtimeClassPath,
                defaultPath: PathMethods.GetDefaultRuntimeClassPath(),
                title: "Runtime Path: ",
                labelWidth: labelWidth,
                filePanelTitle: "Open folder"
                );

            GUIHelper.DrawOpenFolderPathLayout(
                pathText: ref GoogleDataSettings.Instance.editorClassPath,
                defaultPath: PathMethods.GetDefaultEditorClassPath(),
                title: "Editor Path: ",
                labelWidth: labelWidth,
                filePanelTitle: "Open folder"
                );
        }
    }
}