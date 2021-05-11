using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class WaterBalanceModel : ICloneable
    {
        public double PeriodDays_AF1 { get; set; }
        public double SystemInputVolume_B19 { get; set; }
        public double SystemInputVolumeErrorMargin_B21 { get; set; }

        public double AuthorizedConsumption_K12 { get; set; }
        public double AuthorizedConsumptionErrorMargin_K15 { get; set; }
        public double WaterLosses_K29 { get; set; }
        public double WaterLossesErrorMargin_K31 { get; set; }

        public double BilledAuthorizedConsumption_T8 { get; set; }
        public double UnbilledAuthorizedConsumption_T16 { get; set; }
        public double UnbilledAuthorizedConsumptionErrorMargin_T20 { get; set; }
        public double CommercialLosses_T26 { get; set; }
        public double CommercialLossesErrorMargin_T29 { get; set; }
        public double PhysicalLossesM3_T34 { get; set; }
        public double PhyscialLossesErrorMargin_AH35 { get; set; }

        public double BilledMeteredConsumption_AC4 { get; set; }
        public double BilledUnmeteredConsumption_AC9 { get; set; }
        public double UnbilledMeteredConsumption_AC14 { get; set; }

        public double UnbilledUnmeteredConsumption_AC19 { get; set; }
        public double UnbilledUnmeteredConsumptionErrorMargin_AO20 { get; set; }
        public double UnauthorizedConsumption_AC24 { get; set; }
        public double UnauthorizedConsumptionErrorMargin_AO25 { get; set; }
        public double CustomerMeterInaccuraciesAndErrorsM3_AC29 { get; set; }
        public double CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30 { get; set; }

        public double RevenueWaterM3_AY8 { get; set; }
        public double NonRevenueWaterM3_AY24 { get; set; }
        public double NonRevenueWaterErrorMargin_AY26 { get; set; }  

        public object Clone()
        {
            return new WaterBalanceModel()
            {
                PeriodDays_AF1 = PeriodDays_AF1,
                SystemInputVolume_B19 = SystemInputVolume_B19,
                SystemInputVolumeErrorMargin_B21 = SystemInputVolumeErrorMargin_B21,
                AuthorizedConsumption_K12 = AuthorizedConsumption_K12,
                AuthorizedConsumptionErrorMargin_K15 = AuthorizedConsumptionErrorMargin_K15,
                WaterLosses_K29 = WaterLosses_K29,
                WaterLossesErrorMargin_K31 = WaterLossesErrorMargin_K31,
                BilledAuthorizedConsumption_T8 = BilledAuthorizedConsumption_T8,
                UnbilledAuthorizedConsumption_T16 = UnbilledAuthorizedConsumption_T16,
                UnbilledAuthorizedConsumptionErrorMargin_T20 = UnbilledAuthorizedConsumptionErrorMargin_T20,
                CommercialLosses_T26 = CommercialLosses_T26,
                CommercialLossesErrorMargin_T29 = CommercialLossesErrorMargin_T29,
                PhysicalLossesM3_T34 = PhysicalLossesM3_T34,
                PhyscialLossesErrorMargin_AH35 = PhyscialLossesErrorMargin_AH35,
                BilledMeteredConsumption_AC4 = BilledMeteredConsumption_AC4,
                BilledUnmeteredConsumption_AC9 = BilledUnmeteredConsumption_AC9,
                UnbilledMeteredConsumption_AC14 = UnbilledMeteredConsumption_AC14,
                UnbilledUnmeteredConsumption_AC19 = UnbilledUnmeteredConsumption_AC19,
                UnbilledUnmeteredConsumptionErrorMargin_AO20 = UnbilledUnmeteredConsumptionErrorMargin_AO20,
                UnauthorizedConsumption_AC24 = UnauthorizedConsumption_AC24,
                UnauthorizedConsumptionErrorMargin_AO25 = UnauthorizedConsumptionErrorMargin_AO25,
                CustomerMeterInaccuraciesAndErrorsM3_AC29 = CustomerMeterInaccuraciesAndErrorsM3_AC29,
                CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30 = CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30,
                RevenueWaterM3_AY8 = RevenueWaterM3_AY8,
                NonRevenueWaterM3_AY24 = NonRevenueWaterM3_AY24,
                NonRevenueWaterErrorMargin_AY26 = NonRevenueWaterErrorMargin_AY26,
            };
        }
    }
}
