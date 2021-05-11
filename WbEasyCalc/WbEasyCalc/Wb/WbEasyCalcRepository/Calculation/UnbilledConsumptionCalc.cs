using System;
using System.Collections.Generic;
using System.Linq;
using WbEasyCalcModel;

namespace WbEasyCalcRepository.Calculation
{
    public class UnbilledConsumptionCalc
    {
        public static void Calculate(EasyCalcModel easyCalcDataInput)
        {
            var UnbilledMeteredConsumption_D6_D23 = new List<double>
            {
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_MetConsBulkWatSupExpM3_D6,

                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D8,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D9,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D10,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D11,
            };
            var UnbilledUnmeteredConsumptionM3_H6_H23 = new List<double>
            {
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H6,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H7,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H8,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H9,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H10,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H11,
            };
            var UnbilledUnmeteredConsumptionError_J6_J23 = new List<double>
            {
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J6,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J7,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J8,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J9,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J10,
                easyCalcDataInput.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J11,
            };

            easyCalcDataInput.UnbilledConsModel.UnbilledCons_Sum_D32 = UnbilledMeteredConsumption_D6_D23.Sum();
            easyCalcDataInput.UnbilledConsModel.UnbilledCons_BestEstimate_H32 = UnbilledUnmeteredConsumptionM3_H6_H23.Sum();
            var ErrorFactor_O25 = Math.Sqrt(GetErrorFactorized(UnbilledUnmeteredConsumptionM3_H6_H23, UnbilledUnmeteredConsumptionError_J6_J23).Sum());
            easyCalcDataInput.UnbilledConsModel.UnbilledCons_ErrorMarg_J25 = GetUnbilledUnmeteredConsumptionErrorMargin_J25(easyCalcDataInput.UnbilledConsModel.UnbilledCons_BestEstimate_H32, ErrorFactor_O25);

            easyCalcDataInput.UnbilledConsModel.UnbilledCons_Min_H28 = easyCalcDataInput.UnbilledConsModel.UnbilledCons_BestEstimate_H32 == 0 ? 0 : easyCalcDataInput.UnbilledConsModel.UnbilledCons_BestEstimate_H32 * (1 - easyCalcDataInput.UnbilledConsModel.UnbilledCons_ErrorMarg_J25);
            easyCalcDataInput.UnbilledConsModel.UnbilledCons_Max_H30 = easyCalcDataInput.UnbilledConsModel.UnbilledCons_BestEstimate_H32 == 0 ? 0 : easyCalcDataInput.UnbilledConsModel.UnbilledCons_BestEstimate_H32 * (1 + easyCalcDataInput.UnbilledConsModel.UnbilledCons_ErrorMarg_J25);
        }

        private static double GetUnbilledUnmeteredConsumptionErrorMargin_J25(double UnbilledUnmeteredConsumption_H32, double ErrorFactor_O25)
        {
            if (UnbilledUnmeteredConsumption_H32 == 0)
            {
                return 0;
            }

            double j25 = ErrorFactor_O25 * Constants.StandardDistributionFactor / UnbilledUnmeteredConsumption_H32;
            return j25;
        }

        private static List<double> GetErrorFactorized(List<double> UnbilledUnmeteredConsumptionM3_H6_H23, List<double> UnbilledUnmeteredConsumptionError_J6_J23)
        {
            var result = new List<double>();
            for (int i = 0; i < UnbilledUnmeteredConsumptionM3_H6_H23.Count; i++)
            {
                double item = Math.Pow(UnbilledUnmeteredConsumptionM3_H6_H23[i] * UnbilledUnmeteredConsumptionError_J6_J23[i] / Constants.StandardDistributionFactor , 2);
                result.Add(item);
            }

            return result;
        }
    }
}