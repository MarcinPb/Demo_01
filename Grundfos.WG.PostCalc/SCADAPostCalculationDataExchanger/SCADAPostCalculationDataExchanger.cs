using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using AutoMapper;
using Grundfos.OPC;
using Grundfos.WaterDemandCalculation;
using Grundfos.WG.Model;
using Grundfos.WG.ObjectReaders;
using Grundfos.WG.OPC.Publisher;
using Grundfos.WG.OPC.Publisher.Configuration;
using Grundfos.WG.PostCalc;
using Grundfos.WG.PostCalc.DataExchangers;
using Grundfos.WG.PostCalc.DemandCalculation;
using Grundfos.WG.PostCalc.Exceptions;
using Grundfos.WG.PostCalc.Persistence.MapperProfiles;
using Grundfos.WG.PostCalc.Persistence.Repositories;
using Grundfos.WG.PostCalc.PressureCalculation;
using Grundfos.WG.PostCalc.SQLiteEf;
using Grundfos.Workbooks;
using Haestad.DataIntegration;
using Haestad.Domain;
using Haestad.Support.OOP.CommandLine;
using Haestad.Support.OOP.Configuration;
using Haestad.Support.OOP.FileSystem;
using Haestad.Support.OOP.Logging;

namespace SCADAPostCalculationDataExchanger
{
    public class SCADAPostCalculationDataExchanger : ToFileDataExchangerBase, IInProcessPluginDataExchanger
    {
        private IDomainDataSet DomainDataSet { get; set; }
        private IScenario Scenario { get; set; }
        private int ScenarioID { get; set; }

        public SCADAPostCalculationDataExchanger() {}
        public void SetDomainDataSet(IDomainDataSet domainDataSet)
        {
            DomainDataSet = domainDataSet;
            ScenarioID = DomainDataSet.ScenarioManager.ActiveScenarioID;
            Scenario = DomainDataSet.ScenarioManager.Element(ScenarioID) as IScenario;
        }

        public override string DataExchangerTitle => "SCADAPostCalculationDataExchanger";
        public override Version DataExchangerVersion => new Version(1, 0, 0, 0);
        public override string DataExchangerCopyright => "Copyright (c) Grundfos";

        public string RepositoryPath { get; set; }
        public string DemandConfigurationWorkbook { get; private set; }
        public string RatioFormula { get; set; }

        // 
        public string DumpOption { get; private set; }
        public string DumpFolder { get; private set; }
        public bool IsLogToDb { get; set; }
        public string LogDbConnString { get; set; }
        public bool IsCalculationOnDb { get; set; }
        public string OpcServerAddress { get; set; }
        public string WaterInfraConnString { get; set; }

        public override object NewDataExchangeContext(string[] arguments)
        {
            const string SETTINGS_FILE_KEY = "INI";
            CommandLineArgumentsHelper commandLineArgumentsHelper = new CommandLineArgumentsHelper(arguments, new string[] { SETTINGS_FILE_KEY });
            string settingsFileName = commandLineArgumentsHelper.GetCommandLineValue(SETTINGS_FILE_KEY);
            IConfigurationReader configurationReader = new SettingsFileReader(Logger, new FilePath(settingsFileName));

            return new DataExchangerContext(Logger, configurationReader);
        }

