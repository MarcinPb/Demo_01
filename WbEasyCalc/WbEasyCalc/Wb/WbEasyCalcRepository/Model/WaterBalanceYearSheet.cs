using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcRepository.Model
{
    public class WaterBalanceYearSheet : WaterBalanceSheet
    {
        const int days364 = 364; 
        const int days365 = 365; 
        public WaterBalanceYearSheet(EasyCalcSheet data) : base(data) { }

        public override double SystemInputVolume_B19 { get => _data.StartSheet.PeriodDays_M21 > days364 ? _data.SystemInputSheet.SystemInputVolume_D79 : _data.WaterBalanceDaySheet.SystemInputVolume_B19 * days365; }
        public override double BilledMeteredConsumption_AC4 { get => _data.StartSheet.PeriodDays_M21 > days364 ? _data.BilledConsumptionSheet.BilledMeteredConsumption_D6_D25.Sum() : _data.WaterBalanceDaySheet.BilledMeteredConsumption_AC4 * days365; }
        public override double BilledUnmeteredConsumption_AC9 { get => _data.StartSheet.PeriodDays_M21 > days364 ? _data.BilledConsumptionSheet.BilledUnmeteredConsumption_H6_H25.Sum() : _data.WaterBalanceDaySheet.BilledUnmeteredConsumption_AC9 * days365; }
        public override double UnbilledMeteredConsumption_AC14 { get => _data.StartSheet.PeriodDays_M21 > days364 ? _data.UnbilledConsumptionSheet.UnbilledMeteredConsumption_D32 : _data.WaterBalanceDaySheet.UnbilledMeteredConsumption_AC14 * days365; }
        public override double UnbilledUnmeteredConsumption_AC19 { get => _data.StartSheet.PeriodDays_M21 > days364 ? _data.UnbilledConsumptionSheet.UnbilledUnmeteredConsumptionM3_H6_H23.Sum() : _data.WaterBalanceDaySheet.UnbilledUnmeteredConsumption_AC19 * days365; }
        public override double UnauthorizedConsumption_AC24 { get => _data.StartSheet.PeriodDays_M21 > days364 ? _data.UnauthorizedConsumptionSheet.BestEstimateTotal_L31 : _data.WaterBalanceDaySheet.UnauthorizedConsumption_AC24 * days365; }
        public override double CustomerMeterInaccuraciesAndErrorsM3_AC29 { get => _data.StartSheet.PeriodDays_M21 > days364 ? _data.MeterErrorsSheet.BestEstimateTotalM3_L49 : _data.WaterBalanceDaySheet.CustomerMeterInaccuraciesAndErrorsM3_AC29 * days365; }
        public override double AuthorizedConsumptionErrorMargin_K15 { get => _data.WaterBalanceDaySheet.AuthorizedConsumptionErrorMargin_K15; }
        public override double UnbilledAuthorizedConsumptionErrorMargin_T20 { get => _data.WaterBalanceDaySheet.UnbilledAuthorizedConsumptionErrorMargin_T20; }


        public override double WaterLossesErrorMargin_K31 { get => _data.WaterBalanceDaySheet.WaterLossesErrorMargin_K31; }
        public override double CommercialLossesErrorMargin_T29 { get => _data.WaterBalanceDaySheet.CommercialLossesErrorMargin_T29; }

        public override double UnbilledUnmeteredConsumptionErrorMargin_AO20 { get => _data.WaterBalanceDaySheet.UnbilledUnmeteredConsumptionErrorMargin_AO20; }
        public override double UnauthorizedConsumptionErrorMargin_AO25 { get => _data.WaterBalanceDaySheet.UnauthorizedConsumptionErrorMargin_AO25; }
        public override double CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30 { get => _data.WaterBalanceDaySheet.CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30; }

        public override double PhyscialLossesErrorMargin_AH35 { get => _data.WaterBalanceDaySheet.PhyscialLossesErrorMargin_AH35; }
        public override double NonRevenueWaterErrorMargin_AY26 { get => _data.WaterBalanceDaySheet.NonRevenueWaterErrorMargin_AY26; }

    }
}
