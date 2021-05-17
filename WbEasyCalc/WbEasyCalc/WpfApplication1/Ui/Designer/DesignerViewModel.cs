using System;
using System.Collections.Generic;
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
        private List<DesignerObj> _designerObjList;
        private ShpRepo _shapeRepo;

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

        private DesignerObj _pushPin;
        public DesignerObj PushPin
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

                    var designerObjToRemove = _designerObjList.FirstOrDefault(x => x.ObjId == 100000);
                    _designerObjList.Remove(designerObjToRemove);
                }


                // Add a new PushPin to the ObjList collection.
                var objPosition = ObjList.FirstOrDefault(x => x.Id == _objId);
                var mousePosition = e.GetPosition(e.Device.Target);
                var pushPinShp = new PushPinShp() 
                { 
                    Id = 100000, 
                    X = objPosition.X + mousePosition.X, 
                    Y = objPosition.Y + mousePosition.Y, 
                    TypeId = 2, 
                    RelatedId = SelectedItem 
                };
                ObjList.Add(pushPinShp);
                //
                Point designerPushPinPoint = _shapeRepo.ShpPointToDesignerPoint(new Point(pushPinShp.X, pushPinShp.Y));
                var designerObj = new DesignerObj()
                {
                    ObjId = 100000,
                    ObjTypeId = 1000,
                    //ZoneId = _zoneId,
                    AssociatedId = SelectedItem,
                    Xp = designerPushPinPoint.X,
                    Yp = designerPushPinPoint.Y,
                    Geometry = new List<Point> { designerPushPinPoint },
                };
                _designerObjList.Add(designerObj);

                PushPin = designerObj;
            }
        }

        public DesignerViewModel(List<DesignerObj> designerObjList)
        {
            _designerObjList = designerObjList;

            StartDate = Convert.ToDateTime("2021-03-09 11:30");

            MouseLeftButtonDownCmd = new RelayCommand<object>(OnMouseDoubleClickCmdExecute);

            double svgWidth = 800;
            double svgHeight = 600;
            double margin = 20;

            CanvasWidth = svgWidth + 2 * margin;
            CanvasHeight = svgHeight + 2 * margin;

            _shapeRepo = new ShpRepo(svgWidth, svgHeight, margin, _designerObjList);
            List<Shp> list = _shapeRepo.GetShpList();
            ObjList = new ObservableCollection<Shp>(list);
        }

    }
}
