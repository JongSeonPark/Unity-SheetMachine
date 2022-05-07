using System.Linq;
using UnityEngine;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using ChickenGames.SheetMachine.Utils;

namespace ChickenGames.SheetMachine.GoogleSheet
{
    enum SheetsServiceScope
    {
        Drive,
        DriveFile,
        DriveReadonly,
        Spreadsheets,
        SpreadsheetsReadonly
    }

    [CreateAssetMenu(menuName = "SheetMachine/Setting/GoogleData Setting")]
    public class GoogleDataSettings : SingletonScriptableObject<GoogleDataSettings>
    {
        [SerializeField]
        SheetsServiceScope[] scopes = { SheetsServiceScope.SpreadsheetsReadonly };

        string[] GetScopes()
        {
            string ScopeEnumToString(SheetsServiceScope scope)
            {
                switch (scope)
                {
                    case SheetsServiceScope.Drive: return SheetsService.Scope.Drive;
                    case SheetsServiceScope.DriveFile: return SheetsService.Scope.DriveFile;
                    case SheetsServiceScope.DriveReadonly: return SheetsService.Scope.DriveReadonly;
                    case SheetsServiceScope.Spreadsheets: return SheetsService.Scope.Spreadsheets;
                    case SheetsServiceScope.SpreadsheetsReadonly: return SheetsService.Scope.SpreadsheetsReadonly;
                    default: return SheetsService.Scope.SpreadsheetsReadonly;
                }
            }

            return scopes.Select(ScopeEnumToString).ToArray();
        }

        /// <summary>
        /// Google Cloud Platform에서의 프로젝트(어플리케이션)이름을 입력하세요.
        /// </summary>
        public string applicationName = "AppName";
        /// <summary>
        /// Credentials를 다운받으셔서 json 파일의 경로를 넣어주세요.
        /// </summary>
        public string credentialsPath = PathMethods.GetDefaultCredientalPath();

        public string tokenPath = PathMethods.GetDefaultTokenPath();

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

        SheetsService sheetsService;

        public SheetsService Service
        {
            get
            {
                if (sheetsService == null)
                    InitAuthenticate();

                return sheetsService;
            }
        }

        public void InitAuthenticate()
        {
            var credential = GetCredential();

            // Create Google Sheets API service.
            sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });
        }

        private UserCredential GetCredential()
        {
            UserCredential credential;
            //you have to put the file to this path. The credentials.json you can download from the Google Cloud console.
            using (var stream =
               new FileStream(PathMethods.Combine(Application.dataPath, credentialsPath), FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                var credientialFileName = Path.GetFileNameWithoutExtension(credentialsPath);
                string credPath = PathMethods.Combine(Application.dataPath, tokenPath, credientialFileName + "_token.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    GetScopes(),
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                Debug.Log("Credential file has been saved to: " + credPath);
            }

            return credential;
        }

    }
}