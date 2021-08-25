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

namespace WpfApplication1.Ui.TableCustomerMeter
{
    public class ListViewModel : ViewModelBase, IDisposable, IDialogViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region IDialogViewModel
        public string Title { get; set; } = "Customer Meter Table";

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

        public ListViewModel(int typeId = 1)
        {
            try
            {
                OpenRowCmd = new RelayCommand(OpenRowCmdExecute, OpenRowCmdCanExecute);
                LoadData(typeId);
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

        private void LoadData(int typeId)
        {
            InfraData infraData = InfraRepo.GetInfraData();


            var infraObjList = infraData.InfraChangeableData.InfraObjList;
            var infraValueList = infraData.InfraChangeableData.InfraValueList;
            var demandBaseList = infraData.InfraChangeableData.DemandBaseList;
            var demandPatternDict = infraData.InfraChangeableData.DemandPatternDict;
            var zoneDict = infraData.InfraChangeableData.ZoneDict;


            var junctionZoneDict = infraObjList.Where(f => f.ObjTypeId==55)
                .Join(
                    infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Physical_Zone),
                    l => l.ObjId,
                    r => r.ObjId,
                    (l, r) => new { l.ObjId, ZoneId = r.IntValue }
                    )
                .Join(
                    zoneDict,
                    l => l.ZoneId,
                    r => r.ZoneId,
                    (l, r) => new { l.ObjId, Zone = r }
                    )
                .ToDictionary(x => x.ObjId, x => x.Zone);


            var baseList = infraObjList.Where(f => f.ObjTypeId == 73)
                .Join(
                    infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Label),
                    l => l.ObjId,
                    r => r.ObjId,
                    (l, r) => new { Obj = l, ObjName = r.StringValue }
                    )
                .Join(
                    infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.HMIActiveTopologyIsActive),
                    l => l.Obj.ObjId,
                    r => r.ObjId,
                    (l, r) => new { Obj = l.Obj, l.ObjName, IsActive = r.BooleanValue }
                    )
                .Join(
                    infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Demand_AssociatedElement),
                    l => l.Obj.ObjId,
                    r => r.ObjId,
                    (l, r) => new { l.Obj, l.ObjName, l.IsActive, AssociatedElementId = r.IntValue }
                    )
                //.Join(
                //    infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Demand_BaseFlow),
                //    l => l.Obj.ObjId,
                //    r => r.ObjId,
                //    (l, r) => new { l.Obj, l.ObjName, l.IsActive, l.AssociatedElementId, DemandBase = r.FloatValue }
                //    )
                //.Join(
                //    infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Demand_DemandPattern),
                //    l => l.Obj.ObjId,
                //    r => r.ObjId,
                //    (l, r) => new { l.Obj, l.ObjName, l.IsActive, l.AssociatedElementId, l.DemandBase, DemandPatternId = r.IntValue }
                //    )
                //.ToList()
                ;

            var list = baseList
                .Join(
                    infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Demand_BaseFlow),
                    l => l.Obj.ObjId,
                    r => r.ObjId,
                    (l, r) => new { l.Obj, l.ObjName, l.IsActive, l.AssociatedElementId, DemandBase = r.FloatValue }
                    )
                .Join(
                    infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Demand_DemandPattern),
                    l => l.Obj.ObjId,
                    r => r.ObjId,
                    (l, r) => new { l.Obj, l.ObjName, l.IsActive, l.AssociatedElementId, l.DemandBase, DemandPatternId = r.IntValue }
                    )
                .Select(x => new RowViewModel(x.Obj, x.ObjName, x.IsActive ?? false, GetZone(x.AssociatedElementId, junctionZoneDict), x.DemandBase, GetDemandPattern(x.DemandPatternId)))
                .OrderBy(x => x.ObjModel.ObjId)
                .ThenBy(x => x.DemandPatternModel?.Name)
                ;

            List = new ObservableCollection<RowViewModel>(list);
            RowsQty = List.Count;
        }

        private InfraDemandPattern GetDemandPattern(int? demandPatternId)
        {
            return InfraRepo.GetInfraData().InfraChangeableData.DemandPatternDict.FirstOrDefault(f => f.DemandPatternId == (demandPatternId ?? -1));
        }

        private InfraZone GetZone(int? associatedElementId, Dictionary<int, InfraZone> junctionZoneDict)
        {
            return junctionZoneDict.FirstOrDefault(f => f.Key == associatedElementId).Value;
        }
    }
}
