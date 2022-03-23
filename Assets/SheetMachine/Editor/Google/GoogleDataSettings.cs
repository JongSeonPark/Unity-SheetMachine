using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using UnityEditor;
using ChickenGames.SheetMachine.Utils;

namespace ChickenGames.SheetMachine.GoogleSheet
{
    [CreateAssetMenu(menuName = "SheetMachine/Setting/GoogleData Setting")]
    public class GoogleDataSettings : ScriptableObject
    {
        [SerializeField]
        string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly };

        
        /// <summary>
        /// Google Cloud Platform������ ������Ʈ(���ø����̼�)�̸��� �Է��ϼ���.
        /// </summary>
        public string applicationName = "My First Project";
        /// <summary>
        /// Credentials�� �ٿ�����ż� json ������ ��θ� �־��ּ���.
        /// </summary>
        public string credentialsPath = "";

        public string tokenPath = "";

        /// <summary>
        /// A default path where .txt template files are.
        /// </summary>
        public string templatePath = "Asset Packs/QuickSheet/GooglePlugin/Templates";

        /// <summary>
        /// A path where generated ScriptableObject derived class and its data class script files are to be put.
        /// </summary>
        public string runtimePath = string.Empty;

        /// <summary>
        /// A path where generated editor script files are to be put.
        /// </summary>
        public string editorPath = string.Empty;

        private void Awake()
        {
            credentialsPath = PathMethods.GetDefaultCredientalPath();
            tokenPath = PathMethods.GetDefaultTokenPath();
            templatePath = PathMethods.GetDefaultTemplatePath();
            runtimePath = PathMethods.GetDefaultRuntimePath();
            editorPath = PathMethods.GetDefaultEditorPath();
        }

        SheetsService sheetsService;

        public SheetsService Service
        {
            get
            {
                if (sheetsService == null)
                {
                    InitAuthenticate();
                }
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
               new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = Application.dataPath + "/Tokens/token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                Debug.Log("Credential file has been saved to: " + credPath);
            }

            return credential;
        }

