using Database.DataModel;
using GlobalRepository;
using WpfApplication1.Utility;
using NLog;
using WpfApplication1.Ui.Designer.Repo;
using WpfApplication1.Ui.WaterConsumptionMap;
using System;
using System.Windows;
using WpfApplication1.Ui.ImportFromWg;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using Database.DataRepository.Infra;
using ExcelNpoi.ExcelNpoi;

namespace WpfApplication1
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region StatusBar Prop: DatabaseName, SqliteFile

        private string _databaseName;
        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; RaisePropertyChanged(); }
        }

        private string _sqliteFile;
        public string SqliteFile 
        {
            get { return _sqliteFile; }
            set { _sqliteFile = value; RaisePropertyChanged(); }
        }

        #endregion

        #region ViewModel Prop: WbEasyCalcDataViewModel, WaterConsumptionMapViewModel, DemandPatternViewModel

        public Ui.WaterBalanceList.ListViewModel WbEasyCalcDataViewModel { get; set; }
        public MapViewModel WaterConsumptionMapViewModel { get; set; }
        public Ui.DemandPattern.ListViewModel DemandPatternViewModel { get; set; }

        #endregion

        #region Commad Prop: ExitCmd, ShowDemandPatternListCmd, ImportConstantDataCmd, ImportChangeableDataCmd, CreatePostCalcExcelCmd, OptionsCmd,

        public RelayCommand ExitCmd { get; set; }
        private void ExitCmdExecute()
        {
            Application.Current.Shutdown();
        }


        public RelayCommand ShowDemandPatternListCmd { get; set; }
        private void ShowDemandPatternListCmdExecute()
        {
            DialogUtility.ShowModal(new Ui.DemandPattern.ListViewModel());
        }

        public RelayCommand CreatePostCalcExcelCmd { get; set; }
        private void CreatePostCalcExcelCmdExecute()
        {
            try 
            { 
                CommonSaveFileDialog dialog = new CommonSaveFileDialog();
                dialog.Filters.Add(new CommonFileDialogFilter()
                {
                    DisplayName = "Excel files",
                    Extensions = { "xlsx" }
                });
                dialog.DefaultExtension = "xlsx";
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    InfraData infraData = InfraRepo.GetInfraData();
                    PostCalcExcelWriter.Write(
                        dialog.FileName,
                        infraData.InfraChangeableData,
                        infraData.InfraSpecialFieldId
                        );
                    MessageBox.Show($"{dialog.FileName} file was created succesfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public RelayCommand ImportConstantDataCmd { get; set; }
        private void ImportConstantDataCmdExecute()
        {
            DialogUtility.ShowModal(new ImportConstantDataViewModel());
        }


        public RelayCommand ImportChangeableDataCmd { get; set; }
        private void ImportChangeableDataCmdExecute()
        {
            DialogUtility.ShowModal(new ImportChangeableDataViewModel());
        }


        public RelayCommand OptionsCmd { get; set; }
        private void OptionsCmdExecute()
        {
            DialogUtility.ShowModal(new Ui.Configuration.EditedViewModel());
        }

        #endregion

        public MainWindowViewModel()
        {
            try 
            { 
                Logger.Info("'MainWindowViewModel' started.");

                ExitCmd = new RelayCommand(ExitCmdExecute);

                ShowDemandPatternListCmd = new RelayCommand(ShowDemandPatternListCmdExecute);
                ImportConstantDataCmd = new RelayCommand(ImportConstantDataCmdExecute);
                ImportChangeableDataCmd = new RelayCommand(ImportChangeableDataCmdExecute);
                CreatePostCalcExcelCmd = new RelayCommand(CreatePostCalcExcelCmdExecute);
                OptionsCmd = new RelayCommand(OptionsCmdExecute);


                GlobalConfig.InitializeConnection(DatabaseType.Sql);

                WbEasyCalcDataViewModel = new Ui.WaterBalanceList.ListViewModel();

                WaterConsumptionMapViewModel = new Ui.WaterConsumptionMap.MapViewModel(2021, 5, null);
                WaterConsumptionMapViewModel.WaterConsumptionList = GlobalConfig.DataRepository.WaterConsumptionListRepository.GetList();

                // A "Demand Pattern List" tab was deleted. A new "Demand Pattern List" menu option was created instead. 
                //DemandPatternViewModel = new Ui.DemandPattern.ListViewModel();

                // Singleton run before opening designer first time. It takes more or less 5 sek.
                InvokeSingleton();

                Messenger.Default.Register<Ui.WaterBalanceList.ListViewModel>(this, OnSaveOrDeleteModel);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void InvokeSingleton()
        {
            await Task.Run(() => InvokeSingletonInner());
        }
        public void InvokeSingletonInner()
        {
            var x = DesignerRepo.DesignerObjList;
            DatabaseName = $"{GetDatabaseName("WaterInfra_ConnStr")}, {GetDatabaseName("WaterUtility_ConnStr")}";
            SqliteFile = GetSqliteFile();
        }
        private string GetDatabaseName(string name)
        {
            var connString = GetConnectionString(name);
            var databaseName = connString.Split(';')[1].Split('=')[1];

            return databaseName;
        }
        private string GetConnectionString(string name)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        private string GetSqliteFile()
        {
            return System.Configuration.ConfigurationManager.AppSettings["SqliteFile"]; ;
        }


        private void OnSaveOrDeleteModel(Ui.WaterBalanceList.ListViewModel model)
        {
            WaterConsumptionMapViewModel.WaterConsumptionList = GlobalConfig.DataRepository.WaterConsumptionListRepository.GetList();
        }
    }
}
