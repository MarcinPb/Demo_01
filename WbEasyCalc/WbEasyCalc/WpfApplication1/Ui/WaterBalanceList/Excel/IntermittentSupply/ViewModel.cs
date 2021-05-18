using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Ui.WbEasyCalcData.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.IntermittentSupply
{
    public class ViewModel : BaseSheetViewModel
    {
        #region Input props

        private string _interm_Area_B7;
        public string Interm_Area_B7
        {
            get => _interm_Area_B7;
            set { _interm_Area_B7 = value; RaisePropertyChanged(nameof(Interm_Area_B7)); }
        }
        private string _interm_Area_B8;
        public string Interm_Area_B8
        {
            get => _interm_Area_B8;
            set { _interm_Area_B8 = value; RaisePropertyChanged(nameof(Interm_Area_B8)); }
        }
        private string _interm_Area_B9;
        public string Interm_Area_B9
        {
            get => _interm_Area_B9;
            set { _interm_Area_B9 = value; RaisePropertyChanged(nameof(Interm_Area_B9)); }
        }
        private string _interm_Area_B10;
        public string Interm_Area_B10
        {
            get => _interm_Area_B10;
            set { _interm_Area_B10 = value; RaisePropertyChanged(nameof(Interm_Area_B10)); }
        }

        private double _interm_Conn_D7;
        public double Interm_Conn_D7
        {
            get => _interm_Conn_D7;
            set { _interm_Conn_D7 = value; RaisePropertyChanged(nameof(Interm_Conn_D7)); Calculate(); }
        }
        private double _interm_Conn_D8;
        public double Interm_Conn_D8
        {
            get => _interm_Conn_D8;
            set { _interm_Conn_D8 = value; RaisePropertyChanged(nameof(Interm_Conn_D8)); Calculate(); }
        }
        private double _interm_Conn_D9;
        public double Interm_Conn_D9
        {
            get => _interm_Conn_D9;
            set { _interm_Conn_D9 = value; RaisePropertyChanged(nameof(Interm_Conn_D9)); Calculate(); }
        }
        private double _interm_Conn_D10;
        public double Interm_Conn_D10
        {
            get => _interm_Conn_D10;
            set { _interm_Conn_D10 = value; RaisePropertyChanged(nameof(Interm_Conn_D10)); Calculate(); }
        }

        private double _interm_Days_F7;
        public double Interm_Days_F7
        {
            get => _interm_Days_F7;
            set { _interm_Days_F7 = value; RaisePropertyChanged(nameof(Interm_Days_F7)); Calculate(); }
        }
        private double _interm_Days_F8;
        public double Interm_Days_F8
        {
            get => _interm_Days_F8;
            set { _interm_Days_F8 = value; RaisePropertyChanged(nameof(Interm_Days_F8)); Calculate(); }
        }
        private double _interm_Days_F9;
        public double Interm_Days_F9
        {
            get => _interm_Days_F9;
            set { _interm_Days_F9 = value; RaisePropertyChanged(nameof(Interm_Days_F9)); Calculate(); }
        }
        private double _interm_Days_F10;
        public double Interm_Days_F10
        {
            get => _interm_Days_F10;
            set { _interm_Days_F10 = value; RaisePropertyChanged(nameof(Interm_Days_F10)); Calculate(); }
        }

        private double _interm_Hour_H7;
        public double Interm_Hour_H7
        {
            get => _interm_Hour_H7;
            set { _interm_Hour_H7 = value; RaisePropertyChanged(nameof(Interm_Hour_H7)); Calculate(); }
        }
        private double _interm_Hour_H8;
        public double Interm_Hour_H8
        {
            get => _interm_Hour_H8;
            set { _interm_Hour_H8 = value; RaisePropertyChanged(nameof(Interm_Hour_H8)); Calculate(); }
        }
        private double _interm_Hour_H9;
        public double Interm_Hour_H9
        {
            get => _interm_Hour_H9;
            set { _interm_Hour_H9 = value; RaisePropertyChanged(nameof(Interm_Hour_H9)); Calculate(); }
        }
        private double _interm_Hour_H10;
        public double Interm_Hour_H10
        {
            get => _interm_Hour_H10;
            set { _interm_Hour_H10 = value; RaisePropertyChanged(nameof(Interm_Hour_H10)); Calculate(); }
        }
        private double _interm_ErrorMarg_H26;
        public double Interm_ErrorMarg_H26
        {
            get => _interm_ErrorMarg_H26;
            set { _interm_ErrorMarg_H26 = value; RaisePropertyChanged(nameof(Interm_ErrorMarg_H26)); Calculate(); }
        }

        #endregion

        #region Output props

        private double _interm_BestEstimate_H33;
        public double Interm_BestEstimate_H33
        {
            get => _interm_BestEstimate_H33;
            set { _interm_BestEstimate_H33 = value; RaisePropertyChanged(nameof(Interm_BestEstimate_H33)); }
        }
        private double _interm_Min_H29;
        public double Interm_Min_H29
        {
            get => _interm_Min_H29;
            set { _interm_Min_H29 = value; RaisePropertyChanged(nameof(Interm_Min_H29)); }
        }
        private double _interm_Max_H31;
        public double Interm_Max_H31
        {
            get => _interm_Max_H31;
            set { _interm_Max_H31 = value; RaisePropertyChanged(nameof(Interm_Max_H31)); }
        }

        #endregion

        public IntermModel Model => new IntermModel()
        {
            // Input
            Interm_Area_B7 = Interm_Area_B7,
            Interm_Area_B8 = Interm_Area_B8,
            Interm_Area_B9 = Interm_Area_B9,
            Interm_Area_B10 = Interm_Area_B10,
            Interm_Conn_D7 = Interm_Conn_D7,
            Interm_Conn_D8 = Interm_Conn_D8,
            Interm_Conn_D9 = Interm_Conn_D9,
            Interm_Conn_D10 = Interm_Conn_D10,
            Interm_Days_F7 = Interm_Days_F7,
            Interm_Days_F8 = Interm_Days_F8,
            Interm_Days_F9 = Interm_Days_F9,
            Interm_Days_F10 = Interm_Days_F10,
            Interm_Hour_H7 = Interm_Hour_H7,
            Interm_Hour_H8 = Interm_Hour_H8,
            Interm_Hour_H9 = Interm_Hour_H9,
            Interm_Hour_H10 = Interm_Hour_H10,
            Interm_ErrorMarg_H26 = Interm_ErrorMarg_H26,
            // Output
            Interm_Min_H29 = Interm_Min_H29,
            Interm_Max_H31 = Interm_Max_H31,
            Interm_BestEstimate_H33 = Interm_BestEstimate_H33,
        };

        internal void Refreash(IntermModel model)
        {
            Interm_BestEstimate_H33 = model.Interm_BestEstimate_H33;
            Interm_Min_H29 = model.Interm_Min_H29;
            Interm_Max_H31 = model.Interm_Max_H31;
        }

        public ViewModel(IntermModel model)
        {
            if (model == null) return;

            // Input
            Interm_Area_B7 = model.Interm_Area_B7;
            Interm_Area_B8 = model.Interm_Area_B8;
            Interm_Area_B9 = model.Interm_Area_B9;
            Interm_Area_B10 = model.Interm_Area_B10;
            Interm_Conn_D7 = model.Interm_Conn_D7;
            Interm_Conn_D8 = model.Interm_Conn_D8;
            Interm_Conn_D9 = model.Interm_Conn_D9;
            Interm_Conn_D10 = model.Interm_Conn_D10;
            Interm_Days_F7 = model.Interm_Days_F7;
            Interm_Days_F8 = model.Interm_Days_F8;
            Interm_Days_F9 = model.Interm_Days_F9;
            Interm_Days_F10 = model.Interm_Days_F10;
            Interm_Hour_H7 = model.Interm_Hour_H7;
            Interm_Hour_H8 = model.Interm_Hour_H8;
            Interm_Hour_H9 = model.Interm_Hour_H9;
            Interm_Hour_H10 = model.Interm_Hour_H10;
            Interm_ErrorMarg_H26 = model.Interm_ErrorMarg_H26;
        }
    }
}
