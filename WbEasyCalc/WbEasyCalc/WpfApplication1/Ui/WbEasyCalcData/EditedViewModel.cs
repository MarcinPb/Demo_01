using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DataModel;
using DataRepository;
using GlobalRepository;
using Microsoft.WindowsAPICodePack.Dialogs;
using WbEasyCalcModel;
using WpfApplication1.Ui.WbEasyCalcData.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData
{
    public class EditedViewModel : ViewModelBase, IDialogViewModel
    {
        private ItemViewModel _model;
        public ItemViewModel ItemViewModel
        {
            get => _model;
            set { _model = value; RaisePropertyChanged(); }
        }

        public List<IdNamePair> YearList { get; set; }
        public List<IdNamePair> MonthList { get; set; }
        public List<ZoneItem> ZoneItemList { get; set; }
        public List<IdNamePair> J6_List { get; set; }

        #region Commands: LoadDataFromSystemCmd, ImportFromExcelExecute, ImportFromExcelCanExecute

        public RelayCommand LoadDataFromSystemCmd => new RelayCommand(LoadDataFromSystemExecute, LoadDataFromSystemCanExecute);
        private void LoadDataFromSystemExecute()
        {
            LoadDataFromSystem();
        }
        public bool LoadDataFromSystemCanExecute()
        {
            return true;
        }

        public RelayCommand ImportFromExcelCmd => new RelayCommand(ImportFromExcelExecute, ImportFromExcelCanExecute);
        private void ImportFromExcelExecute()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = true,
                Filters =
                {
                    new CommonFileDialogFilter("Excel Files", "*.xls,*.xlsm"),
                }
            };
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string fileName = dialog.FileName;
                ImportFromExcel(fileName);
            }

        }
        public bool ImportFromExcelCanExecute()
        {
            return true;
        }

        public RelayCommand ExportToExcelCmd => new RelayCommand(ExportToExcelExecute, ExportToExcelCanExecute);
        private void ExportToExcelExecute()
        {
            CommonSaveFileDialog dialog = new CommonSaveFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = true,
                DefaultExtension = ".xlsm",
                AlwaysAppendDefaultExtension = true,
                Filters =
                {
                    //new CommonFileDialogFilter("Excel Files", "*.xls,*.xlsm"),
                    new CommonFileDialogFilter("Excel Files", "*.xlsm"),
                }
            };
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string fileName = dialog.FileName;
                ExportToExcel(fileName);
            }
        }
        public bool ExportToExcelCanExecute()
        {
            return true;
        }


        #endregion

        #region IDialogViewModel

        public string Title { get; set; } = "Excel Calculations & Water Consumptions";

        public bool Save()
        {
            DataModel.WbEasyCalcData model = GlobalConfig.DataRepository.WbEasyCalcDataListRepository.SaveItem(ItemViewModel.Model);
            Messenger.Default.Send<DataModel.WbEasyCalcData>(model);
            return true;
        }

        public void Close()
        {
        }

        #endregion

        public EditedViewModel(int id)
        {

            ItemViewModel = new ItemViewModel(GlobalConfig.DataRepository.WbEasyCalcDataListRepository.GetItem(id));

            YearList = GlobalConfig.DataRepository.YearList;
            MonthList = GlobalConfig.DataRepository.MonthList;
            ZoneItemList = GlobalConfig.DataRepository.ZoneList;

            J6_List = new List<IdNamePair> { 
                new IdNamePair(){ Id=1, Name="1"},
                new IdNamePair(){ Id=2, Name="2"},
            };
            Messenger.Default.Register<WaterConsumption.ListViewModel>(this, OnWaterConsumptionChangedReceived);
        }
        private void OnWaterConsumptionChangedReceived(WaterConsumption.ListViewModel obj)
        {
            var sum = obj.List.Sum(x => x.Model.Value);
            ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_BilledMetConsBulkWatSupExpM3_D6 = sum;
        }

        private void LoadDataFromSystem()
        {
            try
            {
                // Invoke spGisModelScadaData on SQL Server.
                var wbEasyCalcData = GlobalConfig.DataRepository.GetAutomaticData(ItemViewModel.YearNo, ItemViewModel.MonthNo, ItemViewModel.ZoneId);

                if (ItemViewModel.MonthNo < 13)
                {
                    ItemViewModel.EasyCalcViewModel.Model.SysInputModel.SysInput_SystemInputVolumeM3_D6 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D6;
                    ItemViewModel.EasyCalcViewModel.Model.BilledConsModel.BilledCons_BilledMetConsBulkWatSupExpM3_D6 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_BilledMetConsBulkWatSupExpM3_D6;
                    //ItemViewModel.EasyCalcViewModel.Model.BilledConsModel.BilledCons_UnbMetConsM3_D8 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D8;
                    
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_DistributionAndTransmissionMains_D7 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D7;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_NoOfConnOfRegCustomers_H10 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_NoOfConnOfRegCustomers_H10;
                }

                // Sum parameters for all year.
                else
                {
                    ItemViewModel.Model.EasyCalcModel = wbEasyCalcData.EasyCalcModel;
                    //ItemViewModel.EasyCalcViewModel.Model = wbEasyCalcData.EasyCalcModel;



                    //ItemViewModel.Start_PeriodDays_M21 = wbEasyCalcData.EasyCalcModel.StartModel.Start_PeriodDays_M21;

                    //ItemViewModel.SysInput_Desc_B6 = wbEasyCalcData.SysInput_Desc_B6;
                    //ItemViewModel.SysInput_Desc_B7 = wbEasyCalcData.SysInput_Desc_B7;
                    //ItemViewModel.SysInput_Desc_B8 = wbEasyCalcData.SysInput_Desc_B8;
                    //ItemViewModel.SysInput_Desc_B9 = wbEasyCalcData.SysInput_Desc_B9;
                    //ItemViewModel.SysInput_SystemInputVolumeM3_D6 = wbEasyCalcData.SysInput_SystemInputVolumeM3_D6;                // @SystemInputVolume
                    //ItemViewModel.SysInput_SystemInputVolumeError_F6 = wbEasyCalcData.SysInput_SystemInputVolumeError_F6;
                    //ItemViewModel.SysInput_SystemInputVolumeM3_D7 = wbEasyCalcData.SysInput_SystemInputVolumeM3_D7;                                     
                    //ItemViewModel.SysInput_SystemInputVolumeError_F7 = wbEasyCalcData.SysInput_SystemInputVolumeError_F7;
                    //ItemViewModel.SysInput_SystemInputVolumeM3_D8 = wbEasyCalcData.SysInput_SystemInputVolumeM3_D8;                                     
                    //ItemViewModel.SysInput_SystemInputVolumeError_F8 = wbEasyCalcData.SysInput_SystemInputVolumeError_F8;
                    //ItemViewModel.SysInput_SystemInputVolumeM3_D9 = wbEasyCalcData.SysInput_SystemInputVolumeM3_D9;                                     
                    //ItemViewModel.SysInput_SystemInputVolumeError_F9 = wbEasyCalcData.SysInput_SystemInputVolumeError_F9;

                    //ItemViewModel.BilledCons_Desc_B8 = wbEasyCalcData.BilledCons_Desc_B8;
                    //ItemViewModel.BilledCons_Desc_B9 = wbEasyCalcData.BilledCons_Desc_B9;
                    //ItemViewModel.BilledCons_Desc_B10 = wbEasyCalcData.BilledCons_Desc_B10;
                    //ItemViewModel.BilledCons_Desc_B11 = wbEasyCalcData.BilledCons_Desc_B11;
                    //ItemViewModel.BilledCons_Desc_F8 = wbEasyCalcData.BilledCons_Desc_F8;
                    //ItemViewModel.BilledCons_Desc_F9 = wbEasyCalcData.BilledCons_Desc_F9;
                    //ItemViewModel.BilledCons_Desc_F10 = wbEasyCalcData.BilledCons_Desc_F10;
                    //ItemViewModel.BilledCons_Desc_F11 = wbEasyCalcData.BilledCons_Desc_F11;

                    //ItemViewModel.UnbilledCons_Desc_D8 = wbEasyCalcData.UnbilledCons_Desc_D8;
                    //ItemViewModel.UnbilledCons_Desc_D9 = wbEasyCalcData.UnbilledCons_Desc_D9;
                    //ItemViewModel.UnbilledCons_Desc_D10 = wbEasyCalcData.UnbilledCons_Desc_D10;
                    //ItemViewModel.UnbilledCons_Desc_D11 = wbEasyCalcData.UnbilledCons_Desc_D11;
                    //ItemViewModel.UnbilledCons_Desc_F6 = wbEasyCalcData.UnbilledCons_Desc_F6;
                    //ItemViewModel.UnbilledCons_Desc_F7 = wbEasyCalcData.UnbilledCons_Desc_F7;
                    //ItemViewModel.UnbilledCons_Desc_F8 = wbEasyCalcData.UnbilledCons_Desc_F8;
                    //ItemViewModel.UnbilledCons_Desc_F9 = wbEasyCalcData.UnbilledCons_Desc_F9;
                    //ItemViewModel.UnbilledCons_Desc_F10 = wbEasyCalcData.UnbilledCons_Desc_F10;
                    //ItemViewModel.UnbilledCons_Desc_F11 = wbEasyCalcData.UnbilledCons_Desc_F11;

                    //ItemViewModel.UnauthCons_Desc_B18 = wbEasyCalcData.UnauthCons_Desc_B18;
                    //ItemViewModel.UnauthCons_Desc_B19 = wbEasyCalcData.UnauthCons_Desc_B19;
                    //ItemViewModel.UnauthCons_Desc_B20 = wbEasyCalcData.UnauthCons_Desc_B20;
                    //ItemViewModel.UnauthCons_Desc_B21 = wbEasyCalcData.UnauthCons_Desc_B21;
                    //ItemViewModel.MetErrors_Desc_D12 = wbEasyCalcData.MetErrors_Desc_D12;
                    //ItemViewModel.MetErrors_Desc_D13 = wbEasyCalcData.MetErrors_Desc_D13;
                    //ItemViewModel.MetErrors_Desc_D14 = wbEasyCalcData.MetErrors_Desc_D14;
                    //ItemViewModel.MetErrors_Desc_D15 = wbEasyCalcData.MetErrors_Desc_D15;
                    //ItemViewModel.Network_Desc_B7 = wbEasyCalcData.Network_Desc_B7;
                    //ItemViewModel.Network_Desc_B8 = wbEasyCalcData.Network_Desc_B8;
                    //ItemViewModel.Network_Desc_B9 = wbEasyCalcData.Network_Desc_B9;
                    //ItemViewModel.Network_Desc_B10 = wbEasyCalcData.Network_Desc_B10;
                    //ItemViewModel.Interm_Area_B7 = wbEasyCalcData.Interm_Area_B7;
                    //ItemViewModel.Interm_Area_B8 = wbEasyCalcData.Interm_Area_B8;
                    //ItemViewModel.Interm_Area_B9 = wbEasyCalcData.Interm_Area_B9;
                    //ItemViewModel.Interm_Area_B10 = wbEasyCalcData.Interm_Area_B10;

                    //ItemViewModel.BilledCons_BilledMetConsBulkWatSupExpM3_D6 = wbEasyCalcData.BilledCons_BilledMetConsBulkWatSupExpM3_D6;               // @ZoneSale
                    //ItemViewModel.BilledCons_BilledUnmetConsBulkWatSupExpM3_H6  = wbEasyCalcData.BilledCons_BilledUnmetConsBulkWatSupExpM3_H6; 

                    //ItemViewModel.BilledCons_UnbMetConsM3_D8 = wbEasyCalcData.BilledCons_UnbMetConsM3_D8;
                    //ItemViewModel.BilledCons_UnbMetConsM3_D9 = wbEasyCalcData.BilledCons_UnbMetConsM3_D9;
                    //ItemViewModel.BilledCons_UnbMetConsM3_D10 = wbEasyCalcData.BilledCons_UnbMetConsM3_D10;
                    //ItemViewModel.BilledCons_UnbMetConsM3_D11 = wbEasyCalcData.BilledCons_UnbMetConsM3_D11;
                    //ItemViewModel.BilledCons_UnbUnmetConsM3_H8 = wbEasyCalcData.BilledCons_UnbUnmetConsM3_H8;
                    //ItemViewModel.BilledCons_UnbUnmetConsM3_H9 = wbEasyCalcData.BilledCons_UnbUnmetConsM3_H9;
                    //ItemViewModel.BilledCons_UnbUnmetConsM3_H10 = wbEasyCalcData.BilledCons_UnbUnmetConsM3_H10;
                    //ItemViewModel.BilledCons_UnbUnmetConsM3_H11 = wbEasyCalcData.BilledCons_UnbUnmetConsM3_H11;

                    //ItemViewModel.UnbilledCons_MetConsBulkWatSupExpM3_D6 = wbEasyCalcData.UnbilledCons_MetConsBulkWatSupExpM3_D6;
                    //ItemViewModel.UnbilledCons_UnbMetConsM3_D8 = wbEasyCalcData.UnbilledCons_UnbMetConsM3_D8;
                    //ItemViewModel.UnbilledCons_UnbMetConsM3_D9 = wbEasyCalcData.UnbilledCons_UnbMetConsM3_D9;
                    //ItemViewModel.UnbilledCons_UnbMetConsM3_D10 = wbEasyCalcData.UnbilledCons_UnbMetConsM3_D10;
                    //ItemViewModel.UnbilledCons_UnbMetConsM3_D11 = wbEasyCalcData.UnbilledCons_UnbMetConsM3_D11;
                    //ItemViewModel.UnbilledCons_UnbUnmetConsM3_H6 = wbEasyCalcData.UnbilledCons_UnbUnmetConsM3_H6;
                    //ItemViewModel.UnbilledCons_UnbUnmetConsM3_H7 = wbEasyCalcData.UnbilledCons_UnbUnmetConsM3_H7;
                    //ItemViewModel.UnbilledCons_UnbUnmetConsM3_H8 = wbEasyCalcData.UnbilledCons_UnbUnmetConsM3_H8;
                    //ItemViewModel.UnbilledCons_UnbUnmetConsM3_H9 = wbEasyCalcData.UnbilledCons_UnbUnmetConsM3_H9;
                    //ItemViewModel.UnbilledCons_UnbUnmetConsM3_H10 = wbEasyCalcData.UnbilledCons_UnbUnmetConsM3_H10;
                    //ItemViewModel.UnbilledCons_UnbUnmetConsM3_H11 = wbEasyCalcData.UnbilledCons_UnbUnmetConsM3_H11;
                    //ItemViewModel.UnbilledCons_UnbUnmetConsError_J6 = wbEasyCalcData.UnbilledCons_UnbUnmetConsError_J6;
                    //ItemViewModel.UnbilledCons_UnbUnmetConsError_J7 = wbEasyCalcData.UnbilledCons_UnbUnmetConsError_J7;
                    //ItemViewModel.UnbilledCons_UnbUnmetConsError_J8 = wbEasyCalcData.UnbilledCons_UnbUnmetConsError_J8;
                    //ItemViewModel.UnbilledCons_UnbUnmetConsError_J9 = wbEasyCalcData.UnbilledCons_UnbUnmetConsError_J9;
                    //ItemViewModel.UnbilledCons_UnbUnmetConsError_J10 = wbEasyCalcData.UnbilledCons_UnbUnmetConsError_J10;
                    //ItemViewModel.UnbilledCons_UnbUnmetConsError_J11 = wbEasyCalcData.UnbilledCons_UnbUnmetConsError_J11;

                    //ItemViewModel.UnauthCons_OthersErrorMargin_F18 = wbEasyCalcData.UnauthCons_OthersErrorMargin_F18;
                    //ItemViewModel.UnauthCons_OthersErrorMargin_F19 = wbEasyCalcData.UnauthCons_OthersErrorMargin_F19;
                    //ItemViewModel.UnauthCons_OthersErrorMargin_F20 = wbEasyCalcData.UnauthCons_OthersErrorMargin_F20;
                    //ItemViewModel.UnauthCons_OthersErrorMargin_F21 = wbEasyCalcData.UnauthCons_OthersErrorMargin_F21;
                    //ItemViewModel.UnauthCons_OthersM3PerDay_J18 = wbEasyCalcData.UnauthCons_OthersM3PerDay_J18;
                    //ItemViewModel.UnauthCons_OthersM3PerDay_J19 = wbEasyCalcData.UnauthCons_OthersM3PerDay_J19;
                    //ItemViewModel.UnauthCons_OthersM3PerDay_J20 = wbEasyCalcData.UnauthCons_OthersM3PerDay_J20;
                    //ItemViewModel.UnauthCons_OthersM3PerDay_J21 = wbEasyCalcData.UnauthCons_OthersM3PerDay_J21;

                    //ItemViewModel.UnauthCons_IllegalConnDomEstNo_D6 = wbEasyCalcData.UnauthCons_IllegalConnDomEstNo_D6;
                    //ItemViewModel.UnauthCons_IllegalConnDomPersPerHouse_H6 = wbEasyCalcData.UnauthCons_IllegalConnDomPersPerHouse_H6;
                    //ItemViewModel.UnauthCons_IllegalConnDomConsLitPerPersDay_J6 = wbEasyCalcData.UnauthCons_IllegalConnDomConsLitPerPersDay_J6;
                    //ItemViewModel.UnauthCons_IllegalConnDomErrorMargin_F6 = wbEasyCalcData.UnauthCons_IllegalConnDomErrorMargin_F6;
                    //ItemViewModel.UnauthCons_IllegalConnOthersErrorMargin_F10 = wbEasyCalcData.UnauthCons_IllegalConnOthersErrorMargin_F10;

                    //ItemViewModel.IllegalConnectionsOthersEstimatedNumber_D10 = wbEasyCalcData.IllegalConnectionsOthersEstimatedNumber_D10;
                    //ItemViewModel.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = wbEasyCalcData.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10;

                    //ItemViewModel.UnauthCons_MeterTampBypEtcEstNo_D14 = wbEasyCalcData.UnauthCons_MeterTampBypEtcEstNo_D14;
                    //ItemViewModel.UnauthCons_MeterTampBypEtcErrorMargin_F14 = wbEasyCalcData.UnauthCons_MeterTampBypEtcErrorMargin_F14;
                    //ItemViewModel.UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 = wbEasyCalcData.UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14;
                    //ItemViewModel.MetErrors_DetailedManualSpec_J6 = wbEasyCalcData.MetErrors_DetailedManualSpec_J6;
                    //ItemViewModel.MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 = wbEasyCalcData.MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8;
                    //ItemViewModel.MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 = wbEasyCalcData.MetErrors_BilledMetConsWoBulkSupErrorMargin_N8;

                    //ItemViewModel.MetErrors_Total_F12 = wbEasyCalcData.MetErrors_Total_F12;
                    //ItemViewModel.MetErrors_Total_F13 = wbEasyCalcData.MetErrors_Total_F13;
                    //ItemViewModel.MetErrors_Total_F14 = wbEasyCalcData.MetErrors_Total_F14;
                    //ItemViewModel.MetErrors_Total_F15 = wbEasyCalcData.MetErrors_Total_F15;
                    //ItemViewModel.MetErrors_Meter_H12 = wbEasyCalcData.MetErrors_Meter_H12;
                    //ItemViewModel.MetErrors_Meter_H13 = wbEasyCalcData.MetErrors_Meter_H13;
                    //ItemViewModel.MetErrors_Meter_H14 = wbEasyCalcData.MetErrors_Meter_H14;
                    //ItemViewModel.MetErrors_Meter_H15 = wbEasyCalcData.MetErrors_Meter_H15;
                    //ItemViewModel.MetErrors_Error_N12 = wbEasyCalcData.MetErrors_Error_N12;
                    //ItemViewModel.MetErrors_Error_N13 = wbEasyCalcData.MetErrors_Error_N13;
                    //ItemViewModel.MetErrors_Error_N14 = wbEasyCalcData.MetErrors_Error_N14;
                    //ItemViewModel.MetErrors_Error_N15 = wbEasyCalcData.MetErrors_Error_N15;

                    //ItemViewModel.MeteredBulkSupplyExportErrorMargin_N32 = wbEasyCalcData.MeteredBulkSupplyExportErrorMargin_N32;
                    //ItemViewModel.UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = wbEasyCalcData.UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34;
                    //ItemViewModel.CorruptMeterReadingPracticessErrorMargin_N38 = wbEasyCalcData.CorruptMeterReadingPracticessErrorMargin_N38;
                    //ItemViewModel.DataHandlingErrorsOffice_L40 = wbEasyCalcData.DataHandlingErrorsOffice_L40;
                    //ItemViewModel.DataHandlingErrorsOfficeErrorMargin_N40 = wbEasyCalcData.DataHandlingErrorsOfficeErrorMargin_N40;

                    //ItemViewModel.MetErrors_MetBulkSupExpMetUnderreg_H32 = wbEasyCalcData.MetErrors_MetBulkSupExpMetUnderreg_H32;
                    //ItemViewModel.MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34  = wbEasyCalcData.MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34;
                    //ItemViewModel.MetErrors_CorruptMetReadPractMetUndrreg_H38 = wbEasyCalcData.MetErrors_CorruptMetReadPractMetUndrreg_H38;
                    //ItemViewModel.Network_DistributionAndTransmissionMains_D7 = wbEasyCalcData.Network_DistributionAndTransmissionMains_D7;              // @NetworkLength 
                    //ItemViewModel.Network_DistributionAndTransmissionMains_D8 = wbEasyCalcData.Network_DistributionAndTransmissionMains_D8;
                    //ItemViewModel.Network_DistributionAndTransmissionMains_D9 = wbEasyCalcData.Network_DistributionAndTransmissionMains_D9;
                    //ItemViewModel.Network_DistributionAndTransmissionMains_D10 = wbEasyCalcData.Network_DistributionAndTransmissionMains_D10;
                    //ItemViewModel.Network_NoOfConnOfRegCustomers_H10 = wbEasyCalcData.Network_NoOfConnOfRegCustomers_H10;                                // @CustomersQuantity 
                    //ItemViewModel.Network_NoOfInactAccountsWSvcConns_H18 = wbEasyCalcData.Network_NoOfInactAccountsWSvcConns_H18;
                    //ItemViewModel.Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 = wbEasyCalcData.Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32;

                    //ItemViewModel.Network_PossibleUnd_D30 = wbEasyCalcData.Network_PossibleUnd_D30;
                    //ItemViewModel.Network_NoCustomers_H7 =  wbEasyCalcData.Network_NoCustomers_H7;
                    //ItemViewModel.Network_ErrorMargin_J7 =  wbEasyCalcData.Network_ErrorMargin_J7;
                    //ItemViewModel.Network_ErrorMargin_J10 = wbEasyCalcData.Network_ErrorMargin_J10;
                    //ItemViewModel.Network_ErrorMargin_J18 = wbEasyCalcData.Network_ErrorMargin_J18;
                    //ItemViewModel.Network_ErrorMargin_J32 = wbEasyCalcData.Network_ErrorMargin_J32;
                    //ItemViewModel.Network_ErrorMargin_D35 = wbEasyCalcData.Network_ErrorMargin_D35;

                    //ItemViewModel.FinancData_G6 =  wbEasyCalcData.FinancData_G6;
                    //ItemViewModel.FinancData_K6 =  wbEasyCalcData.FinancData_K6;
                    //ItemViewModel.FinancData_G8 =  wbEasyCalcData.FinancData_G8;
                    //ItemViewModel.FinancData_D26 = wbEasyCalcData.FinancData_D26;
                    //ItemViewModel.FinancData_G35 = wbEasyCalcData.FinancData_G35;

                    //ItemViewModel.Prs_Area_B7 = wbEasyCalcData.Prs_Area_B7;
                    //ItemViewModel.Prs_Area_B8 = wbEasyCalcData.Prs_Area_B8;
                    //ItemViewModel.Prs_Area_B9 = wbEasyCalcData.Prs_Area_B9;
                    //ItemViewModel.Prs_Area_B10 = wbEasyCalcData.Prs_Area_B10;
                    //ItemViewModel.Prs_ApproxNoOfConn_D7 = wbEasyCalcData.Prs_ApproxNoOfConn_D7;
                    //ItemViewModel.Prs_DailyAvgPrsM_F7 = wbEasyCalcData.Prs_DailyAvgPrsM_F7;
                    //ItemViewModel.Prs_ApproxNoOfConn_D8 = wbEasyCalcData.Prs_ApproxNoOfConn_D8;
                    //ItemViewModel.Prs_DailyAvgPrsM_F8 = wbEasyCalcData.Prs_DailyAvgPrsM_F8;
                    //ItemViewModel.Prs_ApproxNoOfConn_D9 = wbEasyCalcData.Prs_ApproxNoOfConn_D9;
                    //ItemViewModel.Prs_DailyAvgPrsM_F9 = wbEasyCalcData.Prs_DailyAvgPrsM_F9;
                    //ItemViewModel.Prs_ApproxNoOfConn_D10 = wbEasyCalcData.Prs_ApproxNoOfConn_D10;
                    //ItemViewModel.Prs_DailyAvgPrsM_F10 = wbEasyCalcData.Prs_DailyAvgPrsM_F10;
                    //ItemViewModel.Prs_ErrorMarg_F26 = wbEasyCalcData.Prs_ErrorMarg_F26;

                    //ItemViewModel.Interm_Conn_D7 =  wbEasyCalcData.Interm_Conn_D7;
                    //ItemViewModel.Interm_Conn_D8 =  wbEasyCalcData.Interm_Conn_D8;
                    //ItemViewModel.Interm_Conn_D9 =  wbEasyCalcData.Interm_Conn_D9;
                    //ItemViewModel.Interm_Conn_D10 = wbEasyCalcData.Interm_Conn_D10;
                    //ItemViewModel.Interm_Days_F7 =  wbEasyCalcData.Interm_Days_F7;
                    //ItemViewModel.Interm_Days_F8 =  wbEasyCalcData.Interm_Days_F8;
                    //ItemViewModel.Interm_Days_F9 =  wbEasyCalcData.Interm_Days_F9;
                    //ItemViewModel.Interm_Days_F10 = wbEasyCalcData.Interm_Days_F10;
                    //ItemViewModel.Interm_Hour_H7 =  wbEasyCalcData.Interm_Hour_H7;
                    //ItemViewModel.Interm_Hour_H8 =  wbEasyCalcData.Interm_Hour_H8;
                    //ItemViewModel.Interm_Hour_H9 =  wbEasyCalcData.Interm_Hour_H9;
                    //ItemViewModel.Interm_Hour_H10 = wbEasyCalcData.Interm_Hour_H10;

                }

                //ItemViewModel.CalculateExcelNew();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToExcel(string excelFileName)
        {
            try
            {
                EasyCalcModel easyCalcModel = ItemViewModel.Model.EasyCalcModel;
                GlobalConfig.WbEasyCalcExcel.SaveToExcelFile(excelFileName, easyCalcModel);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportFromExcel(string excelFileName)
        {
            try
            {
                EasyCalcModel easyCalcModel = GlobalConfig.WbEasyCalcExcel.LoadFromExcelFile(excelFileName);
                //ItemViewModel.Model.EasyCalcModel = null;
                //ItemViewModel.Model.EasyCalcModel = easyCalcModel;
                //easyCalcModelCalculateExcelNew();

                ItemViewModel.EasyCalcViewModel = new ExcelViewModel(easyCalcModel);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
