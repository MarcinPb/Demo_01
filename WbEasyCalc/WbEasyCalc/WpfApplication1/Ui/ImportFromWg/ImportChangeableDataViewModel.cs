using Database.DataModel.Infra;
using Database.DataRepository.Infra;
using GeometryReader;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private string _message;
        public string Message
        {
            get => _message;
            set { _message = value; RaisePropertyChanged(); }
        }


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
            _logger.Info("Import Changeable Data 1");
            
            //Message = "Start ...";
            var sqliteFile = GetSqliteFile();

            InfraConstantDataLists infraConstantDataLists = InfraRepo.GetInfraConstantData();

            var importer = new Importer();
            importer.ProgressChanged += OnProgressChanged;
            importer.InnerProgressChanged += OnInnerProgressChanged;
            InfraChangeableDataLists importedDataOutputLists = await Task<int>.Run(() => importer.ImportData(sqliteFile, infraConstantDataLists));
            //importer.InnerProgressChanged -= OnInnerProgressChanged;
            //importer.ProgressChanged -= OnProgressChanged;

            InfraRepo.InsertToInfraZone(importedDataOutputLists.ZoneDict);                      //  14601
            InfraRepo.InsertToInfraDemandPattern(importedDataOutputLists.DemandPatternDict);    //  
            InfraRepo.InsertToInfraObj(importedDataOutputLists.InfraObjList);                   //     16
            InfraRepo.InsertToInfraValue(importedDataOutputLists.InfraValueList);               // 518964
            InfraRepo.InsertToInfraGeometry(importedDataOutputLists.InfraGeometryList);         //  25138

            //Message = "Constant data were imported successfullly.";
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


        private string GetSqliteFile()
        {
            return System.Configuration.ConfigurationManager.AppSettings["SqliteFile"]; ;
        }

    }
}