        /*
        ////////////////////////////////
        
        public Spreadsheet GetSheet(string spreadsheetId)
        {
            SpreadsheetsResource.GetRequest req = Service.Spreadsheets.Get(spreadsheetId);
            req.IncludeGridData = true;
            var spreadSheet = req.Execute();

            foreach (Sheet sheet in spreadSheet.Sheets)
            {
                if (sheet.Data == null)
                {
                    Debug.Log($"{sheet.Properties.Title} Sheet �̸��� Ȯ���� �ּ���.");
                    continue;
                }
                for (int c = 0; c < sheet.Data.Count; c++)
                {
                    IList<RowData> rowDatas = sheet.Data[c].RowData;
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                    Debug.Log($"Title: {sheet.Properties.Title}");
                    if (rowDatas == null) continue;
                    for (int j = 0; j < rowDatas.Count; j++)
                    {
                        RowData temp = rowDatas[j];
                        IList<CellData> cellDatas = temp.Values;
                        for (int i = 0; i < cellDatas.Count; i++)
                        {
                            CellData cell = cellDatas[i];
                            string key = $"row: {j}, Column: {i}";
                            Debug.Log($"key: {key}, value: {cell.FormattedValue}");
                            keyValuePairs.Add(key, cell.FormattedValue);
                        }
                    }
                }
            }
            return spreadSheet;
        }


        public string spreadsheetId = "1HcCkNqh9_vpNrzYhKkH5qVvVEzuegK5xsW-oHnNGrGA";
        public string range = "Class TextKey!A1:C";

        public void Init()
        {
            Debug.Log($"Version {SheetsService.Version}");
            Debug.Log($"Service Name {Service.Name}");
            Debug.Log($"Service ApplicationName {Service.ApplicationName}");
            Debug.Log($"Service BatchUri {Service.BatchUri}");
            Debug.Log($"Service BatchPath {Service.BatchPath}");
            Debug.Log($"Service BasePath {Service.BasePath}");

        }
        public void GetRange()
        {
            SpreadsheetsResource.ValuesResource.GetRequest req = Service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange res = req.Execute();
            IList<IList<object>> values = res.Values;
            if (values != null && values.Count > 0)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var row = values[i];
                    for (int j = 0; j < row.Count; j++)
                    {
                        Debug.Log($"{(char)('A' + j)}{i}: {row[j]}");
                    }
                }
            }
            else
            {
                Debug.Log("No data found.");
            }
        }

        public void CreateSheet()
        {
            Spreadsheet spreadSheet = new Spreadsheet();
            spreadSheet.Properties = new SpreadsheetProperties();
            spreadSheet.Properties.Title = "Test�Դϴ�.";


            SpreadsheetsResource.CreateRequest req = Service.Spreadsheets.Create(spreadSheet);
            req.Fields = "SheetId";
            var res = req.Execute();
            Debug.Log(JsonConvert.SerializeObject(res));
        }

        public List<string> ranges = new List<string>();

        public void JustGetRanges()
        {
            var req = Service.Spreadsheets.Get(spreadsheetId);
            req.Ranges = ranges;
            req.IncludeGridData = true;
            var res = req.Execute();
            Debug.Log(JsonConvert.SerializeObject(res));

        }

        public void ActDataFilter()
        {
            List<DataFilter> dataFilters = new List<DataFilter>();
            dataFilters.Add(new DataFilter
            {
                A1Range = "",
                DeveloperMetadataLookup = new DeveloperMetadataLookup { }
            });
            GetSpreadsheetByDataFilterRequest requestBody = new GetSpreadsheetByDataFilterRequest();
            requestBody.DataFilters = dataFilters;
            requestBody.IncludeGridData = false;
            var req = Service.Spreadsheets.GetByDataFilter(requestBody, spreadsheetId);

            var res = req.Execute();
            Debug.Log(JsonConvert.SerializeObject(res));

        }

        public int metadataId = 0;

        public void MetaGet()
        {
            var req = Service.Spreadsheets.DeveloperMetadata.Get(spreadsheetId, metadataId);
            var res = req.Execute();
            Debug.Log(JsonConvert.SerializeObject(res));
        }

        [SerializeField]
        string appendRange = "";
        [SerializeField]
        SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum appendValueInputOption;
        [SerializeField]
        SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum appendInsertDataOption;

        /// <summary>
        /// �߰�
        /// </summary>
        public void Append()
        {
            var reqBody = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> {"asdasd", 1, 2,},
                }
            };


            var req = Service.Spreadsheets.Values.Append(reqBody, spreadsheetId, appendRange);
            req.ValueInputOption = appendValueInputOption;
            req.InsertDataOption = appendInsertDataOption;
            var res = req.Execute();

            Debug.Log(JsonConvert.SerializeObject(res));
        }


        public List<string> batchClearRanges = new List<string>();

        /// <summary>
        /// �ϰ� ����
        /// </summary>
        public void BatchClear()
        {
            BatchClearValuesRequest body = new BatchClearValuesRequest
            {
                Ranges = batchClearRanges
            };
            var req = Service.Spreadsheets.Values.BatchClear(body, spreadsheetId);

            var res = req.Execute();
            Debug.Log(JsonConvert.SerializeObject(res));
        }

        public List<string> batchGetRanges = new List<string>();
        public SpreadsheetsResource.ValuesResource.BatchGetRequest.ValueRenderOptionEnum batchGetValueRenderOption;
        public SpreadsheetsResource.ValuesResource.BatchGetRequest.DateTimeRenderOptionEnum batchGetDateTimeRenderOption;
        public void BatchGet()
        {
            var req = Service.Spreadsheets.Values.BatchGet(spreadsheetId);
            req.Ranges = batchGetRanges;
            req.ValueRenderOption = batchGetValueRenderOption;
            req.DateTimeRenderOption = batchGetDateTimeRenderOption;
            var res = req.Execute();
            Debug.Log(JsonConvert.SerializeObject(res));
        }


        public string batchUpdateValueInputOption;

        public bool? a;

        public void BatchUpdate()
        {
            List<ValueRange> valueRangeDatas = new List<ValueRange> {
                new ValueRange
                {
                    Range = "SoundKey!A20:B20",
                    Values = new List<IList<object>>
                    {
                        new List<object> {0}
                    }
                },
                new ValueRange
                {
                    Range = "SoundKey!A21:B21",
                    Values = new List<IList<object>>
                    {
                        new List<object> {1}
                    }
                }
            };

            BatchUpdateValuesRequest body = new BatchUpdateValuesRequest();
            body.ValueInputOption = batchUpdateValueInputOption;
            body.Data = valueRangeDatas;
            var req = Service.Spreadsheets.Values.BatchUpdate(body, spreadsheetId);
            var res = req.Execute();
            Debug.Log(JsonConvert.SerializeObject(res));
        }
        */
    }
}