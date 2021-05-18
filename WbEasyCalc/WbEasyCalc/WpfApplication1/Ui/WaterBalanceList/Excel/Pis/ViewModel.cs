using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.Pis 
{
    public class ViewModel : BaseSheetViewModel
    {
        private double _pis_F9;
        public double Pis_F9
        {
            get => _pis_F9;
            set { _pis_F9 = value; RaisePropertyChanged(nameof(Pis_F9)); }
        }
        private double _pis_H9;
        public double Pis_H9
        {
            get => _pis_H9;
            set { _pis_H9 = value; RaisePropertyChanged(nameof(Pis_H9)); }
        }
        private double _pis_J9;
        public double Pis_J9
        {
            get => _pis_J9;
            set { _pis_J9 = value; RaisePropertyChanged(nameof(Pis_J9)); }
        }
        private double _pis_L9;
        public double Pis_L9
        {
            get => _pis_L9;
            set { _pis_L9 = value; RaisePropertyChanged(nameof(Pis_L9)); }
        }

        private double _pis_F11;
        public double Pis_F11
        {
            get => _pis_F11;
            set { _pis_F11 = value; RaisePropertyChanged(nameof(Pis_F11)); }
        }
        private double _pis_H11;
        public double Pis_H11
        {
            get => _pis_H11;
            set { _pis_H11 = value; RaisePropertyChanged(nameof(Pis_H11)); }
        }
        private double _pis_J11;
        public double Pis_J11
        {
            get => _pis_J11;
            set { _pis_J11 = value; RaisePropertyChanged(nameof(Pis_J11)); }
        }
        private double _pis_L11;
        public double Pis_L11
        {
            get => _pis_L11;
            set { _pis_L11 = value; RaisePropertyChanged(nameof(Pis_L11)); }
        }

        private double _pis_F17;
        public double Pis_F17
        {
            get => _pis_F17;
            set { _pis_F17 = value; RaisePropertyChanged(nameof(Pis_F17)); }
        }
        private double _pis_H17;
        public double Pis_H17
        {
            get => _pis_H17;
            set { _pis_H17 = value; RaisePropertyChanged(nameof(Pis_H17)); }
        }
        private double _pis_J17;
        public double Pis_J17
        {
            get => _pis_J17;
            set { _pis_J17 = value; RaisePropertyChanged(nameof(Pis_J17)); }
        }
        private double _pis_L17;
        public double Pis_L17
        {
            get => _pis_L17;
            set { _pis_L17 = value; RaisePropertyChanged(nameof(Pis_L17)); }
        }

        private double _pis_F19;
        public double Pis_F19
        {
            get => _pis_F19;
            set { _pis_F19 = value; RaisePropertyChanged(nameof(Pis_F19)); }
        }
        private double _pis_H19;
        public double Pis_H19
        {
            get => _pis_H19;
            set { _pis_H19 = value; RaisePropertyChanged(nameof(Pis_H19)); }
        }
        private double _pis_J19;
        public double Pis_J19
        {
            get => _pis_J19;
            set { _pis_J19 = value; RaisePropertyChanged(nameof(Pis_J19)); }
        }
        private double _pis_L19;
        public double Pis_L19
        {
            get => _pis_L19;
            set { _pis_L19 = value; RaisePropertyChanged(nameof(Pis_L19)); }
        }

        private double _pis_F25;
        public double Pis_F25
        {
            get => _pis_F25;
            set { _pis_F25 = value; RaisePropertyChanged(nameof(Pis_F25)); }
        }
        private double _pis_H25;
        public double Pis_H25
        {
            get => _pis_H25;
            set { _pis_H25 = value; RaisePropertyChanged(nameof(Pis_H25)); }
        }
        private double _pis_J25;
        public double Pis_J25
        {
            get => _pis_J25;
            set { _pis_J25 = value; RaisePropertyChanged(nameof(Pis_J25)); }
        }
        private double _pis_L25;
        public double Pis_L25
        {
            get => _pis_L25;
            set { _pis_L25 = value; RaisePropertyChanged(nameof(Pis_L25)); }
        }

        private double _pis_F27;
        public double Pis_F27
        {
            get => _pis_F27;
            set { _pis_F27 = value; RaisePropertyChanged(nameof(Pis_F27)); }
        }
        private double _pis_H27;
        public double Pis_H27
        {
            get => _pis_H27;
            set { _pis_H27 = value; RaisePropertyChanged(nameof(Pis_H27)); }
        }
        private double _pis_J27;
        public double Pis_J27
        {
            get => _pis_J27;
            set { _pis_J27 = value; RaisePropertyChanged(nameof(Pis_J27)); }
        }
        private double _pis_L27;
        public double Pis_L27
        {
            get => _pis_L27;
            set { _pis_L27 = value; RaisePropertyChanged(nameof(Pis_L27)); }
        }

        private double _pis_F29;
        public double Pis_F29
        {
            get => _pis_F29;
            set { _pis_F29 = value; RaisePropertyChanged(nameof(Pis_F29)); }
        }
        private double _pis_H29;
        public double Pis_H29
        {
            get => _pis_H29;
            set { _pis_H29 = value; RaisePropertyChanged(nameof(Pis_H29)); }
        }
        private double _pis_J29;
        public double Pis_J29
        {
            get => _pis_J29;
            set { _pis_J29 = value; RaisePropertyChanged(nameof(Pis_J29)); }
        }
        private double _pis_L29;
        public double Pis_L29
        {
            get => _pis_L29;
            set { _pis_L29 = value; RaisePropertyChanged(nameof(Pis_L29)); }
        }

        private double _pis_F31;
        public double Pis_F31
        {
            get => _pis_F31;
            set { _pis_F31 = value; RaisePropertyChanged(nameof(Pis_F31)); }
        }
        private double _pis_H31;
        public double Pis_H31
        {
            get => _pis_H31;
            set { _pis_H31 = value; RaisePropertyChanged(nameof(Pis_H31)); }
        }
        private double _pis_J31;

        public double Pis_J31
        {
            get => _pis_J31;
            set { _pis_J31 = value; RaisePropertyChanged(nameof(Pis_J31)); }
        }
        private double _pis_L31;
        public double Pis_L31
        {
            get => _pis_L31;
            set { _pis_L31 = value; RaisePropertyChanged(nameof(Pis_L31)); }
        }

        private double _pis_F37;
        public double Pis_F37
        {
            get => _pis_F37;
            set { _pis_F37 = value; RaisePropertyChanged(nameof(Pis_F37)); }
        }
        private double _pis_H37;
        public double Pis_H37
        {
            get => _pis_H37;
            set { _pis_H37 = value; RaisePropertyChanged(nameof(Pis_H37)); }
        }
        private double _pis_J37;
        public double Pis_J37
        {
            get => _pis_J37;
            set { _pis_J37 = value; RaisePropertyChanged(nameof(Pis_J37)); }
        }
        private double _pis_L37;
        public double Pis_L37
        {
            get => _pis_L37;
            set { _pis_L37 = value; RaisePropertyChanged(nameof(Pis_L37)); }
        }

        private double _pis_F39;
        public double Pis_F39
        {
            get => _pis_F39;
            set { _pis_F39 = value; RaisePropertyChanged(nameof(Pis_F39)); }
        }
        private double _pis_H39;
        public double Pis_H39
        {
            get => _pis_H39;
            set { _pis_H39 = value; RaisePropertyChanged(nameof(Pis_H39)); }
        }
        private double _pis_J39;
        public double Pis_J39
        {
            get => _pis_J39;
            set { _pis_J39 = value; RaisePropertyChanged(nameof(Pis_J39)); }
        }
        private double _pis_L39;
        public double Pis_L39
        {
            get => _pis_L39;
            set { _pis_L39 = value; RaisePropertyChanged(nameof(Pis_L39)); }
        }

        private double _pis_F41;
        public double Pis_F41
        {
            get => _pis_F41;
            set { _pis_F41 = value; RaisePropertyChanged(nameof(Pis_F41)); }
        }
        private double _pis_H41;
        public double Pis_H41
        {
            get => _pis_H41;
            set { _pis_H41 = value; RaisePropertyChanged(nameof(Pis_H41)); }
        }
        private double _pis_J41;
        public double Pis_J41
        {
            get => _pis_J41;
            set { _pis_J41 = value; RaisePropertyChanged(nameof(Pis_J41)); }
        }
        private double _pis_L41;
        public double Pis_L41
        {
            get => _pis_L41;
            set { _pis_L41 = value; RaisePropertyChanged(nameof(Pis_L41)); }
        }

        private double _pis_F47;
        public double Pis_F47
        {
            get => _pis_F47;
            set { _pis_F47 = value; RaisePropertyChanged(nameof(Pis_F47)); }
        }
        private double _pis_H47;
        public double Pis_H47
        {
            get => _pis_H47;
            set { _pis_H47 = value; RaisePropertyChanged(nameof(Pis_H47)); }
        }
        private double _pis_J47;
        public double Pis_J47
        {
            get => _pis_J47;
            set { _pis_J47 = value; RaisePropertyChanged(nameof(Pis_J47)); }
        }
        private double _pis_L47;
        public double Pis_L47
        {
            get => _pis_L47;
            set { _pis_L47 = value; RaisePropertyChanged(nameof(Pis_L47)); }
        }

        private double _pis_F49;
        public double Pis_F49
        {
            get => _pis_F49;
            set { _pis_F49 = value; RaisePropertyChanged(nameof(Pis_F49)); }
        }
        private double _pis_H49;
        public double Pis_H49
        {
            get => _pis_H49;
            set { _pis_H49 = value; RaisePropertyChanged(nameof(Pis_H49)); }
        }
        private double _pis_J49;
        public double Pis_J49
        {
            get => _pis_J49;
            set { _pis_J49 = value; RaisePropertyChanged(nameof(Pis_J49)); }
        }
        private double _pis_L49;
        public double Pis_L49
        {
            get => _pis_L49;
            set { _pis_L49 = value; RaisePropertyChanged(nameof(Pis_L49)); }
        }

        private double _pis_F51;
        public double Pis_F51
        {
            get => _pis_F51;
            set { _pis_F51 = value; RaisePropertyChanged(nameof(Pis_F51)); }
        }
        private double _pis_H51;
        public double Pis_H51
        {
            get => _pis_H51;
            set { _pis_H51 = value; RaisePropertyChanged(nameof(Pis_H51)); }
        }
        private double _pis_J51;
        public double Pis_J51
        {
            get => _pis_J51;
            set { _pis_J51 = value; RaisePropertyChanged(nameof(Pis_J51)); }
        }
        private double _pis_L51;
        public double Pis_L51
        {
            get => _pis_L51;
            set { _pis_L51 = value; RaisePropertyChanged(nameof(Pis_L51)); }
        }


        private string _pis_N27;
        public string Pis_N27
        {
            get => _pis_N27;
            set { _pis_N27 = value; RaisePropertyChanged(nameof(Pis_N27)); }
        }

        private string _pis_P27;
        public string Pis_P27
        {
            get => _pis_P27;
            set { _pis_P27 = value; RaisePropertyChanged(nameof(Pis_P27)); }
        }
        private string _pis_N47;
        public string Pis_N47
        {
            get => _pis_N47;
            set { _pis_N47 = value; RaisePropertyChanged(nameof(Pis_N47)); }
        }

        private string _pis_P47;
        public string Pis_P47
        {
            get => _pis_P47;
            set { _pis_P47 = value; RaisePropertyChanged(nameof(Pis_P47)); }
        }

        public PisModel Model => new PisModel()
        {
            Pis_F9 =  Pis_F9 ,
            Pis_H9 =  Pis_H9 ,
            Pis_J9 =  Pis_J9 ,
            Pis_L9 =  Pis_L9 ,
            Pis_F11 = Pis_F11,
            Pis_H11 = Pis_H11,
            Pis_J11 = Pis_J11,
            Pis_L11 = Pis_L11,
            Pis_F17 = Pis_F17,
            Pis_H17 = Pis_H17,
            Pis_J17 = Pis_J17,
            Pis_L17 = Pis_L17,
            Pis_F19 = Pis_F19,
            Pis_H19 = Pis_H19,
            Pis_J19 = Pis_J19,
            Pis_L19 = Pis_L19,
            Pis_F25 = Pis_F25,
            Pis_H25 = Pis_H25,
            Pis_J25 = Pis_J25,
            Pis_L25 = Pis_L25,
            Pis_F27 = Pis_F27,
            Pis_H27 = Pis_H27,
            Pis_J27 = Pis_J27,
            Pis_L27 = Pis_L27,
            Pis_F29 = Pis_F29,
            Pis_H29 = Pis_H29,
            Pis_J29 = Pis_J29,
            Pis_L29 = Pis_L29,
            Pis_F31 = Pis_F31,
            Pis_H31 = Pis_H31,
            Pis_J31 = Pis_J31,
            Pis_L31 = Pis_L31,
            Pis_F37 = Pis_F37,
            Pis_H37 = Pis_H37,
            Pis_J37 = Pis_J37,
            Pis_L37 = Pis_L37,
            Pis_F39 = Pis_F39,
            Pis_H39 = Pis_H39,
            Pis_J39 = Pis_J39,
            Pis_L39 = Pis_L39,
            Pis_F41 = Pis_F41,
            Pis_H41 = Pis_H41,
            Pis_J41 = Pis_J41,
            Pis_L41 = Pis_L41,
            Pis_F47 = Pis_F47,
            Pis_H47 = Pis_H47,
            Pis_J47 = Pis_J47,
            Pis_L47 = Pis_L47,
            Pis_F49 = Pis_F49,
            Pis_H49 = Pis_H49,
            Pis_J49 = Pis_J49,
            Pis_L49 = Pis_L49,
            Pis_F51 = Pis_F51,
            Pis_H51 = Pis_H51,
            Pis_J51 = Pis_J51,
            Pis_L51 = Pis_L51,
                             
            Pis_N27 = Pis_N27,
            Pis_P27 = Pis_P27,
            Pis_N47 = Pis_N47,
            Pis_P47 = Pis_P47,
        };

        internal void Refreash(PisModel model)
        {
            Pis_F9 = model.Pis_F9;
            Pis_H9 = model.Pis_H9;
            Pis_J9 = model.Pis_J9;
            Pis_L9 = model.Pis_L9;
            Pis_F11 = model.Pis_F11;
            Pis_H11 = model.Pis_H11;
            Pis_J11 = model.Pis_J11;
            Pis_L11 = model.Pis_L11;
            Pis_F17 = model.Pis_F17;
            Pis_H17 = model.Pis_H17;
            Pis_J17 = model.Pis_J17;
            Pis_L17 = model.Pis_L17;
            Pis_F19 = model.Pis_F19;
            Pis_H19 = model.Pis_H19;
            Pis_J19 = model.Pis_J19;
            Pis_L19 = model.Pis_L19;
            Pis_F25 = model.Pis_F25;
            Pis_H25 = model.Pis_H25;
            Pis_J25 = model.Pis_J25;
            Pis_L25 = model.Pis_L25;
            Pis_F27 = model.Pis_F27;
            Pis_H27 = model.Pis_H27;
            Pis_J27 = model.Pis_J27;
            Pis_L27 = model.Pis_L27;
            Pis_F29 = model.Pis_F29;
            Pis_H29 = model.Pis_H29;
            Pis_J29 = model.Pis_J29;
            Pis_L29 = model.Pis_L29;
            Pis_F31 = model.Pis_F31;
            Pis_H31 = model.Pis_H31;
            Pis_J31 = model.Pis_J31;
            Pis_L31 = model.Pis_L31;
            Pis_F37 = model.Pis_F37;
            Pis_H37 = model.Pis_H37;
            Pis_J37 = model.Pis_J37;
            Pis_L37 = model.Pis_L37;
            Pis_F39 = model.Pis_F39;
            Pis_H39 = model.Pis_H39;
            Pis_J39 = model.Pis_J39;
            Pis_L39 = model.Pis_L39;
            Pis_F41 = model.Pis_F41;
            Pis_H41 = model.Pis_H41;
            Pis_J41 = model.Pis_J41;
            Pis_L41 = model.Pis_L41;
            Pis_F47 = model.Pis_F47;
            Pis_H47 = model.Pis_H47;
            Pis_J47 = model.Pis_J47;
            Pis_L47 = model.Pis_L47;
            Pis_F49 = model.Pis_F49;
            Pis_H49 = model.Pis_H49;
            Pis_J49 = model.Pis_J49;
            Pis_L49 = model.Pis_L49;
            Pis_F51 = model.Pis_F51;
            Pis_H51 = model.Pis_H51;
            Pis_J51 = model.Pis_J51;
            Pis_L51 = model.Pis_L51;

            Pis_N27 = model.Pis_N27;
            Pis_P27 = model.Pis_P27;
            Pis_N47 = model.Pis_N47;
            Pis_P47 = model.Pis_P47;
        }



        public ViewModel(PisModel model)
        {
            if (model == null) return;
        }
    }
}
