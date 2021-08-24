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
          
            var waterInfraConnString = exchangeContext.GetString("WaterInfraConnString", @"Server=192.168.0.62\MSSQL2017;Database=WaterInfra;User Id=sa;Password=Gfosln123.;").Replace(":",";");
            this.Logger.WriteMessage(OutputLevel.Info, waterInfraConnString);
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

            // Quality result save to cache and load from cache. dataExchangeContext <- ResultCache.sqlite
            this.PassQualityResults(dataExchangeContext);

            // Publish results to OPC server
            ICollection<OpcMapping> opcMappingList = _zoneDemandDataListCreatorNew.GetOpcMappingList();
            Dictionary<int, int> objIdZoneIdDict = _zoneDemandDataListCreatorNew.GetObjIdZoneIdDict();
            ResultReader resultReader = new ResultReader(Logger, _domainDataSet, _scenario);
            resultReader.GetResults(opcMappingList, objIdZoneIdDict);

            // Wait for time in seconds taken from SQL.
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

                zoneDemandDataList = _zoneDemandDataListCreatorNew.Create(DateTime.Now);
                Helper.DumpToFile(zoneDemandDataList.FirstOrDefault(x => x.ZoneName == TestedZoneName), Path.Combine(_dumpFolder, $"Dump_{DateTime.Now.ToString(DateFormat)}_ZoneDemandData_2_New.xml"));

                #endregion

                #region  Write data to WaterGEMS.

                // Two arrays of int: "Excluded Object IDs" and "Excluded Demand Patterns"
                var demandConfig = this.GetDemandWriterConfigNew(zoneDemandDataList);
                
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
