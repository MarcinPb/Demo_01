using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grundfos.OPC;
using Grundfos.WG.Model;
using Grundfos.Workbooks;
using Haestad.Support.OOP.Logging;
using Grundfos.WaterDemandCalculation.ExtensionMethods;

namespace Grundfos.WaterDemandCalculation
{
    public class ZoneDemandDataListCreatorNew
    {
        private readonly DataContext _dataContext;
        private readonly ActionLogger _logger;

        public double GetMinutesFromMonday { get; set; }

        public ZoneDemandDataListCreatorNew(DataContext dataContext, ActionLogger logger = null)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public List<ZoneDemandData> Create(DateTime calcTime)
        {
            try
            {
                _logger?.WriteMessage(OutputLevel.Info, $"Run: ZoneDemandDataListCreatorNew.Create({calcTime})");


                DataSet dataSet = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(_dataContext.WaterInfraConnString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter
                    {
                        SelectCommand = new SqlCommand("spPostCalcGetObjDemand", sqlConn)
                        {
                            CommandType = CommandType.StoredProcedure
                        }
                    };
                    adapter.SelectCommand.Parameters.Add("@CalcTime", SqlDbType.DateTime).Value = calcTime;
                    adapter.Fill(dataSet);
                }

                List<ZoneDemandData> list = CreateList(dataSet);

                return list;
            }
            catch (Exception e)
            {
                _logger?.WriteMessage(OutputLevel.Errors, $"Getting MSSQL data.\n{e.Message}");
                throw;
            }
        }

        private List<ZoneDemandData> CreateList(DataSet dataSet)
        {
            var list = dataSet.Tables[0].AsEnumerable().Select(x => new 
            {
                ZoneId = x.Field<int>("ZoneId"),
                ZoneName = x.Field<string>("ZoneName"),
                WgDemand = x.Field<double>("WgDemand"),
                ScadaDemand = x.Field<double>("ScadaDemand"),
                DemandAdjustmentRatio = x.Field<double>("DemandAdjustmentRatio"),
                DemandAdjustmentRatioDb = x.Field<double>("DemandAdjustmentRatio"),
            })
            .Distinct()
            .Select(y => new ZoneDemandData { 
                ZoneId = y.ZoneId, 
                ZoneName = y.ZoneName,
                WgDemand = y.WgDemand,
                ScadaDemand = y.ScadaDemand,
                DemandAdjustmentRatio = y.DemandAdjustmentRatio,
                DemandAdjustmentRatioDb = y.DemandAdjustmentRatioDb,
            })
            .ToList();

            list.ForEach(x => x.Demands = CreateDemands(x.ZoneId, dataSet));

            return list;            
        }

        private List<WaterDemandData> CreateDemands(int zoneId, DataSet dataSet)
        {
            var list = dataSet.Tables[0].AsEnumerable()
                .Where(f => f.Field<int>("ZoneId") == zoneId)
                .Select(x => new WaterDemandData
                {
                    ZoneID = x.Field<int>("ZoneId"),
                    ZoneName = x.Field<string>("ZoneName"),

                    ObjectID = x.Field<int>("ObjectId"),
                    ObjectTypeID = x.Field<int>("ObjectTypeId"),
                    ObjectIsExcluded = x.Field<bool>("ObjectIsExcluded"),

                    DemandPatternID = x.Field<int>("DemandPatternId"),
                    DemandPatternName = x.Field<string>("DemandPatternName"),
                    DemandIsExcluded = x.Field<bool>("DemandIsExcluded"),

                    BaseDemandValue = x.Field<double>("BaseDemandValue"),
                    DemandFactorValue = x.Field<double>("DemandFactorValue"),
                    ActualDemandValue = x.Field<double>("ActualDemandValue"),
                    DemandCalculatedValue = x.Field<double>("DemandCalculatedValue"),
                    DemandCalculatedValueDb = x.Field<double>("DemandCalculatedValue"),
                })
                .ToList();

            return list;
        }







