using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class PressureModel : ICloneable
    {
        // Input
        public string Prs_Area_B7 { get; set; }
        public double Prs_ApproxNoOfConn_D7 { get; set; }
        public double Prs_DailyAvgPrsM_F7 { get; set; }
        public string Prs_Area_B8 { get; set; }
        public double Prs_ApproxNoOfConn_D8 { get; set; }
        public double Prs_DailyAvgPrsM_F8 { get; set; }
        public string Prs_Area_B9 { get; set; }
        public double Prs_ApproxNoOfConn_D9 { get; set; }
        public double Prs_DailyAvgPrsM_F9 { get; set; }
        public string Prs_Area_B10 { get; set; }
        public double Prs_ApproxNoOfConn_D10 { get; set; }
        public double Prs_DailyAvgPrsM_F10 { get; set; }
        public double Prs_ErrorMarg_F26 { get; set; }
        // Output
        public double Prs_Min_F29 { get; set; }
        public double Prs_Max_F31 { get; set; }
        public double Prs_BestEstimate_F33 { get; set; }

        public object Clone()
        {
            return new PressureModel()
            {
                // Input
                Prs_Area_B7 = Prs_Area_B7,
                Prs_Area_B8 = Prs_Area_B8,
                Prs_Area_B9 = Prs_Area_B9,
                Prs_Area_B10 = Prs_Area_B10,
                Prs_ApproxNoOfConn_D7 = Prs_ApproxNoOfConn_D7,
                Prs_DailyAvgPrsM_F7 = Prs_DailyAvgPrsM_F7,
                Prs_ApproxNoOfConn_D8 = Prs_ApproxNoOfConn_D8,
                Prs_DailyAvgPrsM_F8 = Prs_DailyAvgPrsM_F8,
                Prs_ApproxNoOfConn_D9 = Prs_ApproxNoOfConn_D9,
                Prs_DailyAvgPrsM_F9 = Prs_DailyAvgPrsM_F9,
                Prs_ApproxNoOfConn_D10 = Prs_ApproxNoOfConn_D10,
                Prs_DailyAvgPrsM_F10 = Prs_DailyAvgPrsM_F10,
                Prs_ErrorMarg_F26 = Prs_ErrorMarg_F26,
                // Output
                Prs_Min_F29 = Prs_Min_F29,
                Prs_Max_F31 = Prs_Max_F31,
                Prs_BestEstimate_F33 = Prs_BestEstimate_F33,
            };
        }
    }
}
