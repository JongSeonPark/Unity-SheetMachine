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
                    Debug.Log($"{(char)(i + 'A')}{j}, cellValue: {cell.StringCellValue} | cellType: {cell.CellType}");
                }
            }
        }

    }
}