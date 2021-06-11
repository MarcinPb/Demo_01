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

namespace WpfApplication2.Ui.DesignerWithPropreryGrid
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
    public class EditedViewModel : ViewModelBase    //, IDialogViewModel
    {
        private List<DesignerObj> _designerObjList;

        #region Upper panel

        //private ItemViewModel _model;
        //public ItemViewModel Model
        //{
        //    get => _model;
        //    set { _model = value; RaisePropertyChanged(); }
        //}


        #endregion


        public DesignerObj PushPin { get; set; }

        public DesignerViewModel DesignerViewModel { get; set; }

        private WpfApplication1.Ui.PropertyGrid.EditedViewModel _propertyGridViewModel;
        public WpfApplication1.Ui.PropertyGrid.EditedViewModel PropertyGridViewModel
        {
            get { return _propertyGridViewModel; }
            set { _propertyGridViewModel = value; RaisePropertyChanged(nameof(PropertyGridViewModel)); }
        }

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
        public EditedViewModel()
        {
            // Designer ------------------------------------------
            _designerObjList = DesignerRepo.DesignerObjList/*.Where(f => f.ZoneId == zoneId)*/.Select(x => (DesignerObj)x.Clone()).ToList();
            //_designerObjList = new List<DesignerObj>();
            DesignerViewModel = new DesignerViewModel(_designerObjList);
            DesignerViewModel.PropertyChanged += DesignerViewModel_PropertyChanged;

            // PropertyGrid --------------------------------------
            PropertyGridViewModel = new WpfApplication1.Ui.PropertyGrid.EditedViewModel();
        }

        private void DesignerViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem" )
            {
                var designerViewModel = (DesignerViewModel)sender;
                var objList = designerViewModel.ObjList;
                var id = designerViewModel.SelectedItem;
                var shp = objList.FirstOrDefault(x => x.Id == id);

                if (shp is PathShp)
                {
                    PropertyGridViewModel = new WpfApplication1.Ui.PropertyGrid.Pipe.EditedViewModel(shp.Id);
                }
                else if (shp is EllipseShp)
                {
                    PropertyGridViewModel = new WpfApplication1.Ui.PropertyGrid.Junction.EditedViewModel(shp.Id);
                }
                else if (shp is RectangleShp)
                {
                    PropertyGridViewModel = new WpfApplication1.Ui.PropertyGrid.CustomerNode.EditedViewModel(shp.Id);
                }
            }
            else if (e.PropertyName == "PushPin")
            {
                PushPin = ((DesignerViewModel)sender).PushPin;
            }
        }
    }
}
