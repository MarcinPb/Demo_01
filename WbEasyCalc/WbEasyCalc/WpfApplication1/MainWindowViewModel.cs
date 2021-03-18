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
using WpfApplication1.Ui.WbEasyCalcData;
using NLog;

namespace WpfApplication1
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public Ui.WbEasyCalcData.ListViewModel WbEasyCalcDataViewModel { get; set; }
        public Ui.WaterConsumptionReport.EditedViewModel WaterConsumptionReportViewModel { get; set; }
        public Ui.Configuration.EditedViewModel ConfigurationViewModel { get; set; }

        public MainWindowViewModel()
        {
            Logger.Info("'MainWindowViewModel' started.");

            GlobalConfig.InitializeConnection(DatabaseType.Sql);
            WbEasyCalcDataViewModel = new Ui.WbEasyCalcData.ListViewModel();
            WaterConsumptionReportViewModel = new Ui.WaterConsumptionReport.EditedViewModel();
            ConfigurationViewModel = new Ui.Configuration.EditedViewModel();
        }

    }
}
