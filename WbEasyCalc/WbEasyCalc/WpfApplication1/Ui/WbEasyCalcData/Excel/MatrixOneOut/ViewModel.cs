using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.MatrixOneOut
{
    public class ViewModel : BaseSheetViewModel
    {
        private string _matOneOut_C11;
        public string MatOneOut_C11
        {
            get => _matOneOut_C11;
            set { _matOneOut_C11 = value; RaisePropertyChanged(); }
        }

        public MatrixOneOutModel Model => new MatrixOneOutModel()
        {
            MatOneOut_C11 = MatOneOut_C11,
        };

        internal void Refreash(MatrixOneOutModel model)
        {
            MatOneOut_C11 = model.MatOneOut_C11;
        }

        public ViewModel(MatrixOneOutModel model)
        {
            if (model == null) return;
        }
    }
}
