using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Ui.WbEasyCalcData.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.Network
{
    public class ViewModel : BaseSheetViewModel
    {

        #region Input props

        private string _network_Desc_B7;
        public string Network_Desc_B7
        {
            get => _network_Desc_B7;
            set { _network_Desc_B7 = value; RaisePropertyChanged(nameof(Network_Desc_B7)); }
        }
        private string _network_Desc_B8;
        public string Network_Desc_B8
        {
            get => _network_Desc_B8;
            set { _network_Desc_B8 = value; RaisePropertyChanged(nameof(Network_Desc_B8)); }
        }
        private string _network_Desc_B9;
        public string Network_Desc_B9
        {
            get => _network_Desc_B9;
            set { _network_Desc_B9 = value; RaisePropertyChanged(nameof(Network_Desc_B9)); }
        }
        private string _network_Desc_B10;
        public string Network_Desc_B10
        {
            get => _network_Desc_B10;
            set { _network_Desc_B10 = value; RaisePropertyChanged(nameof(Network_Desc_B10)); }
        }

        private double _networkDistributionAndTransmissionMainsD7;
        public double Network_DistributionAndTransmissionMains_D7
        {
            get => _networkDistributionAndTransmissionMainsD7;
            set { _networkDistributionAndTransmissionMainsD7 = value; RaisePropertyChanged(nameof(Network_DistributionAndTransmissionMains_D7)); Calculate(); }
        }
        private double _networkDistributionAndTransmissionMainsD8;
        public double Network_DistributionAndTransmissionMains_D8
        {
            get => _networkDistributionAndTransmissionMainsD8;
            set { _networkDistributionAndTransmissionMainsD8 = value; RaisePropertyChanged(nameof(Network_DistributionAndTransmissionMains_D8)); Calculate(); }
        }
        private double _networkDistributionAndTransmissionMainsD9;
        public double Network_DistributionAndTransmissionMains_D9
        {
            get => _networkDistributionAndTransmissionMainsD9;
            set { _networkDistributionAndTransmissionMainsD9 = value; RaisePropertyChanged(nameof(Network_DistributionAndTransmissionMains_D9)); Calculate(); }
        }
        private double _networkDistributionAndTransmissionMainsD10;
        public double Network_DistributionAndTransmissionMains_D10
        {
            get => _networkDistributionAndTransmissionMainsD10;
            set { _networkDistributionAndTransmissionMainsD10 = value; RaisePropertyChanged(nameof(Network_DistributionAndTransmissionMains_D10)); Calculate(); }
        }

        private double _networkNoOfConnOfRegCustomersH10;
        private double _networkNoOfInactAccountsWSvcConnsH18;
        private double _networkAvgLenOfSvcConnFromBoundaryToMeterMH32;
        public double Network_NoOfConnOfRegCustomers_H10
        {
            get => _networkNoOfConnOfRegCustomersH10;
            set { _networkNoOfConnOfRegCustomersH10 = value; RaisePropertyChanged(nameof(Network_NoOfConnOfRegCustomers_H10)); Calculate(); }
        }

        public double Network_NoOfInactAccountsWSvcConns_H18
        {
            get => _networkNoOfInactAccountsWSvcConnsH18;
            set { _networkNoOfInactAccountsWSvcConnsH18 = value; RaisePropertyChanged(nameof(Network_NoOfInactAccountsWSvcConns_H18)); Calculate(); }
        }

        public double Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32
        {
            get => _networkAvgLenOfSvcConnFromBoundaryToMeterMH32;
            set { _networkAvgLenOfSvcConnFromBoundaryToMeterMH32 = value; RaisePropertyChanged(nameof(Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32)); Calculate(); }
        }

        private double _network_PossibleUnd_D30;
        public double Network_PossibleUnd_D30
        {
            get => _network_PossibleUnd_D30;
            set { _network_PossibleUnd_D30 = value; RaisePropertyChanged(nameof(Network_PossibleUnd_D30)); Calculate(); }
        }
        private double _network_NoCustomers_H7;
        public double Network_NoCustomers_H7
        {
            get => _network_NoCustomers_H7;
            set { _network_NoCustomers_H7 = value; RaisePropertyChanged(nameof(Network_NoCustomers_H7)); Calculate(); }
        }

        private double _network_ErrorMargin_J7;
        public double Network_ErrorMargin_J7
        {
            get => _network_ErrorMargin_J7;
            set { _network_ErrorMargin_J7 = value; RaisePropertyChanged(nameof(Network_ErrorMargin_J7)); Calculate(); }
        }

        private double _network_ErrorMargin_J10;
        public double Network_ErrorMargin_J10
        {
            get => _network_ErrorMargin_J10;
            set { _network_ErrorMargin_J10 = value; RaisePropertyChanged(nameof(Network_ErrorMargin_J10)); Calculate(); }
        }
        private double _network_ErrorMargin_J18;
        public double Network_ErrorMargin_J18
        {
            get => _network_ErrorMargin_J18;
            set { _network_ErrorMargin_J18 = value; RaisePropertyChanged(nameof(Network_ErrorMargin_J18)); Calculate(); }
        }
        private double _network_ErrorMargin_J32;
        public double Network_ErrorMargin_J32
        {
            get => _network_ErrorMargin_J32;
            set { _network_ErrorMargin_J32 = value; RaisePropertyChanged(nameof(Network_ErrorMargin_J32)); Calculate(); }
        }
        private double _network_ErrorMargin_D35;
        public double Network_ErrorMargin_D35
        {
            get => _network_ErrorMargin_D35;
            set { _network_ErrorMargin_D35 = value; RaisePropertyChanged(nameof(Network_ErrorMargin_D35)); Calculate(); }
        }

        #endregion

        #region Output props

        private double _network_Total_D28;
        public double Network_Total_D28
        {
            get => _network_Total_D28;
            set { _network_Total_D28 = value; RaisePropertyChanged(nameof(Network_Total_D28)); }
        }
        private double _network_Total_D32;
        public double Network_Total_D32
        {
            get => _network_Total_D32;
            set { _network_Total_D32 = value; RaisePropertyChanged(nameof(Network_Total_D32)); }
        }
        private double _network_Min_D39;
        public double Network_Min_D39
        {
            get => _network_Min_D39;
            set { _network_Min_D39 = value; RaisePropertyChanged(nameof(Network_Min_D39)); }
        }
        private double _network_Max_D41;
        public double Network_Max_D41
        {
            get => _network_Max_D41;
            set { _network_Max_D41 = value; RaisePropertyChanged(nameof(Network_Max_D41)); }
        }
        private double _network_BestEstimate_D43;
        public double Network_BestEstimate_D43
        {
            get => _network_BestEstimate_D43;
            set { _network_BestEstimate_D43 = value; RaisePropertyChanged(nameof(Network_BestEstimate_D43)); }
        }
        private double _network_Number_H21;
        public double Network_Number_H21
        {
            get => _network_Number_H21;
            set { _network_Number_H21 = value; RaisePropertyChanged(nameof(Network_Number_H21)); }
        }
        private double _network_ErrorMarg_J21;
        public double Network_ErrorMarg_J21
        {
            get => _network_ErrorMarg_J21;
            set { _network_ErrorMarg_J21 = value; RaisePropertyChanged(nameof(Network_ErrorMarg_J21)); }
        }
        private double _network_ErrorMarg_J24;
        public double Network_ErrorMarg_J24
        {
            get => _network_ErrorMarg_J24;
            set { _network_ErrorMarg_J24 = value; RaisePropertyChanged(nameof(Network_ErrorMarg_J24)); }
        }
        private double _network_Min_H26;
        public double Network_Min_H26
        {
            get => _network_Min_H26;
            set { _network_Min_H26 = value; RaisePropertyChanged(nameof(Network_Min_H26)); }
        }
        private double _network_Max_H28;
        public double Network_Max_H28
        {
            get => _network_Max_H28;
            set { _network_Max_H28 = value; RaisePropertyChanged(nameof(Network_Max_H28)); }
        }

        private double _network_BestEstimate_H30;
        public double Network_BestEstimate_H30
        {
            get => _network_BestEstimate_H30;
            set { _network_BestEstimate_H30 = value; RaisePropertyChanged(nameof(Network_BestEstimate_H30)); }
        }
        private double _network_Number_H39;
        public double Network_Number_H39
        {
            get => _network_Number_H39;
            set { _network_Number_H39 = value; RaisePropertyChanged(nameof(Network_Number_H39)); }
        }
        private double _network_ErrorMarg_J39;
        public double Network_ErrorMarg_J39
        {
            get => _network_ErrorMarg_J39;
            set { _network_ErrorMarg_J39 = value; RaisePropertyChanged(nameof(Network_ErrorMarg_J39)); }
        }

        #endregion

        public NetworkModel Model => new NetworkModel()
        {
            // Input
            Network_Desc_B7 = Network_Desc_B7,
            Network_Desc_B8 = Network_Desc_B8,
            Network_Desc_B9 = Network_Desc_B9,
            Network_Desc_B10 = Network_Desc_B10,
            Network_DistributionAndTransmissionMains_D7 = Network_DistributionAndTransmissionMains_D7,
            Network_DistributionAndTransmissionMains_D8 = Network_DistributionAndTransmissionMains_D8,
            Network_DistributionAndTransmissionMains_D9 = Network_DistributionAndTransmissionMains_D9,
            Network_DistributionAndTransmissionMains_D10 = Network_DistributionAndTransmissionMains_D10,
            Network_NoOfConnOfRegCustomers_H10 = Network_NoOfConnOfRegCustomers_H10,
            Network_NoOfInactAccountsWSvcConns_H18 = Network_NoOfInactAccountsWSvcConns_H18,
            Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 = Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32,
            Network_PossibleUnd_D30 = Network_PossibleUnd_D30,
            Network_NoCustomers_H7 = Network_NoCustomers_H7,
            Network_ErrorMargin_J7 = Network_ErrorMargin_J7,
            Network_ErrorMargin_J10 = Network_ErrorMargin_J10,
            Network_ErrorMargin_J18 = Network_ErrorMargin_J18,
            Network_ErrorMargin_J32 = Network_ErrorMargin_J32,
            Network_ErrorMargin_D35 = Network_ErrorMargin_D35,
            // Output
            Network_Total_D28 = Network_Total_D28,
            Network_Total_D32 = Network_Total_D32,
            Network_Min_D39 = Network_Min_D39,
            Network_Max_D41 = Network_Max_D41,
            Network_BestEstimate_D43 = Network_BestEstimate_D43,
            Network_Number_H21 = Network_Number_H21,
            Network_ErrorMarg_J21 = Network_ErrorMarg_J21,
            Network_ErrorMarg_J24 = Network_ErrorMarg_J24,
            Network_Min_H26 = Network_Min_H26,
            Network_Max_H28 = Network_Max_H28,
            Network_BestEstimate_H30 = Network_BestEstimate_H30,
            Network_Number_H39 = Network_Number_H39,
            Network_ErrorMarg_J39 = Network_ErrorMarg_J39,
        };

        internal void Refreash(NetworkModel model)
        {
            Network_Total_D28 = model.Network_Total_D28;
            Network_Total_D32 = model.Network_Total_D32;
            Network_Min_D39 = model.Network_Min_D39;
            Network_Max_D41 = model.Network_Max_D41;
            Network_BestEstimate_D43 = model.Network_BestEstimate_D43;
            Network_Number_H21 = model.Network_Number_H21;
            Network_ErrorMarg_J21 = model.Network_ErrorMarg_J21;
            Network_ErrorMarg_J24 = model.Network_ErrorMarg_J24;
            Network_Min_H26 = model.Network_Min_H26;
            Network_Max_H28 = model.Network_Max_H28;
            Network_BestEstimate_H30 = model.Network_BestEstimate_H30;
            Network_Number_H39 = model.Network_Number_H39;
            Network_ErrorMarg_J39 = model.Network_ErrorMarg_J39;
        }

        public ViewModel(NetworkModel model)
        {
            if (model == null) return;

            Network_Desc_B7 = model.Network_Desc_B7;
            Network_Desc_B8 = model.Network_Desc_B8;
            Network_Desc_B9 = model.Network_Desc_B9;
            Network_Desc_B10 = model.Network_Desc_B10;
            Network_DistributionAndTransmissionMains_D7 = model.Network_DistributionAndTransmissionMains_D7;
            Network_DistributionAndTransmissionMains_D8 = model.Network_DistributionAndTransmissionMains_D8;
            Network_DistributionAndTransmissionMains_D9 = model.Network_DistributionAndTransmissionMains_D9;
            Network_DistributionAndTransmissionMains_D10 = model.Network_DistributionAndTransmissionMains_D10;
            Network_NoOfConnOfRegCustomers_H10 = model.Network_NoOfConnOfRegCustomers_H10;
            Network_NoOfInactAccountsWSvcConns_H18 = model.Network_NoOfInactAccountsWSvcConns_H18;
            Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 = model.Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32;
            Network_PossibleUnd_D30 = model.Network_PossibleUnd_D30;
            Network_NoCustomers_H7 = model.Network_NoCustomers_H7;
            Network_ErrorMargin_J7 = model.Network_ErrorMargin_J7;
            Network_ErrorMargin_J10 = model.Network_ErrorMargin_J10;
            Network_ErrorMargin_J18 = model.Network_ErrorMargin_J18;
            Network_ErrorMargin_J32 = model.Network_ErrorMargin_J32;
            Network_ErrorMargin_D35 = model.Network_ErrorMargin_D35;
        }
    }
}
