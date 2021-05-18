using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Database.DataModel;

using GlobalRepository;
using Microsoft.WindowsAPICodePack.Dialogs;
using WbEasyCalcModel;
using WpfApplication1.Utility;
using Microsoft.Maps.MapControl.WPF;
using System.Collections.ObjectModel;
using WpfApplication1.Map;
using System.Windows.Input;
using System.Windows.Controls;
using NLog;
using System.Collections.Specialized;
using Database.DataRepository.Infra;

namespace WpfApplication1.Ui.WaterConsumptionMap
{
    public class MapViewModel : ViewModelBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private int _yearNo;
        private int _monthNo;
        private int _zoneId;


        private List<WaterConsumption> _waterConsumptionList;
        public List<WaterConsumption> WaterConsumptionList
        {
            get => _waterConsumptionList;
            set { _waterConsumptionList = value; RaisePropertyChanged(); LoadData(); }
        }

        private ObservableCollection<IMapItem> _mapItemList;
        public ObservableCollection<IMapItem> MapItemList
        {
            get => _mapItemList;
            set { _mapItemList = value; RaisePropertyChanged(nameof(MapItemList)); }
        }

        public ObservableCollection<IdNamePair> WaterConsumptionCategoryList { get; set; }
        public ObservableCollection<IdNamePair> SelectedWaterConsumptionCategoryList { get; set; }

        public ObservableCollection<IdNamePair> WaterConsumptionStatusList { get; set; }
        public ObservableCollection<IdNamePair> SelectedWaterConsumptionStatusList { get; set; }

        public ObservableCollection<ZoneItem> ZoneItemList { get; set; }
        public ObservableCollection<ZoneItem> SelectedZoneItemList { get; set; }

        private DateTime _filterStartDate;
        public DateTime FilterStartDate
        {
            get => _filterStartDate;
            set { _filterStartDate = value; RaisePropertyChanged(); LoadData(); }
        }
        private DateTime _filterEndDate;
        public DateTime FilterEndDate
        {
            get => _filterEndDate;
            set { _filterEndDate = value; RaisePropertyChanged(); LoadData(); }
        }
        private double _valueFrom;
        public double ValueFrom
        {
            get => _valueFrom;
            set { _valueFrom = value; RaisePropertyChanged(); LoadData(); }
        }
        private double _valueTo;
        public double ValueTo
        {
            get => _valueTo;
            set { _valueTo = value; RaisePropertyChanged(); LoadData(); }
        }

        #region Map

        private Location _center;
        public Location Center
        {
            get => _center;
            set { _center = value; RaisePropertyChanged(); }
        }

        private int _zoomLevel;
        public int ZoomLevel
        {
            get => _zoomLevel;
            set { _zoomLevel = value; RaisePropertyChanged(); }
        }

        private double _mapOpacity;
        public double MapOpacity
        {
            get => _mapOpacity;
            set { _mapOpacity = value; RaisePropertyChanged(); }
        }

        public RelayCommand<object> MouseDoubleClickCmd { get; }

        #endregion

