using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using AutoMapper;
using Grundfos.WaterDemandCalculation;
using Grundfos.WG.Model;
using Grundfos.WG.ObjectReaders;
using Grundfos.WG.OPC.Publisher;
using Grundfos.WG.OPC.Publisher.Configuration;
using Grundfos.WG.PostCalc;
using Grundfos.WG.PostCalc.DataExchangers;
using Grundfos.WG.PostCalc.DemandCalculation;
using Grundfos.WG.PostCalc.Persistence.MapperProfiles;
using Grundfos.WG.PostCalc.Persistence.Repositories;
using Grundfos.WG.PostCalc.PressureCalculation;
using Grundfos.WG.PostCalc.SQLiteEf;
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
        private IDomainDataSet _domainDataSet;
        private IScenario _scenario;
        private int _scenarioID;

        private string _sqliteFile;
        private string _dumpFolder;
        private ZoneDemandDataListCreatorNew _zoneDemandDataListCreatorNew;


        public override string DataExchangerTitle => "SCADAPostCalculationDataExchanger";
        public override Version DataExchangerVersion => new Version(1, 0, 0, 0);
        public override string DataExchangerCopyright => "Copyright (c) Grundfos";

        //public string DumpOption { get; private set; }
        //public string OpcServerAddress { get; set; }
        /*
        public bool IsLogToDb { get; set; }
        public string LogDbConnString { get; set; }
        public bool IsCalculationOnDb { get; set; }
        public string WaterInfraConnString { get; set; }
        */



        public SCADAPostCalculationDataExchanger() {}

        public void SetDomainDataSet(IDomainDataSet domainDataSet)
        {
            _domainDataSet = domainDataSet;
            _scenarioID = _domainDataSet.ScenarioManager.ActiveScenarioID;
            _scenario = _domainDataSet.ScenarioManager.Element(_scenarioID) as IScenario;
        }

        public override object NewDataExchangeContext(string[] arguments)
        {
            const string SETTINGS_FILE_KEY = "INI";
            CommandLineArgumentsHelper commandLineArgumentsHelper = new CommandLineArgumentsHelper(arguments, new string[] { SETTINGS_FILE_KEY });
            var settingsFileName = commandLineArgumentsHelper.GetCommandLineValue(SETTINGS_FILE_KEY);
            IConfigurationReader configurationReader = new SettingsFileReader(Logger, new FilePath(settingsFileName));

            return new DataExchangerContext(Logger, configurationReader);
        }

        public override bool BeforeDoDataExchange(object dataExchangeContext)
        {
            this.Logger.WriteMessage(OutputLevel.Info, "-- BeforeDoDataExchange started --------------------------------------------------------------");

            // exchangeContext already contains ResultCacheDb and DemandConfigurationWorkbook keyValues taken from *.ini file.
            var exchangeContext = (DataExchangerContext)dataExchangeContext;
            
            _sqliteFile = exchangeContext.GetString("ResultCacheDb", @"C:\WG2TW\Grundfos.WG.PostCalc\ResultCache.sqlite");
            _dumpFolder = exchangeContext.GetString("DumpFolder", @"C:\Users\Administrator\AppData\Local\Bentley\SCADAConnect\10");
            //this.DemandConfigurationWorkbook = exchangeContext.GetString("DemandConfigurationWorkbook", @"C:\WG2TW\Grundfos.WG.PostCalc\WaterDemandSettings.xlsx");

            // Rest of *.ini file parameters.
            //this.DumpOption = exchangeContext.GetString("DumpOption", @"1");
            //this.OpcServerAddress = "Kepware.KEPServerEX.V6";            
            //this.IsLogToDb = bool.Parse(exchangeContext.GetString("IsLogToDb", "false"));
            //this.LogDbConnString = exchangeContext.GetString("LogDbConnString", @"Data Source=.\SQLEXPRESS;Initial Catalog=WG;Integrated Security=True").Replace(":",";");
            //this.IsCalculationOnDb = bool.Parse(exchangeContext.GetString("IsCalculationOnDb", "false"));
            

            var waterInfraConnString = exchangeContext.GetString("WaterInfraConnString", @"Server=192.168.0.62\MSSQL2017;Database=WaterInfra;User Id=sa;Password=Gfosln123.;").Replace(":",";");
            ZoneDemandDataListCreatorNew.DataContext dataContextNew = new ZoneDemandDataListCreatorNew.DataContext()
            {
                WaterInfraConnString = waterInfraConnString,
            };
            _zoneDemandDataListCreatorNew = new ZoneDemandDataListCreatorNew(dataContextNew, this.Logger);

            return true;
        }

        public override bool DoDataExchange(object dataExchangeContext)
        {
            this.Logger.WriteMessage(OutputLevel.Info, "-- DoDataExchange started --------------------------------------------------------------------");

            // Dictionary<int, string> <- WaterGEMS {{n, "1 - Przybków"},... {n, "16 - Pompownia"}}
            var zoneReader = new ZoneReader(this._domainDataSet);
            var wgZones = zoneReader.GetZones();

            // dataExchangeContext <- ResultCache.sqlite
            this.PassQualityResults(dataExchangeContext);

            // publish results to OPC server
            this.PublishOpcResults(dataExchangeContext, wgZones);

            // Wait for time in seconds taken from SQL.
            //if (IsLogToDb)
            //{
            //    int seconds = GetDelayTimeFromSql();
            //    this.Logger.WriteMessage(OutputLevel.Info, $"-- SCADAPostCalculationDataExchanger started waiting for {seconds} seconds.");
            //    Thread.Sleep(seconds*1000);
            //    this.Logger.WriteMessage(OutputLevel.Info, $"-- SCADAPostCalculationDataExchanger finished waiting.");
            //}
            int seconds = _zoneDemandDataListCreatorNew.GetDelayTimeFromSql();
            this.Logger.WriteMessage(OutputLevel.Info, $"-- SCADAPostCalculationDataExchanger started waiting for {seconds} seconds.");
            Thread.Sleep(seconds * 1000);
            this.Logger.WriteMessage(OutputLevel.Info, $"-- SCADAPostCalculationDataExchanger finished waiting.");

            // Calculate and set up BaseDemands for Junctions, Hydrants and CustomerMeters in WaterGEMS.
            this.ExchangeWaterDemands(dataExchangeContext);

            return true;
        }

        #region PassQualityResults

        private void PassQualityResults(object dataExchangeContext)
        {
            try
            {
                this.Logger.WriteMessage(OutputLevel.Info, "-- PassQualityResults started -------------------------------------------");

                var db = new DatabaseContext(this._sqliteFile);                      // ResultCache.sqlite
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
                new GenericDataExchanger(this.Logger, this._scenario, this._domainDataSet, repository, ageConfig),
                new GenericDataExchanger(this.Logger, this._scenario, this._domainDataSet, repository, traceConfig),
                new GenericDataExchanger(this.Logger, this._scenario, this._domainDataSet, repository, concentrationConfig),
            };

            return dataExchangers;
        }

        #endregion

        #region PublishOpcResults

        private void PublishOpcResults(object dataExchangeContext, Dictionary<int, string> wgZones)
        {
            try
            {
                this.Logger.WriteMessage(OutputLevel.Info, "-- PublishOpcResults started --------------------------------------------");

                // ICollection<OpcMapping> mappings <- excel.OpcMapping group by FieldName without "Result Attribute Label" column.
                ICollection<OpcMapping> mappings = _zoneDemandDataListCreatorNew.GetOpcMappingList();

                List<OpcPublisher> publishers = this.BuildPublishers(mappings, wgZones);

                this.Logger.WriteMessage(OutputLevel.Info, "Start writing results to OPC.");
                foreach (var publisher in publishers)
                {
                    publisher.PublishResults(this._domainDataSet, this._scenario);
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
        private void ExchangeWaterDemands(object dataExchangeContext)
        {
            this.Logger.WriteMessage(OutputLevel.Info, "-- ExchangeWaterDemands started -----------------------------------------");

            try
            {
                #region Create List<ZoneDemandData>

                List<ZoneDemandData> zoneDemandDataList;

                //ZoneDemandDataListCreatorNew.DataContext dataContextNew = new ZoneDemandDataListCreatorNew.DataContext()
                //{
                //    WaterInfraConnString = WaterInfraConnString,
                //};
                //ZoneDemandDataListCreatorNew zoneDemandDataListCreatorNew = new ZoneDemandDataListCreatorNew(dataContextNew, this.Logger);
                zoneDemandDataList = _zoneDemandDataListCreatorNew.Create(DateTime.Now);
                Helper.DumpToFile(zoneDemandDataList.FirstOrDefault(x => x.ZoneName == TestedZoneName), Path.Combine(_dumpFolder, $"Dump_{DateTime.Now.ToString(DateFormat)}_ZoneDemandData_2_New.xml"));

                #endregion


                #region  Write data to WaterGEMS.

                // Two arrays of int: "Excluded Object IDs" and "Excluded Demand Patterns"
                var demandConfig = this.GetDemandWriterConfigNew(zoneDemandDataList);
                
                //demandConfig.IsCalculationOnDb = IsCalculationOnDb;
                
                var demandWriter = new WaterDemandDataWriter(this.Logger, this._domainDataSet, demandConfig, (DataExchangerContext)dataExchangeContext);

                foreach (var zoneDemand in zoneDemandDataList.Where(x => Math.Abs(x.ScadaDemand) > 0.001))
                {
                    demandWriter.WriteDemands(zoneDemand);
                }

                #endregion
            }
            catch (Exception ex)
            {
                this.Logger.WriteMessage(OutputLevel.Errors, "Errors occured when exchanging water demand.");
                this.Logger.WriteException(ex, true);
            }
        }

        private WaterDemandDataWriterConfiguration GetDemandWriterConfigNew(List<ZoneDemandData> zoneDemandDataList)
        {
            //ZoneDemandDataListCreatorNew.DataContext dataContextNew = new ZoneDemandDataListCreatorNew.DataContext()
            //{
            //    WaterInfraConnString = WaterInfraConnString,
            //};
            //ZoneDemandDataListCreatorNew zoneDemandDataListCreatorNew = new ZoneDemandDataListCreatorNew(dataContextNew, this.Logger);

            // List<string> <- Excel.ExcludedItems["Excluded Object IDs"].
            // {257=PC, 2719=S5, 518=CP1, 701=CP2, 1323=CP3, 1336=CP4, 2255=S6, 2780=S7, 1240=W1, 1239=W2, 1548=CP6}    
            var excludedObjects = _zoneDemandDataListCreatorNew.GetExcludedObjectId(zoneDemandDataList);

            // List<string> <- Excel.ExcludedItems["Excluded Demand Patterns"]. {"nieaktywni1", "Straty1"}.    
            var excludedPatterns = _zoneDemandDataListCreatorNew.GetExcludedDemandPatternId(zoneDemandDataList);

            var demandConfig = new WaterDemandDataWriterConfiguration
            {
                ExcludedObjectIDs = excludedObjects.ToArray(),
                ExcludedDemandPatterns = excludedPatterns.ToArray(),
            };
            return demandConfig;
        }

        #endregion

    }
}
