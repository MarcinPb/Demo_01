using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Database.DataModel;

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
using WpfApplication1.Ui.WaterBalanceList.Excel.MatrixIn;

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
                MeterErrorsModel = MeterErrorsViewModel.Model,
                FinancDataModel = FinancialDataViewModel.Model,
                MatrixOneInModel = MatrixOneInViewModel.Model, 
                MatrixTwoInModel = MatrixTwoInViewModel.Model, 
            });
            return true;
        }

        public void Close()
        {
        }

        #endregion

        public WaterBalanceList.Excel.MeterErrors.ViewModel MeterErrorsViewModel { get; set; }
        public WaterBalanceList.Excel.FinancialData.ViewModel FinancialDataViewModel { get; set; }
        public ViewModel MatrixOneInViewModel { get; set; }
        public ViewModel MatrixTwoInViewModel { get; set; }

        public EditedViewModel()
        {
            var model = GlobalConfig.DataRepository.Option.GetItem(0);
            MeterErrorsViewModel = new WaterBalanceList.Excel.MeterErrors.ViewModel(model.MeterErrorsModel);
            FinancialDataViewModel = new WaterBalanceList.Excel.FinancialData.ViewModel(model.FinancDataModel);
            MatrixOneInViewModel = new WaterBalanceList.Excel.MatrixIn.ViewModel(model.MatrixOneInModel);
            MatrixTwoInViewModel = new WaterBalanceList.Excel.MatrixIn.ViewModel(model.MatrixTwoInModel);
        }
    }
}
