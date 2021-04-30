using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class MatrixOneInModel : ICloneable
    {
        public int SelectedOption { get; set; }

        public double C11 { get; set; }
        public double C12 { get; set; }
        public double C13 { get; set; }
        public double C14 { get; set; }
        public double C21 { get; set; }
        public double C22 { get; set; }
        public double C23 { get; set; }
        public double C24 { get; set; }

        public double D21 { get; set; }
        public double D22 { get; set; }
        public double D23 { get; set; }
        public double D24 { get; set; }

        public double E11 { get; set; }
        public double E12 { get; set; }
        public double E13 { get; set; }
        public double E14 { get; set; }
        public double E21 { get; set; }
        public double E22 { get; set; }
        public double E23 { get; set; }
        public double E24 { get; set; }

        public double F11 { get; set; }
        public double F12 { get; set; }
        public double F13 { get; set; }
        public double F14 { get; set; }
        public double F21 { get; set; }
        public double F22 { get; set; }
        public double F23 { get; set; }
        public double F24 { get; set; }

        public double G11 { get; set; }
        public double G12 { get; set; }
        public double G13 { get; set; }
        public double G14 { get; set; }
        public double G21 { get; set; }
        public double G22 { get; set; }
        public double G23 { get; set; }
        public double G24 { get; set; }

        public double H11 { get; set; }
        public double H12 { get; set; }
        public double H13 { get; set; }
        public double H14 { get; set; }
        public double H21 { get; set; }
        public double H22 { get; set; }
        public double H23 { get; set; }
        public double H24 { get; set; }
        
        public object Clone()
        {
            return new MatrixOneInModel()
            {
                SelectedOption = SelectedOption,

                C11 = C11,
                C12 = C12,
                C13 = C13,
                C14 = C14,
                C21 = C21,
                C22 = C22,
                C23 = C23,
                C24 = C24,
                                            
                D21 = D21,
                D22 = D22,
                D23 = D23,
                D24 = D24,
                                            
                E11 = E11,
                E12 = E12,
                E13 = E13,
                E14 = E14,
                E21 = E21,
                E22 = E22,
                E23 = E23,
                E24 = E24,
                                            
                F11 = F11,
                F12 = F12,
                F13 = F13,
                F14 = F14,
                F21 = F21,
                F22 = F22,
                F23 = F23,
                F24 = F24,
                                            
                G11 = G11,
                G12 = G12,
                G13 = G13,
                G14 = G14,
                G21 = G21,
                G22 = G22,
                G23 = G23,
                G24 = G24,
                                            
                H11 = H11,
                H12 = H12,
                H13 = H13,
                H14 = H14,
                H21 = H21,
                H22 = H22,
                H23 = H23,
                H24 = H24,
            };
        }
    } 
}

