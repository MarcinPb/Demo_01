using System.Collections.Generic;
using System.Linq;
using WbEasyCalcModel;

namespace WbEasyCalcRepository.Calculation
{
    public class BilledConsumptionCalc
    {
        public static void Calculate(EasyCalcModel easyCalcModel)
        {
            easyCalcModel.BilledConsModel.BilledCons_Sum_D28 =
                easyCalcModel.BilledConsModel.BilledCons_BilledMetConsBulkWatSupExpM3_D6 +
                easyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D8 +
                easyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D9 +
                easyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D10 +
                easyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D11;
            easyCalcModel.BilledConsModel.BilledCons_Sum_H28 =
                easyCalcModel.BilledConsModel.BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 +
                easyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H8 +
                easyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H9 +
                easyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H10 +
                easyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H11;
        }
    }
}