        public override bool BeforeDoDataExchange(object dataExchangeContext)
        {
            this.Logger.WriteMessage(OutputLevel.Info, "-- BeforeDoDataExchange started --------------------------------------------------------------");

            // exchangeContext already contains ResultCacheDb and DemandConfigurationWorkbook keyValues taken from *.ini file.
            var exchangeContext = (DataExchangerContext)dataExchangeContext;
            this.RepositoryPath = exchangeContext.GetString("ResultCacheDb", @"C:\WG2TW\Grundfos.WG.PostCalc\ResultCache.sqlite");
            this.DemandConfigurationWorkbook = exchangeContext.GetString("DemandConfigurationWorkbook", @"C:\WG2TW\Grundfos.WG.PostCalc\WaterDemandSettings.xlsx");

            // Rest of *.ini file parameters.
            this.DumpOption = exchangeContext.GetString("DumpOption", @"1");
            this.DumpFolder = exchangeContext.GetString("DumpFolder", @"C:\Users\Administrator\AppData\Local\Bentley\SCADAConnect\10");
            this.IsLogToDb = bool.Parse(exchangeContext.GetString("IsLogToDb", "false"));
            this.LogDbConnString = exchangeContext.GetString("LogDbConnString", @"Data Source=.\SQLEXPRESS;Initial Catalog=WG;Integrated Security=True").Replace(":",";");
            this.IsCalculationOnDb = bool.Parse(exchangeContext.GetString("IsCalculationOnDb", "false"));
            this.OpcServerAddress = "Kepware.KEPServerEX.V6";
            //this.WaterInfraConnString = exchangeContext.GetString("WaterInfraConnString", @"Server=.;Database=WaterInfra;User Id=sa;Password=Gfosln123.;").Replace(":",";");
            this.WaterInfraConnString = exchangeContext.GetString("WaterInfraConnString", @"Server=192.168.0.62\MSSQL2017;Database=WaterInfra;User Id=sa;Password=Gfosln123.;").Replace(":",";");

            //this.RatioFormula = exchangeContext.GetString("RatioFormula", string.Empty);

            //this.Logger.WriteMessage(OutputLevel.Info, $"# IsLogToDb = {IsLogToDb}.");
            //this.Logger.WriteMessage(OutputLevel.Info, $"# LogDbConnString = \"{LogDbConnString}\".");
            //this.Logger.WriteMessage(OutputLevel.Info, $"# IsCalculationOnDb = {IsCalculationOnDb}.");
            //this.Logger.WriteMessage(OutputLevel.Info, $"# RatioFormula = \"{RatioFormula}\".");

            return true;
        }

        public override bool DoDataExchange(object dataExchangeContext)
        {
            this.Logger.WriteMessage(OutputLevel.Info, "-- DoDataExchange started --------------------------------------------------------------------");

            // Dictionary<int, string> <- WaterGEMS {{n, "1 - Przybków"},... {n, "16 - Pompownia"}}
            var zoneReader = new ZoneReader(this.DomainDataSet);
            var wgZones = zoneReader.GetZones();

            // XSSFWorkbook excelReader.Workbook <- Waterdemandsettings.xlsx
            var excelReader = new ExcelReader(this.DemandConfigurationWorkbook);

            // dataExchangeContext <- ResultCache.sqlite
            this.PassQualityResults(dataExchangeContext);

            // publish results to OPC server
            this.PublishOpcResults(dataExchangeContext, excelReader, wgZones);

            /*
            // Take PipeMeterFlowList from OPC and save to database. 
            if (IsLogToDb)
            {
                DbConnector.SavePipeMeterListToDatabase(LogDbConnString, OpcServerAddress);
            }
            */

            // Wait for time in seconds taken from SQL.
            if (IsLogToDb)
            {
                int seconds = GetDelayTimeFromSql();
                this.Logger.WriteMessage(OutputLevel.Info, $"-- SCADAPostCalculationDataExchanger started waiting for {seconds} seconds.");
                Thread.Sleep(seconds*1000);
                this.Logger.WriteMessage(OutputLevel.Info, $"-- SCADAPostCalculationDataExchanger finished waiting.");
            }

            // Calculate and set up BaseDemands for Junctions, Hydrants and customerMeters in WaterGEMS.
            this.ExchangeWaterDemands(dataExchangeContext, excelReader, wgZones);

            return true;
        }

        #region PassQualityResults

