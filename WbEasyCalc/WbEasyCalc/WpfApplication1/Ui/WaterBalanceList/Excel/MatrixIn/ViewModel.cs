using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WaterBalanceList.Excel.MatrixIn
{
    public class ViewModel : BaseSheetViewModel
    {


        private RadioOptions _selectedOption;
        public RadioOptions SelectedOption
        {
            get => _selectedOption;
            set { _selectedOption = value; RaisePropertyChanged(); Calculate(); }
        }



        private double _c11;
        public double C11
        {
            get => _c11;
            set { _c11 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _c12;
        public double C12
        {
            get => _c12;
            set { _c12 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _c13;
        public double C13
        {
            get => _c13;
            set { _c13 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _c14;
        public double C14
        {
            get => _c14;
            set { _c14 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _c21;
        public double C21
        {
            get => _c21;
            set { _c21 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _c22;
        public double C22
        {
            get => _c22;
            set { _c22 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _c23;
        public double C23
        {
            get => _c23;
            set { _c23 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _c24;
        public double C24
        {
            get => _c24;
            set { _c24 = value; RaisePropertyChanged(); Calculate(); }
        }


        private double _d21;
        public double D21
        {
            get => _d21;
            set { _d21 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _d22;
        public double D22
        {
            get => _d22;
            set { _d22 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _d23;
        public double D23
        {
            get => _d23;
            set { _d23 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _d24;
        public double D24
        {
            get => _d24;
            set { _d24 = value; RaisePropertyChanged(); Calculate(); }
        }


        private double _e11;
        public double E11
        {
            get => _e11;
            set { _e11 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _e12;
        public double E12
        {
            get => _e12;
            set { _e12 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _e13;
        public double E13
        {
            get => _e13;
            set { _e13 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _e14;
        public double E14
        {
            get => _e14;
            set { _e14 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _e21;
        public double E21
        {
            get => _e21;
            set { _e21 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _e22;
        public double E22
        {
            get => _e22;
            set { _e22 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _e23;
        public double E23
        {
            get => _e23;
            set { _e23 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _e24;
        public double E24
        {
            get => _e24;
            set { _e24 = value; RaisePropertyChanged(); Calculate(); }
        }



        private double _f11;
        public double F11
        {
            get => _f11;
            set { _f11 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _f12;
        public double F12
        {
            get => _f12;
            set { _f12 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _f13;
        public double F13
        {
            get => _f13;
            set { _f13 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _f14;
        public double F14
        {
            get => _f14;
            set { _f14 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _f21;
        public double F21
        {
            get => _f21;
            set { _f21 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _f22;
        public double F22
        {
            get => _f22;
            set { _f22 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _f23;
        public double F23
        {
            get => _f23;
            set { _f23 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _f24;
        public double F24
        {
            get => _f24;
            set { _f24 = value; RaisePropertyChanged(); Calculate(); }
        }


        private double _g11;
        public double G11
        {
            get => _g11;
            set { _g11 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _g12;
        public double G12
        {
            get => _g12;
            set { _g12 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _g13;
        public double G13
        {
            get => _g13;
            set { _g13 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _g14;
        public double G14
        {
            get => _g14;
            set { _g14 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _g21;
        public double G21
        {
            get => _g21;
            set { _g21 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _g22;
        public double G22
        {
            get => _g22;
            set { _g22 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _g23;
        public double G23
        {
            get => _g23;
            set { _g23 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _g24;
        public double G24
        {
            get => _g24;
            set { _g24 = value; RaisePropertyChanged(); Calculate(); }
        }


        private double _h11;
        public double H11
        {
            get => _h11;
            set { _h11 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _h12;
        public double H12
        {
            get => _h12;
            set { _h12 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _h13;
        public double H13
        {
            get => _h13;
            set { _h13 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _h14;
        public double H14
        {
            get => _h14;
            set { _h14 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _h21;
        public double H21
        {
            get => _h21;
            set { _h21 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _h22;
        public double H22
        {
            get => _h22;
            set { _h22 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _h23;
        public double H23
        {
            get => _h23;
            set { _h23 = value; RaisePropertyChanged(); Calculate(); }
        }
        private double _h24;
        public double H24
        {
            get => _h24;
            set { _h24 = value; RaisePropertyChanged(); Calculate(); }
        }


        public MatrixInModel Model => new MatrixInModel()
        {
            SelectedOption = (int)SelectedOption,

            C11 = C11,
            C12 = C12,
            C13 = C13,
            C14 = C14,
            C21 = C21,
            C22 = C22,
            C23 = C23,
            C24 = C24,
            D21 = D21,
            D22 = D22,
            D23 = D23,
            D24 = D24,
            E11 = E11,
            E12 = E12,
            E13 = E13,
            E14 = E14,
            E21 = E21,
            E22 = E22,
            E23 = E23,
            E24 = E24,
            F11 = F11,
            F12 = F12,
            F13 = F13,
            F14 = F14,
            F21 = F21,
            F22 = F22,
            F23 = F23,
            F24 = F24,
            G11 = G11,
            G12 = G12,
            G13 = G13,
            G14 = G14,
            G21 = G21,
            G22 = G22,
            G23 = G23,
            G24 = G24,
            H11 = H11,
            H12 = H12,
            H13 = H13,
            H14 = H14,
            H21 = H21,
            H22 = H22,
            H23 = H23,
            H24 = H24,
        };

        public void Refreash(MatrixInModel model)
        {
            C11 = model.C11;
            C12 = model.C12;
            C13 = model.C13;
            C14 = model.C14;
            C21 = model.C21;
            C22 = model.C22;
            C23 = model.C23;
            C24 = model.C24;
            D21 = model.D21;
            D22 = model.D22;
            D23 = model.D23;
            D24 = model.D24;
            E11 = model.E11;
            E12 = model.E12;
            E13 = model.E13;
            E14 = model.E14;
            E21 = model.E21;
            E22 = model.E22;
            E23 = model.E23;
            E24 = model.E24;
            F11 = model.F11;
            F12 = model.F12;
            F13 = model.F13;
            F14 = model.F14;
            F21 = model.F21;
            F22 = model.F22;
            F23 = model.F23;
            F24 = model.F24;
            G11 = model.G11;
            G12 = model.G12;
            G13 = model.G13;
            G14 = model.G14;
            G21 = model.G21;
            G22 = model.G22;
            G23 = model.G23;
            G24 = model.G24;
            H11 = model.H11;
            H12 = model.H12;
            H13 = model.H13;
            H14 = model.H14;
            H21 = model.H21;
            H22 = model.H22;
            H23 = model.H23;
            H24 = model.H24;
        }

        public ViewModel(MatrixInModel model)
        {
            if (model == null) return;

            // Input
            SelectedOption = (RadioOptions)model.SelectedOption;
            C11 = model.C11;
            C12 = model.C12;
            C13 = model.C13;
            C14 = model.C14;
            C21 = model.C21;
            C22 = model.C22;
            C23 = model.C23;
            C24 = model.C24;
            D21 = model.D21;
            D22 = model.D22;
            D23 = model.D23;
            D24 = model.D24;
            E11 = model.E11;
            E12 = model.E12;
            E13 = model.E13;
            E14 = model.E14;
            E21 = model.E21;
            E22 = model.E22;
            E23 = model.E23;
            E24 = model.E24;
            F11 = model.F11;
            F12 = model.F12;
            F13 = model.F13;
            F14 = model.F14;
            F21 = model.F21;
            F22 = model.F22;
            F23 = model.F23;
            F24 = model.F24;
            G11 = model.G11;
            G12 = model.G12;
            G13 = model.G13;
            G14 = model.G14;
            G21 = model.G21;
            G22 = model.G22;
            G23 = model.G23;
            G24 = model.G24;
            H11 = model.H11;
            H12 = model.H12;
            H13 = model.H13;
            H14 = model.H14;
            H21 = model.H21;
            H22 = model.H22;
            H23 = model.H23;
            H24 = model.H24;
        }
    }
}
