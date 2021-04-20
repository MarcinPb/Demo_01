using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.PropertyGrid.Junction
{
    public class EditedViewModel : Ui.PropertyGrid.EditedViewModel
    {
        private ItemViewModel _model;
        public ItemViewModel ItemViewModel
        {
            get => _model;
            set { _model = value; RaisePropertyChanged(nameof(ItemViewModel)); }
        }

        public EditedViewModel(int id)
        {
            try
            {
                ItemViewModel = new ItemViewModel(id);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
