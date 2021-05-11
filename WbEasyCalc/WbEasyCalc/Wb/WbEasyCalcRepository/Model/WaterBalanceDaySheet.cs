using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcRepository.Model
{
    public class WaterBalanceDaySheet : WaterBalanceSheet
    {
        public WaterBalanceDaySheet(EasyCalcSheet data) : base(data) { }

        public override double SystemInputVolume_B19 { get => _data.StartSheet.PeriodDays_M21 > 0 ? _data.SystemInputSheet.SystemInputVolume_D79 / _data.StartSheet.PeriodDays_M21 : 0d; }
        public override double BilledMeteredConsumption_AC4 { get => _data.StartSheet.PeriodDays_M21 > 0 ? _data.BilledConsumptionSheet.BilledMeteredConsumption_D6_D25.Sum() / _data.StartSheet.PeriodDays_M21 : 0d; }
        public override double BilledUnmeteredConsumption_AC9 { get => _data.StartSheet.PeriodDays_M21 > 0 ? _data.BilledConsumptionSheet.BilledUnmeteredConsumption_H6_H25.Sum() / _data.StartSheet.PeriodDays_M21 : 0d; }
        public override double UnbilledMeteredConsumption_AC14 { get => _data.StartSheet.PeriodDays_M21 > 0 ? _data.UnbilledConsumptionSheet.UnbilledMeteredConsumption_D32 / _data.StartSheet.PeriodDays_M21 : 0d; }
        public override double UnbilledUnmeteredConsumption_AC19 { get => _data.StartSheet.PeriodDays_M21 > 0 ? _data.UnbilledConsumptionSheet.UnbilledUnmeteredConsumptionM3_H6_H23.Sum() / _data.StartSheet.PeriodDays_M21 : 0d; }
        public override double UnauthorizedConsumption_AC24 { get => _data.StartSheet.PeriodDays_M21 > 0 ? _data.UnauthorizedConsumptionSheet.BestEstimateTotal_L31 / _data.StartSheet.PeriodDays_M21 : 0d; }
        public override double CustomerMeterInaccuraciesAndErrorsM3_AC29 { get => _data.StartSheet.PeriodDays_M21 > 0 ? _data.MeterErrorsSheet.BestEstimateTotalM3_L49 / _data.StartSheet.PeriodDays_M21 : 0d; }
        public override double AuthorizedConsumptionErrorMargin_K15 { get => AuthorizedConsumption_K12 == 0 ? 0 : (_data.UnbilledConsumptionSheet.ErrorFactor_O25 / _data.StartSheet.PeriodDays_M21) * (Constants.StandardDistributionFactor / AuthorizedConsumption_K12); }
        public override double UnbilledAuthorizedConsumptionErrorMargin_T20 { get => UnbilledAuthorizedConsumption_T16 == 0 ? 0 : (_data.UnbilledConsumptionSheet.ErrorFactor_O25 / _data.StartSheet.PeriodDays_M21) * (Constants.StandardDistributionFactor / UnbilledAuthorizedConsumption_T16); }
    }
}
