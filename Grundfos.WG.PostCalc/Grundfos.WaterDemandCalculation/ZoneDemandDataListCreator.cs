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
    public class ZoneDemandDataListCreator
    {
        private readonly DataContext _dataContext;
        private readonly ActionLogger _logger;

        public double GetMinutesFromMonday { get; set; }

        public ZoneDemandDataListCreator(DataContext dataContext, ActionLogger logger = null)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public List<ZoneDemandData> Create()
        {
            try
            {
                _logger?.WriteMessage(OutputLevel.Info, $"Go ....{_dataContext.WgDemandPatternDict.Count}");

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

        private void UpdateObjectIsExcluded(ICollection<WaterDemandData> objectDemandData, List<int> excludedObjectList)
        {
            objectDemandData.ToList().ForEach(x => x.ObjectIsExcluded = excludedObjectList.Any(y => x.ObjectID==y));
        }

        private void UpdateDemandPatternIsExcluded(ICollection<WaterDemandData> objectDemandData, List<string> excludedDemandPatternList)
        {
            objectDemandData.ToList().ForEach(x => x.DemandIsExcluded = excludedDemandPatternList.Any(y => x.DemandPatternName==y));
        }

        public class DataContext
        {
            public Dictionary<int, string> WgZoneDict { get; set; }
            public Dictionary<int, string> WgDemandPatternDict { get; set; }
            public string ExcelFileName { get; set; }
            public string OpcServerAddress { get; set; }
            public DateTime StartComputeTime { get; set; }
            //public Dictionary<string, float> OpcZonesDict { get; set; }
        }

        private void FillPatternIds(ICollection<WaterDemandData> demands, Dictionary<int, string> patterns)
        {
            foreach (var item in demands)
            {
                if (patterns.All(x => x.Value != item.DemandPatternName))
                {
                    throw new Exception($"Could not find pattern definition for pattern ID: {item.DemandPatternName}.");
                }
                item.DemandPatternID = patterns.FirstOrDefault(x => x.Value == item.DemandPatternName).Key;
            }
        }

        private void FillZoneIdsInWaterDemands(ICollection<WaterDemandData> demands, Dictionary<int, string> zones)
        {
            foreach (var item in demands.Where(x => !string.IsNullOrWhiteSpace(x.ZoneName)))
            {
                if (zones.All(x => x.Value != item.ZoneName))
                {
                    throw new Exception($"Could not find zone definition for zone name: {item.ZoneName}.");
                }
                item.ZoneID = zones.FirstOrDefault(x => x.Value == item.ZoneName).Key;
            }
        }

        private static List<string> GetMissingZoneOpcTags(DemandPatternExcelReader excelReader, List<ZoneDemandData> zoneDemands)
        {
            var zoneDefinitions = excelReader.ReadZones();
            var missingZones = new List<string>();
            foreach (var item in zoneDemands)
            {
                var zoneDefinition = zoneDefinitions.FirstOrDefault(x => x.ZoneName.Equals(item.ZoneName, StringComparison.OrdinalIgnoreCase));
                if (zoneDefinition == null)
                {
                    missingZones.Add(item.ZoneName);
                    continue;
                }

                item.OpcTag = zoneDefinition.OpcTag;
            }

            return missingZones;
        }

        public void SaveToDatabase(List<ZoneDemandData> zoneDemandDataList, string conStr, string ratioFormula = null)
        {
            try
            {
                //string query = "SELECT * FROM dbo.ZoneFlowComparison ORDER BY D_TIME DESC";
                using (SqlConnection sqlConn = new SqlConnection(conStr))
                {
                    int logHeaderId;

                    using (SqlCommand cmd = new SqlCommand("spSaveLogHeader", sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@DateTimeIn", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@DateTimeUtcIn", SqlDbType.DateTime).Value = DateTime.UtcNow;
                        cmd.Parameters.Add("@LogTypeId", SqlDbType.Int).Value = 3;  // Set up demands
                        cmd.Parameters.Add("@RatioFormula", SqlDbType.NVarChar, 4000).Value = ratioFormula ?? string.Empty;
                        cmd.Parameters.Add("@MinutesFromMonday", SqlDbType.Float).Value = GetMinutesFromMonday;  // Calculated by last Create method.

                        cmd.Parameters.Add("@LogId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters["@LogId"].Value = 0;

                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        logHeaderId = Convert.ToInt32(cmd.Parameters["@LogId"].Value);
                        sqlConn.Close();
                    }

                    foreach (var zone in zoneDemandDataList)
                    {
                        int logZonetId;

                        using (SqlCommand cmd = new SqlCommand("SaveLogZone", sqlConn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@LogHeaderId", SqlDbType.Int).Value = logHeaderId;
                            cmd.Parameters.Add("@AutoZoneId", SqlDbType.Int).Value = zone.ZoneId;
                            cmd.Parameters.Add("@DemandScada", SqlDbType.Float).Value = zone.ScadaDemand;
                            cmd.Parameters.Add("@DemandWg", SqlDbType.Float).Value = zone.WgDemand;
                            cmd.Parameters.Add("@DemandExcluded", SqlDbType.Float).Value = zone.DemandWgExcluded;
                            cmd.Parameters.Add("@DemandRatio", SqlDbType.Float).Value = zone.DemandAdjustmentRatio;
                            cmd.Parameters.Add("@DemandScadaElement", SqlDbType.Float).Value = zone.DemandScadaElement;

                            cmd.Parameters.Add("@LogId", SqlDbType.Int).Direction = ParameterDirection.Output;
                            cmd.Parameters["@LogId"].Value = 0;

                            sqlConn.Open();
                            cmd.ExecuteNonQuery();
                            sqlConn.Close();
                            logZonetId = Convert.ToInt32(cmd.Parameters["@LogId"].Value);
                        }

                        var objectList = zone
                            .Demands.GroupBy(
                                o1 => new { o1.ObjectID, o1.ObjectTypeID, o1.ObjectIsExcluded },
                                o1 => o1,
                                (key, g) => new { Object1 = key, DemandList = g.ToList() }
                            )
                            .Select(o2 => new
                            {
                                o2.Object1.ObjectID,
                                o2.Object1.ObjectTypeID,
                                o2.Object1.ObjectIsExcluded,
                                //BaseDemandValue = o2.DemandList.Sum(d => d.BaseDemandValue),
                                o2.DemandList
                            })
                            ;

                        foreach (var obj in objectList)
                        {
                            int logObjectId;

                            using (SqlCommand cmd = new SqlCommand("SaveLogObject", sqlConn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@LogZoneId", SqlDbType.Int).Value = logZonetId;
                                cmd.Parameters.Add("@AutoObjectId", SqlDbType.Int).Value = obj.ObjectID;
                                //cmd.Parameters.Add("@Demand", SqlDbType.Float).Value = obj.BaseDemandValue;
                                cmd.Parameters.Add("@IsExcluded", SqlDbType.Bit).Value = obj.ObjectIsExcluded;

                                cmd.Parameters.Add("@LogId", SqlDbType.Int).Direction = ParameterDirection.Output;
                                cmd.Parameters["@LogId"].Value = 0;

                                sqlConn.Open();
                                cmd.ExecuteNonQuery();
                                sqlConn.Close();
                                logObjectId = Convert.ToInt32(cmd.Parameters["@LogId"].Value);
                            }


                            foreach (var demand in obj.DemandList)
                            {
                                using (SqlCommand cmd = new SqlCommand("SaveLogDemand", sqlConn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@LogObjectId", SqlDbType.Int).Value = logObjectId;
                                    cmd.Parameters.Add("@AutoDemandId", SqlDbType.Int).Value = demand.DemandPatternID;
                                    cmd.Parameters.Add("@DemandBase", SqlDbType.Float).Value = demand.BaseDemandValue;
                                    cmd.Parameters.Add("@DemandFactor", SqlDbType.Float).Value = demand.DemandFactorValue;
                                    cmd.Parameters.Add("@DemandActual", SqlDbType.Float).Value = demand.ActualDemandValue;
                                    cmd.Parameters.Add("@IsExcluded", SqlDbType.Bit).Value = demand.DemandIsExcluded;
                                    cmd.Parameters.Add("@DemandCalculated", SqlDbType.Float).Value = demand.DemandCalculatedValue;

                                    cmd.Parameters.Add("@LogId", SqlDbType.Int).Direction = ParameterDirection.Output;
                                    cmd.Parameters["@LogId"].Value = 0;

                                    sqlConn.Open();
                                    cmd.ExecuteNonQuery();
                                    sqlConn.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger?.WriteMessage(OutputLevel.Errors, $"Saving data to database.\n{e.Message}");
                throw;
            }
        }

        public void UpdateAndLoadFromDatabase(List<ZoneDemandData> zoneDemandDataList, string conStr)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(conStr))
                {
                    DataSet dataSet = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter
                    {
                        SelectCommand = new SqlCommand("CalcLastDemand", sqlConn)
                        {
                            CommandType = CommandType.StoredProcedure
                        }
                    };
                    adapter.Fill(dataSet);

                    var dtZoneList = dataSet.Tables[0].AsEnumerable().Select(x => new
                    {
                        AutoZoneId = x.Field<int>("AutoZoneId"),
                        DemandRatioDb = x.Field<double>("DemandRatioDb"),
                    }).ToList();
                    var dtDemandList = dataSet.Tables[1].AsEnumerable().Select(x => new
                    {
                        AutoZoneId = x.Field<int>("AutoZoneId"),
                        AutoObjectId = x.Field<int>("AutoObjectId"),
                        AutoDemandId = x.Field<int>("AutoDemandId"),
                        DemandCalculatedDb = x.Field<double>("DemandCalculatedDb"),
                    }).ToList();
                    foreach (var zone in zoneDemandDataList)
                    {
                        var dtZone = dtZoneList.FirstOrDefault(x => x.AutoZoneId == zone.ZoneId);
                        if (dtZone != null)
                        {
                            zone.DemandAdjustmentRatioDb = dtZone.DemandRatioDb;
                            foreach (var demand in zone.Demands)
                            {
                                var dtDemand = dtDemandList.FirstOrDefault(x =>
                                    x.AutoZoneId == zone.ZoneId && x.AutoObjectId == demand.ObjectID &&
                                    x.AutoDemandId == demand.DemandPatternID);
                                if (dtDemand != null)
                                {
                                    demand.DemandCalculatedValueDb = dtDemand.DemandCalculatedDb;
                                }
                                else
                                {
                                    _logger?.WriteMessage(OutputLevel.Warnings, $"Missing demand pattern: {demand.DemandPatternName}");
                                }
                            }
                        }
                        else
                        {
                            _logger?.WriteMessage(OutputLevel.Warnings, $"Missing zone: {zone.ZoneName}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger?.WriteMessage(OutputLevel.Errors, $"Calculating and loading data from database.\n{e.Message}");
                throw;
            }
        }


        private class PipeMeterOpcValue
        {
            public int QpId { get; set; }
            public int ModelVarId { get; set; }
            public string ModelVarName { get; set; }
            public double Value { get; set; }
        }

        public void SavePipeMeterListToDatabase(string conStr)
        {
            try
            {
                _logger?.WriteMessage(OutputLevel.Info, $"SavePipeMeterListToDatabase ############################");
                
                                using (SqlConnection sqlConn = new SqlConnection(conStr))
                {
                    DataSet dataSet = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter
                    {
                        SelectCommand = new SqlCommand("spLoadPipeMeterList", sqlConn)
                        {
                            CommandType = CommandType.StoredProcedure
                        }
                    };
                    adapter.Fill(dataSet);

                    var pipeMeterList = dataSet.Tables[0].AsEnumerable().Select(x => new PipeMeterOpcValue
                    {
                        QpId = x.Field<int>("QpId"),
                        ModelVarId = x.Field<int>("ModelVarId"),
                        ModelVarName = x.Field<string>("ModelVarName"),
                    }).ToList();

                    FillPipeMeterValueList(pipeMeterList);

                    int logHeaderId;

                    using (SqlCommand cmd = new SqlCommand("spSaveLogHeader", sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@DateTimeIn", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@DateTimeUtcIn", SqlDbType.DateTime).Value = DateTime.UtcNow;
                        cmd.Parameters.Add("@LogTypeId", SqlDbType.Int).Value = 2;  // Save PipeMeter list
                        cmd.Parameters.Add("@RatioFormula", SqlDbType.NVarChar, 4000).Value = string.Empty;
                        cmd.Parameters.Add("@MinutesFromMonday", SqlDbType.Float).Value = 0;  // Calculated by last Create method.

                        cmd.Parameters.Add("@LogId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters["@LogId"].Value = 0;

                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        logHeaderId = Convert.ToInt32(cmd.Parameters["@LogId"].Value);
                        sqlConn.Close();
                    }

                    foreach (var zone in pipeMeterList)
                    {
                        using (SqlCommand cmd = new SqlCommand("spSaveLogPipeMeter", sqlConn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@LogHeaderId", SqlDbType.Int).Value = logHeaderId;
                            cmd.Parameters.Add("@PipeMeterId", SqlDbType.Int).Value = zone.QpId;
                            cmd.Parameters.Add("@Value", SqlDbType.Float).Value = zone.Value;

                            sqlConn.Open();
                            cmd.ExecuteNonQuery();
                            sqlConn.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger?.WriteMessage(OutputLevel.Errors, $"Saving PipeMeterList to database.\n{e.Message}");
                throw;
            }
        }

        private void FillPipeMeterValueList(List<PipeMeterOpcValue> opcTagValueDict)
        {
            using (var opc = new OpcReader(_dataContext.OpcServerAddress))
            {
                foreach (var opcTagValue in opcTagValueDict)
                {
                    opcTagValue.Value = opc.GetDouble($"PipeFlow.DEV.{opcTagValue.ModelVarName}_{opcTagValue.ModelVarId}");
                }
            }
        }
    }
}
