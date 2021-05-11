using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class BilledConsModel : ICloneable
    {
        // Input
        public double BilledCons_BilledMetConsBulkWatSupExpM3_D6 { get; set; }
        public double BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 { get; set; }
        public string BilledCons_Desc_B8 { get; set; }
        public string BilledCons_Desc_B9 { get; set; }
        public string BilledCons_Desc_B10 { get; set; }
        public string BilledCons_Desc_B11 { get; set; }
        public string BilledCons_Desc_F8 { get; set; }
        public string BilledCons_Desc_F9 { get; set; }
        public string BilledCons_Desc_F10 { get; set; }
        public string BilledCons_Desc_F11 { get; set; }
        public double BilledCons_UnbMetConsM3_D8 { get; set; }
        public double BilledCons_UnbMetConsM3_D9 { get; set; }
        public double BilledCons_UnbMetConsM3_D10 { get; set; }
        public double BilledCons_UnbMetConsM3_D11 { get; set; }
        public double BilledCons_UnbUnmetConsM3_H8 { get; set; }
        public double BilledCons_UnbUnmetConsM3_H9 { get; set; }
        public double BilledCons_UnbUnmetConsM3_H10 { get; set; }
        public double BilledCons_UnbUnmetConsM3_H11 { get; set; }
        // Output
        public double BilledCons_Sum_D28 { get; set; }
        public double BilledCons_Sum_H28 { get; set; }


        public object Clone()
        {
            return new BilledConsModel()
            {
                // Input
                BilledCons_Desc_B8 = BilledCons_Desc_B8,
                BilledCons_Desc_B9 = BilledCons_Desc_B9,
                BilledCons_Desc_B10 = BilledCons_Desc_B10,
                BilledCons_Desc_B11 = BilledCons_Desc_B11,
                BilledCons_Desc_F8 = BilledCons_Desc_F8,
                BilledCons_Desc_F9 = BilledCons_Desc_F9,
                BilledCons_Desc_F10 = BilledCons_Desc_F10,
                BilledCons_Desc_F11 = BilledCons_Desc_F11,
                BilledCons_BilledMetConsBulkWatSupExpM3_D6 = BilledCons_BilledMetConsBulkWatSupExpM3_D6,
                BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 = BilledCons_BilledUnmetConsBulkWatSupExpM3_H6,
                BilledCons_UnbMetConsM3_D8 = BilledCons_UnbMetConsM3_D8,
                BilledCons_UnbMetConsM3_D9 = BilledCons_UnbMetConsM3_D9,
                BilledCons_UnbMetConsM3_D10 = BilledCons_UnbMetConsM3_D10,
                BilledCons_UnbMetConsM3_D11 = BilledCons_UnbMetConsM3_D11,
                BilledCons_UnbUnmetConsM3_H8 = BilledCons_UnbUnmetConsM3_H8,
                BilledCons_UnbUnmetConsM3_H9 = BilledCons_UnbUnmetConsM3_H9,
                BilledCons_UnbUnmetConsM3_H10 = BilledCons_UnbUnmetConsM3_H10,
                BilledCons_UnbUnmetConsM3_H11 = BilledCons_UnbUnmetConsM3_H11,
                // Output
                BilledCons_Sum_D28 = BilledCons_Sum_D28,
                BilledCons_Sum_H28 = BilledCons_Sum_H28,
            };
        }
    }
}
