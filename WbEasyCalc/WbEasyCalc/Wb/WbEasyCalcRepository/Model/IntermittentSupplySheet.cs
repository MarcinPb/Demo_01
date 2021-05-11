using System;
using System.Collections.Generic;
using System.Linq;

namespace WbEasyCalcRepository.Model
{
    public class IntermittentSupplySheet
    {
        public List<double> Interm_Conn_D7_24_List { get; set; }
        public List<double> Interm_Days_F7_24_List { get; set; }
        public List<double> Interm_Hour_H7_24_List { get; set; }

        public double SupplyTimeBestEstimate_H33 { get => GetSupplyTimeBestEstimate_H33(); }
        private double GetSupplyTimeBestEstimate_H33()
        {
            double result = 0;
            if (Interm_Conn_D7_24_List.Sum() > 0)
            {
                for (int i = 0; i < Interm_Conn_D7_24_List.Count; i++)
                {
                    result += Interm_Conn_D7_24_List[i] * Interm_Days_F7_24_List[i] * Interm_Hour_H7_24_List[i];
                }
                result /= Interm_Conn_D7_24_List.Sum();
                result /= 7;
            }
            else
            {
                result = 24;
            }

            return result;
        }

        public double ErrorMargin_H26 { get; set; }

        public double Interm_Min_F29 { get => this.GetInterm_Min_F29(); }
        private double GetInterm_Min_F29()
        {
            return SupplyTimeBestEstimate_H33 * (1 - ErrorMargin_H26);
        }
        public double Interm_Max_F31 { get => this.GetInterm_Max_F31(); }
        private double GetInterm_Max_F31()
        {
            double result;

            if (SupplyTimeBestEstimate_H33 == 24)
            {
                result = 24;
            }
            else
            {
                if (SupplyTimeBestEstimate_H33 * (1 + ErrorMargin_H26) > 24)
                {
                    result = 24;
                }
                else
                {
                    result = SupplyTimeBestEstimate_H33 * (1 + ErrorMargin_H26);
                }
            }
            return result;
        }
    }
}