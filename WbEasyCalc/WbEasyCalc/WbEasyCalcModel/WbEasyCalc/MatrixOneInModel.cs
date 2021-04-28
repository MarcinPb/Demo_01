using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class MatrixOneInModel : ICloneable
    {
        public double MatOneIn_C11 { get; set; }
        public double MatOneIn_C12 { get; set; }
        public double MatOneIn_C13 { get; set; }
        public double MatOneIn_C14 { get; set; }
        public double MatOneIn_C21 { get; set; }
        public double MatOneIn_C22 { get; set; }
        public double MatOneIn_C23 { get; set; }
        public double MatOneIn_C24 { get; set; }

        public double MatOneIn_D21 { get; set; }
        public double MatOneIn_D22 { get; set; }
        public double MatOneIn_D23 { get; set; }
        public double MatOneIn_D24 { get; set; }

        public double MatOneIn_E11 { get; set; }
        public double MatOneIn_E12 { get; set; }
        public double MatOneIn_E13 { get; set; }
        public double MatOneIn_E14 { get; set; }
        public double MatOneIn_E21 { get; set; }
        public double MatOneIn_E22 { get; set; }
        public double MatOneIn_E23 { get; set; }
        public double MatOneIn_E24 { get; set; }

        public double MatOneIn_F11 { get; set; }
        public double MatOneIn_F12 { get; set; }
        public double MatOneIn_F13 { get; set; }
        public double MatOneIn_F14 { get; set; }
        public double MatOneIn_F21 { get; set; }
        public double MatOneIn_F22 { get; set; }
        public double MatOneIn_F23 { get; set; }
        public double MatOneIn_F24 { get; set; }

        public double MatOneIn_G11 { get; set; }
        public double MatOneIn_G12 { get; set; }
        public double MatOneIn_G13 { get; set; }
        public double MatOneIn_G14 { get; set; }
        public double MatOneIn_G21 { get; set; }
        public double MatOneIn_G22 { get; set; }
        public double MatOneIn_G23 { get; set; }
        public double MatOneIn_G24 { get; set; }

        public double MatOneIn_H11 { get; set; }
        public double MatOneIn_H12 { get; set; }
        public double MatOneIn_H13 { get; set; }
        public double MatOneIn_H14 { get; set; }
        public double MatOneIn_H21 { get; set; }
        public double MatOneIn_H22 { get; set; }
        public double MatOneIn_H23 { get; set; }
        public double MatOneIn_H24 { get; set; }

        public object Clone()
        {
            return new MatrixOneInModel()
            {
                MatOneIn_C11 = MatOneIn_C11,
                MatOneIn_C12 = MatOneIn_C12,
                MatOneIn_C13 = MatOneIn_C13,
                MatOneIn_C14 = MatOneIn_C14,
                MatOneIn_C21 = MatOneIn_C21,
                MatOneIn_C22 = MatOneIn_C22,
                MatOneIn_C23 = MatOneIn_C23,
                MatOneIn_C24 = MatOneIn_C24,
                                            
                MatOneIn_D21 = MatOneIn_D21,
                MatOneIn_D22 = MatOneIn_D22,
                MatOneIn_D23 = MatOneIn_D23,
                MatOneIn_D24 = MatOneIn_D24,
                                            
                MatOneIn_E11 = MatOneIn_E11,
                MatOneIn_E12 = MatOneIn_E12,
                MatOneIn_E13 = MatOneIn_E13,
                MatOneIn_E14 = MatOneIn_E14,
                MatOneIn_E21 = MatOneIn_E21,
                MatOneIn_E22 = MatOneIn_E22,
                MatOneIn_E23 = MatOneIn_E23,
                MatOneIn_E24 = MatOneIn_E24,
                                            
                MatOneIn_F11 = MatOneIn_F11,
                MatOneIn_F12 = MatOneIn_F12,
                MatOneIn_F13 = MatOneIn_F13,
                MatOneIn_F14 = MatOneIn_F14,
                MatOneIn_F21 = MatOneIn_F21,
                MatOneIn_F22 = MatOneIn_F22,
                MatOneIn_F23 = MatOneIn_F23,
                MatOneIn_F24 = MatOneIn_F24,
                                            
                MatOneIn_G11 = MatOneIn_G11,
                MatOneIn_G12 = MatOneIn_G12,
                MatOneIn_G13 = MatOneIn_G13,
                MatOneIn_G14 = MatOneIn_G14,
                MatOneIn_G21 = MatOneIn_G21,
                MatOneIn_G22 = MatOneIn_G22,
                MatOneIn_G23 = MatOneIn_G23,
                MatOneIn_G24 = MatOneIn_G24,
                                            
                MatOneIn_H11 = MatOneIn_H11,
                MatOneIn_H12 = MatOneIn_H12,
                MatOneIn_H13 = MatOneIn_H13,
                MatOneIn_H14 = MatOneIn_H14,
                MatOneIn_H21 = MatOneIn_H21,
                MatOneIn_H22 = MatOneIn_H22,
                MatOneIn_H23 = MatOneIn_H23,
                MatOneIn_H24 = MatOneIn_H24,
            };
        }
    } 
}