        public MapViewModel(int yearNo, int monthNo, int zoneId)
        {
            try
            {
                Logger.Info("New 'EditedViewModel' was created.");

                _yearNo = yearNo;
                _monthNo = monthNo;
                _zoneId = zoneId;

                MapOpacity = 1;
                ZoomLevel = 11;
                Center = new Location(51.20150, 16.17970);


                WaterConsumptionCategoryList = new ObservableCollection<IdNamePair>(GlobalConfig.DataRepository.WaterConsumptionCategoryList);
                SelectedWaterConsumptionCategoryList = new ObservableCollection<IdNamePair>(GlobalConfig.DataRepository.WaterConsumptionCategoryList);
                SelectedWaterConsumptionCategoryList.CollectionChanged += SelectedWaterConsumptionCategoryList_CollectionChanged;

                WaterConsumptionStatusList = new ObservableCollection<IdNamePair>(GlobalConfig.DataRepository.WaterConsumptionStatusList);
                SelectedWaterConsumptionStatusList = new ObservableCollection<IdNamePair>(GlobalConfig.DataRepository.WaterConsumptionStatusList);
                SelectedWaterConsumptionStatusList.CollectionChanged += SelectedWaterConsumptionStatusList_CollectionChanged;

                ZoneItemList = new ObservableCollection<ZoneItem>(GlobalConfig.DataRepository.ZoneList);
                SelectedZoneItemList = new ObservableCollection<ZoneItem>(GlobalConfig.DataRepository.ZoneList);
                SelectedZoneItemList.CollectionChanged += SelectedZoneItemList_CollectionChanged;

                if (_yearNo == 0)
                {
                    FilterStartDate = new DateTime(yearNo, 1, 1, 0, 0, 0);
                    FilterEndDate = FilterStartDate.AddYears(1).AddSeconds(-1); ;
                }
                else
                {
                    FilterStartDate = new DateTime(_yearNo, _monthNo, 1, 0, 0, 0);
                    FilterEndDate = FilterStartDate.AddMonths(1).AddSeconds(-1);
                }

                ValueFrom = 0;
                ValueTo = 999999;

                //LoadData();

                MouseDoubleClickCmd = new RelayCommand<object>(MouseDoubleClick);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void SelectedZoneItemList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LoadData();
        }

        private void SelectedWaterConsumptionStatusList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LoadData();
        }

        private void SelectedWaterConsumptionCategoryList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            if (WaterConsumptionList == null) { return; }

            //var rowModelList1 = GlobalConfig.DataRepository.WaterConsumptionListRepositoryTemp.GetList();
            var rowModelList1 = WaterConsumptionList;
            var rowModelList = rowModelList1.Where(f =>
                SelectedWaterConsumptionCategoryList.Any(ct => f.WaterConsumptionCategoryId == ct.Id) &&
                SelectedWaterConsumptionStatusList.Any(st => f.WaterConsumptionStatusId == st.Id) &&
                SelectedZoneItemList.Any(zn => _zoneId == zn.ZoneId) &&
                f.StartDate >= FilterStartDate &&
                f.EndDate <= FilterEndDate &&
                f.Value >= ValueFrom &&
                f.Value <= ValueTo
            );

            var mapItemList = rowModelList.Select(x => new MapItem1()
            {
                Id = 1,
                TypeId = 1,
                Name = GetPushPinName(x),
                Location = GetLocationFromGis(x.Lontitude, x.Latitude),
            });
            MapItemList = new ObservableCollection<IMapItem>(mapItemList);
        }

        private Location GetLocationFromGis(double x, double y)
        {
            var moveXx = -63.4400885695583;
            var multiXx = 0.0000142625456859728;

            var moveYy = 0.562678511340267;
            var multiYy = 0.00000892357598435176;

            return new Location(moveYy + y * multiYy, moveXx + x * multiXx);
        }

        private void MouseDoubleClick(object obj)
        {
            //var ea = (MouseEventArgs)obj;
            //var originalSource = ea.OriginalSource;

            //if (originalSource is Border)
            //{
            //    var map = (Microsoft.Maps.MapControl.WPF.Map)ea.Source;

            //    Point mousePosition = ea.GetPosition(map);
            //    Location mouseLocation = map.ViewportPointToLocation(mousePosition);

            //    var editedViewModel = new WaterConsumption.EditedViewModel(0);
            //    var result = DialogUtility.ShowModal(editedViewModel);
            //    editedViewModel.Dispose();
            //}
        }

        private string GetPushPinName(WaterConsumption waterConsumption)
        {
            var infraObjName = InfraRepo.GetInfraData().InfraChangeableData.InfraValueList.FirstOrDefault(x => x.ObjId == waterConsumption.RelatedId && x.FieldId == 2).StringValue;    // Label
            return $"{infraObjName} - {waterConsumption.Value} m3 - {waterConsumption.StartDate} - {waterConsumption.EndDate}";
        }
    }
}
