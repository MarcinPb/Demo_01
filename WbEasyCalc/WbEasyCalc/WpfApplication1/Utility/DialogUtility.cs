using System.Windows;

namespace WpfApplication1.Utility
{
    public class DialogUtility
    {
        public static bool? ShowModal(object viewModel)
        {
            DialogWindow dialog = new DialogWindow {DataContext = viewModel};
            dialog.Owner = Application.Current.MainWindow;
            return dialog.ShowDialog();
        }
    }
}
