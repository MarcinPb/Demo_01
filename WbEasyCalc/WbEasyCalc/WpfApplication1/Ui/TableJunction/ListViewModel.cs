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

namespace WpfApplication1.Ui.TableJunction
{
    public class ListViewModel : ViewModelBase, IDisposable, IDialogViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region IDialogViewModel
        public string Title { get; set; } = "Junction Table";

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

                var id = SelectedRow.ObjModel.ObjId;

                var editedViewModel = new EditedViewModel(SelectedRow);
                var result = DialogUtility.ShowModal(editedViewModel);
                if ((bool)result) 
                {
                    LoadData();
                    SelectedRow = List.FirstOrDefault(x => x.ObjModel.ObjId == id);
                }
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

        public ListViewModel()
        {
            try
            {
                OpenRowCmd = new RelayCommand(OpenRowCmdExecute, OpenRowCmdCanExecute);
                LoadData();
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

        private void LoadData()
        {
            InfraData infraData = InfraRepo.GetInfraData();


            var infraObjList = infraData.InfraChangeableData.InfraObjList;
            var infraValueList = infraData.InfraChangeableData.InfraValueList;
            var demandBaseList = infraData.InfraChangeableData.DemandBaseList;
            var demandPatternDict = infraData.InfraChangeableData.DemandPatternDict;
            var zoneDict = infraData.InfraChangeableData.ZoneDict;

            //var demandSettingObjList = infraData.InfraChangeableData.DemandSettingObjList;
            var demandSettingsObjList = InfraRepo.TableCustomerMeter.GetList();
            
            // Reading data for DemandSettings
            var demandSettingsList = demandSettingsObjList
                .Join(
                    demandPatternDict,
                    l => l.DemandPatternId,
                    r => r.DemandPatternId,
                    (l, r) => new { l.ObjId, l.IsExcluded, l.DemandBaseValue, DemandPattern = r }
                    )
                .ToList()
                ;

            var junctionDemandPatternList = infraObjList.Where(f => f.ObjTypeId == 55)
                .Join(
                    infraValueList,
                    l => l.ObjId,
                    r => r.ObjId,
                    (l, r) => new { l.ObjId, r.ValueId }
                    )
                .Join(
                    demandBaseList,
                    l => l.ValueId,
                    r => r.ValueId,
                    (l, r) => new { l.ObjId, r.DemandPatternId }
                    )
                .GroupBy(x => x.ObjId)
                .Select(group => new { ObjId = group.Key, Count = group.Count() })
                .ToList()
                //.ToDictionary(x => x.ObjId, x => x.Count)
                ;


            var baseList = infraObjList.Where(f => f.ObjTypeId == 55)
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
                    infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Physical_Zone),
                    l => l.Obj.ObjId,
                    r => r.ObjId,
                    (l, r) => new { Obj = l.Obj, l.ObjName, l.IsActive, ZoneId = r.IntValue }
                    )
                .Join(
                    infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Physical_NodeElevation),
                    l => l.Obj.ObjId,
                    r => r.ObjId,
                    (l, r) => new { Obj = l.Obj, l.ObjName, l.IsActive, l.ZoneId, NodeElevation = r.FloatValue }
                    )
                .ToList()
                ;

            //var list = baseList
            //    .Join(
            //        infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Demand_BaseFlow),
            //        l => l.Obj.ObjId,
            //        r => r.ObjId,
            //        (l, r) => new { l.Obj, l.ObjName, l.IsActive, l.AssociatedElementId, DemandBaseDmSet = r.FloatValue }
            //        )
            //    .Join(
            //        infraValueList.Where(f => f.FieldId == infraData.InfraSpecialFieldId.Demand_DemandPattern),
            //        l => l.Obj.ObjId,
            //        r => r.ObjId,
            //        (l, r) => new { l.Obj, l.ObjName, l.IsActive, l.AssociatedElementId, l.DemandBaseDmSet, DemandPatternId = r.IntValue }
            //        )
            //    .Select(x => new RowViewModel(x.Obj, x.ObjName, x.IsActive ?? false, GetZone(x.AssociatedElementId, junctionZoneDict), x.DemandBaseDmSet, GetDemandPattern(x.DemandPatternId)))
            //    .OrderBy(x => x.ObjModel.ObjId)
            //    .ThenBy(x => x.DemandPatternModelDmSet?.Name)
            //    ;


            var list = baseList
                .Select(x => new
                    {
                        Object = x,
                        DemandSettings = demandSettingsList.FirstOrDefault(f => f.ObjId == x.Obj.ObjId)
                    })
                .Select(x => new RowViewModel(
                    x.Object.Obj, 
                    x.Object.ObjName, 
                    GetDemandPatternCountFormated(junctionDemandPatternList.FirstOrDefault(f => f.ObjId == x.Object.Obj.ObjId)?.Count), 
                    0,                                                              
                    null,                                                             
                    x.Object.IsActive ?? false, 
                    GetZone(x.Object.ZoneId), 
                    x.Object.NodeElevation,
                    x.DemandSettings?.DemandBaseValue,
                    x.DemandSettings?.DemandPattern,
                    x.DemandSettings?.IsExcluded ?? false       
                    ))
                .OrderBy(x => x.ObjModel.ObjId)
                .ThenBy(x => x.DemandPatternNameDmSet)
                .ToList()
                ;

            List = new ObservableCollection<RowViewModel>(list);
            RowsQty = List.Count;
        }

        private InfraDemandPattern GetDemandPattern(int? demandPatternId)
        {
            return InfraRepo.GetInfraData().InfraChangeableData.DemandPatternDict.FirstOrDefault(f => f.DemandPatternId == (demandPatternId ?? -1));
        }

        private InfraZone GetZone(int? zoneId)
        {
            return InfraRepo.GetInfraData().InfraChangeableData.ZoneDict.FirstOrDefault(f => f.ZoneId == zoneId);
        }
        private string GetDemandPatternCountFormated(int? count)
        {
            return $"<Collection: {count ?? 0} items>";
        }
    }
}
