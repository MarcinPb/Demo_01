using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class MetErrorsModel : ICloneable
    {
        // Input
        public int MetErrors_DetailedManualSpec_J6 { get; set; }                     //
        public double MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 { get; set; }
        public double MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 { get; set; }
        public string MetErrors_Desc_D12 { get; set; }
        public string MetErrors_Desc_D13 { get; set; }
        public string MetErrors_Desc_D14 { get; set; }
        public string MetErrors_Desc_D15 { get; set; }
        public double MetErrors_Total_F12 { get; set; }
        public double MetErrors_Total_F13 { get; set; }
        public double MetErrors_Total_F14 { get; set; }
        public double MetErrors_Total_F15 { get; set; }
        public double MetErrors_Meter_H12 { get; set; }
        public double MetErrors_Meter_H13 { get; set; }
        public double MetErrors_Meter_H14 { get; set; }
        public double MetErrors_Meter_H15 { get; set; }
        public double MetErrors_Error_N12 { get; set; }
        public double MetErrors_Error_N13 { get; set; }
        public double MetErrors_Error_N14 { get; set; }
        public double MetErrors_Error_N15 { get; set; }
        public double MeteredBulkSupplyExportErrorMargin_N32 { get; set; }
        public double UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 { get; set; }
        public double CorruptMeterReadingPracticessErrorMargin_N38 { get; set; }
        public double DataHandlingErrorsOffice_L40 { get; set; }
        public double DataHandlingErrorsOfficeErrorMargin_N40 { get; set; }
        public double MetErrors_MetBulkSupExpMetUnderreg_H32 { get; set; }
        public double MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34 { get; set; }
        public double MetErrors_CorruptMetReadPractMetUndrreg_H38 { get; set; }
        // Output
        public double MetErrors_Total_F8 { get; set; }
        public double MetErrors_Total_F32 { get; set; }
        public double MetErrors_Total_F34 { get; set; }
        public double MetErrors_Total_F38 { get; set; }
        public double MetErrors_Total_L8 { get; set; }
        public double MetErrors_Total_L32 { get; set; }
        public double MetErrors_Total_L34 { get; set; }
        public double MetErrors_Total_L38 { get; set; }
        public double MetErrors_ErrorMarg_N42 { get; set; }
        public double MetErrors_Min_L45 { get; set; }
        public double MetErrors_Max_L47 { get; set; }
        public double MetErrors_BestEstimate_L49 { get; set; }
        public double MetErrors_Total_L12 { get; set; }
        public double MetErrors_Total_L13 { get; set; }
        public double MetErrors_Total_L14 { get; set; }
        public double MetErrors_Total_L15 { get; set; }

        public object Clone()
        {
            return new MetErrorsModel()
            {
                // Input
                MetErrors_DetailedManualSpec_J6 = MetErrors_DetailedManualSpec_J6,
                MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 = MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8,
                MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 = MetErrors_BilledMetConsWoBulkSupErrorMargin_N8,
                MetErrors_Desc_D12 = MetErrors_Desc_D12,
                MetErrors_Desc_D13 = MetErrors_Desc_D13,
                MetErrors_Desc_D14 = MetErrors_Desc_D14,
                MetErrors_Desc_D15 = MetErrors_Desc_D15,
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
        }
    }
}
