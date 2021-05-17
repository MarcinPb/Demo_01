using Database.DataModel;
using GlobalRepository;
using WpfApplication1.Utility;
using NLog;
using WpfApplication1.Ui.Designer.Repo;

namespace WpfApplication1
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public Ui.WbEasyCalcData.ListViewModel WbEasyCalcDataViewModel { get; set; }
        public Ui.WaterConsumptionReport.EditedViewModel WaterConsumptionReportViewModel { get; set; }

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

            // Singleton run before opening designer first time. It takes more or less 5 sek.
            var designerObjList1 = DesignerRepo.DesignerObjList;
        }

    }
}
