using System;
using System.Collections.Generic;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WbEasyCalcRepository.Calculation;
using WbEasyCalcRepository.Model;

namespace WbEasyCalcRepository
{
    public class WbEasyCalc : IWbEasyCalc
    {
        public EasyCalcDataOutput Calculate(EasyCalcDataInput easyCalcDataInput)
        {
            EasyCalcSheet easyCalcSheetData = ReadSheetData(easyCalcDataInput);

            EasyCalcDataOutput easyCalcDataOutput = new EasyCalcDataOutput()
            {
                WaterBalanceDay = GetWaterBalanceModel(easyCalcSheetData.WaterBalanceDaySheet),
                WaterBalancePeriod = GetWaterBalanceModel(easyCalcSheetData.WaterBalanceSheet),
                WaterBalanceYear = GetWaterBalanceModel(easyCalcSheetData.WaterBalanceYearSheet),

                Pis = GetPisModel(easyCalcSheetData.PiSheet),
                MatOneOut = GetMatOneOutModel(easyCalcSheetData.MatOneOutSheet),
                MatTwoOut = GetMatOneOutModel(easyCalcSheetData.MatTwoOutSheet),

                SysInput_ErrorMarg_F72 = easyCalcSheetData.SystemInputSheet.ErrorMargin_F72,
                SysInput_Min_D75 = easyCalcSheetData.SystemInputSheet.Min_D75,
                SysInput_Max_D77 = easyCalcSheetData.SystemInputSheet.Max_D77,
                SysInput_BestEstimate_D79 = easyCalcSheetData.SystemInputSheet.SystemInputVolume_D79,

                BilledCons_Sum_D28 = easyCalcSheetData.BilledConsumptionSheet.BilledMeteredConsumption_D28,
                BilledCons_Sum_H28 = easyCalcSheetData.BilledConsumptionSheet.BilledUnmeteredConsumption_H28,

                UnbilledCons_Sum_D32 = easyCalcSheetData.UnbilledConsumptionSheet.UnbilledMeteredConsumption_D32,
                UnbilledCons_ErrorMarg_J25 = easyCalcSheetData.UnbilledConsumptionSheet.UnbilledUnmeteredConsumptionErrorMargin_J25,
                UnbilledCons_Min_H28 = easyCalcSheetData.UnbilledConsumptionSheet.Min_H28,
                UnbilledCons_Max_H30 = easyCalcSheetData.UnbilledConsumptionSheet.Max_H30,
                UnbilledCons_BestEstimate_H32 = easyCalcSheetData.UnbilledConsumptionSheet.UnbilledUnmeteredConsumption_H32,

                UnauthCons_Total_L6 = easyCalcSheetData.UnauthorizedConsumptionSheet.IllegalConnectionsDomesticConsumptionTotalM3_L6,
                UnauthCons_Total_L10 = easyCalcSheetData.UnauthorizedConsumptionSheet.IllegalConnectionsOthersConsumptionTotalM3_L10,
                UnauthCons_Total_L14 = easyCalcSheetData.UnauthorizedConsumptionSheet.MeterTamperingBypassesEtcConsumptionTotalM3_L14,
                UnauthCons_ErrorMarg_F24 = easyCalcSheetData.UnauthorizedConsumptionSheet.ErrorMargin_F24,
                UnauthCons_Min_L27 = easyCalcSheetData.UnauthorizedConsumptionSheet.Min_L27,
                UnauthCons_Max_L29 = easyCalcSheetData.UnauthorizedConsumptionSheet.Max_L29,
                UnauthCons_BestEstimate_L31 = easyCalcSheetData.UnauthorizedConsumptionSheet.BestEstimateTotal_L31,
                UnauthCons_Total_L18 = easyCalcSheetData.UnauthorizedConsumptionSheet.Total_L18_L22[0],
                UnauthCons_Total_L19 = easyCalcSheetData.UnauthorizedConsumptionSheet.Total_L18_L22[1],
                UnauthCons_Total_L20 = easyCalcSheetData.UnauthorizedConsumptionSheet.Total_L18_L22[2],
                UnauthCons_Total_L21 = easyCalcSheetData.UnauthorizedConsumptionSheet.Total_L18_L22[3],

                MetErrors_Total_F8 = easyCalcSheetData.MeterErrorsSheet.BilledMeteredConsumptionWithoutBulkSupplyTotalM3_F8,
                MetErrors_Total_F32 = easyCalcSheetData.MeterErrorsSheet.MeteredBulkSupplyExportTotalM3_F32,
                MetErrors_Total_F34 = easyCalcSheetData.MeterErrorsSheet.UnbilledMeteredConsumptionWithoutBulkSupplyM3_F34,
                MetErrors_Total_F38 = easyCalcSheetData.MeterErrorsSheet.CorruptMeterReadingPracticesTotalM3_F38,
                MetErrors_Total_L8 = easyCalcSheetData.MeterErrorsSheet.BilledMeteredConsumptionWithoutBulkSupplyTotalM3_L8,
                MetErrors_Total_L32 = easyCalcSheetData.MeterErrorsSheet.MeteredBulkSupplyExportTotalWithMeterUnderregistrationM3_L32,
                MetErrors_Total_L34 = easyCalcSheetData.MeterErrorsSheet.UnbilledMeteredConsumptionWithoutBulkSupplyWithMeterUnderregistration_L34,
                MetErrors_Total_L38 = easyCalcSheetData.MeterErrorsSheet.CorruptMeterReadingPracticesWithMeterUnderregistrationM3_L38,
                MetErrors_ErrorMarg_N42 = easyCalcSheetData.MeterErrorsSheet.ErrorMarginTotal_N42,
                MetErrors_Min_L45 = easyCalcSheetData.MeterErrorsSheet.Min_L45,
                MetErrors_Max_L47 = easyCalcSheetData.MeterErrorsSheet.Max_L47,
                MetErrors_BestEstimate_L49 = easyCalcSheetData.MeterErrorsSheet.BestEstimateTotalM3_L49,
                MetErrors_Total_L12 = easyCalcSheetData.MeterErrorsSheet.BilledMeteredConsumptionManuallyEnteredWithMeterUnderregistrationM3_L12_L28[0],
                MetErrors_Total_L13 = easyCalcSheetData.MeterErrorsSheet.BilledMeteredConsumptionManuallyEnteredWithMeterUnderregistrationM3_L12_L28[1],
                MetErrors_Total_L14 = easyCalcSheetData.MeterErrorsSheet.BilledMeteredConsumptionManuallyEnteredWithMeterUnderregistrationM3_L12_L28[2],
                MetErrors_Total_L15 = easyCalcSheetData.MeterErrorsSheet.BilledMeteredConsumptionManuallyEnteredWithMeterUnderregistrationM3_L12_L28[3],

                Network_Total_D28 = easyCalcSheetData.NetworkSheet.DistributionAndTransmissionMainsTotalKm_D28,
                //Network_Total_D32 = Network_Total_D32,
                Network_Min_D39 = easyCalcSheetData.NetworkSheet.DistributionAndTransmissionMainsMinimum_D33,
                Network_Max_D41 = easyCalcSheetData.NetworkSheet.DistributionAndTransmissionMainsMaximum_D35,
                Network_BestEstimate_D43 = easyCalcSheetData.NetworkSheet.DistributionAndTransmissionMainsBestEstimate_D37,
                Network_Number_H21 = easyCalcSheetData.NetworkSheet.EstimatedNumberOfIllegalConnections_H21,
                Network_ErrorMarg_J21 = easyCalcSheetData.NetworkSheet.Network_ErrorMarg_J21,
                Network_ErrorMarg_J24 = easyCalcSheetData.NetworkSheet.Network_ErrorMarg_J24,
                Network_Min_H26 = easyCalcSheetData.NetworkSheet.Network_Min_H26,
                Network_Max_H28 = easyCalcSheetData.NetworkSheet.Network_Max_H28,
                Network_BestEstimate_H30 = easyCalcSheetData.NetworkSheet.ServiceConnectionsBestEstimate_H30,
                Network_Number_H39 = easyCalcSheetData.NetworkSheet.LenOfServConnFromBoundToMeterKm_H39,
                Network_ErrorMarg_J39 = easyCalcSheetData.NetworkSheet.Network_ErrorMarg_J39,

                Prs_Min_F29 = easyCalcSheetData.PressureSheet.Prs_Min_F29,
                Prs_Max_F31 = easyCalcSheetData.PressureSheet.Prs_Max_F31,
                Prs_BestEstimate_F33 = easyCalcSheetData.PressureSheet.AveragePressureBestEstimate_F33,

                Interm_BestEstimate_H33 = easyCalcSheetData.IntermittentSupplySheet.SupplyTimeBestEstimate_H33,
                Interm_Min_H29 = easyCalcSheetData.IntermittentSupplySheet.Interm_Min_F29,
                Interm_Max_H31 = easyCalcSheetData.IntermittentSupplySheet.Interm_Max_F31,


                FinancData_G13 = easyCalcSheetData.FinancialDataSheet.FinancData_G13,
                FinancData_G15 = easyCalcSheetData.FinancialDataSheet.FinancData_G15,
                FinancData_G17 = easyCalcSheetData.FinancialDataSheet.FinancData_G17,
                FinancData_G19 = easyCalcSheetData.FinancialDataSheet.FinancData_G19,
                FinancData_G20 = easyCalcSheetData.FinancialDataSheet.FinancData_G20,
                FinancData_G22 = easyCalcSheetData.FinancialDataSheet.FinancData_G22,
                FinancData_D24 = easyCalcSheetData.FinancialDataSheet.FinancData_D24,
                FinancData_G31 = easyCalcSheetData.FinancialDataSheet.FinancData_G31,
                FinancData_K8  = easyCalcSheetData.FinancialDataSheet.FinancData_K8 ,
                FinancData_K13 = easyCalcSheetData.FinancialDataSheet.FinancData_K13,
                FinancData_K15 = easyCalcSheetData.FinancialDataSheet.FinancData_K15,
                FinancData_K17 = easyCalcSheetData.FinancialDataSheet.FinancData_K17,
                FinancData_K19 = easyCalcSheetData.FinancialDataSheet.FinancData_K19,
                FinancData_K20 = easyCalcSheetData.FinancialDataSheet.FinancData_K20,
                FinancData_K22 = easyCalcSheetData.FinancialDataSheet.FinancData_K22,
                FinancData_K31 = easyCalcSheetData.FinancialDataSheet.FinancData_K31,
                FinancData_K35 = easyCalcSheetData.FinancialDataSheet.FinancData_K35,
            };

            return easyCalcDataOutput;
        }

