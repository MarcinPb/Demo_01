using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class UnauthConsModel : ICloneable
    {
        // Input
        public string UnauthCons_Desc_B18 { get; set; }
        public string UnauthCons_Desc_B19 { get; set; }
        public string UnauthCons_Desc_B20 { get; set; }
        public string UnauthCons_Desc_B21 { get; set; }
        public int UnauthCons_IllegalConnDomEstNo_D6 { get; set; }                      //        
        public double UnauthCons_IllegalConnDomPersPerHouse_H6 { get; set; }
        public double UnauthCons_IllegalConnDomConsLitPerPersDay_J6 { get; set; }
        public double UnauthCons_IllegalConnDomErrorMargin_F6 { get; set; }
        public double UnauthCons_IllegalConnOthersErrorMargin_F10 { get; set; }
        public double IllegalConnectionsOthersEstimatedNumber_D10 { get; set; }
        public double IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 { get; set; }
        public double UnauthCons_MeterTampBypEtcEstNo_D14 { get; set; }
        public double UnauthCons_MeterTampBypEtcErrorMargin_F14 { get; set; }
        public double UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 { get; set; }
        public double UnauthCons_OthersErrorMargin_F18 { get; set; }
        public double UnauthCons_OthersErrorMargin_F19 { get; set; }
        public double UnauthCons_OthersErrorMargin_F20 { get; set; }
        public double UnauthCons_OthersErrorMargin_F21 { get; set; }
        public double UnauthCons_OthersM3PerDay_J18 { get; set; }
        public double UnauthCons_OthersM3PerDay_J19 { get; set; }
        public double UnauthCons_OthersM3PerDay_J20 { get; set; }
        public double UnauthCons_OthersM3PerDay_J21 { get; set; }
        // Output
        public double UnauthCons_Total_L6 { get; set; }
        public double UnauthCons_Total_L10 { get; set; }
        public double UnauthCons_Total_L14 { get; set; }
        public double UnauthCons_Total_L18 { get; set; }
        public double UnauthCons_Total_L19 { get; set; }
        public double UnauthCons_Total_L20 { get; set; }
        public double UnauthCons_Total_L21 { get; set; }
        public double UnauthCons_ErrorMarg_F24 { get; set; }
        public double UnauthCons_Min_L27 { get; set; }
        public double UnauthCons_Max_L29 { get; set; }
        public double UnauthCons_BestEstimate_L31 { get; set; }

        public object Clone()
        {
            return new UnauthConsModel()
            {
                // Input
                UnauthCons_Desc_B18 = UnauthCons_Desc_B18,
                UnauthCons_Desc_B19 = UnauthCons_Desc_B19,
                UnauthCons_Desc_B20 = UnauthCons_Desc_B20,
                UnauthCons_Desc_B21 = UnauthCons_Desc_B21,
                UnauthCons_OthersErrorMargin_F18 = UnauthCons_OthersErrorMargin_F18,
                UnauthCons_OthersErrorMargin_F19 = UnauthCons_OthersErrorMargin_F19,
                UnauthCons_OthersErrorMargin_F20 = UnauthCons_OthersErrorMargin_F20,
                UnauthCons_OthersErrorMargin_F21 = UnauthCons_OthersErrorMargin_F21,
                UnauthCons_OthersM3PerDay_J18 = UnauthCons_OthersM3PerDay_J18,
                UnauthCons_OthersM3PerDay_J19 = UnauthCons_OthersM3PerDay_J19,
                UnauthCons_OthersM3PerDay_J20 = UnauthCons_OthersM3PerDay_J20,
                UnauthCons_OthersM3PerDay_J21 = UnauthCons_OthersM3PerDay_J21,
                UnauthCons_IllegalConnDomEstNo_D6 = UnauthCons_IllegalConnDomEstNo_D6,
                UnauthCons_IllegalConnDomPersPerHouse_H6 = UnauthCons_IllegalConnDomPersPerHouse_H6,
                UnauthCons_IllegalConnDomConsLitPerPersDay_J6 = UnauthCons_IllegalConnDomConsLitPerPersDay_J6,
                UnauthCons_IllegalConnDomErrorMargin_F6 = UnauthCons_IllegalConnDomErrorMargin_F6,
                UnauthCons_IllegalConnOthersErrorMargin_F10 = UnauthCons_IllegalConnOthersErrorMargin_F10,
                IllegalConnectionsOthersEstimatedNumber_D10 = IllegalConnectionsOthersEstimatedNumber_D10,
                IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10,
                UnauthCons_MeterTampBypEtcEstNo_D14 = UnauthCons_MeterTampBypEtcEstNo_D14,
                UnauthCons_MeterTampBypEtcErrorMargin_F14 = UnauthCons_MeterTampBypEtcErrorMargin_F14,
                UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 = UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14,
                // Output
                UnauthCons_Total_L6 = UnauthCons_Total_L6,
                UnauthCons_Total_L10 = UnauthCons_Total_L10,
                UnauthCons_Total_L14 = UnauthCons_Total_L14,
                UnauthCons_Total_L18 = UnauthCons_Total_L18,
                UnauthCons_Total_L19 = UnauthCons_Total_L19,
                UnauthCons_Total_L20 = UnauthCons_Total_L20,
                UnauthCons_Total_L21 = UnauthCons_Total_L21,
                UnauthCons_ErrorMarg_F24 = UnauthCons_ErrorMarg_F24,
                UnauthCons_Min_L27 = UnauthCons_Min_L27,
                UnauthCons_Max_L29 = UnauthCons_Max_L29,
                UnauthCons_BestEstimate_L31 = UnauthCons_BestEstimate_L31,
            };
        }
    }
}
