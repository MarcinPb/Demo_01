using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Ui.WbEasyCalcData.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.UnbCons
{
    public class ViewModel : BaseSheetViewModel
    {
        #region Input props

        private string _unbilledCons_Desc_D8;
        public string UnbilledCons_Desc_D8
        {
            get => _unbilledCons_Desc_D8;
            set { _unbilledCons_Desc_D8 = value; RaisePropertyChanged(nameof(UnbilledCons_Desc_D8)); }
        }
        private string _unbilledCons_Desc_D9;
        public string UnbilledCons_Desc_D9
        {
            get => _unbilledCons_Desc_D9;
            set { _unbilledCons_Desc_D9 = value; RaisePropertyChanged(nameof(UnbilledCons_Desc_D9)); }
        }
        private string _unbilledCons_Desc_D10;
        public string UnbilledCons_Desc_D10
        {
            get => _unbilledCons_Desc_D10;
            set { _unbilledCons_Desc_D10 = value; RaisePropertyChanged(nameof(UnbilledCons_Desc_D10)); }
        }
        private string _unbilledCons_Desc_D11;
        public string UnbilledCons_Desc_D11
        {
            get => _unbilledCons_Desc_D11;
            set { _unbilledCons_Desc_D11 = value; RaisePropertyChanged(nameof(UnbilledCons_Desc_D11)); }
        }

        private string _unbilledCons_Desc_F6;
        public string UnbilledCons_Desc_F6
        {
            get => _unbilledCons_Desc_F6;
            set { _unbilledCons_Desc_F6 = value; RaisePropertyChanged(nameof(UnbilledCons_Desc_F6)); }
        }
        private string _unbilledCons_Desc_F7;
        public string UnbilledCons_Desc_F7
        {
            get => _unbilledCons_Desc_F7;
            set { _unbilledCons_Desc_F7 = value; RaisePropertyChanged(nameof(UnbilledCons_Desc_F7)); }
        }
        private string _unbilledCons_Desc_F8;
        public string UnbilledCons_Desc_F8
        {
            get => _unbilledCons_Desc_F8;
            set { _unbilledCons_Desc_F8 = value; RaisePropertyChanged(nameof(UnbilledCons_Desc_F8)); }
        }
        private string _unbilledCons_Desc_F9;
        public string UnbilledCons_Desc_F9
        {
            get => _unbilledCons_Desc_F9;
            set { _unbilledCons_Desc_F9 = value; RaisePropertyChanged(nameof(UnbilledCons_Desc_F9)); }
        }
        private string _unbilledCons_Desc_F10;
        public string UnbilledCons_Desc_F10
        {
            get => _unbilledCons_Desc_F10;
            set { _unbilledCons_Desc_F10 = value; RaisePropertyChanged(nameof(UnbilledCons_Desc_F10)); }
        }
        private string _unbilledCons_Desc_F11;
        public string UnbilledCons_Desc_F11
        {
            get => _unbilledCons_Desc_F11;
            set { _unbilledCons_Desc_F11 = value; RaisePropertyChanged(nameof(UnbilledCons_Desc_F11)); }
        }

        private double _unbilledConsMetConsBulkWatSupExpM3D6;
        public double UnbilledCons_MetConsBulkWatSupExpM3_D6
        {
            get => _unbilledConsMetConsBulkWatSupExpM3D6;
            set { _unbilledConsMetConsBulkWatSupExpM3D6 = value; RaisePropertyChanged(nameof(UnbilledCons_MetConsBulkWatSupExpM3_D6)); Calculate(); }
        }

        private double _unbilledCons_UnbMetConsM3_D8;
        public double UnbilledCons_UnbMetConsM3_D8
        {
            get => _unbilledCons_UnbMetConsM3_D8;
            set { _unbilledCons_UnbMetConsM3_D8 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbMetConsM3_D8)); Calculate(); }
        }
        private double _unbilledCons_UnbMetConsM3_D9;
        public double UnbilledCons_UnbMetConsM3_D9
        {
            get => _unbilledCons_UnbMetConsM3_D9;
            set { _unbilledCons_UnbMetConsM3_D9 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbMetConsM3_D9)); Calculate(); }
        }
        private double _unbilledCons_UnbMetConsM3_D10;
        public double UnbilledCons_UnbMetConsM3_D10
        {
            get => _unbilledCons_UnbMetConsM3_D10;
            set { _unbilledCons_UnbMetConsM3_D10 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbMetConsM3_D10)); Calculate(); }
        }
        private double _unbilledCons_UnbMetConsM3_D11;
        public double UnbilledCons_UnbMetConsM3_D11
        {
            get => _unbilledCons_UnbMetConsM3_D11;
            set { _unbilledCons_UnbMetConsM3_D11 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbMetConsM3_D11)); Calculate(); }
        }





        private double _unbilledCons_UnbUnmetConsM3_H6;
        public double UnbilledCons_UnbUnmetConsM3_H6
        {
            get => _unbilledCons_UnbUnmetConsM3_H6;
            set { _unbilledCons_UnbUnmetConsM3_H6 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbUnmetConsM3_H6)); Calculate(); }
        }
        private double _unbilledCons_UnbUnmetConsM3_H7;
        public double UnbilledCons_UnbUnmetConsM3_H7
        {
            get => _unbilledCons_UnbUnmetConsM3_H7;
            set { _unbilledCons_UnbUnmetConsM3_H7 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbUnmetConsM3_H7)); Calculate(); }
        }
        private double _unbilledCons_UnbUnmetConsM3_H8;
        public double UnbilledCons_UnbUnmetConsM3_H8
        {
            get => _unbilledCons_UnbUnmetConsM3_H8;
            set { _unbilledCons_UnbUnmetConsM3_H8 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbUnmetConsM3_H8)); Calculate(); }
        }
        private double _unbilledCons_UnbUnmetConsM3_H9;
        public double UnbilledCons_UnbUnmetConsM3_H9
        {
            get => _unbilledCons_UnbUnmetConsM3_H9;
            set { _unbilledCons_UnbUnmetConsM3_H9 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbUnmetConsM3_H9)); Calculate(); }
        }
        private double _unbilledCons_UnbUnmetConsM3_H10;
        public double UnbilledCons_UnbUnmetConsM3_H10
        {
            get => _unbilledCons_UnbUnmetConsM3_H10;
            set { _unbilledCons_UnbUnmetConsM3_H10 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbUnmetConsM3_H10)); Calculate(); }
        }
        private double _unbilledCons_UnbUnmetConsM3_H11;
        public double UnbilledCons_UnbUnmetConsM3_H11
        {
            get => _unbilledCons_UnbUnmetConsM3_H11;
            set { _unbilledCons_UnbUnmetConsM3_H11 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbUnmetConsM3_H11)); Calculate(); }
        }
        private double _unbilledCons_UnbUnmetConsError_J6;
        public double UnbilledCons_UnbUnmetConsError_J6
        {
            get => _unbilledCons_UnbUnmetConsError_J6;
            set { _unbilledCons_UnbUnmetConsError_J6 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbUnmetConsError_J6)); Calculate(); }
        }
        private double _unbilledCons_UnbUnmetConsError_J7;
        public double UnbilledCons_UnbUnmetConsError_J7
        {
            get => _unbilledCons_UnbUnmetConsError_J7;
            set { _unbilledCons_UnbUnmetConsError_J7 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbUnmetConsError_J7)); Calculate(); }
        }
        private double _unbilledCons_UnbUnmetConsError_J8;
        public double UnbilledCons_UnbUnmetConsError_J8
        {
            get => _unbilledCons_UnbUnmetConsError_J8;
            set { _unbilledCons_UnbUnmetConsError_J8 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbUnmetConsError_J8)); Calculate(); }
        }
        private double _unbilledCons_UnbUnmetConsError_J9;
        public double UnbilledCons_UnbUnmetConsError_J9
        {
            get => _unbilledCons_UnbUnmetConsError_J9;
            set { _unbilledCons_UnbUnmetConsError_J9 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbUnmetConsError_J9)); Calculate(); }
        }
        private double _unbilledCons_UnbUnmetConsError_J10;
        public double UnbilledCons_UnbUnmetConsError_J10
        {
            get => _unbilledCons_UnbUnmetConsError_J10;
            set { _unbilledCons_UnbUnmetConsError_J10 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbUnmetConsError_J10)); Calculate(); }
        }
        private double _unbilledCons_UnbUnmetConsError_J11;
        public double UnbilledCons_UnbUnmetConsError_J11
        {
            get => _unbilledCons_UnbUnmetConsError_J11;
            set { _unbilledCons_UnbUnmetConsError_J11 = value; RaisePropertyChanged(nameof(UnbilledCons_UnbUnmetConsError_J11)); Calculate(); }
        }

        #endregion

        #region Output props

        private double _unbilledCons_Sum_D32;
        public double UnbilledCons_Sum_D32
        {
            get => _unbilledCons_Sum_D32;
            set { _unbilledCons_Sum_D32 = value; RaisePropertyChanged(nameof(UnbilledCons_Sum_D32)); }
        }
        private double _unbilledCons_ErrorMarg_J25;
        public double UnbilledCons_ErrorMarg_J25
        {
            get => _unbilledCons_ErrorMarg_J25;
            set { _unbilledCons_ErrorMarg_J25 = value; RaisePropertyChanged(nameof(UnbilledCons_ErrorMarg_J25)); }
        }
        private double _unbilledCons_Min_H28;
        public double UnbilledCons_Min_H28
        {
            get => _unbilledCons_Min_H28;
            set { _unbilledCons_Min_H28 = value; RaisePropertyChanged(nameof(UnbilledCons_Min_H28)); }
        }
        private double _unbilledCons_Max_H30;
        public double UnbilledCons_Max_H30
        {
            get => _unbilledCons_Max_H30;
            set { _unbilledCons_Max_H30 = value; RaisePropertyChanged(nameof(UnbilledCons_Max_H30)); }
        }
        private double _unbilledCons_BestEstimate_H32;
        public double UnbilledCons_BestEstimate_H32
        {
            get => _unbilledCons_BestEstimate_H32;
            set { _unbilledCons_BestEstimate_H32 = value; RaisePropertyChanged(nameof(UnbilledCons_BestEstimate_H32)); }
        }

        #endregion

        public UnbilledConsModel Model => new UnbilledConsModel()
        {
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

        internal void Refreash(UnbilledConsModel model)
        {
            UnbilledCons_Sum_D32 = model.UnbilledCons_Sum_D32;
            UnbilledCons_ErrorMarg_J25 = model.UnbilledCons_ErrorMarg_J25;
            UnbilledCons_Min_H28 = model.UnbilledCons_Min_H28;
            UnbilledCons_Max_H30 = model.UnbilledCons_Max_H30;
            UnbilledCons_BestEstimate_H32 = model.UnbilledCons_BestEstimate_H32;
        }

        public ViewModel(UnbilledConsModel model)
        {
            if (model == null) return;

            // Input
            UnbilledCons_Desc_D8 = model.UnbilledCons_Desc_D8;
            UnbilledCons_Desc_D9 = model.UnbilledCons_Desc_D9;
            UnbilledCons_Desc_D10 = model.UnbilledCons_Desc_D10;
            UnbilledCons_Desc_D11 = model.UnbilledCons_Desc_D11;
            UnbilledCons_Desc_F6 = model.UnbilledCons_Desc_F6;
            UnbilledCons_Desc_F7 = model.UnbilledCons_Desc_F7;
            UnbilledCons_Desc_F8 = model.UnbilledCons_Desc_F8;
            UnbilledCons_Desc_F9 = model.UnbilledCons_Desc_F9;
            UnbilledCons_Desc_F10 = model.UnbilledCons_Desc_F10;
            UnbilledCons_Desc_F11 = model.UnbilledCons_Desc_F11;
            UnbilledCons_MetConsBulkWatSupExpM3_D6 = model.UnbilledCons_MetConsBulkWatSupExpM3_D6;

            UnbilledCons_UnbMetConsM3_D8 = model.UnbilledCons_UnbMetConsM3_D8;
            UnbilledCons_UnbMetConsM3_D9 = model.UnbilledCons_UnbMetConsM3_D9;
            UnbilledCons_UnbMetConsM3_D10 = model.UnbilledCons_UnbMetConsM3_D10;
            UnbilledCons_UnbMetConsM3_D11 = model.UnbilledCons_UnbMetConsM3_D11;
            UnbilledCons_UnbUnmetConsM3_H6 = model.UnbilledCons_UnbUnmetConsM3_H6;
            UnbilledCons_UnbUnmetConsM3_H7 = model.UnbilledCons_UnbUnmetConsM3_H7;
            UnbilledCons_UnbUnmetConsM3_H8 = model.UnbilledCons_UnbUnmetConsM3_H8;
            UnbilledCons_UnbUnmetConsM3_H9 = model.UnbilledCons_UnbUnmetConsM3_H9;
            UnbilledCons_UnbUnmetConsM3_H10 = model.UnbilledCons_UnbUnmetConsM3_H10;
            UnbilledCons_UnbUnmetConsM3_H11 = model.UnbilledCons_UnbUnmetConsM3_H11;
            UnbilledCons_UnbUnmetConsError_J6 = model.UnbilledCons_UnbUnmetConsError_J6;
            UnbilledCons_UnbUnmetConsError_J7 = model.UnbilledCons_UnbUnmetConsError_J7;
            UnbilledCons_UnbUnmetConsError_J8 = model.UnbilledCons_UnbUnmetConsError_J8;
            UnbilledCons_UnbUnmetConsError_J9 = model.UnbilledCons_UnbUnmetConsError_J9;
            UnbilledCons_UnbUnmetConsError_J10 = model.UnbilledCons_UnbUnmetConsError_J10;
            UnbilledCons_UnbUnmetConsError_J11 = model.UnbilledCons_UnbUnmetConsError_J11;
        }
    }
}
