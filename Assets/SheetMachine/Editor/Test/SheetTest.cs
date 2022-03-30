using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using UnityEngine.EventSystems;
using ChickenGames.SheetMachine.Utils;

[CreateAssetMenu(menuName = "EXCEL SHEET TEST")]
public class SheetTest : ScriptableObject
{
    string sheetPath = "";
    private IWorkbook workbook = null;

    private void Awake()
    {
        sheetPath = PathMethods.Combine(Application.dataPath, "XlsSheet", "Data", "Battle", "Battle.xlsx");

        using (FileStream fileStream = new FileStream(sheetPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            var suffix = Path.GetExtension(sheetPath);
            if (suffix == ".xls")
                workbook = new HSSFWorkbook(fileStream);
            else if (suffix == ".xlsx")
                workbook = new XSSFWorkbook(fileStream);
            else
            {
                Debug.Log("없어용.");
            }

            int sheetIdx = -1;
            for (int k = 0; k < workbook.NumberOfSheets; k++)
            {
                var sheet = workbook.GetSheetAt(k);
                sheetIdx++;

                int count = 0;

                string lastCell = "";

                for (int i = 0; i < sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    var cells = row.Cells;
                    for (int j = 0; j < cells.Count; j++)
                    {
                        var cell = cells[j];
                        //Debug.Log($"ArrayFormulaRange: {cell.ArrayFormulaRange}");
                        //Debug.Log($"cell: {cell.BooleanCellValue}");
                        //Debug.Log($"cell: {cell.CachedFormulaResultType}");
                        //Debug.Log($"cell: {cell.CellComment}");
                        //Debug.Log($"cell: {cell.CellFormula}");
                        //Debug.Log($"cell: {cell.CellStyle}");
                        //Debug.Log($"cell: {cell.CellType}");
                        //Debug.Log($"cell: {cell.ColumnIndex}");


                        //var cellData = Convert.ChangeType(cell, typeof(string));

                        string cellName = $"{(char)(j + 'A')}{ i + 1}";
                        //Debug.Log($"cellType{cellName}: {cell.CellType}");
                        count++;
                        switch (cell.CellType)
                        {
                            case NPOI.SS.UserModel.CellType.Unknown:
                                break;
                            case NPOI.SS.UserModel.CellType.Numeric:
                                Debug.Log($"cell{cellName}: {cell.NumericCellValue}");
                                break;
                            case NPOI.SS.UserModel.CellType.String:
                                Debug.Log($"cell{cellName}: {cell.StringCellValue}");
                                break;
                            case NPOI.SS.UserModel.CellType.Formula:
                                Debug.Log($"cell{cellName}: {cell.CellFormula}");
                                break;
                            case NPOI.SS.UserModel.CellType.Blank:
                                //Debug.Log($"cell{cellName}: {null}");
                                break;
                            case NPOI.SS.UserModel.CellType.Boolean:
                                Debug.Log($"cell{cellName}: {cell.BooleanCellValue}");
                                break;
                            case NPOI.SS.UserModel.CellType.Error:
                                break;
                            default:
                                break;
                        }

                        lastCell = cellName;
                    }
                }

                Debug.LogWarning($"<color=red>SheetName: {sheet.SheetName}, Count: {count}, lastCell: {lastCell}</color>");

            }

        }
    }

}
