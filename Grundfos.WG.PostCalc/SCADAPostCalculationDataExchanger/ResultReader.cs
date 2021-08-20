using Grundfos.WG.OPC.Publisher.Configuration;
using Haestad.Domain;
using Haestad.Support.OOP.Logging;
using Haestad.Support.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public void GetResults()
        {
            try
            {
                _logger?.WriteMessage(OutputLevel.Info, $"-- Test: GetResults(). ----------------------------------------");

                var configuration = new PublisherConfiguration
                {
                    ResultRecordName = StandardResultRecordName.IdahoPressureNodeResults,                       // "IdahoPressureNodeResults"
                    ResultAttributeRecordName = StandardResultRecordName.IdahoPressureNodeResults_NodePressure, // "IdahoPressureNodeResults_NodePressure"
                    FieldName = StandardFieldName.PipeStatus,                                                   // "PipeStatus"
                    ElementTypes = new DomainElementType[]
                    {
                            DomainElementType.BaseIdahoNodeElementManager,                                      // 50
                    },
                };

                var dict = GetResults(configuration);
                _logger?.WriteMessage(OutputLevel.Info, $"Dictionary count: {dict.Count}.");
                foreach (var item in dict.Take(20))
                {
                    _logger?.WriteMessage(OutputLevel.Info, $"\t{item.Key}, \t{item.Value}");
                }
            }
            catch (Exception ex)
            {
                _logger?.WriteMessage(OutputLevel.Errors, "Errors occured in GetResults().");
                _logger?.WriteException(ex, true);
            }
        }

        private Dictionary<int, double> GetResults(PublisherConfiguration configuration)
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
    }
}
