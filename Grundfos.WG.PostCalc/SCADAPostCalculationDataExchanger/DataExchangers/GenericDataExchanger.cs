using Grundfos.WG.PostCalc.Persistence.Model;
using Grundfos.WG.PostCalc.Persistence.Repositories;
using Haestad.Domain;
using Haestad.Support.OOP.Logging;
using Haestad.Support.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Grundfos.WG.PostCalc.DataExchangers
{
    public class GenericDataExchanger : DataExchangerBase
    {
        public GenericDataExchanger(
            ActionLogger logger,
            IScenario scenario,
            IDomainDataSet domainDataSet,
            IPostCalcRepository repository,
            DataExchangerConfiguration configuration)
            : base(logger, scenario, domainDataSet)
        {
            this.Repository = repository;
            this.Configuration = configuration;
        }

        public IPostCalcRepository Repository { get; }
        public DataExchangerConfiguration Configuration { get; }

        /// <summary>
        /// 1. Save last WG simulation to 'simulationValues' dictionary (ObjectID, Value) for a particular attribute (ex: IdahoWaterQualityResults_CalculatedAge).
        /// 2. Load simulation from SQLite Results table to 'storedValues' dictionary (ObjectID, Value) for a particular attribute (ex: IdahoWaterQualityResults_CalculatedAge).
        /// 3. Check if current process runs first time after a break by comparison current process info to process info saved in SQLite ResultTimestamps table.
        ///     if true (process runs first time after a break):
        ///         1. Update (or insert) current process info to SQLite ResultTimestamps table.
        ///         2. Update WG based on 'storedValues' provided ObjectID exists in simulationValues. WG.Field = storedValues["ObjectID"].Value * Configuration.ConversionFactor.
        ///     else
        ///         1. Replace (or insert) content of SQLite Results table with WG simulationValues dictionary.
        ///         2. Update WG based on simulationValues. WG.Field = simulationValues["ObjectID"].Value * Configuration.ConversionFactor.
        /// </summary>
        /// <param name="dataExchangeContext">WG: IdahoWaterQualityResults_Calculated[Age|Trace|Concentration]</param>
        /// <returns></returns>
        public override bool DoDataExchange(object dataExchangeContext)
        {
            try
            {
                this.Logger?.WriteMessage(OutputLevel.Info, $"DataExchangerConfiguration = '{this.Configuration}'");

                // Log the start of the process with the "Info" level priority.
                // (Users can control the verbosity of log output to only see the level of detail they want).
                // ex.: StandardResultRecordName.IdahoWaterQualityResults_CalculatedAge
                this.Logger?.WriteMessage(OutputLevel.Info, $"Reading {this.Configuration.ResultAttributeRecordName}...");

                // Acquire the numerical engine name that supports Water Quality results.
                // This could also be hard coded as: StandardCalculationOptionFieldName.EpaNetEngine
                // ex.: StandardResultRecordName.IdahoWaterQualityResults
                string engineName = this.Scenario.GetActiveNumericalEngineTypeName(this.Configuration.ResultRecordName);

                // Acquire the relevant field or fields that we want to read results for.
                // .ResultField(string name, string numericalEngineType, string resultRecordTypeName)
                IResultTimeVariantField timeVariantField = this.DomainDataSet.FieldManager.ResultField(
                    this.Configuration.ResultAttributeRecordName,   // ex.: StandardResultRecordName.IdahoWaterQualityResults_CalculatedAge 
                    engineName,                                     
                    this.Configuration.ResultRecordName             // ex.: StandardResultRecordName.IdahoWaterQualityResults
                ) as IResultTimeVariantField;

                // Acquire the "look ahead" time steps that have been simulated (we want to use the last time step).
                double[] timeSteps = this.DomainDataSet.NumericalEngine(engineName).ResultDataConnection.TimeStepsInSeconds(this.Scenario.Id);
                // My
                this.Logger?.WriteMessage(OutputLevel.Info, $"# WG timeSteps.Count = {timeSteps.Length}");
                foreach (var item in timeSteps.Take(10))
                {
                    this.Logger?.WriteMessage(OutputLevel.Info, $"\t{item}");
                }

                // Specify the element types that we want to load WQ data for.
                var elementTypes = new HmIDCollection
                {
                    (int)DomainElementType.IdahoJunctionElementManager,
                    (int)DomainElementType.IdahoTankElementManager,
                    (int)DomainElementType.IdahoHydrantElementManager,
                    (int)DomainElementType.IdahoReservoirElementManager
                };

                // Dictionary<int, double> simulationValues <- WaterGames todo: which elements on WaterGEMS UI?
                var simulationValues = this.GetSimulationValues(timeVariantField, timeSteps, elementTypes);
                // My
                this.Logger?.WriteMessage(OutputLevel.Info, $"# WG simulationValues.Count = {simulationValues.Count}");
                foreach (var keyValuePair in simulationValues.Take(10))
                {
                    this.Logger?.WriteMessage(OutputLevel.Info, $"\t{keyValuePair.Key} - {keyValuePair.Value}");                    
                }

                // Dictionary<int, double> storedValues <- SQLite: SELECT ObjectID, Value FROM Results WHERE Attribute = 'IdahoWaterQualityResults_CalculatedAge' 
                var storedValues = this.Repository.GetResultsByAttribute(this.Configuration.ResultAttributeRecordName).ToDictionary(x => x.ObjectID, x => x.Value);
                // My
                this.Logger?.WriteMessage(OutputLevel.Info, $"# SQLite storedValues.Count = {storedValues.Count}");
                foreach (var keyValuePair in storedValues.Take(10))
                {
                    this.Logger?.WriteMessage(OutputLevel.Info, $"\t{keyValuePair.Key} - {keyValuePair.Value}");                    
                }
                this.Logger?.WriteMessage(OutputLevel.Info, $"# simulationValues not found in storedValues:");
                foreach (var keyValuePair in simulationValues.Where(x => storedValues.All(y => x.Key != y.Key)))
                {
                    this.Logger?.WriteMessage(OutputLevel.Info, $"\t{keyValuePair.Key} - {keyValuePair.Value}");                    
                }
                this.Logger?.WriteMessage(OutputLevel.Info, $"# storedValues not found in simulationValues:");
                foreach (var keyValuePair in storedValues.Where(x => simulationValues.All(y => x.Key != y.Key)))
                {
                    this.Logger?.WriteMessage(OutputLevel.Info, $"\t{keyValuePair.Key} - {keyValuePair.Value}");                    
                }

                // Checks if the current process exists in the SQLite processes table (ResultTimestamps). Return true if not exists.
                bool processWasInterrupted = this.ProcessWasInterrupted(this.Configuration.ResultAttributeRecordName, out ProcessInfo processInfo);
                // My
                this.Logger?.WriteMessage(OutputLevel.Info, $"# SQLite processWasInterrupted = {processWasInterrupted}");

                if (!processWasInterrupted)
                {
                    var data = simulationValues
                        .Where(x => !double.IsNaN(x.Value))
                        .Select(x => new Result { ObjectID = x.Key, Value = x.Value });
                    this.Repository.ClearResultsByAttribute(this.Configuration.ResultAttributeRecordName);
                    this.Repository.SetResults(this.Configuration.ResultAttributeRecordName, data);
                }
                else
                {
                    var resultTimestamp = new ResultTimestamp
                    {
                        Attribute = this.Configuration.ResultAttributeRecordName,
                        ProcessID = processInfo.ProcessID,
                        ProcessStartTime = processInfo.ProcessStartTime
                    };
                    this.Repository.SetResultTimestamp(resultTimestamp);
                }

                this.Repository.SaveChanges();


                // WRITING THE DATA ----

                this.Logger?.WriteMessage(OutputLevel.Info, $"Writing {this.Configuration.ResultAttributeRecordName}...");

                // Acquire the alternative (or alternatives) that we want to write the WQ data back to.
                int alternativeId = this.Scenario.AlternativeID((int)this.Configuration.Alternative);   // Alternative = AlternativeType.AgeAlternative

                // Acquired the input field (or input fields) that we want to write the WQ data to.
                IEditField field = DomainDataSet
                    .AlternativeManager((int)this.Configuration.Alternative)    // Alternative = AlternativeType.AgeAlternative
                    .AlternativeField(
                        this.Configuration.FieldName,                           // FieldName = StandardFieldName.Age_InitialAge,
                        (int)DomainElementType.BaseIdahoNodeElementManager,
                        alternativeId
                    ) as IEditField;

                int valuesWrittenCount = 0;

                // Loops through the values and write them into the model.
                foreach (var item in simulationValues)
                {
                    int elementID = item.Key;
                    double doubleValue;
                    if (processWasInterrupted)
                    {
                        // doubleValue is taken from SQLite storedValues.
                        if (!storedValues.TryGetValue(elementID, out doubleValue))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        // doubleValue is taken from current WG simulationValues.
                        doubleValue = item.Value;
                    }

                    double valueToWrite = doubleValue * this.Configuration.ConversionFactor;
                    field.SetValue(elementID, valueToWrite);

                    this.Logger?.WriteMessage(OutputLevel.Debug, $"{this.Configuration.ResultAttributeRecordName}: ID={elementID}, Value={doubleValue}");

                    // Count the value
                    valuesWrittenCount++;

                    // My
                    if (valuesWrittenCount <= 10)
                    {
                        this.Logger?.WriteMessage(OutputLevel.Info, $"\tID={elementID}, Value={doubleValue}, {doubleValue}*{this.Configuration.ConversionFactor}={valueToWrite}");
                    }
                }

                this.Logger?.WriteMessage(OutputLevel.Info, $"Wrote {valuesWrittenCount} {this.Configuration.ResultAttributeRecordName} values.");

                return true;
            }
            catch (Exception e)
            {
                this.Logger?.WriteMessage(OutputLevel.Errors, $"Error encountered during processing of {this.Configuration.ResultAttributeRecordName} results.");
                this.Logger?.WriteException(e, true);

                return false;
            }
        }

        //private bool TryGetAtrributeValue(bool useDefault, int key, Dictionary<int, double> data, double defaultValue, out double result)
        //{
        //    if (!useDefault)
        //    {
        //        result = defaultValue;
        //        return true;
        //    }

        //    return data.TryGetValue(key, out result);
        //}

        private Dictionary<int, double> GetSimulationValues(IResultTimeVariantField timeVariantField, double[] timeSteps, HmIDCollection elementTypes)
        {
            var result = new Dictionary<int, double>();
            var values = timeVariantField.GetValues(elementTypes, this.Scenario.Id, timeSteps.Length - 1);
            IDictionaryEnumerator enumerator = values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                int elementID = (int)enumerator.Key;
                double doubleValue = (double)enumerator.Value;
                result[elementID] = doubleValue;
            }

            return result;
        }

        /// <summary>
        /// Checks if the current process exists in the SQLite processes table (ResultTimestamps). Return true if not exists.  
        /// </summary>
        /// <param name="attribute">ex.: "IdahoWaterQualityResults_CalculatedAge"</param>
        /// <param name="currentProcessInfo">Current process info: System.Diagnostics.ProcessID and System.Diagnostics.StartTime.</param>
        /// <returns></returns>
        private bool ProcessWasInterrupted(string attribute, out ProcessInfo currentProcessInfo)
        {
            currentProcessInfo = ProcessInfoService.GetCurrentProcessInfo();
            // My
            this.Logger?.WriteMessage(OutputLevel.Info, $"# currentProcessInfo = '{currentProcessInfo}'");
            var dbProcessInfo = this.Repository.GetResultTimestamp(attribute);
            if (dbProcessInfo == null)
            {
                return true;
            }

            bool processWasInterrupted = currentProcessInfo.ProcessID != dbProcessInfo.ProcessID || currentProcessInfo.ProcessStartTime != dbProcessInfo.ProcessStartTime;
            return processWasInterrupted;
        }
    }
}
