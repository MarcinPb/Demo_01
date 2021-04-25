using Database.DataModel;
using Database.DataRepository;
using GeometryReader;
using System;
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
        public EditedViewModel DesignerViewModel { get; set; }

        //public DesignerViewModel DesignerViewModel { get; set; }

        //private Ui.PropertyGrid.EditedViewModel _propertyGridViewModel;
        //public Ui.PropertyGrid.EditedViewModel PropertyGridViewModel
        //{
        //    get { return _propertyGridViewModel; }
        //    set { _propertyGridViewModel = value; RaisePropertyChanged(nameof(PropertyGridViewModel)); }
        //}



        #region ImportChangeableData

        private readonly string _sqliteFile = @"K:\temp\sandbox\Nowy model testowy\testOPC.wtg.sqlite";

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

        public RelayCommand<object> ImportDataCmd { get; }

        private async void ImportDataCmdExecute(object obj)
        {
            InfraConstantDataLists infraConstantDataLists = InfraRepo.GetInfraConstantData();

            var importer = new Importer();
            importer.ProgressChanged += OnProgressChanged;
            importer.InnerProgressChanged += OnInnerProgressChanged;
            InfraChangeableDataLists importedDataOutputLists = await Task<int>.Run(() => importer.ImportData(_sqliteFile, infraConstantDataLists));
            //importer.InnerProgressChanged -= OnInnerProgressChanged;
            //importer.ProgressChanged -= OnProgressChanged;

            InfraRepo.InsertToInfraZone(importedDataOutputLists.ZoneDict);
            InfraRepo.InsertToInfraObj(importedDataOutputLists.InfraObjList);
            InfraRepo.InsertToInfraValue(importedDataOutputLists.InfraValueList);
            InfraRepo.InsertToInfraGeometry(importedDataOutputLists.InfraGeometryList);
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


        #region Open Designer

        public RelayCommand OpenDesignerCmd { get; }
        private void OpenRowCmdExecute()
        {
            var editedViewModel = new EditedViewModel(6773);
            var result = DialogUtility.ShowModal(editedViewModel);
            //editedViewModel.Dispose();
        }
        public bool OpenRowCmdCanExecute()
        {
            return true;
        }

        public RelayCommand OpenDesignerNextCmd { get; }
        private void OpenRowNextCmdExecute()
        {
            var editedViewModel = new EditedViewModel(6774);
            var result = DialogUtility.ShowModal(editedViewModel);
            //editedViewModel.Dispose();
        }
        public bool OpenRowNextCmdCanExecute()
        {
            return true;
        }

        #endregion


        public MainWindowViewModel()
        {
            ImportDataCmd = new RelayCommand<object>(ImportDataCmdExecute);
            OpenDesignerCmd = new RelayCommand(OpenRowCmdExecute, OpenRowCmdCanExecute);
            OpenDesignerNextCmd = new RelayCommand(OpenRowNextCmdExecute, OpenRowNextCmdCanExecute);

            // Singleton run before opening designer first time. It takes more or less 5 sek.
            var designerObjList1 = DesignerRepoTwo.DesignerObjList;

            DesignerViewModel = new EditedViewModel(6773);


            //DesignerViewModel = new DesignerViewModel();
            //PropertyGridViewModel = new Ui.PropertyGrid.EditedViewModel();

            //Messenger.Default.Register<Shp>(this, OnShpReceived);
        }









        //private void OnShpReceived(Shp shp)
        //{
        //    if (shp is PathShp)
        //    {
        //        PropertyGridViewModel = new Ui.PropertyGrid.Pipe.EditedViewModel(shp.Id);
        //    }
        //    else if (shp is EllipseShp)
        //    {
        //        PropertyGridViewModel = new Ui.PropertyGrid.Junction.EditedViewModel(shp.Id);
        //    }
        //    else if (shp is RectangleShp)
        //    {
        //        PropertyGridViewModel = new Ui.PropertyGrid.CustomerNode.EditedViewModel(shp.Id);
        //    }
        //}
    }
}
