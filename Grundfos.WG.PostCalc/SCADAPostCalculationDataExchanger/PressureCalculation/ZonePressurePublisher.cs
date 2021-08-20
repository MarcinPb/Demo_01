using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grundfos.OPC.Model;
using Grundfos.WG.OPC.Publisher;
using Grundfos.WG.OPC.Publisher.Configuration;
using Haestad.Domain;
using Haestad.Support.OOP.Logging;
using Haestad.Support.Support;

namespace Grundfos.WG.PostCalc.PressureCalculation
{
    public class ZonePressurePublisher : OpcPublisher
    {
        public ZonePressurePublisher(ZonePressurePublisherConfiguration configuration, ActionLogger logger) : base(configuration, logger)
        {
        }

        public ZonePressurePublisherConfiguration ZonePressurePublisherConfiguration { get => (ZonePressurePublisherConfiguration)this.Configuration; }

        public override void PublishResults(IDomainDataSet domainDataSet, IScenario scenario)
        {
            // Log the start of the process with the "Info" level priority.
            // (Users can control the verbosity of log output to only see the level of detail they want).
            this.Logger?.WriteMessage(OutputLevel.Info, $"Reading {this.Configuration.ResultAttributeRecordName} ...");

            // Acquire the numerical engine name that supports Water Quality results.
            // This could also be hard coded as: StandardCalculationOptionFieldName.EpaNetEngine
            string engineName = scenario.GetActiveNumericalEngineTypeName(
                this.Configuration.ResultRecordName
                );

            // Acquire the relevant field or fields that we want to read results for.
            IResultTimeVariantField timeVariantField = domainDataSet.FieldManager.ResultField(
                this.Configuration.ResultAttributeRecordName,
                engineName,
                this.Configuration.ResultRecordName
                ) as IResultTimeVariantField;

            double[] timeSteps = domainDataSet.NumericalEngine(engineName).ResultDataConnection.TimeStepsInSeconds(scenario.Id);

            var elementTypes = new HmIDCollection(this.Configuration.ElementTypes.Select(x => (int)x).ToArray());

            var simulationValues = this.GetSimulationValues(timeVariantField, timeSteps, elementTypes, scenario.Id);
            var objectsWithZones = this.ReadZones(domainDataSet);
            
            // Added 2021-08-20 in order to test
            this.Logger?.WriteMessage(OutputLevel.Info, $"SimulationValues:");
            //foreach(var item in simulationValues
            //    .Where(f => objectsWithZones.Any(z => z.Key == f.Key && z.Value == 6775))
            //    .OrderBy(o => o.Key)
            //    )
            //{
            //    this.Logger?.WriteMessage(OutputLevel.Info, $"\t{item.Key}\t{item.Value}");
            //}
            //

            List<ZonePressureData> zonePressures = this.GetPressureData(simulationValues, objectsWithZones);
            var pressureDictionary = zonePressures.ToDictionary(x => x.ZoneID, x => x.AveragePressure);

            // Added 2021-08-20 in order to test
            this.Logger?.WriteMessage(OutputLevel.Info, $"Average pressure for zones:");
            foreach(var item in pressureDictionary.OrderBy(o => o.Key))
            {
                this.Logger?.WriteMessage(OutputLevel.Info, $"\t{item.Key}\t{item.Value}");
            }
            //

            this.PublishValuesToOpc(pressureDictionary);

            this.Logger?.WriteMessage(OutputLevel.Info, $"Published {simulationValues.Count} {this.Configuration.ResultAttributeRecordName} values to OPC.");
        }

        private List<ZonePressureData> GetPressureData(Dictionary<int, double> simulationValues, Dictionary<int, int> objectsWithZones)
        {
            var convertedValues = simulationValues.ToDictionary(x => x.Key, x => x.Value * Constants.Pressure_PSI_2_mH2O);
            foreach (var item in convertedValues)
            {
                this.Logger.WriteMessage(OutputLevel.Debug, $"Value\t{item.Key}\t{item.Value}");
            }
            var result = new List<ZonePressureData>();
            var zones = objectsWithZones.GroupBy(x => x.Value).Select(x => new { Zone = x.Key, ElementIDs = x.Select(xx => xx.Key).ToList() }).ToList();
            foreach (var zone in zones)
            {
                double sum = 0;
                int count = 0;
                foreach (var elementID in zone.ElementIDs)
                {
                    if (convertedValues.TryGetValue(elementID, out double value) && !double.IsNaN(value))
                    {
                        sum += value;
                        count++;
                    }
                    else
                    {

                    }
                }

                double average = sum / count;
                result.Add(new ZonePressureData { ZoneID = zone.Zone, AveragePressure = average, Sum = sum, Count = count });
            }

            return result;
        }

        /// <summary>
        /// Creates dictionary for all jubctions (ObjType=55) with Key = JunctionId and Value = ZoneId
        /// </summary>
        /// <param name="domainDataSet">WaterGEMS IDomainDataSet</param>
        /// <returns>Dictionary for all jubctions (ObjType=55) with Key = JunctionId and Value = ZoneId</returns>
        private Dictionary<int, int> ReadZones(IDomainDataSet domainDataSet)
        {
            var result = new Dictionary<int, int>();

            int elementTypeID = Constants.NodeID;
            var manager = domainDataSet.DomainElementManager(elementTypeID);
            var elementIDs = manager.ElementIDs();

            var supportedFields = manager.SupportedFields().Cast<IField>().ToDictionary(x => x.Name, x => x);
            var zoneField = supportedFields["Physical_Zone"];
            foreach (var elementID in elementIDs)
            {
                var rawZoneID = zoneField.GetValue(elementID);
                int zoneID = rawZoneID is int ? (int)rawZoneID : -1;
                result[elementID] = zoneID;
            }

            return result;
        }
    }
}
