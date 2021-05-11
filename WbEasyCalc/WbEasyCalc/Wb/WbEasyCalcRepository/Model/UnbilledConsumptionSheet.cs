using System;
using System.Collections.Generic;
using System.Linq;

namespace WbEasyCalcRepository.Model
{
    public class UnbilledConsumptionSheet
    {
        public UnbilledConsumptionSheet()
        {
            this.UnbilledMeteredConsumptionWithoutBulkSupply_D8_D23 = new List<double>();
            this.UnbilledUnmeteredConsumptionM3_H6_H23 = new List<double>();
            this.UnbilledUnmeteredConsumptionError_J6_J23 = new List<double>();
        }
        public double MeteredConsumptionBulkWaterSupplyExportM3_D6 { get; set; }
        public List<double> UnbilledMeteredConsumption_D6_D23 { get => this.UnbilledMeteredConsumptionWithoutBulkSupply_D8_D23.Union(new List<double> { MeteredConsumptionBulkWaterSupplyExportM3_D6 }).ToList(); }
        public List<double> UnbilledMeteredConsumptionWithoutBulkSupply_D8_D23 { get; set; }
        public double UnbilledMeteredConsumption_D32 { get => this.UnbilledMeteredConsumption_D6_D23.Sum(); }
        public List<double> UnbilledUnmeteredConsumptionM3_H6_H23 { get; set; }
        public List<double> UnbilledUnmeteredConsumptionError_J6_J23 { get; set; }
        public double UnbilledUnmeteredConsumption_H32 { get => this.UnbilledUnmeteredConsumptionM3_H6_H23.Sum(); }
        public double ErrorFactor_O25 { get => Math.Sqrt(this.GetErrorFactorized().Sum()); }
        public double UnbilledUnmeteredConsumptionErrorMargin_J25 { get => GetUnbilledUnmeteredConsumptionErrorMargin_J25(); }
        private double GetUnbilledUnmeteredConsumptionErrorMargin_J25()
        {
            if (this.UnbilledUnmeteredConsumption_H32 == 0)
            {
                return 0;
            }

            double j25 = this.ErrorFactor_O25 * Constants.StandardDistributionFactor / this.UnbilledUnmeteredConsumption_H32;
            return j25;
        }

        private List<double> GetErrorFactorized()
        {
            var result = new List<double>();
            for (int i = 0; i < this.UnbilledUnmeteredConsumptionM3_H6_H23.Count; i++)
            {
                double item = Math.Pow(this.UnbilledUnmeteredConsumptionM3_H6_H23[i] * this.UnbilledUnmeteredConsumptionError_J6_J23[i] / Constants.StandardDistributionFactor , 2);
                result.Add(item);
            }

            return result;
        }

        public double Min_H28 { get => this.GetMin_H28(); }
        private double GetMin_H28()
        {
            return UnbilledUnmeteredConsumption_H32 == 0 ? 0 : UnbilledUnmeteredConsumption_H32 * (1 - UnbilledUnmeteredConsumptionErrorMargin_J25);
        }
        public double Max_H30 { get => this.GetMax_H30(); }
        private double GetMax_H30()
        {
            return UnbilledUnmeteredConsumption_H32 == 0 ? 0 : UnbilledUnmeteredConsumption_H32 * (1 + UnbilledUnmeteredConsumptionErrorMargin_J25);
        }


    }
}