        private EasyCalcSheet ReadSheetData(EasyCalcDataInput easyCalcDataInput)
        {
            var data = new EasyCalcSheet();

            data.StartSheet = new StartSheet
            {
                PeriodDays_M21 = easyCalcDataInput.Start_PeriodDays_M21,
            };

            data.SystemInputSheet = new SystemInputSheet
            {
                SystemInputVolumeM3_D6_D70 = new List<double> 
                { 
                    easyCalcDataInput.SysInput_SystemInputVolumeM3_D6,  
                    easyCalcDataInput.SysInput_SystemInputVolumeM3_D7,
                    easyCalcDataInput.SysInput_SystemInputVolumeM3_D8,  
                    easyCalcDataInput.SysInput_SystemInputVolumeM3_D9,
                },
                SystemInputVolumeError_F6_F70 = new List<double> 
                { 
                    easyCalcDataInput.SysInput_SystemInputVolumeError_F6, 
                    easyCalcDataInput.SysInput_SystemInputVolumeError_F7,
                    easyCalcDataInput.SysInput_SystemInputVolumeError_F8, 
                    easyCalcDataInput.SysInput_SystemInputVolumeError_F9,
                }
            };

            data.BilledConsumptionSheet = new BilledConsumptionSheet
            {
                BilledMeteredConsumptionBulkWaterSupplyExportM3_D6 = easyCalcDataInput.BilledCons_BilledMetConsBulkWatSupExpM3_D6,
                BulledUnmeteredConsumptionBulkWaterSupplyExportM3_H6 = easyCalcDataInput.BilledCons_BilledUnmetConsBulkWatSupExpM3_H6,
                BilledMeteredConsumptionWithoutBulkSupply_D8_D25 = new List<double> 
                { 
                    easyCalcDataInput.BilledCons_UnbMetConsM3_D8, 
                    easyCalcDataInput.BilledCons_UnbMetConsM3_D9, 
                    easyCalcDataInput.BilledCons_UnbMetConsM3_D10, 
                    easyCalcDataInput.BilledCons_UnbMetConsM3_D11, 
                },
                BilledUnmeteredConsumptionWithoutBulkSupply_H8_H25 = new List<double> 
                { 
                    easyCalcDataInput.BilledCons_UnbUnmetConsM3_H8, 
                    easyCalcDataInput.BilledCons_UnbUnmetConsM3_H9, 
                    easyCalcDataInput.BilledCons_UnbUnmetConsM3_H10, 
                    easyCalcDataInput.BilledCons_UnbUnmetConsM3_H11, 
                }, 
            };

            data.UnbilledConsumptionSheet = new UnbilledConsumptionSheet
            {
                MeteredConsumptionBulkWaterSupplyExportM3_D6 = easyCalcDataInput.UnbilledCons_MetConsBulkWatSupExpM3_D6,

                UnbilledMeteredConsumptionWithoutBulkSupply_D8_D23 = new List<double> 
                { 
                    easyCalcDataInput.UnbilledCons_UnbMetConsM3_D8, 
                    easyCalcDataInput.UnbilledCons_UnbMetConsM3_D9, 
                    easyCalcDataInput.UnbilledCons_UnbMetConsM3_D10, 
                    easyCalcDataInput.UnbilledCons_UnbMetConsM3_D11, 
                },
                UnbilledUnmeteredConsumptionM3_H6_H23 = new List<double> 
                { 
                    easyCalcDataInput.UnbilledCons_UnbUnmetConsM3_H6, 
                    easyCalcDataInput.UnbilledCons_UnbUnmetConsM3_H7, 
                    easyCalcDataInput.UnbilledCons_UnbUnmetConsM3_H8, 
                    easyCalcDataInput.UnbilledCons_UnbUnmetConsM3_H9, 
                    easyCalcDataInput.UnbilledCons_UnbUnmetConsM3_H10, 
                    easyCalcDataInput.UnbilledCons_UnbUnmetConsM3_H11, 
                },
                UnbilledUnmeteredConsumptionError_J6_J23 = new List<double> 
                { 
                    easyCalcDataInput.UnbilledCons_UnbUnmetConsError_J6, 
                    easyCalcDataInput.UnbilledCons_UnbUnmetConsError_J7, 
                    easyCalcDataInput.UnbilledCons_UnbUnmetConsError_J8, 
                    easyCalcDataInput.UnbilledCons_UnbUnmetConsError_J9, 
                    easyCalcDataInput.UnbilledCons_UnbUnmetConsError_J10, 
                    easyCalcDataInput.UnbilledCons_UnbUnmetConsError_J11, 
                },
            };

            data.UnauthorizedConsumptionSheet = new UnauthorizedConsumptionSheet(data)
            {
                OthersErrorMargin_F18_F22 = new List<double>
                {
                    easyCalcDataInput.UnauthCons_OthersErrorMargin_F18, 
                    easyCalcDataInput.UnauthCons_OthersErrorMargin_F19, 
                    easyCalcDataInput.UnauthCons_OthersErrorMargin_F20, 
                    easyCalcDataInput.UnauthCons_OthersErrorMargin_F21, 
                },
                OthersM3PerDay_J18_J22 = new List<double>
                {
                    easyCalcDataInput.UnauthCons_OthersM3PerDay_J18, 
                    easyCalcDataInput.UnauthCons_OthersM3PerDay_J19, 
                    easyCalcDataInput.UnauthCons_OthersM3PerDay_J20, 
                    easyCalcDataInput.UnauthCons_OthersM3PerDay_J21, 
                },
                IllegalConnectionsDomesticEstimatedNumber_D6 = easyCalcDataInput.UnauthCons_IllegalConnDomEstNo_D6,
                IllegalConnectionsDomesticPersonsPerHouse_H6 = easyCalcDataInput.UnauthCons_IllegalConnDomPersPerHouse_H6,
                IllegalConnectionsDomesticConsumptionLitersPerPersonPerDay_J6 = easyCalcDataInput.UnauthCons_IllegalConnDomConsLitPerPersDay_J6,
                IllegalConnectionsDomesticErrorMargin_F6 = easyCalcDataInput.UnauthCons_IllegalConnDomErrorMargin_F6,

                IllegalConnectionsOthersErrorMargin_F10 = easyCalcDataInput.UnauthCons_IllegalConnOthersErrorMargin_F10,

                IllegalConnectionsOthersEstimatedNumber_D10 = easyCalcDataInput.IllegalConnectionsOthersEstimatedNumber_D10,
                IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = easyCalcDataInput.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10,

                MeterTamperingBypassesEtcEstimatedNumber_D14 = easyCalcDataInput.UnauthCons_MeterTampBypEtcEstNo_D14,
                MeterTamperingBypassesEtcErrorMargin_F14 = easyCalcDataInput.UnauthCons_MeterTampBypEtcErrorMargin_F14,
                MeterTamperingBypassesEtcConsumptionLitersPerCustomerPerDay_J14 = easyCalcDataInput.UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14,
            };

            data.MeterErrorsSheet = new MeterErrorsSheet(data)
            {
                DetailedManualSpec_J6 = easyCalcDataInput.MetErrors_DetailedManualSpec_J6,
                BilledMeteredConsumptionWithoutBulkSupplyMeterUnderregistration_H8 = easyCalcDataInput.MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8,
                BilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N8 = easyCalcDataInput.MetErrors_BilledMetConsWoBulkSupErrorMargin_N8,

                BilledMeteredConsumptionManuallyEnteredM3_F12_F28 = new List<double>
                {
                    easyCalcDataInput.MetErrors_Total_F12,
                    easyCalcDataInput.MetErrors_Total_F13,
                    easyCalcDataInput.MetErrors_Total_F14,
                    easyCalcDataInput.MetErrors_Total_F15,
                },
                BilledMeteredConsumptionManuallyEnteredMeterUnderregistration_H12_H28 = new List<double>
                {
                    easyCalcDataInput.MetErrors_Meter_H12,
                    easyCalcDataInput.MetErrors_Meter_H13,
                    easyCalcDataInput.MetErrors_Meter_H14,
                    easyCalcDataInput.MetErrors_Meter_H15,
                },
                BilledMeteredConsumptionManuallyEnteredErrorMargin_N12_N28 = new List<double>
                {
                    easyCalcDataInput.MetErrors_Error_N12,
                    easyCalcDataInput.MetErrors_Error_N13,
                    easyCalcDataInput.MetErrors_Error_N14,
                    easyCalcDataInput.MetErrors_Error_N15,
                },

                MeteredBulkSupplyExportErrorMargin_N32 = easyCalcDataInput.MeteredBulkSupplyExportErrorMargin_N32,
                UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = easyCalcDataInput.UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34,
                CorruptMeterReadingPracticessErrorMargin_N38 = easyCalcDataInput.CorruptMeterReadingPracticessErrorMargin_N38,
                DataHandlingErrorsOffice_L40 = easyCalcDataInput.DataHandlingErrorsOffice_L40,
                DataHandlingErrorsOfficeErrorMargin_N40 = easyCalcDataInput.DataHandlingErrorsOfficeErrorMargin_N40,

                MeteredBulkSupplyExportMetereUnderregistration_H32 = easyCalcDataInput.MetErrors_MetBulkSupExpMetUnderreg_H32,
                UnbilledMeteredConsumptionWithoutBulkSupplyMeterUnderregistration_H34 = easyCalcDataInput.MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34,
                CorruptMeterReadingPracticesMeterUnderregistration_H38 = easyCalcDataInput.MetErrors_CorruptMetReadPractMetUndrreg_H38,
            };

            data.NetworkSheet = new NetworkSheet(data)
            {
                DistributionAndTransmissionMainsEntries_D7_D26 = new List<double> 
                { 
                    easyCalcDataInput.Network_DistributionAndTransmissionMains_D7, 
                    easyCalcDataInput.Network_DistributionAndTransmissionMains_D8, 
                    easyCalcDataInput.Network_DistributionAndTransmissionMains_D9, 
                    easyCalcDataInput.Network_DistributionAndTransmissionMains_D10, 
                },
                NumberOfConnectionsOfRegsteredCustomers_H10 = easyCalcDataInput.Network_NoOfConnOfRegCustomers_H10,
                NumberOfInactiveAccountsWServiceConnections_H18 = easyCalcDataInput.Network_NoOfInactAccountsWSvcConns_H18,
                AvgLenOfServiceConnectionFromBoundaryToMeterM_H32 = easyCalcDataInput.Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32,

                DistributionAndTransmissionMainsPossibleUnderestimation_D30 = easyCalcDataInput.Network_PossibleUnd_D30,
                Network_NoCustomers_H7 = easyCalcDataInput.Network_NoCustomers_H7,
                Network_ErrorMargin_J7 = easyCalcDataInput.Network_ErrorMargin_J7,
                Network_ErrorMargin_J10 = easyCalcDataInput.Network_ErrorMargin_J10,
                Network_ErrorMargin_J18 = easyCalcDataInput.Network_ErrorMargin_J18,
                Network_ErrorMargin_J32 = easyCalcDataInput.Network_ErrorMargin_J32,
                //Network_ErrorMargin_D35 = easyCalcDataInput.Network_ErrorMargin_D35,
            };

            data.PressureSheet = new PressureSheet()
            {
                ApproximateNumberOfConnections_D7_D24 = new List<double> 
                { 
                    //easyCalcDataInput.Prs_ApproxNoOfConn_D7, 
                    easyCalcDataInput.Prs_ApproxNoOfConn_D8, 
                    easyCalcDataInput.Prs_ApproxNoOfConn_D9, 
                    easyCalcDataInput.Prs_ApproxNoOfConn_D10, 
                },
                DailyAveragePressureM_F7_F24 = new List<double> 
                { 
                    //easyCalcDataInput.Prs_DailyAvgPrsM_F7, 
                    easyCalcDataInput.Prs_DailyAvgPrsM_F8, 
                    easyCalcDataInput.Prs_DailyAvgPrsM_F9, 
                    easyCalcDataInput.Prs_DailyAvgPrsM_F10, 
                },
                Prs_ApproxNoOfConn_D7 = easyCalcDataInput.Prs_ApproxNoOfConn_D7,
                Prs_DailyAvgPrsM_F7 = easyCalcDataInput.Prs_DailyAvgPrsM_F7,
                Prs_ErrorMarg_F26 = easyCalcDataInput.Prs_ErrorMarg_F26,
            };

            data.IntermittentSupplySheet = new IntermittentSupplySheet() 
            { 
                Interm_Conn_D7_24_List = new List<double> 
                {
                    easyCalcDataInput.Interm_Conn_D7,
                    easyCalcDataInput.Interm_Conn_D8,
                    easyCalcDataInput.Interm_Conn_D9,
                    easyCalcDataInput.Interm_Conn_D10,
                },
                Interm_Days_F7_24_List = new List<double> 
                { 
                    easyCalcDataInput.Interm_Days_F7,
                    easyCalcDataInput.Interm_Days_F8,
                    easyCalcDataInput.Interm_Days_F9,
                    easyCalcDataInput.Interm_Days_F10,
                },
                Interm_Hour_H7_24_List = new List<double> 
                { 
                    easyCalcDataInput.Interm_Hour_H7,
                    easyCalcDataInput.Interm_Hour_H8,
                    easyCalcDataInput.Interm_Hour_H9,
                    easyCalcDataInput.Interm_Hour_H10,
                },
                ErrorMargin_H26 = easyCalcDataInput.Interm_ErrorMarg_H26,
            };

            data.FinancialDataSheet = new FinancialDataSheet(data)
            {
                FinancData_G6 = easyCalcDataInput.FinancData_G6,
                FinancData_K6 = easyCalcDataInput.FinancData_K6,
                FinancData_G8 = easyCalcDataInput.FinancData_G8,
                FinancData_D26 = easyCalcDataInput.FinancData_D26,
                FinancData_G35 = easyCalcDataInput.FinancData_G35,
            };

            data.WaterBalanceSheet = new WaterBalanceSheet(data);
            data.WaterBalanceDaySheet = new WaterBalanceDaySheet(data);
            data.WaterBalanceYearSheet = new WaterBalanceYearSheet(data);

            data.PiSheet = new PiSheet(data);




            data.MatOneInSheet = new MatrixInSheet
            {
                SelectedOption = easyCalcDataInput.MatOneIn_SelectedOption,

                C11 = easyCalcDataInput.MatOneIn_C11,
                C12 = easyCalcDataInput.MatOneIn_C12,
                C13 = easyCalcDataInput.MatOneIn_C13,
                C14 = easyCalcDataInput.MatOneIn_C14,
                //C15 = easyCalcDataInput.MatOneIn_C15,
                C21 = easyCalcDataInput.MatOneIn_C21,
                C22 = easyCalcDataInput.MatOneIn_C22,
                C23 = easyCalcDataInput.MatOneIn_C23,
                C24 = easyCalcDataInput.MatOneIn_C24,
                //C25 = easyCalcDataInput.MatOneIn_C25,
                D21 = easyCalcDataInput.MatOneIn_D21,
                D22 = easyCalcDataInput.MatOneIn_D22,
                D23 = easyCalcDataInput.MatOneIn_D23,
                D24 = easyCalcDataInput.MatOneIn_D24,
                //D25 = easyCalcDataInput.MatOneIn_D25,
                E11 = easyCalcDataInput.MatOneIn_E11,
                E12 = easyCalcDataInput.MatOneIn_E12,
                E13 = easyCalcDataInput.MatOneIn_E13,
                E14 = easyCalcDataInput.MatOneIn_E14,
                //E15 = easyCalcDataInput.MatOneIn_E15,
                E21 = easyCalcDataInput.MatOneIn_E21,
                E22 = easyCalcDataInput.MatOneIn_E22,
                E23 = easyCalcDataInput.MatOneIn_E23,
                E24 = easyCalcDataInput.MatOneIn_E24,
                //E25 = easyCalcDataInput.MatOneIn_E25,
                F11 = easyCalcDataInput.MatOneIn_F11,
                F12 = easyCalcDataInput.MatOneIn_F12,
                F13 = easyCalcDataInput.MatOneIn_F13,
                F14 = easyCalcDataInput.MatOneIn_F14,
                //F15 = easyCalcDataInput.MatOneIn_F15,
                F21 = easyCalcDataInput.MatOneIn_F21,
                F22 = easyCalcDataInput.MatOneIn_F22,
                F23 = easyCalcDataInput.MatOneIn_F23,
                F24 = easyCalcDataInput.MatOneIn_F24,
                //F25 = easyCalcDataInput.MatOneIn_F25,
                G11 = easyCalcDataInput.MatOneIn_G11,
                G12 = easyCalcDataInput.MatOneIn_G12,
                G13 = easyCalcDataInput.MatOneIn_G13,
                G14 = easyCalcDataInput.MatOneIn_G14,
                //G15 = easyCalcDataInput.MatOneIn_G15,
                G21 = easyCalcDataInput.MatOneIn_G21,
                G22 = easyCalcDataInput.MatOneIn_G22,
                G23 = easyCalcDataInput.MatOneIn_G23,
                G24 = easyCalcDataInput.MatOneIn_G24,
                //G25 = easyCalcDataInput.MatOneIn_G25,
                H11 = easyCalcDataInput.MatOneIn_H11,
                H12 = easyCalcDataInput.MatOneIn_H12,
                H13 = easyCalcDataInput.MatOneIn_H13,
                H14 = easyCalcDataInput.MatOneIn_H14,
                //H15 = easyCalcDataInput.MatOneIn_H15,
                H21 = easyCalcDataInput.MatOneIn_H21,
                H22 = easyCalcDataInput.MatOneIn_H22,
                H23 = easyCalcDataInput.MatOneIn_H23,
                H24 = easyCalcDataInput.MatOneIn_H24,
                //H25 = easyCalcDataInput.MatOneIn_H25,
            };

            data.MatOneOutSheet = new MatrixOutSheet(data.MatOneInSheet);
            //data.MatTwoOutSheet = new MatOneOutSheet(data);




            return data;
        }

