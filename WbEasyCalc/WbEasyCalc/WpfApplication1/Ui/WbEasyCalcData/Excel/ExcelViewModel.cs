using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WbEasyCalcRepository;
//using WpfApplication1.Ui.WbEasyCalcData.Excel.WaterBalance;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel
{
    public class ExcelViewModel : ViewModelBase, IDisposable
    {

        public Start.ViewModel StartViewModel { get; set; }
        public SysInput.ViewModel SysInputViewModel { get; set; }
        public BilledCons.ViewModel BilledConsViewModel { get; set; }
        public UnbCons.ViewModel UnbConsViewModel { get; set; }
        public UnauthCons.ViewModel UnauthConsViewModel { get; set; }
        public MeterErrors.ViewModel MeterErrorsViewModel { get; set; }
        public Network.ViewModel NetworkViewModel { get; set; }
        public Pressure.ViewModel PressureViewModel { get; set; }
        public IntermittentSupply.ViewModel IntermittentSupplyViewModel { get; set; }
        public FinancialData.ViewModel FinancialDataViewModel { get; set; }



        private WaterBalance.ViewModel _waterBalanceDayViewModel;
        public WaterBalance.ViewModel WaterBalanceDayViewModel
        {
            get => _waterBalanceDayViewModel;
            set { _waterBalanceDayViewModel = value; RaisePropertyChanged(nameof(WaterBalanceDayViewModel)); }
        }
        private WaterBalance.ViewModel _waterBalancePeriodViewModel;
        public WaterBalance.ViewModel WaterBalancePeriodViewModel
        {
            get => _waterBalancePeriodViewModel;
            set { _waterBalancePeriodViewModel = value; RaisePropertyChanged(nameof(WaterBalancePeriodViewModel)); }
        }
        private WaterBalance.ViewModel _waterBalanceYearViewModel;
        public WaterBalance.ViewModel WaterBalanceYearViewModel
        {
            get => _waterBalanceYearViewModel;
            set { _waterBalanceYearViewModel = value; RaisePropertyChanged(nameof(WaterBalanceYearViewModel)); }
        }



        public Pis.ViewModel PisViewModel { get; set; }
        public MatrixIn.ViewModel MatrixOneInViewModel { get; set; }
        public MatrixIn.ViewModel MatrixTwoInViewModel { get; set; }
        public MatrixOneOut.ViewModel MatrixOneOutViewModel { get; set; }
        public MatrixOneOut.ViewModel MatrixTwoOutViewModel { get; set; }


        //public EasyCalcModel Model { get; set; }

        public EasyCalcModel Model => new EasyCalcModel()
        {
            StartModel = StartViewModel.Model,
            SysInputModel = SysInputViewModel.Model,
            BilledConsModel = BilledConsViewModel.Model,
            UnbilledConsModel = UnbConsViewModel.Model,
            UnauthConsModel = UnauthConsViewModel.Model,
            MetErrorsModel = MeterErrorsViewModel.Model,
            NetworkModel = NetworkViewModel.Model,
            PressureModel = PressureViewModel.Model,
            IntermModel = IntermittentSupplyViewModel.Model,
            FinancDataModel = FinancialDataViewModel.Model,
            WaterBalanceDay = WaterBalanceDayViewModel.Model,
            WaterBalancePeriod = WaterBalancePeriodViewModel.Model,
            WaterBalanceYear = WaterBalanceYearViewModel.Model,
            Pis = PisViewModel.Model,
            MatrixOneIn = MatrixOneInViewModel.Model,
            MatrixTwoIn = MatrixTwoInViewModel.Model,
            MatrixOneOut = MatrixOneOutViewModel.Model,
        };

        public ExcelViewModel(EasyCalcModel model)
        {
            if (model == null) return;
            Messenger.Default.Register<BaseSheetViewModel>(this, OnBaseSheetViewModelReceived);
            
            StartViewModel = new Start.ViewModel(model.StartModel);
            SysInputViewModel = new SysInput.ViewModel(model.SysInputModel);
            BilledConsViewModel = new BilledCons.ViewModel(model.BilledConsModel);
            UnbConsViewModel = new UnbCons.ViewModel(model.UnbilledConsModel);
            UnauthConsViewModel = new UnauthCons.ViewModel(model.UnauthConsModel);
            MeterErrorsViewModel = new MeterErrors.ViewModel(model.MetErrorsModel);
            NetworkViewModel = new Network.ViewModel(model.NetworkModel);
            PressureViewModel = new Pressure.ViewModel(model.PressureModel);
            IntermittentSupplyViewModel = new IntermittentSupply.ViewModel(model.IntermModel);
            FinancialDataViewModel = new FinancialData.ViewModel(model.FinancDataModel);
            WaterBalanceDayViewModel = new WaterBalance.ViewModel(model.WaterBalanceDay);
            WaterBalancePeriodViewModel = new WaterBalance.ViewModel(model.WaterBalancePeriod);
            WaterBalanceYearViewModel = new WaterBalance.ViewModel(model.WaterBalanceYear);
            PisViewModel = new Pis.ViewModel(model.Pis);
            MatrixOneInViewModel = new MatrixIn.ViewModel(model.MatrixOneIn);
            MatrixTwoInViewModel = new MatrixIn.ViewModel(model.MatrixTwoIn);
            MatrixOneOutViewModel = new MatrixOneOut.ViewModel(model.MatrixOneOut);
            MatrixTwoOutViewModel = new MatrixOneOut.ViewModel(model.MatrixTwoOut);

            BaseSheetViewModelCalculate();
        }
        public void Dispose()
        {
            Messenger.Default.Unregister(this);
        }

        private void OnBaseSheetViewModelReceived(BaseSheetViewModel sheet)
        {
            BaseSheetViewModelCalculate();
        }
        private void BaseSheetViewModelCalculate()
        {
            if (
                StartViewModel == null ||
                SysInputViewModel == null ||
                BilledConsViewModel == null ||
                UnbConsViewModel == null ||
                UnauthConsViewModel == null ||
                MeterErrorsViewModel == null ||
                NetworkViewModel == null ||
                PressureViewModel == null ||
                IntermittentSupplyViewModel == null ||
                FinancialDataViewModel == null ||
                WaterBalanceDayViewModel == null ||
                WaterBalancePeriodViewModel == null ||
                WaterBalanceYearViewModel == null ||
                PisViewModel == null ||
                MatrixOneInViewModel == null ||
                MatrixTwoInViewModel == null ||
                MatrixOneOutViewModel == null ||
                MatrixTwoOutViewModel == null
                )
            {
                return;
            }

            var easyCalcModel = Model;

            WbEasyCalc.CalculateNew(easyCalcModel);

            //StartViewModel.Refreash(easyCalcModel);
            SysInputViewModel.Refreash(easyCalcModel.SysInputModel);
            BilledConsViewModel.Refreash(easyCalcModel.BilledConsModel);
            UnbConsViewModel.Refreash(easyCalcModel.UnbilledConsModel);
            UnauthConsViewModel.Refreash(easyCalcModel.UnauthConsModel);
            MeterErrorsViewModel.Refreash(easyCalcModel.MetErrorsModel);
            NetworkViewModel.Refreash(easyCalcModel.NetworkModel);
            PressureViewModel.Refreash(easyCalcModel.PressureModel);
            IntermittentSupplyViewModel.Refreash(easyCalcModel.IntermModel);
            FinancialDataViewModel.Refreash(easyCalcModel.FinancDataModel);
            WaterBalanceDayViewModel.Refreash(easyCalcModel.WaterBalanceDay);
            WaterBalancePeriodViewModel.Refreash(easyCalcModel.WaterBalancePeriod);
            WaterBalanceYearViewModel.Refreash(easyCalcModel.WaterBalanceYear);
            PisViewModel.Refreash(easyCalcModel.Pis);
            MatrixOneOutViewModel.Refreash(easyCalcModel.MatrixOneOut);
            MatrixTwoOutViewModel.Refreash(easyCalcModel.MatrixTwoOut);
        }
    }
}
