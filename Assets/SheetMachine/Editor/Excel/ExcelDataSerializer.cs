using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System;
using System.Linq;
using System.ComponentModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ChickenGames.SheetMachine.ExcelSheet
{
    public class ExcelDataSerializer
    {
        private readonly IWorkbook workbook = null;
        private readonly ISheet sheet = null;
        public ExcelDataSerializer(string path, string sheetName = "")
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    string extension = Path.GetExtension(path);

                    if (extension == ".xls")
                        workbook = new HSSFWorkbook(fileStream);
                    else if (extension == ".xlsx")
                    {
#if UNITY_EDITOR_OSX
                        throw new Exception("xlsx is not supported on OSX.");
#else
                        workbook = new XSSFWorkbook(fileStream);
#endif
                    }
                    else
                    {
                        throw new Exception("Wrong file.");
                    }

                    if (!string.IsNullOrEmpty(sheetName))
                    {
                        sheet = workbook.GetSheet(sheetName);
                        if (sheet == null)
                            Debug.LogError($"Cannot find sheet '{sheetName}'.");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            for (int i = 0; i < sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                for (int j = 0; j < row.LastCellNum; j++)
                {
                    ICell cell = row.GetCell(j);
                    //Debug.Log($"{(char)(i + 'A')}{j}, cellValue: {cell.StringCellValue} | cellType: {cell.CellType}");


                    string cellIdx = $"{(char)(i + 'A')}{j}";
                    string cellVal = "";
                    switch (cell.CellType)
                    {
                        case NPOI.SS.UserModel.CellType.Unknown:
                            break;
                        case NPOI.SS.UserModel.CellType.Numeric:
                            cellVal = cell.NumericCellValue.ToString();
                            break;
                        case NPOI.SS.UserModel.CellType.String:
                            cellVal = cell.StringCellValue;
                            break;
                        case NPOI.SS.UserModel.CellType.Formula:
                            //cellVal = cell.CellFormula;
                            break;
                        case NPOI.SS.UserModel.CellType.Blank:
                            break;
                        case NPOI.SS.UserModel.CellType.Boolean:
                            cellVal = cell.BooleanCellValue.ToString();
                            break;
                        case NPOI.SS.UserModel.CellType.Error:
                            break;
                    }
                    Debug.Log($"{cellIdx}, cellValue: {cellVal} | cellType: {cell.CellType}");

                }
            }

            var columnHeaders = GetColumHeaeders(1, 2);
        }

        public List<ColumnHeader> GetColumHeaeders(int? typeRangeIndex = null, int? arrayRangeIndex = null)
        {
            List<ColumnHeader> tmpColumnList = new List<ColumnHeader>();

            IRow headerRow = sheet.GetRow(0);
            IRow typeRow = typeRangeIndex.HasValue ? sheet.GetRow(typeRangeIndex.Value) : null ;
            IRow isArrayRow = arrayRangeIndex.HasValue ? sheet.GetRow(arrayRangeIndex.Value) : null;

            for (int i = 0; i < headerRow.Cells.Count; i++)
            {
                string headerName = headerRow.Cells[i].StringCellValue;
                string typeStr = typeRangeIndex.HasValue && typeRow.Cells.Count > i ? typeRow.Cells[i].StringCellValue : null;
                bool isArray = arrayRangeIndex.HasValue && isArrayRow.Cells.Count > i ? isArrayRow.Cells[i].BooleanCellValue : false;
                ColumnHeader column = new ColumnHeader(headerName, isArray, typeStr);

                tmpColumnList.Add(column);
            }
            return tmpColumnList;
        }
    }
}