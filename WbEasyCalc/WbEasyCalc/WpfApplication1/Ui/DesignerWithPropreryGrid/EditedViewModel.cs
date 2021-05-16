using Database.DataModel;
using Database.DataModel.Infra;
using Database.DataRepository.Infra;
using GlobalRepository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfApplication1.Ui.Designer;
using WpfApplication1.Ui.Designer.Model;
using WpfApplication1.Ui.Designer.Model.ShapeModel;
using WpfApplication1.Ui.Designer.Repo;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.DesignerWithPropreryGrid
{
    /// <summary>
    /// ViewModel for a WaterConsumption item taken from a WaterConsumption list or a newly created. 
    /// ViewModel consists of three panels:
    ///     Upper base panel with properties of the item.
    ///     Left designer panel with a designer with objects of a water works schema for a particular zone.
    ///     Right PropertyGrid panel with choosen object propetries.  
    /// ViewModel is opened in a DialogWindow (implements IDialogViewModel), thus it consist Save and Cancel methods.
    /// Save method saves:
    ///     Properties from upper base panel.
    ///     Properties of PushPin: Lontitude, Latitude and RelatedId
    /// </summary>
    public class EditedViewModel : ViewModelBase, IDialogViewModel
    {
        private int _zoneId;
        private List<DesignerObj> _designerObjList;

        #region Upper panel

        private ItemViewModel _model;
        public ItemViewModel Model
        {
            get => _model;
            set { _model = value; RaisePropertyChanged(); }
        }

        public List<IdNamePair> WaterConsumptionCategoryList { get; set; }

        private ObservableCollection<IdNamePair> _waterConsumptionStatusList;
        public ObservableCollection<IdNamePair> WaterConsumptionStatusList
        {
            get => _waterConsumptionStatusList;
            set { _waterConsumptionStatusList = value; RaisePropertyChanged(); }
        }

        #endregion


        public DesignerObj PushPin { get; set; }

        public DesignerViewModel DesignerViewModel { get; set; }

        private Ui.PropertyGrid.EditedViewModel _propertyGridViewModel;
        public Ui.PropertyGrid.EditedViewModel PropertyGridViewModel
        {
            get { return _propertyGridViewModel; }
            set { _propertyGridViewModel = value; RaisePropertyChanged(nameof(PropertyGridViewModel)); }
        }

        #region IDialogViewModel

        public string Title { get; set; } = "Designer";

        public bool Save()
        {
            var pushPin = _designerObjList.FirstOrDefault(x => x.ObjId == 100000);
            if (PushPin == null)
            {
                MessageBox.Show("You can not save water consumption without its location.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            WaterConsumption model = Model.Model;
            model.Lontitude = PushPin.Xp;
            model.Latitude = PushPin.Yp;
            model.RelatedId = (int)PushPin.AssociatedId;

            model = GlobalConfig.DataRepository.WaterConsumptionListRepositoryTemp.SaveItem(model);
            Messenger.Default.Send<WaterConsumption>(model);
            return true;
        }

        public void Close()
        {
        }

        #endregion

        /// <summary>
        /// Ctor, creates three ViewModels and two related lookout lists.
        /// ViewModels:
        ///     Model - ItemViewModel for a WaterConsumption taken from the list or newly created 
        ///     DesignerViewModel - 
        ///     PropertyGridViewModel -
        /// Lists:
        ///     WaterConsumptionCategoryList
        ///     WaterConsumptionStatusList
        /// </summary>
        /// <param name="waterConsumptionId">Id of an item from a WaterConsumption list. Case waterConsumptionId==0 means that a new item is created.</param>
        /// <param name="zoneId">Zone Id</param>
        public EditedViewModel(int waterConsumptionId, int zoneId)
        {
            _zoneId = zoneId;

            // Upper base panel - Model --------------------------
            Model = new ItemViewModel(GlobalConfig.DataRepository.WaterConsumptionListRepositoryTemp.GetItem(waterConsumptionId));
            Model.PropertyChanged += Model_PropertyChanged;

            WaterConsumptionCategoryList = GlobalConfig.DataRepository.WaterConsumptionCategoryList;
            WaterConsumptionStatusList = GetWaterConsumptionStatusList();

            // Designer ------------------------------------------
            _designerObjList = DesignerRepoTwo.DesignerObjList.Where(f => f.ZoneId == zoneId).Select(x => (DesignerObj)x.Clone()).ToList();
            if (waterConsumptionId != 0)
            {
                PushPin = new DesignerObj 
                { 
                    ObjId = 100000, 
                    ObjTypeId = 1000, 
                    ZoneId = _zoneId, 
                    AssociatedId = Model.RelatedId,
                    Xp = Model.Lontitude, 
                    Yp = Model.Latitude,
                    Geometry = new List<Point> { new Point(Model.Lontitude, Model.Latitude) },
                };
                _designerObjList.Add(PushPin);
            }
            DesignerViewModel = new DesignerViewModel(_designerObjList);
            DesignerViewModel.PropertyChanged += DesignerViewModel_PropertyChanged;

            // PropertyGrid --------------------------------------
            PropertyGridViewModel = new Ui.PropertyGrid.EditedViewModel();
            if (waterConsumptionId != 0)
            {
                DesignerViewModel.SelectedItem = Model.RelatedId;
            }
        }


        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WaterConsumptionCategoryId")
            {
                OnCategoryChange(null);
            }
        }

        private void OnCategoryChange(ItemViewModel obj)
        {
            WaterConsumptionStatusList = GetWaterConsumptionStatusList();
            Model.WaterConsumptionStatusId = WaterConsumptionStatusList.FirstOrDefault().Id;
        }

        private ObservableCollection<IdNamePair> GetWaterConsumptionStatusList()
        {
            //return GlobalConfig.DataRepository.WaterConsumptionStatusList;
            var categoryId = Model.Model.WaterConsumptionCategoryId;
            var statusIdList = GlobalConfig.DataRepository.WaterConsumptionCategoryStatusExcelList.Where(x => x.CategoryId == categoryId);
            var statusList = GlobalConfig.DataRepository.WaterConsumptionStatusList.Where(x => statusIdList.Any(y => x.Id == y.StatusId));
            return new ObservableCollection<IdNamePair>(statusList);
        }

        private void DesignerViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem" )
            {
                var designerViewModel = (DesignerViewModel)sender;
                var objList = designerViewModel.ObjList;
                var id = designerViewModel.SelectedItem;
                var shp = objList.FirstOrDefault(x => x.Id == id);

                OnShpReceived(shp);
            }
            else if (e.PropertyName == "PushPin")
            {
                //OnShpReceived(((DesignerViewModel)sender).PushPin);
                PushPin = ((DesignerViewModel)sender).PushPin;
            }
        }
        private void OnShpReceived(Shp shp)
        {
            if (shp is PathShp)
            {
                PropertyGridViewModel = new Ui.PropertyGrid.Pipe.EditedViewModel(shp.Id);
            }
            else if (shp is EllipseShp)
            {
                PropertyGridViewModel = new Ui.PropertyGrid.Junction.EditedViewModel(shp.Id);
            }
            else if (shp is RectangleShp)
            {
                PropertyGridViewModel = new Ui.PropertyGrid.CustomerNode.EditedViewModel(shp.Id);
            }
            //else if (shp is PushPinShp)
            //{
            //    PushPin = ShpToDesignerObj((PushPinShp)shp);
            //}
        }


        #region Waste

        //private DesignerObj ShpToDesignerObj(PushPinShp shp)
        //{
        //    var xx = (PushPin.X - _margin) / xFactor + pointTopLeft.X;
        //    var yy = pointBottomRight.Y - (PushPin.Y - margin) / xFactor;

        //    var point = new Point(shp.X, shp.Y);

        //    return new DesignerObj()
        //    {
        //        ObjId = 100000,
        //        ObjTypeId = 1000,
        //        ZoneId = _zoneId,
        //        AssociatedId = Model.RelatedId,
        //        Xp = point.X,
        //        Yp = point.Y,
        //        Geometry = new List<Point> { point },
        //    };
        //}


        //double svgWidth = 800;
        //double svgHeight = 600;
        //double margin = 20;

        //List<DesignerObj> designerObjList = DesignerRepoTwo.DesignerObjList.Where(f => f.ZoneId == 6773).Select(x => (DesignerObj)x.Clone()).ToList();

        //var pointTopLeft = GetPointTopLeft(designerObjList);
        //var pointBottomRight = GetPointBottomRight(designerObjList);
        //var xFactor = svgWidth / (pointBottomRight.X - pointTopLeft.X);
        //var yFactor = svgHeight / (pointBottomRight.Y - pointTopLeft.Y);

        //var xx = (PushPin.X - margin) / xFactor + pointTopLeft.X;
        //var yy = pointBottomRight.Y - (PushPin.Y - margin) / xFactor; 

        //private static Point GetPointTopLeft(IEnumerable<DesignerObj> junctionList)
        //{
        //    var xMin = junctionList.Min(x => x.Geometry[0].X);
        //    var yMin = junctionList.Min(x => x.Geometry[0].Y);
        //    return new Point(xMin, yMin);
        //}
        //private static Point GetPointBottomRight(IEnumerable<DesignerObj> junctionList)
        //{
        //    var xMax = junctionList.Max(x => x.Geometry[0].X);
        //    var yMax = junctionList.Max(x => x.Geometry[0].Y);
        //    return new Point(xMax, yMax);
        //}

        #endregion
    }
}
