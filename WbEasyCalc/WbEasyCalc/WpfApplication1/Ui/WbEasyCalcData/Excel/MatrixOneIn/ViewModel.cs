using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.MatrixOneIn
{
    public class ViewModel : BaseSheetViewModel
    {
        private double _matOneIn_C11;
        public double MatOneIn_C11
        {
            get => _matOneIn_C11;
            set { _matOneIn_C11 = value; RaisePropertyChanged(nameof(MatOneIn_C11)); Calculate(); }
        }
        private double _matOneIn_C12;
        public double MatOneIn_C12
        {
            get => _matOneIn_C12;
            set { _matOneIn_C12 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_C13;
        public double MatOneIn_C13
        {
            get => _matOneIn_C13;
            set { _matOneIn_C13 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_C14;
        public double MatOneIn_C14
        {
            get => _matOneIn_C14;
            set { _matOneIn_C14 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_C21;
        public double MatOneIn_C21
        {
            get => _matOneIn_C21;
            set { _matOneIn_C21 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_C22;
        public double MatOneIn_C22
        {
            get => _matOneIn_C22;
            set { _matOneIn_C22 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_C23;
        public double MatOneIn_C23
        {
            get => _matOneIn_C23;
            set { _matOneIn_C23 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_C24;
        public double MatOneIn_C24
        {
            get => _matOneIn_C24;
            set { _matOneIn_C24 = value; RaisePropertyChanged(); }
        }


        private double _matOneIn_D21;
        public double MatOneIn_D21
        {
            get => _matOneIn_D21;
            set { _matOneIn_D21 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_D22;
        public double MatOneIn_D22
        {
            get => _matOneIn_D22;
            set { _matOneIn_D22 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_D23;
        public double MatOneIn_D23
        {
            get => _matOneIn_D23;
            set { _matOneIn_D23 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_D24;
        public double MatOneIn_D24
        {
            get => _matOneIn_D24;
            set { _matOneIn_D24 = value; RaisePropertyChanged(); }
        }


        private double _matOneIn_E11;
        public double MatOneIn_E11
        {
            get => _matOneIn_E11;
            set { _matOneIn_E11 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_E12;
        public double MatOneIn_E12
        {
            get => _matOneIn_E12;
            set { _matOneIn_E12 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_E13;
        public double MatOneIn_E13
        {
            get => _matOneIn_E13;
            set { _matOneIn_E13 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_E14;
        public double MatOneIn_E14
        {
            get => _matOneIn_E14;
            set { _matOneIn_E14 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_E21;
        public double MatOneIn_E21
        {
            get => _matOneIn_E21;
            set { _matOneIn_E21 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_E22;
        public double MatOneIn_E22
        {
            get => _matOneIn_E22;
            set { _matOneIn_E22 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_E23;
        public double MatOneIn_E23
        {
            get => _matOneIn_E23;
            set { _matOneIn_E23 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_E24;
        public double MatOneIn_E24
        {
            get => _matOneIn_E24;
            set { _matOneIn_E24 = value; RaisePropertyChanged(); }
        }



        private double _matOneIn_F11;
        public double MatOneIn_F11
        {
            get => _matOneIn_F11;
            set { _matOneIn_F11 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_F12;
        public double MatOneIn_F12
        {
            get => _matOneIn_F12;
            set { _matOneIn_F12 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_F13;
        public double MatOneIn_F13
        {
            get => _matOneIn_F13;
            set { _matOneIn_F13 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_F14;
        public double MatOneIn_F14
        {
            get => _matOneIn_F14;
            set { _matOneIn_F14 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_F21;
        public double MatOneIn_F21
        {
            get => _matOneIn_F21;
            set { _matOneIn_F21 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_F22;
        public double MatOneIn_F22
        {
            get => _matOneIn_F22;
            set { _matOneIn_F22 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_F23;
        public double MatOneIn_F23
        {
            get => _matOneIn_F23;
            set { _matOneIn_F23 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_F24;
        public double MatOneIn_F24
        {
            get => _matOneIn_F24;
            set { _matOneIn_F24 = value; RaisePropertyChanged(); }
        }


        private double _matOneIn_G11;
        public double MatOneIn_G11
        {
            get => _matOneIn_G11;
            set { _matOneIn_G11 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_G12;
        public double MatOneIn_G12
        {
            get => _matOneIn_G12;
            set { _matOneIn_G12 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_G13;
        public double MatOneIn_G13
        {
            get => _matOneIn_G13;
            set { _matOneIn_G13 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_G14;
        public double MatOneIn_G14
        {
            get => _matOneIn_G14;
            set { _matOneIn_G14 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_G21;
        public double MatOneIn_G21
        {
            get => _matOneIn_G21;
            set { _matOneIn_G21 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_G22;
        public double MatOneIn_G22
        {
            get => _matOneIn_G22;
            set { _matOneIn_G22 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_G23;
        public double MatOneIn_G23
        {
            get => _matOneIn_G23;
            set { _matOneIn_G23 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_G24;
        public double MatOneIn_G24
        {
            get => _matOneIn_G24;
            set { _matOneIn_G24 = value; RaisePropertyChanged(); }
        }


        private double _matOneIn_H11;
        public double MatOneIn_H11
        {
            get => _matOneIn_H11;
            set { _matOneIn_H11 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_H12;
        public double MatOneIn_H12
        {
            get => _matOneIn_H12;
            set { _matOneIn_H12 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_H13;
        public double MatOneIn_H13
        {
            get => _matOneIn_H13;
            set { _matOneIn_H13 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_H14;
        public double MatOneIn_H14
        {
            get => _matOneIn_H14;
            set { _matOneIn_H14 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_H21;
        public double MatOneIn_H21
        {
            get => _matOneIn_H21;
            set { _matOneIn_H21 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_H22;
        public double MatOneIn_H22
        {
            get => _matOneIn_H22;
            set { _matOneIn_H22 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_H23;
        public double MatOneIn_H23
        {
            get => _matOneIn_H23;
            set { _matOneIn_H23 = value; RaisePropertyChanged(); }
        }
        private double _matOneIn_H24;
        public double MatOneIn_H24
        {
            get => _matOneIn_H24;
            set { _matOneIn_H24 = value; RaisePropertyChanged(); }
        }


        public MatrixOneInModel Model => new MatrixOneInModel()
        {
            MatOneIn_C11 = MatOneIn_C11,
            MatOneIn_C12 = MatOneIn_C12,
            MatOneIn_C13 = MatOneIn_C13,
            MatOneIn_C14 = MatOneIn_C14,
            MatOneIn_C21 = MatOneIn_C21,
            MatOneIn_C22 = MatOneIn_C22,
            MatOneIn_C23 = MatOneIn_C23,
            MatOneIn_C24 = MatOneIn_C24,

            MatOneIn_D21 = MatOneIn_D21,
            MatOneIn_D22 = MatOneIn_D22,
            MatOneIn_D23 = MatOneIn_D23,
            MatOneIn_D24 = MatOneIn_D24,

            MatOneIn_E11 = MatOneIn_E11,
            MatOneIn_E12 = MatOneIn_E12,
            MatOneIn_E13 = MatOneIn_E13,
            MatOneIn_E14 = MatOneIn_E14,
            MatOneIn_E21 = MatOneIn_E21,
            MatOneIn_E22 = MatOneIn_E22,
            MatOneIn_E23 = MatOneIn_E23,
            MatOneIn_E24 = MatOneIn_E24,

            MatOneIn_F11 = MatOneIn_F11,
            MatOneIn_F12 = MatOneIn_F12,
            MatOneIn_F13 = MatOneIn_F13,
            MatOneIn_F14 = MatOneIn_F14,
            MatOneIn_F21 = MatOneIn_F21,
            MatOneIn_F22 = MatOneIn_F22,
            MatOneIn_F23 = MatOneIn_F23,
            MatOneIn_F24 = MatOneIn_F24,

            MatOneIn_G11 = MatOneIn_G11,
            MatOneIn_G12 = MatOneIn_G12,
            MatOneIn_G13 = MatOneIn_G13,
            MatOneIn_G14 = MatOneIn_G14,
            MatOneIn_G21 = MatOneIn_G21,
            MatOneIn_G22 = MatOneIn_G22,
            MatOneIn_G23 = MatOneIn_G23,
            MatOneIn_G24 = MatOneIn_G24,

            MatOneIn_H11 = MatOneIn_H11,
            MatOneIn_H12 = MatOneIn_H12,
            MatOneIn_H13 = MatOneIn_H13,
            MatOneIn_H14 = MatOneIn_H14,
            MatOneIn_H21 = MatOneIn_H21,
            MatOneIn_H22 = MatOneIn_H22,
            MatOneIn_H23 = MatOneIn_H23,
            MatOneIn_H24 = MatOneIn_H24,
        };

        public void Refreash(MatrixOneInModel model)
        {
            MatOneIn_C11 = model.MatOneIn_C11;
            MatOneIn_C12 = model.MatOneIn_C12;
            MatOneIn_C13 = model.MatOneIn_C13;
            MatOneIn_C14 = model.MatOneIn_C14;
            MatOneIn_C21 = model.MatOneIn_C21;
            MatOneIn_C22 = model.MatOneIn_C22;
            MatOneIn_C23 = model.MatOneIn_C23;
            MatOneIn_C24 = model.MatOneIn_C24;                                       
            MatOneIn_D21 = model.MatOneIn_D21;
            MatOneIn_D22 = model.MatOneIn_D22;
            MatOneIn_D23 = model.MatOneIn_D23;
            MatOneIn_D24 = model.MatOneIn_D24;                                       
            MatOneIn_E11 = model.MatOneIn_E11;
            MatOneIn_E12 = model.MatOneIn_E12;
            MatOneIn_E13 = model.MatOneIn_E13;
            MatOneIn_E14 = model.MatOneIn_E14;
            MatOneIn_E21 = model.MatOneIn_E21;
            MatOneIn_E22 = model.MatOneIn_E22;
            MatOneIn_E23 = model.MatOneIn_E23;
            MatOneIn_E24 = model.MatOneIn_E24;                                       
            MatOneIn_F11 = model.MatOneIn_F11;
            MatOneIn_F12 = model.MatOneIn_F12;
            MatOneIn_F13 = model.MatOneIn_F13;
            MatOneIn_F14 = model.MatOneIn_F14;
            MatOneIn_F21 = model.MatOneIn_F21;
            MatOneIn_F22 = model.MatOneIn_F22;
            MatOneIn_F23 = model.MatOneIn_F23;
            MatOneIn_F24 = model.MatOneIn_F24;                                       
            MatOneIn_G11 = model.MatOneIn_G11;
            MatOneIn_G12 = model.MatOneIn_G12;
            MatOneIn_G13 = model.MatOneIn_G13;
            MatOneIn_G14 = model.MatOneIn_G14;
            MatOneIn_G21 = model.MatOneIn_G21;
            MatOneIn_G22 = model.MatOneIn_G22;
            MatOneIn_G23 = model.MatOneIn_G23;
            MatOneIn_G24 = model.MatOneIn_G24;                                      
            MatOneIn_H11 = model.MatOneIn_H11;
            MatOneIn_H12 = model.MatOneIn_H12;
            MatOneIn_H13 = model.MatOneIn_H13;
            MatOneIn_H14 = model.MatOneIn_H14;
            MatOneIn_H21 = model.MatOneIn_H21;
            MatOneIn_H22 = model.MatOneIn_H22;
            MatOneIn_H23 = model.MatOneIn_H23;
            MatOneIn_H24 = model.MatOneIn_H24;
        }

        public ViewModel(MatrixOneInModel model)
        {
            if (model == null) return;

            // Input
            MatOneIn_C11 = model.MatOneIn_C11;
        }
    }
}
