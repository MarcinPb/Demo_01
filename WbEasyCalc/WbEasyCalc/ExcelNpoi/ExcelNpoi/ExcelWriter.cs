using System.IO;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
//using Grundfos.WG.Model;

namespace ExcelNpoi
{
    public class ExcelWriter
    {
        /*
        private string _sqliteDbFile = @"C:\WG2TW\WGModel\testOPC.wtg.sqlite";

        public static void Write(
            string excelFileName, 
            Dictionary<int, string> idahoPatternList,
            IList<IdahoPatternPatternCurve> idahoPatternPatternCurveList,
            IList<WaterDemandData> waterDemandDataList,
            IList<Pipe> pipeList,
            Dictionary<int, string> zoneList
            )
        {
            using (var fs = new FileStream(excelFileName, FileMode.Create, FileAccess.Write))
            {
                string[] strArr = { " - " };
                int rowIndex;
                IRow row;

                IWorkbook workbook = new XSSFWorkbook();

                ISheet sheet1 = workbook.CreateSheet("DemandPatterns");
                rowIndex = 0;
                row = sheet1.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue("Pattern Name");
                row.CreateCell(1).SetCellValue("Start (min)");
                row.CreateCell(2).SetCellValue("Value");

                int? missingSupportElementId = idahoPatternPatternCurveList
                    .FirstOrDefault(x => idahoPatternList.All(y => x.SupportElementId != y.Key))?.SupportElementId;
                if (missingSupportElementId.HasValue)
                {
                    throw new Exception($"Could not find DemandPattern for DemandPatternCurve ID: {missingSupportElementId}.");
                }

                var list = idahoPatternPatternCurveList.Join(
                    idahoPatternList,
                    l => l.SupportElementId,
                    r => r.Key,
                    (l, r) => new { IdahoPatternPatternCurve = l, IdahoPatternName = r.Value }
                );
                foreach (var item in list)
                {
                    rowIndex++;
                    row = sheet1.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(item.IdahoPatternName);
                    row.CreateCell(1).SetCellValue(item.IdahoPatternPatternCurve.PatternCurveTimeFromStart/60);
                    row.CreateCell(2).SetCellValue(item.IdahoPatternPatternCurve.PatternCurveMultiplier);
                }

                ISheet sheet2 = workbook.CreateSheet("ExcludedItems");
                rowIndex = 0;
                row = sheet2.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue("Excluded Object IDs");
                row.CreateCell(1).SetCellValue("Excluded Demand Patterns");

                ISheet sheet3 = workbook.CreateSheet("Zones");
                rowIndex = 0;
                row = sheet3.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue("Zone Name");
                row.CreateCell(1).SetCellValue("OPC Zone Demand Tag");
                foreach (var item in zoneList)
                {
                    rowIndex++;
                    row = sheet3.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(item.Value);
                    row.CreateCell(1).SetCellValue($"Control.DEV.ZoneDemand_{item.Value.Split(strArr, StringSplitOptions.None)[1]}");
                }

                ISheet sheet4 = workbook.CreateSheet("OpcMapping");
                rowIndex = 0;
                row = sheet4.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue("FieldName");
                row.CreateCell(1).SetCellValue("Element ID");
                row.CreateCell(2).SetCellValue("Element Label");
                row.CreateCell(3).SetCellValue("Enabled");
                row.CreateCell(4).SetCellValue("OPC Tag");
                row.CreateCell(5).SetCellValue("Result Attribute Label");
                // Pipe
                foreach (var item in pipeList)
                {
                    rowIndex++;
                    row = sheet4.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue("PipeStatus");
                    row.CreateCell(1).SetCellValue(item.Id);
                    row.CreateCell(2).SetCellValue(item.Label);
                    row.CreateCell(3).SetCellValue(item.IsActive);
                    row.CreateCell(4).SetCellValue($"PipeIsOpen.DEV.{item.Label}_{item.Id}");
                    row.CreateCell(5).SetCellValue("Is Open?");
                }
                // Hydrant
                foreach (var item in waterDemandDataList.Where(x => x.ObjectTypeID == 54))
                {
                    rowIndex++;
                    row = sheet4.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue("TankPercentFull");
                    row.CreateCell(1).SetCellValue(item.ObjectID);
                    row.CreateCell(2).SetCellValue(item.ObjectName);
                    row.CreateCell(3).SetCellValue(item.IsActive);
                    row.CreateCell(4).SetCellValue($"Other.DEV.TankPrcFul_{item.ObjectName}_{item.ObjectID}");
                    row.CreateCell(5).SetCellValue("Percent Full");
                }
                // Zone
                foreach (var item in zoneList)
                {
                    rowIndex++;
                    row = sheet4.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue("ZoneAveragePressure");
                    row.CreateCell(1).SetCellValue(item.Key);
                    row.CreateCell(2).SetCellValue(item.Value);
                    row.CreateCell(3).SetCellValue(true);
                    row.CreateCell(4).SetCellValue($"Other.DEV.ZoneAvgPrs_{item.Value.Split(strArr, StringSplitOptions.None)[1]}");
                    row.CreateCell(5).SetCellValue("None");
                }

                ISheet sheet5 = workbook.CreateSheet("ObjectData");
                rowIndex = 0;
                row = sheet5.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue("ObjectID");
                row.CreateCell(1).SetCellValue("ObjectTypeID");
                row.CreateCell(2).SetCellValue("DemandPatternName");
                row.CreateCell(3).SetCellValue("BaseDemandValue");
                row.CreateCell(4).SetCellValue("ZoneName");
                row.CreateCell(5).SetCellValue("IsActive");
                foreach (var item in waterDemandDataList)
                {
                    rowIndex++;
                    row = sheet5.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(item.ObjectID);
                    row.CreateCell(1).SetCellValue(item.ObjectTypeID);
                    row.CreateCell(2).SetCellValue(item.DemandPatternName);
                    row.CreateCell(3).SetCellValue(item.BaseDemandValue);
                    row.CreateCell(4).SetCellValue(item.ZoneName);
                    row.CreateCell(5).SetCellValue(item.IsActive);
                }

                ISheet sheet6 = workbook.CreateSheet("ApplicationSettings");
                rowIndex = 0;
                row = sheet6.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue("Name");
                row.CreateCell(1).SetCellValue("Value");
                rowIndex++;
                row = sheet6.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue("SimulationStartDate");
                row.CreateCell(1).SetCellValue(new DateTime(2019, 9, 30, 12, 15, 0));
                rowIndex++;
                row = sheet6.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue("SimulationIntervalMinutes");
                row.CreateCell(1).SetCellValue(10);

                workbook.Write(fs);
            }
        }
        */

