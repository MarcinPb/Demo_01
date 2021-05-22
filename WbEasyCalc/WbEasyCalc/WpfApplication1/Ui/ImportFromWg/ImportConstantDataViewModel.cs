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
    public class ImportConstantDataViewModel : ViewModelBase, IDialogViewModel
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region IDialogViewModel
        public string Title { get; set; } = "Import Constant Data";

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

        public ImportConstantDataViewModel()
        {
            Message = "Start ...";
            _logger.Info("Import Constant Data 1");


            var sqliteFile = GetSqliteFile();
            var importer = new Importer();
            _logger.Info("Import Constant Data 2");
            var importedBaseOutputLists = importer.ImportBase(sqliteFile);
            _logger.Info("Import Constant Data 3");
            InfraRepo.InsertToInfraObjType(importedBaseOutputLists.InfraObjTypeList);
            _logger.Info("Import Constant Data 4");
            InfraRepo.InsertToInfraField(importedBaseOutputLists.ImportedFieldList);
            _logger.Info("Import Constant Data 5");

            Message = "Constant data were imported successfullly.";
        }

        private string GetSqliteFile()
        {
            return System.Configuration.ConfigurationManager.AppSettings["SqliteFile"]; ;
        }

    }
}
