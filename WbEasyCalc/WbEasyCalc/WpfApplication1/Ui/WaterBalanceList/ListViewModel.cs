using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Database.DataModel;
using GlobalRepository;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WaterBalanceList
{
    public class ListViewModel : ViewModelBase, IDisposable
    {
        #region Props: List, SelectedRow, RowsQty

        private ObservableCollection<RowViewModel> _list;
        public ObservableCollection<RowViewModel> List
        {
            get { return _list; }
            set
            {
                _list = value;
                RaisePropertyChanged();
            }
        }

        private RowViewModel _selectedRow;
        public RowViewModel SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                RaisePropertyChanged();

                OpenRowCmd.RaiseCanExecuteChanged();
                RemoveRowCmd.RaiseCanExecuteChanged();
                CloneCmd.RaiseCanExecuteChanged();
                CreateAllCmd.RaiseCanExecuteChanged();

                WbEasyCalcDataEditedViewModel = null;
                //OpenRowCmdExecute();
            }
        }

        private int _rowsQty;
        public int RowsQty
        {
            get { return _rowsQty; }
            set
            {
                _rowsQty = value;
                RaisePropertyChanged();
            }
        }



        private EditedViewModel _customerEditedViewModel;
        public EditedViewModel WbEasyCalcDataEditedViewModel
        {
            get => _customerEditedViewModel;
            set
            {
                _customerEditedViewModel = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands: AddRowCmd, OpenRowCmd, RemoveRowCmd, CloneCmd, CreateAllCmdExecute, ArchiwSelectedItemsCmd

        public RelayCommand AddRowCmd { get; }
        private void AddRowCmdExecute()
        {
            if (SelectedRow != null)
            {
                SelectedRow = null;
            }
            var editedViewModel = new EditedViewModel(0);
            var result = DialogUtility.ShowModal(editedViewModel);
            editedViewModel.Dispose();
        }
        public bool AddRowCmdCanExecute()
        {
            return true;
        }

        public RelayCommand OpenRowCmd { get; }
     
        private void OpenRowCmdExecute()
        {
            if (SelectedRow == null)
            {
                return;
            }

            var editedViewModel = new EditedViewModel(SelectedRow.Model.WbEasyCalcDataId);
            var result = DialogUtility.ShowModal(editedViewModel);
            editedViewModel.Dispose();
        }
        public bool OpenRowCmdCanExecute()
        {
            return SelectedRow != null;
        }

        public RelayCommand RemoveRowCmd { get; }
        private void RemoveRowCmdExecute()
        {
            try
            {
                if (SelectedRow == null) return;
                var res = MessageBox.Show(
                    "Are you sure to delete record?",
                    "Question",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );
                if (res == MessageBoxResult.Yes)
                {
                    GlobalConfig.DataRepository.WbEasyCalcDataListRepository.DeleteItem(SelectedRow.Model.WbEasyCalcDataId);
                    LoadData();
                    SelectedRow = null;

                    Messenger.Default.Send<ListViewModel>(this);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public bool RemoveRowCmdCanExecute()
        {
            return SelectedRow != null && SelectedRow.Model.IsArchive==false;
        }


        public RelayCommand CloneCmd { get; }

        private void CloneCmdExecute()
        {
            try
            {
                if (SelectedRow == null) return;
                int id = GlobalConfig.DataRepository.WbEasyCalcDataListRepository.Clone(SelectedRow.Model.WbEasyCalcDataId);
                LoadData();
                SelectedRow = List.FirstOrDefault(x => x.Model.WbEasyCalcDataId == id);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CloneCmdCanExecute()
        {
            return SelectedRow != null;
        }

        public RelayCommand CreateAllCmd { get; }

        private void CreateAllCmdExecute()
        {
            try
            {
                int recordQty = GlobalConfig.DataRepository.WbEasyCalcDataListRepository.CreateAll();
                if (recordQty==0)
                {
                    MessageBox.Show("No records was added.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    LoadData();
                    var maxId = List.Max(x => x.Model.WbEasyCalcDataId);
                    SelectedRow = List.FirstOrDefault(x => x.Model.WbEasyCalcDataId == maxId);
                    MessageBox.Show($"{recordQty} records was added.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CreateAllCmdCanExecute()
        {
            // Only if at least one row is archived.
            return List.Any(x => x.Model.IsArchive==true);
        }


        public RelayCommand<IList> ArchiwSelectedItemsCmd { get; }

        private void ReadSelectedItemsExecute(object selectedItems)
        {
            var idListString = ((IList<object>)selectedItems).Select(x => (RowViewModel)x).Select(y => y.Model.WbEasyCalcDataId.ToString()).Aggregate((p, n) => p + "," + n);
            MessageBox.Show($"Selected Id list: {idListString}.");
        }
        #endregion

        public ListViewModel()
        {
            try
            {
                AddRowCmd = new RelayCommand(AddRowCmdExecute, AddRowCmdCanExecute);
                OpenRowCmd = new RelayCommand(OpenRowCmdExecute, OpenRowCmdCanExecute);
                RemoveRowCmd = new RelayCommand(RemoveRowCmdExecute, RemoveRowCmdCanExecute);
                //SaveRowCmd = new RelayCommand(SaveRowCmdExecute, SaveRowCmdCanExecute);
                CloneCmd = new RelayCommand(CloneCmdExecute, CloneCmdCanExecute);
                CreateAllCmd = new RelayCommand(CreateAllCmdExecute, CreateAllCmdCanExecute);

                ArchiwSelectedItemsCmd = new RelayCommand<IList>(ReadSelectedItemsExecute);

                Messenger.Default.Register<WbEasyCalcData>(this, OnSaveModel);
                LoadData();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Dispose()
        {
            Messenger.Default.Unregister(this);
        }

        private void OnSaveModel(WbEasyCalcData model)
        {
            LoadData();
            SelectedRow = List.FirstOrDefault(x => x.Model.WbEasyCalcDataId == model.WbEasyCalcDataId);

            Messenger.Default.Send<ListViewModel>(this);
        }

        private void LoadData()
        {
            List = new ObservableCollection<RowViewModel>(GlobalConfig.DataRepository.WbEasyCalcDataListRepository.GetList().Select(x => new RowViewModel(x)).OrderByDescending(x => x.Model.WbEasyCalcDataId).ToList());
            RowsQty = List.Count;
        }

    }
}
