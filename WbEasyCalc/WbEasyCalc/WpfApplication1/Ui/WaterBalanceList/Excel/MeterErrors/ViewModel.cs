using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Ui.WbEasyCalcData.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.MeterErrors
{
    public class ViewModel : BaseSheetViewModel
    {
        #region Input props

        private string _metErrors_Desc_D12;
        public string MetErrors_Desc_D12
        {
            get => _metErrors_Desc_D12;
            set { _metErrors_Desc_D12 = value; RaisePropertyChanged(nameof(MetErrors_Desc_D12)); }
        }
        private string _metErrors_Desc_D13;
        public string MetErrors_Desc_D13
        {
            get => _metErrors_Desc_D13;
            set { _metErrors_Desc_D13 = value; RaisePropertyChanged(nameof(MetErrors_Desc_D13)); }
        }
        private string _metErrors_Desc_D14;
        public string MetErrors_Desc_D14
        {
            get => _metErrors_Desc_D14;
            set { _metErrors_Desc_D14 = value; RaisePropertyChanged(nameof(MetErrors_Desc_D14)); }
        }
        private string _metErrors_Desc_D15;
        public string MetErrors_Desc_D15
        {
            get => _metErrors_Desc_D15;
            set { _metErrors_Desc_D15 = value; RaisePropertyChanged(nameof(MetErrors_Desc_D15)); }
        }



        private int _metErrorsDetailedManualSpecJ6;
        private double _metErrorsBilledMetConsWoBulkSupMetUndrregH8;
        private double _metErrorsBilledMetConsWoBulkSupErrorMarginN8;
        private double _metErrorsMetBulkSupExpMetUnderregH32;
        private double _metErrorsUnbillMetConsWoBulkSupplMetUndrregH34;
        private double _metErrorsCorruptMetReadPractMetUndrregH38;
        public int MetErrors_DetailedManualSpec_J6
        {
            get => _metErrorsDetailedManualSpecJ6;
            set { _metErrorsDetailedManualSpecJ6 = value; RaisePropertyChanged(nameof(MetErrors_DetailedManualSpec_J6)); Calculate(); }
        }

        public double MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8
        {
            get => _metErrorsBilledMetConsWoBulkSupMetUndrregH8;
            set { _metErrorsBilledMetConsWoBulkSupMetUndrregH8 = value; RaisePropertyChanged(nameof(MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8)); Calculate(); }
        }

        public double MetErrors_BilledMetConsWoBulkSupErrorMargin_N8
        {
            get => _metErrorsBilledMetConsWoBulkSupErrorMarginN8;
            set { _metErrorsBilledMetConsWoBulkSupErrorMarginN8 = value; RaisePropertyChanged(nameof(MetErrors_BilledMetConsWoBulkSupErrorMargin_N8)); Calculate(); }
        }
        public double MetErrors_MetBulkSupExpMetUnderreg_H32
        {
            get => _metErrorsMetBulkSupExpMetUnderregH32;
            set { _metErrorsMetBulkSupExpMetUnderregH32 = value; RaisePropertyChanged(nameof(MetErrors_MetBulkSupExpMetUnderreg_H32)); Calculate(); }
        }

        public double MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34
        {
            get => _metErrorsUnbillMetConsWoBulkSupplMetUndrregH34;
            set { _metErrorsUnbillMetConsWoBulkSupplMetUndrregH34 = value; RaisePropertyChanged(nameof(MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34)); Calculate(); }
        }

        public double MetErrors_CorruptMetReadPractMetUndrreg_H38
        {
            get => _metErrorsCorruptMetReadPractMetUndrregH38;
            set { _metErrorsCorruptMetReadPractMetUndrregH38 = value; RaisePropertyChanged(nameof(MetErrors_CorruptMetReadPractMetUndrreg_H38)); Calculate(); }
        }



        private double _metErrors_Total_F12;
        public double MetErrors_Total_F12
        {
            get => _metErrors_Total_F12;
            set { _metErrors_Total_F12 = value; RaisePropertyChanged(nameof(MetErrors_Total_F12)); Calculate(); }
        }
        private double _metErrors_Total_F13;
        public double MetErrors_Total_F13
        {
            get => _metErrors_Total_F13;
            set { _metErrors_Total_F13 = value; RaisePropertyChanged(nameof(MetErrors_Total_F13)); Calculate(); }
        }
        private double _metErrors_Total_F14;
        public double MetErrors_Total_F14
        {
            get => _metErrors_Total_F14;
            set { _metErrors_Total_F14 = value; RaisePropertyChanged(nameof(MetErrors_Total_F14)); Calculate(); }
        }
        private double _metErrors_Total_F15;
        public double MetErrors_Total_F15
        {
            get => _metErrors_Total_F15;
            set { _metErrors_Total_F15 = value; RaisePropertyChanged(nameof(MetErrors_Total_F15)); Calculate(); }
        }

        private double _metErrors_Meter_H12;
        public double MetErrors_Meter_H12
        {
            get => _metErrors_Meter_H12;
            set { _metErrors_Meter_H12 = value; RaisePropertyChanged(nameof(MetErrors_Meter_H12)); Calculate(); }
        }
        private double _metErrors_Meter_H13;
        public double MetErrors_Meter_H13
        {
            get => _metErrors_Meter_H13;
            set { _metErrors_Meter_H13 = value; RaisePropertyChanged(nameof(MetErrors_Meter_H13)); Calculate(); }
        }
        private double _metErrors_Meter_H14;
        public double MetErrors_Meter_H14
        {
            get => _metErrors_Meter_H14;
            set { _metErrors_Meter_H14 = value; RaisePropertyChanged(nameof(MetErrors_Meter_H14)); Calculate(); }
        }
        private double _metErrors_Meter_H15;
        public double MetErrors_Meter_H15
        {
            get => _metErrors_Meter_H15;
            set { _metErrors_Meter_H15 = value; RaisePropertyChanged(nameof(MetErrors_Meter_H15)); Calculate(); }
        }

        private double _metErrors_Error_N12;
        public double MetErrors_Error_N12
        {
            get => _metErrors_Error_N12;
            set { _metErrors_Error_N12 = value; RaisePropertyChanged(nameof(MetErrors_Error_N12)); Calculate(); }
        }
        private double _metErrors_Error_N13;
        public double MetErrors_Error_N13
        {
            get => _metErrors_Error_N13;
            set { _metErrors_Error_N13 = value; RaisePropertyChanged(nameof(MetErrors_Error_N13)); Calculate(); }
        }
        private double _metErrors_Error_N14;
        public double MetErrors_Error_N14
        {
            get => _metErrors_Error_N14;
            set { _metErrors_Error_N14 = value; RaisePropertyChanged(nameof(MetErrors_Error_N14)); Calculate(); }
        }
        private double _metErrors_Error_N15;
        public double MetErrors_Error_N15
        {
            get => _metErrors_Error_N15;
            set { _metErrors_Error_N15 = value; RaisePropertyChanged(nameof(MetErrors_Error_N15)); Calculate(); }
        }

        private double _meteredBulkSupplyExportErrorMargin_N32;
        public double MeteredBulkSupplyExportErrorMargin_N32
        {
            get => _meteredBulkSupplyExportErrorMargin_N32;
            set { _meteredBulkSupplyExportErrorMargin_N32 = value; RaisePropertyChanged(nameof(MeteredBulkSupplyExportErrorMargin_N32)); Calculate(); }
        }

        private double _unbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34;
        public double UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34
        {
            get => _unbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34;
            set { _unbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = value; RaisePropertyChanged(nameof(UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34)); Calculate(); }
        }
        private double _corruptMeterReadingPracticessErrorMargin_N38;
        public double CorruptMeterReadingPracticessErrorMargin_N38
        {
            get => _corruptMeterReadingPracticessErrorMargin_N38;
            set { _corruptMeterReadingPracticessErrorMargin_N38 = value; RaisePropertyChanged(nameof(CorruptMeterReadingPracticessErrorMargin_N38)); Calculate(); }
        }
        private double _dataHandlingErrorsOffice_L40;
        public double DataHandlingErrorsOffice_L40
        {
            get => _dataHandlingErrorsOffice_L40;
            set { _dataHandlingErrorsOffice_L40 = value; RaisePropertyChanged(nameof(DataHandlingErrorsOffice_L40)); Calculate(); }
        }
        private double _dataHandlingErrorsOfficeErrorMargin_N40;
        public double DataHandlingErrorsOfficeErrorMargin_N40
        {
            get => _dataHandlingErrorsOfficeErrorMargin_N40;
            set { _dataHandlingErrorsOfficeErrorMargin_N40 = value; RaisePropertyChanged(nameof(DataHandlingErrorsOfficeErrorMargin_N40)); Calculate(); }
        }


        #endregion

        #region Output props

        private double _metErrors_Total_F8;
        public double MetErrors_Total_F8
        {
            get => _metErrors_Total_F8;
            set { _metErrors_Total_F8 = value; RaisePropertyChanged(nameof(MetErrors_Total_F8)); }
        }
        private double _metErrors_Total_F32;
        public double MetErrors_Total_F32
        {
            get => _metErrors_Total_F32;
            set { _metErrors_Total_F32 = value; RaisePropertyChanged(nameof(MetErrors_Total_F32)); }
        }
        private double _metErrors_Total_F34;
        public double MetErrors_Total_F34
        {
            get => _metErrors_Total_F34;
            set { _metErrors_Total_F34 = value; RaisePropertyChanged(nameof(MetErrors_Total_F34)); }
        }
        private double _metErrors_Total_F38;
        public double MetErrors_Total_F38
        {
            get => _metErrors_Total_F38;
            set { _metErrors_Total_F38 = value; RaisePropertyChanged(nameof(MetErrors_Total_F38)); }
        }

        private double _metErrors_Total_L8;
        public double MetErrors_Total_L8
        {
            get => _metErrors_Total_L8;
            set { _metErrors_Total_L8 = value; RaisePropertyChanged(nameof(MetErrors_Total_L8)); }
        }
        private double _metErrors_Total_L32;
        public double MetErrors_Total_L32
        {
            get => _metErrors_Total_L32;
            set { _metErrors_Total_L32 = value; RaisePropertyChanged(nameof(MetErrors_Total_L32)); }
        }
        private double _metErrors_Total_L34;
        public double MetErrors_Total_L34
        {
            get => _metErrors_Total_L34;
            set { _metErrors_Total_L34 = value; RaisePropertyChanged(nameof(MetErrors_Total_L34)); }
        }
        private double _metErrors_Total_L38;
        public double MetErrors_Total_L38
        {
            get => _metErrors_Total_L38;
            set { _metErrors_Total_L38 = value; RaisePropertyChanged(nameof(MetErrors_Total_L38)); }
        }

        private double _metErrors_ErrorMarg_N42;
        public double MetErrors_ErrorMarg_N42
        {
            get => _metErrors_ErrorMarg_N42;
            set { _metErrors_ErrorMarg_N42 = value; RaisePropertyChanged(nameof(MetErrors_ErrorMarg_N42)); }
        }
        private double _metErrors_Min_L45;
        public double MetErrors_Min_L45
        {
            get => _metErrors_Min_L45;
            set { _metErrors_Min_L45 = value; RaisePropertyChanged(nameof(MetErrors_Min_L45)); }
        }
        private double _metErrors_Max_L47;
        public double MetErrors_Max_L47
        {
            get => _metErrors_Max_L47;
            set { _metErrors_Max_L47 = value; RaisePropertyChanged(nameof(MetErrors_Max_L47)); }
        }
        private double _metErrors_BestEstimate_L49;
        public double MetErrors_BestEstimate_L49
        {
            get => _metErrors_BestEstimate_L49;
            set { _metErrors_BestEstimate_L49 = value; RaisePropertyChanged(nameof(MetErrors_BestEstimate_L49)); }
        }



        private double _metErrors_Total_L12;
        public double MetErrors_Total_L12
        {
            get => _metErrors_Total_L12;
            set { _metErrors_Total_L12 = value; RaisePropertyChanged(nameof(MetErrors_Total_L12)); }
        }
        private double _metErrors_Total_L13;
        public double MetErrors_Total_L13
        {
            get => _metErrors_Total_L13;
            set { _metErrors_Total_L13 = value; RaisePropertyChanged(nameof(MetErrors_Total_L13)); }
        }
        private double _metErrors_Total_L14;
        public double MetErrors_Total_L14
        {
            get => _metErrors_Total_L14;
            set { _metErrors_Total_L14 = value; RaisePropertyChanged(nameof(MetErrors_Total_L14)); }
        }
        private double _metErrors_Total_L15;
        public double MetErrors_Total_L15
        {
            get => _metErrors_Total_L15;
            set { _metErrors_Total_L15 = value; RaisePropertyChanged(nameof(MetErrors_Total_L15)); }
        }

        #endregion

        public MetErrorsModel Model => new MetErrorsModel()
        {
            MetErrors_Desc_D12 = MetErrors_Desc_D12,
            MetErrors_Desc_D13 = MetErrors_Desc_D13,
            MetErrors_Desc_D14 = MetErrors_Desc_D14,
            MetErrors_Desc_D15 = MetErrors_Desc_D15,
            MetErrors_DetailedManualSpec_J6 = MetErrors_DetailedManualSpec_J6,
            MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 = MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8,
            MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 = MetErrors_BilledMetConsWoBulkSupErrorMargin_N8,

            MetErrors_Total_F12 = MetErrors_Total_F12,
            MetErrors_Total_F13 = MetErrors_Total_F13,
            MetErrors_Total_F14 = MetErrors_Total_F14,
            MetErrors_Total_F15 = MetErrors_Total_F15,
            MetErrors_Meter_H12 = MetErrors_Meter_H12,
            MetErrors_Meter_H13 = MetErrors_Meter_H13,
            MetErrors_Meter_H14 = MetErrors_Meter_H14,
            MetErrors_Meter_H15 = MetErrors_Meter_H15,
            MetErrors_Error_N12 = MetErrors_Error_N12,
            MetErrors_Error_N13 = MetErrors_Error_N13,
            MetErrors_Error_N14 = MetErrors_Error_N14,
            MetErrors_Error_N15 = MetErrors_Error_N15,

            MeteredBulkSupplyExportErrorMargin_N32 = MeteredBulkSupplyExportErrorMargin_N32,
            UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34,
            CorruptMeterReadingPracticessErrorMargin_N38 = CorruptMeterReadingPracticessErrorMargin_N38,
            DataHandlingErrorsOffice_L40 = DataHandlingErrorsOffice_L40,
            DataHandlingErrorsOfficeErrorMargin_N40 = DataHandlingErrorsOfficeErrorMargin_N40,

            MetErrors_MetBulkSupExpMetUnderreg_H32 = MetErrors_MetBulkSupExpMetUnderreg_H32,
            MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34 = MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34,
            MetErrors_CorruptMetReadPractMetUndrreg_H38 = MetErrors_CorruptMetReadPractMetUndrreg_H38,

            // Output
            MetErrors_Total_F8 = MetErrors_Total_F8,
            MetErrors_Total_F32 = MetErrors_Total_F32,
            MetErrors_Total_F34 = MetErrors_Total_F34,
            MetErrors_Total_F38 = MetErrors_Total_F38,
            MetErrors_Total_L8 = MetErrors_Total_L8,
            MetErrors_Total_L32 = MetErrors_Total_L32,
            MetErrors_Total_L34 = MetErrors_Total_L34,
            MetErrors_Total_L38 = MetErrors_Total_L38,
            MetErrors_ErrorMarg_N42 = MetErrors_ErrorMarg_N42,
            MetErrors_Min_L45 = MetErrors_Min_L45,
            MetErrors_Max_L47 = MetErrors_Max_L47,
            MetErrors_BestEstimate_L49 = MetErrors_BestEstimate_L49,

            MetErrors_Total_L12 = MetErrors_Total_L12,
            MetErrors_Total_L13 = MetErrors_Total_L13,
            MetErrors_Total_L14 = MetErrors_Total_L14,
            MetErrors_Total_L15 = MetErrors_Total_L15,

        };

        internal void Refreash(MetErrorsModel model)
        {
            MetErrors_Total_F8 = model.MetErrors_Total_F8;
            MetErrors_Total_F32 = model.MetErrors_Total_F32;
            MetErrors_Total_F34 = model.MetErrors_Total_F34;
            MetErrors_Total_F38 = model.MetErrors_Total_F38;
            MetErrors_Total_L8 = model.MetErrors_Total_L8;
            MetErrors_Total_L32 = model.MetErrors_Total_L32;
            MetErrors_Total_L34 = model.MetErrors_Total_L34;
            MetErrors_Total_L38 = model.MetErrors_Total_L38;
            MetErrors_ErrorMarg_N42 = model.MetErrors_ErrorMarg_N42;
            MetErrors_Min_L45 = model.MetErrors_Min_L45;
            MetErrors_Max_L47 = model.MetErrors_Max_L47;
            MetErrors_BestEstimate_L49 = model.MetErrors_BestEstimate_L49;
            MetErrors_Total_L12 = model.MetErrors_Total_L12;
            MetErrors_Total_L13 = model.MetErrors_Total_L13;
            MetErrors_Total_L14 = model.MetErrors_Total_L14;
            MetErrors_Total_L15 = model.MetErrors_Total_L15;
        }

        public ViewModel(MetErrorsModel model)
        {
            if (model == null) return;

            // Input
            MetErrors_Desc_D12 = model.MetErrors_Desc_D12;
            MetErrors_Desc_D13 = model.MetErrors_Desc_D13;
            MetErrors_Desc_D14 = model.MetErrors_Desc_D14;
            MetErrors_Desc_D15 = model.MetErrors_Desc_D15;
            MetErrors_DetailedManualSpec_J6 = model.MetErrors_DetailedManualSpec_J6;
            MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 = model.MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8;
            MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 = model.MetErrors_BilledMetConsWoBulkSupErrorMargin_N8;

            MetErrors_Total_F12 = model.MetErrors_Total_F12;
            MetErrors_Total_F13 = model.MetErrors_Total_F13;
            MetErrors_Total_F14 = model.MetErrors_Total_F14;
            MetErrors_Total_F15 = model.MetErrors_Total_F15;
            MetErrors_Meter_H12 = model.MetErrors_Meter_H12;
            MetErrors_Meter_H13 = model.MetErrors_Meter_H13;
            MetErrors_Meter_H14 = model.MetErrors_Meter_H14;
            MetErrors_Meter_H15 = model.MetErrors_Meter_H15;
            MetErrors_Error_N12 = model.MetErrors_Error_N12;
            MetErrors_Error_N13 = model.MetErrors_Error_N13;
            MetErrors_Error_N14 = model.MetErrors_Error_N14;
            MetErrors_Error_N15 = model.MetErrors_Error_N15;

            MeteredBulkSupplyExportErrorMargin_N32 = model.MeteredBulkSupplyExportErrorMargin_N32;
            UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = model.UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34;
            CorruptMeterReadingPracticessErrorMargin_N38 = model.CorruptMeterReadingPracticessErrorMargin_N38;
            DataHandlingErrorsOffice_L40 = model.DataHandlingErrorsOffice_L40;
            DataHandlingErrorsOfficeErrorMargin_N40 = model.DataHandlingErrorsOfficeErrorMargin_N40;

            MetErrors_MetBulkSupExpMetUnderreg_H32 = model.MetErrors_MetBulkSupExpMetUnderreg_H32;
            MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34 = model.MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34;
            MetErrors_CorruptMetReadPractMetUndrreg_H38 = model.MetErrors_CorruptMetReadPractMetUndrreg_H38;
        }
    }
}
