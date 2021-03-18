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

namespace WpfApplication1.Ui.Configuration
{
    public class EditedViewModel : ViewModelBase, IDialogViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region IDialogViewModel

        public string Title { get; set; } = "Options";

        public bool Save()
        {
            //DataModel.WaterConsumption model = GlobalConfig.DataRepository.WaterConsumptionListRepositoryTemp.SaveItem(Model.Model);
            //Messenger.Default.Send<EditedViewModel>(this);
            return true;
        }

        public void Close()
        {
        }

        #endregion




        //private ItemViewModel _model;
        //public ItemViewModel Model
        //{
        //    get => _model;
        //    set { _model = value; RaisePropertyChanged(); }
        //}

        public List<IdNamePair> WaterConsumptionCategoryList { get; set; }
        public List<IdNamePair> WaterConsumptionStatusList { get; set; }
        public List<ZoneItem> ZoneItemList { get; set; }


        public ObservableCollection<IMapItem> MapItemList { get; set; }

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

                //Model = new ItemViewModel(GlobalConfig.DataRepository.WaterConsumptionListRepository.GetItem(id));

                WaterConsumptionCategoryList = GlobalConfig.DataRepository.WaterConsumptionCategoryList;
                WaterConsumptionStatusList = GlobalConfig.DataRepository.WaterConsumptionStatusList;
                ZoneItemList = GlobalConfig.DataRepository.ZoneList;

                MapOpacity = 1;
                ZoomLevel = 15;
                Location defaulLocation = new Location(51.20150, 16.17970);
                Center = defaulLocation;

                MapItemList = new ObservableCollection<IMapItem>()
                {
                    new MapItem1()
                    {
                        Id = 1,
                        TypeId = 1,
                        Location = defaulLocation,
                        Name = GetPushPinName(defaulLocation)
                    }
                };
                //Model.Latitude = defaulLocation.Latitude;
                //Model.Lontitude = defaulLocation.Longitude;


                MouseMoveCmd = new RelayCommand<object>(MouseMove);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
