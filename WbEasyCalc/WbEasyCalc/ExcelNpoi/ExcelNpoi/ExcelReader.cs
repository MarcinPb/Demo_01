using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ExcelNpoi.Model;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ExcelNpoi
{
    public class ExcelReader
    {
        private const string SettingsSheetName = "ApplicationSettings";

        public ExcelReader(string filePath)
        {
            FilePath = filePath;
            Workbook = ReadFile();
        }

        public string FilePath { get; }
        public XSSFWorkbook Workbook { get; }

        private XSSFWorkbook ReadFile()
        {
            XSSFWorkbook wbk;
            using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                wbk = new XSSFWorkbook(file);
            }

            return wbk;
        }

        /*
        public T ReadSetting<T>(string settingName)
        {
            var sheet = this.Workbook.GetSheet(SettingsSheetName);
            if (sheet == null)
            {
                string message = string.Format("Could not find setting sheet: {0}.", SettingsSheetName);
                throw new Exception(message);
            }

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }

                var settingNameCell = row.GetCell(0);
                if (settingNameCell == null)
                {
                    continue;
                }

                if (!settingNameCell.StringCellValue.Equals(settingName, System.StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var valueCell = row.GetCell(1);
                if (valueCell == null)
                {
                    throw new Exception("The value was empty");
                }

                T value = this.GetCellValue<T>(valueCell);
                return value;
            }

            throw new Exception("Could not find the requested setting.");
        }
        */

        public T ReadCell<T>(string sheetName, string cellName)
        {
            string ascii = "ABCDEFGHIJKLMNOPQRSTUWXYZ";

            string columnName = Regex.Match(cellName, @"[A-Z]").Value;
            string rowName = Regex.Match(cellName, @"\d+").Value;

            int columnNo = ascii.IndexOf(columnName);
            int rowNo = Convert.ToInt32(rowName)-1;

            return ReadCell<T>(sheetName, columnNo, rowNo);
        }

        public T ReadCell<T>(string sheetName, int columnNo, int rowNo)
        {
            var sheet = this.Workbook.GetSheet(sheetName);
            if (sheet == null)
            {
                string message = $"Could not find setting sheet: {SettingsSheetName}.";
                throw new Exception(message);
            }

            IRow row = sheet.GetRow(rowNo);
            ICell valueCell = row.GetCell(columnNo);

            T value = this.GetCellValue<T>(valueCell);
            return value;
        }

        private T GetCellValue<T>(ICell cell)
        {
            Type t = typeof(T);
            if (t == typeof(double) || t == typeof(int))
            {
                return (T)Convert.ChangeType(cell.NumericCellValue, t);
            }

            if (t == typeof(string))
            {
                return (T)Convert.ChangeType(cell.StringCellValue, t);
            }

            if (typeof(T) == typeof(bool))
            {
                return (T)Convert.ChangeType(cell.BooleanCellValue, t);
            }

            if (typeof(T) == typeof(DateTime))
            {
                return (T)Convert.ChangeType(cell.DateCellValue, t);
            }

            throw new ArgumentException($"The requested type is not supported: {t.ToString()}", nameof(T));
        }





        //private void WriteFile()
        //{
        //    XSSFWorkbook wbk;
        //    using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite))
        //    {
        //        wbk = new XSSFWorkbook(fs);

        //        WriteToCell()


        //        wbk.Write(fs);
        //    }
        //}

        public void WriteToCell<T>(string sheetName, string cellName, T value)
        {
            string ascii = "ABCDEFGHIJKLMNOPQRSTUWXYZ";

            string columnName = Regex.Match(cellName, @"[A-Z]").Value;
            string rowName = Regex.Match(cellName, @"\d+").Value;

            int columnNo = ascii.IndexOf(columnName);
            int rowNo = Convert.ToInt32(rowName) - 1;

            WriteToCell(sheetName, columnNo, rowNo, value);
        }
        //public CellByRowColumnNo<T> CellByNameToCellByRowColumnNo<T>(CellByName<T> cellByName)
        //{
        //    string ascii = "ABCDEFGHIJKLMNOPQRSTUWXYZ";

        //    string columnName = Regex.Match(cellByName.CellName, @"[A-Z]").Value;
        //    string rowName = Regex.Match(cellByName.CellName, @"\d+").Value;

        //    int columnNo = ascii.IndexOf(columnName);
        //    int rowNo = Convert.ToInt32(rowName) - 1;

        //    return new CellByRowColumnNo<T>(cellByName.SheetName, columnNo, rowNo, cellByName.CellValue, cellByName.CellValueType);
        //}

        public void WriteToCell<T>(string sheetName, int columnNo, int rowNo, T value)
        {
            XSSFWorkbook wbk = Workbook;
            //using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            //{
            //    wbk = new XSSFWorkbook(fs);
            //    fs.Close();
            //}
            var sheet = wbk.GetSheet(sheetName);
            if (sheet == null)
            {
                string message = $"Could not find setting sheet: {SettingsSheetName}.";
                throw new Exception(message);
            }

            IRow row = sheet.GetRow(rowNo);
            ICell cell = row.GetCell(columnNo);

            //T value = this.GetCellValue<T>(valueCell);
            SetCellValue(cell, value);

            //var ggg = row.GetCell(columnNo);

            //using (FileStream fs = new FileStream(@"K:\It\Work\Grundfos\2020-03-03\Repos_my\Repo\wbeasycalc\Grundfos.WB.EasyCalc.Console\WpfApp1\bin\Debug\TestData\tttt.xlsm", FileMode.CreateNew, FileAccess.Write))
            //{
            //    wbk.Write(fs);
            //    fs.Close();
            //}
        }

        //public List<CellByRowColumnNo> WriteCellList(List<CellByName> cellList)
        //{
        //    var cellMappedList = cellList.Select(x => CellByNameToCellByRowColumnNo(x)).ToList();
        //    return cellMappedList;
        //}

        public void WriteCellList(List<CellByRowColumnNo> cellList)
        {
            string fileName = @"K:\It\Work\Grundfos\2020-03-03\Repos_my\Repo\wbeasycalc\Grundfos.WB.EasyCalc.Console\WpfApp1\bin\Debug\TestData\tttt.xlsm";
            WriteCellList(fileName, cellList);
        }

        public void WriteCellList(string fileName, List<CellByRowColumnNo> cellList)
        {
            XSSFWorkbook wbk;
            //using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            //{
            //    wbk = new XSSFWorkbook(fs);
            //    fs.Close();
            //}
            wbk = Workbook;

            foreach (var cellItem in cellList)
            {
                var sheet = wbk.GetSheet(cellItem.SheetName);
                if (sheet == null)
                {
                    string message = $"Could not find setting sheet: {cellItem.SheetName}.";
                    throw new Exception(message);
                }

                IRow row = sheet.GetRow(cellItem.RowNo);
                ICell cell = row.GetCell(cellItem.ColumnNo);

                //T value = this.GetCellValue<T>(valueCell);
                SetCellValue(cell, cellItem.CellValue, cellItem.CellValueType);

            }

            //using (FileStream fs = new FileStream(@"K:\It\Work\Grundfos\2020-03-03\Repos_my\Repo\wbeasycalc\Grundfos.WB.EasyCalc.Console\WpfApp1\bin\Debug\TestData\tttt.xlsm", FileMode.CreateNew, FileAccess.Write))

            if (File.Exists(fileName)) { File.Delete(fileName); }

            using (FileStream fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
            {
                wbk.Write(fs);
                fs.Close();
            }
        }

        public void WriteToFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName)) { File.Delete(fileName); }

                using (FileStream fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
                {
                    Workbook.Write(fs);
                    fs.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void WriteCellValue<T>(ICell cell, T value)
        {
            SetCellValue(cell, value);
        }


        private void SetCellValue<T>(ICell cell, T value)
        {
            Type t = typeof(T);
            if (t == typeof(int))
            {
                cell.SetCellValue(Convert.ToInt32(value));
                return;
            }
            if (t == typeof(double))
            {
                cell.SetCellValue(Convert.ToDouble(value)); 
                return;
            }
            if (t == typeof(string))
            {
                cell.SetCellValue(value==null ? string.Empty : value.ToString()); 
                return;
            }
            if (typeof(T) == typeof(bool))
            {
                cell.SetCellValue(Convert.ToBoolean(value)); 
                return;
            }
            if (typeof(T) == typeof(DateTime))
            {
                cell.SetCellValue(Convert.ToBoolean(value)); 
                return;
            }
            if (typeof(T) == typeof(DateTime))
            {
                cell.SetCellValue(Convert.ToDateTime(value)); 
                return;
            }

            throw new ArgumentException($"The requested type is not supported: {t.ToString()}", nameof(T));
        }
        private void SetCellValue(ICell cell, object value, Type t)
        {
            //Type t = typeof(T);
            if (t == typeof(int))
            {
                cell.SetCellValue(Convert.ToInt32(value));
                return;
            }
            if (t == typeof(double))
            {
                cell.SetCellValue(Convert.ToDouble(value));
                return;
            }
            if (t == typeof(string))
            {
                cell.SetCellValue(value.ToString());
                return;
            }
            if (t == typeof(bool))
            {
                cell.SetCellValue(Convert.ToBoolean(value));
                return;
            }
            if (t == typeof(DateTime))
            {
                cell.SetCellValue(Convert.ToDateTime(value));
                return;
            }

            throw new ArgumentException($"The requested type is not supported: {t.ToString()}", nameof(t));
        }
    }
}
