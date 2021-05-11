using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class UnbilledConsModel : ICloneable
    {
        // Input
        public string UnbilledCons_Desc_D8 { get; set; }
        public string UnbilledCons_Desc_D9 { get; set; }
        public string UnbilledCons_Desc_D10 { get; set; }
        public string UnbilledCons_Desc_D11 { get; set; }
        public string UnbilledCons_Desc_F6 { get; set; }
        public string UnbilledCons_Desc_F7 { get; set; }
        public string UnbilledCons_Desc_F8 { get; set; }
        public string UnbilledCons_Desc_F9 { get; set; }
        public string UnbilledCons_Desc_F10 { get; set; }
        public string UnbilledCons_Desc_F11 { get; set; }
        public double UnbilledCons_MetConsBulkWatSupExpM3_D6 { get; set; }
        public double UnbilledCons_UnbMetConsM3_D8 { get; set; }
        public double UnbilledCons_UnbMetConsM3_D9 { get; set; }
        public double UnbilledCons_UnbMetConsM3_D10 { get; set; }
        public double UnbilledCons_UnbMetConsM3_D11 { get; set; }
        public double UnbilledCons_UnbUnmetConsM3_H6 { get; set; }
        public double UnbilledCons_UnbUnmetConsM3_H7 { get; set; }
        public double UnbilledCons_UnbUnmetConsM3_H8 { get; set; }
        public double UnbilledCons_UnbUnmetConsM3_H9 { get; set; }
        public double UnbilledCons_UnbUnmetConsM3_H10 { get; set; }
        public double UnbilledCons_UnbUnmetConsM3_H11 { get; set; }
        public double UnbilledCons_UnbUnmetConsError_J6 { get; set; }
        public double UnbilledCons_UnbUnmetConsError_J7 { get; set; }
        public double UnbilledCons_UnbUnmetConsError_J8 { get; set; }
        public double UnbilledCons_UnbUnmetConsError_J9 { get; set; }
        public double UnbilledCons_UnbUnmetConsError_J10 { get; set; }
        public double UnbilledCons_UnbUnmetConsError_J11 { get; set; }
        // Output
        public double UnbilledCons_Sum_D32 { get; set; }
        public double UnbilledCons_ErrorMarg_J25 { get; set; }
        public double UnbilledCons_Min_H28 { get; set; }
        public double UnbilledCons_Max_H30 { get; set; }
        public double UnbilledCons_BestEstimate_H32 { get; set; }

        public object Clone()
        {
            return new UnbilledConsModel()
            {
                // Input
                UnbilledCons_Desc_D8 = UnbilledCons_Desc_D8,
                UnbilledCons_Desc_D9 = UnbilledCons_Desc_D9,
                UnbilledCons_Desc_D10 = UnbilledCons_Desc_D10,
                UnbilledCons_Desc_D11 = UnbilledCons_Desc_D11,
                UnbilledCons_Desc_F6 = UnbilledCons_Desc_F6,
                UnbilledCons_Desc_F7 = UnbilledCons_Desc_F7,
                UnbilledCons_Desc_F8 = UnbilledCons_Desc_F8,
                UnbilledCons_Desc_F9 = UnbilledCons_Desc_F9,
                UnbilledCons_Desc_F10 = UnbilledCons_Desc_F10,
                UnbilledCons_Desc_F11 = UnbilledCons_Desc_F11,
                UnbilledCons_MetConsBulkWatSupExpM3_D6 = UnbilledCons_MetConsBulkWatSupExpM3_D6,
                UnbilledCons_UnbMetConsM3_D8 = UnbilledCons_UnbMetConsM3_D8,
                UnbilledCons_UnbMetConsM3_D9 = UnbilledCons_UnbMetConsM3_D9,
                UnbilledCons_UnbMetConsM3_D10 = UnbilledCons_UnbMetConsM3_D10,
                UnbilledCons_UnbMetConsM3_D11 = UnbilledCons_UnbMetConsM3_D11,
                UnbilledCons_UnbUnmetConsM3_H6 = UnbilledCons_UnbUnmetConsM3_H6,
                UnbilledCons_UnbUnmetConsM3_H7 = UnbilledCons_UnbUnmetConsM3_H7,
                UnbilledCons_UnbUnmetConsM3_H8 = UnbilledCons_UnbUnmetConsM3_H8,
                UnbilledCons_UnbUnmetConsM3_H9 = UnbilledCons_UnbUnmetConsM3_H9,
                UnbilledCons_UnbUnmetConsM3_H10 = UnbilledCons_UnbUnmetConsM3_H10,
                UnbilledCons_UnbUnmetConsM3_H11 = UnbilledCons_UnbUnmetConsM3_H11,
                UnbilledCons_UnbUnmetConsError_J6 = UnbilledCons_UnbUnmetConsError_J6,
                UnbilledCons_UnbUnmetConsError_J7 = UnbilledCons_UnbUnmetConsError_J7,
                UnbilledCons_UnbUnmetConsError_J8 = UnbilledCons_UnbUnmetConsError_J8,
                UnbilledCons_UnbUnmetConsError_J9 = UnbilledCons_UnbUnmetConsError_J9,
                UnbilledCons_UnbUnmetConsError_J10 = UnbilledCons_UnbUnmetConsError_J10,
                UnbilledCons_UnbUnmetConsError_J11 = UnbilledCons_UnbUnmetConsError_J11,
                // Output
                UnbilledCons_Sum_D32 = UnbilledCons_Sum_D32,
                UnbilledCons_ErrorMarg_J25 = UnbilledCons_ErrorMarg_J25,
                UnbilledCons_Min_H28 = UnbilledCons_Min_H28,
                UnbilledCons_Max_H30 = UnbilledCons_Max_H30,
                UnbilledCons_BestEstimate_H32 = UnbilledCons_BestEstimate_H32,
            };
        }
    }
}
