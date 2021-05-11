using System.Collections.Generic;
using System.Linq;

namespace WbEasyCalcRepository.Model
{
    internal class BilledConsumptionSheet
    {
        public BilledConsumptionSheet()
        {
            this.BilledMeteredConsumptionWithoutBulkSupply_D8_D25 = new List<double>() { };
            this.BilledUnmeteredConsumptionWithoutBulkSupply_H8_H25 = new List<double>();
        }

        public double BilledMeteredConsumptionBulkWaterSupplyExportM3_D6 { get; set; }
        public List<double> BilledMeteredConsumption_D6_D25 { get => this.BilledMeteredConsumptionWithoutBulkSupply_D8_D25.Union(new List<double> { this.BilledMeteredConsumptionBulkWaterSupplyExportM3_D6 }).ToList(); }
        public List<double> BilledMeteredConsumptionWithoutBulkSupply_D8_D25 { get; set; }
        public double BilledMeteredConsumption_D28 { get => this.BilledMeteredConsumption_D6_D25.Sum(); }
        public double BulledUnmeteredConsumptionBulkWaterSupplyExportM3_H6 { get; set; }
        public List<double> BilledUnmeteredConsumption_H6_H25 { get => this.BilledUnmeteredConsumptionWithoutBulkSupply_H8_H25.Union(new List<double> { this.BulledUnmeteredConsumptionBulkWaterSupplyExportM3_H6 }).ToList(); }
        public List<double> BilledUnmeteredConsumptionWithoutBulkSupply_H8_H25 { get; set; }
        public double BilledUnmeteredConsumption_H28 { get => this.BilledUnmeteredConsumption_H6_H25.Sum(); }
    }
}