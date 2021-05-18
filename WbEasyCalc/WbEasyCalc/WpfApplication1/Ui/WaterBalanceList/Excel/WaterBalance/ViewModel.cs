using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.WaterBalance
{
    public class ViewModel : BaseSheetViewModel
    {
        #region Props output

        private double _systemInputVolume_B19;
        private double _systemInputVolumeErrorMargin_B21;
        private double _authorizedConsumption_K12;
        private double _authorizedConsumptionErrorMargin_K15;
        private double _waterLosses_K29;
        private double _waterLossesErrorMargin_K31;
        private double _billedAuthorizedConsumption_T8;
        private double _unbilledAuthorizedConsumption_T16;
        private double _unbilledAuthorizedConsumptionErrorMargin_T20;
        private double _commercialLosses_T26;
        private double _commercialLossesErrorMargin_T29;
        private double _physicalLossesM3T34;
        private double _physcialLossesErrorMarginAh35;
        private double _billedMeteredConsumptionAc4;
        private double _billedUnmeteredConsumptionAc9;
        private double _unbilledMeteredConsumptionAc14;
        private double _unbilledUnmeteredConsumptionAc19;
        private double _unbilledUnmeteredConsumptionErrorMarginAo20;
        private double _unauthorizedConsumptionAc24;
        private double _unauthorizedConsumptionErrorMarginAo25;
        private double _customerMeterInaccuraciesAndErrorsM3Ac29;
        private double _customerMeterInaccuraciesAndErrorsErrorMarginAo30;
        private double _revenueWaterM3Ay8;
        private double _nonRevenueWaterM3Ay24;
        private double _nonRevenueWaterErrorMarginAy26;

        private double _periodDays_AF1;
        public double PeriodDays_AF1
        {
            get { return _periodDays_AF1; }
            set { _periodDays_AF1 = value; RaisePropertyChanged(nameof(PeriodDays_AF1)); }
        }
        public double SystemInputVolume_B19
        {
            get { return _systemInputVolume_B19; }
            set { _systemInputVolume_B19 = value; RaisePropertyChanged(nameof(SystemInputVolume_B19)); }
        }

        public double SystemInputVolumeErrorMargin_B21
        {
            get { return _systemInputVolumeErrorMargin_B21; }
            set { _systemInputVolumeErrorMargin_B21 = value; RaisePropertyChanged(nameof(SystemInputVolumeErrorMargin_B21)); }
        }

        public double AuthorizedConsumption_K12
        {
            get => _authorizedConsumption_K12;
            set { _authorizedConsumption_K12 = value; RaisePropertyChanged(nameof(AuthorizedConsumption_K12)); }
        }
        public double AuthorizedConsumptionErrorMargin_K15
        {
            get => _authorizedConsumptionErrorMargin_K15;
            set { _authorizedConsumptionErrorMargin_K15 = value; RaisePropertyChanged(nameof(AuthorizedConsumptionErrorMargin_K15)); }
        }

        public double WaterLosses_K29
        {
            get => _waterLosses_K29;
            set { _waterLosses_K29 = value; RaisePropertyChanged(nameof(WaterLosses_K29)); }
        }

        public double WaterLossesErrorMargin_K31
        {
            get => _waterLossesErrorMargin_K31;
            set { _waterLossesErrorMargin_K31 = value; RaisePropertyChanged(nameof(WaterLossesErrorMargin_K31)); }
        }

        public double BilledAuthorizedConsumption_T8
        {
            get => _billedAuthorizedConsumption_T8;
            set { _billedAuthorizedConsumption_T8 = value; RaisePropertyChanged(nameof(BilledAuthorizedConsumption_T8)); }
        }

        public double UnbilledAuthorizedConsumption_T16
        {
            get => _unbilledAuthorizedConsumption_T16;
            set { _unbilledAuthorizedConsumption_T16 = value; RaisePropertyChanged(nameof(UnbilledAuthorizedConsumption_T16)); }
        }

        public double UnbilledAuthorizedConsumptionErrorMargin_T20
        {
            get => _unbilledAuthorizedConsumptionErrorMargin_T20;
            set { _unbilledAuthorizedConsumptionErrorMargin_T20 = value; RaisePropertyChanged(nameof(UnbilledAuthorizedConsumptionErrorMargin_T20)); }
        }

        public double CommercialLosses_T26
        {
            get => _commercialLosses_T26;
            set { _commercialLosses_T26 = value; RaisePropertyChanged(nameof(CommercialLosses_T26)); }
        }

        public double CommercialLossesErrorMargin_T29
        {
            get => _commercialLossesErrorMargin_T29;
            set { _commercialLossesErrorMargin_T29 = value; RaisePropertyChanged(nameof(CommercialLossesErrorMargin_T29)); }
        }

        public double PhysicalLossesM3_T34
        {
            get => _physicalLossesM3T34;
            set { _physicalLossesM3T34 = value; RaisePropertyChanged(nameof(PhysicalLossesM3_T34)); }
        }

        public double PhyscialLossesErrorMargin_AH35
        {
            get => _physcialLossesErrorMarginAh35;
            set { _physcialLossesErrorMarginAh35 = value; RaisePropertyChanged(nameof(PhyscialLossesErrorMargin_AH35)); }
        }

        public double BilledMeteredConsumption_AC4
        {
            get => _billedMeteredConsumptionAc4;
            set { _billedMeteredConsumptionAc4 = value; RaisePropertyChanged(nameof(BilledMeteredConsumption_AC4)); }
        }

        public double BilledUnmeteredConsumption_AC9
        {
            get => _billedUnmeteredConsumptionAc9;
            set { _billedUnmeteredConsumptionAc9 = value; RaisePropertyChanged(nameof(BilledUnmeteredConsumption_AC9)); }
        }

        public double UnbilledMeteredConsumption_AC14
        {
            get => _unbilledMeteredConsumptionAc14;
            set { _unbilledMeteredConsumptionAc14 = value; RaisePropertyChanged(nameof(UnbilledMeteredConsumption_AC14)); }
        }

        public double UnbilledUnmeteredConsumption_AC19
        {
            get => _unbilledUnmeteredConsumptionAc19;
            set { _unbilledUnmeteredConsumptionAc19 = value; RaisePropertyChanged(nameof(UnbilledUnmeteredConsumption_AC19)); }
        }

        public double UnbilledUnmeteredConsumptionErrorMargin_AO20
        {
            get => _unbilledUnmeteredConsumptionErrorMarginAo20;
            set { _unbilledUnmeteredConsumptionErrorMarginAo20 = value; RaisePropertyChanged(nameof(UnbilledUnmeteredConsumptionErrorMargin_AO20)); }
        }

        public double UnauthorizedConsumption_AC24
        {
            get => _unauthorizedConsumptionAc24;
            set { _unauthorizedConsumptionAc24 = value; RaisePropertyChanged(nameof(UnauthorizedConsumption_AC24)); }
        }

        public double UnauthorizedConsumptionErrorMargin_AO25
        {
            get => _unauthorizedConsumptionErrorMarginAo25;
            set { _unauthorizedConsumptionErrorMarginAo25 = value; RaisePropertyChanged(nameof(UnauthorizedConsumptionErrorMargin_AO25)); }
        }

        public double CustomerMeterInaccuraciesAndErrorsM3_AC29
        {
            get => _customerMeterInaccuraciesAndErrorsM3Ac29;
            set { _customerMeterInaccuraciesAndErrorsM3Ac29 = value; RaisePropertyChanged(nameof(CustomerMeterInaccuraciesAndErrorsM3_AC29)); }
        }

        public double CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30
        {
            get => _customerMeterInaccuraciesAndErrorsErrorMarginAo30;
            set { _customerMeterInaccuraciesAndErrorsErrorMarginAo30 = value; RaisePropertyChanged(nameof(CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30)); }
        }

        public double RevenueWaterM3_AY8
        {
            get => _revenueWaterM3Ay8;
            set { _revenueWaterM3Ay8 = value; RaisePropertyChanged(nameof(RevenueWaterM3_AY8)); }
        }

        public double NonRevenueWaterM3_AY24
        {
            get => _nonRevenueWaterM3Ay24;
            set { _nonRevenueWaterM3Ay24 = value; RaisePropertyChanged(nameof(NonRevenueWaterM3_AY24)); }
        }

        public double NonRevenueWaterErrorMargin_AY26
        {
            get => _nonRevenueWaterErrorMarginAy26;
            set { _nonRevenueWaterErrorMarginAy26 = value; RaisePropertyChanged(nameof(NonRevenueWaterErrorMargin_AY26)); }
        }
        #endregion

        public WaterBalanceModel Model => new WaterBalanceModel()
        {
            PeriodDays_AF1 = PeriodDays_AF1,
            SystemInputVolume_B19 = SystemInputVolume_B19,
            SystemInputVolumeErrorMargin_B21 = SystemInputVolumeErrorMargin_B21,
            AuthorizedConsumption_K12 = AuthorizedConsumption_K12,
            AuthorizedConsumptionErrorMargin_K15 = AuthorizedConsumptionErrorMargin_K15,
            WaterLosses_K29 = WaterLosses_K29,
            WaterLossesErrorMargin_K31 = WaterLossesErrorMargin_K31,
            BilledAuthorizedConsumption_T8 = BilledAuthorizedConsumption_T8,
            UnbilledAuthorizedConsumption_T16 = UnbilledAuthorizedConsumption_T16,
            UnbilledAuthorizedConsumptionErrorMargin_T20 = UnbilledAuthorizedConsumptionErrorMargin_T20,
            CommercialLosses_T26 = CommercialLosses_T26,
            CommercialLossesErrorMargin_T29 = CommercialLossesErrorMargin_T29,
            PhysicalLossesM3_T34 = PhysicalLossesM3_T34,
            PhyscialLossesErrorMargin_AH35 = PhyscialLossesErrorMargin_AH35,
            BilledMeteredConsumption_AC4 = BilledMeteredConsumption_AC4,
            BilledUnmeteredConsumption_AC9 = BilledUnmeteredConsumption_AC9,
            UnbilledMeteredConsumption_AC14 = UnbilledMeteredConsumption_AC14,
            UnbilledUnmeteredConsumption_AC19 = UnbilledUnmeteredConsumption_AC19,
            UnbilledUnmeteredConsumptionErrorMargin_AO20 = UnbilledUnmeteredConsumptionErrorMargin_AO20,
            UnauthorizedConsumption_AC24 = UnauthorizedConsumption_AC24,
            UnauthorizedConsumptionErrorMargin_AO25 = UnauthorizedConsumptionErrorMargin_AO25,
            CustomerMeterInaccuraciesAndErrorsM3_AC29 = CustomerMeterInaccuraciesAndErrorsM3_AC29,
            CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30 = CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30,
            RevenueWaterM3_AY8 = RevenueWaterM3_AY8,
            NonRevenueWaterM3_AY24 = NonRevenueWaterM3_AY24,
            NonRevenueWaterErrorMargin_AY26 = NonRevenueWaterErrorMargin_AY26,
        };

        internal void Refreash(WaterBalanceModel model)
        {
            PeriodDays_AF1 = model.PeriodDays_AF1;
            SystemInputVolume_B19 = model.SystemInputVolume_B19;
            SystemInputVolumeErrorMargin_B21 = model.SystemInputVolumeErrorMargin_B21;
            AuthorizedConsumption_K12 = model.AuthorizedConsumption_K12;
            AuthorizedConsumptionErrorMargin_K15 = model.AuthorizedConsumptionErrorMargin_K15;
            WaterLosses_K29 = model.WaterLosses_K29;
            WaterLossesErrorMargin_K31 = model.WaterLossesErrorMargin_K31;
            BilledAuthorizedConsumption_T8 = model.BilledAuthorizedConsumption_T8;
            UnbilledAuthorizedConsumption_T16 = model.UnbilledAuthorizedConsumption_T16;
            UnbilledAuthorizedConsumptionErrorMargin_T20 = model.UnbilledAuthorizedConsumptionErrorMargin_T20;
            CommercialLosses_T26 = model.CommercialLosses_T26;
            CommercialLossesErrorMargin_T29 = model.CommercialLossesErrorMargin_T29;
            PhysicalLossesM3_T34 = model.PhysicalLossesM3_T34;
            PhyscialLossesErrorMargin_AH35 = model.PhyscialLossesErrorMargin_AH35;
            BilledMeteredConsumption_AC4 = model.BilledMeteredConsumption_AC4;
            BilledUnmeteredConsumption_AC9 = model.BilledUnmeteredConsumption_AC9;
            UnbilledMeteredConsumption_AC14 = model.UnbilledMeteredConsumption_AC14;
            UnbilledUnmeteredConsumption_AC19 = model.UnbilledUnmeteredConsumption_AC19;
            UnbilledUnmeteredConsumptionErrorMargin_AO20 = model.UnbilledUnmeteredConsumptionErrorMargin_AO20;
            UnauthorizedConsumption_AC24 = model.UnauthorizedConsumption_AC24;
            UnauthorizedConsumptionErrorMargin_AO25 = model.UnauthorizedConsumptionErrorMargin_AO25;
            CustomerMeterInaccuraciesAndErrorsM3_AC29 = model.CustomerMeterInaccuraciesAndErrorsM3_AC29;
            CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30 = model.CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30;
            RevenueWaterM3_AY8 = model.RevenueWaterM3_AY8;
            NonRevenueWaterM3_AY24 = model.NonRevenueWaterM3_AY24;
            NonRevenueWaterErrorMargin_AY26 = model.NonRevenueWaterErrorMargin_AY26;
        }

        public ViewModel(WaterBalanceModel model)
        {
            if (model == null) return;
        }
    }
}