        private static WaterBalanceModel GetWaterBalanceModel(WaterBalanceSheet sheet)
        {
            WaterBalanceModel model = new WaterBalanceModel
            {
                PeriodDays_AF1 = sheet.PeriodDays_AF1,
                SystemInputVolume_B19 = sheet.SystemInputVolume_B19,
                SystemInputVolumeErrorMargin_B21 = sheet.SystemInputVolumeErrorMargin_B21,

                AuthorizedConsumption_K12 = sheet.AuthorizedConsumption_K12,
                AuthorizedConsumptionErrorMargin_K15 = sheet.AuthorizedConsumptionErrorMargin_K15,
                WaterLosses_K29 = sheet.WaterLosses_K29,
                WaterLossesErrorMargin_K31 = sheet.WaterLossesErrorMargin_K31,

                BilledAuthorizedConsumption_T8 = sheet.BilledAuthorizedConsumption_T8,
                UnbilledAuthorizedConsumption_T16 = sheet.UnbilledAuthorizedConsumption_T16,
                UnbilledAuthorizedConsumptionErrorMargin_T20 = sheet.UnbilledAuthorizedConsumptionErrorMargin_T20,
                CommercialLosses_T26 = sheet.CommercialLosses_T26,
                CommercialLossesErrorMargin_T29 = sheet.CommercialLossesErrorMargin_T29,
                PhysicalLossesM3_T34 = sheet.PhysicalLossesM3_T34,
                PhyscialLossesErrorMargin_AH35 = sheet.PhyscialLossesErrorMargin_AH35,

                BilledMeteredConsumption_AC4 = sheet.BilledMeteredConsumption_AC4,
                BilledUnmeteredConsumption_AC9 = sheet.BilledUnmeteredConsumption_AC9,
                UnbilledMeteredConsumption_AC14 = sheet.UnbilledMeteredConsumption_AC14,

                UnbilledUnmeteredConsumption_AC19 = sheet.UnbilledUnmeteredConsumption_AC19,
                UnbilledUnmeteredConsumptionErrorMargin_AO20 = sheet.UnbilledUnmeteredConsumptionErrorMargin_AO20,
                UnauthorizedConsumption_AC24 = sheet.UnauthorizedConsumption_AC24,
                UnauthorizedConsumptionErrorMargin_AO25 = sheet.UnauthorizedConsumptionErrorMargin_AO25,
                CustomerMeterInaccuraciesAndErrorsM3_AC29 = sheet.CustomerMeterInaccuraciesAndErrorsM3_AC29,
                CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30 = sheet.CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30,

                RevenueWaterM3_AY8 = sheet.RevenueWaterM3_AY8,
                NonRevenueWaterM3_AY24 = sheet.NonRevenueWaterM3_AY24,
                NonRevenueWaterErrorMargin_AY26 = sheet.NonRevenueWaterErrorMargin_AY26,

            };
            return model;
        }
        private static PisModel GetPisModel(PiSheet pisSheet)
        {
            PisModel model = new PisModel
            {
                Pis_F9 =  pisSheet.Pis_F9,
                Pis_H9 =  pisSheet.Pis_H9,
                Pis_J9 =  pisSheet.Pis_J9,
                Pis_L9 =  pisSheet.Pis_L9,
                Pis_F11 = pisSheet.Pis_F11,
                Pis_H11 = pisSheet.Pis_H11,
                Pis_J11 = pisSheet.Pis_J11,
                Pis_L11 = pisSheet.Pis_L11,
                Pis_F17 = pisSheet.Pis_F17,
                Pis_H17 = pisSheet.Pis_H17,
                Pis_J17 = pisSheet.Pis_J17,
                Pis_L17 = pisSheet.Pis_L17,
                Pis_F19 = pisSheet.Pis_F19,
                Pis_H19 = pisSheet.Pis_H19,
                Pis_J19 = pisSheet.Pis_J19,
                Pis_L19 = pisSheet.Pis_L19,
                Pis_F25 = pisSheet.Pis_F25,
                Pis_H25 = pisSheet.Pis_H25,
                Pis_J25 = pisSheet.Pis_J25,
                Pis_L25 = pisSheet.Pis_L25,
                Pis_F27 = pisSheet.Pis_F27,
                Pis_H27 = pisSheet.Pis_H27,
                Pis_J27 = pisSheet.Pis_J27,
                Pis_L27 = pisSheet.Pis_L27,
                Pis_F29 = pisSheet.Pis_F29,
                Pis_H29 = pisSheet.Pis_H29,
                Pis_J29 = pisSheet.Pis_J29,
                Pis_L29 = pisSheet.Pis_L29,
                Pis_F31 = pisSheet.Pis_F31,
                Pis_H31 = pisSheet.Pis_H31,
                Pis_J31 = pisSheet.Pis_J31,
                Pis_L31 = pisSheet.Pis_L31,
                Pis_F37 = pisSheet.Pis_F37,
                Pis_H37 = pisSheet.Pis_H37,
                Pis_J37 = pisSheet.Pis_J37,
                Pis_L37 = pisSheet.Pis_L37,
                Pis_F39 = pisSheet.Pis_F39,
                Pis_H39 = pisSheet.Pis_H39,
                Pis_J39 = pisSheet.Pis_J39,
                Pis_L39 = pisSheet.Pis_L39,
                Pis_F41 = pisSheet.Pis_F41,
                Pis_H41 = pisSheet.Pis_H41,
                Pis_J41 = pisSheet.Pis_J41,
                Pis_L41 = pisSheet.Pis_L41,
                Pis_F47 = pisSheet.Pis_F47,
                Pis_H47 = pisSheet.Pis_H47,
                Pis_J47 = pisSheet.Pis_J47,
                Pis_L47 = pisSheet.Pis_L47,
                Pis_F49 = pisSheet.Pis_F49,
                Pis_H49 = pisSheet.Pis_H49,
                Pis_J49 = pisSheet.Pis_J49,
                Pis_L49 = pisSheet.Pis_L49,
                Pis_F51 = pisSheet.Pis_F51,
                Pis_H51 = pisSheet.Pis_H51,
                Pis_J51 = pisSheet.Pis_J51,
                Pis_L51 = pisSheet.Pis_L51,

                Pis_N27 = pisSheet.Pis_N27,
                Pis_P27 = pisSheet.Pis_P27,
                Pis_N47 = pisSheet.Pis_N47,
                Pis_P47 = pisSheet.Pis_P47,

            };
            return model;
        }
        private static MatrixOutModel GetMatOneOutModel(MatrixOutSheet sheet)
        {
            MatrixOutModel model = new MatrixOutModel
            {
                C11 = sheet.C11,
                C12 = sheet.C12,
                C13 = sheet.C13,
                C14 = sheet.C14,
                C15 = sheet.C15,
                C21 = sheet.C21,
                C22 = sheet.C22,
                C23 = sheet.C23,
                C24 = sheet.C24,
                C25 = sheet.C25,
                D21 = sheet.D21,
                D22 = sheet.D22,
                D23 = sheet.D23,
                D24 = sheet.D24,
                D25 = sheet.D25,
                E11 = sheet.E11,
                E12 = sheet.E12,
                E13 = sheet.E13,
                E14 = sheet.E14,
                E15 = sheet.E15,
                E21 = sheet.E21,
                E22 = sheet.E22,
                E23 = sheet.E23,
                E24 = sheet.E24,
                E25 = sheet.E25,
                F11 = sheet.F11,
                F12 = sheet.F12,
                F13 = sheet.F13,
                F14 = sheet.F14,
                F15 = sheet.F15,
                F21 = sheet.F21,
                F22 = sheet.F22,
                F23 = sheet.F23,
                F24 = sheet.F24,
                F25 = sheet.F25,
                G11 = sheet.G11,
                G12 = sheet.G12,
                G13 = sheet.G13,
                G14 = sheet.G14,
                G15 = sheet.G15,
                G21 = sheet.G21,
                G22 = sheet.G22,
                G23 = sheet.G23,
                G24 = sheet.G24,
                G25 = sheet.G25,
                H11 = sheet.H11,
                H12 = sheet.H12,
                H13 = sheet.H13,
                H14 = sheet.H14,
                H15 = sheet.H15,
                H21 = sheet.H21,
                H22 = sheet.H22,
                H23 = sheet.H23,
                H24 = sheet.H24,
                H25 = sheet.H25,
            };
            return model;
        }





