using System;
using System.ComponentModel;
using System.Windows;

namespace WpfApplication1.Utility
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        private static int _level;
        private IDialogViewModel _viewModel;

        public DialogWindow()
        {
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null && !_viewModel.Save()) return;
            this.DialogResult = true;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _level++;
            Window mainWindow = Application.Current.MainWindow;
            this.Left = mainWindow.Left + _level * 100;
            this.Top = mainWindow.Top + _level * 50;

            _viewModel = this.DataContext as IDialogViewModel;
            this.Title = _viewModel.Title;
        }

        private void DialogWindow_OnClosed(object sender, EventArgs e)
        {
            _level--;
            _viewModel.Close();
        }
    }
}
