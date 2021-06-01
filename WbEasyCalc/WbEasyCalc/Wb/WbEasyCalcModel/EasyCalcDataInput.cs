using System;

namespace WbEasyCalcModel
{
    public class EasyCalcDataInput : ICloneable
    {
        public int Start_PeriodDays_M21 { get; set; }

        public string SysInput_Desc_B6 { get; set; }
        public string SysInput_Desc_B7 { get; set; }
        public string SysInput_Desc_B8 { get; set; }
        public string SysInput_Desc_B9 { get; set; }
        public double SysInput_SystemInputVolumeM3_D6 { get; set; }
        public double SysInput_SystemInputVolumeM3_D7 { get; set; }
        public double SysInput_SystemInputVolumeM3_D8 { get; set; }
        public double SysInput_SystemInputVolumeM3_D9 { get; set; }

        public double SysInput_SystemInputVolumeError_F6 { get; set; }
        public double SysInput_SystemInputVolumeError_F7 { get; set; }
        public double SysInput_SystemInputVolumeError_F8 { get; set; }
        public double SysInput_SystemInputVolumeError_F9 { get; set; }

        public string BilledCons_Desc_B8 { get; set; }
        public string BilledCons_Desc_B9 { get; set; }
        public string BilledCons_Desc_B10 { get; set; }
        public string BilledCons_Desc_B11 { get; set; }
        public string BilledCons_Desc_F8 { get; set; }
        public string BilledCons_Desc_F9 { get; set; }
        public string BilledCons_Desc_F10 { get; set; }
        public string BilledCons_Desc_F11 { get; set; }

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


        public double BilledCons_BilledMetConsBulkWatSupExpM3_D6 { get; set; }
        public double BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 { get; set; }

        public double BilledCons_UnbMetConsM3_D8 { get; set; }
        public double BilledCons_UnbMetConsM3_D9 { get; set; }
        public double BilledCons_UnbMetConsM3_D10 { get; set; }
        public double BilledCons_UnbMetConsM3_D11 { get; set; }
        public double BilledCons_UnbUnmetConsM3_H8 { get; set; }
        public double BilledCons_UnbUnmetConsM3_H9 { get; set; }
        public double BilledCons_UnbUnmetConsM3_H10 { get; set; }
        public double BilledCons_UnbUnmetConsM3_H11 { get; set; }


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

        public double UnauthCons_OthersErrorMargin_F18 { get; set; }
        public double UnauthCons_OthersErrorMargin_F19 { get; set; }
        public double UnauthCons_OthersErrorMargin_F20 { get; set; }
        public double UnauthCons_OthersErrorMargin_F21 { get; set; }
        public double UnauthCons_OthersM3PerDay_J18 { get; set; }
        public double UnauthCons_OthersM3PerDay_J19 { get; set; }
        public double UnauthCons_OthersM3PerDay_J20 { get; set; }
        public double UnauthCons_OthersM3PerDay_J21 { get; set; }

        public int UnauthCons_IllegalConnDomEstNo_D6 { get; set; }
        public double UnauthCons_IllegalConnDomPersPerHouse_H6 { get; set; }
        public double UnauthCons_IllegalConnDomConsLitPerPersDay_J6 { get; set; }
        public double UnauthCons_IllegalConnDomErrorMargin_F6 { get; set; }
        public double UnauthCons_IllegalConnOthersErrorMargin_F10 { get; set; }

        public double IllegalConnectionsOthersEstimatedNumber_D10 { get; set; }
        public double IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 { get; set; }

        public double UnauthCons_MeterTampBypEtcEstNo_D14 { get; set; }
        public double UnauthCons_MeterTampBypEtcErrorMargin_F14 { get; set; }
        public double UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 { get; set; }
        public bool   MetErrors_DetailedManualSpec_J6 { get; set; }
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

        public double FinancData_G6 { get; set; }
        public string FinancData_K6 { get; set; }
        public double FinancData_G8 { get; set; }
        public double FinancData_D26 { get; set; }
        public double FinancData_G35 { get; set; }

