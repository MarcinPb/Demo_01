using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.MatrixOut
{
    public class ViewModel : BaseSheetViewModel
    {
        private string _c11;
        public string C11
        {
            get => _c11;
            set { _c11 = value; RaisePropertyChanged(); }
        }
        private string _c12;
        public string C12
        {
            get => _c12;
            set { _c12 = value; RaisePropertyChanged(); }
        }
        private string _c13;
        public string C13
        {
            get => _c13;
            set { _c13 = value; RaisePropertyChanged(); }
        }
        private string _c14;
        public string C14
        {
            get => _c14;
            set { _c14 = value; RaisePropertyChanged(); }
        }
        private string _c15;
        public string C15
        {
            get => _c15;
            set { _c15 = value; RaisePropertyChanged(); }
        }
        private string _c21;
        public string C21
        {
            get => _c21;
            set { _c21 = value; RaisePropertyChanged(); }
        }
        private string _c22;
        public string C22
        {
            get => _c22;
            set { _c22 = value; RaisePropertyChanged(); }
        }
        private string _c23;
        public string C23
        {
            get => _c23;
            set { _c23 = value; RaisePropertyChanged(); }
        }
        private string _c24;
        public string C24
        {
            get => _c24;
            set { _c24 = value; RaisePropertyChanged(); }
        }
        private string _c25;
        public string C25
        {
            get => _c25;
            set { _c25 = value; RaisePropertyChanged(); }
        }


        private string _d21;
        public string D21
        {
            get => _d21;
            set { _d21 = value; RaisePropertyChanged(); }
        }
        private string _d22;
        public string D22
        {
            get => _d22;
            set { _d22 = value; RaisePropertyChanged(); }
        }
        private string _d23;
        public string D23
        {
            get => _d23;
            set { _d23 = value; RaisePropertyChanged(); }
        }
        private string _d24;
        public string D24
        {
            get => _d24;
            set { _d24 = value; RaisePropertyChanged(); }
        }
        private string _d25;
        public string D25
        {
            get => _d25;
            set { _d25 = value; RaisePropertyChanged(); }
        }


        private string _e11;
        public string E11
        {
            get => _e11;
            set { _e11 = value; RaisePropertyChanged(); }
        }
        private string _e12;
        public string E12
        {
            get => _e12;
            set { _e12 = value; RaisePropertyChanged(); }
        }
        private string _e13;
        public string E13
        {
            get => _e13;
            set { _e13 = value; RaisePropertyChanged(); }
        }
        private string _e14;
        public string E14
        {
            get => _e14;
            set { _e14 = value; RaisePropertyChanged(); }
        }
        private string _e15;
        public string E15
        {
            get => _e15;
            set { _e15 = value; RaisePropertyChanged(); }
        }
        private string _e21;
        public string E21
        {
            get => _e21;
            set { _e21 = value; RaisePropertyChanged(); }
        }
        private string _e22;
        public string E22
        {
            get => _e22;
            set { _e22 = value; RaisePropertyChanged(); }
        }
        private string _e23;
        public string E23
        {
            get => _e23;
            set { _e23 = value; RaisePropertyChanged(); }
        }
        private string _e24;
        public string E24
        {
            get => _e24;
            set { _e24 = value; RaisePropertyChanged(); }
        }
        private string _e25;
        public string E25
        {
            get => _e25;
            set { _e25 = value; RaisePropertyChanged(); }
        }



        private string _f11;
        public string F11
        {
            get => _f11;
            set { _f11 = value; RaisePropertyChanged(); }
        }
        private string _f12;
        public string F12
        {
            get => _f12;
            set { _f12 = value; RaisePropertyChanged(); }
        }
        private string _f13;
        public string F13
        {
            get => _f13;
            set { _f13 = value; RaisePropertyChanged(); }
        }
        private string _f14;
        public string F14
        {
            get => _f14;
            set { _f14 = value; RaisePropertyChanged(); }
        }
        private string _f15;
        public string F15
        {
            get => _f15;
            set { _f15 = value; RaisePropertyChanged(); }
        }
        private string _f21;
        public string F21
        {
            get => _f21;
            set { _f21 = value; RaisePropertyChanged(); }
        }
        private string _f22;
        public string F22
        {
            get => _f22;
            set { _f22 = value; RaisePropertyChanged(); }
        }
        private string _f23;
        public string F23
        {
            get => _f23;
            set { _f23 = value; RaisePropertyChanged(); }
        }
        private string _f24;
        public string F24
        {
            get => _f24;
            set { _f24 = value; RaisePropertyChanged(); }
        }
        private string _f25;
        public string F25
        {
            get => _f25;
            set { _f25 = value; RaisePropertyChanged(); }
        }


        private string _g11;
        public string G11
        {
            get => _g11;
            set { _g11 = value; RaisePropertyChanged(); }
        }
        private string _g12;
        public string G12
        {
            get => _g12;
            set { _g12 = value; RaisePropertyChanged(); }
        }
        private string _g13;
        public string G13
        {
            get => _g13;
            set { _g13 = value; RaisePropertyChanged(); }
        }
        private string _g14;
        public string G14
        {
            get => _g14;
            set { _g14 = value; RaisePropertyChanged(); }
        }
        private string _g15;
        public string G15
        {
            get => _g15;
            set { _g15 = value; RaisePropertyChanged(); }
        }
        private string _g21;
        public string G21
        {
            get => _g21;
            set { _g21 = value; RaisePropertyChanged(); }
        }
        private string _g22;
        public string G22
        {
            get => _g22;
            set { _g22 = value; RaisePropertyChanged(); }
        }
        private string _g23;
        public string G23
        {
            get => _g23;
            set { _g23 = value; RaisePropertyChanged(); }
        }
        private string _g24;
        public string G24
        {
            get => _g24;
            set { _g24 = value; RaisePropertyChanged(); }
        }
        private string _g25;
        public string G25
        {
            get => _g25;
            set { _g25 = value; RaisePropertyChanged(); }
        }


        private string _h11;
        public string H11
        {
            get => _h11;
            set { _h11 = value; RaisePropertyChanged(); }
        }
        private string _h12;
        public string H12
        {
            get => _h12;
            set { _h12 = value; RaisePropertyChanged(); }
        }
        private string _h13;
        public string H13
        {
            get => _h13;
            set { _h13 = value; RaisePropertyChanged(); }
        }
        private string _h14;
        public string H14
        {
            get => _h14;
            set { _h14 = value; RaisePropertyChanged(); }
        }
        private string _h15;
        public string H15
        {
            get => _h15;
            set { _h15 = value; RaisePropertyChanged(); }
        }
        private string _h21;
        public string H21
        {
            get => _h21;
            set { _h21 = value; RaisePropertyChanged(); }
        }
        private string _h22;
        public string H22
        {
            get => _h22;
            set { _h22 = value; RaisePropertyChanged(); }
        }
        private string _h23;
        public string H23
        {
            get => _h23;
            set { _h23 = value; RaisePropertyChanged(); }
        }
        private string _h24;
        public string H24
        {
            get => _h24;
            set { _h24 = value; RaisePropertyChanged(); }
        }
        private string _h25;
        public string H25
        {
            get => _h25;
            set { _h25 = value; RaisePropertyChanged(); }
        }








        public MatrixOutModel Model => new MatrixOutModel()
        {
            C11 = C11,
            C12 = C12,
            C13 = C13,
            C14 = C14,
            C15 = C15,
            C21 = C21,
            C22 = C22,
            C23 = C23,
            C24 = C24,
            C25 = C25,
            D21 = D21,
            D22 = D22,
            D23 = D23,
            D24 = D24,
            D25 = D25,
            E11 = E11,
            E12 = E12,
            E13 = E13,
            E14 = E14,
            E15 = E15,
            E21 = E21,
            E22 = E22,
            E23 = E23,
            E24 = E24,
            E25 = E25,
            F11 = F11,
            F12 = F12,
            F13 = F13,
            F14 = F14,
            F15 = F15,
            F21 = F21,
            F22 = F22,
            F23 = F23,
            F24 = F24,
            F25 = F25,
            G11 = G11,
            G12 = G12,
            G13 = G13,
            G14 = G14,
            G15 = G15,
            G21 = G21,
            G22 = G22,
            G23 = G23,
            G24 = G24,
            G25 = G25,
            H11 = H11,
            H12 = H12,
            H13 = H13,
            H14 = H14,
            H15 = H15,
            H21 = H21,
            H22 = H22,
            H23 = H23,
            H24 = H24,
            H25 = H25,
        };

        internal void Refreash(MatrixOutModel model)
        {
            C11 = model.C11;
            C12 = model.C12;
            C13 = model.C13;
            C14 = model.C14;
            C15 = model.C15;
            C21 = model.C21;
            C22 = model.C22;
            C23 = model.C23;
            C24 = model.C24;
            C25 = model.C25;
            D21 = model.D21;
            D22 = model.D22;
            D23 = model.D23;
            D24 = model.D24;
            D25 = model.D25;
            E11 = model.E11;
            E12 = model.E12;
            E13 = model.E13;
            E14 = model.E14;
            E15 = model.E15;
            E21 = model.E21;
            E22 = model.E22;
            E23 = model.E23;
            E24 = model.E24;
            E25 = model.E25;
            F11 = model.F11;
            F12 = model.F12;
            F13 = model.F13;
            F14 = model.F14;
            F15 = model.F15;
            F21 = model.F21;
            F22 = model.F22;
            F23 = model.F23;
            F24 = model.F24;
            F25 = model.F25;
            G11 = model.G11;
            G12 = model.G12;
            G13 = model.G13;
            G14 = model.G14;
            G15 = model.G15;
            G21 = model.G21;
            G22 = model.G22;
            G23 = model.G23;
            G24 = model.G24;
            G25 = model.G25;
            H11 = model.H11;
            H12 = model.H12;
            H13 = model.H13;
            H14 = model.H14;
            H15 = model.H15;
            H21 = model.H21;
            H22 = model.H22;
            H23 = model.H23;
            H24 = model.H24;
            H25 = model.H25;
        }

        public ViewModel(MatrixOutModel model)
        {
            if (model == null) return;
        }
    }
}
