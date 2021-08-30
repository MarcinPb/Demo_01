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

namespace WpfApplication1.Ui.DemandPattern
{
    public class EditedViewModel : ViewModelBase, IDialogViewModel, IDisposable
    {
        private ItemViewModel _model;
        public ItemViewModel ItemViewModel
        {
            get => _model;
            set { _model = value; RaisePropertyChanged(); }
        }

        #region IDialogViewModel

        public string Title { get; set; } = "Demand Patern";

        public bool Save()
        {
            try
            {
                InfraRepo.ExcludedDemmandPattern.SaveItem(ItemViewModel.Id, ItemViewModel.IsExcluded);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public void Close()
        {
        }

        #endregion

        public EditedViewModel(RowViewModel rowViewModel)
        {
            var model = InfraRepo.GetInfraData().InfraChangeableData.DemandPatternDict.FirstOrDefault(x => x.DemandPatternId == rowViewModel.Model.DemandPatternId);
            var isExcluded = rowViewModel.IsExcluded;
            ItemViewModel = new ItemViewModel(model, isExcluded);
        }

        public void Dispose()
        {
            ItemViewModel.Dispose();
        }
    }
}
