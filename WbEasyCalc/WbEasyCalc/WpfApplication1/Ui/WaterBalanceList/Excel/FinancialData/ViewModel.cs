using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Ui.WbEasyCalcData.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.FinancialData
{
    public class ViewModel : BaseSheetViewModel
    {
        #region Input props

        private double _financData_G6;
        public double FinancData_G6
        {
            get => _financData_G6;
            set { _financData_G6 = value; RaisePropertyChanged(nameof(FinancData_G6)); Calculate(); }
        }
        private string _financData_K6;
        public string FinancData_K6
        {
            get => _financData_K6;
            set { _financData_K6 = value; RaisePropertyChanged(nameof(FinancData_K6)); Calculate(); }
        }
        private double _financData_G8;
        public double FinancData_G8
        {
            get => _financData_G8;
            set { _financData_G8 = value; RaisePropertyChanged(nameof(FinancData_G8)); Calculate(); }
        }
        private double _financData_D26;
        public double FinancData_D26
        {
            get => _financData_D26;
            set { _financData_D26 = value; RaisePropertyChanged(nameof(FinancData_D26)); Calculate(); }
        }
        private double _financData_G35;
        public double FinancData_G35
        {
            get => _financData_G35;
            set { _financData_G35 = value; RaisePropertyChanged(nameof(FinancData_G35)); Calculate(); }
        }

        #endregion

        #region Output props

        private double _financData_G13;
        public double FinancData_G13
        {
            get => _financData_G13;
            set { _financData_G13 = value; RaisePropertyChanged(nameof(FinancData_G13)); }
        }
        private double _financData_G15;
        public double FinancData_G15
        {
            get => _financData_G15;
            set { _financData_G15 = value; RaisePropertyChanged(nameof(FinancData_G15)); }
        }
        private double _financData_G17;
        public double FinancData_G17
        {
            get => _financData_G17;
            set { _financData_G17 = value; RaisePropertyChanged(nameof(FinancData_G17)); }
        }
        private double _financData_G19;
        public double FinancData_G19
        {
            get => _financData_G19;
            set { _financData_G19 = value; RaisePropertyChanged(nameof(FinancData_G19)); }
        }
        private double _financData_G20;
        public double FinancData_G20
        {
            get => _financData_G20;
            set { _financData_G20 = value; RaisePropertyChanged(nameof(FinancData_G20)); }
        }
        private double _financData_G22;
        public double FinancData_G22
        {
            get => _financData_G22;
            set { _financData_G22 = value; RaisePropertyChanged(nameof(FinancData_G22)); }
        }
        private double _financData_D24;
        public double FinancData_D24
        {
            get => _financData_D24;
            set { _financData_D24 = value; RaisePropertyChanged(nameof(FinancData_D24)); }
        }
        private double _financData_G31;
        public double FinancData_G31
        {
            get => _financData_G31;
            set { _financData_G31 = value; RaisePropertyChanged(nameof(FinancData_G31)); }
        }

        private string _financData_K8;
        public string FinancData_K8
        {
            get => _financData_K8;
            set { _financData_K8 = value; RaisePropertyChanged(nameof(FinancData_K8)); }
        }
        private string _financData_K13;
        public string FinancData_K13
        {
            get => _financData_K13;
            set { _financData_K13 = value; RaisePropertyChanged(nameof(FinancData_K13)); }
        }
        private string _financData_K15;
        public string FinancData_K15
        {
            get => _financData_K15;
            set { _financData_K15 = value; RaisePropertyChanged(nameof(FinancData_K15)); }
        }
        private string _financData_K17;
        public string FinancData_K17
        {
            get => _financData_K17;
            set { _financData_K17 = value; RaisePropertyChanged(nameof(FinancData_K17)); }
        }
        private string _financData_K19;
        public string FinancData_K19
        {
            get => _financData_K19;
            set { _financData_K19 = value; RaisePropertyChanged(nameof(FinancData_K19)); }
        }
        private string _financData_K20;
        public string FinancData_K20
        {
            get => _financData_K20;
            set { _financData_K20 = value; RaisePropertyChanged(nameof(FinancData_K20)); }
        }
        private string _financData_K22;
        public string FinancData_K22
        {
            get => _financData_K22;
            set { _financData_K22 = value; RaisePropertyChanged(nameof(FinancData_K22)); }
        }
        private string _financData_K31;
        public string FinancData_K31
        {
            get => _financData_K31;
            set { _financData_K31 = value; RaisePropertyChanged(nameof(FinancData_K31)); }
        }
        private string _financData_K35;
        public string FinancData_K35
        {
            get => _financData_K35;
            set { _financData_K35 = value; RaisePropertyChanged(nameof(FinancData_K35)); }
        }


        #endregion

        public FinancDataModel Model => new FinancDataModel()
        {
            // Input
            FinancData_G6 = FinancData_G6,
            FinancData_K6 = FinancData_K6,
            FinancData_G8 = FinancData_G8,
            FinancData_D26 = FinancData_D26,
            FinancData_G35 = FinancData_G35,
            // Output
            FinancData_G13 = FinancData_G13,
            FinancData_G15 = FinancData_G15,
            FinancData_G17 = FinancData_G17,
            FinancData_G19 = FinancData_G19,
            FinancData_G20 = FinancData_G20,
            FinancData_G22 = FinancData_G22,
            FinancData_D24 = FinancData_D24,
            FinancData_G31 = FinancData_G31,
            FinancData_K8 = FinancData_K8,
            FinancData_K13 = FinancData_K13,
            FinancData_K15 = FinancData_K15,
            FinancData_K17 = FinancData_K17,
            FinancData_K19 = FinancData_K19,
            FinancData_K20 = FinancData_K20,
            FinancData_K22 = FinancData_K22,
            FinancData_K31 = FinancData_K31,
            FinancData_K35 = FinancData_K35,
        };

        internal void Refreash(FinancDataModel model)
        {
            FinancData_G13 = model.FinancData_G13;
            FinancData_G15 = model.FinancData_G15;
            FinancData_G17 = model.FinancData_G17;
            FinancData_G19 = model.FinancData_G19;
            FinancData_G20 = model.FinancData_G20;
            FinancData_G22 = model.FinancData_G22;
            FinancData_D24 = model.FinancData_D24;
            FinancData_G31 = model.FinancData_G31;
            FinancData_K8 = model.FinancData_K8;
            FinancData_K13 = model.FinancData_K13;
            FinancData_K15 = model.FinancData_K15;
            FinancData_K17 = model.FinancData_K17;
            FinancData_K19 = model.FinancData_K19;
            FinancData_K20 = model.FinancData_K20;
            FinancData_K22 = model.FinancData_K22;
            FinancData_K31 = model.FinancData_K31;
            FinancData_K35 = model.FinancData_K35;
        }
        public ViewModel(FinancDataModel model)
        {
            if (model == null) return;

            // Input
            FinancData_G6 = model.FinancData_G6;
            FinancData_K6 = model.FinancData_K6;
            FinancData_G8 = model.FinancData_G8;
            FinancData_D26 = model.FinancData_D26;
            FinancData_G35 = model.FinancData_G35;
        }
    }
}
