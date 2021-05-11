using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class MatrixOutModel : ICloneable
    {
        public string C11 { get; set; }
        public string C12 { get; set; }
        public string C13 { get; set; }
        public string C14 { get; set; }
        public string C15 { get; set; }
        public string C21 { get; set; }
        public string C22 { get; set; }
        public string C23 { get; set; }
        public string C24 { get; set; }
        public string C25 { get; set; }
        public string D21 { get; set; }
        public string D22 { get; set; }
        public string D23 { get; set; }
        public string D24 { get; set; }
        public string D25 { get; set; }
        public string E11 { get; set; }
        public string E12 { get; set; }
        public string E13 { get; set; }
        public string E14 { get; set; }
        public string E15 { get; set; }
        public string E21 { get; set; }
        public string E22 { get; set; }
        public string E23 { get; set; }
        public string E24 { get; set; }
        public string E25 { get; set; }
        public string F11 { get; set; }
        public string F12 { get; set; }
        public string F13 { get; set; }
        public string F14 { get; set; }
        public string F15 { get; set; }
        public string F21 { get; set; }
        public string F22 { get; set; }
        public string F23 { get; set; }
        public string F24 { get; set; }
        public string F25 { get; set; }
        public string G11 { get; set; }
        public string G12 { get; set; }
        public string G13 { get; set; }
        public string G14 { get; set; }
        public string G15 { get; set; }
        public string G21 { get; set; }
        public string G22 { get; set; }
        public string G23 { get; set; }
        public string G24 { get; set; }
        public string G25 { get; set; }
        public string H11 { get; set; }
        public string H12 { get; set; }
        public string H13 { get; set; }
        public string H14 { get; set; }
        public string H15 { get; set; }
        public string H21 { get; set; }
        public string H22 { get; set; }
        public string H23 { get; set; }
        public string H24 { get; set; }
        public string H25 { get; set; }

        public object Clone()
        {
            return new MatrixOutModel()
            {
                C11 = C11,
                C12 = C12,
                C13 = C13,
                C14 = C14,
                C15 = C15,
                C21 = C21,
                C22 = C22,
                C23 = C23,
                C24 = C24,
                C25 = C25,
                D21 = D21,
                D22 = D22,
                D23 = D23,
                D24 = D24,
                D25 = D25,
                E11 = E11,
                E12 = E12,
                E13 = E13,
                E14 = E14,
                E15 = E15,
                E21 = E21,
                E22 = E22,
                E23 = E23,
                E24 = E24,
                E25 = E25,
                F11 = F11,
                F12 = F12,
                F13 = F13,
                F14 = F14,
                F15 = F15,
                F21 = F21,
                F22 = F22,
                F23 = F23,
                F24 = F24,
                F25 = F25,
                G11 = G11,
                G12 = G12,
                G13 = G13,
                G14 = G14,
                G15 = G15,
                G21 = G21,
                G22 = G22,
                G23 = G23,
                G24 = G24,
                G25 = G25,
                H11 = H11,
                H12 = H12,
                H13 = H13,
                H14 = H14,
                H15 = H15,
                H21 = H21,
                H22 = H22,
                H23 = H23,
                H24 = H24,
                H25 = H25,
            };
        }
    } 
}

