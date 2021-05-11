using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class SysInputModel : ICloneable
    {
        // Input
        public string SysInput_Desc_B6 { get; set; }
        public string SysInput_Desc_B7 { get; set; }
        public string SysInput_Desc_B8 { get; set; }
        public string SysInput_Desc_B9 { get; set; }
        public double SysInput_SystemInputVolumeM3_D6 { get; set; }
        public double SysInput_SystemInputVolumeError_F6 { get; set; }
        public double SysInput_SystemInputVolumeM3_D7 { get; set; }
        public double SysInput_SystemInputVolumeError_F7 { get; set; }
        public double SysInput_SystemInputVolumeM3_D8 { get; set; }
        public double SysInput_SystemInputVolumeError_F8 { get; set; }
        public double SysInput_SystemInputVolumeM3_D9 { get; set; }
        public double SysInput_SystemInputVolumeError_F9 { get; set; }

        // Output
        public double SysInput_ErrorMarg_F72 { get; set; }
        public double SysInput_Min_D75 { get; set; }
        public double SysInput_Max_D77 { get; set; }
        public double SysInput_BestEstimate_D79 { get; set; }

        public object Clone()
        {
            return new SysInputModel()
            {
                // Input
                SysInput_Desc_B6 = SysInput_Desc_B6,
                SysInput_Desc_B7 = SysInput_Desc_B7,
                SysInput_Desc_B8 = SysInput_Desc_B8,
                SysInput_Desc_B9 = SysInput_Desc_B9,
                SysInput_SystemInputVolumeM3_D6 = SysInput_SystemInputVolumeM3_D6,
                SysInput_SystemInputVolumeError_F6 = SysInput_SystemInputVolumeError_F6,
                SysInput_SystemInputVolumeM3_D7 = SysInput_SystemInputVolumeM3_D7,
                SysInput_SystemInputVolumeError_F7 = SysInput_SystemInputVolumeError_F7,
                SysInput_SystemInputVolumeM3_D8 = SysInput_SystemInputVolumeM3_D8,
                SysInput_SystemInputVolumeError_F8 = SysInput_SystemInputVolumeError_F8,
                SysInput_SystemInputVolumeM3_D9 = SysInput_SystemInputVolumeM3_D9,
                SysInput_SystemInputVolumeError_F9 = SysInput_SystemInputVolumeError_F9,
                // Output
                SysInput_ErrorMarg_F72 = SysInput_ErrorMarg_F72,
                SysInput_Min_D75 = SysInput_Min_D75,
                SysInput_Max_D77 = SysInput_Max_D77,
                SysInput_BestEstimate_D79 = SysInput_BestEstimate_D79,
            };
        }
    }
}
