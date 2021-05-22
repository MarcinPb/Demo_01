using Database.DataModel.Infra;
using Database.DataRepository.Infra;
using GeometryReader;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.ImportFromWg
{
    public class ImportChangeableDataViewModel : ViewModelBase, IDialogViewModel
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region IDialogViewModel
        public string Title { get; set; } = "Import Changeable Data";

        public bool Save()
        {
            return true;
        }

        public void Close()
        {
        }

        #endregion

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



        public ImportChangeableDataViewModel()
        {
            ImportChangeableData();
        }

        public async void ImportChangeableData()
        {
            try
            {
                _logger.Info("Import Changeable Data 1");

                DatabaseName = GetDatabaseName("WaterInfra_5_ConnStr");
                SqliteFile = GetSqliteFile();

                InfraConstantDataLists infraConstantDataLists = InfraRepo.GetInfraConstantData();

                var importer = new Importer();
                importer.OuterProgressChanged += OnOuterProgressChanged;
                importer.InnerProgressChanged += OnInnerProgressChanged;
                InfraChangeableDataLists importedDataOutputLists = await Task<int>.Run(() => importer.ImportData(SqliteFile, infraConstantDataLists));
                importer.InnerProgressChanged -= OnInnerProgressChanged;
                importer.OuterProgressChanged -= OnOuterProgressChanged;

                InfraRepo.InsertToInfraZone(importedDataOutputLists.ZoneDict);                      //  14601
                InfraRepo.InsertToInfraDemandPattern(importedDataOutputLists.DemandPatternDict);    //  
                InfraRepo.InsertToInfraObj(importedDataOutputLists.InfraObjList);                   //     16
                _logger.Info($"InsertToInfraValue = {importedDataOutputLists.InfraValueList.Count}.");
                InfraRepo.InsertToInfraValue(importedDataOutputLists.InfraValueList);               // 518964
                InfraRepo.InsertToInfraGeometry(importedDataOutputLists.InfraGeometryList);         //  25138

                ProgressMessage = "Data were saved to database successfully."; 
            }
            catch (Exception e)
            {
                _logger.Info(e.Message);
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnInnerProgressChanged(object sender, GeometryReader.ProgressEventArgs e)
        {
            InnerProgressPercent = e.ProgressRatio;
            InnerProgressMessage = e.Message;
        }

        private void OnOuterProgressChanged(object sender, GeometryReader.ProgressEventArgs e)
        {
            ProgressPercent = e.ProgressRatio;
            ProgressMessage = e.Message;
        }

        private string GetDatabaseName(string name = "WaterInfra_5_ConnStr")
        {
            var connString = GetConnectionString("WaterInfra_5_ConnStr");
            var databaseName = connString.Split(';')[1].Split('=')[1];

            return databaseName;
        }
        private string GetConnectionString(string name = "WaterInfra_5_ConnStr")
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        private string GetSqliteFile()
        {
            return System.Configuration.ConfigurationManager.AppSettings["SqliteFile"]; ;
        }

    }
}
