using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using GlobalRepository;
using WbEasyCalcModel;
using WbEasyCalcRepository;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.DesignerWithPropreryGrid
{

    public class ItemViewModel : ViewModelBase
    {
        public Database.DataModel.WaterConsumption Model => new Database.DataModel.WaterConsumption()
        {
            WaterConsumptionId = Id,
            WbEasyCalcDataId = WbEasyCalcDataId,
            Description = Description,
            IsArchive = IsArchive,
            IsAccepted = IsAccepted,

            WaterConsumptionCategoryId = WaterConsumptionCategoryId,
            WaterConsumptionStatusId = WaterConsumptionStatusId,
            ZoneId = ZoneId,

            StartDate = StartDate,
            EndDate = EndDate,
            Latitude = Latitude,
            Lontitude = Lontitude,
            Value = Value,
        };

        #region Props ViewModel: Id, ZoneId,...

        private int _id;
        public int Id
        {
            get => _id;
            set { _id = value; RaisePropertyChanged(nameof(Id)); }
        }

        private int _wbEasyCalcDataId;
        public int WbEasyCalcDataId
        {
            get => _wbEasyCalcDataId;
            set { _wbEasyCalcDataId = value; RaisePropertyChanged(nameof(WbEasyCalcDataId)); }
        }

        private string _createLogin;
        public string CreateLogin
        {
            get => _createLogin;
            set { _createLogin = value; RaisePropertyChanged(nameof(CreateLogin)); }
        }

        private DateTime _createDate;
        public DateTime CreateDate
        {
            get => _createDate;
            set { _createDate = value; RaisePropertyChanged(nameof(CreateDate)); }
        }

        private string _modifyLogin;
        public string ModifyLogin
        {
            get => _modifyLogin;
            set { _modifyLogin = value; RaisePropertyChanged(nameof(ModifyLogin)); }
        }

        private DateTime _modifyDate;
        public DateTime ModifyDate
        {
            get => _modifyDate;
            set { _modifyDate = value; RaisePropertyChanged(nameof(ModifyDate)); }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        private bool _isArchive;
        public bool IsArchive
        {
            get
            {
                return _isArchive;
            }
            set
            {
                _isArchive = value;
                RaisePropertyChanged();
            }
        }

        private bool _isAccepted;
        public bool IsAccepted
        {
            get
            {
                return _isAccepted;
            }
            set
            {
                _isAccepted = value;
                RaisePropertyChanged();
            }
        }

        private int _waterConsumptionCategoryId;
        public int WaterConsumptionCategoryId
        {
            get => _waterConsumptionCategoryId;
            set 
            { 
                _waterConsumptionCategoryId = value; 
                RaisePropertyChanged(nameof(WaterConsumptionCategoryId));
                Messenger.Default.Send<ItemViewModel>(this);
            }
        }

        private int _waterConsumptionStatusId;
        public int WaterConsumptionStatusId
        {
            get => _waterConsumptionStatusId;
            set { _waterConsumptionStatusId = value; RaisePropertyChanged(nameof(WaterConsumptionStatusId)); }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; RaisePropertyChanged(nameof(StartDate)); }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; RaisePropertyChanged(nameof(EndDate)); }
        }


        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set { _latitude = value; RaisePropertyChanged(nameof(Latitude)); }
        }

        private double _lontitude;
        public double Lontitude
        {
            get => _lontitude;
            set { _lontitude = value; RaisePropertyChanged(nameof(Lontitude)); }
        }

        private double _value;
        public double Value
        {
            get => _value;
            set { _value = value; RaisePropertyChanged(nameof(Value)); }
        }

        private int _zoneId;
        public int ZoneId
        {
            get
            {
                return _zoneId;
            }
            set
            {
                _zoneId = value;
                RaisePropertyChanged("ZoneId");
            }
        }

        #endregion


        public ItemViewModel(Database.DataModel.WaterConsumption model)
        {
            Id = model.WaterConsumptionId;

            if (model.WaterConsumptionId != 0)
            {
                WbEasyCalcDataId = model.WbEasyCalcDataId;
                ZoneId = model.ZoneId;
                WaterConsumptionCategoryId = model.WaterConsumptionCategoryId;
                WaterConsumptionStatusId = model.WaterConsumptionStatusId;
                StartDate = model.StartDate;
                EndDate = model.EndDate;
            }
            else
            {
                WbEasyCalcDataId = GlobalConfig.DataRepository.WbEasyCalcDataListRepository.GetList().First().WbEasyCalcDataId; 
                ZoneId = GlobalConfig.DataRepository.ZoneList.First().ZoneId;
                WaterConsumptionCategoryId = GlobalConfig.DataRepository.WaterConsumptionCategoryList.First().Id;
                WaterConsumptionStatusId = GlobalConfig.DataRepository.WaterConsumptionStatusList.First().Id;
                StartDate = DateTime.Now;
                EndDate = DateTime.Now;
            }

            Description = model.Description;
            IsArchive = model.IsArchive;
            IsAccepted = model.IsAccepted;

            Latitude = model.Latitude;
            Lontitude = model.Lontitude;
            Value = model.Value;
        }
    }
}
