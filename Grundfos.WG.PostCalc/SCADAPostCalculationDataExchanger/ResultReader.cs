using Grundfos.OPC;
using Grundfos.OPC.Model;
using Grundfos.WG.OPC.Publisher.Configuration;
using Haestad.Domain;
using Haestad.Support.OOP.Logging;
using Haestad.Support.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Grundfos.WG.PostCalc
{
    public class ResultReader
    {
        private ActionLogger _logger;
        private IDomainDataSet _domainDataSet;
        private IScenario _scenario;

        public ResultReader(ActionLogger logger, IDomainDataSet domainDataSet, IScenario scenario)
        {
            _logger = logger;
            _domainDataSet = domainDataSet;
            _scenario = scenario;
        }


        public void GetResults(ICollection<OpcMapping> opcMappingList, Dictionary<int, int> objIdZoneIdDict)
        {
            try
            {
                _logger?.WriteMessage(OutputLevel.Info, $"-- Test: GetResults(). ----------------------------------------");

                GetResultParam getResultParam;
                Dictionary<int, double> resultDict;

                // ZoneAveragePressure
                getResultParam = new GetResultParam
                {
                    ResultRecordName = StandardResultRecordName.IdahoPressureNodeResults,                       // "IdahoPressureNodeResults"
                    ResultAttributeRecordName = StandardResultRecordName.IdahoPressureNodeResults_NodePressure, // "IdahoPressureNodeResults_NodePressure"
                    FieldName = StandardFieldName.PipeStatus,                                                   // "PipeStatus"
                    ElementTypes = new DomainElementType[]
                    {
                            DomainElementType.BaseIdahoNodeElementManager,                                      // 50
                    },
                };
                resultDict = GetResults(getResultParam);
                //_logger?.WriteMessage(OutputLevel.Info, $"ZoneAveragePressure count: {resultDict.Count}.");
                var zoneAveragelist = resultDict
                    .Where(d => !double.IsNaN(d.Value))
                    .Join(
                        objIdZoneIdDict,
                        l => l.Key,
                        r => r.Key,
                        (l, r) => new { ZoneId = r.Value, ObjPressure = l.Value * Constants.Pressure_PSI_2_mH2O }
                        )
                    .GroupBy(x => x.ZoneId)
                    .Select(g => new { ZoneId = g.Key, Avg = g.Average(y => y.ObjPressure) })
                    .ToDictionary(d => d.ZoneId, d => d.Avg);
                //_logger?.WriteMessage(OutputLevel.Info, $"Average pressure for zones.");
                //foreach (var item in zoneAveragelist.OrderBy(x => x.Key))
                //{
                //    _logger?.WriteMessage(OutputLevel.Info, $"\t{item.Key}, \t{item.Value}");
                //}
                SendToOpc(zoneAveragelist, opcMappingList, "ZoneAveragePressure");

                // TankPercentFull
                getResultParam = new GetResultParam
                {
                    ResultRecordName = StandardResultRecordName.IdahoConventionalTankResults,
                    ResultAttributeRecordName = StandardResultRecordName.IdahoConventionalTankResults_CalculatedPercentFull,
                    FieldName = StandardFieldName.ElementType_TankPercentFull,
                    ElementTypes = new DomainElementType[]
                    {
                        DomainElementType.IdahoTankElementManager,
                    },
                };
                resultDict = GetResults(getResultParam);
                SendToOpc(resultDict, opcMappingList, "TankPercentFull");

                // PipeStatus
                getResultParam = new GetResultParam
                {
                    ResultRecordName = StandardResultRecordName.IdahoPipeResults,
                    ResultAttributeRecordName = StandardResultRecordName.PipeResultControlStatus,
                    FieldName = StandardFieldName.PipeStatus,
                    ElementTypes = new DomainElementType[]
                    {
                        DomainElementType.IdahoPipeElementManager,
                    },
                };
                resultDict = GetResults(getResultParam);
                SendToOpc(resultDict, opcMappingList, "PipeStatus");
            }
            catch (Exception ex)
            {
                _logger?.WriteMessage(OutputLevel.Errors, "Errors occured in GetResults().");
                _logger?.WriteException(ex, true);
            }
        }



        private Dictionary<int, double> GetResults(GetResultParam configuration)
        {
            // Log the start of the process with the "Info" level priority.
            // (Users can control the verbosity of log output to only see the level of detail they want).
            this._logger?.WriteMessage(OutputLevel.Info, $"Get results from WaterGEMS for: \"{configuration.ResultAttributeRecordName}\".");

            // Acquire the numerical engine name that supports Water Quality results.
            // This could also be hard coded as: StandardCalculationOptionFieldName.EpaNetEngine
            string engineName = _scenario.GetActiveNumericalEngineTypeName(
                configuration.ResultRecordName
                );

            // Acquire the relevant field or fields that we want to read results for.
            IResultTimeVariantField timeVariantField = _domainDataSet.FieldManager.ResultField(
                configuration.ResultAttributeRecordName,
                engineName,
                configuration.ResultRecordName
                ) as IResultTimeVariantField;

            double[] timeSteps = _domainDataSet.NumericalEngine(engineName).ResultDataConnection.TimeStepsInSeconds(_scenario.Id);

            var elementTypes = new HmIDCollection(configuration.ElementTypes.Select(x => (int)x).ToArray());

            var simulationValues = this.GetSimulationValues(timeVariantField, timeSteps, elementTypes, _scenario.Id);

            return simulationValues;
        }

        private Dictionary<int, double> GetSimulationValues(IResultTimeVariantField timeVariantField, double[] timeSteps, HmIDCollection elementTypes, int scenarioID)
        {
            var result = new Dictionary<int, double>();
            var values = timeVariantField.GetValues(elementTypes, scenarioID, timeSteps.Length - 1);
            IDictionaryEnumerator enumerator = values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                int elementID = (int)enumerator.Key;
                double doubleValue = Convert.ToDouble(enumerator.Value);
                result[elementID] = doubleValue;
            }

            return result;
        }

        private void SendToOpc(Dictionary<int, double> simulationValueList, ICollection<OpcMapping> opcMappingList, string fieldName)
        {
            _logger?.WriteMessage(OutputLevel.Info, $"\"{fieldName}\" simulation values count: {simulationValueList.Count}.");

            ICollection<OpcWriteValue> writeValues = simulationValueList.Join(
                    opcMappingList.FirstOrDefault(x => x.FieldName == fieldName).Mappings,
                    l => l.Key,
                    r => r.ElementID,
                    (l, r) => new OpcWriteValue() { TagName = r.OpcTag, Value = l.Value }
                )
                .ToList();

            _logger?.WriteMessage(OutputLevel.Info, $"\"{fieldName}\" OPC values count: {writeValues.Count}.");
            //foreach (var item in writeValues)
            //{
            //    _logger?.WriteMessage(OutputLevel.Info, $"\t{item.TagName}, \t{item.Value}");
            //}

            var publisher = new OpcWriter("Kepware.KEPServerEX.V6");
            publisher.Publish(writeValues.ToArray());
            publisher.Dispose();
        }

        private class GetResultParam
        {
            public string ResultRecordName { get; set; }
            public string ResultAttributeRecordName { get; set; }
            public string FieldName { get; set; }
            public DomainElementType[] ElementTypes { get; set; }

            //public AlternativeType Alternative { get; set; }
        }


    }
}
