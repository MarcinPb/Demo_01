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
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.DesignerWithPropreryGrid
{
    public class EditedViewModel : ViewModelBase, IDialogViewModel
    {
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


        public PushPinShp PushPin { get; set; }

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
            if (PushPin == null)
            {
                MessageBox.Show("You can not save water consumption without its location.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            WaterConsumption model = Model.Model;
            model.Lontitude = PushPin.X;
            model.Latitude = PushPin.Y;
            model.RelatedId = PushPin.RelatedId;

            model = GlobalConfig.DataRepository.WaterConsumptionListRepositoryTemp.SaveItem(model);
            Messenger.Default.Send<WaterConsumption>(model);
            return true;
        }

        public void Close()
        {
        }

        #endregion

        public EditedViewModel(int id)
        {
            // Consumption Model --------------------------
            Model = new ItemViewModel(GlobalConfig.DataRepository.WaterConsumptionListRepositoryTemp.GetItem(id));
            Model.PropertyChanged += Model_PropertyChanged;

            WaterConsumptionCategoryList = GlobalConfig.DataRepository.WaterConsumptionCategoryList;
            WaterConsumptionStatusList = GetWaterConsumptionStatusList();

            // Designer -----------------------------------
            if (id != 0)
            {
                PushPin = new PushPinShp { X = Model.Lontitude, Y = Model.Latitude, ZoneId = 1, TypeId = 4, Id=100000, RelatedId = Model.RelatedId };
            }
            DesignerViewModel = new DesignerViewModel(6773, PushPin);
            DesignerViewModel.PropertyChanged += DesignerViewModel_PropertyChanged;

            // PropertyGrid -----------------------------------
            PropertyGridViewModel = new Ui.PropertyGrid.EditedViewModel();
            if (id != 0)
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
                OnShpReceived(((DesignerViewModel)sender).PushPin);
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
            else if (shp is PushPinShp)
            {
                PushPin = (PushPinShp)shp;
            }
        }
    }
}
