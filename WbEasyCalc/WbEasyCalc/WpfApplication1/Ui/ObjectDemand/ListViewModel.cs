using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Database.DataModel;
using Database.DataModel.Infra;
using Database.DataRepository.Infra;
using Database.DataRepository.WaterConsumption;
using GlobalRepository;
using NLog;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.ObjectDemand
{
    public class ListViewModel : ViewModelBase, IDisposable, IDialogViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region IDialogViewModel
        public string Title { get; set; } = "Demand Junction";

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

        #endregion

        #region Commands: OpenRowCmd

        public RelayCommand OpenRowCmd { get; }

        private void OpenRowCmdExecute()
        {
            try
            {
                if (SelectedRow == null) { return; }

                //var editedViewModel = new EditedViewModel(SelectedRow.Model.DemandPatternCurveId);
                //var result = DialogUtility.ShowModal(editedViewModel);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        public bool OpenRowCmdCanExecute()
        {
            return SelectedRow != null;
        }

        #endregion

        public ListViewModel(int id = 0)
        {
            try
            {
                OpenRowCmd = new RelayCommand(OpenRowCmdExecute, OpenRowCmdCanExecute);
                LoadData(id);
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Dispose()
        {
        }

        private void LoadData(int id)
        {
            InfraData infraData = InfraRepo.GetInfraData();


            var infraObjList = InfraRepo.GetInfraData().InfraChangeableData.InfraObjList;
            var infraValueList = InfraRepo.GetInfraData().InfraChangeableData.InfraValueList;
            var demandBaseList = InfraRepo.GetInfraData().InfraChangeableData.DemandBaseList;
            var demandPatternDict = InfraRepo.GetInfraData().InfraChangeableData.DemandPatternDict;
            var list = infraObjList
                .Join(
                    infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Label),
                    l => l.ObjId,
                    r => r.ObjId,
                    (l, r) => new { Obj = l, ObjName = r.StringValue }
                    )
                .Join(
                    infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Physical_Zone),
                    l => l.Obj.ObjId,
                    r => r.ObjId,
                    (l, r) => new { Obj = l.Obj, l.ObjName, ZoneId = r.IntValue }
                    )
                .Join(
                    infraValueList,
                    l => l.Obj.ObjId,
                    r => r.ObjId,
                    (l, r) => new { l.Obj, l.ObjName, l.ZoneId, Value = r }
                    )
                .Join(
                    demandBaseList,
                    l => l.Value.ValueId,
                    r => r.ValueId,
                    (l, r) => new { l.Obj, l.ObjName, l.ZoneId, l.Value, DemandBase = r }
                    )
                .Join(
                    demandPatternDict,
                    l => l.DemandBase.DemandPatternId,
                    r => r.DemandPatternId,
                    (l, r) => new { l.Obj, l.ObjName, Zone = GetZone(l.ZoneId), l.Value, l.DemandBase, DemandPattern = r }
                )
                .Select(x => new RowViewModel(x.Obj, x.ObjName, x.Zone, x.DemandBase, x.DemandPattern))
                .OrderBy(x => x.ObjModel.ObjId)
                .ThenBy(x => x.DemandPatternModel.Name)
                .ToList()
                ;
            List = new ObservableCollection<RowViewModel>(list);
            RowsQty = List.Count;
        }

        private InfraZone GetZone(int? zoneId)
        {
            return InfraRepo.GetInfraData().InfraChangeableData.ZoneDict.FirstOrDefault(f => f.ZoneId==zoneId);
        }
    }
}
