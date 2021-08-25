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

namespace WpfApplication1.Ui.TableCustomerMeter
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
            return true;
        }

        public void Close()
        {
        }

        #endregion

        public EditedViewModel(int id)
        {

            var model = InfraRepo.GetInfraData().InfraChangeableData.DemandPatternDict.FirstOrDefault(x => x.DemandPatternId == id);
            ItemViewModel = new ItemViewModel(model);
        }

        public void Dispose()
        {
            ItemViewModel.Dispose();
        }
    }
}
