using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DataModel;
using DataRepository;
using GlobalRepository;
using Microsoft.WindowsAPICodePack.Dialogs;
using WbEasyCalcModel;
using WpfApplication1.Utility;
using Microsoft.Maps.MapControl.WPF;
using System.Collections.ObjectModel;
using WpfApplication1.Map;
using System.Windows.Input;
using System.Windows.Controls;
using NLog;
using WbEasyCalcModel.WbEasyCalc;

namespace WpfApplication1.Ui.Configuration
{
    public class EditedViewModel : ViewModelBase, IDialogViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region IDialogViewModel

        public string Title { get; set; } = "Option";

        public bool Save()
        {
            GlobalConfig.DataRepository.Option.SaveItem(new Option() 
            { 
                FinancDataModel = FinancialDataViewModel.Model,
                MatrixOneInModel = MatrixOneInViewModel.Model, 
            });
            return true;
        }

        public void Close()
        {
        }

        #endregion

        public WbEasyCalcData.Excel.FinancialData.ViewModel FinancialDataViewModel { get; set; }
        public WbEasyCalcData.Excel.MatrixOneIn.ViewModel MatrixOneInViewModel { get; set; }

        public EditedViewModel()
        {
            var model = GlobalConfig.DataRepository.Option.GetItem(0);
            FinancialDataViewModel = new WbEasyCalcData.Excel.FinancialData.ViewModel(model.FinancDataModel);
            MatrixOneInViewModel = new WbEasyCalcData.Excel.MatrixOneIn.ViewModel(model.MatrixOneInModel);
        }
    }
}
