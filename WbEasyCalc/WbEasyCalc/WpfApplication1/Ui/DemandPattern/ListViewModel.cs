using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Database.DataModel;
using Database.DataRepository.Infra;
using GlobalRepository;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.DemandPattern
{
    public class ListViewModel : ViewModelBase, IDisposable, IDialogViewModel
    {
        #region IDialogViewModel
        public string Title { get; set; } = "Import Constant Data";

        public bool Save()
        {
            return true;
        }

        public void Close()
        {
        }

        #endregion




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



        //private EditedViewModel _customerEditedViewModel;
        //public EditedViewModel WbEasyCalcDataEditedViewModel
        //{
        //    get => _customerEditedViewModel;
        //    set
        //    {
        //        _customerEditedViewModel = value;
        //        RaisePropertyChanged();
        //    }
        //}

        #endregion

        #region Commands: AddRowCmd, OpenRowCmd, RemoveRowCmd, CloneCmd, CreateAllCmdExecute, ArchiwSelectedItemsCmd

        //public RelayCommand AddRowCmd { get; }
        //private void AddRowCmdExecute()
        //{
        //    if (SelectedRow != null)
        //    {
        //        SelectedRow = null;
        //    }
        //    //var editedViewModel = new EditedViewModel(0);
        //    var result = DialogUtility.ShowModal(editedViewModel);
        //    editedViewModel.Dispose();
        //}
        //public bool AddRowCmdCanExecute()
        //{
        //    return true;
        //}

        public RelayCommand OpenRowCmd { get; }
     
        private void OpenRowCmdExecute()
        {
            if (SelectedRow == null) { return; }

            var id = SelectedRow.Model.DemandPatternId;

            var editedViewModel = new EditedViewModel(SelectedRow);
            var result = DialogUtility.ShowModal(editedViewModel);
            if ((bool)result)
            {
                LoadData();
                SelectedRow = List.FirstOrDefault(x => x.Model.DemandPatternId == id);
            }
            editedViewModel.Dispose();
        }
        public bool OpenRowCmdCanExecute()
        {
            return SelectedRow != null;
        }

        //public RelayCommand RemoveRowCmd { get; }
        //private void RemoveRowCmdExecute()
        //{
        //    try
        //    {
        //        if (SelectedRow == null) return;
        //        var res = MessageBox.Show(
        //            "Are you sure to delete record?",
        //            "Question",
        //            MessageBoxButton.YesNo,
        //            MessageBoxImage.Question
        //        );
        //        if (res == MessageBoxResult.Yes)
        //        {
        //            GlobalConfig.DataRepository.WbEasyCalcDataListRepository.DeleteItem(SelectedRow.Model.DemandPatternId);
        //            LoadData();
        //            SelectedRow = null;

        //            Messenger.Default.Send<ListViewModel>(this);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        //public bool RemoveRowCmdCanExecute()
        //{
        //    return false;
        //}


        //public RelayCommand CloneCmd { get; }

        //private void CloneCmdExecute()
        //{
        //    try
        //    {
        //        if (SelectedRow == null) return;
        //        int id = GlobalConfig.DataRepository.WbEasyCalcDataListRepository.Clone(SelectedRow.Model.DemandPatternId);
        //        LoadData();
        //        SelectedRow = List.FirstOrDefault(x => x.Model.DemandPatternId == id);
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //public bool CloneCmdCanExecute()
        //{
        //    return SelectedRow != null;
        //}

        #endregion

        public ListViewModel()
        {
            try
            {
                //AddRowCmd = new RelayCommand(AddRowCmdExecute, AddRowCmdCanExecute);
                OpenRowCmd = new RelayCommand(OpenRowCmdExecute, OpenRowCmdCanExecute);
                //RemoveRowCmd = new RelayCommand(RemoveRowCmdExecute, RemoveRowCmdCanExecute);
                //SaveRowCmd = new RelayCommand(SaveRowCmdExecute, SaveRowCmdCanExecute);
                //CloneCmd = new RelayCommand(CloneCmdExecute, CloneCmdCanExecute);

                //Messenger.Default.Register<WbEasyCalcData>(this, OnSaveModel);
                LoadData();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Dispose()
        {
            //Messenger.Default.Unregister(this);
        }

        //private void OnSaveModel(WbEasyCalcData model)
        //{
        //    LoadData();
        //    SelectedRow = List.FirstOrDefault(x => x.Model.DemandPatternId == model.WbEasyCalcDataId);

        //    Messenger.Default.Send<ListViewModel>(this);
        //}

        private void LoadData()
        {
            var excludedPatternList = InfraRepo.ExcludedDemmandPattern.GetList();

            var list = InfraRepo.GetInfraData().InfraChangeableData.DemandPatternDict
                .Select(x => new RowViewModel(x, excludedPatternList.Any(f => f.Id==x.DemandPatternId)))
                .OrderBy(x => x.Model.DemandPatternId)
                .ToList()
                ;

            List = new ObservableCollection<RowViewModel>(list);
            RowsQty = List.Count;
        }

    }
}