        public static void CalculateNew(EasyCalcModel easyCalcModel)
        {
            EasyCalcSheet easyCalcSheetData = CalcSheetData(easyCalcModel);


            SysInputModel sysInputModel = GetSysInputModel(easyCalcSheetData.SystemInputSheet);
            easyCalcModel.SysInputModel.SysInput_ErrorMarg_F72 = sysInputModel.SysInput_ErrorMarg_F72;
            easyCalcModel.SysInputModel.SysInput_Min_D75 = sysInputModel.SysInput_Min_D75;
            easyCalcModel.SysInputModel.SysInput_Max_D77 = sysInputModel.SysInput_Max_D77;
            easyCalcModel.SysInputModel.SysInput_BestEstimate_D79 = sysInputModel.SysInput_BestEstimate_D79;

            BilledConsumptionCalc.Calculate(easyCalcModel);
            UnbilledConsumptionCalc.Calculate(easyCalcModel);

            var unauthConsModel = GetUnauthConsModel(easyCalcSheetData.UnauthorizedConsumptionSheet);
            easyCalcModel.UnauthConsModel.UnauthCons_Total_L6 = unauthConsModel.UnauthCons_Total_L6;
            easyCalcModel.UnauthConsModel.UnauthCons_Total_L10 = unauthConsModel.UnauthCons_Total_L10;
            easyCalcModel.UnauthConsModel.UnauthCons_Total_L14 = unauthConsModel.UnauthCons_Total_L14;
            easyCalcModel.UnauthConsModel.UnauthCons_ErrorMarg_F24 = unauthConsModel.UnauthCons_ErrorMarg_F24;
            easyCalcModel.UnauthConsModel.UnauthCons_Min_L27 = unauthConsModel.UnauthCons_Min_L27;
            easyCalcModel.UnauthConsModel.UnauthCons_Max_L29 = unauthConsModel.UnauthCons_Max_L29;
            easyCalcModel.UnauthConsModel.UnauthCons_BestEstimate_L31 = unauthConsModel.UnauthCons_BestEstimate_L31;
            easyCalcModel.UnauthConsModel.UnauthCons_Total_L18 = unauthConsModel.UnauthCons_Total_L18;
            easyCalcModel.UnauthConsModel.UnauthCons_Total_L19 = unauthConsModel.UnauthCons_Total_L19;
            easyCalcModel.UnauthConsModel.UnauthCons_Total_L20 = unauthConsModel.UnauthCons_Total_L20;
            easyCalcModel.UnauthConsModel.UnauthCons_Total_L21 = unauthConsModel.UnauthCons_Total_L21;

            var metErrorsModel = GetMetErrorsModel(easyCalcSheetData.MeterErrorsSheet);
            easyCalcModel.MetErrorsModel.MetErrors_Total_F8 = metErrorsModel.MetErrors_Total_F8;
            easyCalcModel.MetErrorsModel.MetErrors_Total_F32 = metErrorsModel.MetErrors_Total_F32;
            easyCalcModel.MetErrorsModel.MetErrors_Total_F34 = metErrorsModel.MetErrors_Total_F34;
            easyCalcModel.MetErrorsModel.MetErrors_Total_F38 = metErrorsModel.MetErrors_Total_F38;
            easyCalcModel.MetErrorsModel.MetErrors_Total_L8 = metErrorsModel.MetErrors_Total_L8;
            easyCalcModel.MetErrorsModel.MetErrors_Total_L32 = metErrorsModel.MetErrors_Total_L32;
            easyCalcModel.MetErrorsModel.MetErrors_Total_L34 = metErrorsModel.MetErrors_Total_L34;
            easyCalcModel.MetErrorsModel.MetErrors_Total_L38 = metErrorsModel.MetErrors_Total_L38;
            easyCalcModel.MetErrorsModel.MetErrors_ErrorMarg_N42 = metErrorsModel.MetErrors_ErrorMarg_N42;
            easyCalcModel.MetErrorsModel.MetErrors_Min_L45 = metErrorsModel.MetErrors_Min_L45;
            easyCalcModel.MetErrorsModel.MetErrors_Max_L47 = metErrorsModel.MetErrors_Max_L47;
            easyCalcModel.MetErrorsModel.MetErrors_BestEstimate_L49 = metErrorsModel.MetErrors_BestEstimate_L49;
            easyCalcModel.MetErrorsModel.MetErrors_Total_L12 = metErrorsModel.MetErrors_Total_L12;
            easyCalcModel.MetErrorsModel.MetErrors_Total_L13 = metErrorsModel.MetErrors_Total_L13;
            easyCalcModel.MetErrorsModel.MetErrors_Total_L14 = metErrorsModel.MetErrors_Total_L14;
            easyCalcModel.MetErrorsModel.MetErrors_Total_L15 = metErrorsModel.MetErrors_Total_L15;

            var networkModel = GetNetworkModel(easyCalcSheetData.NetworkSheet);
            easyCalcModel.NetworkModel.Network_Total_D28 = networkModel.Network_Total_D28;
            easyCalcModel.NetworkModel.Network_Total_D32 = networkModel.Network_Total_D32;
            easyCalcModel.NetworkModel.Network_Min_D39 = networkModel.Network_Min_D39;
            easyCalcModel.NetworkModel.Network_Max_D41 = networkModel.Network_Max_D41;
            easyCalcModel.NetworkModel.Network_BestEstimate_D43 = networkModel.Network_BestEstimate_D43;
            easyCalcModel.NetworkModel.Network_Number_H21 = networkModel.Network_Number_H21;
            easyCalcModel.NetworkModel.Network_ErrorMarg_J21 = networkModel.Network_ErrorMarg_J21;
            easyCalcModel.NetworkModel.Network_ErrorMarg_J24 = networkModel.Network_ErrorMarg_J24;
            easyCalcModel.NetworkModel.Network_Min_H26 = networkModel.Network_Min_H26;
            easyCalcModel.NetworkModel.Network_Max_H28 = networkModel.Network_Max_H28;
            easyCalcModel.NetworkModel.Network_BestEstimate_H30 = networkModel.Network_BestEstimate_H30;
            easyCalcModel.NetworkModel.Network_Number_H39 = networkModel.Network_Number_H39;
            easyCalcModel.NetworkModel.Network_ErrorMarg_J39 = networkModel.Network_ErrorMarg_J39;

            var pressureModel = GetPressureModel(easyCalcSheetData.PressureSheet);
            easyCalcModel.PressureModel.Prs_BestEstimate_F33 = pressureModel.Prs_BestEstimate_F33;
            easyCalcModel.PressureModel.Prs_Min_F29 = pressureModel.Prs_Min_F29;
            easyCalcModel.PressureModel.Prs_Max_F31 = pressureModel.Prs_Max_F31;

            var intermModel = GetIntermModel(easyCalcSheetData.IntermittentSupplySheet);
            easyCalcModel.IntermModel.Interm_BestEstimate_H33 = intermModel.Interm_BestEstimate_H33;
            easyCalcModel.IntermModel.Interm_Min_H29 = intermModel.Interm_Min_H29;
            easyCalcModel.IntermModel.Interm_Max_H31 = intermModel.Interm_Max_H31;

            easyCalcModel.WaterBalanceDay = GetWaterBalanceModel(easyCalcSheetData.WaterBalanceDaySheet);
            easyCalcModel.WaterBalancePeriod = GetWaterBalanceModel(easyCalcSheetData.WaterBalanceSheet);
            easyCalcModel.WaterBalanceYear = GetWaterBalanceModel(easyCalcSheetData.WaterBalanceYearSheet);

            var model = GetFinancDataModel(easyCalcSheetData.FinancialDataSheet);
            easyCalcModel.FinancDataModel.FinancData_G13 = model.FinancData_G13;
            easyCalcModel.FinancDataModel.FinancData_G15 = model.FinancData_G15;
            easyCalcModel.FinancDataModel.FinancData_G17 = model.FinancData_G17;
            easyCalcModel.FinancDataModel.FinancData_G19 = model.FinancData_G19;
            easyCalcModel.FinancDataModel.FinancData_G20 = model.FinancData_G20;
            easyCalcModel.FinancDataModel.FinancData_G22 = model.FinancData_G22;
            easyCalcModel.FinancDataModel.FinancData_D24 = model.FinancData_D24;
            easyCalcModel.FinancDataModel.FinancData_G31 = model.FinancData_G31;
            easyCalcModel.FinancDataModel.FinancData_K8 = model.FinancData_K8;
            easyCalcModel.FinancDataModel.FinancData_K13 = model.FinancData_K13;
            easyCalcModel.FinancDataModel.FinancData_K15 = model.FinancData_K15;
            easyCalcModel.FinancDataModel.FinancData_K17 = model.FinancData_K17;
            easyCalcModel.FinancDataModel.FinancData_K19 = model.FinancData_K19;
            easyCalcModel.FinancDataModel.FinancData_K20 = model.FinancData_K20;
            easyCalcModel.FinancDataModel.FinancData_K22 = model.FinancData_K22;
            easyCalcModel.FinancDataModel.FinancData_K31 = model.FinancData_K31;
            easyCalcModel.FinancDataModel.FinancData_K35 = model.FinancData_K35;

            easyCalcModel.Pis = GetPisModel(easyCalcSheetData.PiSheet);
            easyCalcModel.MatrixOneOut = GetMatOneOutModel(easyCalcSheetData.MatOneOutSheet);
            easyCalcModel.MatrixTwoOut = GetMatOneOutModel(easyCalcSheetData.MatTwoOutSheet);
        }