        public string Prs_Area_B7 { get; set; }
        public string Prs_Area_B8 { get; set; }
        public string Prs_Area_B9 { get; set; }
        public string Prs_Area_B10 { get; set; }
        //public double Prs_ApproxNoOfConn_D7 { get; set; }
        public double Prs_ApproxNoOfConn_D8 { get; set; }
        public double Prs_ApproxNoOfConn_D9 { get; set; }
        public double Prs_ApproxNoOfConn_D10 { get; set; }

        public double Prs_DailyAvgPrsM_F7 { get; set; }
        public double Prs_DailyAvgPrsM_F8 { get; set; }
        public double Prs_DailyAvgPrsM_F9 { get; set; }
        public double Prs_DailyAvgPrsM_F10 { get; set; }

        public double Prs_ErrorMarg_F26 { get; set; }

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
        public double Interm_ErrorMarg_H26 { get; set; }

        public int MatOneIn_SelectedOption { get; set; }
        public double MatOneIn_C11 { get; set; }
        public double MatOneIn_C12 { get; set; }
        public double MatOneIn_C13 { get; set; }
        public double MatOneIn_C14 { get; set; }
        public double MatOneIn_C15 { get; set; }
        public double MatOneIn_C21 { get; set; }
        public double MatOneIn_C22 { get; set; }
        public double MatOneIn_C23 { get; set; }
        public double MatOneIn_C24 { get; set; }
        public double MatOneIn_C25 { get; set; }
        public double MatOneIn_D21 { get; set; }
        public double MatOneIn_D22 { get; set; }
        public double MatOneIn_D23 { get; set; }
        public double MatOneIn_D24 { get; set; }
        public double MatOneIn_D25 { get; set; }
        public double MatOneIn_E11 { get; set; }
        public double MatOneIn_E12 { get; set; }
        public double MatOneIn_E13 { get; set; }
        public double MatOneIn_E14 { get; set; }
        public double MatOneIn_E15 { get; set; }
        public double MatOneIn_E21 { get; set; }
        public double MatOneIn_E22 { get; set; }
        public double MatOneIn_E23 { get; set; }
        public double MatOneIn_E24 { get; set; }
        public double MatOneIn_E25 { get; set; }
        public double MatOneIn_F11 { get; set; }
        public double MatOneIn_F12 { get; set; }
        public double MatOneIn_F13 { get; set; }
        public double MatOneIn_F14 { get; set; }
        public double MatOneIn_F15 { get; set; }
        public double MatOneIn_F21 { get; set; }
        public double MatOneIn_F22 { get; set; }
        public double MatOneIn_F23 { get; set; }
        public double MatOneIn_F24 { get; set; }
        public double MatOneIn_F25 { get; set; }
        public double MatOneIn_G11 { get; set; }
        public double MatOneIn_G12 { get; set; }
        public double MatOneIn_G13 { get; set; }
        public double MatOneIn_G14 { get; set; }
        public double MatOneIn_G15 { get; set; }
        public double MatOneIn_G21 { get; set; }
        public double MatOneIn_G22 { get; set; }
        public double MatOneIn_G23 { get; set; }
        public double MatOneIn_G24 { get; set; }
        public double MatOneIn_G25 { get; set; }
        public double MatOneIn_H11 { get; set; }
        public double MatOneIn_H12 { get; set; }
        public double MatOneIn_H13 { get; set; }
        public double MatOneIn_H14 { get; set; }
        public double MatOneIn_H15 { get; set; }
        public double MatOneIn_H21 { get; set; }
        public double MatOneIn_H22 { get; set; }
        public double MatOneIn_H23 { get; set; }
        public double MatOneIn_H24 { get; set; }
        public double MatOneIn_H25 { get; set; }



