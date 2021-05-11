using Database.DataModel.Infra;
using Database.DataRepository.Infra;
using GeometryReader;
using System;
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
        public Shp PushPin { get; set; }

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
            return true;
        }

        public void Close()
        {
        }

        #endregion


        public EditedViewModel(int? zoneId = null, Shp locationPoint = null)
        {
            DesignerViewModel = new DesignerViewModel(zoneId, locationPoint);
            PropertyGridViewModel = new Ui.PropertyGrid.EditedViewModel();

            Messenger.Default.Register<Shp>(this, OnShpReceived);
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
                PushPin = shp;
            }
        }
    }
}
