using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Ui.WaterBalanceList.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WaterBalanceList.Excel.SysInput
{
    public class ViewModel : BaseSheetViewModel
    {
        #region Input props

        private string _sysInput_Desc_B6;
        public string SysInput_Desc_B6
        {
            get => _sysInput_Desc_B6;
            set { _sysInput_Desc_B6 = value; RaisePropertyChanged(nameof(SysInput_Desc_B6)); }
        }
        private string _sysInput_Desc_B7;
        public string SysInput_Desc_B7
        {
            get => _sysInput_Desc_B7;
            set { _sysInput_Desc_B7 = value; RaisePropertyChanged(nameof(SysInput_Desc_B7)); }
        }
        private string _sysInput_Desc_B8;
        public string SysInput_Desc_B8
        {
            get => _sysInput_Desc_B8;
            set { _sysInput_Desc_B8 = value; RaisePropertyChanged(nameof(SysInput_Desc_B8)); }
        }
        private string _sysInput_Desc_B9;
        public string SysInput_Desc_B9
        {
            get => _sysInput_Desc_B9;
            set { _sysInput_Desc_B9 = value; RaisePropertyChanged(nameof(SysInput_Desc_B9)); }
        }

        private double _sysInputSystemInputVolumeM3D6;
        public double SysInput_SystemInputVolumeM3_D6
        {
            get => _sysInputSystemInputVolumeM3D6;
            set { _sysInputSystemInputVolumeM3D6 = value; RaisePropertyChanged(nameof(SysInput_SystemInputVolumeM3_D6)); Calculate(); }
        }
        private double _sysInputSystemInputVolumeErrorF6;
        public double SysInput_SystemInputVolumeError_F6
        {
            get => _sysInputSystemInputVolumeErrorF6;
            set { _sysInputSystemInputVolumeErrorF6 = value; RaisePropertyChanged(nameof(SysInput_SystemInputVolumeError_F6)); Calculate(); }
        }

        private double _sysInputSystemInputVolumeM3D7;
        public double SysInput_SystemInputVolumeM3_D7
        {
            get => _sysInputSystemInputVolumeM3D7;
            set { _sysInputSystemInputVolumeM3D7 = value; RaisePropertyChanged(nameof(SysInput_SystemInputVolumeM3_D7)); Calculate(); }
        }
        private double _sysInputSystemInputVolumeErrorF7;
        public double SysInput_SystemInputVolumeError_F7
        {
            get => _sysInputSystemInputVolumeErrorF7;
            set { _sysInputSystemInputVolumeErrorF7 = value; RaisePropertyChanged(nameof(SysInput_SystemInputVolumeError_F7)); Calculate(); }
        }

        private double _sysInputSystemInputVolumeM3D8;
        public double SysInput_SystemInputVolumeM3_D8
        {
            get => _sysInputSystemInputVolumeM3D8;
            set { _sysInputSystemInputVolumeM3D8 = value; RaisePropertyChanged(nameof(SysInput_SystemInputVolumeM3_D8)); Calculate(); }
        }
        private double _sysInputSystemInputVolumeErrorF8;
        public double SysInput_SystemInputVolumeError_F8
        {
            get => _sysInputSystemInputVolumeErrorF8;
            set { _sysInputSystemInputVolumeErrorF8 = value; RaisePropertyChanged(nameof(SysInput_SystemInputVolumeError_F8)); Calculate(); }
        }

        private double _sysInputSystemInputVolumeM3D9;
        public double SysInput_SystemInputVolumeM3_D9
        {
            get => _sysInputSystemInputVolumeM3D9;
            set { _sysInputSystemInputVolumeM3D9 = value; RaisePropertyChanged(nameof(SysInput_SystemInputVolumeM3_D9)); Calculate(); }
        }
        private double _sysInputSystemInputVolumeErrorF9;
        public double SysInput_SystemInputVolumeError_F9
        {
            get => _sysInputSystemInputVolumeErrorF9;
            set { _sysInputSystemInputVolumeErrorF9 = value; RaisePropertyChanged(nameof(SysInput_SystemInputVolumeError_F9)); Calculate(); }
        }

        #endregion

        #region Output props

        private double _sysInput_ErrorMarg_F72;
        public double SysInput_ErrorMarg_F72
        {
            get => _sysInput_ErrorMarg_F72;
            set { _sysInput_ErrorMarg_F72 = value; RaisePropertyChanged(nameof(SysInput_ErrorMarg_F72)); }
        }
        private double _sysInput_Min_D75;
        public double SysInput_Min_D75
        {
            get => _sysInput_Min_D75;
            set { _sysInput_Min_D75 = value; RaisePropertyChanged(nameof(SysInput_Min_D75)); }
        }
        private double _sysInput_Max_D77;
        public double SysInput_Max_D77
        {
            get => _sysInput_Max_D77;
            set { _sysInput_Max_D77 = value; RaisePropertyChanged(nameof(SysInput_Max_D77)); }
        }
        private double _sysInput_BestEstimate_D79;
        public double SysInput_BestEstimate_D79
        {
            get => _sysInput_BestEstimate_D79;
            set { _sysInput_BestEstimate_D79 = value; RaisePropertyChanged(nameof(SysInput_BestEstimate_D79)); }
        }

        #endregion

        public SysInputModel Model => new SysInputModel()
        {
            // Input
            SysInput_Desc_B6 = SysInput_Desc_B6,
            SysInput_Desc_B7 = SysInput_Desc_B7,
            SysInput_Desc_B8 = SysInput_Desc_B8,
            SysInput_Desc_B9 = SysInput_Desc_B9,
            SysInput_SystemInputVolumeM3_D6 = SysInput_SystemInputVolumeM3_D6,
            SysInput_SystemInputVolumeError_F6 = SysInput_SystemInputVolumeError_F6,
            SysInput_SystemInputVolumeM3_D7 = SysInput_SystemInputVolumeM3_D7,
            SysInput_SystemInputVolumeError_F7 = SysInput_SystemInputVolumeError_F7,
            SysInput_SystemInputVolumeM3_D8 = SysInput_SystemInputVolumeM3_D8,
            SysInput_SystemInputVolumeError_F8 = SysInput_SystemInputVolumeError_F8,
            SysInput_SystemInputVolumeM3_D9 = SysInput_SystemInputVolumeM3_D9,
            SysInput_SystemInputVolumeError_F9 = SysInput_SystemInputVolumeError_F9,
            // Output
            SysInput_ErrorMarg_F72 = SysInput_ErrorMarg_F72,
            SysInput_Min_D75 = SysInput_Min_D75,
            SysInput_Max_D77 = SysInput_Max_D77,
            SysInput_BestEstimate_D79 = SysInput_BestEstimate_D79,
        };

        internal void Refreash(SysInputModel model)
        {
            SysInput_ErrorMarg_F72 = model.SysInput_ErrorMarg_F72;
            SysInput_Min_D75 = model.SysInput_Min_D75;
            SysInput_Max_D77 = model.SysInput_Max_D77;
            SysInput_BestEstimate_D79 = model.SysInput_BestEstimate_D79;
        }
        
        //public ViewModel() { }

        public ViewModel(SysInputModel model)
        {
            if (model == null) return;

            // Input
            SysInput_Desc_B6 = model.SysInput_Desc_B6;
            SysInput_Desc_B7 = model.SysInput_Desc_B7;
            SysInput_Desc_B8 = model.SysInput_Desc_B8;
            SysInput_Desc_B9 = model.SysInput_Desc_B9;
            SysInput_SystemInputVolumeM3_D6 = model.SysInput_SystemInputVolumeM3_D6;
            SysInput_SystemInputVolumeError_F6 = model.SysInput_SystemInputVolumeError_F6;
            SysInput_SystemInputVolumeM3_D7 = model.SysInput_SystemInputVolumeM3_D7;
            SysInput_SystemInputVolumeError_F7 = model.SysInput_SystemInputVolumeError_F7;
            SysInput_SystemInputVolumeM3_D8 = model.SysInput_SystemInputVolumeM3_D8;
            SysInput_SystemInputVolumeError_F8 = model.SysInput_SystemInputVolumeError_F8;
            SysInput_SystemInputVolumeM3_D9 = model.SysInput_SystemInputVolumeM3_D9;
            SysInput_SystemInputVolumeError_F9 = model.SysInput_SystemInputVolumeError_F9;
        }

    }
}
