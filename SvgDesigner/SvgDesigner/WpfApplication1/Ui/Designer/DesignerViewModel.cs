using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Shapes;
using WpfApplication1.Ui.Designer.Model;
using WpfApplication1.Ui.Designer.Model.ShapeModel;
using WpfApplication1.Ui.Designer.Repo;
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
            ObjList.Add(new PushPinShp() { Id = 100000, X = 210, Y = 30, TypeId = 2 });
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

        public RelayCommand<object> MouseLeftButtonDownCmd { get; }
        private int id;
        private void OnMouseDoubleClickCmdExecute(object obj)
        {
            MouseButtonEventArgs e = (MouseButtonEventArgs)obj;
            if (e.ClickCount == 1)
            {
                id = 0;
                if (e.Device.Target is Line)
                {
                    id = Convert.ToInt32(((Line)e.Device.Target).Tag);
                }
                else if (e.Device.Target is Path)
                {
                    id = Convert.ToInt32(((Path)e.Device.Target).Tag);
                }
                else if (e.Device.Target is Ellipse)
                {
                    id = Convert.ToInt32(((Ellipse)e.Device.Target).Tag);
                }
                else if (e.Device.Target is Rectangle)
                {
                    id = Convert.ToInt32(((Rectangle)e.Device.Target).Tag);

                }
                SelectedItem = id;
                var shp = ObjList.FirstOrDefault(x => x.Id == id);
                Messenger.Default.Send(shp);
            }
            else if (e.ClickCount == 2)
            {
                var position = e.GetPosition(e.Device.Target);

                var objToRemove = ObjList.FirstOrDefault(x => x.Id == 100000);
                if (objToRemove != null)
                {
                    ObjList.Remove(objToRemove);
                }

                var objPosition = ObjList.FirstOrDefault(x => x.Id == id);
                ObjList.Add(new PushPinShp() { Id = 100000, X = objPosition.X + position.X, Y = objPosition.Y + position.Y, TypeId = 2 });
            }
        }

        public DesignerViewModel(int? zoneId = null)
        {
            StartDate = Convert.ToDateTime("2021-03-09 11:30");

            MouseLeftButtonDownCmd = new RelayCommand<object>(OnMouseDoubleClickCmdExecute);

            double svgWidth = 800;
            double svgHeight = 800;
            double margin = 20;

            CanvasWidth = svgWidth + 2 * margin;
            CanvasHeight = svgHeight + 2 * margin;

            var list = new DesignerRepo(zoneId).GetShpList(svgWidth, svgHeight, margin);
            ObjList = new ObservableCollection<Shp>(list);
        }

    }
}
