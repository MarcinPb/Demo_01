using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcRepository.Model
{
    public class FinancialDataSheet
    {
        private readonly EasyCalcSheet _data;

        public FinancialDataSheet(EasyCalcSheet data)
        {
            _data = data;
        }

        public double FinancData_G6 { get; set; }
        public double FinancData_G8 { get; set; }
        public double FinancData_D26 { get; set; }
        public double FinancData_G35 { get; set; }

        public double FinancData_G13 { get => _data.WaterBalanceYearSheet.UnbilledMeteredConsumption_AC14 * FinancData_G6; }
        public double FinancData_G15 { get => _data.WaterBalanceYearSheet.UnbilledUnmeteredConsumption_AC19 * FinancData_G6; }
        public double FinancData_G17 { get => _data.WaterBalanceYearSheet.CommercialLosses_T26 * FinancData_G6; }
        public double FinancData_G19 { get => _data.WaterBalanceYearSheet.CustomerMeterInaccuraciesAndErrorsM3_AC29 * FinancData_G6; }
        public double FinancData_G20 { get => _data.WaterBalanceYearSheet.UnauthorizedConsumption_AC24 * FinancData_G6; }
        public double FinancData_D24 { get => _data.WaterBalanceDaySheet.PhysicalLossesM3_T34; }
        public double FinancData_G22 { get => GetFinancData_G22(); }
        private double GetFinancData_G22()
        {
            if (_data.StartSheet.PeriodDays_M21==366)
            {
                return ((FinancData_D24-FinancData_D26) * FinancData_G8 + (FinancData_D26*FinancData_G6)) * 366;
            }
            else
            {
                return ((FinancData_D24-FinancData_D26) * FinancData_G8 + (FinancData_D26*FinancData_G6)) * 365;
            };
        }

        public double FinancData_G31 { get => FinancData_G13 + FinancData_G15 + FinancData_G17 + FinancData_G22; }


        public string FinancData_K6 { get; set; }
        public string FinancData_K8 { get => FinancData_K6; }
        public string FinancData_K13 { get => FinancData_K6; }
        public string FinancData_K15 { get => FinancData_K6; }
        public string FinancData_K17 { get => FinancData_K6; }
        public string FinancData_K19 { get => FinancData_K6; }
        public string FinancData_K20 { get => FinancData_K6; }
        public string FinancData_K22 { get => FinancData_K6; }
        public string FinancData_K31 { get => FinancData_K6; }
        public string FinancData_K35 { get => FinancData_K6; }


    }
}
