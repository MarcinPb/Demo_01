using WbEasyCalcRepository.Model;

namespace WbEasyCalcRepository
{
    public class EasyCalcSheet
    {
        public StartSheet StartSheet { get; set; }
        public UnauthorizedConsumptionSheet UnauthorizedConsumptionSheet { get; set; }
        internal BilledConsumptionSheet BilledConsumptionSheet { get; set; }
        public UnbilledConsumptionSheet UnbilledConsumptionSheet { get; set; }
        public MeterErrorsSheet MeterErrorsSheet { get; set; }
        public SystemInputSheet SystemInputSheet { get; set; }
        public NetworkSheet NetworkSheet { get; set; }
        public FinancialDataSheet FinancialDataSheet { get; set; }
        public PressureSheet PressureSheet { get; set; }
        public IntermittentSupplySheet IntermittentSupplySheet { get; set; }
        public WaterBalanceDaySheet WaterBalanceDaySheet { get; set; }
        public WaterBalanceSheet WaterBalanceSheet { get; set; }
        public WaterBalanceYearSheet WaterBalanceYearSheet { get; set; }
        public PiSheet PiSheet { get; set; }

        public MatOneInSheet MatOneInSheet { get; set; }
        public MatOneOutSheet MatOneOutSheet { get; set; }
    }
}
