using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
        private int _objId;

        public DateTime StartDate { get; set; }
        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }

        public ObservableCollection<Shp> ObjList { get; set; }

        private int _selectedItem;
        public int SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; RaisePropertyChanged(); }
        }

        private Shp _pushPin;
        public Shp PushPin
        {
            get => _pushPin;
            set { _pushPin = value; RaisePropertyChanged(); }
        }

        public RelayCommand<object> MouseLeftButtonDownCmd { get; }
        private void OnMouseDoubleClickCmdExecute(object obj)
        {
            MouseButtonEventArgs e = (MouseButtonEventArgs)obj;
            if (e.ClickCount == 1)
            {
                _objId = 0;
                if (e.Device.Target is Path)
                {
                    _objId = Convert.ToInt32(((Path)e.Device.Target).Tag);
                }
                else if (e.Device.Target is Ellipse)
                {
                    _objId = Convert.ToInt32(((Ellipse)e.Device.Target).Tag);
                }
                else if (e.Device.Target is Rectangle)
                {
                    _objId = Convert.ToInt32(((Rectangle)e.Device.Target).Tag);
                }
                else 
                {
                    return;
                }
                SelectedItem = _objId;
            }
            else if (e.ClickCount == 2)
            {
                // Remove an old PushPin form the ObjList collection.
                var objToRemove = ObjList.FirstOrDefault(x => x.Id == 100000);
                if (objToRemove != null) {
                    ObjList.Remove(objToRemove);
                }

                // Add a new PushPin to the ObjList collection.
                var objPosition = ObjList.FirstOrDefault(x => x.Id == _objId);
                var mousePosition = e.GetPosition(e.Device.Target);
                PushPin = new PushPinShp() { Id = 100000, X = objPosition.X + mousePosition.X, Y = objPosition.Y + mousePosition.Y, TypeId = 2, RelatedId = SelectedItem };
                ObjList.Add(PushPin);
            }
        }

        public DesignerViewModel(int? zoneId = null, Shp locationPoint = null)
        {
            StartDate = Convert.ToDateTime("2021-03-09 11:30");

            MouseLeftButtonDownCmd = new RelayCommand<object>(OnMouseDoubleClickCmdExecute);

            double svgWidth = 800;
            double svgHeight = 600;
            double margin = 20;

            CanvasWidth = svgWidth + 2 * margin;
            CanvasHeight = svgHeight + 2 * margin;

            var designerObjList = DesignerRepoTwo.DesignerObjList.Where(f => f.ZoneId == zoneId).Select(x => (DesignerObj)x.Clone()).ToList();

            var list = ShpRepo.GetShpList(svgWidth, svgHeight, margin, designerObjList) ;

            ObjList = new ObservableCollection<Shp>(list);

            if (locationPoint != null)
            {
                SelectedItem = locationPoint.Id;

                PushPin = locationPoint;
                ObjList.Add(PushPin);
            }
        }
    }
}
