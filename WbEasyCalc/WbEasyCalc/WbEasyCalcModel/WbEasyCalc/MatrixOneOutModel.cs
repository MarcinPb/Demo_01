using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class MatrixOneOutModel : ICloneable
    {
        public string MatOneOut_C11 { get; set; }

        public object Clone()
        {
            return new MatrixOneOutModel()
            {
                MatOneOut_C11 = MatOneOut_C11,
            };
        }
    } 
}

