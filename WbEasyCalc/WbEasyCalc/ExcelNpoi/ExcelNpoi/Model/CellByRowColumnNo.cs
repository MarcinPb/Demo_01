using System;

namespace ExcelNpoi.Model
{
    public class CellByRowColumnNo
    {
        public string SheetName { get; set; }
        public int ColumnNo { get; set; }
        public int RowNo { get; set; }
        public object CellValue { get; set; }
        public Type CellValueType { get; set; }

        public CellByRowColumnNo()
        {
        }

        public CellByRowColumnNo(string sheetName, int columnNo, int rowNo, object value, Type t)
        {
            SheetName = sheetName;
            ColumnNo = columnNo;
            RowNo = rowNo;
            CellValue = value;
            CellValueType = t;
        }
        //public CellByRowColumnNo(string sheetName, int columnNo, int rowNo, T value, Type t)
        //{
        //    SheetName = sheetName;
        //    ColumnNo = columnNo;
        //    RowNo = rowNo;
        //    CellValue = value;
        //    CellValueType = t;
        //}

        public override string ToString()
        {
            return $"{SheetName} - {ColumnNo} - {RowNo} - {CellValue} - {CellValueType}";
        }
    }
}
