using System;
using System.Collections.Generic;
using System.Linq;

namespace WbEasyCalcRepository.Model
{
    public class UnauthorizedConsumptionSheet
    {
        private readonly EasyCalcSheet data;

        public UnauthorizedConsumptionSheet(EasyCalcSheet data)
        {
            this.OthersM3PerDay_J18_J22 = new List<double>();
            this.OthersErrorMargin_F18_F22 = new List<double>();
            this.data = data;
        }
        public int IllegalConnectionsDomesticEstimatedNumber_D6 { get; set; }
        public double IllegalConnectionsDomesticPersonsPerHouse_H6 { get; set; }
        public double IllegalConnectionsDomesticConsumptionLitersPerPersonPerDay_J6 { get; set; }
        public double IllegalConnectionsOthersEstimatedNumber_D10 { get; set; }
        public double IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 { get; set; }
        public double MeterTamperingBypassesEtcEstimatedNumber_D14 { get; set; }
        public double MeterTamperingBypassesEtcConsumptionLitersPerCustomerPerDay_J14 { get; set; }
        public List<double> OthersM3PerDay_J18_J22 { get; set; }
        public List<double> OthersErrorMargin_F18_F22 { get; set; }
        public List<double> Total_L18_L22 { get => GetTotal_L18_L22(); }

        private List<double> GetTotal_L18_L22()
        {
            return OthersM3PerDay_J18_J22.Select(x => x * this.data.StartSheet.PeriodDays_M21).ToList();
        }

        public double BestEstimateTotal_L31 { get => this.GetBestEstimateTotal(); }
        public double ErrorMargin_F24 { get => this.GetErrorMargin_F24(); }
        public double ErrorFactor_P24 { get => this.GetErrorFactor(); }
        public double IllegalConnectionsDomesticConsumptionTotalM3_L6 { get; set; }
        public double IllegalConnectionsDomesticErrorMargin_F6 { get; set; }
        public double IllegalConnectionsOthersErrorMargin_F10 { get; set; }
        public double IllegalConnectionsOthersConsumptionTotalM3_L10 { get; private set; }
        public double MeterTamperingBypassesEtcConsumptionTotalM3_L14 { get; private set; }
        public double MeterTamperingBypassesEtcErrorMargin_F14 { get; set; }

        private double GetErrorMargin_F24()
        {

            double f24 = this.BestEstimateTotal_L31 == 0 ? 0d : this.ErrorFactor_P24 * Constants.StandardDistributionFactor / this.BestEstimateTotal_L31;
            return f24;
        }

        private double GetErrorFactor()
        {
            this.IllegalConnectionsDomesticConsumptionTotalM3_L6 =
                this.IllegalConnectionsDomesticEstimatedNumber_D6
                * this.IllegalConnectionsDomesticPersonsPerHouse_H6
                * this.IllegalConnectionsDomesticConsumptionLitersPerPersonPerDay_J6
                * this.data.StartSheet.PeriodDays_M21
                / 1000;
            double p6 = this.IllegalConnectionsDomesticConsumptionTotalM3_L6
                * this.IllegalConnectionsDomesticErrorMargin_F6
                / Constants.StandardDistributionFactor;
            double q6 = Math.Pow(p6, 2);

            this.IllegalConnectionsOthersConsumptionTotalM3_L10 =
                this.IllegalConnectionsOthersEstimatedNumber_D10
                * this.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10
                * this.data.StartSheet.PeriodDays_M21
                / 1000;
            double p10 = this.IllegalConnectionsOthersConsumptionTotalM3_L10
                * this.IllegalConnectionsOthersErrorMargin_F10
                / Constants.StandardDistributionFactor;
            double q10 = Math.Pow(p10, 2);

            this.MeterTamperingBypassesEtcConsumptionTotalM3_L14 =
                this.MeterTamperingBypassesEtcEstimatedNumber_D14
                * this.MeterTamperingBypassesEtcConsumptionLitersPerCustomerPerDay_J14
                * this.data.StartSheet.PeriodDays_M21
                / 1000;
            double p14 = this.MeterTamperingBypassesEtcConsumptionTotalM3_L14
                * this.MeterTamperingBypassesEtcErrorMargin_F14
                / Constants.StandardDistributionFactor;
            double q14 = Math.Pow(p14, 2);

            double others = this.GetFactorizedOthersSum();

            double q24 = q6 + q10 + q14 + others;

            // double q24 
            double p24 = Math.Sqrt(q24);
            return p24;
        }

        private double GetFactorizedOthersSum()
        {
            List<double> result = new List<double>();
            for (int i = 0; i < this.OthersM3PerDay_J18_J22.Count; i++)
            {
                result.Add(
                    Math.Pow(
                        this.OthersM3PerDay_J18_J22[i]
                        * this.data.StartSheet.PeriodDays_M21
                        * this.OthersErrorMargin_F18_F22[i]
                        / Constants.StandardDistributionFactor,
                    2)
                );
            }

            return result.Sum();
        }

        private double GetBestEstimateTotal()
        {
            double illegalConnectionsDomesticTotalM3_L6 =
                this.IllegalConnectionsDomesticEstimatedNumber_D6
                * this.IllegalConnectionsDomesticPersonsPerHouse_H6
                * this.IllegalConnectionsDomesticConsumptionLitersPerPersonPerDay_J6
                * this.data.StartSheet.PeriodDays_M21
                / 1000;
            double illegalConnectionsOthersTotalM3_L10 =
                this.IllegalConnectionsOthersEstimatedNumber_D10
                * this.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10
                * this.data.StartSheet.PeriodDays_M21
                / 1000;
            double meterTamperingBypassesEtcTotalM3_L14 =
                this.MeterTamperingBypassesEtcEstimatedNumber_D14
                * this.MeterTamperingBypassesEtcConsumptionLitersPerCustomerPerDay_J14
                * this.data.StartSheet.PeriodDays_M21
                / 1000;
            double othersM3 = this.OthersM3PerDay_J18_J22.Sum()
                * this.data.StartSheet.PeriodDays_M21;
            return illegalConnectionsDomesticTotalM3_L6
                + illegalConnectionsOthersTotalM3_L10
                + meterTamperingBypassesEtcTotalM3_L14
                + othersM3;
        }

        public double Min_L27 { get => this.GetMin_L27(); }
        private double GetMin_L27()
        {
            return BestEstimateTotal_L31 * (1 - ErrorMargin_F24);
        }
        public double Max_L29 { get => this.GetMax_L29(); }
        private double GetMax_L29()
        {
            return BestEstimateTotal_L31 * (1 + ErrorMargin_F24);
        }
    }
}