        private void PassQualityResults(object dataExchangeContext)
        {
            try
            {
                this.Logger.WriteMessage(OutputLevel.Info, "-- PassQualityResults started -------------------------------------------");

                var db = new DatabaseContext(this.RepositoryPath);                      // ResultCache.sqlite
                var mapper = BuildMapper();
                var repo = new PostCalcRepository(db, mapper);

                // List<Grundfos.WG.PostCalc.DataExchangers.DataExchangerBase>
                var dataExchangers = this.BuildDataExchangers(repo);
                dataExchangers.ForEach(x => x.DoDataExchange(dataExchangeContext));
            }
            catch (Exception ex)
            {
                this.Logger.WriteMessage(OutputLevel.Errors, "Errors occured when passing quality results.");
                this.Logger.WriteException(ex, true);
            }
        }

        private static IMapper BuildMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(ResultProfile).Assembly);
            });
            var mapper = new Mapper(mapperConfig);
            return mapper;
        }

        // WG: .ResultField(string name, string numericalEngineType, string resultRecordTypeName)
        //  ResultRecordName = IdahoWaterQualityResults                 resultRecordTypeName    
        //      IdahoWaterQualityResults_CalculatedAge - 3600               name
        //      IdahoWaterQualityResults_CalculatedTrace - 0.01             name
        //      IdahoWaterQualityResults_CalculatedConcentration - 1        name
        //  DomainElementType:
        //      IdahoJunctionElementManager
        //      IdahoTankElementManager
        private List<GenericDataExchanger> BuildDataExchangers(IPostCalcRepository repository)
        {
            var ageConfig = new DataExchangerConfiguration
            {
                ResultRecordName = StandardResultRecordName.IdahoWaterQualityResults,
                ResultAttributeRecordName = StandardResultRecordName.IdahoWaterQualityResults_CalculatedAge,
                Alternative = AlternativeType.AgeAlternative,
                FieldName = StandardFieldName.Age_InitialAge,
                ConversionFactor = 3600,
            };

            var traceConfig = new DataExchangerConfiguration
            {
                ResultRecordName = StandardResultRecordName.IdahoWaterQualityResults,
                ResultAttributeRecordName = StandardResultRecordName.IdahoWaterQualityResults_CalculatedTrace,
                Alternative = AlternativeType.TraceAlternative,
                FieldName = StandardFieldName.Trace_InitialTrace,
                ConversionFactor = 0.01,
            };

            var concentrationConfig = new DataExchangerConfiguration
            {
                ResultRecordName = StandardResultRecordName.IdahoWaterQualityResults,
                ResultAttributeRecordName = StandardResultRecordName.IdahoWaterQualityResults_CalculatedConcentration,
                Alternative = AlternativeType.ConstituentAlternative,
                FieldName = StandardFieldName.Constituent_InitialConcentration,
                ConversionFactor = 1,
            };

            var dataExchangers = new List<Grundfos.WG.PostCalc.DataExchangers.GenericDataExchanger>
            {
                new GenericDataExchanger(this.Logger, this.Scenario, this.DomainDataSet, repository, ageConfig),
                new GenericDataExchanger(this.Logger, this.Scenario, this.DomainDataSet, repository, traceConfig),
                new GenericDataExchanger(this.Logger, this.Scenario, this.DomainDataSet, repository, concentrationConfig),
            };

            return dataExchangers;
        }

        #endregion

        #region PublishOpcResults

        private void PublishOpcResults(object dataExchangeContext, ExcelReader excelReader, Dictionary<int, string> wgZones)
        {
            try
            {
                this.Logger.WriteMessage(OutputLevel.Info, "-- PublishOpcResults started --------------------------------------------");

                // ICollection<OpcMapping> mappings <- excel.OpcMapping group by FieldName without "Result Attribute Label" column.
                var mappingReader = new OpcMappingReader(this.Logger, excelReader);
                ICollection<OpcMapping> mappings = mappingReader.ReadMappings();

                // List<OpcPublisher> publishers <- ICollection<OpcMapping> * Dictionary<int, string>
                List<OpcPublisher> publishers = this.BuildPublishers(mappings, wgZones);

                this.Logger.WriteMessage(OutputLevel.Info, "Start writing results to OPC.");
                foreach (var publisher in publishers)
                {
                    publisher.PublishResults(this.DomainDataSet, this.Scenario);
                }
                this.Logger.WriteMessage(OutputLevel.Info, "Finished writing results to OPC.");
            }
            catch (Exception ex)
            {
                this.Logger.WriteMessage(OutputLevel.Errors, "Errors occured when publishing the results to OPC.");
                this.Logger.WriteException(ex, true);
            }
        }

        private List<OpcPublisher> BuildPublishers(ICollection<OpcMapping> mappings, Dictionary<int, string> wgZones)
        {
            var publishers = new List<OpcPublisher>();

            var nodeMapping = mappings.FirstOrDefault(x => x.FieldName.Equals("ZoneAveragePressure", StringComparison.OrdinalIgnoreCase));
            if (nodeMapping != null)
            {
                var nodePressureConfig = new ZonePressurePublisherConfiguration
                {
                    ResultRecordName = StandardResultRecordName.IdahoPressureNodeResults,
                    ResultAttributeRecordName = StandardResultRecordName.IdahoPressureNodeResults_NodePressure,
                    FieldName = StandardFieldName.PipeStatus,
                    ConversionFactor = 1,
                    ElementTypes = new DomainElementType[]
                    {
                        DomainElementType.BaseIdahoNodeElementManager,
                    },
                    Mappings = nodeMapping.Mappings.ToDictionary(x => x.ElementID, x => x),
                    Zones = wgZones,
                };
                var nodePublisher = new ZonePressurePublisher(nodePressureConfig, this.Logger);
                publishers.Add(nodePublisher);
            }

            var tankPercentMapping = mappings.FirstOrDefault(x => x.FieldName.Equals("TankPercentFull", StringComparison.OrdinalIgnoreCase));
            if (tankPercentMapping != null)
            {
                var tankPercentFillConfig = new PublisherConfiguration
                {
                    ResultRecordName = StandardResultRecordName.IdahoConventionalTankResults,
                    ResultAttributeRecordName = StandardResultRecordName.IdahoConventionalTankResults_CalculatedPercentFull,
                    FieldName = StandardFieldName.ElementType_TankPercentFull,
                    ConversionFactor = 1,
                    ElementTypes = new DomainElementType[]
                    {
                        DomainElementType.IdahoTankElementManager,
                    },
                    Mappings = tankPercentMapping.Mappings.ToDictionary(x => x.ElementID, x => x),
                };
                var publisher = new OpcPublisher(tankPercentFillConfig, this.Logger);
                publishers.Add(publisher);
            }

            var pipeMapping = mappings.FirstOrDefault(x => x.FieldName.Equals("PipeStatus", StringComparison.OrdinalIgnoreCase));
            if (pipeMapping != null)
            {
                var pipeOpenStateConfig = new PublisherConfiguration
                {
                    ResultRecordName = StandardResultRecordName.IdahoPipeResults,
                    ResultAttributeRecordName = StandardResultRecordName.PipeResultControlStatus,
                    FieldName = StandardFieldName.PipeStatus,
                    ConversionFactor = 1,
                    ElementTypes = new DomainElementType[]
                    {
                        DomainElementType.IdahoPipeElementManager,
                    },
                    Mappings = pipeMapping.Mappings.ToDictionary(x => x.ElementID, x => x),
                };
                var pipeOpenPublisher = new OpcPublisher(pipeOpenStateConfig, this.Logger);
                publishers.Add(pipeOpenPublisher);
            }

            return publishers;
        }

        #endregion

        #region ExchangeWaterDemands

        private const string TestedZoneName = "7 - Tranzyt";
        private const string DateFormat = "yyyy-MM-dd_HH-mm-ss_fffffff";
        private void ExchangeWaterDemands(object dataExchangeContext, ExcelReader excelReader, Dictionary<int, string> wgZones)
        {
            this.Logger.WriteMessage(OutputLevel.Info, "-- ExchangeWaterDemands started -----------------------------------------");

            if (excelReader == null)
            {
                throw new ArgumentNullException(nameof(excelReader));
            }

            try
            {
                /*
                // Step 1.
                // Fill List<WaterDemandData> based on excel.ObjectData (11004), WG.Zones (16) and WG.DemandPatterns (51 with "Fixed").
                // Fields AssociatedElementID and ActualDemandValue are not filled.

                // Get zones (16 rec.) from WaterGEMS model.
                this.Logger.WriteMessage(OutputLevel.Info, $"{wgZones.Count} zones have been read from WaterGEMS model.");
                foreach (var zone in wgZones)
                {
                    this.Logger.WriteMessage(OutputLevel.Info, $"\t{zone.Key}, \"{zone.Value}\"");
                }
                */
                // Dictionary<string, int> patterns (51 rec.) = WaterGEMS->DemandPattern {{"Urz", 1}, {"Mw", 2},... {"Fixed", -1}}
                var patternReader = new WaterDemandPatternCurveReader(this.DomainDataSet);
                var patterns = patternReader.GetPatterns();
                this.Logger.WriteMessage(OutputLevel.Info, $"{patterns.Count} demand patterns have been read from WaterGEMS model.");
                foreach (var pattern in patterns)
                {
                    this.Logger.WriteMessage(OutputLevel.Info, $"\t\"{pattern.Key}\", {pattern.Value}");
                }
                /*
                // List<WaterDemandData> objectDemandData (11004 rec.) = Excel.ObjectData  (2986 rec. ???)
                //  ObjectID=6871,
                //  ObjectTypeID=73,
                //  DemandPatternName="Mw",
                //  DemandPatternID=,
                //  ZoneName="1 - Przybków",
                //  ZoneID=,
                //  BaseDemandValue=0.0470416666666677,
                //  AssociatedElementID=,
                //  ActualDemandValue=,
                var objectDataReader = new ObjectDataExcelReader(excelReader);
                var objectDemandData = objectDataReader.ReadObjects();
                // log: 11004 objects with demand information have been read from Excel file.
                this.Logger.WriteMessage(OutputLevel.Info, $"{objectDemandData.Count} objects with demand information have been read from Excel file.");

                // List<WaterDemandData> = Excel.ObjectData
                //  ObjectID=6871,
                //  ObjectTypeID=73,
                //  DemandPatternName="Mw",
                //  DemandPatternID=6842,               <--
                //  ZoneName="1 - Przybków",
                //  ZoneID=6773,                        <--
                //  BaseDemandValue=0.0470416666666677,
                //  AssociatedElementID=,
                //  ActualDemandValue=,
                // Fills objectDemandData DemandPatternID: objectDemandData[n].DemandPatternID <- patterns[where Name==Name].ID
                this.FillPatternIds(objectDemandData, patterns);
                // Fills objectDemandData ZoneID 
                this.FillZoneIdsInWaterDemands(objectDemandData, wgZones.ToDictionary(x => x.Value, x => x.Key, StringComparer.OrdinalIgnoreCase));

                // List<WaterDemandData> objectDemandData (2986 rec.) = Excel.ObjectData {{6871, 73, "Mw", 3333, 0.0470416666666677, "1 - Przybków", 2222, },... }
                //DumpWaterDemandData(objectDemandData.ToList());

                // Step 2.
                // Get List<ZoneDemandData>. Prepare TotalDemandCalculation demandTotalizer 
                */
                var waterDemandExcelReader = new DemandPatternExcelReader(excelReader);
                /*
                var demandTotalizer = BuildDemandTotalizer(waterDemandExcelReader, excelReader);

                var now = DateTime.Now;
                // List<ZoneDemandData>
                var zoneDemands = this.GetZoneDemands(wgZones, objectDemandData, waterDemandExcelReader, demandTotalizer, now);


                // Log only
                foreach (var item in zoneDemands)
                {
                    string message = $"Total demand for zone {item.ZoneName}: WaterGEMS = {item.WgDemand}, SCADA: {item.ScadaDemand}, ratio: {item.DemandAdjustmentRatio}.";
                    this.Logger.WriteMessage(OutputLevel.Info, message);
                }
                Helper.DumpToFile(zoneDemands.FirstOrDefault(x => x.ZoneName == TestedZoneName), Path.Combine(DumpFolder, $"Dump_{DateTime.Now.ToString(DateFormat)}_ZoneDemandData_1.xml"));
                */

                #region ZoneDemandDataListCreator.Create

                List<ZoneDemandData> zoneDemandDataList;

                /*
                ZoneDemandDataListCreator.DataContext dataContext = new ZoneDemandDataListCreator.DataContext()
                {
                    WgZoneDict = wgZones,
                    WgDemandPatternDict = patterns.ToDictionary(x => x.Value, x => x.Key),
                    ExcelFileName = this.DemandConfigurationWorkbook,
                    OpcServerAddress = this.OpcServerAddress,
                    StartComputeTime = DateTime.Now,    //new DateTime(2020, 05, 04, 0, 46, 32),
                };
                ZoneDemandDataListCreator zoneDemandDataListCreator = new ZoneDemandDataListCreator(dataContext, this.Logger);
                zoneDemandDataList = zoneDemandDataListCreator.Create();
                if (this.IsLogToDb)
                {
                    //string conStr = @"Data Source=WIN-6SC244KSC3K\SQL2017;Initial Catalog=WG;Integrated Security=True";
                    string conStr = this.LogDbConnString;
                    //zoneDemandDataListCreator.SavePipeMeterListToDatabase(conStr);

                    zoneDemandDataListCreator.SaveToDatabase(zoneDemandDataList, conStr, RatioFormula);
                    if (this.IsCalculationOnDb)
                    {
                        zoneDemandDataListCreator.UpdateAndLoadFromDatabase(zoneDemandDataList, conStr);
                    }
                }
                // Log only
                zoneDemandDataList.ForEach(x => this.Logger.WriteMessage(
                    OutputLevel.Info,
                    $"# tal demand for zone {x.ZoneName}: WaterGEMS = {x.WgDemand}, SCADA: {x.ScadaDemand}, ratio: {x.DemandAdjustmentRatio}."
                    ));
                Helper.DumpToFile(zoneDemandDataList.FirstOrDefault(x => x.ZoneName == TestedZoneName), Path.Combine(DumpFolder, $"Dump_{DateTime.Now.ToString(DateFormat)}_ZoneDemandData_2.xml"));
                */

                ZoneDemandDataListCreatorNew.DataContext dataContextNew = new ZoneDemandDataListCreatorNew.DataContext()
                {
                    WaterInfraConnString = WaterInfraConnString,
                };
                ZoneDemandDataListCreatorNew zoneDemandDataListCreatorNew = new ZoneDemandDataListCreatorNew(dataContextNew, this.Logger);
                zoneDemandDataList = zoneDemandDataListCreatorNew.Create(DateTime.Now);
                Helper.DumpToFile(zoneDemandDataList.FirstOrDefault(x => x.ZoneName == TestedZoneName), Path.Combine(DumpFolder, $"Dump_{DateTime.Now.ToString(DateFormat)}_ZoneDemandData_2_New.xml"));

                #endregion


                // Step 3.
                // Write data to WG.

                // Two arrays of int: "Excluded Object IDs" and "Excluded Demand Patterns"
                var demandConfig = this.GetDemandWriterConfig(patterns, waterDemandExcelReader);
                demandConfig.IsCalculationOnDb = IsCalculationOnDb;

                var demandWriter = new WaterDemandDataWriter(this.Logger, this.DomainDataSet, demandConfig, (DataExchangerContext)dataExchangeContext);

                //foreach (var zoneDemand in zoneDemands.Where(x => x.ScadaDemand > 0.001))
                foreach (var zoneDemand in zoneDemandDataList.Where(x => Math.Abs(x.ScadaDemand) > 0.001))
                {
                    demandWriter.WriteDemands(zoneDemand);
                }
            }
            catch (Exception ex)
            {
                this.Logger.WriteMessage(OutputLevel.Errors, "Errors occured when exchanging water demand.");
                this.Logger.WriteException(ex, true);
            }
        }

        /*
        private void DumpWaterDemandData(IList<WaterDemandData> demandData)
        {
            using (var file = new StreamWriter(@"C:\Temp\demand-data.txt"))
            {
                file.WriteLine("ObjectID\tObjectTypeID\tDemandPatternID\tDemandPatternName\tBaseDemandValue\tZoneID\tZoneName");
                foreach (var item in demandData)
                {
                    file.WriteLine($"{item.ObjectID}\t{item.ObjectTypeID}\t{item.DemandPatternID}\t{item.DemandPatternName}\t{item.BaseDemandValue}\t{item.ZoneID}\t{item.ZoneName}");
                }
            }
        }
        */

        private WaterDemandDataWriterConfiguration GetDemandWriterConfig(Dictionary<string, int> patterns, DemandPatternExcelReader excelReader)
        {
            // List<string> <- Excel.ExcludedItems["Excluded Object IDs"].
            // {257=PC, 2719=S5, 518=CP1, 701=CP2, 1323=CP3, 1336=CP4, 2255=S6, 2780=S7, 1247=W1, 1239=W2, 1548=CP6}    
            var excludedObjects = excelReader.ReadExcludedObjects();

            // List<string> <- Excel.ExcludedItems["Excluded Demand Patterns"]. {"nieaktywni", "Straty"}.    
            var excludedPatterns = excelReader.ReadExcludedPatterns();

            // Only that patterns which exist in WG
            var patternsToExclude = patterns
                .Where(x => excludedPatterns.Contains(x.Key))
                .Select(x => x.Value)
                .ToArray();

            var demandConfig = new WaterDemandDataWriterConfiguration
            {
                ExcludedObjectIDs = excludedObjects.ToArray(),
                ExcludedDemandPatterns = patternsToExclude,
            };
            return demandConfig;
        }
        private WaterDemandDataWriterConfiguration GetDemandWriterConfigNew(List<ZoneDemandData> zoneDemandDataList)
        {
            ZoneDemandDataListCreatorNew.DataContext dataContextNew = new ZoneDemandDataListCreatorNew.DataContext()
            {
                WaterInfraConnString = WaterInfraConnString,
            };
            ZoneDemandDataListCreatorNew zoneDemandDataListCreatorNew = new ZoneDemandDataListCreatorNew(dataContextNew, this.Logger);

            // List<string> <- Excel.ExcludedItems["Excluded Object IDs"].
            // {257=PC, 2719=S5, 518=CP1, 701=CP2, 1323=CP3, 1336=CP4, 2255=S6, 2780=S7, 1240=W1, 1239=W2, 1548=CP6}    
            var excludedObjects = zoneDemandDataListCreatorNew.GetExcludedObjectId(zoneDemandDataList);

            // List<string> <- Excel.ExcludedItems["Excluded Demand Patterns"]. {"nieaktywni1", "Straty1"}.    
            var excludedPatterns = zoneDemandDataListCreatorNew.GetExcludedDemandPatternId(zoneDemandDataList);

            var demandConfig = new WaterDemandDataWriterConfiguration
            {
                ExcludedObjectIDs = excludedObjects.ToArray(),
                ExcludedDemandPatterns = excludedPatterns.ToArray(),
            };
            return demandConfig;
        }

        /*
        private List<ZoneDemandData> GetZoneDemands(
            Dictionary<int, string> wgZones, 
            ICollection<WaterDemandData> demandData, 
            DemandPatternExcelReader excelReader, 
            TotalDemandCalculation demandTotalizer, 
            DateTime now)
        {
            var zoneDemands = demandData
                .GroupBy(x => x.ZoneName)
                .Select(x => new ZoneDemandData { ZoneName = x.Key, Demands = x.ToList() })
                .ToList();
            var noZoneDemands = zoneDemands.Where(x => string.IsNullOrEmpty(x.ZoneName)).ToList();
            if (noZoneDemands.Count > 0)
            {
                var numberOfItems = noZoneDemands.Sum(x => x.Demands.Count);
                this.Logger.WriteMessage(OutputLevel.Warnings, $"Found {numberOfItems} demands with no zone specified.");
                noZoneDemands.ForEach(x => zoneDemands.Remove(x));
            }

            foreach (var item in zoneDemands)
            {
                item.WgDemand = demandTotalizer.GetTotalDemand(item.Demands, now);
            }

            List<string> missingZoneOpcTags = GetMissingZoneOpcTags(excelReader, zoneDemands);
            if (missingZoneOpcTags.Count > 0)
            {
                string message = string.Format("Could not find OPC tag for zones {0}", string.Join(", ", missingZoneOpcTags));
                this.Logger.WriteMessage(OutputLevel.Warnings, message);
            }

            using (var opc = new OpcReader(this.OpcServerAddress))
            {
                foreach (var item in zoneDemands)
                {
                    item.ScadaDemand = opc.GetDouble(item.OpcTag);
                    item.DemandAdjustmentRatio = item.WgDemand == 0 ? 0 : item.ScadaDemand / item.WgDemand;
                }
            }

            return zoneDemands;
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

        private static TotalDemandCalculation BuildDemandTotalizer(DemandPatternExcelReader waterDemandExcelReader, ExcelReader excelReader)
        {
            var adjuster = new TimeshiftAdjuster(7 * 24 * 60);
            var interpolator = new Interpolator(adjuster);
            var patternService = new DemandPatternService(waterDemandExcelReader);
            var demandService = new DemandService(interpolator, patternService);
            var settings = new SimulationTimeResolverConfiguration
            {
                // 30.09.2019  12:15:00
                SimulationStartTime = excelReader.ReadSetting<DateTime>(Grundfos.WG.PostCalc.Constants.ApplicationSetting_SimulationStartDate),
                // 10
                SimulationIntervalMinutes = excelReader.ReadSetting<int>(Grundfos.WG.PostCalc.Constants.ApplicationSetting_SimulationIntervalMinutes)
            };
            var demandTotalizer = new TotalDemandCalculation(demandService, new SimulationTimeResolver(settings));
            return demandTotalizer;
        }

        private void FillPatternIds(ICollection<WaterDemandData> demands, Dictionary<string, int> patterns)
        {
            foreach (var item in demands)
            {
                if (!patterns.TryGetValue(item.DemandPatternName, out int patternId))
                {
                    throw new DataExchangerException(string.Format("Could not find pattern definition for pattern ID: {0}.", item.DemandPatternName));
                }

                item.DemandPatternID = patternId;
            }
        }

        private void FillZoneIdsInWaterDemands(ICollection<WaterDemandData> demands, Dictionary<string, int> zones)
        {
            foreach (var item in demands.Where(x => !string.IsNullOrWhiteSpace(x.ZoneName)))
            {
                if (!zones.TryGetValue(item.ZoneName, out int zoneId))
                {
                    throw new DataExchangerException(string.Format("Could not find zone definition for zone name: {0}.", item.ZoneName));
                }

                item.ZoneID = zoneId;
            }
        }
        */
        #endregion

        private int GetDelayTimeFromSql()
        {
            try
            {
                int delayTime;
                using (SqlConnection sqlConn = new SqlConnection(LogDbConnString))
                {
                    using (SqlCommand cmd = new SqlCommand("spGetDelayTime", sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Seconds", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters["@Seconds"].Value = 0;

                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                        delayTime = Convert.ToInt32(cmd.Parameters["@Seconds"].Value);
                        sqlConn.Close();
                    }
                }

                return delayTime;
            }
            catch (Exception e)
            {
                this.Logger.WriteMessage(OutputLevel.Errors, $"Saving data to database.\n{e.Message}");
                throw;
            }
        }
    }
}