        /*


                var excelReader = new ExcelReader(_dataContext.ExcelFileName);
                DateTime excelSettingSimulationStartTime = new DateTime(2019, 09, 30, 12, 15, 0);
                int excelSettingSimulationIntervalMinutes = 10;

                // Fill ICollection<WaterDemandData> objectDemandData
                var objectDataExcelReader = new ObjectDataExcelReader(excelReader);
                ICollection<WaterDemandData> objectDemandData = objectDataExcelReader.ReadObjects(); // Excel.ObjectData

                var demandPatternExcelReader = new DemandPatternExcelReader(excelReader);

                // List<int> <- Excel.ExcludedItems["Excluded Object IDs"].
                // {257=PC, 2719=S5, 518=CP1, 701=CP2, 1323=CP3, 1336=CP4, 2255=S6, 2780=S7, 1247=W1, 1239=W2, 1548=CP6}    
                var excludedObjectList = demandPatternExcelReader.ReadExcludedObjects();
                this.UpdateObjectIsExcluded(objectDemandData, excludedObjectList);

                // List<string> <- Excel.ExcludedItems["Excluded Demand Patterns"].
                // {"nieaktywni", "Straty"}.    
                var excludedDemandPatternList = demandPatternExcelReader.ReadExcludedPatterns();
                this.UpdateDemandPatternIsExcluded(objectDemandData, excludedDemandPatternList);

                this.FillPatternIds(objectDemandData, _dataContext.WgDemandPatternDict);
                this.FillZoneIdsInWaterDemands(objectDemandData, _dataContext.WgZoneDict);

                // List<ZoneDemandData> zoneDemandDataList <-- group by <-- ICollection<WaterDemandData> objectDemandData 
                List<ZoneDemandData> zoneDemandDataList = objectDemandData
                    .GroupBy(x => x.ZoneName)
                    .Select(x => new ZoneDemandData { ZoneId = x.FirstOrDefault()==null ? 0 : x.FirstOrDefault().ZoneID, ZoneName = x.Key, Demands = x.ToList() })
                    .ToList();
                var noZoneDemands = zoneDemandDataList.Where(x => string.IsNullOrEmpty(x.ZoneName)).ToList();
                if (noZoneDemands.Count > 0)
                {
                    var numberOfItems = noZoneDemands.Sum(x => x.Demands.Count);
                    _logger?.WriteMessage(OutputLevel.Warnings, $"Found {numberOfItems} demands with no zone specified.");
                    noZoneDemands.ForEach(x => zoneDemandDataList.Remove(x));
                }

                var waterDemandExcelReader = new DemandPatternExcelReader(excelReader);

                // Fill OpcTag in zoneDemandDataList
                List<string> missingZoneOpcTags = GetMissingZoneOpcTags(waterDemandExcelReader, zoneDemandDataList);
                if (missingZoneOpcTags.Count > 0)
                {
                    string message = $"Could not find OPC tag for zones {string.Join(", ", missingZoneOpcTags)}";
                    _logger?.WriteMessage(OutputLevel.Warnings, message);
                }


                // Demand patterns and interpolation
                var adjuster = new TimeshiftAdjuster(7 * 24 * 60);
                var interpolator = new Interpolator(adjuster);
                var patternService = new DemandPatternService(waterDemandExcelReader);
                var demandService = new DemandService(interpolator, patternService);
                var settings = new SimulationTimeResolverConfiguration
                {
                    SimulationStartTime = excelSettingSimulationStartTime,              // 2019-09-30 12:15
                    SimulationIntervalMinutes = excelSettingSimulationIntervalMinutes,  // 10
                };
                var totalDemandCalculation = new TotalDemandCalculation(demandService, new SimulationTimeResolver(settings));
                // Update zoneDemandData.Demands.DemandFactorValue <- Excel.DemandPatterns.Value
                zoneDemandDataList.ForEach(x => totalDemandCalculation.UpdateDemandFactorValue(x.Demands, _dataContext.StartComputeTime));
                // Set up MinutesFromMonday.
                var simulationTimeResolver = new SimulationTimeResolver(settings);
                var simulationTimestamp = simulationTimeResolver.GetSimulationTimestamp(_dataContext.StartComputeTime);
                GetMinutesFromMonday = simulationTimestamp.MinutesFromMonday();


                // Update zoneDemandData.ScadaDemand <- OPC.10MinValue
                //double demandScadaElement_05;
                //double demandScadaElement_06;
                using (var opc = new OpcReader(_dataContext.OpcServerAddress))
                {
                    zoneDemandDataList.ForEach(x => x.ScadaDemand = opc.GetDouble(x.OpcTag));
                }

                // Calculate and update:
                //  zone.WgDemand
                //  zone.DemandWgExcluded
                //  zone.DemandAdjustmentRatio
                //  zone.demand.ActualDemandValue
                zoneDemandDataList.ForEach(zone =>
                {
                    zone.Demands
                        .ForEach(
                            demand =>
                            demand.ActualDemandValue = 
                                demand.ObjectIsExcluded || demand.DemandIsExcluded ? 
                                demand.BaseDemandValue : 
                                demand.BaseDemandValue * demand.DemandFactorValue
                         );
                    zone.WgDemand = 
                        zone.Demands
                        .Sum(demand => demand.ActualDemandValue);
                    //zone.DemandAdjustmentRatio = 
                    //    zone.ScadaDemand / zone.WgDemand;
                    zone.DemandWgExcluded =
                        zone.Demands
                        .Where(demand => demand.ObjectIsExcluded || demand.DemandIsExcluded)
                        .Sum(demand => demand.ActualDemandValue);
                    zone.DemandAdjustmentRatio =
                        Math.Abs(zone.WgDemand - zone.DemandWgExcluded) < 0.000001 ?
                        0 :
                        (zone.ScadaDemand - zone.DemandWgExcluded) / (zone.WgDemand - zone.DemandWgExcluded);
                    foreach (var demand in zone.Demands)
                    {
                        demand.DemandCalculatedValue =
                            demand.ObjectIsExcluded || demand.DemandIsExcluded ?
                            demand.ActualDemandValue :
                            demand.ActualDemandValue * zone.DemandAdjustmentRatio; 
                            // * Constants.Flow_M3H_2_WG;
                    }
                });

                return zoneDemandDataList;
            }
            catch (Exception e)
            {
                _logger?.WriteMessage(OutputLevel.Errors, $"Creating data.\n{e.Message}");
                throw;
            }
        }
        */
        public class DataContext
        {
            public string WaterInfraConnString { get; set; }
        }
    }
}
