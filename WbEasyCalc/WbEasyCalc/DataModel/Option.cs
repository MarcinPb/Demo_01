using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel.WbEasyCalc;

namespace DataModel
{
    public class Option
    {
        public FinancDataModel FinancDataModel { get; set; }
        public MatrixOneInModel MatrixOneInModel { get; set; }
        public MatrixOneInModel MatrixTwoInModel { get; set; }
    }
}
