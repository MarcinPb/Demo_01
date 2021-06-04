using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using AutoMapper;
using Database.DataModel;
using Database.DataModel.Infra;
using Database.DataRepository.Infra;
using Database.DataRepository.WaterConsumption;
using GlobalRepository;
using Microsoft.WindowsAPICodePack.Dialogs;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Ui.WaterBalanceList.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WaterBalanceList
{
    public class EditedViewModel : ViewModelBase, IDialogViewModel, IDisposable
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

        #region Commands: LoadDataFromSystemCmd, ExportToExcelCmd, ExportToExcelCmd

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
            if (!ValidateItem()) { return false; }

            WbEasyCalcData model = GlobalConfig.DataRepository.WbEasyCalcDataListRepository.SaveItem(ItemViewModel.Model);
            Messenger.Default.Send<WbEasyCalcData>(model);
            return true;
        }

        /// <summary>
        /// Chacks two conditions. 
        /// Checks whether StartDate and EndDate each Water Consumption item are in a current Water Balance month (and year).
        /// checks whether objec where Water Consumption took place is in a current Water Balance zone.
        /// </summary>
        /// <returns>Success boolean value.</returns>
        private bool ValidateItem()
        {
            var yearNo = ItemViewModel.Model.YearNo;
            var monthNo = ItemViewModel.Model.MonthNo;
            if (ItemViewModel.Model.WaterConsumptionModelList.Any(x => (monthNo < 13 && (x.StartDate.Month != monthNo || x.EndDate.Month != monthNo)) || x.StartDate.Year != yearNo || x.EndDate.Year != yearNo))
            {
                MessageBox.Show("One of Water Consumption item has wrong Start or End Date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            const int ZoneFieldId = 647;
            var zoneId = ItemViewModel.Model.ZoneId;
            var objIdList = InfraRepo.GetInfraData().InfraChangeableData.InfraValueList.Where(f => f.FieldId == ZoneFieldId && f.IntValue== zoneId);                       
            if (!ItemViewModel.Model.WaterConsumptionModelList.All(x => objIdList.Any(y => x.RelatedId == y.ObjId)))
            {
                MessageBox.Show("One of Water Consumption item does not belong to a choosen Zone.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public void Close()
        {
        }

        #endregion

        public EditedViewModel(int id)
        {

            var model = GlobalConfig.DataRepository.WbEasyCalcDataListRepository.GetItem(id);

            //if (id == 0)
            //{
            //    model.EasyCalcModel.MatrixOneIn.SelectedOption = 1;
            //    model.EasyCalcModel.MatrixOneIn.C11 = 333;
            //}

            ItemViewModel = new ItemViewModel(model);

            YearList = GlobalConfig.DataRepository.YearList;
            MonthList = GlobalConfig.DataRepository.MonthList;
            ZoneItemList = GlobalConfig.DataRepository.ZoneList;



            J6_List = new List<IdNamePair> { 
                new IdNamePair(){ Id=1, Name="1"},
                new IdNamePair(){ Id=2, Name="2"},
            };
            Messenger.Default.Register<WaterConsumptionList.ListViewModel>(this, OnWaterConsumptionChangedReceived);
        }
        public void Dispose()
        {
            ItemViewModel.Dispose();
            Messenger.Default.Unregister(this);
        }

        private void OnWaterConsumptionChangedReceived(WaterConsumptionList.ListViewModel waterConsumptionListViewModel)
        {
            //ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_BilledMetConsBulkWatSupExpM3_D6 = sum;
            var waterConsumptionList = waterConsumptionListViewModel.List.Select(x => x.Model);
            var WaterConsumptionCategoryStatusExcelList = GlobalConfig.DataRepository.WaterConsumptionCategoryStatusExcelList;
            var list = waterConsumptionList.Join(
                WaterConsumptionCategoryStatusExcelList,
                l => l.WaterConsumptionCategoryId * 1000 + l.WaterConsumptionStatusId,
                r => r.CategoryId * 1000 + r.StatusId,
                (l, r) => new { r.ExcelCellName, l.Value, }
                );
            var grouppedList = list.GroupBy(g => g.ExcelCellName).Select(x => new { x.Key, Sum = x.Sum(c => c.Value) });

            // Clear all Excel cells filled by WaterConsumption sums.  
            List<string> keyList = GlobalConfig.DataRepository.WaterConsumptionCategoryStatusExcelList.Select(x => x.ExcelCellName).Distinct().ToList();
            foreach (var key in keyList)
            {
                if (key.StartsWith("Bil"))
                {
                    SetValue(ItemViewModel.EasyCalcViewModel.BilledConsViewModel, key, 0);
                }
                else
                {
                    SetValue(ItemViewModel.EasyCalcViewModel.UnbConsViewModel, key, 0);
                }
            }

            // Fill all Excel cells by WaterConsumption sums.  
            foreach (var item in grouppedList)
            {
                if (item.Key.StartsWith("Bil"))
                {
                    SetValue(ItemViewModel.EasyCalcViewModel.BilledConsViewModel, item.Key, item.Sum);
                }
                else
                { 
                    SetValue(ItemViewModel.EasyCalcViewModel.UnbConsViewModel, item.Key, item.Sum);
                }                
            }

            ItemViewModel.WaterConsumptionReportViewModel.WaterConsumptionList = GlobalConfig.DataRepository.WaterConsumptionListRepositoryTemp.GetList();
        }
        public void SetValue<T>(T obj, string propertyName, object value)
        {
            // these should be cached if possible
            Type type = typeof(T);
            PropertyInfo pi = type.GetProperty(propertyName);

            pi.SetValue(obj, Convert.ChangeType(value, pi.PropertyType), null);
        }

        private void LoadDataFromSystem()
        {
            try
            {
                if (ItemViewModel.MonthNo < 13)
                {
                    var wbEasyCalcData = GlobalConfig.DataRepository.GetWbGisModelScadaData(ItemViewModel.YearNo, ItemViewModel.MonthNo, ItemViewModel.ZoneId);
                    
                    ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_SystemInputVolumeM3_D6 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D6;                   
                    ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_UnbMetConsM3_D8 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D8;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_DistributionAndTransmissionMains_D7 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D7;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_NoCustomers_H7 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_NoCustomers_H7;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_NoOfConnOfRegCustomers_H10 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_NoOfConnOfRegCustomers_H10;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_NoOfInactAccountsWSvcConns_H18 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_NoOfInactAccountsWSvcConns_H18;
                    ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_DailyAvgPrsM_F7 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_DailyAvgPrsM_F7;
                }
                else
                {
                    // Sum parameters for all year.
                    var wbEasyCalcData = GlobalConfig.DataRepository.GetWbYearData(ItemViewModel.YearNo, ItemViewModel.ZoneId);

                    ItemViewModel.EasyCalcViewModel.StartViewModel.Start_PeriodDays_M21 = wbEasyCalcData.EasyCalcModel.StartModel.Start_PeriodDays_M21;

                    // Text
                    //ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_Desc_B6 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_Desc_B6;
                    //ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_Desc_B7 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_Desc_B7;
                    //ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_Desc_B8 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_Desc_B8;
                    //ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_Desc_B9 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_Desc_B9;                   
                    // Data
                    ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_SystemInputVolumeM3_D6 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D6;                // @SystemInputVolume
                    ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_SystemInputVolumeError_F6 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeError_F6;
                    ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_SystemInputVolumeM3_D7 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D7;
                    ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_SystemInputVolumeError_F7 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeError_F7;
                    ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_SystemInputVolumeM3_D8 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D8;
                    ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_SystemInputVolumeError_F8 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeError_F8;
                    ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_SystemInputVolumeM3_D9 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D9;
                    ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_SystemInputVolumeError_F9 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeError_F9;

                    // Text
                    //ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_Desc_B8 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_Desc_B8;
                    //ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_Desc_B9 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_Desc_B9;
                    //ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_Desc_B10 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_Desc_B10;
                    //ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_Desc_B11 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_Desc_B11;
                    //ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_Desc_F8 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_Desc_F8;
                    //ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_Desc_F9 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_Desc_F9;
                    //ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_Desc_F10 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_Desc_F10;
                    //ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_Desc_F11 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_Desc_F11;
                    // Data
                    ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_BilledMetConsBulkWatSupExpM3_D6 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_BilledMetConsBulkWatSupExpM3_D6;               // @ZoneSale
                    ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_BilledUnmetConsBulkWatSupExpM3_H6;
                    ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_UnbMetConsM3_D8 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D8;
                    ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_UnbMetConsM3_D9 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D9;
                    ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_UnbMetConsM3_D10 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D10;
                    ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_UnbMetConsM3_D11 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D11;
                    ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_UnbUnmetConsM3_H8 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H8;
                    ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_UnbUnmetConsM3_H9 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H9;
                    ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_UnbUnmetConsM3_H10 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H10;
                    ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_UnbUnmetConsM3_H11 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H11;

                    // Text
                    //ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_Desc_D8 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_D8;
                    //ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_Desc_D9 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_D9;
                    //ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_Desc_D10 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_D10;
                    //ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_Desc_D11 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_D11;
                    //ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_Desc_F6 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_F6;
                    //ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_Desc_F7 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_F7;
                    //ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_Desc_F8 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_F8;
                    //ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_Desc_F9 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_F9;
                    //ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_Desc_F10 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_F10;
                    //ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_Desc_F11 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_F11;
                    // Data
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_MetConsBulkWatSupExpM3_D6 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_MetConsBulkWatSupExpM3_D6;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbMetConsM3_D8 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D8;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbMetConsM3_D9 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D9;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbMetConsM3_D10 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D10;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbMetConsM3_D11 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D11;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbUnmetConsM3_H6 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H6;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbUnmetConsM3_H7 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H7;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbUnmetConsM3_H8 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H8;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbUnmetConsM3_H9 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H9;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbUnmetConsM3_H10 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H10;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbUnmetConsM3_H11 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H11;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbUnmetConsError_J6 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J6;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbUnmetConsError_J7 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J7;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbUnmetConsError_J8 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J8;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbUnmetConsError_J9 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J9;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbUnmetConsError_J10 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J10;
                    ItemViewModel.EasyCalcViewModel.UnbConsViewModel.UnbilledCons_UnbUnmetConsError_J11 = wbEasyCalcData.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J11;

                    // Text
                    //ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_Desc_B18 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_Desc_B18;
                    //ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_Desc_B19 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_Desc_B19;
                    //ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_Desc_B20 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_Desc_B20;
                    //ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_Desc_B21 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_Desc_B21;
                    // Data
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_OthersErrorMargin_F18 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_OthersErrorMargin_F18;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_OthersErrorMargin_F19 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_OthersErrorMargin_F19;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_OthersErrorMargin_F20 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_OthersErrorMargin_F20;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_OthersErrorMargin_F21 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_OthersErrorMargin_F21;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_OthersM3PerDay_J18 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_OthersM3PerDay_J18;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_OthersM3PerDay_J19 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_OthersM3PerDay_J19;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_OthersM3PerDay_J20 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_OthersM3PerDay_J20;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_OthersM3PerDay_J21 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_OthersM3PerDay_J21;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_IllegalConnDomEstNo_D6 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_IllegalConnDomEstNo_D6;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_IllegalConnDomPersPerHouse_H6 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_IllegalConnDomPersPerHouse_H6;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_IllegalConnDomConsLitPerPersDay_J6 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_IllegalConnDomConsLitPerPersDay_J6;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_IllegalConnDomErrorMargin_F6 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_IllegalConnDomErrorMargin_F6;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_IllegalConnOthersErrorMargin_F10 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_IllegalConnOthersErrorMargin_F10;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.IllegalConnectionsOthersEstimatedNumber_D10 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.IllegalConnectionsOthersEstimatedNumber_D10;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_MeterTampBypEtcEstNo_D14 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_MeterTampBypEtcEstNo_D14;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_MeterTampBypEtcErrorMargin_F14 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_MeterTampBypEtcErrorMargin_F14;
                    ItemViewModel.EasyCalcViewModel.UnauthConsViewModel.UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 = wbEasyCalcData.EasyCalcModel.UnauthConsModel.UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14;

                    // Text
                    //ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Desc_D12 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Desc_D12;
                    //ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Desc_D13 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Desc_D13;
                    //ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Desc_D14 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Desc_D14;
                    //ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Desc_D15 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Desc_D15;
                    // Data
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Total_F12 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Total_F12;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Total_F13 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Total_F13;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Total_F14 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Total_F14;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Total_F15 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Total_F15;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Meter_H12 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Meter_H12;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Meter_H13 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Meter_H13;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Meter_H14 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Meter_H14;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Meter_H15 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Meter_H15;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Error_N12 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Error_N12;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Error_N13 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Error_N13;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Error_N14 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Error_N14;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_Error_N15 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_Error_N15;
                    //ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_DetailedManualSpec_J6 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_DetailedManualSpec_J6;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_BilledMetConsWoBulkSupErrorMargin_N8;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MeteredBulkSupplyExportErrorMargin_N32 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MeteredBulkSupplyExportErrorMargin_N32;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.CorruptMeterReadingPracticessErrorMargin_N38 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.CorruptMeterReadingPracticessErrorMargin_N38;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.DataHandlingErrorsOffice_L40 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.DataHandlingErrorsOffice_L40;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.DataHandlingErrorsOfficeErrorMargin_N40 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.DataHandlingErrorsOfficeErrorMargin_N40;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_MetBulkSupExpMetUnderreg_H32 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_MetBulkSupExpMetUnderreg_H32;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34;
                    ItemViewModel.EasyCalcViewModel.MeterErrorsViewModel.MetErrors_CorruptMetReadPractMetUndrreg_H38 = wbEasyCalcData.EasyCalcModel.MetErrorsModel.MetErrors_CorruptMetReadPractMetUndrreg_H38;

                    // Text
                    //ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_Desc_B7 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_Desc_B7;
                    //ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_Desc_B8 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_Desc_B8;
                    //ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_Desc_B9 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_Desc_B9;
                    //ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_Desc_B10 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_Desc_B10;
                    // Data
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_DistributionAndTransmissionMains_D7 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D7;              // @NetworkLength 
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_DistributionAndTransmissionMains_D8 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D8;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_DistributionAndTransmissionMains_D9 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D9;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_DistributionAndTransmissionMains_D10 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D10;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_NoOfConnOfRegCustomers_H10 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_NoOfConnOfRegCustomers_H10;                                // @CustomersQuantity 
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_NoOfInactAccountsWSvcConns_H18 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_NoOfInactAccountsWSvcConns_H18;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_PossibleUnd_D30 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_PossibleUnd_D30;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_NoCustomers_H7 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_NoCustomers_H7;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_ErrorMargin_J7 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_ErrorMargin_J7;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_ErrorMargin_J10 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_ErrorMargin_J10;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_ErrorMargin_J18 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_ErrorMargin_J18;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_ErrorMargin_J32 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_ErrorMargin_J32;
                    ItemViewModel.EasyCalcViewModel.NetworkViewModel.Network_ErrorMargin_D35 = wbEasyCalcData.EasyCalcModel.NetworkModel.Network_ErrorMargin_D35;

                    // Text
                    //ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_Area_B7 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_Area_B7;
                    //ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_Area_B8 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_Area_B8;
                    //ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_Area_B9 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_Area_B9;
                    //ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_Area_B10 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_Area_B10;
                    // Data
                    //ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_ApproxNoOfConn_D7 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_ApproxNoOfConn_D7;
                    ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_DailyAvgPrsM_F7 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_DailyAvgPrsM_F7;
                    ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_ApproxNoOfConn_D8 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_ApproxNoOfConn_D8;
                    ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_DailyAvgPrsM_F8 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_DailyAvgPrsM_F8;
                    ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_ApproxNoOfConn_D9 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_ApproxNoOfConn_D9;
                    ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_DailyAvgPrsM_F9 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_DailyAvgPrsM_F9;
                    ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_ApproxNoOfConn_D10 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_ApproxNoOfConn_D10;
                    ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_DailyAvgPrsM_F10 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_DailyAvgPrsM_F10;
                    ItemViewModel.EasyCalcViewModel.PressureViewModel.Prs_ErrorMarg_F26 = wbEasyCalcData.EasyCalcModel.PressureModel.Prs_ErrorMarg_F26;

                    // Text
                    //ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Area_B7 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Area_B7;
                    //ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Area_B8 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Area_B8;
                    //ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Area_B9 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Area_B9;
                    //ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Area_B10 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Area_B10;
                    // Data
                    ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Conn_D7 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Conn_D7;
                    ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Conn_D8 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Conn_D8;
                    ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Conn_D9 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Conn_D9;
                    ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Conn_D10 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Conn_D10;
                    ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Days_F7 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Days_F7;
                    ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Days_F8 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Days_F8;
                    ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Days_F9 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Days_F9;
                    ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Days_F10 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Days_F10;
                    ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Hour_H7 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Hour_H7;
                    ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Hour_H8 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Hour_H8;
                    ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Hour_H9 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Hour_H9;
                    ItemViewModel.EasyCalcViewModel.IntermittentSupplyViewModel.Interm_Hour_H10 = wbEasyCalcData.EasyCalcModel.IntermModel.Interm_Hour_H10;

                    // Data
                    //ItemViewModel.EasyCalcViewModel.FinancialDataViewModel.FinancData_G6 = wbEasyCalcData.EasyCalcModel.FinancDataModel.FinancData_G6;
                    //ItemViewModel.EasyCalcViewModel.FinancialDataViewModel.FinancData_K6 = wbEasyCalcData.EasyCalcModel.FinancDataModel.FinancData_K6;
                    //ItemViewModel.EasyCalcViewModel.FinancialDataViewModel.FinancData_G8 = wbEasyCalcData.EasyCalcModel.FinancDataModel.FinancData_G8;
                    ItemViewModel.EasyCalcViewModel.FinancialDataViewModel.FinancData_D26 = wbEasyCalcData.EasyCalcModel.FinancDataModel.FinancData_D26;
                    ItemViewModel.EasyCalcViewModel.FinancialDataViewModel.FinancData_G35 = wbEasyCalcData.EasyCalcModel.FinancDataModel.FinancData_G35;

                    // list 
                    GlobalConfig.DataRepository.WaterConsumptionListRepositoryTemp = new ListRepositoryTemp(wbEasyCalcData.WaterConsumptionModelList);
                    ItemViewModel.WaterConsumptionListViewModel.List = new ObservableCollection<WaterConsumptionList.RowViewModel>(wbEasyCalcData.WaterConsumptionModelList.Select(x => new WaterConsumptionList.RowViewModel(x)));


                    //ItemViewModel.EasyCalcViewModel.SysInputViewModel.SysInput_Desc_B6 = wbEasyCalcData.EasyCalcModel.SysInputModel.SysInput_Desc_B6;
                    //var config = new MapperConfiguration(cfg => cfg.CreateMap<SysInputModel, Excel.SysInput.ViewModel>());
                    //var mapper = config.CreateMapper();
                    //ItemViewModel.EasyCalcViewModel.SysInputViewModel = mapper.Map<Excel.SysInput.ViewModel>(wbEasyCalcData.EasyCalcModel.SysInputModel);

                    //ItemViewModel.EasyCalcViewModel.BilledConsViewModel.BilledCons_BilledMetConsBulkWatSupExpM3_D6 = wbEasyCalcData.EasyCalcModel.BilledConsModel.BilledCons_BilledMetConsBulkWatSupExpM3_D6;               // @ZoneSale
                    //config = new MapperConfiguration(cfg => cfg.CreateMap<BilledConsModel, Excel.BilledCons.ViewModel>());
                    //mapper = config.CreateMapper();
                    //ItemViewModel.EasyCalcViewModel.BilledConsViewModel = mapper.Map<Excel.BilledCons.ViewModel>(wbEasyCalcData.EasyCalcModel.BilledConsModel);

                    //var config = new MapperConfiguration(cfg => {
                    //    cfg.CreateMap<EasyCalcModel, ExcelViewModel>();
                    //    cfg.CreateMap<SysInputModel, Excel.SysInput.ViewModel>();
                    //});
                    //var mapper = config.CreateMapper();
                    //ItemViewModel.EasyCalcViewModel = mapper.Map<ExcelViewModel>(wbEasyCalcData.EasyCalcModel);

                    //IMapper mapper;
                    //mapper = new MapperConfiguration(cfg => cfg.CreateMap<SysInputModel, Excel.SysInput.ViewModel>()).CreateMapper();
                    //ItemViewModel.EasyCalcViewModel.SysInputViewModel = mapper.Map<Excel.SysInput.ViewModel>(wbEasyCalcData.EasyCalcModel.SysInputModel);
                    //mapper = new MapperConfiguration(cfg => cfg.CreateMap<BilledConsModel, Excel.BilledCons.ViewModel>()).CreateMapper();
                    //ItemViewModel.EasyCalcViewModel.BilledConsViewModel = mapper.Map<Excel.BilledCons.ViewModel>(wbEasyCalcData.EasyCalcModel.BilledConsModel);

                    //ItemViewModel.EasyCalcViewModel.BaseSheetViewModelCalculate();
                }

                //ItemViewModel.CalculateExcelNew();
                MessageBox.Show("GIS, Model (WaterGEMS) and SCADA data were imported succefully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBox.Show("Data were exported to the Excel file succefully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Loads data from an Excel file to a new EasyCalcModel model. 
        /// Loads MatrixOneIn and MatrixTwoIn from a currently openned EasyCalcModel model to a new created EasyCalcModel model.
        /// Loads a new created EasyCalcModel model to a currently openned EasyCalcModel model.
        /// </summary>
        /// <param name="excelFileName">Full name imported Excel file.</param>
        private void ImportFromExcel(string excelFileName)
        {
            try
            {
                if (ItemViewModel.EasyCalcViewModel != null) 
                { 
                    ItemViewModel.EasyCalcViewModel.Dispose();  
                }
                EasyCalcModel easyCalcModel = GlobalConfig.WbEasyCalcExcel.LoadFromExcelFile(excelFileName);
                
                easyCalcModel.MatrixOneIn = ItemViewModel.EasyCalcViewModel.Model.MatrixOneIn;
                easyCalcModel.MatrixTwoIn = ItemViewModel.EasyCalcViewModel.Model.MatrixTwoIn;

                ItemViewModel.EasyCalcViewModel = new ExcelViewModel(easyCalcModel);

                MessageBox.Show("Data were imported from the Excel file succefully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
