using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class PisModel : ICloneable
    {
        public double Pis_F9 { get; set; }
        public double Pis_H9 { get; set; }
        public double Pis_J9 { get; set; }
        public double Pis_L9 { get; set; }
        public double Pis_F11 { get; set; }
        public double Pis_H11 { get; set; }
        public double Pis_J11 { get; set; }
        public double Pis_L11 { get; set; }
        public double Pis_F17 { get; set; }
        public double Pis_H17 { get; set; }
        public double Pis_J17 { get; set; }
        public double Pis_L17 { get; set; }
        public double Pis_F19 { get; set; }
        public double Pis_H19 { get; set; }
        public double Pis_J19 { get; set; }
        public double Pis_L19 { get; set; }
        public double Pis_F25 { get; set; }
        public double Pis_H25 { get; set; }
        public double Pis_J25 { get; set; }
        public double Pis_L25 { get; set; }
        public double Pis_F27 { get; set; }
        public double Pis_H27 { get; set; }
        public double Pis_J27 { get; set; }
        public double Pis_L27 { get; set; }
        public double Pis_F29 { get; set; }
        public double Pis_H29 { get; set; }
        public double Pis_J29 { get; set; }
        public double Pis_L29 { get; set; }
        public double Pis_F31 { get; set; }
        public double Pis_H31 { get; set; }
        public double Pis_J31 { get; set; }
        public double Pis_L31 { get; set; }
        public double Pis_F37 { get; set; }
        public double Pis_H37 { get; set; }
        public double Pis_J37 { get; set; }
        public double Pis_L37 { get; set; }
        public double Pis_F39 { get; set; }
        public double Pis_H39 { get; set; }
        public double Pis_J39 { get; set; }
        public double Pis_L39 { get; set; }
        public double Pis_F41 { get; set; }
        public double Pis_H41 { get; set; }
        public double Pis_J41 { get; set; }
        public double Pis_L41 { get; set; }
        public double Pis_F47 { get; set; }
        public double Pis_H47 { get; set; }
        public double Pis_J47 { get; set; }
        public double Pis_L47 { get; set; }
        public double Pis_F49 { get; set; }
        public double Pis_H49 { get; set; }
        public double Pis_J49 { get; set; }
        public double Pis_L49 { get; set; }
        public double Pis_F51 { get; set; }
        public double Pis_H51 { get; set; }
        public double Pis_J51 { get; set; }
        public double Pis_L51 { get; set; }

        public string Pis_N27 { get; set; }
        public string Pis_P27 { get; set; }
        public string Pis_N47 { get; set; }
        public string Pis_P47 { get; set; }



        public object Clone()
        {
            return new PisModel()
            {
                Pis_F9 = Pis_F9,
                Pis_H9 = Pis_H9,
                Pis_J9 = Pis_J9,
                Pis_L9 = Pis_L9,
                Pis_F11 = Pis_F11,
                Pis_H11 = Pis_H11,
                Pis_J11 = Pis_J11,
                Pis_L11 = Pis_L11,
                Pis_F17 = Pis_F17,
                Pis_H17 = Pis_H17,
                Pis_J17 = Pis_J17,
                Pis_L17 = Pis_L17,
                Pis_F19 = Pis_F19,
                Pis_H19 = Pis_H19,
                Pis_J19 = Pis_J19,
                Pis_L19 = Pis_L19,
                Pis_F25 = Pis_F25,
                Pis_H25 = Pis_H25,
                Pis_J25 = Pis_J25,
                Pis_L25 = Pis_L25,
                Pis_F27 = Pis_F27,
                Pis_H27 = Pis_H27,
                Pis_J27 = Pis_J27,
                Pis_L27 = Pis_L27,
                Pis_F29 = Pis_F29,
                Pis_H29 = Pis_H29,
                Pis_J29 = Pis_J29,
                Pis_L29 = Pis_L29,
                Pis_F31 = Pis_F31,
                Pis_H31 = Pis_H31,
                Pis_J31 = Pis_J31,
                Pis_L31 = Pis_L31,
                Pis_F37 = Pis_F37,
                Pis_H37 = Pis_H37,
                Pis_J37 = Pis_J37,
                Pis_L37 = Pis_L37,
                Pis_F39 = Pis_F39,
                Pis_H39 = Pis_H39,
                Pis_J39 = Pis_J39,
                Pis_L39 = Pis_L39,
                Pis_F41 = Pis_F41,
                Pis_H41 = Pis_H41,
                Pis_J41 = Pis_J41,
                Pis_L41 = Pis_L41,
                Pis_F47 = Pis_F47,
                Pis_H47 = Pis_H47,
                Pis_J47 = Pis_J47,
                Pis_L47 = Pis_L47,
                Pis_F49 = Pis_F49,
                Pis_H49 = Pis_H49,
                Pis_J49 = Pis_J49,
                Pis_L49 = Pis_L49,
                Pis_F51 = Pis_F51,
                Pis_H51 = Pis_H51,
                Pis_J51 = Pis_J51,
                Pis_L51 = Pis_L51,

                Pis_N27 = Pis_N27,
                Pis_P27 = Pis_P27,
                Pis_N47 = Pis_N47,
                Pis_P47 = Pis_P47,

            };
        }
    } 
}

