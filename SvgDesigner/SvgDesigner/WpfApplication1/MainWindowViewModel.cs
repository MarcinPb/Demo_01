using Database.DataModel.Infra;
using Database.DataRepository.Infra;
using GeometryReader;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfApplication1.Ui.Designer;
using WpfApplication1.Ui.Designer.Model;
using WpfApplication1.Ui.Designer.Model.ShapeModel;
using WpfApplication1.Ui.Designer.Repo;
using WpfApplication1.Ui.DesignerWithPropreryGrid;
using WpfApplication1.Utility;

namespace WpfApplication1
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Data Import

        private string _databaseName;
        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; RaisePropertyChanged(); }
        }

        public string SqliteFile { get; set; }

        private double _progressPercent;
        public double ProgressPercent
        {
            get { return _progressPercent; }
            set { _progressPercent = value; RaisePropertyChanged(nameof(ProgressPercent)); }
        }
        private string _progressMessage;
        public string ProgressMessage
        {
            get { return _progressMessage; }
            set { _progressMessage = value; RaisePropertyChanged(nameof(ProgressMessage)); }
        }
        private double _innerProgressPercent;
        public double InnerProgressPercent
        {
            get { return _innerProgressPercent; }
            set { _innerProgressPercent = value; RaisePropertyChanged(nameof(InnerProgressPercent)); }
        }
        private string _innerProgressMessage;
        public string InnerProgressMessage
        {
            get { return _innerProgressMessage; }
            set { _innerProgressMessage = value; RaisePropertyChanged(nameof(InnerProgressMessage)); }
        }


        public RelayCommand ImportConstantDataCmd { get; }
        private void ImportConstantDataCmdExecute()
        {
            var importer = new Importer();
            var importedBaseOutputLists = importer.ImportBase(SqliteFile);
            InfraRepo.InsertToInfraObjType(importedBaseOutputLists.InfraObjTypeList);
            InfraRepo.InsertToInfraField(importedBaseOutputLists.ImportedFieldList);
        }

        public RelayCommand<object> ImportChangableDataCmd { get; }
        private async void ImportChangableDataCmdExecute(object obj)
        {
            InfraConstantDataLists infraConstantDataLists = InfraRepo.GetInfraConstantData();

            var importer = new Importer();
            importer.ProgressChanged += OnProgressChanged;
            importer.InnerProgressChanged += OnInnerProgressChanged;
            InfraChangeableDataLists importedDataOutputLists = await Task<int>.Run(() => importer.ImportData(SqliteFile, infraConstantDataLists));
            //importer.InnerProgressChanged -= OnInnerProgressChanged;
            //importer.ProgressChanged -= OnProgressChanged;

            InfraRepo.InsertToInfraZone(importedDataOutputLists.ZoneDict);                      //  14601
            InfraRepo.InsertToInfraDemandPattern(importedDataOutputLists.DemandPatternDict);    //  
            InfraRepo.InsertToInfraObj(importedDataOutputLists.InfraObjList);                   //     16
            InfraRepo.InsertToInfraValue(importedDataOutputLists.InfraValueList);               // 518964
            InfraRepo.InsertToInfraGeometry(importedDataOutputLists.InfraGeometryList);         //  25138
        }

        private void OnInnerProgressChanged(object sender, GeometryReader.ProgressEventArgs e)
        {
            InnerProgressPercent = e.ProgressRatio;
            InnerProgressMessage = e.Message;
        }

        private void OnProgressChanged(object sender, GeometryReader.ProgressEventArgs e)
        {
            ProgressPercent = e.ProgressRatio;
            ProgressMessage = e.Message;
        }

        #endregion

        public EditedViewModel DesignerViewModel { get; set; }

        #region Open Designer

        public RelayCommand OpenDesignerCmd { get; }
        private void OpenRowCmdExecute()
        {
            OpenRowNextCmdExecute(6773);
        }
        public bool OpenRowCmdCanExecute()
        {
            return true;
        }

        public RelayCommand OpenDesignerNextCmd { get; }
        private void OpenRowNextCmdExecute()
        {
            OpenRowNextCmdExecute(6774);
        }
        public bool OpenRowNextCmdCanExecute()
        {
            return true;
        }

        private Shp _pushPinPoint;
        private void OpenRowNextCmdExecute(int zoneId)
        {
            var editedViewModel = new EditedViewModel(zoneId, _pushPinPoint);
            var result = DialogUtility.ShowModal(editedViewModel);
            if (result == true)
            {
                _pushPinPoint = editedViewModel.PushPin;
                MessageBox.Show($"Water consumption location: X={_pushPinPoint.X}, Y={_pushPinPoint.Y}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);         
            }
            //editedViewModel.Dispose();
        }

        #endregion


        public MainWindowViewModel()
        {
            ImportChangableDataCmd = new RelayCommand<object>(ImportChangableDataCmdExecute);
            ImportConstantDataCmd = new RelayCommand(ImportConstantDataCmdExecute);
            OpenDesignerCmd = new RelayCommand(OpenRowCmdExecute, OpenRowCmdCanExecute);
            OpenDesignerNextCmd = new RelayCommand(OpenRowNextCmdExecute, OpenRowNextCmdCanExecute);

            SqliteFile = GetSqliteFile();
            DatabaseName = GetDatabaseName("WaterInfra_5_ConnStr");

            // Singleton run before opening designer first time. It takes more or less 5 sek.
            var designerObjList1 = DesignerRepoTwo.DesignerObjList;

            if (designerObjList1.Count == 0) { return; }

            var zoneId = InfraRepo.GetInfraData().InfraChangeableData.ZoneDict.FirstOrDefault().ZoneId;
            DesignerViewModel = new EditedViewModel(zoneId);
        }


        private string GetSqliteFile()
        {
            return System.Configuration.ConfigurationManager.AppSettings["SqliteFile"]; ;
        }
        private string GetDatabaseName(string name = "WaterInfra_5_ConnStr")
        {
            var connString = GetConnectionString("WaterInfra_5_ConnStr");
            var databaseName = connString.Split(';')[1].Split('=')[1];

            return databaseName;
        }
        private string GetConnectionString(string name = "WaterInfra_5_ConnStr")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

    }
}
