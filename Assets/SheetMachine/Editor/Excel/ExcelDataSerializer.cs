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
        }

        public List<ColumnHeader> GetColumnHeaders(int? typeRangeIndex = null, int? arrayRangeIndex = null)
        {
            List<ColumnHeader> tmpColumnList = new List<ColumnHeader>();

            IRow headerRow = sheet.GetRow(0);
            IRow typeRow = typeRangeIndex.HasValue ? sheet.GetRow(typeRangeIndex.Value) : null;
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

        public T[] Deserialize<T>(int dataRowStartIndex)
        {
            Type t = typeof(T);
            List<T> r = new List<T>();

            List<HeaderRowInfo> headerRowInfo = new List<HeaderRowInfo>();
            var headerRow = sheet.GetRow(0);

            for (int i = 0; i < headerRow.Cells.Count; i++)
            {
                var header = headerRow.GetCell(i).StringCellValue;

                var field = t.GetField(header, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (field != null)
                {
                    headerRowInfo.Add(new HeaderRowInfo
                    {
                        index = i,
                        field = field,
                    });
                }
            }

            for (int j = dataRowStartIndex; j < sheet.LastRowNum; j++)
            {
                T inst = (T)Activator.CreateInstance(t);
                var row = sheet.GetRow(j);

                for (int i = 0; i < headerRowInfo.Count; i++)
                {
                    var headerInfo = headerRowInfo[i];
                    var cell = row.LastCellNum > headerInfo.index ? (string)row[headerInfo.index] : "";
                    var type = headerInfo.field.FieldType;
                    object value = new object();

                    if (type.IsArray)
                    {
                        const char DELIMETER = ','; // '\n'

                        if (type.GetElementType().IsEnum)
                        {
                            var values = cell.Split(DELIMETER)
                                .Select(s => s.Replace(" ", string.Empty))
                                .Select(i => Enum.Parse(type.GetElementType(), i))
                                .ToArray();

                            Array array = (Array)Activator.CreateInstance(type, values.Length);

                            for (int k = 0; k < values.Length; k++)
                            {
                                array.SetValue(values[k], k);
                            }

                            value = array;
                        }
                        else
                        {
                            var values = cell.Split(DELIMETER)
                                .Select(s => s.Replace(" ", string.Empty))
                                .ToArray();

                            Array array = (Array)Activator.CreateInstance(type, values.Length);

                            for (int k = 0; k < values.Length; k++)
                            {
                                var temp = Convert.ChangeType(values[k], type.GetElementType());
                                array.SetValue(temp, k);
                            }

                            value = array;
                        }
                    }
                    else
                    {
                        if (type.IsEnum)
                        {
                            value = Enum.Parse(type, cell.Replace(" ", string.Empty));
                        }
                        else
                        {
                            if (cell == "" && type != typeof(string))
                                cell = "0";

                            value = Convert.ChangeType(cell, type);
                        }
                    }
                    headerInfo.field.SetValue(inst, value);
                }
                r.Add(inst);
            }
            return null;
        }

        class HeaderRowInfo
        {
            public int index;
            public FieldInfo field;
        }
    }