using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DataRepository;
using GlobalRepository;
using WbEasyCalcModel;
using WbEasyCalcRepository;
using WpfApplication1.Ui.WbEasyCalcData.Excel;
//using WpfApplication1.Ui.WbEasyCalcData.ViewModel.Tabs;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData
{

    public class ItemViewModel : ViewModelBase
    {

        #region Props ViewModel: Id, ZoneId,...

        private int _id;
        public int Id
        {
            get => _id;
            set { _id = value; RaisePropertyChanged(nameof(Id)); }
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


        private int _yearNo;
        public int YearNo
        {
            get
            {
                return _yearNo;
            }
            set
            {
                _yearNo = value;
                RaisePropertyChanged("YearNo");
                CalculateDaysNumber();
            }
        }
        public string YearName => GlobalConfig.DataRepository.YearList.FirstOrDefault(x => x.Id == YearNo)?.Name;

        private int _monthNo;
        public int MonthNo
        {
            get
            {
                return _monthNo;
            }
            set
            {
                _monthNo = value;
                RaisePropertyChanged("MonthNo");
                CalculateDaysNumber();
            }
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
                //if (IsArchive == false)
                //{
                //    IsAccepted = false;
                //}
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

        #endregion

        //private Ui.WbEasyCalcData.WaterConsumption.ListViewModel _waterConsumptionViewModel;
        //public Ui.WbEasyCalcData.WaterConsumption.ListViewModel WaterConsumptionListViewModel
        //{
        //    get => _waterConsumptionViewModel;
        //    set { _waterConsumptionViewModel = value; RaisePropertyChanged(); }
        //}
        public Ui.WbEasyCalcData.WaterConsumption.ListViewModel WaterConsumptionListViewModel { get; set; }



        private ExcelViewModel _easyCalcViewModel;
        public ExcelViewModel EasyCalcViewModel
        {
            get => _easyCalcViewModel;
            set { _easyCalcViewModel = value; RaisePropertyChanged(nameof(EasyCalcViewModel)); }
        }


        public WaterConsumptionMap.MapViewModel WaterConsumptionReportViewModel { get; set; }


        public DataModel.WbEasyCalcData Model
        {
            get
            {
                return new DataModel.WbEasyCalcData()
                {
                    WbEasyCalcDataId = this.Id,

                    CreateLogin = CreateLogin,
                    CreateDate = CreateDate,
                    ModifyLogin = ModifyLogin,
                    ModifyDate = ModifyDate,

                    YearNo = YearNo,
                    MonthNo = MonthNo,
                    ZoneId = ZoneId,
                    Description = Description,
                    IsArchive = IsArchive,
                    IsAccepted = IsAccepted,

                    WaterConsumptionModelList = WaterConsumptionListViewModel.List.Select(x => x.Model).ToList(),
                    EasyCalcModel = EasyCalcViewModel?.Model,
                };
            }
        }

        public ItemViewModel(DataModel.WbEasyCalcData model)
        {
            Id = model.WbEasyCalcDataId;

            if (model.WbEasyCalcDataId != 0)
            {
                ZoneId = model.ZoneId;
                YearNo = model.YearNo;
                MonthNo = model.MonthNo;
                //Start_PeriodDays_M21 = model.EasyCalcModel.StartModel.Start_PeriodDays_M21;
            }
            else
            {
                ZoneId = GlobalConfig.DataRepository.ZoneList.First().ZoneId;
                YearNo = DateTime.Now.Year;
                MonthNo = DateTime.Now.Month;
                //Start_PeriodDays_M21 = model.Start_PeriodDays_M21;
                CalculateDaysNumber();
            }

            CreateLogin = model.CreateLogin;
            CreateDate = model.CreateDate;
            ModifyLogin = model.ModifyLogin;
            ModifyDate = model.ModifyDate;

            Description = model.Description;
            IsArchive = model.IsArchive;
            IsAccepted = model.IsAccepted;

            EasyCalcViewModel = new ExcelViewModel(model.EasyCalcModel);

            WaterConsumptionListViewModel = new Ui.WbEasyCalcData.WaterConsumption.ListViewModel(Id);

            WaterConsumptionReportViewModel = new Ui.WbEasyCalcData.WaterConsumptionMap.MapViewModel(YearNo, MonthNo, ZoneId);
        }

        private void CalculateDaysNumber()
        {
            if (MonthNo == 0 || YearNo == 0)
            {
                return;
            }
            //Start_PeriodDays_M21 = MonthNo == 13 ? new DateTime(YearNo, 12, 31).DayOfYear : DateTime.DaysInMonth(YearNo, MonthNo);
        }

    }
}
