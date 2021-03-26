using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Ui.WbEasyCalcData.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.BilledCons
{
    public class ViewModel : BaseSheetViewModel
    {
        #region Input props

        private string _billedCons_Desc_B8;
        public string BilledCons_Desc_B8
        {
            get => _billedCons_Desc_B8;
            set { _billedCons_Desc_B8 = value; RaisePropertyChanged(nameof(BilledCons_Desc_B8)); }
        }
        private string _billedCons_Desc_B9;
        public string BilledCons_Desc_B9
        {
            get => _billedCons_Desc_B9;
            set { _billedCons_Desc_B9 = value; RaisePropertyChanged(nameof(BilledCons_Desc_B9)); }
        }
        private string _billedCons_Desc_B10;
        public string BilledCons_Desc_B10
        {
            get => _billedCons_Desc_B10;
            set { _billedCons_Desc_B10 = value; RaisePropertyChanged(nameof(BilledCons_Desc_B10)); }
        }
        private string _billedCons_Desc_B11;
        public string BilledCons_Desc_B11
        {
            get => _billedCons_Desc_B11;
            set { _billedCons_Desc_B11 = value; RaisePropertyChanged(nameof(BilledCons_Desc_B11)); }
        }

        private string _billedCons_Desc_F8;
        public string BilledCons_Desc_F8
        {
            get => _billedCons_Desc_F8;
            set { _billedCons_Desc_F8 = value; RaisePropertyChanged(nameof(BilledCons_Desc_F8)); }
        }
        private string _billedCons_Desc_F9;
        public string BilledCons_Desc_F9
        {
            get => _billedCons_Desc_F9;
            set { _billedCons_Desc_F9 = value; RaisePropertyChanged(nameof(BilledCons_Desc_F9)); }
        }
        private string _billedCons_Desc_F10;
        public string BilledCons_Desc_F10
        {
            get => _billedCons_Desc_F10;
            set { _billedCons_Desc_F10 = value; RaisePropertyChanged(nameof(BilledCons_Desc_F10)); }
        }
        private string _billedCons_Desc_F11;
        public string BilledCons_Desc_F11
        {
            get => _billedCons_Desc_F11;
            set { _billedCons_Desc_F11 = value; RaisePropertyChanged(nameof(BilledCons_Desc_F11)); }
        }


        private double _billedConsBilledMetConsBulkWatSupExpM3D6;
        public double BilledCons_BilledMetConsBulkWatSupExpM3_D6
        {
            get => _billedConsBilledMetConsBulkWatSupExpM3D6;
            set { _billedConsBilledMetConsBulkWatSupExpM3D6 = value; RaisePropertyChanged(nameof(BilledCons_BilledMetConsBulkWatSupExpM3_D6)); Calculate(); }
        }

        private double _billedConsBilledUnmetConsBulkWatSupExpM3H6;
        public double BilledCons_BilledUnmetConsBulkWatSupExpM3_H6
        {
            get => _billedConsBilledUnmetConsBulkWatSupExpM3H6;
            set { _billedConsBilledUnmetConsBulkWatSupExpM3H6 = value; RaisePropertyChanged(nameof(BilledCons_BilledUnmetConsBulkWatSupExpM3_H6)); Calculate(); }
        }

        private double _billedCons_UnbMetConsM3_D8;
        public double BilledCons_UnbMetConsM3_D8
        {
            get => _billedCons_UnbMetConsM3_D8;
            set { _billedCons_UnbMetConsM3_D8 = value; RaisePropertyChanged(nameof(BilledCons_UnbMetConsM3_D8)); Calculate(); }
        }
        private double _billedCons_UnbMetConsM3_D9;
        public double BilledCons_UnbMetConsM3_D9
        {
            get => _billedCons_UnbMetConsM3_D9;
            set { _billedCons_UnbMetConsM3_D9 = value; RaisePropertyChanged(nameof(BilledCons_UnbMetConsM3_D9)); Calculate(); }
        }
        private double _billedCons_UnbMetConsM3_D10;
        public double BilledCons_UnbMetConsM3_D10
        {
            get => _billedCons_UnbMetConsM3_D10;
            set { _billedCons_UnbMetConsM3_D10 = value; RaisePropertyChanged(nameof(BilledCons_UnbMetConsM3_D10)); Calculate(); }
        }
        private double _billedCons_UnbMetConsM3_D11;
        public double BilledCons_UnbMetConsM3_D11
        {
            get => _billedCons_UnbMetConsM3_D11;
            set { _billedCons_UnbMetConsM3_D11 = value; RaisePropertyChanged(nameof(BilledCons_UnbMetConsM3_D11)); Calculate(); }
        }
        private double _billedCons_UnbUnmetConsM3_H8;
        public double BilledCons_UnbUnmetConsM3_H8
        {
            get => _billedCons_UnbUnmetConsM3_H8;
            set { _billedCons_UnbUnmetConsM3_H8 = value; RaisePropertyChanged(nameof(BilledCons_UnbUnmetConsM3_H8)); Calculate(); }
        }
        private double _billedCons_UnbUnmetConsM3_H9;
        public double BilledCons_UnbUnmetConsM3_H9
        {
            get => _billedCons_UnbUnmetConsM3_H9;
            set { _billedCons_UnbUnmetConsM3_H9 = value; RaisePropertyChanged(nameof(BilledCons_UnbUnmetConsM3_H9)); Calculate(); }
        }
        private double _billedCons_UnbUnmetConsM3_H10;
        public double BilledCons_UnbUnmetConsM3_H10
        {
            get => _billedCons_UnbUnmetConsM3_H10;
            set { _billedCons_UnbUnmetConsM3_H10 = value; RaisePropertyChanged(nameof(BilledCons_UnbUnmetConsM3_H10)); Calculate(); }
        }
        private double _billedCons_UnbUnmetConsM3_H11;
        public double BilledCons_UnbUnmetConsM3_H11
        {
            get => _billedCons_UnbUnmetConsM3_H11;
            set { _billedCons_UnbUnmetConsM3_H11 = value; RaisePropertyChanged(nameof(BilledCons_UnbUnmetConsM3_H11)); Calculate(); }
        }

        #endregion

        #region Output props

        private double _billedCons_Sum_D28;
        public double BilledCons_Sum_D28
        {
            get => _billedCons_Sum_D28;
            set { _billedCons_Sum_D28 = value; RaisePropertyChanged(nameof(BilledCons_Sum_D28)); }
        }
        private double _billedCons_Sum_H28;
        public double BilledCons_Sum_H28
        {
            get => _billedCons_Sum_H28;
            set { _billedCons_Sum_H28 = value; RaisePropertyChanged(nameof(BilledCons_Sum_H28)); }
        }

        #endregion

        public BilledConsModel Model => new BilledConsModel()
        {
            BilledCons_Desc_B8 = BilledCons_Desc_B8,
            BilledCons_Desc_B9 = BilledCons_Desc_B9,
            BilledCons_Desc_B10 = BilledCons_Desc_B10,
            BilledCons_Desc_B11 = BilledCons_Desc_B11,
            BilledCons_Desc_F8 = BilledCons_Desc_F8,
            BilledCons_Desc_F9 = BilledCons_Desc_F9,
            BilledCons_Desc_F10 = BilledCons_Desc_F10,
            BilledCons_Desc_F11 = BilledCons_Desc_F11,
            BilledCons_BilledMetConsBulkWatSupExpM3_D6 = BilledCons_BilledMetConsBulkWatSupExpM3_D6,
            BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 = BilledCons_BilledUnmetConsBulkWatSupExpM3_H6,
            BilledCons_UnbMetConsM3_D8 = BilledCons_UnbMetConsM3_D8,
            BilledCons_UnbMetConsM3_D9 = BilledCons_UnbMetConsM3_D9,
            BilledCons_UnbMetConsM3_D10 = BilledCons_UnbMetConsM3_D10,
            BilledCons_UnbMetConsM3_D11 = BilledCons_UnbMetConsM3_D11,
            BilledCons_UnbUnmetConsM3_H8 = BilledCons_UnbUnmetConsM3_H8,
            BilledCons_UnbUnmetConsM3_H9 = BilledCons_UnbUnmetConsM3_H9,
            BilledCons_UnbUnmetConsM3_H10 = BilledCons_UnbUnmetConsM3_H10,
            BilledCons_UnbUnmetConsM3_H11 = BilledCons_UnbUnmetConsM3_H11,
            // Output
            BilledCons_Sum_D28 = BilledCons_Sum_D28,
            BilledCons_Sum_H28 = BilledCons_Sum_H28,
        };

        internal void Refreash(BilledConsModel model)
        {
            BilledCons_Sum_D28 = model.BilledCons_Sum_D28;
            BilledCons_Sum_H28 = model.BilledCons_Sum_H28;
        }

        //public ViewModel() { }
        public ViewModel(BilledConsModel model)
        {
            if (model == null) return;

            // Input
            BilledCons_Desc_B8 = model.BilledCons_Desc_B8;
            BilledCons_Desc_B9 = model.BilledCons_Desc_B9;
            BilledCons_Desc_B10 = model.BilledCons_Desc_B10;
            BilledCons_Desc_B11 = model.BilledCons_Desc_B11;
            BilledCons_Desc_F8 = model.BilledCons_Desc_F8;
            BilledCons_Desc_F9 = model.BilledCons_Desc_F9;
            BilledCons_Desc_F10 = model.BilledCons_Desc_F10;
            BilledCons_Desc_F11 = model.BilledCons_Desc_F11;
            BilledCons_BilledMetConsBulkWatSupExpM3_D6 = model.BilledCons_BilledMetConsBulkWatSupExpM3_D6;
            BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 = model.BilledCons_BilledUnmetConsBulkWatSupExpM3_H6;
            BilledCons_UnbMetConsM3_D8 = model.BilledCons_UnbMetConsM3_D8;
            BilledCons_UnbMetConsM3_D9 = model.BilledCons_UnbMetConsM3_D9;
            BilledCons_UnbMetConsM3_D10 = model.BilledCons_UnbMetConsM3_D10;
            BilledCons_UnbMetConsM3_D11 = model.BilledCons_UnbMetConsM3_D11;
            BilledCons_UnbUnmetConsM3_H8 = model.BilledCons_UnbUnmetConsM3_H8;
            BilledCons_UnbUnmetConsM3_H9 = model.BilledCons_UnbUnmetConsM3_H9;
            BilledCons_UnbUnmetConsM3_H10 = model.BilledCons_UnbUnmetConsM3_H10;
            BilledCons_UnbUnmetConsM3_H11 = model.BilledCons_UnbUnmetConsM3_H11;
        }
    }
}
