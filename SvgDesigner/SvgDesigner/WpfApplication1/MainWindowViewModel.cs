using GeometryModel;
using GeometryReader;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfApplication1.ShapeModel;
using WpfApplication1.Ui.Designer;
using WpfApplication1.Utility;

namespace WpfApplication1
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly string _sqliteFile = @"K:\temp\sandbox\Nowy model testowy\testOPC.wtg.sqlite";

        public DesignerViewModel DesignerViewModel { get; set; }

        private Ui.Designer.EditedViewModel _propertyGridViewModel;
        public Ui.Designer.EditedViewModel PropertyGridViewModel
        {
            get { return _propertyGridViewModel; }
            set { _propertyGridViewModel = value; RaisePropertyChanged(nameof(PropertyGridViewModel)); }
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



        public RelayCommand<object> ImportDataCmd { get; }

        public MainWindowViewModel()
        {
            ImportDataCmd = new RelayCommand<object>(ImportDataCmdExecute);

            DesignerViewModel = new DesignerViewModel();
            PropertyGridViewModel = new Ui.Designer.EditedViewModel();

            Messenger.Default.Register<Shp>(this, OnShpReceived);
        }

        private async void ImportDataCmdExecute(object obj)
        {
            var importer = new Importer();
            importer.ProgressChanged += OnProgressChanged;
            importer.InnerProgressChanged += OnInnerProgressChanged;
            await Task.Run(() => importer.ImportData(_sqliteFile));
            //importer.ProgressChanged -= OnProgressChanged;
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


        private void OnShpReceived(Shp shp)
        {
            if (shp is LinkMy)
            {
                PropertyGridViewModel = new Ui.Designer.Pipe.EditedViewModel(shp.Id);
            }
            else if (shp is ObjMy)
            {
                PropertyGridViewModel = new Ui.Designer.Junction.EditedViewModel(shp.Id);
            }
            else if (shp is CnShp)
            {
                PropertyGridViewModel = new Ui.Designer.CustomerNode.EditedViewModel(shp.Id);
            }
        }
    }
}
