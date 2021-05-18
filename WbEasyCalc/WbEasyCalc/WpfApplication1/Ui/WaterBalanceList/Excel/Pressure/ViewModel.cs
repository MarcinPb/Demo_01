using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Ui.WbEasyCalcData.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.Pressure
{
    public class ViewModel : BaseSheetViewModel
    {
        #region Input props

        private string _prs_Area_B7;
        public string Prs_Area_B7
        {
            get => _prs_Area_B7;
            set { _prs_Area_B7 = value; RaisePropertyChanged(nameof(Prs_Area_B7)); }
        }
        private string _prs_Area_B8;
        public string Prs_Area_B8
        {
            get => _prs_Area_B8;
            set { _prs_Area_B8 = value; RaisePropertyChanged(nameof(Prs_Area_B8)); }
        }
        private string _prs_Area_B9;
        public string Prs_Area_B9
        {
            get => _prs_Area_B9;
            set { _prs_Area_B9 = value; RaisePropertyChanged(nameof(Prs_Area_B9)); }
        }
        private string _prs_Area_B10;
        public string Prs_Area_B10
        {
            get => _prs_Area_B10;
            set { _prs_Area_B10 = value; RaisePropertyChanged(nameof(Prs_Area_B10)); }
        }

        private double _prsApproxNoOfConnD7;
        private double _prsDailyAvgPrsMF7;
        private double _prsApproxNoOfConnD8;
        private double _prsDailyAvgPrsMF8;
        private double _prsApproxNoOfConnD9;
        private double _prsDailyAvgPrsMF9;
        private double _prsApproxNoOfConnD10;
        private double _prsDailyAvgPrsMF10;
        private double _prs_ErrorMarg_F26;

        public double Prs_ApproxNoOfConn_D7
        {
            get => _prsApproxNoOfConnD7;
            set { _prsApproxNoOfConnD7 = value; RaisePropertyChanged(nameof(Prs_ApproxNoOfConn_D7)); Calculate(); }
        }
        public double Prs_DailyAvgPrsM_F7
        {
            get => _prsDailyAvgPrsMF7;
            set { _prsDailyAvgPrsMF7 = value; RaisePropertyChanged(nameof(Prs_DailyAvgPrsM_F7)); Calculate(); }
        }
        public double Prs_ApproxNoOfConn_D8
        {
            get => _prsApproxNoOfConnD8;
            set { _prsApproxNoOfConnD8 = value; RaisePropertyChanged(nameof(Prs_ApproxNoOfConn_D8)); Calculate(); }
        }
        public double Prs_DailyAvgPrsM_F8
        {
            get => _prsDailyAvgPrsMF8;
            set { _prsDailyAvgPrsMF8 = value; RaisePropertyChanged(nameof(Prs_DailyAvgPrsM_F8)); Calculate(); }
        }
        public double Prs_ApproxNoOfConn_D9
        {
            get => _prsApproxNoOfConnD9;
            set { _prsApproxNoOfConnD9 = value; RaisePropertyChanged(nameof(Prs_ApproxNoOfConn_D9)); Calculate(); }
        }
        public double Prs_DailyAvgPrsM_F9
        {
            get => _prsDailyAvgPrsMF9;
            set { _prsDailyAvgPrsMF9 = value; RaisePropertyChanged(nameof(Prs_DailyAvgPrsM_F9)); Calculate(); }
        }
        public double Prs_ApproxNoOfConn_D10
        {
            get => _prsApproxNoOfConnD10;
            set { _prsApproxNoOfConnD10 = value; RaisePropertyChanged(nameof(Prs_ApproxNoOfConn_D10)); Calculate(); }
        }
        public double Prs_DailyAvgPrsM_F10
        {
            get => _prsDailyAvgPrsMF10;
            set { _prsDailyAvgPrsMF10 = value; RaisePropertyChanged(nameof(Prs_DailyAvgPrsM_F10)); Calculate(); }
        }


        public double Prs_ErrorMarg_F26
        {
            get => _prs_ErrorMarg_F26;
            set { _prs_ErrorMarg_F26 = value; RaisePropertyChanged(nameof(Prs_ErrorMarg_F26)); Calculate(); }
        }

        #endregion

        #region Output props

        private double _prs_BestEstimate_F33;
        public double Prs_BestEstimate_F33
        {
            get => _prs_BestEstimate_F33;
            set { _prs_BestEstimate_F33 = value; RaisePropertyChanged(nameof(Prs_BestEstimate_F33)); }
        }
        private double _prs_Min_F29;
        public double Prs_Min_F29
        {
            get => _prs_Min_F29;
            set { _prs_Min_F29 = value; RaisePropertyChanged(nameof(Prs_Min_F29)); }
        }
        private double _prs_Max_F31;
        public double Prs_Max_F31
        {
            get => _prs_Max_F31;
            set { _prs_Max_F31 = value; RaisePropertyChanged(nameof(Prs_Max_F31)); }
        }

        #endregion

        public PressureModel Model => new PressureModel()
        {
            // Input
            Prs_Area_B7 = Prs_Area_B7,
            Prs_Area_B8 = Prs_Area_B8,
            Prs_Area_B9 = Prs_Area_B9,
            Prs_Area_B10 = Prs_Area_B10,
            Prs_ApproxNoOfConn_D7 = Prs_ApproxNoOfConn_D7,
            Prs_DailyAvgPrsM_F7 = Prs_DailyAvgPrsM_F7,
            Prs_ApproxNoOfConn_D8 = Prs_ApproxNoOfConn_D8,
            Prs_DailyAvgPrsM_F8 = Prs_DailyAvgPrsM_F8,
            Prs_ApproxNoOfConn_D9 = Prs_ApproxNoOfConn_D9,
            Prs_DailyAvgPrsM_F9 = Prs_DailyAvgPrsM_F9,
            Prs_ApproxNoOfConn_D10 = Prs_ApproxNoOfConn_D10,
            Prs_DailyAvgPrsM_F10 = Prs_DailyAvgPrsM_F10,
            Prs_ErrorMarg_F26 = Prs_ErrorMarg_F26,
            // Output
            Prs_BestEstimate_F33 = Prs_BestEstimate_F33,
            Prs_Min_F29 = Prs_Min_F29,
            Prs_Max_F31 = Prs_Max_F31,
        };

        internal void Refreash(PressureModel model)
        {
            Prs_BestEstimate_F33 = model.Prs_BestEstimate_F33;
            Prs_Min_F29 = model.Prs_Min_F29;
            Prs_Max_F31 = model.Prs_Max_F31;
        }

        public ViewModel(PressureModel model)
        {
            if (model == null) return;

            // Input
            Prs_Area_B7 = model.Prs_Area_B7;
            Prs_Area_B8 = model.Prs_Area_B8;
            Prs_Area_B9 = model.Prs_Area_B9;
            Prs_Area_B10 = model.Prs_Area_B10;
            Prs_ApproxNoOfConn_D7 = model.Prs_ApproxNoOfConn_D7;
            Prs_DailyAvgPrsM_F7 = model.Prs_DailyAvgPrsM_F7;
            Prs_ApproxNoOfConn_D8 = model.Prs_ApproxNoOfConn_D8;
            Prs_DailyAvgPrsM_F8 = model.Prs_DailyAvgPrsM_F8;
            Prs_ApproxNoOfConn_D9 = model.Prs_ApproxNoOfConn_D9;
            Prs_DailyAvgPrsM_F9 = model.Prs_DailyAvgPrsM_F9;
            Prs_ApproxNoOfConn_D10 = model.Prs_ApproxNoOfConn_D10;
            Prs_DailyAvgPrsM_F10 = model.Prs_DailyAvgPrsM_F10;
            Prs_ErrorMarg_F26 = model.Prs_ErrorMarg_F26;
        }
    }
}
