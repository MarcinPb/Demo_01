using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Shapes;
using WpfApplication1.Repo;
using WpfApplication1.ShapeModel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.Designer
{
    public class DesignerViewModel : ViewModelBase
    {
        public DateTime StartDate { get; set; }
        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }

        public ObservableCollection<Shp> ObjList { get; set; }

        private int _selectedItem;
        public int SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; RaisePropertyChanged(nameof(SelectedItem)); }
        }

        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new RelayCommand(OnAddExecute, () => true)); }
        }
        private void OnAddExecute()
        {
            ObjList.Add(new EllipseShp() { Id = 9, X = 210, Y = 30, Width = 10, Height = 10, TypeId = 2 });
        }

        private ICommand _moveCommand;
        public ICommand MoveCommand
        {
            get { return _moveCommand ?? (_moveCommand = new RelayCommand(OnMoveExecute, () => true)); }
        }
        private void OnMoveExecute()
        {
            var obj = ObjList.LastOrDefault();
            if (obj != null) obj.X += 20;
        }

        public RelayCommand<object> OnMouseDoubleClickCmd { get; }
        private void OnMouseDoubleClickCmdExecute(object obj)
        {
            int id = 0;
            MouseEventArgs e = (MouseEventArgs)obj;
            var position = e.GetPosition(e.Device.Target);
            if (e.Device.Target is Line)
            {
                var line = (Line)e.Device.Target;
                id = Convert.ToInt32(((Line)e.Device.Target).Tag);
            }
            if (e.Device.Target is Ellipse)
            {
                id = Convert.ToInt32(((Ellipse)e.Device.Target).Tag);
            }
            if (e.Device.Target is Rectangle)
            {
                id = Convert.ToInt32(((Rectangle)e.Device.Target).Tag);

            }
            SelectedItem = id;
            var shp = ObjList.FirstOrDefault(x => x.Id == id);
            Messenger.Default.Send(shp);
        }

        public DesignerViewModel()
        {
            StartDate = Convert.ToDateTime("2021-03-09 11:30");

            OnMouseDoubleClickCmd = new RelayCommand<object>(OnMouseDoubleClickCmdExecute);

            double svgWidth = 800;
            double svgHeight = 800;
            double margin = 20;

            CanvasWidth = svgWidth + 2 * margin;
            CanvasHeight = svgHeight + 2 * margin;


            var list = new DesignerRepo().GetShpList(svgWidth, svgHeight, margin);
            ObjList = new ObservableCollection<Shp>(list);
        }

    }
}
