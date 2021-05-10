using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel.WbEasyCalc;

namespace Database.DataModel
{
    public class Option
    {
        public FinancDataModel FinancDataModel { get; set; }
        public MatrixInModel MatrixOneInModel { get; set; }
        public MatrixInModel MatrixTwoInModel { get; set; }
    }
}
