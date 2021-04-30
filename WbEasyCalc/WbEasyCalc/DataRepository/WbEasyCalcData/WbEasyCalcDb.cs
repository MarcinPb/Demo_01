using System;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using DataModel;

namespace DataRepository.WbEasyCalcData
{
    public class WbEasyCalcDb
    {
        public int WbEasyCalcDataId { get; set; }

        public string CreateLogin { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyLogin { get; set; }
        public DateTime ModifyDate { get; set; }

        public int YearNo { get; set; }
        public int MonthNo { get; set; }
        public int ZoneId { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsArchive { get; set; }
        public bool IsAccepted { get; set; }

        // Start
        public int Start_PeriodDays_M21 { get; set; } 

        // SysInput
        public string SysInput_Desc_B6 { get; set; }
        public string SysInput_Desc_B7 { get; set; }
        public string SysInput_Desc_B8 { get; set; }
        public string SysInput_Desc_B9 { get; set; }
        public double SysInput_SystemInputVolumeM3_D6 { get; set; }
        public double SysInput_SystemInputVolumeError_F6 { get; set; }
        public double SysInput_SystemInputVolumeM3_D7 { get; set; }
        public double SysInput_SystemInputVolumeError_F7 { get; set; }
        public double SysInput_SystemInputVolumeM3_D8 { get; set; }
        public double SysInput_SystemInputVolumeError_F8 { get; set; }
        public double SysInput_SystemInputVolumeM3_D9 { get; set; }
        public double SysInput_SystemInputVolumeError_F9 { get; set; }

        // BilledCons
        public double BilledCons_BilledMetConsBulkWatSupExpM3_D6 { get; set; }
        public double BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 { get; set; }

        public string BilledCons_Desc_B8 { get; set; }
        public string BilledCons_Desc_B9 { get; set; }
        public string BilledCons_Desc_B10 { get; set; }
        public string BilledCons_Desc_B11 { get; set; }
        public string BilledCons_Desc_F8 { get; set; }
        public string BilledCons_Desc_F9 { get; set; }
        public string BilledCons_Desc_F10 { get; set; }
        public string BilledCons_Desc_F11 { get; set; }

        public double BilledCons_UnbMetConsM3_D8 { get; set; }
        public double BilledCons_UnbMetConsM3_D9 { get; set; }
        public double BilledCons_UnbMetConsM3_D10 { get; set; }
        public double BilledCons_UnbMetConsM3_D11 { get; set; }
        public double BilledCons_UnbUnmetConsM3_H8 { get; set; }
        public double BilledCons_UnbUnmetConsM3_H9 { get; set; }
        public double BilledCons_UnbUnmetConsM3_H10 { get; set; }
        public double BilledCons_UnbUnmetConsM3_H11 { get; set; }

        public double BilledCons_Sum_D28 { get; set; }
        public double BilledCons_Sum_H28 { get; set; }



        public string UnbilledCons_Desc_D8 { get; set; }
        public string UnbilledCons_Desc_D9 { get; set; }
        public string UnbilledCons_Desc_D10 { get; set; }
        public string UnbilledCons_Desc_D11 { get; set; }
        public string UnbilledCons_Desc_F6 { get; set; }
        public string UnbilledCons_Desc_F7 { get; set; }
        public string UnbilledCons_Desc_F8 { get; set; }
        public string UnbilledCons_Desc_F9 { get; set; }
        public string UnbilledCons_Desc_F10 { get; set; }
        public string UnbilledCons_Desc_F11 { get; set; }
        
        public string UnauthCons_Desc_B18 { get; set; }
        public string UnauthCons_Desc_B19 { get; set; }
        public string UnauthCons_Desc_B20 { get; set; }
        public string UnauthCons_Desc_B21 { get; set; }
        
        public string MetErrors_Desc_D12 { get; set; }
        public string MetErrors_Desc_D13 { get; set; }
        public string MetErrors_Desc_D14 { get; set; }
        public string MetErrors_Desc_D15 { get; set; }
        
        public string Network_Desc_B7 { get; set; }
        public string Network_Desc_B8 { get; set; }
        public string Network_Desc_B9 { get; set; }
        public string Network_Desc_B10 { get; set; }
        
        public string Interm_Area_B7 { get; set; }
        public string Interm_Area_B8 { get; set; }
        public string Interm_Area_B9 { get; set; }
        public string Interm_Area_B10 { get; set; }
    



        public double UnbilledCons_MetConsBulkWatSupExpM3_D6 { get; set; }

        public double UnbilledCons_UnbMetConsM3_D8 { get; set; }
        public double UnbilledCons_UnbMetConsM3_D9 { get; set; }
        public double UnbilledCons_UnbMetConsM3_D10 { get; set; }
        public double UnbilledCons_UnbMetConsM3_D11 { get; set; }
        public double UnbilledCons_UnbUnmetConsM3_H6 { get; set; }
        public double UnbilledCons_UnbUnmetConsM3_H7 { get; set; }
        public double UnbilledCons_UnbUnmetConsM3_H8 { get; set; }
        public double UnbilledCons_UnbUnmetConsM3_H9 { get; set; }
        public double UnbilledCons_UnbUnmetConsM3_H10 { get; set; }
        public double UnbilledCons_UnbUnmetConsM3_H11 { get; set; }
        public double UnbilledCons_UnbUnmetConsError_J6 { get; set; }
        public double UnbilledCons_UnbUnmetConsError_J7 { get; set; }
        public double UnbilledCons_UnbUnmetConsError_J8 { get; set; }
        public double UnbilledCons_UnbUnmetConsError_J9 { get; set; }
        public double UnbilledCons_UnbUnmetConsError_J10 { get; set; }
        public double UnbilledCons_UnbUnmetConsError_J11 { get; set; }


        public int UnauthCons_IllegalConnDomEstNo_D6 { get; set; }                      //        
        public double UnauthCons_IllegalConnDomPersPerHouse_H6 { get; set; }
        public double UnauthCons_IllegalConnDomConsLitPerPersDay_J6 { get; set; }
        public double UnauthCons_IllegalConnDomErrorMargin_F6 { get; set; }
        public double UnauthCons_IllegalConnOthersErrorMargin_F10 { get; set; }
        public double IllegalConnectionsOthersEstimatedNumber_D10 { get; set; }
        public double IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 { get; set; }
        public double UnauthCons_MeterTampBypEtcEstNo_D14 { get; set; }
        public double UnauthCons_MeterTampBypEtcErrorMargin_F14 { get; set; }
        public double UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 { get; set; }

        public double UnauthCons_OthersErrorMargin_F18 { get; set; }
        public double UnauthCons_OthersErrorMargin_F19 { get; set; }
        public double UnauthCons_OthersErrorMargin_F20 { get; set; }
        public double UnauthCons_OthersErrorMargin_F21 { get; set; }
        public double UnauthCons_OthersM3PerDay_J18 { get; set; }
        public double UnauthCons_OthersM3PerDay_J19 { get; set; }
        public double UnauthCons_OthersM3PerDay_J20 { get; set; }
        public double UnauthCons_OthersM3PerDay_J21 { get; set; }


        public int MetErrors_DetailedManualSpec_J6 { get; set; }                    
        public double MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 { get; set; }
        public double MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 { get; set; }

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



        public double Network_DistributionAndTransmissionMains_D7 { get; set; }
        public double Network_DistributionAndTransmissionMains_D8 { get; set; }
        public double Network_DistributionAndTransmissionMains_D9 { get; set; }
        public double Network_DistributionAndTransmissionMains_D10 { get; set; }

        public double Network_NoOfConnOfRegCustomers_H10 { get; set; }
        public double Network_NoOfInactAccountsWSvcConns_H18 { get; set; }
        public double Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 { get; set; }

        public double Network_PossibleUnd_D30 { get; set; }
        public double Network_NoCustomers_H7 { get; set; }
        public double Network_ErrorMargin_J7 { get; set; }
        public double Network_ErrorMargin_J10 { get; set; }
        public double Network_ErrorMargin_J18 { get; set; }
        public double Network_ErrorMargin_J32 { get; set; }
        public double Network_ErrorMargin_D35 { get; set; }

        public double Network_Total_D28 { get; set; }
        public double Network_Total_D32 { get; set; }
        public double Network_Min_D39 { get; set; }
        public double Network_Max_D41 { get; set; }
        public double Network_BestEstimate_D43 { get; set; }
        public double Network_Number_H21 { get; set; }
        public double Network_ErrorMarg_J21 { get; set; }
        public double Network_ErrorMarg_J24 { get; set; }
        public double Network_Min_H26 { get; set; }
        public double Network_Max_H28 { get; set; }
        public double Network_BestEstimate_H30 { get; set; }
        public double Network_Number_H39 { get; set; }
        public double Network_ErrorMarg_J39 { get; set; }

        public double FinancData_G6 { get; set; }
        public string FinancData_K6 { get; set; }
        public double FinancData_G8 { get; set; }
        public double FinancData_D26 { get; set; }
        public double FinancData_G35 { get; set; }


        public string Prs_Area_B7 { get; set; }
        public double Prs_ApproxNoOfConn_D7 { get; set; }
        public double Prs_DailyAvgPrsM_F7 { get; set; }
        public string Prs_Area_B8 { get; set; }
        public double Prs_ApproxNoOfConn_D8 { get; set; }
        public double Prs_DailyAvgPrsM_F8 { get; set; }
        public string Prs_Area_B9 { get; set; }
        public double Prs_ApproxNoOfConn_D9 { get; set; }
        public double Prs_DailyAvgPrsM_F9 { get; set; }
        public string Prs_Area_B10 { get; set; }
        public double Prs_ApproxNoOfConn_D10 { get; set; }
        public double Prs_DailyAvgPrsM_F10 { get; set; }
        public double Prs_ErrorMarg_F26 { get; set; }
        public double Prs_Min_F29 { get; set; }
        public double Prs_Max_F31 { get; set; }
        public double Prs_BestEstimate_F33 { get; set; }

        public double SysInput_ErrorMarg_F72 { get; set; }
        public double SysInput_Min_D75 { get; set; }
        public double SysInput_Max_D77 { get; set; }
        public double SysInput_BestEstimate_D79 { get; set; }



        
        public double UnbilledCons_Sum_D32 { get; set; }
        public double UnbilledCons_ErrorMarg_J25 { get; set; }
        public double UnbilledCons_Min_H28 { get; set; }
        public double UnbilledCons_Max_H30 { get; set; }
        public double UnbilledCons_BestEstimate_H32 { get; set; }

        public double UnauthCons_Total_L6 { get; set; }
        public double UnauthCons_Total_L10 { get; set; }
        public double UnauthCons_Total_L14 { get; set; }
        public double UnauthCons_ErrorMarg_F24 { get; set; }
        public double UnauthCons_Min_L27 { get; set; }
        public double UnauthCons_Max_L29 { get; set; }
        public double UnauthCons_BestEstimate_L31 { get; set; }

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

        public double UnauthCons_Total_L18 { get; set; }
        public double UnauthCons_Total_L19 { get; set; }
        public double UnauthCons_Total_L20 { get; set; }
        public double UnauthCons_Total_L21 { get; set; }


        public double Interm_ErrorMarg_H26 { get; set; }
        public double Interm_Min_H29 { get; set; }
        public double Interm_Max_H31 { get; set; }
        public double Interm_BestEstimate_H33 { get; set; }

        public double Interm_Conn_D7 { get; set; }
        public double Interm_Conn_D8 { get; set; }
        public double Interm_Conn_D9 { get; set; }
        public double Interm_Conn_D10 { get; set; }
        public double Interm_Days_F7 { get; set; }
        public double Interm_Days_F8 { get; set; }
        public double Interm_Days_F9 { get; set; }
        public double Interm_Days_F10 { get; set; }
        public double Interm_Hour_H7 { get; set; }
        public double Interm_Hour_H8 { get; set; }
        public double Interm_Hour_H9 { get; set; }
        public double Interm_Hour_H10 { get; set; }


        public double FinancData_G13 { get; set; }
        public double FinancData_G15 { get; set; }
        public double FinancData_G17 { get; set; }
        public double FinancData_G19 { get; set; }
        public double FinancData_G20 { get; set; }
        public double FinancData_G22 { get; set; }
        public double FinancData_D24 { get; set; }
        public double FinancData_G31 { get; set; }
        public string FinancData_K8  { get; set; }
        public string FinancData_K13 { get; set; }
        public string FinancData_K15 { get; set; }
        public string FinancData_K17 { get; set; }
        public string FinancData_K19 { get; set; }
        public string FinancData_K20 { get; set; }
        public string FinancData_K22 { get; set; }
        public string FinancData_K31 { get; set; }
        public string FinancData_K35 { get; set; }


        public int MatrixOneIn_SelectedOption { get; set; }
        public double MatrixOneIn_C11 { get; set; }
        public double MatrixOneIn_C12 { get; set; }
        public double MatrixOneIn_C13 { get; set; }
        public double MatrixOneIn_C14 { get; set; }
        public double MatrixOneIn_C21 { get; set; }
        public double MatrixOneIn_C22 { get; set; }
        public double MatrixOneIn_C23 { get; set; }
        public double MatrixOneIn_C24 { get; set; }
        public double MatrixOneIn_D21 { get; set; }
        public double MatrixOneIn_D22 { get; set; }
        public double MatrixOneIn_D23 { get; set; }
        public double MatrixOneIn_D24 { get; set; }
        public double MatrixOneIn_E11 { get; set; }
        public double MatrixOneIn_E12 { get; set; }
        public double MatrixOneIn_E13 { get; set; }
        public double MatrixOneIn_E14 { get; set; }
        public double MatrixOneIn_E21 { get; set; }
        public double MatrixOneIn_E22 { get; set; }
        public double MatrixOneIn_E23 { get; set; }
        public double MatrixOneIn_E24 { get; set; }
        public double MatrixOneIn_F11 { get; set; }
        public double MatrixOneIn_F12 { get; set; }
        public double MatrixOneIn_F13 { get; set; }
        public double MatrixOneIn_F14 { get; set; }
        public double MatrixOneIn_F21 { get; set; }
        public double MatrixOneIn_F22 { get; set; }
        public double MatrixOneIn_F23 { get; set; }
        public double MatrixOneIn_F24 { get; set; }
        public double MatrixOneIn_G11 { get; set; }
        public double MatrixOneIn_G12 { get; set; }
        public double MatrixOneIn_G13 { get; set; }
        public double MatrixOneIn_G14 { get; set; }
        public double MatrixOneIn_G21 { get; set; }
        public double MatrixOneIn_G22 { get; set; }
        public double MatrixOneIn_G23 { get; set; }
        public double MatrixOneIn_G24 { get; set; }
        public double MatrixOneIn_H11 { get; set; }
        public double MatrixOneIn_H12 { get; set; }
        public double MatrixOneIn_H13 { get; set; }
        public double MatrixOneIn_H14 { get; set; }
        public double MatrixOneIn_H21 { get; set; }
        public double MatrixOneIn_H22 { get; set; }
        public double MatrixOneIn_H23 { get; set; }
        public double MatrixOneIn_H24 { get; set; }


        public double Pis_F9 { get; set; }
        public double Pis_H9 { get; set; }
        public double Pis_J9 { get; set; }
        public double Pis_L9 { get; set; }
        public double Pis_F11 { get; set; }
        public double Pis_H11 { get; set; }
        public double Pis_J11 { get; set; }
        public double Pis_L11 { get; set; }
        public double Pis_F17 { get; set; }
        public double Pis_H17 { get; set; }
        public double Pis_J17 { get; set; }
        public double Pis_L17 { get; set; }
        public double Pis_F19 { get; set; }
        public double Pis_H19 { get; set; }
        public double Pis_J19 { get; set; }
        public double Pis_L19 { get; set; }
        public double Pis_F25 { get; set; }
        public double Pis_H25 { get; set; }
        public double Pis_J25 { get; set; }
        public double Pis_L25 { get; set; }
        public double Pis_F27 { get; set; }
        public double Pis_H27 { get; set; }
        public double Pis_J27 { get; set; }
        public double Pis_L27 { get; set; }
        public double Pis_F29 { get; set; }
        public double Pis_H29 { get; set; }
        public double Pis_J29 { get; set; }
        public double Pis_L29 { get; set; }
        public double Pis_F31 { get; set; }
        public double Pis_H31 { get; set; }
        public double Pis_J31 { get; set; }
        public double Pis_L31 { get; set; }
        public double Pis_F37 { get; set; }
        public double Pis_H37 { get; set; }
        public double Pis_J37 { get; set; }
        public double Pis_L37 { get; set; }
        public double Pis_F39 { get; set; }
        public double Pis_H39 { get; set; }
        public double Pis_J39 { get; set; }
        public double Pis_L39 { get; set; }
        public double Pis_F41 { get; set; }
        public double Pis_H41 { get; set; }
        public double Pis_J41 { get; set; }
        public double Pis_L41 { get; set; }
        public double Pis_F47 { get; set; }
        public double Pis_H47 { get; set; }
        public double Pis_J47 { get; set; }
        public double Pis_L47 { get; set; }
        public double Pis_F49 { get; set; }
        public double Pis_H49 { get; set; }
        public double Pis_J49 { get; set; }
        public double Pis_L49 { get; set; }
        public double Pis_F51 { get; set; }
        public double Pis_H51 { get; set; }
        public double Pis_J51 { get; set; }
        public double Pis_L51 { get; set; }

        public string Pis_N27 { get; set; }
        public string Pis_P27 { get; set; }
        public string Pis_N47 { get; set; }
        public string Pis_P47 { get; set; }

        // output
        //public EasyCalcDataOutput EasyCalcDataOutput { get; set; }
        public double SystemInputVolume_B19 { get; set; }
        public double SystemInputVolumeErrorMargin_B21 { get; set; }
        public double AuthorizedConsumption_K12 { get; set; }
        public double AuthorizedConsumptionErrorMargin_K15 { get; set; }
        public double WaterLosses_K29 { get; set; }
        public double WaterLossesErrorMargin_K31 { get; set; }
        public double BilledAuthorizedConsumption_T8 { get; set; }
        public double UnbilledAuthorizedConsumption_T16 { get; set; }
        public double UnbilledAuthorizedConsumptionErrorMargin_T20 { get; set; }
        public double CommercialLosses_T26 { get; set; }
        public double CommercialLossesErrorMargin_T29 { get; set; }
        public double PhysicalLossesM3_T34 { get; set; }
        public double PhyscialLossesErrorMargin_AH35 { get; set; }
        public double BilledMeteredConsumption_AC4 { get; set; }
        public double BilledUnmeteredConsumption_AC9 { get; set; }
        public double UnbilledMeteredConsumption_AC14 { get; set; }
        public double UnbilledUnmeteredConsumption_AC19 { get; set; }
        public double UnbilledUnmeteredConsumptionErrorMargin_AO20 { get; set; }
        public double UnauthorizedConsumption_AC24 { get; set; }
        public double UnauthorizedConsumptionErrorMargin_AO25 { get; set; }
        public double CustomerMeterInaccuraciesAndErrorsM3_AC29 { get; set; }
        public double CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30 { get; set; }
        public double RevenueWaterM3_AY8 { get; set; }
        public double NonRevenueWaterM3_AY24 { get; set; }
        public double NonRevenueWaterErrorMargin_AY26 { get; set; }


        public DataModel.WbEasyCalcData GetWbEasyCalcData()
        {
            return new DataModel.WbEasyCalcData()
            {
                WbEasyCalcDataId = WbEasyCalcDataId,
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

                EasyCalcModel = new EasyCalcModel()
                {
                    StartModel = new StartModel
                    {
                        Start_PeriodDays_M21 = Start_PeriodDays_M21,
                    },

                    SysInputModel = new SysInputModel
                    {
                        SysInput_Desc_B6 = SysInput_Desc_B6,
                        SysInput_Desc_B7 = SysInput_Desc_B7,
                        SysInput_Desc_B8 = SysInput_Desc_B8,
                        SysInput_Desc_B9 = SysInput_Desc_B9,
                        SysInput_SystemInputVolumeM3_D6 = SysInput_SystemInputVolumeM3_D6,
                        SysInput_SystemInputVolumeError_F6 = SysInput_SystemInputVolumeError_F6,
                        SysInput_SystemInputVolumeM3_D7 = SysInput_SystemInputVolumeM3_D7,
                        SysInput_SystemInputVolumeError_F7 = SysInput_SystemInputVolumeError_F7,
                        SysInput_SystemInputVolumeM3_D8 = SysInput_SystemInputVolumeM3_D8,
                        SysInput_SystemInputVolumeError_F8 = SysInput_SystemInputVolumeError_F8,
                        SysInput_SystemInputVolumeM3_D9 = SysInput_SystemInputVolumeM3_D9,
                        SysInput_SystemInputVolumeError_F9 = SysInput_SystemInputVolumeError_F9,
                    },

                    BilledConsModel = new BilledConsModel
                    {
                        BilledCons_Desc_B8 = BilledCons_Desc_B8,
                        BilledCons_Desc_B9 = BilledCons_Desc_B9,
                        BilledCons_Desc_B10 = BilledCons_Desc_B10,
                        BilledCons_Desc_B11 = BilledCons_Desc_B11,
                        BilledCons_Desc_F8 = BilledCons_Desc_F8,
                        BilledCons_Desc_F9 = BilledCons_Desc_F9,
                        BilledCons_Desc_F10 = BilledCons_Desc_F10,
                        BilledCons_Desc_F11 = BilledCons_Desc_F11,

                        BilledCons_BilledMetConsBulkWatSupExpM3_D6 = BilledCons_BilledMetConsBulkWatSupExpM3_D6,
                        BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 = BilledCons_BilledUnmetConsBulkWatSupExpM3_H6,

                        BilledCons_UnbMetConsM3_D8 = BilledCons_UnbMetConsM3_D8,
                        BilledCons_UnbMetConsM3_D9 = BilledCons_UnbMetConsM3_D9,
                        BilledCons_UnbMetConsM3_D10 = BilledCons_UnbMetConsM3_D10,
                        BilledCons_UnbMetConsM3_D11 = BilledCons_UnbMetConsM3_D11,
                        BilledCons_UnbUnmetConsM3_H8 = BilledCons_UnbUnmetConsM3_H8,
                        BilledCons_UnbUnmetConsM3_H9 = BilledCons_UnbUnmetConsM3_H9,
                        BilledCons_UnbUnmetConsM3_H10 = BilledCons_UnbUnmetConsM3_H10,
                        BilledCons_UnbUnmetConsM3_H11 = BilledCons_UnbUnmetConsM3_H11,
                    },

                    UnbilledConsModel = new UnbilledConsModel
                    {
                        UnbilledCons_Desc_D8 = UnbilledCons_Desc_D8,
                        UnbilledCons_Desc_D9 = UnbilledCons_Desc_D9,
                        UnbilledCons_Desc_D10 = UnbilledCons_Desc_D10,
                        UnbilledCons_Desc_D11 = UnbilledCons_Desc_D11,
                        UnbilledCons_Desc_F6 = UnbilledCons_Desc_F6,
                        UnbilledCons_Desc_F7 = UnbilledCons_Desc_F7,
                        UnbilledCons_Desc_F8 = UnbilledCons_Desc_F8,
                        UnbilledCons_Desc_F9 = UnbilledCons_Desc_F9,
                        UnbilledCons_Desc_F10 = UnbilledCons_Desc_F10,
                        UnbilledCons_Desc_F11 = UnbilledCons_Desc_F11,

                        UnbilledCons_MetConsBulkWatSupExpM3_D6 = UnbilledCons_MetConsBulkWatSupExpM3_D6,

                        UnbilledCons_UnbMetConsM3_D8 = UnbilledCons_UnbMetConsM3_D8,
                        UnbilledCons_UnbMetConsM3_D9 = UnbilledCons_UnbMetConsM3_D9,
                        UnbilledCons_UnbMetConsM3_D10 = UnbilledCons_UnbMetConsM3_D10,
                        UnbilledCons_UnbMetConsM3_D11 = UnbilledCons_UnbMetConsM3_D11,
                        UnbilledCons_UnbUnmetConsM3_H6 = UnbilledCons_UnbUnmetConsM3_H6,
                        UnbilledCons_UnbUnmetConsM3_H7 = UnbilledCons_UnbUnmetConsM3_H7,
                        UnbilledCons_UnbUnmetConsM3_H8 = UnbilledCons_UnbUnmetConsM3_H8,
                        UnbilledCons_UnbUnmetConsM3_H9 = UnbilledCons_UnbUnmetConsM3_H9,
                        UnbilledCons_UnbUnmetConsM3_H10 = UnbilledCons_UnbUnmetConsM3_H10,
                        UnbilledCons_UnbUnmetConsM3_H11 = UnbilledCons_UnbUnmetConsM3_H11,
                        UnbilledCons_UnbUnmetConsError_J6 = UnbilledCons_UnbUnmetConsError_J6,
                        UnbilledCons_UnbUnmetConsError_J7 = UnbilledCons_UnbUnmetConsError_J7,
                        UnbilledCons_UnbUnmetConsError_J8 = UnbilledCons_UnbUnmetConsError_J8,
                        UnbilledCons_UnbUnmetConsError_J9 = UnbilledCons_UnbUnmetConsError_J9,
                        UnbilledCons_UnbUnmetConsError_J10 = UnbilledCons_UnbUnmetConsError_J10,
                        UnbilledCons_UnbUnmetConsError_J11 = UnbilledCons_UnbUnmetConsError_J11,


                    },

                    UnauthConsModel = new UnauthConsModel 
                    { 
                        UnauthCons_Desc_B18 = UnauthCons_Desc_B18,
                        UnauthCons_Desc_B19 = UnauthCons_Desc_B19,
                        UnauthCons_Desc_B20 = UnauthCons_Desc_B20,
                        UnauthCons_Desc_B21 = UnauthCons_Desc_B21,
                        UnauthCons_OthersErrorMargin_F18 = UnauthCons_OthersErrorMargin_F18,
                        UnauthCons_OthersErrorMargin_F19 = UnauthCons_OthersErrorMargin_F19,
                        UnauthCons_OthersErrorMargin_F20 = UnauthCons_OthersErrorMargin_F20,
                        UnauthCons_OthersErrorMargin_F21 = UnauthCons_OthersErrorMargin_F21,
                        UnauthCons_OthersM3PerDay_J18 = UnauthCons_OthersM3PerDay_J18,
                        UnauthCons_OthersM3PerDay_J19 = UnauthCons_OthersM3PerDay_J19,
                        UnauthCons_OthersM3PerDay_J20 = UnauthCons_OthersM3PerDay_J20,
                        UnauthCons_OthersM3PerDay_J21 = UnauthCons_OthersM3PerDay_J21,

                        UnauthCons_IllegalConnDomEstNo_D6 = UnauthCons_IllegalConnDomEstNo_D6,
                        UnauthCons_IllegalConnDomErrorMargin_F6 = UnauthCons_IllegalConnDomErrorMargin_F6,
                        UnauthCons_IllegalConnDomPersPerHouse_H6 = UnauthCons_IllegalConnDomPersPerHouse_H6,
                        UnauthCons_IllegalConnDomConsLitPerPersDay_J6 = UnauthCons_IllegalConnDomConsLitPerPersDay_J6,
                        UnauthCons_IllegalConnOthersErrorMargin_F10 = UnauthCons_IllegalConnOthersErrorMargin_F10,
                        IllegalConnectionsOthersEstimatedNumber_D10 = IllegalConnectionsOthersEstimatedNumber_D10,
                        IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10,
                        UnauthCons_MeterTampBypEtcEstNo_D14 = UnauthCons_MeterTampBypEtcEstNo_D14,
                        UnauthCons_MeterTampBypEtcErrorMargin_F14 = UnauthCons_MeterTampBypEtcErrorMargin_F14,
                        UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 = UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14,


                    },

                    MetErrorsModel = new MetErrorsModel
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

                    },

                    NetworkModel = new NetworkModel
                    {
                        Network_Desc_B7 = Network_Desc_B7,
                        Network_Desc_B8 = Network_Desc_B8,
                        Network_Desc_B9 = Network_Desc_B9,
                        Network_Desc_B10 = Network_Desc_B10,
                        Network_DistributionAndTransmissionMains_D7 = Network_DistributionAndTransmissionMains_D7,
                        Network_DistributionAndTransmissionMains_D8 = Network_DistributionAndTransmissionMains_D8,
                        Network_DistributionAndTransmissionMains_D9 = Network_DistributionAndTransmissionMains_D9,
                        Network_DistributionAndTransmissionMains_D10 = Network_DistributionAndTransmissionMains_D10,
                        Network_NoOfConnOfRegCustomers_H10 = Network_NoOfConnOfRegCustomers_H10,
                        Network_NoOfInactAccountsWSvcConns_H18 = Network_NoOfInactAccountsWSvcConns_H18,
                        Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 = Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32,

                        Network_PossibleUnd_D30 = Network_PossibleUnd_D30,
                        Network_NoCustomers_H7 = Network_NoCustomers_H7,
                        Network_ErrorMargin_J7 = Network_ErrorMargin_J7,
                        Network_ErrorMargin_J10 = Network_ErrorMargin_J10,
                        Network_ErrorMargin_J18 = Network_ErrorMargin_J18,
                        Network_ErrorMargin_J32 = Network_ErrorMargin_J32,
                        Network_ErrorMargin_D35 = Network_ErrorMargin_D35,


                    },

                    PressureModel = new PressureModel
                    {
                        Prs_Area_B7 = Prs_Area_B7,
                        Prs_Area_B8 = Prs_Area_B8,
                        Prs_Area_B9 = Prs_Area_B9,
                        Prs_Area_B10 = Prs_Area_B10,
                        Prs_ApproxNoOfConn_D7 = Prs_ApproxNoOfConn_D7,
                        Prs_DailyAvgPrsM_F7 = Prs_DailyAvgPrsM_F7,
                        Prs_ApproxNoOfConn_D8 = Prs_ApproxNoOfConn_D8,
                        Prs_DailyAvgPrsM_F8 = Prs_DailyAvgPrsM_F8,
                        Prs_ApproxNoOfConn_D9 = Prs_ApproxNoOfConn_D9,
                        Prs_DailyAvgPrsM_F9 = Prs_DailyAvgPrsM_F9,
                        Prs_ApproxNoOfConn_D10 = Prs_ApproxNoOfConn_D10,
                        Prs_DailyAvgPrsM_F10 = Prs_DailyAvgPrsM_F10,
                        Prs_ErrorMarg_F26 = Prs_ErrorMarg_F26,


                    },

                    IntermModel = new IntermModel
                    {
                        Interm_Area_B7 = Interm_Area_B7,
                        Interm_Area_B8 = Interm_Area_B8,
                        Interm_Area_B9 = Interm_Area_B9,
                        Interm_Area_B10 = Interm_Area_B10,
                        Interm_Conn_D7 = Interm_Conn_D7,
                        Interm_Conn_D8 = Interm_Conn_D8,
                        Interm_Conn_D9 = Interm_Conn_D9,
                        Interm_Conn_D10 = Interm_Conn_D10,
                        Interm_Days_F7 = Interm_Days_F7,
                        Interm_Days_F8 = Interm_Days_F8,
                        Interm_Days_F9 = Interm_Days_F9,
                        Interm_Days_F10 = Interm_Days_F10,
                        Interm_Hour_H7 = Interm_Hour_H7,
                        Interm_Hour_H8 = Interm_Hour_H8,
                        Interm_Hour_H9 = Interm_Hour_H9,
                        Interm_Hour_H10 = Interm_Hour_H10,
                        Interm_ErrorMarg_H26 = Interm_ErrorMarg_H26,

                    },

                    FinancDataModel = new FinancDataModel
                    {
                        FinancData_G6 = FinancData_G6,
                        FinancData_K6 = FinancData_K6,
                        FinancData_G8 = FinancData_G8,
                        FinancData_D26 = FinancData_D26,
                        FinancData_G35 = FinancData_G35,
                    },

                    MatrixOneIn = new MatrixOneInModel
                    {
                        SelectedOption = MatrixOneIn_SelectedOption,
                        C11 = MatrixOneIn_C11,
                        C12 = MatrixOneIn_C12,
                        C13 = MatrixOneIn_C13,
                        C14 = MatrixOneIn_C14,
                        C21 = MatrixOneIn_C21,
                        C22 = MatrixOneIn_C22,
                        C23 = MatrixOneIn_C23,
                        C24 = MatrixOneIn_C24,
                        D21 = MatrixOneIn_D21,
                        D22 = MatrixOneIn_D22,
                        D23 = MatrixOneIn_D23,
                        D24 = MatrixOneIn_D24,
                        E11 = MatrixOneIn_E11,
                        E12 = MatrixOneIn_E12,
                        E13 = MatrixOneIn_E13,
                        E14 = MatrixOneIn_E14,
                        E21 = MatrixOneIn_E21,
                        E22 = MatrixOneIn_E22,
                        E23 = MatrixOneIn_E23,
                        E24 = MatrixOneIn_E24,
                        F11 = MatrixOneIn_F11,
                        F12 = MatrixOneIn_F12,
                        F13 = MatrixOneIn_F13,
                        F14 = MatrixOneIn_F14,
                        F21 = MatrixOneIn_F21,
                        F22 = MatrixOneIn_F22,
                        F23 = MatrixOneIn_F23,
                        F24 = MatrixOneIn_F24,
                        G11 = MatrixOneIn_G11,
                        G12 = MatrixOneIn_G12,
                        G13 = MatrixOneIn_G13,
                        G14 = MatrixOneIn_G14,
                        G21 = MatrixOneIn_G21,
                        G22 = MatrixOneIn_G22,
                        G23 = MatrixOneIn_G23,
                        G24 = MatrixOneIn_G24,
                        H11 = MatrixOneIn_H11,
                        H12 = MatrixOneIn_H12,
                        H13 = MatrixOneIn_H13,
                        H14 = MatrixOneIn_H14,
                        H21 = MatrixOneIn_H21,
                        H22 = MatrixOneIn_H22,
                        H23 = MatrixOneIn_H23,
                        H24 = MatrixOneIn_H24,
                    },

                    WaterBalanceDay = new WaterBalanceModel(),
                    WaterBalancePeriod = new WaterBalanceModel()
                    { 
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
                    },
                    WaterBalanceYear = new WaterBalanceModel(),
                    Pis = new PisModel(),
                },
            };
        }
    }
}