        public object Clone()
        {
            return new EasyCalcDataInput()
            {
                Start_PeriodDays_M21 = Start_PeriodDays_M21,

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


                BilledCons_Desc_B8 = BilledCons_Desc_B8,
                BilledCons_Desc_B9 = BilledCons_Desc_B9,
                BilledCons_Desc_B10 = BilledCons_Desc_B10,
                BilledCons_Desc_B11 = BilledCons_Desc_B11,
                BilledCons_Desc_F8 = BilledCons_Desc_F8,
                BilledCons_Desc_F9 = BilledCons_Desc_F9,
                BilledCons_Desc_F10 = BilledCons_Desc_F10,
                BilledCons_Desc_F11 = BilledCons_Desc_F11,
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
                UnauthCons_Desc_B18 = UnauthCons_Desc_B18,
                UnauthCons_Desc_B19 = UnauthCons_Desc_B19,
                UnauthCons_Desc_B20 = UnauthCons_Desc_B20,
                UnauthCons_Desc_B21 = UnauthCons_Desc_B21,
                MetErrors_Desc_D12 = MetErrors_Desc_D12,
                MetErrors_Desc_D13 = MetErrors_Desc_D13,
                MetErrors_Desc_D14 = MetErrors_Desc_D14,
                MetErrors_Desc_D15 = MetErrors_Desc_D15,
                Network_Desc_B7 = Network_Desc_B7,
                Network_Desc_B8 = Network_Desc_B8,
                Network_Desc_B9 = Network_Desc_B9,
                Network_Desc_B10 = Network_Desc_B10,
                Interm_Area_B7 = Interm_Area_B7,
                Interm_Area_B8 = Interm_Area_B8,
                Interm_Area_B9 = Interm_Area_B9,
                Interm_Area_B10 = Interm_Area_B10,


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

                UnauthCons_OthersErrorMargin_F18 = UnauthCons_OthersErrorMargin_F18,
                UnauthCons_OthersErrorMargin_F19 = UnauthCons_OthersErrorMargin_F19,
                UnauthCons_OthersErrorMargin_F20 = UnauthCons_OthersErrorMargin_F20,
                UnauthCons_OthersErrorMargin_F21 = UnauthCons_OthersErrorMargin_F21,
                UnauthCons_OthersM3PerDay_J18 = UnauthCons_OthersM3PerDay_J18,
                UnauthCons_OthersM3PerDay_J19 = UnauthCons_OthersM3PerDay_J19,
                UnauthCons_OthersM3PerDay_J20 = UnauthCons_OthersM3PerDay_J20,
                UnauthCons_OthersM3PerDay_J21 = UnauthCons_OthersM3PerDay_J21,

                UnauthCons_IllegalConnDomEstNo_D6 = UnauthCons_IllegalConnDomEstNo_D6,
                UnauthCons_IllegalConnDomPersPerHouse_H6 = UnauthCons_IllegalConnDomPersPerHouse_H6,
                UnauthCons_IllegalConnDomConsLitPerPersDay_J6 = UnauthCons_IllegalConnDomConsLitPerPersDay_J6,
                UnauthCons_IllegalConnDomErrorMargin_F6 = UnauthCons_IllegalConnDomErrorMargin_F6,
                UnauthCons_IllegalConnOthersErrorMargin_F10 = UnauthCons_IllegalConnOthersErrorMargin_F10,
                UnauthCons_MeterTampBypEtcEstNo_D14 = UnauthCons_MeterTampBypEtcEstNo_D14,
                UnauthCons_MeterTampBypEtcErrorMargin_F14 = UnauthCons_MeterTampBypEtcErrorMargin_F14,
                UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 = UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14,

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

                FinancData_G6 = FinancData_G6,
                FinancData_K6 = FinancData_K6,
                FinancData_G8 = FinancData_G8,
                FinancData_D26 = FinancData_D26,
                FinancData_G35 = FinancData_G35,

                Prs_Area_B7 = Prs_Area_B7,
                Prs_Area_B8 = Prs_Area_B8,
                Prs_Area_B9 = Prs_Area_B9,
                Prs_Area_B10 = Prs_Area_B10,
                //Prs_ApproxNoOfConn_D7 = Prs_ApproxNoOfConn_D7,
                Prs_DailyAvgPrsM_F7 = Prs_DailyAvgPrsM_F7,
                Prs_ApproxNoOfConn_D8 = Prs_ApproxNoOfConn_D8,
                Prs_DailyAvgPrsM_F8 = Prs_DailyAvgPrsM_F8,
                Prs_ApproxNoOfConn_D9 = Prs_ApproxNoOfConn_D9,
                Prs_DailyAvgPrsM_F9 = Prs_DailyAvgPrsM_F9,
                Prs_ApproxNoOfConn_D10 = Prs_ApproxNoOfConn_D10,
                Prs_DailyAvgPrsM_F10 = Prs_DailyAvgPrsM_F10,

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


                MatOneIn_SelectedOption = MatOneIn_SelectedOption,

                MatOneIn_C11 = MatOneIn_C11,
                MatOneIn_C12 = MatOneIn_C12,
                MatOneIn_C13 = MatOneIn_C13,
                MatOneIn_C14 = MatOneIn_C14,
                MatOneIn_C15 = MatOneIn_C15,
                MatOneIn_C21 = MatOneIn_C21,
                MatOneIn_C22 = MatOneIn_C22,
                MatOneIn_C23 = MatOneIn_C23,
                MatOneIn_C24 = MatOneIn_C24,
                MatOneIn_C25 = MatOneIn_C25,
                MatOneIn_D21 = MatOneIn_D21,
                MatOneIn_D22 = MatOneIn_D22,
                MatOneIn_D23 = MatOneIn_D23,
                MatOneIn_D24 = MatOneIn_D24,
                MatOneIn_D25 = MatOneIn_D25,
                MatOneIn_E11 = MatOneIn_E11,
                MatOneIn_E12 = MatOneIn_E12,
                MatOneIn_E13 = MatOneIn_E13,
                MatOneIn_E14 = MatOneIn_E14,
                MatOneIn_E15 = MatOneIn_E15,
                MatOneIn_E21 = MatOneIn_E21,
                MatOneIn_E22 = MatOneIn_E22,
                MatOneIn_E23 = MatOneIn_E23,
                MatOneIn_E24 = MatOneIn_E24,
                MatOneIn_E25 = MatOneIn_E25,
                MatOneIn_F11 = MatOneIn_F11,
                MatOneIn_F12 = MatOneIn_F12,
                MatOneIn_F13 = MatOneIn_F13,
                MatOneIn_F14 = MatOneIn_F14,
                MatOneIn_F15 = MatOneIn_F15,
                MatOneIn_F21 = MatOneIn_F21,
                MatOneIn_F22 = MatOneIn_F22,
                MatOneIn_F23 = MatOneIn_F23,
                MatOneIn_F24 = MatOneIn_F24,
                MatOneIn_F25 = MatOneIn_F25,
                MatOneIn_G11 = MatOneIn_G11,
                MatOneIn_G12 = MatOneIn_G12,
                MatOneIn_G13 = MatOneIn_G13,
                MatOneIn_G14 = MatOneIn_G14,
                MatOneIn_G21 = MatOneIn_G21,
                MatOneIn_G22 = MatOneIn_G22,
                MatOneIn_G23 = MatOneIn_G23,
                MatOneIn_G24 = MatOneIn_G24,
                MatOneIn_G25 = MatOneIn_G25,
                MatOneIn_H11 = MatOneIn_H11,
                MatOneIn_H12 = MatOneIn_H12,
                MatOneIn_H13 = MatOneIn_H13,
                MatOneIn_H14 = MatOneIn_H14,
                MatOneIn_H15 = MatOneIn_H15,
                MatOneIn_H21 = MatOneIn_H21,
                MatOneIn_H22 = MatOneIn_H22,
                MatOneIn_H23 = MatOneIn_H23,
                MatOneIn_H24 = MatOneIn_H24,
                MatOneIn_H25 = MatOneIn_H25,

            };
        }

    }
}
