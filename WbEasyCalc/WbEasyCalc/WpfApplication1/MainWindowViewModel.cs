using Database.DataModel;
using GlobalRepository;
using WpfApplication1.Utility;
using NLog;
using WpfApplication1.Ui.Designer.Repo;
using WpfApplication1.Ui.WaterConsumptionMap;
using System;
using System.Windows;

namespace WpfApplication1
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public Ui.WaterBalanceList.ListViewModel WbEasyCalcDataViewModel { get; set; }
        //public Ui.WaterConsumptionReport.EditedViewModel WaterConsumptionReportViewModel { get; set; }
        public MapViewModel WaterConsumptionMapViewModel { get; set; }

        public RelayCommand OptionsCmd { get; set; }
        private void OptionsCmdExecute()
        {
            DialogUtility.ShowModal(new Ui.Configuration.EditedViewModel());
        }

        public MainWindowViewModel()
        {
            try 
            { 
                Logger.Info("'MainWindowViewModel' started.");
                OptionsCmd = new RelayCommand(OptionsCmdExecute);

                GlobalConfig.InitializeConnection(DatabaseType.Sql);

                WbEasyCalcDataViewModel = new Ui.WaterBalanceList.ListViewModel();

                //WaterConsumptionReportViewModel = new Ui.WaterConsumptionReport.EditedViewModel();
                WaterConsumptionMapViewModel = new Ui.WaterConsumptionMap.MapViewModel(2021, 5, null);
                WaterConsumptionMapViewModel.WaterConsumptionList = GlobalConfig.DataRepository.WaterConsumptionListRepository.GetList();

                // Singleton run before opening designer first time. It takes more or less 5 sek.
                var designerObjList1 = DesignerRepo.DesignerObjList;

                Messenger.Default.Register<Ui.WaterBalanceList.ListViewModel>(this, OnSaveOrDeleteModel);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnSaveOrDeleteModel(Ui.WaterBalanceList.ListViewModel model)
        {
            WaterConsumptionMapViewModel.WaterConsumptionList = GlobalConfig.DataRepository.WaterConsumptionListRepository.GetList();
        }
    }
}
