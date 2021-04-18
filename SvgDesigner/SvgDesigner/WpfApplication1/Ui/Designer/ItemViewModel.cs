using Database.DataModel;
using Database.DataRepository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Utility;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace WpfApplication1.Ui.Designer
{
    public class ItemViewModel : ViewModelBase
    {

        private int _id;
        [Category("<General>")]
        [DisplayName("ID")]
        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        private string _name;
        [Category("<General>")]
        [DisplayName("Label")]
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged("Name"); }
        }

        [Category("Active Topology")]
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [Category("Physical")]
        [DisplayName("Zone")]
        [Description("This property uses the DoubleUpDown as the default editor.")] 
        //[ItemsSource(typeof(FontSizeItemsSource))]
        public string Zone { get; set; }

        protected DesignerObj _model;
        public ItemViewModel(int id)
        {
            //_model = new DesignerInfraRepo().GetItem(id);

            //Id = _model.ID;
            //Name = _model.Label;

            //IsActive = _model.IsActive;
            //Zone = _model.Zone;
        }


        [Category("Physical")]
        [DisplayName("FontSizeItemsSource")]
        [Browsable(false)]
        public ObservableCollection<string> FontSizeItemsSource
        {
            get
            {
                ObservableCollection<string> sizes = new ObservableCollection<string>();

                // Items generation could be made here
                sizes.Add("1 - Przybków");
                sizes.Add("2 - Stare Miasto");
                sizes.Add("3 - Kopernik");
                sizes.Add("4 - Piekary");
                sizes.Add("5 - Północ");
                sizes.Add("6 - ZPW");
                return sizes;
            }

        }
    }

    //public class FontSizeItemsSource : IItemsSource
    //{
    //    public ItemCollection GetValues()
    //    {
    //        ItemCollection sizes = new ItemCollection();
    //        sizes.Add(1, "1 - Przybków");
    //        sizes.Add(2, "2 - Stare Miasto");
    //        sizes.Add(3, "3 - Piekary");
    //        sizes.Add(4, "4 - Kopernik");
    //        sizes.Add(5, "3 - Północ");
    //        sizes.Add(6, "4 - ZPW");
    //        return sizes;
    //    }
    //}

}
