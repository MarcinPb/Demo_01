using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DataModel;
using DataRepository;
using GlobalRepository;
using WpfApplication1.Utility;
//using WpfApplication1.Ui.WbEasyCalcData;
using NLog;
using WpfApplication1.Ui.WbEasyCalcData.WaterConsumptionMap;

namespace WpfApplication1
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public Ui.WbEasyCalcData.ListViewModel WbEasyCalcDataViewModel { get; set; }
        public Ui.WaterConsumptionReport.EditedViewModel WaterConsumptionReportViewModel { get; set; }
        public Ui.Configuration.EditedViewModel ConfigurationViewModel { get; set; }


        public RelayCommand OptionsCmd { get; set; }
        private void OptionsCmdExecute()
        {
            DialogUtility.ShowModal(new Ui.Configuration.EditedViewModel());
        }




        public MainWindowViewModel()
        {
            Logger.Info("'MainWindowViewModel' started.");
            OptionsCmd = new RelayCommand(OptionsCmdExecute);

            GlobalConfig.InitializeConnection(DatabaseType.Sql);

            WbEasyCalcDataViewModel = new Ui.WbEasyCalcData.ListViewModel();
            WaterConsumptionReportViewModel = new Ui.WaterConsumptionReport.EditedViewModel();
        }

    }
}
