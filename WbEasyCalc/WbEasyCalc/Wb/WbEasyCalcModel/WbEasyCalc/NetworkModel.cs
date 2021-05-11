using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class NetworkModel : ICloneable
    {
        // Input
        public string Network_Desc_B7 { get; set; }
        public string Network_Desc_B8 { get; set; }
        public string Network_Desc_B9 { get; set; }
        public string Network_Desc_B10 { get; set; }
        public double Network_DistributionAndTransmissionMains_D7 { get; set; }
        public double Network_DistributionAndTransmissionMains_D8 { get; set; }
        public double Network_DistributionAndTransmissionMains_D9 { get; set; }
        public double Network_DistributionAndTransmissionMains_D10 { get; set; }
        public double Network_PossibleUnd_D30 { get; set; }
        public double Network_ErrorMargin_D35 { get; set; }
        public double Network_NoCustomers_H7 { get; set; }
        public double Network_ErrorMargin_J7 { get; set; }
        public double Network_NoOfConnOfRegCustomers_H10 { get; set; }
        public double Network_ErrorMargin_J10 { get; set; }
        public double Network_NoOfInactAccountsWSvcConns_H18 { get; set; }
        public double Network_ErrorMargin_J18 { get; set; }
        public double Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 { get; set; }
        public double Network_ErrorMargin_J32 { get; set; }
        // Output
        public double Network_Total_D28 { get; set; }
        public double Network_Total_D32 { get; set; }
        public double Network_Min_D39 { get; set; }
        public double Network_Max_D41 { get; set; }
        public double Network_BestEstimate_D43 { get; set; }
        public double Network_Number_H21 { get; set; }
        public double Network_ErrorMarg_J21 { get; set; }
        public double Network_ErrorMarg_J24 { get; set; }
        public double Network_Min_H26 { get; set; }
        public double Network_Max_H28 { get; set; }
        public double Network_BestEstimate_H30 { get; set; }
        public double Network_Number_H39 { get; set; }
        public double Network_ErrorMarg_J39 { get; set; }

        public object Clone()
        {
            return new NetworkModel()
            {
                // Input
                Network_Desc_B7 = Network_Desc_B7,
                Network_Desc_B8 = Network_Desc_B8,
                Network_Desc_B9 = Network_Desc_B9,
                Network_Desc_B10 = Network_Desc_B10,
                Network_DistributionAndTransmissionMains_D7 = Network_DistributionAndTransmissionMains_D7,
                Network_DistributionAndTransmissionMains_D8 = Network_DistributionAndTransmissionMains_D8,
                Network_DistributionAndTransmissionMains_D9 = Network_DistributionAndTransmissionMains_D9,
                Network_DistributionAndTransmissionMains_D10 = Network_DistributionAndTransmissionMains_D10,
                Network_PossibleUnd_D30 = Network_PossibleUnd_D30,
                Network_ErrorMargin_D35 = Network_ErrorMargin_D35,
                Network_NoCustomers_H7 = Network_NoCustomers_H7,
                Network_ErrorMargin_J7 = Network_ErrorMargin_J7,
                Network_NoOfConnOfRegCustomers_H10 = Network_NoOfConnOfRegCustomers_H10,
                Network_NoOfInactAccountsWSvcConns_H18 = Network_NoOfInactAccountsWSvcConns_H18,
                Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 = Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32,
                Network_ErrorMargin_J10 = Network_ErrorMargin_J10,
                Network_ErrorMargin_J18 = Network_ErrorMargin_J18,
                Network_ErrorMargin_J32 = Network_ErrorMargin_J32,
                // Output
                Network_Total_D28 = Network_Total_D28,
                Network_Total_D32 = Network_Total_D32,
                Network_Min_D39 = Network_Min_D39,
                Network_Max_D41 = Network_Max_D41,
                Network_BestEstimate_D43 = Network_BestEstimate_D43,
                Network_Number_H21 = Network_Number_H21,
                Network_ErrorMarg_J21 = Network_ErrorMarg_J21,
                Network_ErrorMarg_J24 = Network_ErrorMarg_J24,
                Network_Min_H26 = Network_Min_H26,
                Network_Max_H28 = Network_Max_H28,
                Network_BestEstimate_H30 = Network_BestEstimate_H30,
                Network_Number_H39 = Network_Number_H39,
                Network_ErrorMarg_J39 = Network_ErrorMarg_J39,
            };
        }
    }
}
