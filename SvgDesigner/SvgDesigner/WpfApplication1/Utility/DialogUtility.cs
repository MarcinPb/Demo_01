namespace WpfApplication1.Utility
{
    public class DialogUtility
    {
        public static bool? ShowModal(object viewModel)
        {
            DialogWindow dialog = new DialogWindow {DataContext = viewModel};
            return dialog.ShowDialog();
        }
    }
}
