using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using ChickenGames.SheetMachine.GoogleSheet;
using Newtonsoft.Json;

[CreateAssetMenu(menuName = "GOOGLE SHEET TEST")]
public class GoogleSheetTest : ScriptableObject
{
    public SheetsService Service
    {
        get => GoogleDataSettings.Instance.Service;
    }

    //private void Awake()
    //{
    //    Init();
    //    GetRange();
    //    CreateSheet();
    //    JustGetRanges();
    //    ActDataFilter();
    //    MetaGet();
    //}

    public Spreadsheet GetSheet(string spreadsheetId)
    {
        SpreadsheetsResource.GetRequest req = Service.Spreadsheets.Get(spreadsheetId);
        req.IncludeGridData = true;
        var spreadSheet = req.Execute();

        foreach (Sheet sheet in spreadSheet.Sheets)
        {
            if (sheet.Data == null)
            {
                Debug.Log($"{sheet.Properties.Title} Sheet 이름을 확인해 주세요.");
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
        spreadSheet.Properties.Title = "Test입니다.";


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
    /// 추가
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
    /// 일괄 지움
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
}