        public static void Write1()
        {
            var newFile = @"C:\WG2TW\Grundfos.WG.PostCalc\newbook.core.xlsx";

            using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {

                IWorkbook workbook = new XSSFWorkbook();

                ISheet sheet1 = workbook.CreateSheet("Sheet1");

                sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
                var rowIndex = 0;
                IRow row = sheet1.CreateRow(rowIndex);
                row.Height = 30 * 80;
                row.CreateCell(0).SetCellValue("this is content");
                sheet1.AutoSizeColumn(0);
                rowIndex++;

                var sheet2 = workbook.CreateSheet("Sheet2");
                var style1 = workbook.CreateCellStyle();
                style1.FillForegroundColor = HSSFColor.Blue.Index2;
                style1.FillPattern = FillPattern.SolidForeground;

                var style2 = workbook.CreateCellStyle();
                style2.FillForegroundColor = HSSFColor.Yellow.Index2;
                style2.FillPattern = FillPattern.SolidForeground;

                var cell2 = sheet2.CreateRow(0).CreateCell(0);
                cell2.CellStyle = style1;
                cell2.SetCellValue(0);

                cell2 = sheet2.CreateRow(1).CreateCell(0);
                cell2.CellStyle = style2;
                cell2.SetCellValue(1);

                workbook.Write(fs);
            }
        }

    }
}
