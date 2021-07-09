using Database.DataModel.Infra;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelNpoi.ExcelNpoi
{
    public class PostCalcExcelWriter
    {
        private class ObjectDataSheet
        {
            public int ObjTypeId { get; set; }
            public int ObjId { get; set; }
            public bool IsActive { get; set; }
            public string ZoneName { get; set; }
            public double? DemandBase { get; set; }
            public string DemandPatternName { get; set; }
        }
        private class OpcMappingSheet
        {
            public int ObjTypeId { get; set; }
            public int ObjId { get; set; }
            public string Label { get; set; }
            public bool IsActive { get; set; }
        }
    


        public static void Write(string excelFileName, InfraChangeableDataLists infraChangeableDataList, InfraSpecialFieldId infraSpecialFieldId)
        {
            List<InfraDemandPattern> demandPatternList = infraChangeableDataList.DemandPatternDict;
            List<InfraDemandPatternCurve> demandPatternCurveList = infraChangeableDataList.DemandPatternCurveList;
            List<InfraZone> zoneList = infraChangeableDataList.ZoneDict;

            // Validation
            int? missingSupportElementId = demandPatternCurveList
                .FirstOrDefault(x => demandPatternList.All(y => x.DemandPatternId != y.DemandPatternId))?.DemandPatternId;
            if (missingSupportElementId.HasValue)
            {
                throw new Exception($"Could not find DemandPattern for DemandPatternCurve ID: {missingSupportElementId}.");
            }


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
                // 
                var list = demandPatternCurveList.Join(
                    demandPatternList,
                    l => l.DemandPatternId,
                    r => r.DemandPatternId,
                    (l, r) => new { IdahoPatternPatternCurve = l, IdahoPatternName = r.Name }
                );
                foreach (var item in list)
                {
                    rowIndex++;
                    row = sheet1.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(item.IdahoPatternName);
                    row.CreateCell(1).SetCellValue(item.IdahoPatternPatternCurve.TimeFromStart / 60);
                    row.CreateCell(2).SetCellValue(item.IdahoPatternPatternCurve.Multiplier);
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
                    row.CreateCell(0).SetCellValue(item.Name);
                    //row.CreateCell(1).SetCellValue($"Control.DEV.ZoneDemand_{item.Name.Split(strArr, StringSplitOptions.None)[1]}");
                    row.CreateCell(1).SetCellValue($"Control.DEV.ZoneDemand_{GetOpcZoneDemandTag(item.Name)}");
                }
                rowIndex++;
                row = sheet3.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue("Total Demand");
                row.CreateCell(1).SetCellValue("Control.DEV.ScadaTotalDemand");


                ISheet sheet4 = workbook.CreateSheet("OpcMapping");
                rowIndex = 0;
                row = sheet4.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue("FieldName");
                row.CreateCell(1).SetCellValue("Element ID");
                row.CreateCell(2).SetCellValue("Element Label");
                row.CreateCell(3).SetCellValue("Enabled");
                row.CreateCell(4).SetCellValue("OPC Tag");
                row.CreateCell(5).SetCellValue("Result Attribute Label");
                foreach (var item in GetOpcMappingSheetList(infraChangeableDataList, infraSpecialFieldId).OrderByDescending(x => x.ObjTypeId).ThenBy(x => x.ObjId))
                {
                    rowIndex++;
                    row = sheet4.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(item.ObjTypeId==69 ? "PipeStatus" : "TankPercentFull");
                    row.CreateCell(1).SetCellValue(item.ObjId);
                    row.CreateCell(2).SetCellValue(item.Label);
                    row.CreateCell(3).SetCellValue(item.IsActive.ToString().ToUpper());
                    row.CreateCell(4).SetCellValue($"Other.DEV.TankPrcFul_{item.Label}_{item.ObjId}");
                    row.CreateCell(5).SetCellValue(item.ObjTypeId == 69 ? "Is Open?" : "Percent Full");
                }
                // Zone
                foreach (var item in zoneList)
                {
                    rowIndex++;
                    row = sheet4.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue("ZoneAveragePressure");
                    row.CreateCell(1).SetCellValue(item.ZoneId);
                    row.CreateCell(2).SetCellValue(item.Name);
                    row.CreateCell(3).SetCellValue(true);
                    //row.CreateCell(4).SetCellValue($"Other.DEV.ZoneAvgPrs_{item.Name.Split(strArr, StringSplitOptions.None)[1]}");
                    row.CreateCell(4).SetCellValue($"Other.DEV.ZoneAvgPrs_{GetOpcZoneDemandTag(item.Name)}");
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
                foreach (var item in GetObjectDataSheetList(infraChangeableDataList, infraSpecialFieldId))
                {
                    rowIndex++;
                    row = sheet5.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(item.ObjId);
                    row.CreateCell(1).SetCellValue(item.ObjTypeId);
                    row.CreateCell(2).SetCellValue(item.DemandPatternName);
                    row.CreateCell(3).SetCellValue(item.DemandBase ?? 0);
                    row.CreateCell(4).SetCellValue(item.ZoneName);
                    row.CreateCell(5).SetCellValue(((bool)item.IsActive).ToString().ToUpper());
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
        private static List<OpcMappingSheet> GetOpcMappingSheetList(InfraChangeableDataLists infraChangeableDataList, InfraSpecialFieldId infraSpecialFieldId)
        {
            var objValueLabelList = infraChangeableDataList.InfraObjList
                .Where(f =>
                    f.ObjTypeId == 54 ||    // Hydrant
                    f.ObjTypeId == 69       // Pipe
                )
                .Join(
                    infraChangeableDataList.InfraValueList.Where(f =>
                        f.FieldId == infraSpecialFieldId.Label      // Label=2
                    ),
                    l => l.ObjId,
                    r => r.ObjId,
                    (l, r) => new  
                    { 
                        ObjId = l.ObjId, 
                        ObjTypeId = l.ObjTypeId, 
                        Label = r.StringValue, 
                    }
                )
                .ToList();

            var objValueIsActiveList = infraChangeableDataList.InfraObjList
                .Where(f =>
                    f.ObjTypeId == 54 ||    // Hydrant
                    f.ObjTypeId == 69       // Pipe
                )
                .Join(
                    infraChangeableDataList.InfraValueList.Where(f =>
                        f.FieldId == infraSpecialFieldId.HMIActiveTopologyIsActive    // HMIActiveTopologyIsActive=645
                    ),
                    l => l.ObjId,
                    r => r.ObjId,
                    (l, r) => new  
                    { 
                        ObjId = l.ObjId, 
                        ObjTypeId = l.ObjTypeId, 
                        IsActive = (bool)r.BooleanValue, 
                    }
                )
                .ToList();

            var objValueList = objValueLabelList.Join(
                objValueIsActiveList,
                l => l.ObjId,
                r => r.ObjId,
                (x, y) => new OpcMappingSheet
                    {
                        ObjTypeId = x.ObjTypeId,
                        ObjId = x.ObjId,
                        Label = x.Label,
                        IsActive = y.IsActive,
                    }
                )
                .ToList();

            return objValueList;
        }

        private static List<ObjectDataSheet> GetObjectDataSheetList(InfraChangeableDataLists infraChangeableDataList, InfraSpecialFieldId infraSpecialFieldId)
        {
            var objValueList = infraChangeableDataList.InfraObjList
                .Where(f =>
                    f.ObjTypeId == 54 ||
                    f.ObjTypeId == 55 ||
                    f.ObjTypeId == 73
                )
                .Join(
                    infraChangeableDataList.InfraValueList.Where(f =>
                        f.FieldId == infraSpecialFieldId.Demand_AssociatedElement ||    // Demand_AssociatedElement=769
                        f.FieldId == infraSpecialFieldId.Demand_BaseFlow ||             // Demand_BaseFlow=767 
                        f.FieldId == infraSpecialFieldId.Demand_DemandPattern ||        // Demand_DemandPattern=768 
                        f.FieldId == infraSpecialFieldId.DemandCollection ||            // DemandCollection=757 
                        f.FieldId == infraSpecialFieldId.HMIActiveTopologyIsActive ||   // HMIActiveTopologyIsActive=645        
                        f.FieldId == infraSpecialFieldId.Physical_Zone                  // Physical_Zone=647
                    ),
                    l => l.ObjId,
                    r => r.ObjId,
                    (l, r) => new { l.ObjId, l.ObjTypeId, r.ValueId, r.FieldId, r.IntValue, r.StringValue, r.FloatValue, r.BooleanValue }
                )
                .ToList();

            // IsActive
            var objIsActiveList = objValueList.Where(f => f.FieldId == 645).Select(x => new { x.ObjTypeId, x.ObjId, IsActive = x.BooleanValue }).ToList();

            // ObjId, ZoneName
            // Junction
            var junctionZoneList = objValueList
                .Where(f =>
                    f.FieldId == infraSpecialFieldId.Physical_Zone &&          // Physical_Zone=647
                    (
                        f.ObjTypeId == 54 ||
                        f.ObjTypeId == 55
                    )
                )
                .Join(
                    infraChangeableDataList.ZoneDict,
                    l => l.IntValue,
                    r => r.ZoneId,
                    (x, y) => new { x.ObjId, ZoneName = y.Name }
                )
                .ToList();
            // CustomerMeter
            var customerMeterZoneList = objValueList
                .Where(f =>
                    f.ObjTypeId == 73 &&                                        // CustomerMeterTypeId
                    f.FieldId == infraSpecialFieldId.Demand_AssociatedElement   // Demand_AssociatedElement=769
                )
                .Join(
                    junctionZoneList,
                    l => l.IntValue,
                    r => r.ObjId,
                    (x, y) => new { x.ObjId, y.ZoneName }
                )
                .ToList();
            // All
            var objZoneList = junctionZoneList.Union(customerMeterZoneList).ToList();


            // ObjId, DemandBase, DemandPattern
            var demandBaseWithName = infraChangeableDataList.DemandBaseList
                .Join(
                    infraChangeableDataList.DemandPatternDict,
                    l => l.DemandPatternId,
                    r => r.DemandPatternId,
                    (x, y) => new { x.ValueId, x.DemandPatternId, y.Name, x.DemandBase }
                )
                .ToList();
            // Junction
            var objJunctionDemandPatternList = objValueList
                .Join(
                    demandBaseWithName,
                    l => l.ValueId,
                    r => r.ValueId,
                    (x, y) => new { x.ObjId, y.DemandBase, DemandPatternName = y.Name }
                )
                .ToList();
            // CustomerMeter DemandBase
            var objCustomerMeterDemandBaseList = objValueList
                .Where(f => f.FieldId == infraSpecialFieldId.Demand_BaseFlow)           // Demand_BaseFlow=767
                .Select(x => new { x.ObjId, DemandBase = (double)x.FloatValue })
                .ToList();
            // CustomerMeter DemandPattern
            var objCustomerMeterDemandPatternIdList = objValueList
                .Where(f => f.FieldId == infraSpecialFieldId.Demand_DemandPattern)      // Demand_DemandPattern=768
                .Select(x => new { x.ObjId, DemandPatternId = x.IntValue ?? -1 })
                .ToList();
            // CustomerMeter
            var objCustomerMeterDemandPatternList = objCustomerMeterDemandBaseList
                .Join(
                    objCustomerMeterDemandPatternIdList,
                    l => l.ObjId,
                    r => r.ObjId,
                    (x, y) => new { x.ObjId, x.DemandBase, y.DemandPatternId }
                )
                .Join(
                    infraChangeableDataList.DemandPatternDict,
                    l => l.DemandPatternId,
                    r => r.DemandPatternId,
                    (x, y) => new { x.ObjId, x.DemandBase, DemandPatternName = y.Name }
                )
                .ToList();
            // All
            var objDemandPatternList = objJunctionDemandPatternList.Union(objCustomerMeterDemandPatternList).ToList();



            var objIsActiveZoneList = objIsActiveList
                .GroupJoin(
                    objZoneList,
                    l => l.ObjId,
                    r => r.ObjId,
                    (i, list) => new { Item = i, List = list }
                )
                .SelectMany(
                    list1 => list1.List.DefaultIfEmpty(),
                    (x, y) => new { x.Item.ObjTypeId, x.Item.ObjId, x.Item.IsActive, y?.ZoneName }
                )
                .ToList();

            var objIsActiveZoneDemandList = objIsActiveZoneList
                .GroupJoin(
                    objDemandPatternList,
                    l => l.ObjId,
                    r => r.ObjId,
                    (i, list) => new { Item = i, List = list }
                )
                .SelectMany(
                    list1 => list1.List.DefaultIfEmpty(),
                    //(x, y) => new { x.Item.ObjTypeId, x.Item.ObjId, x.Item.IsActive, x.Item.ZoneName, y?.DemandBase, y?.DemandPatternName }
                    (x, y) => new ObjectDataSheet 
                    { 
                        ObjTypeId = x.Item.ObjTypeId, 
                        ObjId = x.Item.ObjId, 
                        IsActive = (bool)x.Item.IsActive, 
                        ZoneName = x.Item.ZoneName, 
                        DemandBase = y?.DemandBase, 
                        DemandPatternName = y?.DemandPatternName 
                    }
                )
                .ToList();

            return objIsActiveZoneDemandList;
        }

        private static string GetOpcZoneDemandTag(string zoneName)
        {
            string[] strArr = { " - " };
            zoneName = zoneName.Split(strArr, StringSplitOptions.None)[1];

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add(" ", "");
            dict.Add(".", "");
            dict.Add("ó", "o");
            dict.Add("ł", "l");

            foreach(var item in dict)
            {
                zoneName = zoneName.Replace(item.Key, item.Value);
            }

            return zoneName;
        }
    }
}
