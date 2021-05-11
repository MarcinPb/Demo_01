using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class IntermModel : ICloneable
    {
        // Input
        public string Interm_Area_B7 { get; set; }
        public string Interm_Area_B8 { get; set; }
        public string Interm_Area_B9 { get; set; }
        public string Interm_Area_B10 { get; set; }
        public double Interm_Conn_D7 { get; set; }
        public double Interm_Conn_D8 { get; set; }
        public double Interm_Conn_D9 { get; set; }
        public double Interm_Conn_D10 { get; set; }
        public double Interm_Days_F7 { get; set; }
        public double Interm_Days_F8 { get; set; }
        public double Interm_Days_F9 { get; set; }
        public double Interm_Days_F10 { get; set; }
        public double Interm_Hour_H7 { get; set; }
        public double Interm_Hour_H8 { get; set; }
        public double Interm_Hour_H9 { get; set; }
        public double Interm_Hour_H10 { get; set; }
        public double Interm_ErrorMarg_H26 { get; set; }
        // Output
        public double Interm_Min_H29 { get; set; }
        public double Interm_Max_H31 { get; set; }
        public double Interm_BestEstimate_H33 { get; set; }

        public object Clone()
        {
            return new IntermModel()
            {
                // Input
                Interm_Area_B7 = Interm_Area_B7,
                Interm_Area_B8 = Interm_Area_B8,
                Interm_Area_B9 = Interm_Area_B9,
                Interm_Area_B10 = Interm_Area_B10,
                Interm_Conn_D7 = Interm_Conn_D7,
                Interm_Conn_D8 = Interm_Conn_D8,
                Interm_Conn_D9 = Interm_Conn_D9,
                Interm_Conn_D10 = Interm_Conn_D10,
                Interm_Days_F7 = Interm_Days_F7,
                Interm_Days_F8 = Interm_Days_F8,
                Interm_Days_F9 = Interm_Days_F9,
                Interm_Days_F10 = Interm_Days_F10,
                Interm_Hour_H7 = Interm_Hour_H7,
                Interm_Hour_H8 = Interm_Hour_H8,
                Interm_Hour_H9 = Interm_Hour_H9,
                Interm_Hour_H10 = Interm_Hour_H10,
                Interm_ErrorMarg_H26 = Interm_ErrorMarg_H26,
                // Output
                Interm_Min_H29 = Interm_Min_H29,
                Interm_Max_H31 = Interm_Max_H31,
                Interm_BestEstimate_H33 = Interm_BestEstimate_H33,
            };
        }
    }
}
