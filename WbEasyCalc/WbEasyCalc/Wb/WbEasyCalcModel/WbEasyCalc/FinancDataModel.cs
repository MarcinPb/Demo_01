using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class FinancDataModel : ICloneable
    {
        // Input
        public double FinancData_G6 { get; set; }
        public string FinancData_K6 { get; set; } //= "PLN";
        public double FinancData_G8 { get; set; }
        public double FinancData_D26 { get; set; }
        public double FinancData_G35 { get; set; }
        // Output
        public double FinancData_G13 { get; set; }
        public double FinancData_G15 { get; set; }
        public double FinancData_G17 { get; set; }
        public double FinancData_G19 { get; set; }
        public double FinancData_G20 { get; set; }
        public double FinancData_G22 { get; set; }
        public double FinancData_D24 { get; set; }
        public double FinancData_G31 { get; set; }
        public string FinancData_K8 { get; set; }
        public string FinancData_K13 { get; set; }
        public string FinancData_K15 { get; set; }
        public string FinancData_K17 { get; set; }
        public string FinancData_K19 { get; set; }
        public string FinancData_K20 { get; set; }
        public string FinancData_K22 { get; set; }
        public string FinancData_K31 { get; set; }
        public string FinancData_K35 { get; set; }

        public object Clone()
        {
            return new FinancDataModel()
            {
                // Input
                FinancData_G6 = FinancData_G6,
                FinancData_K6 = FinancData_K6,
                FinancData_G8 = FinancData_G8,
                FinancData_D26 = FinancData_D26,
                FinancData_G35 = FinancData_G35,
                // Output
                FinancData_G13 = FinancData_G13,
                FinancData_G15 = FinancData_G15,
                FinancData_G17 = FinancData_G17,
                FinancData_G19 = FinancData_G19,
                FinancData_G20 = FinancData_G20,
                FinancData_G22 = FinancData_G22,
                FinancData_D24 = FinancData_D24,
                FinancData_G31 = FinancData_G31,
                FinancData_K8 = FinancData_K8,
                FinancData_K13 = FinancData_K13,
                FinancData_K15 = FinancData_K15,
                FinancData_K17 = FinancData_K17,
                FinancData_K19 = FinancData_K19,
                FinancData_K20 = FinancData_K20,
                FinancData_K22 = FinancData_K22,
                FinancData_K31 = FinancData_K31,
                FinancData_K35 = FinancData_K35,
            };
        }
    }
}
