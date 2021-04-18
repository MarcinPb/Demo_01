using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using WpfApplication1.ShapeModel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.Designer
{
    public class EditedViewModel : ViewModelBase
    {
        public EditedViewModel()
        {
            //Messenger.Default.Register<Shp>(this, OnShpReceived);
        }
        //public virtual void OnShpReceived(Shp shp)
        //{
        //    var i = shp.Id;
        //}
    }
}
