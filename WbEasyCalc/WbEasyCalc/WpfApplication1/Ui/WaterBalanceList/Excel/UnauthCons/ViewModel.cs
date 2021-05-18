using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Ui.WbEasyCalcData.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.UnauthCons
{
    public class ViewModel : BaseSheetViewModel
    {
        #region Input props

        private string _unauthCons_Desc_B18;
        public string UnauthCons_Desc_B18
        {
            get => _unauthCons_Desc_B18;
            set { _unauthCons_Desc_B18 = value; RaisePropertyChanged(nameof(UnauthCons_Desc_B18)); }
        }
        private string _unauthCons_Desc_B19;
        public string UnauthCons_Desc_B19
        {
            get => _unauthCons_Desc_B19;
            set { _unauthCons_Desc_B19 = value; RaisePropertyChanged(nameof(UnauthCons_Desc_B19)); }
        }
        private string _unauthCons_Desc_B20;
        public string UnauthCons_Desc_B20
        {
            get => _unauthCons_Desc_B20;
            set { _unauthCons_Desc_B20 = value; RaisePropertyChanged(nameof(UnauthCons_Desc_B20)); }
        }
        private string _unauthCons_Desc_B21;
        public string UnauthCons_Desc_B21
        {
            get => _unauthCons_Desc_B21;
            set { _unauthCons_Desc_B21 = value; RaisePropertyChanged(nameof(UnauthCons_Desc_B21)); }
        }

        private double _unauthCons_OthersErrorMargin_F18;
        public double UnauthCons_OthersErrorMargin_F18
        {
            get => _unauthCons_OthersErrorMargin_F18;
            set { _unauthCons_OthersErrorMargin_F18 = value; RaisePropertyChanged(nameof(UnauthCons_OthersErrorMargin_F18)); Calculate(); }
        }
        private double _unauthCons_OthersErrorMargin_F19;
        public double UnauthCons_OthersErrorMargin_F19
        {
            get => _unauthCons_OthersErrorMargin_F19;
            set { _unauthCons_OthersErrorMargin_F19 = value; RaisePropertyChanged(nameof(UnauthCons_OthersErrorMargin_F19)); Calculate(); }
        }
        private double _unauthCons_OthersErrorMargin_F20;
        public double UnauthCons_OthersErrorMargin_F20
        {
            get => _unauthCons_OthersErrorMargin_F20;
            set { _unauthCons_OthersErrorMargin_F20 = value; RaisePropertyChanged(nameof(UnauthCons_OthersErrorMargin_F20)); Calculate(); }
        }
        private double _unauthCons_OthersErrorMargin_F21;
        public double UnauthCons_OthersErrorMargin_F21
        {
            get => _unauthCons_OthersErrorMargin_F21;
            set { _unauthCons_OthersErrorMargin_F21 = value; RaisePropertyChanged(nameof(UnauthCons_OthersErrorMargin_F21)); Calculate(); }
        }

        private double _unauthCons_OthersM3PerDay_J18;
        public double UnauthCons_OthersM3PerDay_J18
        {
            get => _unauthCons_OthersM3PerDay_J18;
            set { _unauthCons_OthersM3PerDay_J18 = value; RaisePropertyChanged(nameof(UnauthCons_OthersM3PerDay_J18)); Calculate(); }
        }
        private double _unauthCons_OthersM3PerDay_J19;
        public double UnauthCons_OthersM3PerDay_J19
        {
            get => _unauthCons_OthersM3PerDay_J19;
            set { _unauthCons_OthersM3PerDay_J19 = value; RaisePropertyChanged(nameof(UnauthCons_OthersM3PerDay_J19)); Calculate(); }
        }
        private double _unauthCons_OthersM3PerDay_J20;
        public double UnauthCons_OthersM3PerDay_J20
        {
            get => _unauthCons_OthersM3PerDay_J20;
            set { _unauthCons_OthersM3PerDay_J20 = value; RaisePropertyChanged(nameof(UnauthCons_OthersM3PerDay_J20)); Calculate(); }
        }
        private double _unauthCons_OthersM3PerDay_J21;
        public double UnauthCons_OthersM3PerDay_J21
        {
            get => _unauthCons_OthersM3PerDay_J21;
            set { _unauthCons_OthersM3PerDay_J21 = value; RaisePropertyChanged(nameof(UnauthCons_OthersM3PerDay_J21)); Calculate(); }
        }



        private int _unauthConsIllegalConnDomEstNoD6;
        private double _unauthConsIllegalConnDomPersPerHouseH6;
        private double _unauthConsIllegalConnDomConsLitPerPersDayJ6;
        private double _unauthConsIllegalConnDomErrorMarginF6;
        private double _unauthConsIllegalConnOthersErrorMarginF10;
        private double _unauthConsMeterTampBypEtcEstNoD14;
        private double _unauthConsMeterTampBypEtcErrorMarginF14;
        private double _unauthConsMeterTampBypEtcConsLitPerCustDayJ14;

        public int UnauthCons_IllegalConnDomEstNo_D6
        {
            get => _unauthConsIllegalConnDomEstNoD6;
            set { _unauthConsIllegalConnDomEstNoD6 = value; RaisePropertyChanged(nameof(UnauthCons_IllegalConnDomEstNo_D6)); Calculate(); }
        }

        public double UnauthCons_IllegalConnDomPersPerHouse_H6
        {
            get => _unauthConsIllegalConnDomPersPerHouseH6;
            set { _unauthConsIllegalConnDomPersPerHouseH6 = value; RaisePropertyChanged(nameof(UnauthCons_IllegalConnDomPersPerHouse_H6)); Calculate(); }
        }

        public double UnauthCons_IllegalConnDomConsLitPerPersDay_J6
        {
            get => _unauthConsIllegalConnDomConsLitPerPersDayJ6;
            set { _unauthConsIllegalConnDomConsLitPerPersDayJ6 = value; RaisePropertyChanged(nameof(UnauthCons_IllegalConnDomConsLitPerPersDay_J6)); Calculate(); }
        }

        public double UnauthCons_IllegalConnDomErrorMargin_F6
        {
            get => _unauthConsIllegalConnDomErrorMarginF6;
            set { _unauthConsIllegalConnDomErrorMarginF6 = value; RaisePropertyChanged(nameof(UnauthCons_IllegalConnDomErrorMargin_F6)); Calculate(); }
        }

        public double UnauthCons_IllegalConnOthersErrorMargin_F10
        {
            get => _unauthConsIllegalConnOthersErrorMarginF10;
            set { _unauthConsIllegalConnOthersErrorMarginF10 = value; RaisePropertyChanged(nameof(UnauthCons_IllegalConnOthersErrorMargin_F10)); Calculate(); }
        }


        private double _illegalConnectionsOthersEstimatedNumber_D10;
        public double IllegalConnectionsOthersEstimatedNumber_D10
        {
            get => _illegalConnectionsOthersEstimatedNumber_D10;
            set { _illegalConnectionsOthersEstimatedNumber_D10 = value; RaisePropertyChanged(nameof(IllegalConnectionsOthersEstimatedNumber_D10)); Calculate(); }
        }

        private double _illegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10;
        public double IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10
        {
            get => _illegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10;
            set { _illegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = value; RaisePropertyChanged(nameof(IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10)); Calculate(); }
        }




        public double UnauthCons_MeterTampBypEtcEstNo_D14
        {
            get => _unauthConsMeterTampBypEtcEstNoD14;
            set { _unauthConsMeterTampBypEtcEstNoD14 = value; RaisePropertyChanged(nameof(UnauthCons_MeterTampBypEtcEstNo_D14)); Calculate(); }
        }

        public double UnauthCons_MeterTampBypEtcErrorMargin_F14
        {
            get => _unauthConsMeterTampBypEtcErrorMarginF14;
            set { _unauthConsMeterTampBypEtcErrorMarginF14 = value; RaisePropertyChanged(nameof(UnauthCons_MeterTampBypEtcErrorMargin_F14)); Calculate(); }
        }

        public double UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14
        {
            get => _unauthConsMeterTampBypEtcConsLitPerCustDayJ14;
            set { _unauthConsMeterTampBypEtcConsLitPerCustDayJ14 = value; RaisePropertyChanged(nameof(UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14)); Calculate(); }
        }


        #endregion

        #region Output props

        private double _unauthCons_Total_L6;
        public double UnauthCons_Total_L6
        {
            get => _unauthCons_Total_L6;
            set { _unauthCons_Total_L6 = value; RaisePropertyChanged(nameof(UnauthCons_Total_L6)); }
        }
        private double _unauthCons_Total_L10;
        public double UnauthCons_Total_L10
        {
            get => _unauthCons_Total_L10;
            set { _unauthCons_Total_L10 = value; RaisePropertyChanged(nameof(UnauthCons_Total_L10)); }
        }
        private double _unauthCons_Total_L14;
        public double UnauthCons_Total_L14
        {
            get => _unauthCons_Total_L14;
            set { _unauthCons_Total_L14 = value; RaisePropertyChanged(nameof(UnauthCons_Total_L14)); }
        }
        private double _unauthCons_ErrorMarg_F24;
        public double UnauthCons_ErrorMarg_F24
        {
            get => _unauthCons_ErrorMarg_F24;
            set { _unauthCons_ErrorMarg_F24 = value; RaisePropertyChanged(nameof(UnauthCons_ErrorMarg_F24)); }
        }
        private double _unauthCons_Min_L27;
        public double UnauthCons_Min_L27
        {
            get => _unauthCons_Min_L27;
            set { _unauthCons_Min_L27 = value; RaisePropertyChanged(nameof(UnauthCons_Min_L27)); }
        }
        private double _unauthCons_Max_L29;
        public double UnauthCons_Max_L29
        {
            get => _unauthCons_Max_L29;
            set { _unauthCons_Max_L29 = value; RaisePropertyChanged(nameof(UnauthCons_Max_L29)); }
        }
        private double _unauthCons_BestEstimate_L31;
        public double UnauthCons_BestEstimate_L31
        {
            get => _unauthCons_BestEstimate_L31;
            set { _unauthCons_BestEstimate_L31 = value; RaisePropertyChanged(nameof(UnauthCons_BestEstimate_L31)); }
        }

        private double _unauthCons_Total_L18;
        public double UnauthCons_Total_L18
        {
            get => _unauthCons_Total_L18;
            set { _unauthCons_Total_L18 = value; RaisePropertyChanged(nameof(UnauthCons_Total_L18)); }
        }
        private double _unauthCons_Total_L19;
        public double UnauthCons_Total_L19
        {
            get => _unauthCons_Total_L19;
            set { _unauthCons_Total_L19 = value; RaisePropertyChanged(nameof(UnauthCons_Total_L19)); }
        }
        private double _unauthCons_Total_L20;
        public double UnauthCons_Total_L20
        {
            get => _unauthCons_Total_L20;
            set { _unauthCons_Total_L20 = value; RaisePropertyChanged(nameof(UnauthCons_Total_L20)); }
        }
        private double _unauthCons_Total_L21;
        public double UnauthCons_Total_L21
        {
            get => _unauthCons_Total_L21;
            set { _unauthCons_Total_L21 = value; RaisePropertyChanged(nameof(UnauthCons_Total_L21)); }
        }

        #endregion

        public UnauthConsModel Model => new UnauthConsModel()
        {
            UnauthCons_Desc_B18 = UnauthCons_Desc_B18,
            UnauthCons_Desc_B19 = UnauthCons_Desc_B19,
            UnauthCons_Desc_B20 = UnauthCons_Desc_B20,
            UnauthCons_Desc_B21 = UnauthCons_Desc_B21,
            UnauthCons_OthersErrorMargin_F18 = UnauthCons_OthersErrorMargin_F18,
            UnauthCons_OthersErrorMargin_F19 = UnauthCons_OthersErrorMargin_F19,
            UnauthCons_OthersErrorMargin_F20 = UnauthCons_OthersErrorMargin_F20,
            UnauthCons_OthersErrorMargin_F21 = UnauthCons_OthersErrorMargin_F21,
            UnauthCons_OthersM3PerDay_J18 = UnauthCons_OthersM3PerDay_J18,
            UnauthCons_OthersM3PerDay_J19 = UnauthCons_OthersM3PerDay_J19,
            UnauthCons_OthersM3PerDay_J20 = UnauthCons_OthersM3PerDay_J20,
            UnauthCons_OthersM3PerDay_J21 = UnauthCons_OthersM3PerDay_J21,

            UnauthCons_IllegalConnDomEstNo_D6 = UnauthCons_IllegalConnDomEstNo_D6,
            UnauthCons_IllegalConnDomPersPerHouse_H6 = UnauthCons_IllegalConnDomPersPerHouse_H6,
            UnauthCons_IllegalConnDomConsLitPerPersDay_J6 = UnauthCons_IllegalConnDomConsLitPerPersDay_J6,
            UnauthCons_IllegalConnDomErrorMargin_F6 = UnauthCons_IllegalConnDomErrorMargin_F6,
            UnauthCons_IllegalConnOthersErrorMargin_F10 = UnauthCons_IllegalConnOthersErrorMargin_F10,

            IllegalConnectionsOthersEstimatedNumber_D10 = IllegalConnectionsOthersEstimatedNumber_D10,
            IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10,

            UnauthCons_MeterTampBypEtcEstNo_D14 = UnauthCons_MeterTampBypEtcEstNo_D14,
            UnauthCons_MeterTampBypEtcErrorMargin_F14 = UnauthCons_MeterTampBypEtcErrorMargin_F14,
            UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 = UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14,

            // Output
            UnauthCons_Total_L6 = UnauthCons_Total_L6,
            UnauthCons_Total_L10 = UnauthCons_Total_L10,
            UnauthCons_Total_L14 = UnauthCons_Total_L14,
            UnauthCons_ErrorMarg_F24 = UnauthCons_ErrorMarg_F24,
            UnauthCons_Min_L27 = UnauthCons_Min_L27,
            UnauthCons_Max_L29 = UnauthCons_Max_L29,
            UnauthCons_BestEstimate_L31 = UnauthCons_BestEstimate_L31,
            UnauthCons_Total_L18 = UnauthCons_Total_L18,
            UnauthCons_Total_L19 = UnauthCons_Total_L19,
            UnauthCons_Total_L20 = UnauthCons_Total_L20,
            UnauthCons_Total_L21 = UnauthCons_Total_L21,

        };

        internal void Refreash(UnauthConsModel model)
        {
            UnauthCons_Total_L6 = model.UnauthCons_Total_L6;
            UnauthCons_Total_L10 = model.UnauthCons_Total_L10;
            UnauthCons_Total_L14 = model.UnauthCons_Total_L14;
            UnauthCons_ErrorMarg_F24 = model.UnauthCons_ErrorMarg_F24;
            UnauthCons_Min_L27 = model.UnauthCons_Min_L27;
            UnauthCons_Max_L29 = model.UnauthCons_Max_L29;
            UnauthCons_BestEstimate_L31 = model.UnauthCons_BestEstimate_L31;
            UnauthCons_Total_L18 = model.UnauthCons_Total_L18;
            UnauthCons_Total_L19 = model.UnauthCons_Total_L19;
            UnauthCons_Total_L20 = model.UnauthCons_Total_L20;
            UnauthCons_Total_L21 = model.UnauthCons_Total_L21;
        }

        public ViewModel(UnauthConsModel model)
        {
            if (model == null) return;

            // Input
            UnauthCons_Desc_B18 = model.UnauthCons_Desc_B18;
            UnauthCons_Desc_B19 = model.UnauthCons_Desc_B19;
            UnauthCons_Desc_B20 = model.UnauthCons_Desc_B20;
            UnauthCons_Desc_B21 = model.UnauthCons_Desc_B21;
            UnauthCons_OthersErrorMargin_F18 = model.UnauthCons_OthersErrorMargin_F18;
            UnauthCons_OthersErrorMargin_F19 = model.UnauthCons_OthersErrorMargin_F19;
            UnauthCons_OthersErrorMargin_F20 = model.UnauthCons_OthersErrorMargin_F20;
            UnauthCons_OthersErrorMargin_F21 = model.UnauthCons_OthersErrorMargin_F21;
            UnauthCons_OthersM3PerDay_J18 = model.UnauthCons_OthersM3PerDay_J18;
            UnauthCons_OthersM3PerDay_J19 = model.UnauthCons_OthersM3PerDay_J19;
            UnauthCons_OthersM3PerDay_J20 = model.UnauthCons_OthersM3PerDay_J20;
            UnauthCons_OthersM3PerDay_J21 = model.UnauthCons_OthersM3PerDay_J21;

            UnauthCons_IllegalConnDomEstNo_D6 = model.UnauthCons_IllegalConnDomEstNo_D6;
            UnauthCons_IllegalConnDomPersPerHouse_H6 = model.UnauthCons_IllegalConnDomPersPerHouse_H6;
            UnauthCons_IllegalConnDomConsLitPerPersDay_J6 = model.UnauthCons_IllegalConnDomConsLitPerPersDay_J6;
            UnauthCons_IllegalConnDomErrorMargin_F6 = model.UnauthCons_IllegalConnDomErrorMargin_F6;
            UnauthCons_IllegalConnOthersErrorMargin_F10 = model.UnauthCons_IllegalConnOthersErrorMargin_F10;

            IllegalConnectionsOthersEstimatedNumber_D10 = model.IllegalConnectionsOthersEstimatedNumber_D10;
            IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = model.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10;

            UnauthCons_MeterTampBypEtcEstNo_D14 = model.UnauthCons_MeterTampBypEtcEstNo_D14;
            UnauthCons_MeterTampBypEtcErrorMargin_F14 = model.UnauthCons_MeterTampBypEtcErrorMargin_F14;
            UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 = model.UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14;
        }
    }
}