        private static EasyCalcSheet CalcSheetData(EasyCalcModel easyCalcModel)
        {
            var data = new EasyCalcSheet();

            data.StartSheet = new StartSheet
            {
                PeriodDays_M21 = easyCalcModel.StartModel.Start_PeriodDays_M21,
            };

            data.SystemInputSheet = new SystemInputSheet
            {
                SystemInputVolumeM3_D6_D70 = new List<double>
                {
                    easyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D6,
                    easyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D7,
                    easyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D8,
                    easyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D9,
                },
                SystemInputVolumeError_F6_F70 = new List<double>
                {
                    easyCalcModel.SysInputModel.SysInput_SystemInputVolumeError_F6,
                    easyCalcModel.SysInputModel.SysInput_SystemInputVolumeError_F7,
                    easyCalcModel.SysInputModel.SysInput_SystemInputVolumeError_F8,
                    easyCalcModel.SysInputModel.SysInput_SystemInputVolumeError_F9,
                }
            };

            data.BilledConsumptionSheet = new BilledConsumptionSheet
            {
                BilledMeteredConsumptionBulkWaterSupplyExportM3_D6 = easyCalcModel.BilledConsModel.BilledCons_BilledMetConsBulkWatSupExpM3_D6,
                BulledUnmeteredConsumptionBulkWaterSupplyExportM3_H6 = easyCalcModel.BilledConsModel.BilledCons_BilledUnmetConsBulkWatSupExpM3_H6,
                BilledMeteredConsumptionWithoutBulkSupply_D8_D25 = new List<double>
                {
                    easyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D8,
                    easyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D9,
                    easyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D10,
                    easyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D11,
                },
                BilledUnmeteredConsumptionWithoutBulkSupply_H8_H25 = new List<double>
                {
                    easyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H8,
                    easyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H9,
                    easyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H10,
                    easyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H11,
                },
            };

            data.UnbilledConsumptionSheet = new UnbilledConsumptionSheet
            {
                MeteredConsumptionBulkWaterSupplyExportM3_D6 = easyCalcModel.UnbilledConsModel.UnbilledCons_MetConsBulkWatSupExpM3_D6,

                UnbilledMeteredConsumptionWithoutBulkSupply_D8_D23 = new List<double>
                {
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D8,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D9,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D10,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D11,
                },
                UnbilledUnmeteredConsumptionM3_H6_H23 = new List<double>
                {
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H6,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H7,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H8,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H9,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H10,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H11,
                },
                UnbilledUnmeteredConsumptionError_J6_J23 = new List<double>
                {
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J6,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J7,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J8,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J9,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J10,
                    easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J11,
                },
            };

            data.UnauthorizedConsumptionSheet = new UnauthorizedConsumptionSheet(data)
            {
                OthersErrorMargin_F18_F22 = new List<double>
                {
                    easyCalcModel.UnauthConsModel.UnauthCons_OthersErrorMargin_F18,
                    easyCalcModel.UnauthConsModel.UnauthCons_OthersErrorMargin_F19,
                    easyCalcModel.UnauthConsModel.UnauthCons_OthersErrorMargin_F20,
                    easyCalcModel.UnauthConsModel.UnauthCons_OthersErrorMargin_F21,
                },
                OthersM3PerDay_J18_J22 = new List<double>
                {
                    easyCalcModel.UnauthConsModel.UnauthCons_OthersM3PerDay_J18,
                    easyCalcModel.UnauthConsModel.UnauthCons_OthersM3PerDay_J19,
                    easyCalcModel.UnauthConsModel.UnauthCons_OthersM3PerDay_J20,
                    easyCalcModel.UnauthConsModel.UnauthCons_OthersM3PerDay_J21,
                },
                IllegalConnectionsDomesticEstimatedNumber_D6 = easyCalcModel.UnauthConsModel.UnauthCons_IllegalConnDomEstNo_D6,
                IllegalConnectionsDomesticPersonsPerHouse_H6 = easyCalcModel.UnauthConsModel.UnauthCons_IllegalConnDomPersPerHouse_H6,
                IllegalConnectionsDomesticConsumptionLitersPerPersonPerDay_J6 = easyCalcModel.UnauthConsModel.UnauthCons_IllegalConnDomConsLitPerPersDay_J6,
                IllegalConnectionsDomesticErrorMargin_F6 = easyCalcModel.UnauthConsModel.UnauthCons_IllegalConnDomErrorMargin_F6,

                IllegalConnectionsOthersErrorMargin_F10 = easyCalcModel.UnauthConsModel.UnauthCons_IllegalConnOthersErrorMargin_F10,

                IllegalConnectionsOthersEstimatedNumber_D10 = easyCalcModel.UnauthConsModel.IllegalConnectionsOthersEstimatedNumber_D10,
                IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = easyCalcModel.UnauthConsModel.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10,

                MeterTamperingBypassesEtcEstimatedNumber_D14 = easyCalcModel.UnauthConsModel.UnauthCons_MeterTampBypEtcEstNo_D14,
                MeterTamperingBypassesEtcErrorMargin_F14 = easyCalcModel.UnauthConsModel.UnauthCons_MeterTampBypEtcErrorMargin_F14,
                MeterTamperingBypassesEtcConsumptionLitersPerCustomerPerDay_J14 = easyCalcModel.UnauthConsModel.UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14,
            };
            data.MeterErrorsSheet = new MeterErrorsSheet(data)
            {
                DetailedManualSpec_J6 = easyCalcModel.MetErrorsModel.MetErrors_DetailedManualSpec_J6==2,
                BilledMeteredConsumptionWithoutBulkSupplyMeterUnderregistration_H8 = easyCalcModel.MetErrorsModel.MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8,
                BilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N8 = easyCalcModel.MetErrorsModel.MetErrors_BilledMetConsWoBulkSupErrorMargin_N8,

                BilledMeteredConsumptionManuallyEnteredM3_F12_F28 = new List<double>
                {
                    easyCalcModel.MetErrorsModel.MetErrors_Total_F12,
                    easyCalcModel.MetErrorsModel.MetErrors_Total_F13,
                    easyCalcModel.MetErrorsModel.MetErrors_Total_F14,
                    easyCalcModel.MetErrorsModel.MetErrors_Total_F15,
                },
                BilledMeteredConsumptionManuallyEnteredMeterUnderregistration_H12_H28 = new List<double>
                {
                    easyCalcModel.MetErrorsModel.MetErrors_Meter_H12,
                    easyCalcModel.MetErrorsModel.MetErrors_Meter_H13,
                    easyCalcModel.MetErrorsModel.MetErrors_Meter_H14,
                    easyCalcModel.MetErrorsModel.MetErrors_Meter_H15,
                },
                BilledMeteredConsumptionManuallyEnteredErrorMargin_N12_N28 = new List<double>
                {
                    easyCalcModel.MetErrorsModel.MetErrors_Error_N12,
                    easyCalcModel.MetErrorsModel.MetErrors_Error_N13,
                    easyCalcModel.MetErrorsModel.MetErrors_Error_N14,
                    easyCalcModel.MetErrorsModel.MetErrors_Error_N15,
                },

                MeteredBulkSupplyExportErrorMargin_N32 = easyCalcModel.MetErrorsModel.MeteredBulkSupplyExportErrorMargin_N32,
                UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = easyCalcModel.MetErrorsModel.UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34,
                CorruptMeterReadingPracticessErrorMargin_N38 = easyCalcModel.MetErrorsModel.CorruptMeterReadingPracticessErrorMargin_N38,
                DataHandlingErrorsOffice_L40 = easyCalcModel.MetErrorsModel.DataHandlingErrorsOffice_L40,
                DataHandlingErrorsOfficeErrorMargin_N40 = easyCalcModel.MetErrorsModel.DataHandlingErrorsOfficeErrorMargin_N40,

                MeteredBulkSupplyExportMetereUnderregistration_H32 = easyCalcModel.MetErrorsModel.MetErrors_MetBulkSupExpMetUnderreg_H32,
                UnbilledMeteredConsumptionWithoutBulkSupplyMeterUnderregistration_H34 = easyCalcModel.MetErrorsModel.MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34,
                CorruptMeterReadingPracticesMeterUnderregistration_H38 = easyCalcModel.MetErrorsModel.MetErrors_CorruptMetReadPractMetUndrreg_H38,
            };
            data.NetworkSheet = new NetworkSheet(data)
            {
                DistributionAndTransmissionMainsEntries_D7_D26 = new List<double> 
                { 
                    easyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D7, 
                    easyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D8, 
                    easyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D9,
                    easyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D10, 
                },
                NumberOfConnectionsOfRegsteredCustomers_H10 = easyCalcModel.NetworkModel.Network_NoOfConnOfRegCustomers_H10,
                NumberOfInactiveAccountsWServiceConnections_H18 = easyCalcModel.NetworkModel.Network_NoOfInactAccountsWSvcConns_H18,
                AvgLenOfServiceConnectionFromBoundaryToMeterM_H32 = easyCalcModel.NetworkModel.Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32,

                DistributionAndTransmissionMainsPossibleUnderestimation_D30 = easyCalcModel.NetworkModel.Network_PossibleUnd_D30,
                Network_NoCustomers_H7 =  easyCalcModel.NetworkModel.Network_NoCustomers_H7,
                Network_ErrorMargin_J7 =  easyCalcModel.NetworkModel.Network_ErrorMargin_J7,
                Network_ErrorMargin_J10 = easyCalcModel.NetworkModel.Network_ErrorMargin_J10,
                Network_ErrorMargin_J18 = easyCalcModel.NetworkModel.Network_ErrorMargin_J18,
                Network_ErrorMargin_J32 = easyCalcModel.NetworkModel.Network_ErrorMargin_J32,
                //Network_ErrorMargin_D35 = easyCalcDataInput.NetworkModel.Network_ErrorMargin_D35,
            };

            data.PressureSheet = new PressureSheet()
            {
                ApproximateNumberOfConnections_D7_D24 = new List<double> 
                { 
                    //easyCalcModel.PressureModel.Prs_ApproxNoOfConn_D7, 
                    easyCalcModel.PressureModel.Prs_ApproxNoOfConn_D8, 
                    easyCalcModel.PressureModel.Prs_ApproxNoOfConn_D9,
                    easyCalcModel.PressureModel.Prs_ApproxNoOfConn_D10, 
                },
                DailyAveragePressureM_F7_F24 = new List<double> 
                { 
                    //easyCalcModel.PressureModel.Prs_DailyAvgPrsM_F7, 
                    easyCalcModel.PressureModel.Prs_DailyAvgPrsM_F8, 
                    easyCalcModel.PressureModel.Prs_DailyAvgPrsM_F9,
                    easyCalcModel.PressureModel.Prs_DailyAvgPrsM_F10, 
                },
                Prs_ApproxNoOfConn_D7 =   easyCalcModel.PressureModel.Prs_ApproxNoOfConn_D7,
                Prs_DailyAvgPrsM_F7 =   easyCalcModel.PressureModel.Prs_DailyAvgPrsM_F7, 
                Prs_ErrorMarg_F26 = easyCalcModel.PressureModel.Prs_ErrorMarg_F26,
            };

            data.IntermittentSupplySheet = new IntermittentSupplySheet() 
            { 
                Interm_Conn_D7_24_List = new List<double> 
                {
                    easyCalcModel.IntermModel.Interm_Conn_D7,
                    easyCalcModel.IntermModel.Interm_Conn_D8,
                    easyCalcModel.IntermModel.Interm_Conn_D9,
                    easyCalcModel.IntermModel.Interm_Conn_D10,
                },
                Interm_Days_F7_24_List = new List<double> 
                { 
                    easyCalcModel.IntermModel.Interm_Days_F7,
                    easyCalcModel.IntermModel.Interm_Days_F8,
                    easyCalcModel.IntermModel.Interm_Days_F9,
                    easyCalcModel.IntermModel.Interm_Days_F10,
                },
                Interm_Hour_H7_24_List = new List<double> 
                { 
                    easyCalcModel.IntermModel.Interm_Hour_H7,
                    easyCalcModel.IntermModel.Interm_Hour_H8,
                    easyCalcModel.IntermModel.Interm_Hour_H9,
                    easyCalcModel.IntermModel.Interm_Hour_H10,
                },
                ErrorMargin_H26 = easyCalcModel.IntermModel.Interm_ErrorMarg_H26,
            };

            data.FinancialDataSheet = new FinancialDataSheet(data)
            {
                FinancData_G6 = easyCalcModel.FinancDataModel.FinancData_G6,
                FinancData_K6 = easyCalcModel.FinancDataModel.FinancData_K6,
                FinancData_G8 = easyCalcModel.FinancDataModel.FinancData_G8,
                FinancData_D26 = easyCalcModel.FinancDataModel.FinancData_D26,
                FinancData_G35 = easyCalcModel.FinancDataModel.FinancData_G35,
            };

            data.WaterBalanceSheet = new WaterBalanceSheet(data);
            data.WaterBalanceDaySheet = new WaterBalanceDaySheet(data);
            data.WaterBalanceYearSheet = new WaterBalanceYearSheet(data);

            data.PiSheet = new PiSheet(data);

            data.MatOneInSheet = new MatrixInSheet
            {
                SelectedOption = easyCalcModel.MatrixOneIn.SelectedOption,

                C11 = easyCalcModel.MatrixOneIn.C11,
                C12 = easyCalcModel.MatrixOneIn.C12,
                C13 = easyCalcModel.MatrixOneIn.C13,
                C14 = easyCalcModel.MatrixOneIn.C14,
                //C15 = easyCalcModel.MatrixOneIn.C15,
                C21 = easyCalcModel.MatrixOneIn.C21,
                C22 = easyCalcModel.MatrixOneIn.C22,
                C23 = easyCalcModel.MatrixOneIn.C23,
                C24 = easyCalcModel.MatrixOneIn.C24,
                //C25 = easyCalcModel.MatrixOneIn.C25,
                D21 = easyCalcModel.MatrixOneIn.D21,
                D22 = easyCalcModel.MatrixOneIn.D22,
                D23 = easyCalcModel.MatrixOneIn.D23,
                D24 = easyCalcModel.MatrixOneIn.D24,
                //D25 = easyCalcModel.MatrixOneIn.D25,
                E11 = easyCalcModel.MatrixOneIn.E11,
                E12 = easyCalcModel.MatrixOneIn.E12,
                E13 = easyCalcModel.MatrixOneIn.E13,
                E14 = easyCalcModel.MatrixOneIn.E14,
                //E15 = easyCalcModel.MatrixOneIn.E15,
                E21 = easyCalcModel.MatrixOneIn.E21,
                E22 = easyCalcModel.MatrixOneIn.E22,
                E23 = easyCalcModel.MatrixOneIn.E23,
                E24 = easyCalcModel.MatrixOneIn.E24,
                //E25 = easyCalcModel.MatrixOneIn.E25,
                F11 = easyCalcModel.MatrixOneIn.F11,
                F12 = easyCalcModel.MatrixOneIn.F12,
                F13 = easyCalcModel.MatrixOneIn.F13,
                F14 = easyCalcModel.MatrixOneIn.F14,
                //F15 = easyCalcModel.MatrixOneIn.F15,
                F21 = easyCalcModel.MatrixOneIn.F21,
                F22 = easyCalcModel.MatrixOneIn.F22,
                F23 = easyCalcModel.MatrixOneIn.F23,
                F24 = easyCalcModel.MatrixOneIn.F24,
                //F25 = easyCalcModel.MatrixOneIn.F25,
                G11 = easyCalcModel.MatrixOneIn.G11,
                G12 = easyCalcModel.MatrixOneIn.G12,
                G13 = easyCalcModel.MatrixOneIn.G13,
                G14 = easyCalcModel.MatrixOneIn.G14,
                //G15 = easyCalcModel.MatrixOneIn.G15,
                G21 = easyCalcModel.MatrixOneIn.G21,
                G22 = easyCalcModel.MatrixOneIn.G22,
                G23 = easyCalcModel.MatrixOneIn.G23,
                G24 = easyCalcModel.MatrixOneIn.G24,
                //G25 = easyCalcModel.MatrixOneIn.G25,
                H11 = easyCalcModel.MatrixOneIn.H11,
                H12 = easyCalcModel.MatrixOneIn.H12,
                H13 = easyCalcModel.MatrixOneIn.H13,
                H14 = easyCalcModel.MatrixOneIn.H14,
                //H15 = easyCalcModel.MatrixOneIn.H15,
                H21 = easyCalcModel.MatrixOneIn.H21,
                H22 = easyCalcModel.MatrixOneIn.H22,
                H23 = easyCalcModel.MatrixOneIn.H23,
                H24 = easyCalcModel.MatrixOneIn.H24,
                //H25 = easyCalcModel.MatrixOneIn.H25,
            };
            data.MatOneOutSheet = new MatrixOutSheet(data.MatOneInSheet);

            data.MatTwoInSheet = new MatrixInSheet
            {
                SelectedOption = easyCalcModel.MatrixTwoIn.SelectedOption,

                C11 = easyCalcModel.MatrixTwoIn.C11,
                C12 = easyCalcModel.MatrixTwoIn.C12,
                C13 = easyCalcModel.MatrixTwoIn.C13,
                C14 = easyCalcModel.MatrixTwoIn.C14,
                //C15 = easyCalcModel.MatrixTwoIn.C15,
                C21 = easyCalcModel.MatrixTwoIn.C21,
                C22 = easyCalcModel.MatrixTwoIn.C22,
                C23 = easyCalcModel.MatrixTwoIn.C23,
                C24 = easyCalcModel.MatrixTwoIn.C24,
                //C25 = easyCalcModel.MatrixTwoIn.C25,
                D21 = easyCalcModel.MatrixTwoIn.D21,
                D22 = easyCalcModel.MatrixTwoIn.D22,
                D23 = easyCalcModel.MatrixTwoIn.D23,
                D24 = easyCalcModel.MatrixTwoIn.D24,
                //D25 = easyCalcModel.MatrixTwoIn.D25,
                E11 = easyCalcModel.MatrixTwoIn.E11,
                E12 = easyCalcModel.MatrixTwoIn.E12,
                E13 = easyCalcModel.MatrixTwoIn.E13,
                E14 = easyCalcModel.MatrixTwoIn.E14,
                //E15 = easyCalcModel.MatrixTwoIn.E15,
                E21 = easyCalcModel.MatrixTwoIn.E21,
                E22 = easyCalcModel.MatrixTwoIn.E22,
                E23 = easyCalcModel.MatrixTwoIn.E23,
                E24 = easyCalcModel.MatrixTwoIn.E24,
                //E25 = easyCalcModel.MatrixTwoIn.E25,
                F11 = easyCalcModel.MatrixTwoIn.F11,
                F12 = easyCalcModel.MatrixTwoIn.F12,
                F13 = easyCalcModel.MatrixTwoIn.F13,
                F14 = easyCalcModel.MatrixTwoIn.F14,
                //F15 = easyCalcModel.MatrixTwoIn.F15,
                F21 = easyCalcModel.MatrixTwoIn.F21,
                F22 = easyCalcModel.MatrixTwoIn.F22,
                F23 = easyCalcModel.MatrixTwoIn.F23,
                F24 = easyCalcModel.MatrixTwoIn.F24,
                //F25 = easyCalcModel.MatrixTwoIn.F25,
                G11 = easyCalcModel.MatrixTwoIn.G11,
                G12 = easyCalcModel.MatrixTwoIn.G12,
                G13 = easyCalcModel.MatrixTwoIn.G13,
                G14 = easyCalcModel.MatrixTwoIn.G14,
                //G15 = easyCalcModel.MatrixTwoIn.G15,
                G21 = easyCalcModel.MatrixTwoIn.G21,
                G22 = easyCalcModel.MatrixTwoIn.G22,
                G23 = easyCalcModel.MatrixTwoIn.G23,
                G24 = easyCalcModel.MatrixTwoIn.G24,
                //G25 = easyCalcModel.MatrixTwoIn.G25,
                H11 = easyCalcModel.MatrixTwoIn.H11,
                H12 = easyCalcModel.MatrixTwoIn.H12,
                H13 = easyCalcModel.MatrixTwoIn.H13,
                H14 = easyCalcModel.MatrixTwoIn.H14,
                //H15 = easyCalcModel.MatrixTwoIn.H15,
                H21 = easyCalcModel.MatrixTwoIn.H21,
                H22 = easyCalcModel.MatrixTwoIn.H22,
                H23 = easyCalcModel.MatrixTwoIn.H23,
                H24 = easyCalcModel.MatrixTwoIn.H24,
                //H25 = easyCalcModel.MatrixTwoIn.H25,
            };
            data.MatTwoOutSheet = new MatrixOutSheet(data.MatTwoInSheet);

            return data;
        }


