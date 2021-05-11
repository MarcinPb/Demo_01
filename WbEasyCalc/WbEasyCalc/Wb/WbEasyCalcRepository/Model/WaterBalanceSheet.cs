using System;
using System.Linq;

namespace WbEasyCalcRepository.Model
{
    public class WaterBalanceSheet
    {
        protected readonly EasyCalcSheet _data;

        public WaterBalanceSheet(EasyCalcSheet data)
        {
            this._data = data;
        }

        public virtual double PeriodDays_AF1 { get => _data.StartSheet.PeriodDays_M21; }

        public virtual double SystemInputVolume_B19 { get => _data.StartSheet.PeriodDays_M21 > 0 ? _data.SystemInputSheet.SystemInputVolume_D79 : 0d; }
        public virtual double BilledMeteredConsumption_AC4 { get => _data.BilledConsumptionSheet.BilledMeteredConsumption_D6_D25.Sum();}
        public virtual double BilledUnmeteredConsumption_AC9 { get => _data.BilledConsumptionSheet.BilledUnmeteredConsumption_H6_H25.Sum(); }
        public virtual double UnbilledMeteredConsumption_AC14 { get => _data.UnbilledConsumptionSheet.UnbilledMeteredConsumption_D32; }
        public virtual double UnbilledUnmeteredConsumption_AC19 { get => _data.UnbilledConsumptionSheet.UnbilledUnmeteredConsumptionM3_H6_H23.Sum(); }
        public double BilledAuthorizedConsumption_T8 { get => BilledMeteredConsumption_AC4 + BilledUnmeteredConsumption_AC9; }
        public double UnbilledAuthorizedConsumption_T16 { get => UnbilledMeteredConsumption_AC14 + UnbilledUnmeteredConsumption_AC19; }
        public double AuthorizedConsumption_K12 { get => BilledAuthorizedConsumption_T8 + UnbilledAuthorizedConsumption_T16; }
        public double WaterLosses_K29 { get => SystemInputVolume_B19 - AuthorizedConsumption_K12; }
        public virtual double UnauthorizedConsumption_AC24 { get => _data.UnauthorizedConsumptionSheet.BestEstimateTotal_L31; }
        public virtual double CustomerMeterInaccuraciesAndErrorsM3_AC29 { get => _data.MeterErrorsSheet.BestEstimateTotalM3_L49; }
        public double CommercialLosses_T26 { get => UnauthorizedConsumption_AC24 + CustomerMeterInaccuraciesAndErrorsM3_AC29; }
        public double PhysicalLossesM3_T34 { get => WaterLosses_K29 - CommercialLosses_T26; }


        public virtual double UnauthorizedConsumptionErrorMargin_AO25 { get => _data.UnauthorizedConsumptionSheet.ErrorMargin_F24; }
        public double SystemInputVolumeErrorMargin_B21 { get => _data.SystemInputSheet.ErrorMargin_F72; }
        public virtual double AuthorizedConsumptionErrorMargin_K15 { get => AuthorizedConsumption_K12 == 0 ? 0 : _data.UnbilledConsumptionSheet.ErrorFactor_O25 * Constants.StandardDistributionFactor / AuthorizedConsumption_K12; }
        public virtual double CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30 { get => _data.MeterErrorsSheet.ErrorMarginTotal_N42; }
        public virtual double UnbilledAuthorizedConsumptionErrorMargin_T20 { get => UnbilledAuthorizedConsumption_T16 == 0 ? 0 : _data.UnbilledConsumptionSheet.ErrorFactor_O25 * Constants.StandardDistributionFactor / UnbilledAuthorizedConsumption_T16; }
        public virtual double UnbilledUnmeteredConsumptionErrorMargin_AO20 { get => _data.UnbilledConsumptionSheet.UnbilledUnmeteredConsumptionErrorMargin_J25; }
        public virtual double CommercialLossesErrorMargin_T29 { get => GetCommercialLossesErrorMargin_T29(); }
        private double GetCommercialLossesErrorMargin_T29()
        {
            double bj25 = UnauthorizedConsumption_AC24 * UnauthorizedConsumptionErrorMargin_AO25 / Constants.StandardDistributionFactor;
            double bk25 = Math.Pow(bj25, 2);
            double bj30 = CustomerMeterInaccuraciesAndErrorsM3_AC29 * CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30 / Constants.StandardDistributionFactor;
            double bk30 = Math.Pow(bj30, 2);
            double bk32 = bk25 + bk30;
            double bj32 = Math.Sqrt(bk32);

            return CommercialLosses_T26 == 0d ? 0d : bj32 * Constants.StandardDistributionFactor / CommercialLosses_T26;
        }
        public double PhyscialLossesErrorMarginFactor_BL35 { get => this.GetPhyscialLossesErrorMarginFactor_BL35(); }
        private double GetPhyscialLossesErrorMarginFactor_BL35()
        {
            double bm15 = Math.Pow(this.AuthorizedConsumption_K12 * this.AuthorizedConsumptionErrorMargin_K15 / Constants.StandardDistributionFactor, 2);
            double bm25 = Math.Pow(this.SystemInputVolume_B19 * this.SystemInputVolumeErrorMargin_B21 / Constants.StandardDistributionFactor, 2);
            double bm30 = Math.Pow(this.CommercialLosses_T26 * this.CommercialLossesErrorMargin_T29 / Constants.StandardDistributionFactor, 2);
            double bm35 = bm15 + bm25 + bm30;
            double bl35 = Math.Sqrt(bm35);

            return bl35;
        }
        public virtual double PhyscialLossesErrorMargin_AH35 { get => PhysicalLossesM3_T34 == 0 ? 0d : PhyscialLossesErrorMarginFactor_BL35 * Constants.StandardDistributionFactor / PhysicalLossesM3_T34; }
        public virtual double WaterLossesErrorMargin_K31 { get => GetWaterLossesErrorMargin_K31(); }
        private double GetWaterLossesErrorMargin_K31()
        {
            double bl15 = AuthorizedConsumption_K12 * AuthorizedConsumptionErrorMargin_K15 / Constants.StandardDistributionFactor;
            double bl25 = SystemInputVolume_B19 * SystemInputVolumeErrorMargin_B21 / Constants.StandardDistributionFactor;
            double bm15 = Math.Pow(bl15, 2);
            double bm25 = Math.Pow(bl25, 2);
            double bm34 = bm15 + bm25;
            double bl34 = Math.Sqrt(bm34);

            return WaterLosses_K29 == 0d ? 0d : bl34 * Constants.StandardDistributionFactor / WaterLosses_K29;
        }
        public double RevenueWaterM3_AY8 { get => BilledAuthorizedConsumption_T8; }
        public double NonRevenueWaterM3_AY24 { get => SystemInputVolume_B19 - BilledAuthorizedConsumption_T8; }
        public double NonRevenueWaterErrorMarginFactor_BL25 { get => this.GetNonRevenueWaterErrorMarginFactor_BL25(); }
        private double GetNonRevenueWaterErrorMarginFactor_BL25()
        {
            double bl25 = this.SystemInputVolume_B19 * this.SystemInputVolumeErrorMargin_B21 / Constants.StandardDistributionFactor;
            return bl25;
        }
        public virtual double NonRevenueWaterErrorMargin_AY26 { get => NonRevenueWaterM3_AY24 == 0 ? 0d : NonRevenueWaterErrorMarginFactor_BL25 * Constants.StandardDistributionFactor / NonRevenueWaterM3_AY24; }

    }
}
