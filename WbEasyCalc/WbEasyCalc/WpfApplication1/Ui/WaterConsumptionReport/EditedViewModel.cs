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
using WpfApplication1.Ui.WbEasyCalcData.WaterConsumption;

namespace WpfApplication1.Ui.WaterConsumptionReport
{
    public class EditedViewModel : ViewModelBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
            set { _filterStartDate = value; RaisePropertyChanged(nameof(FilterStartDate)); LoadData(); }
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

        public RelayCommand<object> MouseMoveCmd { get; }

        #endregion

        public EditedViewModel()
        {
            try
            {
                Logger.Info("New 'EditedViewModel' was created.");

                MapOpacity = 1;
                ZoomLevel = 15;
                Center = new Location(51.20150, 16.17970);        


                WaterConsumptionCategoryList = GlobalConfig.DataRepository.WaterConsumptionCategoryList;
                WaterConsumptionStatusList = GlobalConfig.DataRepository.WaterConsumptionStatusList;
                ZoneItemList = GlobalConfig.DataRepository.ZoneList;
                FilterStartDate = new DateTime(2019, 1, 1, 0, 0, 0);
                FilterEndDate = new DateTime(2022, 1, 1, 0, 0, 0);

                LoadData();

                MouseMoveCmd = new RelayCommand<object>(MouseMove);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void LoadData()
        {
            var rowModelList = GlobalConfig.DataRepository.WaterConsumptionListRepository.GetList().Where(x => x.StartDate >= FilterStartDate && x.EndDate <= FilterEndDate).Select(x => new RowViewModel(x));
            var mapItemList = rowModelList.Select(x => new MapItem1()
            {
                Id = 1,
                TypeId = 1,
                Location = new Location(x.Model.Latitude, x.Model.Lontitude),
                Name = GetPushPinName(new Location(x.Model.Latitude, x.Model.Lontitude)),
            });
            MapItemList = new ObservableCollection<IMapItem>(mapItemList);
        }


        private void MouseMove(object obj)
        {
            var ea = (MouseEventArgs)obj;
            var originalSource = ea.OriginalSource;

            if (originalSource is Border)
            {
                var map = (Microsoft.Maps.MapControl.WPF.Map)ea.Source;

                Point mousePosition = ea.GetPosition(map);
                Location mouseLocation = map.ViewportPointToLocation(mousePosition);

                MapItemList[0].Location = mouseLocation;
                MapItemList[0].Name = GetPushPinName(mouseLocation);

                //Model.Latitude = mouseLocation.Latitude;
                //Model.Lontitude = mouseLocation.Longitude;
            }
        }

        private string GetPushPinName(Location location) => $"{location.Latitude} - {location.Longitude}";
    }
}
