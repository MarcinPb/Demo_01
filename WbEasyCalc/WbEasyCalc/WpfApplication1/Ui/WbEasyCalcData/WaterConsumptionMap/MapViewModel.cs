using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DataModel;
using DataRepository;
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

namespace WpfApplication1.Ui.WbEasyCalcData.WaterConsumptionMap
{
    public class MapViewModel : ViewModelBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private int _yearNo;
        private int _monthNo;
        private int _zoneId;

        //private ItemViewModel _model;
        //public ItemViewModel Model
        //{
        //    get => _model;
        //    set { _model = value; RaisePropertyChanged(); }
        //}

        public List<IdNamePair> WaterConsumptionCategoryList { get; set; }
        public List<IdNamePair> WaterConsumptionStatusList { get; set; }
        public List<ZoneItem> ZoneItemList { get; set; }

        private DateTime _filterStartDate;
        public DateTime FilterStartDate
        {
            get => _filterStartDate;
            set { _filterStartDate = value; RaisePropertyChanged(nameof(FilterStartDate)); LoadData(_yearNo, _monthNo, _zoneId); }
        }

        public DateTime FilterEndDate { get; set; }

        private ObservableCollection<IMapItem> _mapItemList;
        public ObservableCollection<IMapItem> MapItemList
        {
            get => _mapItemList;
            set { _mapItemList = value; RaisePropertyChanged(nameof(MapItemList)); }
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

        //public EditedViewModel(int id)
        //{
        //    var WbEasyCalcDataModel = GlobalConfig.DataRepository.WbEasyCalcDataListRepository.GetItem(id);
        //    _yearNo = WbEasyCalcDataModel.YearNo;
        //    _monthNo = WbEasyCalcDataModel.MonthNo;
        //    _zoneId = WbEasyCalcDataModel.ZoneId;
        //}

        public MapViewModel(int yearNo, int monthNo, int zoneId)
        {
            try
            {
                Logger.Info("New 'EditedViewModel' was created.");

                _yearNo = yearNo;
                _monthNo = monthNo;
                _zoneId = zoneId;

                MapOpacity = 1;
                ZoomLevel = 15;
                Center = new Location(51.20150, 16.17970);


                WaterConsumptionCategoryList = GlobalConfig.DataRepository.WaterConsumptionCategoryList;
                WaterConsumptionStatusList = GlobalConfig.DataRepository.WaterConsumptionStatusList;
                ZoneItemList = GlobalConfig.DataRepository.ZoneList;

                if (_yearNo == 0)
                {
                    FilterStartDate = new DateTime(2019, 1, 1, 0, 0, 0);
                    FilterEndDate = new DateTime(2022, 1, 1, 0, 0, 0);
                }
                else
                {
                    FilterStartDate = new DateTime(_yearNo, _monthNo, 1, 0, 0, 0);
                    FilterEndDate = FilterStartDate.AddMonths(1).AddSeconds(-1);
                }

                LoadData(_yearNo, _monthNo, _zoneId);

                MouseDoubleClickCmd = new RelayCommand<object>(MouseDoubleClick);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void LoadData(int yearNo, int monthNo, int zoneId)
        {
            var rowModelList = GlobalConfig.DataRepository.WaterConsumptionListRepository.GetList().Where(x => x.StartDate >= FilterStartDate && x.EndDate <= FilterEndDate).Select(x => new RowViewModel(x));
            if (_yearNo != 0)
            {
                rowModelList = rowModelList.Where(x => x.Model.ZoneId == _zoneId);
            }
            var mapItemList = rowModelList.Select(x => new MapItem1()
            {
                Id = 1,
                TypeId = 1,
                Location = new Location(x.Model.Latitude, x.Model.Lontitude),
                Name = GetPushPinName(new Location(x.Model.Latitude, x.Model.Lontitude)),
            });
            MapItemList = new ObservableCollection<IMapItem>(mapItemList);
        }

        private void MouseDoubleClick(object obj)
        {
            var ea = (MouseEventArgs)obj;
            var originalSource = ea.OriginalSource;

            if (originalSource is Border)
            {
                var map = (Microsoft.Maps.MapControl.WPF.Map)ea.Source;

                Point mousePosition = ea.GetPosition(map);
                Location mouseLocation = map.ViewportPointToLocation(mousePosition);

                var editedViewModel = new WaterConsumption.EditedViewModel(0);
                var result = DialogUtility.ShowModal(editedViewModel);
                editedViewModel.Dispose();
            }
        }

        private string GetPushPinName(Location location) => $"{location.Latitude} - {location.Longitude}";
    }
}