        private static SysInputModel GetSysInputModel(SystemInputSheet sheet)
        {
            SysInputModel model = new SysInputModel
            {
                SysInput_ErrorMarg_F72 = sheet.ErrorMargin_F72,
                SysInput_Min_D75 = sheet.Min_D75,
                SysInput_Max_D77 = sheet.Max_D77,
                SysInput_BestEstimate_D79 = sheet.SystemInputVolume_D79,
            };

            return model;
        }
        private static UnauthConsModel GetUnauthConsModel(UnauthorizedConsumptionSheet sheet)
        {
            UnauthConsModel model = new UnauthConsModel
            {
                UnauthCons_Total_L6 = sheet.IllegalConnectionsDomesticConsumptionTotalM3_L6,
                UnauthCons_Total_L10 = sheet.IllegalConnectionsOthersConsumptionTotalM3_L10,
                UnauthCons_Total_L14 = sheet.MeterTamperingBypassesEtcConsumptionTotalM3_L14,
                UnauthCons_ErrorMarg_F24 = sheet.ErrorMargin_F24,
                UnauthCons_Min_L27 = sheet.Min_L27,
                UnauthCons_Max_L29 = sheet.Max_L29,
                UnauthCons_BestEstimate_L31 = sheet.BestEstimateTotal_L31,
                UnauthCons_Total_L18 = sheet.Total_L18_L22[0],
                UnauthCons_Total_L19 = sheet.Total_L18_L22[1],
                UnauthCons_Total_L20 = sheet.Total_L18_L22[2],
                UnauthCons_Total_L21 = sheet.Total_L18_L22[3],
            };

            return model;
        }
        private static MetErrorsModel GetMetErrorsModel(MeterErrorsSheet sheet)
        {
            MetErrorsModel model = new MetErrorsModel
            {
                MetErrors_Total_F8 = sheet.BilledMeteredConsumptionWithoutBulkSupplyTotalM3_F8,
                MetErrors_Total_F32 = sheet.MeteredBulkSupplyExportTotalM3_F32,
                MetErrors_Total_F34 = sheet.UnbilledMeteredConsumptionWithoutBulkSupplyM3_F34,
                MetErrors_Total_F38 = sheet.CorruptMeterReadingPracticesTotalM3_F38,
                MetErrors_Total_L8 = sheet.BilledMeteredConsumptionWithoutBulkSupplyTotalM3_L8,
                MetErrors_Total_L32 = sheet.MeteredBulkSupplyExportTotalWithMeterUnderregistrationM3_L32,
                MetErrors_Total_L34 = sheet.UnbilledMeteredConsumptionWithoutBulkSupplyWithMeterUnderregistration_L34,
                MetErrors_Total_L38 = sheet.CorruptMeterReadingPracticesWithMeterUnderregistrationM3_L38,
                MetErrors_ErrorMarg_N42 = sheet.ErrorMarginTotal_N42,
                MetErrors_Min_L45 = sheet.Min_L45,
                MetErrors_Max_L47 = sheet.Max_L47,
                MetErrors_BestEstimate_L49 = sheet.BestEstimateTotalM3_L49,
                MetErrors_Total_L12 = sheet.BilledMeteredConsumptionManuallyEnteredWithMeterUnderregistrationM3_L12_L28[0],
                MetErrors_Total_L13 = sheet.BilledMeteredConsumptionManuallyEnteredWithMeterUnderregistrationM3_L12_L28[1],
                MetErrors_Total_L14 = sheet.BilledMeteredConsumptionManuallyEnteredWithMeterUnderregistrationM3_L12_L28[2],
                MetErrors_Total_L15 = sheet.BilledMeteredConsumptionManuallyEnteredWithMeterUnderregistrationM3_L12_L28[3],
            };

            return model;
        }
        private static NetworkModel GetNetworkModel(NetworkSheet sheet)
        {
            NetworkModel model = new NetworkModel
            {
                Network_Total_D28 = sheet.DistributionAndTransmissionMainsTotalKm_D28,
                //Network_Total_D32 = Network_Total_D32,
                Network_Min_D39 = sheet.DistributionAndTransmissionMainsMinimum_D33,
                Network_Max_D41 = sheet.DistributionAndTransmissionMainsMaximum_D35,
                Network_BestEstimate_D43 = sheet.DistributionAndTransmissionMainsBestEstimate_D37,
                Network_Number_H21 = sheet.EstimatedNumberOfIllegalConnections_H21,
                Network_ErrorMarg_J21 = sheet.Network_ErrorMarg_J21,
                Network_ErrorMarg_J24 = sheet.Network_ErrorMarg_J24,
                Network_Min_H26 = sheet.Network_Min_H26,
                Network_Max_H28 = sheet.Network_Max_H28,
                Network_BestEstimate_H30 = sheet.ServiceConnectionsBestEstimate_H30,
                Network_Number_H39 = sheet.LenOfServConnFromBoundToMeterKm_H39,
                Network_ErrorMarg_J39 = sheet.Network_ErrorMarg_J39,
            };

            return model;
        }
        private static PressureModel GetPressureModel(PressureSheet sheet)
        {
            PressureModel model = new PressureModel
            {
                Prs_Min_F29 = sheet.Prs_Min_F29,
                Prs_Max_F31 = sheet.Prs_Max_F31,
                Prs_BestEstimate_F33 = sheet.AveragePressureBestEstimate_F33,
            };

            return model;
        }
        private static IntermModel GetIntermModel(IntermittentSupplySheet sheet)
        {
            IntermModel model = new IntermModel
            {
                Interm_BestEstimate_H33 = sheet.SupplyTimeBestEstimate_H33,
                Interm_Min_H29 = sheet.Interm_Min_F29,
                Interm_Max_H31 = sheet.Interm_Max_F31,
            };

            return model;
        }
        private static FinancDataModel GetFinancDataModel(FinancialDataSheet sheet)
        {
            FinancDataModel model = new FinancDataModel
            {
                FinancData_G13 = sheet.FinancData_G13,
                FinancData_G15 = sheet.FinancData_G15,
                FinancData_G17 = sheet.FinancData_G17,
                FinancData_G19 = sheet.FinancData_G19,
                FinancData_G20 = sheet.FinancData_G20,
                FinancData_G22 = sheet.FinancData_G22,
                FinancData_D24 = sheet.FinancData_D24,
                FinancData_G31 = sheet.FinancData_G31,
                FinancData_K8 =  sheet.FinancData_K8,
                FinancData_K13 = sheet.FinancData_K13,
                FinancData_K15 = sheet.FinancData_K15,
                FinancData_K17 = sheet.FinancData_K17,
                FinancData_K19 = sheet.FinancData_K19,
                FinancData_K20 = sheet.FinancData_K20,
                FinancData_K22 = sheet.FinancData_K22,
                FinancData_K31 = sheet.FinancData_K31,
                FinancData_K35 = sheet.FinancData_K35,
            };

            return model;
        }
    }
}
