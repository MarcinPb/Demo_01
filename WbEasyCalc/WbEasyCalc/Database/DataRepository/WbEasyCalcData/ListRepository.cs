using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Database.DataRepository.WbEasyCalcData
{
    public class ListRepository : IListRepository
    {

        public List<Database.DataModel.WbEasyCalcData> GetList()
        {
            using (IDbConnection cnn = new SqlConnection(_cnnString))
            {
                string sql = "dbo.spWbEasyCalcDataList";
                var list = cnn.Query<WbEasyCalcDb>(sql, commandType: CommandType.StoredProcedure).ToList();
                var resultList = list.Select(x => x.GetWbEasyCalcData()).ToList();
                return resultList;
            }
        }

        public DataModel.WbEasyCalcData GetItem(int id)
        {
            using (IDbConnection connection = new SqlConnection(_cnnString))
            {
                var p = new DynamicParameters();

                p.Add("@id", id, dbType: DbType.Int32, direction: ParameterDirection.Input);

                p.Add("@ZoneId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@YearNo", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MonthNo", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Description", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@IsArchive", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                p.Add("@IsAccepted", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                p.Add("@Start_PeriodDays_M21", DbType.Int32, direction: ParameterDirection.Output);

                p.Add("@SysInput_Desc_B6", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@SysInput_Desc_B7", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@SysInput_Desc_B8", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@SysInput_Desc_B9", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeM3_D6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeError_F6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeM3_D7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeError_F7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeM3_D8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeError_F8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeM3_D9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeError_F9", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@BilledCons_Desc_B8", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@BilledCons_Desc_B9", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@BilledCons_Desc_B10", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@BilledCons_Desc_B11", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@BilledCons_Desc_F8", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@BilledCons_Desc_F9", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@BilledCons_Desc_F10", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@BilledCons_Desc_F11", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@BilledCons_BilledMetConsBulkWatSupExpM3_D6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@BilledCons_BilledUnmetConsBulkWatSupExpM3_H6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@BilledCons_UnbMetConsM3_D8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@BilledCons_UnbMetConsM3_D9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@BilledCons_UnbMetConsM3_D10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@BilledCons_UnbMetConsM3_D11", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@BilledCons_UnbUnmetConsM3_H8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@BilledCons_UnbUnmetConsM3_H9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@BilledCons_UnbUnmetConsM3_H10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@BilledCons_UnbUnmetConsM3_H11", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@UnbilledCons_Desc_D8", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_Desc_D9", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_Desc_D10", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_Desc_D11", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_Desc_F6", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_Desc_F7", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_Desc_F8", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_Desc_F9", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_Desc_F10", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_Desc_F11", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_MetConsBulkWatSupExpM3_D6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbMetConsM3_D8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbMetConsM3_D9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbMetConsM3_D10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbMetConsM3_D11", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbUnmetConsM3_H6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbUnmetConsM3_H7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbUnmetConsM3_H8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbUnmetConsM3_H9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbUnmetConsM3_H10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbUnmetConsM3_H11", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbUnmetConsError_J6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbUnmetConsError_J7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbUnmetConsError_J8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbUnmetConsError_J9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbUnmetConsError_J10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledCons_UnbUnmetConsError_J11", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@UnauthCons_Desc_B18", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_Desc_B19", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_Desc_B20", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_Desc_B21", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_OthersErrorMargin_F18", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_OthersErrorMargin_F19", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_OthersErrorMargin_F20", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_OthersErrorMargin_F21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_OthersM3PerDay_J18", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_OthersM3PerDay_J19", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_OthersM3PerDay_J20", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_OthersM3PerDay_J21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_IllegalConnDomEstNo_D6", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_IllegalConnDomPersPerHouse_H6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_IllegalConnDomConsLitPerPersDay_J6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_IllegalConnDomErrorMargin_F6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_IllegalConnOthersErrorMargin_F10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@IllegalConnectionsOthersEstimatedNumber_D10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_MeterTampBypEtcEstNo_D14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_MeterTampBypEtcErrorMargin_F14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@MetErrors_Desc_D12", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Desc_D13", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Desc_D14", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Desc_D15", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@MetErrors_DetailedManualSpec_J6", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_BilledMetConsWoBulkSupErrorMargin_N8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Total_F12", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Total_F13", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Total_F14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Total_F15", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Meter_H12", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Meter_H13", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Meter_H14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Meter_H15", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Error_N12", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Error_N13", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Error_N14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_Error_N15", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MeteredBulkSupplyExportErrorMargin_N32", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@CorruptMeterReadingPracticessErrorMargin_N38", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@DataHandlingErrorsOffice_L40", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@DataHandlingErrorsOfficeErrorMargin_N40", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_MetBulkSupExpMetUnderreg_H32", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MetErrors_CorruptMetReadPractMetUndrreg_H38", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@Network_Desc_B7", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@Network_Desc_B8", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@Network_Desc_B9", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@Network_Desc_B10", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@Network_DistributionAndTransmissionMains_D7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_DistributionAndTransmissionMains_D8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_DistributionAndTransmissionMains_D9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_DistributionAndTransmissionMains_D10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_NoOfConnOfRegCustomers_H10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_NoOfInactAccountsWSvcConns_H18", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_PossibleUnd_D30", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_NoCustomers_H7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_ErrorMargin_J7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_ErrorMargin_J10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_ErrorMargin_J18", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_ErrorMargin_J32", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_ErrorMargin_D35", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@Prs_Area_B7", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@Prs_Area_B8", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@Prs_Area_B9", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@Prs_Area_B10", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@Prs_ApproxNoOfConn_D7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_DailyAvgPrsM_F7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_ApproxNoOfConn_D8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_DailyAvgPrsM_F8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_ApproxNoOfConn_D9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_DailyAvgPrsM_F9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_ApproxNoOfConn_D10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_DailyAvgPrsM_F10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_ErrorMarg_F26", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@Interm_Area_B7", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@Interm_Area_B8", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@Interm_Area_B9", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@Interm_Area_B10", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@Interm_Conn_D7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Interm_Conn_D8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Interm_Conn_D9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Interm_Conn_D10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Interm_Days_F7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Interm_Days_F8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Interm_Days_F9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Interm_Days_F10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Interm_Hour_H7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Interm_Hour_H8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Interm_Hour_H9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Interm_Hour_H10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Interm_ErrorMarg_H26", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@FinancData_G6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@FinancData_K6", dbType: DbType.String, size: 400, direction: ParameterDirection.Output);
                p.Add("@FinancData_G8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@FinancData_D26", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@FinancData_G35", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@MatrixOneIn_SelectedOption", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_C11", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_C12", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_C13", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_C14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_C21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_C22", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_C23", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_C24", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_D21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_D22", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_D23", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_D24", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_E11", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_E12", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_E13", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_E14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_E21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_E22", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_E23", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_E24", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_F11", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_F12", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_F13", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_F14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_F21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_F22", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_F23", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_F24", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_G11", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_G12", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_G13", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_G14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_G21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_G22", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_G23", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_G24", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_H11", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_H12", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_H13", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_H14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_H21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_H22", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_H23", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixOneIn_H24", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@MatrixTwoIn_SelectedOption", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_D21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_D22", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_D23", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_D24", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_E11", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_E12", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_E13", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_E14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_E21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_E22", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_E23", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_E24", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_F11", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_F12", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_F13", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_F14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_F21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_F22", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_F23", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_F24", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_G11", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_G12", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_G13", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_G14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_G21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_G22", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_G23", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_G24", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_H11", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_H12", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_H13", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_H14", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_H21", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_H22", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_H23", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@MatrixTwoIn_H24", dbType: DbType.Double, direction: ParameterDirection.Output);


                connection.Execute("dbo.spWbEasyCalcDataGet", p, commandType: CommandType.StoredProcedure);

                DataModel.WbEasyCalcData model = new DataModel.WbEasyCalcData()
                {
                    WbEasyCalcDataId = id,

                    ZoneId = p.Get<int>("@ZoneId"),
                    YearNo = p.Get<int>("@YearNo"),
                    MonthNo = p.Get<int>("@MonthNo"),
                    Description = p.Get<string>("@Description"),
                    IsArchive = p.Get<bool>("@IsArchive"),
                    IsAccepted = p.Get<bool>("@IsAccepted"),

                    EasyCalcModel = new WbEasyCalcModel.EasyCalcModel()
                    {
                        StartModel = new WbEasyCalcModel.WbEasyCalc.StartModel
                        {
                            Start_PeriodDays_M21 = p.Get<int>("@Start_PeriodDays_M21"),
                        },
                        SysInputModel = new WbEasyCalcModel.WbEasyCalc.SysInputModel
                        {
                            SysInput_Desc_B6 = p.Get<string>("@SysInput_Desc_B6"),
                            SysInput_Desc_B7 = p.Get<string>("@SysInput_Desc_B7"),
                            SysInput_Desc_B8 = p.Get<string>("@SysInput_Desc_B8"),
                            SysInput_Desc_B9 = p.Get<string>("@SysInput_Desc_B9"),
                            SysInput_SystemInputVolumeM3_D6 = p.Get<double>("@SysInput_SystemInputVolumeM3_D6"),
                            SysInput_SystemInputVolumeError_F6 = p.Get<double>("@SysInput_SystemInputVolumeError_F6"),
                            SysInput_SystemInputVolumeM3_D7 = p.Get<double>("@SysInput_SystemInputVolumeM3_D7"),
                            SysInput_SystemInputVolumeError_F7 = p.Get<double>("@SysInput_SystemInputVolumeError_F7"),
                            SysInput_SystemInputVolumeM3_D8 = p.Get<double>("@SysInput_SystemInputVolumeM3_D8"),
                            SysInput_SystemInputVolumeError_F8 = p.Get<double>("@SysInput_SystemInputVolumeError_F8"),
                            SysInput_SystemInputVolumeM3_D9 = p.Get<double>("@SysInput_SystemInputVolumeM3_D9"),
                            SysInput_SystemInputVolumeError_F9 = p.Get<double>("@SysInput_SystemInputVolumeError_F9"),
                        },
                        BilledConsModel = new WbEasyCalcModel.WbEasyCalc.BilledConsModel
                        {
                            BilledCons_Desc_B8  = p.Get<string>("@BilledCons_Desc_B8"),
                            BilledCons_Desc_B9  = p.Get<string>("@BilledCons_Desc_B9"),
                            BilledCons_Desc_B10 = p.Get<string>("@BilledCons_Desc_B10"),
                            BilledCons_Desc_B11 = p.Get<string>("@BilledCons_Desc_B11"),
                            BilledCons_Desc_F8  = p.Get<string>("@BilledCons_Desc_F8"),
                            BilledCons_Desc_F9  = p.Get<string>("@BilledCons_Desc_F9"),
                            BilledCons_Desc_F10 = p.Get<string>("@BilledCons_Desc_F10"),
                            BilledCons_Desc_F11 = p.Get<string>("@BilledCons_Desc_F11"),
                            BilledCons_BilledMetConsBulkWatSupExpM3_D6 = p.Get<double>("@BilledCons_BilledMetConsBulkWatSupExpM3_D6"),              
                            BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 = p.Get<double>("@BilledCons_BilledUnmetConsBulkWatSupExpM3_H6"),                             
                            BilledCons_UnbMetConsM3_D8 = p.Get<double>("@BilledCons_UnbMetConsM3_D8"),           
                            BilledCons_UnbMetConsM3_D9 = p.Get<double>("@BilledCons_UnbMetConsM3_D9"),           
                            BilledCons_UnbMetConsM3_D10 = p.Get<double>("@BilledCons_UnbMetConsM3_D10"),           
                            BilledCons_UnbMetConsM3_D11 = p.Get<double>("@BilledCons_UnbMetConsM3_D11"),           
                            BilledCons_UnbUnmetConsM3_H8 = p.Get<double>("@BilledCons_UnbUnmetConsM3_H8"),
                            BilledCons_UnbUnmetConsM3_H9 = p.Get<double>("@BilledCons_UnbUnmetConsM3_H9"),
                            BilledCons_UnbUnmetConsM3_H10 = p.Get<double>("@BilledCons_UnbUnmetConsM3_H10"),
                            BilledCons_UnbUnmetConsM3_H11 = p.Get<double>("@BilledCons_UnbUnmetConsM3_H11"),
                        },
                        UnbilledConsModel = new WbEasyCalcModel.WbEasyCalc.UnbilledConsModel
                        {
                            UnbilledCons_Desc_D8 = p.Get<string>("@UnbilledCons_Desc_D8"),
                            UnbilledCons_Desc_D9 = p.Get<string>("@UnbilledCons_Desc_D9"),
                            UnbilledCons_Desc_D10 = p.Get<string>("@UnbilledCons_Desc_D10"),
                            UnbilledCons_Desc_D11 = p.Get<string>("@UnbilledCons_Desc_D11"),
                            UnbilledCons_Desc_F6 = p.Get<string>("@UnbilledCons_Desc_F6"),
                            UnbilledCons_Desc_F7 = p.Get<string>("@UnbilledCons_Desc_F7"),
                            UnbilledCons_Desc_F8 = p.Get<string>("@UnbilledCons_Desc_F8"),
                            UnbilledCons_Desc_F9 = p.Get<string>("@UnbilledCons_Desc_F9"),
                            UnbilledCons_Desc_F10 = p.Get<string>("@UnbilledCons_Desc_F10"),
                            UnbilledCons_Desc_F11 = p.Get<string>("@UnbilledCons_Desc_F11"),
                            UnbilledCons_MetConsBulkWatSupExpM3_D6 = p.Get<double>("@UnbilledCons_MetConsBulkWatSupExpM3_D6"),
                            UnbilledCons_UnbMetConsM3_D8 = p.Get<double>("@UnbilledCons_UnbMetConsM3_D8"),
                            UnbilledCons_UnbMetConsM3_D9 = p.Get<double>("@UnbilledCons_UnbMetConsM3_D9"),
                            UnbilledCons_UnbMetConsM3_D10 = p.Get<double>("@UnbilledCons_UnbMetConsM3_D10"),
                            UnbilledCons_UnbMetConsM3_D11 = p.Get<double>("@UnbilledCons_UnbMetConsM3_D11"),
                            UnbilledCons_UnbUnmetConsM3_H6 = p.Get<double>("@UnbilledCons_UnbUnmetConsM3_H6"),
                            UnbilledCons_UnbUnmetConsM3_H7 = p.Get<double>("@UnbilledCons_UnbUnmetConsM3_H7"),
                            UnbilledCons_UnbUnmetConsM3_H8 = p.Get<double>("@UnbilledCons_UnbUnmetConsM3_H8"),
                            UnbilledCons_UnbUnmetConsM3_H9 = p.Get<double>("@UnbilledCons_UnbUnmetConsM3_H9"),
                            UnbilledCons_UnbUnmetConsM3_H10 = p.Get<double>("@UnbilledCons_UnbUnmetConsM3_H10"),
                            UnbilledCons_UnbUnmetConsM3_H11 = p.Get<double>("@UnbilledCons_UnbUnmetConsM3_H11"),
                            UnbilledCons_UnbUnmetConsError_J6 = p.Get<double>("@UnbilledCons_UnbUnmetConsError_J6"),
                            UnbilledCons_UnbUnmetConsError_J7 = p.Get<double>("@UnbilledCons_UnbUnmetConsError_J7"),
                            UnbilledCons_UnbUnmetConsError_J8 = p.Get<double>("@UnbilledCons_UnbUnmetConsError_J8"),
                            UnbilledCons_UnbUnmetConsError_J9 = p.Get<double>("@UnbilledCons_UnbUnmetConsError_J9"),
                            UnbilledCons_UnbUnmetConsError_J10 = p.Get<double>("@UnbilledCons_UnbUnmetConsError_J10"),
                            UnbilledCons_UnbUnmetConsError_J11 = p.Get<double>("@UnbilledCons_UnbUnmetConsError_J11"),
                        },
                        UnauthConsModel = new WbEasyCalcModel.WbEasyCalc.UnauthConsModel
                        {
                            UnauthCons_Desc_B18 = p.Get<string>("@UnauthCons_Desc_B18"),
                            UnauthCons_Desc_B19 = p.Get<string>("@UnauthCons_Desc_B19"),
                            UnauthCons_Desc_B20 = p.Get<string>("@UnauthCons_Desc_B20"),
                            UnauthCons_Desc_B21 = p.Get<string>("@UnauthCons_Desc_B21"),
                            UnauthCons_OthersErrorMargin_F18 = p.Get<double>("@UnauthCons_OthersErrorMargin_F18"),
                            UnauthCons_OthersErrorMargin_F19 = p.Get<double>("@UnauthCons_OthersErrorMargin_F19"),
                            UnauthCons_OthersErrorMargin_F20 = p.Get<double>("@UnauthCons_OthersErrorMargin_F20"),
                            UnauthCons_OthersErrorMargin_F21 = p.Get<double>("@UnauthCons_OthersErrorMargin_F21"),
                            UnauthCons_OthersM3PerDay_J18 = p.Get<double>("@UnauthCons_OthersM3PerDay_J18"),
                            UnauthCons_OthersM3PerDay_J19 = p.Get<double>("@UnauthCons_OthersM3PerDay_J19"),
                            UnauthCons_OthersM3PerDay_J20 = p.Get<double>("@UnauthCons_OthersM3PerDay_J20"),
                            UnauthCons_OthersM3PerDay_J21 = p.Get<double>("@UnauthCons_OthersM3PerDay_J21"),

                            UnauthCons_IllegalConnDomEstNo_D6 = p.Get<int>("@UnauthCons_IllegalConnDomEstNo_D6"),
                            UnauthCons_IllegalConnDomPersPerHouse_H6 = p.Get<double>("@UnauthCons_IllegalConnDomPersPerHouse_H6"),
                            UnauthCons_IllegalConnDomConsLitPerPersDay_J6 = p.Get<double>("@UnauthCons_IllegalConnDomConsLitPerPersDay_J6"),
                            UnauthCons_IllegalConnDomErrorMargin_F6 = p.Get<double>("@UnauthCons_IllegalConnDomErrorMargin_F6"),
                            UnauthCons_IllegalConnOthersErrorMargin_F10 = p.Get<double>("@UnauthCons_IllegalConnOthersErrorMargin_F10"),
                            IllegalConnectionsOthersEstimatedNumber_D10 = p.Get<double>("@IllegalConnectionsOthersEstimatedNumber_D10"),
                            IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = p.Get<double>("@IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10"),
                            UnauthCons_MeterTampBypEtcEstNo_D14 = p.Get<double>("@UnauthCons_MeterTampBypEtcEstNo_D14"),
                            UnauthCons_MeterTampBypEtcErrorMargin_F14 = p.Get<double>("@UnauthCons_MeterTampBypEtcErrorMargin_F14"),
                            UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 = p.Get<double>("@UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14"),
                        },
                        MetErrorsModel = new WbEasyCalcModel.WbEasyCalc.MetErrorsModel
                        {
                            MetErrors_Desc_D12  = p.Get<string>("@MetErrors_Desc_D12"),
                            MetErrors_Desc_D13  = p.Get<string>("@MetErrors_Desc_D13"),
                            MetErrors_Desc_D14  = p.Get<string>("@MetErrors_Desc_D14"),
                            MetErrors_Desc_D15  = p.Get<string>("@MetErrors_Desc_D15"),
                            MetErrors_DetailedManualSpec_J6 = p.Get<int>("@MetErrors_DetailedManualSpec_J6"),                                    
                            MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 = p.Get<double>("@MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8"),     
                            MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 = p.Get<double>("@MetErrors_BilledMetConsWoBulkSupErrorMargin_N8"),

                            MetErrors_Total_F12 = p.Get<double>("@MetErrors_Total_F12"),
                            MetErrors_Total_F13 = p.Get<double>("@MetErrors_Total_F13"),
                            MetErrors_Total_F14 = p.Get<double>("@MetErrors_Total_F14"),
                            MetErrors_Total_F15 = p.Get<double>("@MetErrors_Total_F15"),
                            MetErrors_Meter_H12 = p.Get<double>("@MetErrors_Meter_H12"),
                            MetErrors_Meter_H13 = p.Get<double>("@MetErrors_Meter_H13"),
                            MetErrors_Meter_H14 = p.Get<double>("@MetErrors_Meter_H14"),
                            MetErrors_Meter_H15 = p.Get<double>("@MetErrors_Meter_H15"),
                            MetErrors_Error_N12 = p.Get<double>("@MetErrors_Error_N12"),
                            MetErrors_Error_N13 = p.Get<double>("@MetErrors_Error_N13"),
                            MetErrors_Error_N14 = p.Get<double>("@MetErrors_Error_N14"),
                            MetErrors_Error_N15 = p.Get<double>("@MetErrors_Error_N15"),
                            MeteredBulkSupplyExportErrorMargin_N32 = p.Get<double>("@MeteredBulkSupplyExportErrorMargin_N32"),                   
                            UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = p.Get<double>("@UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34"),   
                            CorruptMeterReadingPracticessErrorMargin_N38 = p.Get<double>("@CorruptMeterReadingPracticessErrorMargin_N38"),                               
                            DataHandlingErrorsOffice_L40 = p.Get<double>("@DataHandlingErrorsOffice_L40"),                                                               
                            DataHandlingErrorsOfficeErrorMargin_N40 = p.Get<double>("@DataHandlingErrorsOfficeErrorMargin_N40"),                                         
                            MetErrors_MetBulkSupExpMetUnderreg_H32 = p.Get<double>("@MetErrors_MetBulkSupExpMetUnderreg_H32"),                       
                            MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34 = p.Get<double>("@MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34"),   
                            MetErrors_CorruptMetReadPractMetUndrreg_H38 = p.Get<double>("@MetErrors_CorruptMetReadPractMetUndrreg_H38"),             
                        },
                        NetworkModel = new WbEasyCalcModel.WbEasyCalc.NetworkModel
                        {
                            Network_Desc_B7  = p.Get<string>("@Network_Desc_B7"),
                            Network_Desc_B8  = p.Get<string>("@Network_Desc_B8"),
                            Network_Desc_B9  = p.Get<string>("@Network_Desc_B9"),
                            Network_Desc_B10 = p.Get<string>("@Network_Desc_B10"),
                            Network_DistributionAndTransmissionMains_D7 = p.Get<double>("@Network_DistributionAndTransmissionMains_D7"),            
                            Network_NoOfConnOfRegCustomers_H10 = p.Get<double>("@Network_NoOfConnOfRegCustomers_H10"),                              
                            Network_NoOfInactAccountsWSvcConns_H18 = p.Get<double>("@Network_NoOfInactAccountsWSvcConns_H18"),                      
                            Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 = p.Get<double>("@Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32"),    
                            Network_PossibleUnd_D30 = p.Get<double>("@Network_PossibleUnd_D30"),
                            Network_NoCustomers_H7  = p.Get<double>("@Network_NoCustomers_H7"),
                            Network_ErrorMargin_J7  = p.Get<double>("@Network_ErrorMargin_J7"),
                            Network_ErrorMargin_J10 = p.Get<double>("@Network_ErrorMargin_J10"),
                            Network_ErrorMargin_J18 = p.Get<double>("@Network_ErrorMargin_J18"),
                            Network_ErrorMargin_J32 = p.Get<double>("@Network_ErrorMargin_J32"),
                        },
                        PressureModel = new WbEasyCalcModel.WbEasyCalc.PressureModel
                        {
                            Prs_Area_B7  = p.Get<string>("Prs_Area_B7"),
                            Prs_Area_B8  = p.Get<string>("Prs_Area_B8"),
                            Prs_Area_B9  = p.Get<string>("Prs_Area_B9"),
                            Prs_Area_B10 = p.Get<string>("Prs_Area_B10"),
                            Prs_ApproxNoOfConn_D7 = p.Get<double>("@Prs_ApproxNoOfConn_D7"),                                   
                            Prs_DailyAvgPrsM_F7 = p.Get<double>("@Prs_DailyAvgPrsM_F7"),                                       
                            Prs_ApproxNoOfConn_D8 = p.Get<double>("@Prs_ApproxNoOfConn_D8"),                                   
                            Prs_DailyAvgPrsM_F8 = p.Get<double>("@Prs_DailyAvgPrsM_F8"),                                       
                            Prs_ApproxNoOfConn_D9 = p.Get<double>("@Prs_ApproxNoOfConn_D9"),                                   
                            Prs_DailyAvgPrsM_F9 = p.Get<double>("@Prs_DailyAvgPrsM_F9"),                                       
                            Prs_ApproxNoOfConn_D10 = p.Get<double>("@Prs_ApproxNoOfConn_D10"),                                 
                            Prs_DailyAvgPrsM_F10 = p.Get<double>("@Prs_DailyAvgPrsM_F10"),                                     
                            Prs_ErrorMarg_F26 = p.Get<double>("@Prs_ErrorMarg_F26"),                                                    
                        },
                        IntermModel = new WbEasyCalcModel.WbEasyCalc.IntermModel
                        {
                            Interm_Area_B7  = p.Get<string>("Interm_Area_B7"),
                            Interm_Area_B8  = p.Get<string>("Interm_Area_B8"),
                            Interm_Area_B9  = p.Get<string>("Interm_Area_B9"),
                            Interm_Area_B10 = p.Get<string>("Interm_Area_B10"),
                            Interm_Conn_D7  = p.Get<double>("Interm_Conn_D7"),
                            Interm_Conn_D8  = p.Get<double>("Interm_Conn_D8"),
                            Interm_Conn_D9  = p.Get<double>("Interm_Conn_D9"),
                            Interm_Conn_D10 = p.Get<double>("Interm_Conn_D10"),
                            Interm_Days_F7  = p.Get<double>("Interm_Days_F7"),
                            Interm_Days_F8  = p.Get<double>("Interm_Days_F8"),
                            Interm_Days_F9  = p.Get<double>("Interm_Days_F9"),
                            Interm_Days_F10 = p.Get<double>("Interm_Days_F10"),
                            Interm_Hour_H7  = p.Get<double>("Interm_Hour_H7"),
                            Interm_Hour_H8  = p.Get<double>("Interm_Hour_H8"),
                            Interm_Hour_H9  = p.Get<double>("Interm_Hour_H9"),
                            Interm_Hour_H10 = p.Get<double>("Interm_Hour_H10"),
                        },
                        FinancDataModel = new WbEasyCalcModel.WbEasyCalc.FinancDataModel
                        {
                            FinancData_G6  = p.Get<double>("FinancData_G6"),
                            FinancData_K6  = p.Get<string>("FinancData_K6"),
                            FinancData_G8  = p.Get<double>("FinancData_G8"),
                            FinancData_D26 = p.Get<double>("FinancData_D26"),
                            FinancData_G35 = p.Get<double>("FinancData_G35"),
                        },
                        MatrixOneIn = new WbEasyCalcModel.WbEasyCalc.MatrixInModel
                        {
                            SelectedOption = p.Get<int>("@MatrixOneIn_SelectedOption"),
                            C11 = p.Get<double>("@MatrixOneIn_C11"),
                            C12 = p.Get<double>("@MatrixOneIn_C12"),
                            C13 = p.Get<double>("@MatrixOneIn_C13"),
                            C14 = p.Get<double>("@MatrixOneIn_C14"),
                            C21 = p.Get<double>("@MatrixOneIn_C21"),
                            C22 = p.Get<double>("@MatrixOneIn_C22"),
                            C23 = p.Get<double>("@MatrixOneIn_C23"),
                            C24 = p.Get<double>("@MatrixOneIn_C24"),
                            D21 = p.Get<double>("@MatrixOneIn_D21"),
                            D22 = p.Get<double>("@MatrixOneIn_D22"),
                            D23 = p.Get<double>("@MatrixOneIn_D23"),
                            D24 = p.Get<double>("@MatrixOneIn_D24"),
                            E11 = p.Get<double>("@MatrixOneIn_E11"),
                            E12 = p.Get<double>("@MatrixOneIn_E12"),
                            E13 = p.Get<double>("@MatrixOneIn_E13"),
                            E14 = p.Get<double>("@MatrixOneIn_E14"),
                            E21 = p.Get<double>("@MatrixOneIn_E21"),
                            E22 = p.Get<double>("@MatrixOneIn_E22"),
                            E23 = p.Get<double>("@MatrixOneIn_E23"),
                            E24 = p.Get<double>("@MatrixOneIn_E24"),
                            F11 = p.Get<double>("@MatrixOneIn_F11"),
                            F12 = p.Get<double>("@MatrixOneIn_F12"),
                            F13 = p.Get<double>("@MatrixOneIn_F13"),
                            F14 = p.Get<double>("@MatrixOneIn_F14"),
                            F21 = p.Get<double>("@MatrixOneIn_F21"),
                            F22 = p.Get<double>("@MatrixOneIn_F22"),
                            F23 = p.Get<double>("@MatrixOneIn_F23"),
                            F24 = p.Get<double>("@MatrixOneIn_F24"),
                            G11 = p.Get<double>("@MatrixOneIn_G11"),
                            G12 = p.Get<double>("@MatrixOneIn_G12"),
                            G13 = p.Get<double>("@MatrixOneIn_G13"),
                            G14 = p.Get<double>("@MatrixOneIn_G14"),
                            G21 = p.Get<double>("@MatrixOneIn_G21"),
                            G22 = p.Get<double>("@MatrixOneIn_G22"),
                            G23 = p.Get<double>("@MatrixOneIn_G23"),
                            G24 = p.Get<double>("@MatrixOneIn_G24"),
                            H11 = p.Get<double>("@MatrixOneIn_H11"),
                            H12 = p.Get<double>("@MatrixOneIn_H12"),
                            H13 = p.Get<double>("@MatrixOneIn_H13"),
                            H14 = p.Get<double>("@MatrixOneIn_H14"),
                            H21 = p.Get<double>("@MatrixOneIn_H21"),
                            H22 = p.Get<double>("@MatrixOneIn_H22"),
                            H23 = p.Get<double>("@MatrixOneIn_H23"),
                            H24 = p.Get<double>("@MatrixOneIn_H24"),
                        },
                        MatrixTwoIn = new WbEasyCalcModel.WbEasyCalc.MatrixInModel
                        {
                            SelectedOption = p.Get<int>("@MatrixTwoIn_SelectedOption"),
                            D21 = p.Get<double>("@MatrixTwoIn_D21"),
                            D22 = p.Get<double>("@MatrixTwoIn_D22"),
                            D23 = p.Get<double>("@MatrixTwoIn_D23"),
                            D24 = p.Get<double>("@MatrixTwoIn_D24"),
                            E11 = p.Get<double>("@MatrixTwoIn_E11"),
                            E12 = p.Get<double>("@MatrixTwoIn_E12"),
                            E13 = p.Get<double>("@MatrixTwoIn_E13"),
                            E14 = p.Get<double>("@MatrixTwoIn_E14"),
                            E21 = p.Get<double>("@MatrixTwoIn_E21"),
                            E22 = p.Get<double>("@MatrixTwoIn_E22"),
                            E23 = p.Get<double>("@MatrixTwoIn_E23"),
                            E24 = p.Get<double>("@MatrixTwoIn_E24"),
                            F11 = p.Get<double>("@MatrixTwoIn_F11"),
                            F12 = p.Get<double>("@MatrixTwoIn_F12"),
                            F13 = p.Get<double>("@MatrixTwoIn_F13"),
                            F14 = p.Get<double>("@MatrixTwoIn_F14"),
                            F21 = p.Get<double>("@MatrixTwoIn_F21"),
                            F22 = p.Get<double>("@MatrixTwoIn_F22"),
                            F23 = p.Get<double>("@MatrixTwoIn_F23"),
                            F24 = p.Get<double>("@MatrixTwoIn_F24"),
                            G11 = p.Get<double>("@MatrixTwoIn_G11"),
                            G12 = p.Get<double>("@MatrixTwoIn_G12"),
                            G13 = p.Get<double>("@MatrixTwoIn_G13"),
                            G14 = p.Get<double>("@MatrixTwoIn_G14"),
                            G21 = p.Get<double>("@MatrixTwoIn_G21"),
                            G22 = p.Get<double>("@MatrixTwoIn_G22"),
                            G23 = p.Get<double>("@MatrixTwoIn_G23"),
                            G24 = p.Get<double>("@MatrixTwoIn_G24"),
                            H11 = p.Get<double>("@MatrixTwoIn_H11"),
                            H12 = p.Get<double>("@MatrixTwoIn_H12"),
                            H13 = p.Get<double>("@MatrixTwoIn_H13"),
                            H14 = p.Get<double>("@MatrixTwoIn_H14"),
                            H21 = p.Get<double>("@MatrixTwoIn_H21"),
                            H22 = p.Get<double>("@MatrixTwoIn_H22"),
                            H23 = p.Get<double>("@MatrixTwoIn_H23"),
                            H24 = p.Get<double>("@MatrixTwoIn_H24"),
                        },
                    }
                };

                return model;
            }
        }

        public DataModel.WbEasyCalcData SaveItem(DataModel.WbEasyCalcData model)
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            using (IDbConnection connection = new SqlConnection(_cnnString))
            {
                var p = new DynamicParameters();

                p.Add("@id", model.WbEasyCalcDataId, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                p.Add("@UserName", userName);
                p.Add("@ZoneId", model.ZoneId);
                p.Add("@YearNo", model.YearNo);
                p.Add("@MonthNo", model.MonthNo);
                p.Add("@Description", model.Description);
                p.Add("@IsArchive", model.IsArchive);
                p.Add("@IsAccepted", model.IsAccepted);

                // input
                p.Add("@Start_PeriodDays_M21", model.EasyCalcModel.StartModel.Start_PeriodDays_M21);

                p.Add("@SysInput_Desc_B6", model.EasyCalcModel.SysInputModel.SysInput_Desc_B6);
                p.Add("@SysInput_Desc_B7", model.EasyCalcModel.SysInputModel.SysInput_Desc_B7);
                p.Add("@SysInput_Desc_B8", model.EasyCalcModel.SysInputModel.SysInput_Desc_B8);
                p.Add("@SysInput_Desc_B9", model.EasyCalcModel.SysInputModel.SysInput_Desc_B9);
                p.Add("@SysInput_SystemInputVolumeM3_D6", model.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D6);
                p.Add("@SysInput_SystemInputVolumeError_F6", model.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeError_F6);
                p.Add("@SysInput_SystemInputVolumeM3_D7", model.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D7);
                p.Add("@SysInput_SystemInputVolumeError_F7", model.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeError_F7);
                p.Add("@SysInput_SystemInputVolumeM3_D8", model.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D8);
                p.Add("@SysInput_SystemInputVolumeError_F8", model.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeError_F8);
                p.Add("@SysInput_SystemInputVolumeM3_D9", model.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeM3_D9);
                p.Add("@SysInput_SystemInputVolumeError_F9", model.EasyCalcModel.SysInputModel.SysInput_SystemInputVolumeError_F9);

                p.Add("@BilledCons_Desc_B8", model.EasyCalcModel.BilledConsModel.BilledCons_Desc_B8);
                p.Add("@BilledCons_Desc_B9", model.EasyCalcModel.BilledConsModel.BilledCons_Desc_B9);
                p.Add("@BilledCons_Desc_B10", model.EasyCalcModel.BilledConsModel.BilledCons_Desc_B10);
                p.Add("@BilledCons_Desc_B11", model.EasyCalcModel.BilledConsModel.BilledCons_Desc_B11);
                p.Add("@BilledCons_Desc_F8", model.EasyCalcModel.BilledConsModel.BilledCons_Desc_F8);
                p.Add("@BilledCons_Desc_F9", model.EasyCalcModel.BilledConsModel.BilledCons_Desc_F9);
                p.Add("@BilledCons_Desc_F10", model.EasyCalcModel.BilledConsModel.BilledCons_Desc_F10);
                p.Add("@BilledCons_Desc_F11", model.EasyCalcModel.BilledConsModel.BilledCons_Desc_F11);
                p.Add("@BilledCons_BilledMetConsBulkWatSupExpM3_D6", model.EasyCalcModel.BilledConsModel.BilledCons_BilledMetConsBulkWatSupExpM3_D6);
                p.Add("@BilledCons_BilledUnmetConsBulkWatSupExpM3_H6", model.EasyCalcModel.BilledConsModel.BilledCons_BilledUnmetConsBulkWatSupExpM3_H6);
                p.Add("@BilledCons_UnbMetConsM3_D8", model.EasyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D8);
                p.Add("@BilledCons_UnbMetConsM3_D9", model.EasyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D9);
                p.Add("@BilledCons_UnbMetConsM3_D10", model.EasyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D10);
                p.Add("@BilledCons_UnbMetConsM3_D11", model.EasyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D11);
                p.Add("@BilledCons_UnbUnmetConsM3_H8", model.EasyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H8);
                p.Add("@BilledCons_UnbUnmetConsM3_H9", model.EasyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H9);
                p.Add("@BilledCons_UnbUnmetConsM3_H10", model.EasyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H10);
                p.Add("@BilledCons_UnbUnmetConsM3_H11", model.EasyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H11);

                p.Add("@UnbilledCons_Desc_D8", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_D8);
                p.Add("@UnbilledCons_Desc_D9", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_D9);
                p.Add("@UnbilledCons_Desc_D10", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_D10);
                p.Add("@UnbilledCons_Desc_D11", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_D11);
                p.Add("@UnbilledCons_Desc_F6", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_F6);
                p.Add("@UnbilledCons_Desc_F7", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_F7);
                p.Add("@UnbilledCons_Desc_F8", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_F8);
                p.Add("@UnbilledCons_Desc_F9", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_F9);
                p.Add("@UnbilledCons_Desc_F10", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_F10);
                p.Add("@UnbilledCons_Desc_F11", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_Desc_F11);
                p.Add("@UnbilledCons_MetConsBulkWatSupExpM3_D6", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_MetConsBulkWatSupExpM3_D6);
                p.Add("@UnbilledCons_UnbMetConsM3_D8", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D8);
                p.Add("@UnbilledCons_UnbMetConsM3_D9", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D9);
                p.Add("@UnbilledCons_UnbMetConsM3_D10", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D10);
                p.Add("@UnbilledCons_UnbMetConsM3_D11", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D11);
                p.Add("@UnbilledCons_UnbUnmetConsM3_H6", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H6);
                p.Add("@UnbilledCons_UnbUnmetConsM3_H7", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H7);
                p.Add("@UnbilledCons_UnbUnmetConsM3_H8", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H8);
                p.Add("@UnbilledCons_UnbUnmetConsM3_H9", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H9);
                p.Add("@UnbilledCons_UnbUnmetConsM3_H10", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H10);
                p.Add("@UnbilledCons_UnbUnmetConsM3_H11", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H11);
                p.Add("@UnbilledCons_UnbUnmetConsError_J6", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J6);
                p.Add("@UnbilledCons_UnbUnmetConsError_J7", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J7);
                p.Add("@UnbilledCons_UnbUnmetConsError_J8", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J8);
                p.Add("@UnbilledCons_UnbUnmetConsError_J9", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J9);
                p.Add("@UnbilledCons_UnbUnmetConsError_J10", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J10);
                p.Add("@UnbilledCons_UnbUnmetConsError_J11", model.EasyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J11);

                p.Add("@UnauthCons_Desc_B18", model.EasyCalcModel.UnauthConsModel.UnauthCons_Desc_B18);
                p.Add("@UnauthCons_Desc_B19", model.EasyCalcModel.UnauthConsModel.UnauthCons_Desc_B19);
                p.Add("@UnauthCons_Desc_B20", model.EasyCalcModel.UnauthConsModel.UnauthCons_Desc_B20);
                p.Add("@UnauthCons_Desc_B21", model.EasyCalcModel.UnauthConsModel.UnauthCons_Desc_B21);
                p.Add("@UnauthCons_OthersErrorMargin_F18", model.EasyCalcModel.UnauthConsModel.UnauthCons_OthersErrorMargin_F18);
                p.Add("@UnauthCons_OthersErrorMargin_F19", model.EasyCalcModel.UnauthConsModel.UnauthCons_OthersErrorMargin_F19);
                p.Add("@UnauthCons_OthersErrorMargin_F20", model.EasyCalcModel.UnauthConsModel.UnauthCons_OthersErrorMargin_F20);
                p.Add("@UnauthCons_OthersErrorMargin_F21", model.EasyCalcModel.UnauthConsModel.UnauthCons_OthersErrorMargin_F21);
                p.Add("@UnauthCons_OthersM3PerDay_J18", model.EasyCalcModel.UnauthConsModel.UnauthCons_OthersM3PerDay_J18);
                p.Add("@UnauthCons_OthersM3PerDay_J19", model.EasyCalcModel.UnauthConsModel.UnauthCons_OthersM3PerDay_J19);
                p.Add("@UnauthCons_OthersM3PerDay_J20", model.EasyCalcModel.UnauthConsModel.UnauthCons_OthersM3PerDay_J20);
                p.Add("@UnauthCons_OthersM3PerDay_J21", model.EasyCalcModel.UnauthConsModel.UnauthCons_OthersM3PerDay_J21);
                p.Add("@UnauthCons_IllegalConnDomEstNo_D6", model.EasyCalcModel.UnauthConsModel.UnauthCons_IllegalConnDomEstNo_D6);
                p.Add("@UnauthCons_IllegalConnDomPersPerHouse_H6", model.EasyCalcModel.UnauthConsModel.UnauthCons_IllegalConnDomPersPerHouse_H6);
                p.Add("@UnauthCons_IllegalConnDomConsLitPerPersDay_J6", model.EasyCalcModel.UnauthConsModel.UnauthCons_IllegalConnDomConsLitPerPersDay_J6);
                p.Add("@UnauthCons_IllegalConnDomErrorMargin_F6", model.EasyCalcModel.UnauthConsModel.UnauthCons_IllegalConnDomErrorMargin_F6);
                p.Add("@UnauthCons_IllegalConnOthersErrorMargin_F10", model.EasyCalcModel.UnauthConsModel.UnauthCons_IllegalConnOthersErrorMargin_F10);
                p.Add("@IllegalConnectionsOthersEstimatedNumber_D10", model.EasyCalcModel.UnauthConsModel.IllegalConnectionsOthersEstimatedNumber_D10);
                p.Add("@IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10", model.EasyCalcModel.UnauthConsModel.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10);
                p.Add("@UnauthCons_MeterTampBypEtcEstNo_D14", model.EasyCalcModel.UnauthConsModel.UnauthCons_MeterTampBypEtcEstNo_D14);
                p.Add("@UnauthCons_MeterTampBypEtcErrorMargin_F14", model.EasyCalcModel.UnauthConsModel.UnauthCons_MeterTampBypEtcErrorMargin_F14);
                p.Add("@UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14", model.EasyCalcModel.UnauthConsModel.UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14);

                p.Add("@MetErrors_Desc_D12", model.EasyCalcModel.MetErrorsModel.MetErrors_Desc_D12);
                p.Add("@MetErrors_Desc_D13", model.EasyCalcModel.MetErrorsModel.MetErrors_Desc_D13);
                p.Add("@MetErrors_Desc_D14", model.EasyCalcModel.MetErrorsModel.MetErrors_Desc_D14);
                p.Add("@MetErrors_Desc_D15", model.EasyCalcModel.MetErrorsModel.MetErrors_Desc_D15);
                p.Add("@MetErrors_DetailedManualSpec_J6", model.EasyCalcModel.MetErrorsModel.MetErrors_DetailedManualSpec_J6);
                p.Add("@MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8", model.EasyCalcModel.MetErrorsModel.MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8);
                p.Add("@MetErrors_BilledMetConsWoBulkSupErrorMargin_N8", model.EasyCalcModel.MetErrorsModel.MetErrors_BilledMetConsWoBulkSupErrorMargin_N8);
                p.Add("@MetErrors_Total_F12", model.EasyCalcModel.MetErrorsModel.MetErrors_Total_F12);
                p.Add("@MetErrors_Total_F13", model.EasyCalcModel.MetErrorsModel.MetErrors_Total_F13);
                p.Add("@MetErrors_Total_F14", model.EasyCalcModel.MetErrorsModel.MetErrors_Total_F14);
                p.Add("@MetErrors_Total_F15", model.EasyCalcModel.MetErrorsModel.MetErrors_Total_F15);
                p.Add("@MetErrors_Meter_H12", model.EasyCalcModel.MetErrorsModel.MetErrors_Meter_H12);
                p.Add("@MetErrors_Meter_H13", model.EasyCalcModel.MetErrorsModel.MetErrors_Meter_H13);
                p.Add("@MetErrors_Meter_H14", model.EasyCalcModel.MetErrorsModel.MetErrors_Meter_H14);
                p.Add("@MetErrors_Meter_H15", model.EasyCalcModel.MetErrorsModel.MetErrors_Meter_H15);
                p.Add("@MetErrors_Error_N12", model.EasyCalcModel.MetErrorsModel.MetErrors_Error_N12);
                p.Add("@MetErrors_Error_N13", model.EasyCalcModel.MetErrorsModel.MetErrors_Error_N13);
                p.Add("@MetErrors_Error_N14", model.EasyCalcModel.MetErrorsModel.MetErrors_Error_N14);
                p.Add("@MetErrors_Error_N15", model.EasyCalcModel.MetErrorsModel.MetErrors_Error_N15);
                p.Add("@MeteredBulkSupplyExportErrorMargin_N32", model.EasyCalcModel.MetErrorsModel.MeteredBulkSupplyExportErrorMargin_N32);
                p.Add("@UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34", model.EasyCalcModel.MetErrorsModel.UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34);
                p.Add("@CorruptMeterReadingPracticessErrorMargin_N38", model.EasyCalcModel.MetErrorsModel.CorruptMeterReadingPracticessErrorMargin_N38);
                p.Add("@DataHandlingErrorsOffice_L40", model.EasyCalcModel.MetErrorsModel.DataHandlingErrorsOffice_L40);
                p.Add("@DataHandlingErrorsOfficeErrorMargin_N40", model.EasyCalcModel.MetErrorsModel.DataHandlingErrorsOfficeErrorMargin_N40);
                p.Add("@MetErrors_MetBulkSupExpMetUnderreg_H32", model.EasyCalcModel.MetErrorsModel.MetErrors_MetBulkSupExpMetUnderreg_H32);
                p.Add("@MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34", model.EasyCalcModel.MetErrorsModel.MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34);
                p.Add("@MetErrors_CorruptMetReadPractMetUndrreg_H38", model.EasyCalcModel.MetErrorsModel.MetErrors_CorruptMetReadPractMetUndrreg_H38);


                p.Add("@Network_Desc_B7", model.EasyCalcModel.NetworkModel.Network_Desc_B7);
                p.Add("@Network_Desc_B8", model.EasyCalcModel.NetworkModel.Network_Desc_B8);
                p.Add("@Network_Desc_B9", model.EasyCalcModel.NetworkModel.Network_Desc_B9);
                p.Add("@Network_Desc_B10", model.EasyCalcModel.NetworkModel.Network_Desc_B10);
                p.Add("@Network_DistributionAndTransmissionMains_D7", model.EasyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D7);
                p.Add("@Network_DistributionAndTransmissionMains_D8", model.EasyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D8);
                p.Add("@Network_DistributionAndTransmissionMains_D9", model.EasyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D9);
                p.Add("@Network_DistributionAndTransmissionMains_D10", model.EasyCalcModel.NetworkModel.Network_DistributionAndTransmissionMains_D10);
                p.Add("@Network_NoOfConnOfRegCustomers_H10", model.EasyCalcModel.NetworkModel.Network_NoOfConnOfRegCustomers_H10);
                p.Add("@Network_NoOfInactAccountsWSvcConns_H18", model.EasyCalcModel.NetworkModel.Network_NoOfInactAccountsWSvcConns_H18);
                p.Add("@Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32", model.EasyCalcModel.NetworkModel.Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32);
                p.Add("@Network_PossibleUnd_D30", model.EasyCalcModel.NetworkModel.Network_PossibleUnd_D30);
                p.Add("@Network_NoCustomers_H7", model.EasyCalcModel.NetworkModel.Network_NoCustomers_H7);
                p.Add("@Network_ErrorMargin_J7", model.EasyCalcModel.NetworkModel.Network_ErrorMargin_J7);
                p.Add("@Network_ErrorMargin_J10", model.EasyCalcModel.NetworkModel.Network_ErrorMargin_J10);
                p.Add("@Network_ErrorMargin_J18", model.EasyCalcModel.NetworkModel.Network_ErrorMargin_J18);
                p.Add("@Network_ErrorMargin_J32", model.EasyCalcModel.NetworkModel.Network_ErrorMargin_J32);
                p.Add("@Network_ErrorMargin_D35", model.EasyCalcModel.NetworkModel.Network_ErrorMargin_D35);

                p.Add("@Prs_Area_B7", model.EasyCalcModel.PressureModel.Prs_Area_B7);
                p.Add("@Prs_Area_B8", model.EasyCalcModel.PressureModel.Prs_Area_B8);
                p.Add("@Prs_Area_B9", model.EasyCalcModel.PressureModel.Prs_Area_B9);
                p.Add("@Prs_Area_B10", model.EasyCalcModel.PressureModel.Prs_Area_B10);
                p.Add("@Prs_ApproxNoOfConn_D7", model.EasyCalcModel.PressureModel.Prs_ApproxNoOfConn_D7);
                p.Add("@Prs_DailyAvgPrsM_F7", model.EasyCalcModel.PressureModel.Prs_DailyAvgPrsM_F7);
                p.Add("@Prs_ApproxNoOfConn_D8", model.EasyCalcModel.PressureModel.Prs_ApproxNoOfConn_D8);
                p.Add("@Prs_DailyAvgPrsM_F8", model.EasyCalcModel.PressureModel.Prs_DailyAvgPrsM_F8);
                p.Add("@Prs_ApproxNoOfConn_D9", model.EasyCalcModel.PressureModel.Prs_ApproxNoOfConn_D9);
                p.Add("@Prs_DailyAvgPrsM_F9", model.EasyCalcModel.PressureModel.Prs_DailyAvgPrsM_F9);
                p.Add("@Prs_ApproxNoOfConn_D10", model.EasyCalcModel.PressureModel.Prs_ApproxNoOfConn_D10);
                p.Add("@Prs_DailyAvgPrsM_F10", model.EasyCalcModel.PressureModel.Prs_DailyAvgPrsM_F10);
                p.Add("@Prs_ErrorMarg_F26", model.EasyCalcModel.PressureModel.Prs_ErrorMarg_F26);



                p.Add("@Interm_Area_B7", model.EasyCalcModel.IntermModel.Interm_Area_B7);
                p.Add("@Interm_Area_B8", model.EasyCalcModel.IntermModel.Interm_Area_B8);
                p.Add("@Interm_Area_B9", model.EasyCalcModel.IntermModel.Interm_Area_B9);
                p.Add("@Interm_Area_B10", model.EasyCalcModel.IntermModel.Interm_Area_B10);
                p.Add("@Interm_Conn_D7", model.EasyCalcModel.IntermModel.Interm_Conn_D7);
                p.Add("@Interm_Conn_D8", model.EasyCalcModel.IntermModel.Interm_Conn_D8);
                p.Add("@Interm_Conn_D9", model.EasyCalcModel.IntermModel.Interm_Conn_D9);
                p.Add("@Interm_Conn_D10", model.EasyCalcModel.IntermModel.Interm_Conn_D10);
                p.Add("@Interm_Days_F7", model.EasyCalcModel.IntermModel.Interm_Days_F7);
                p.Add("@Interm_Days_F8", model.EasyCalcModel.IntermModel.Interm_Days_F8);
                p.Add("@Interm_Days_F9", model.EasyCalcModel.IntermModel.Interm_Days_F9);
                p.Add("@Interm_Days_F10", model.EasyCalcModel.IntermModel.Interm_Days_F10);
                p.Add("@Interm_Hour_H7", model.EasyCalcModel.IntermModel.Interm_Hour_H7);
                p.Add("@Interm_Hour_H8", model.EasyCalcModel.IntermModel.Interm_Hour_H8);
                p.Add("@Interm_Hour_H9", model.EasyCalcModel.IntermModel.Interm_Hour_H9);
                p.Add("@Interm_Hour_H10", model.EasyCalcModel.IntermModel.Interm_Hour_H10);
                p.Add("@Interm_ErrorMarg_H26", model.EasyCalcModel.IntermModel.Interm_ErrorMarg_H26);

                p.Add("@FinancData_G6", model.EasyCalcModel.FinancDataModel.FinancData_G6);
                p.Add("@FinancData_K6", model.EasyCalcModel.FinancDataModel.FinancData_K6);
                p.Add("@FinancData_G8", model.EasyCalcModel.FinancDataModel.FinancData_G8);
                p.Add("@FinancData_D26", model.EasyCalcModel.FinancDataModel.FinancData_D26);
                p.Add("@FinancData_G35", model.EasyCalcModel.FinancDataModel.FinancData_G35);

                p.Add("@MatrixOneIn_SelectedOption", model.EasyCalcModel.MatrixOneIn.SelectedOption);
                p.Add("@MatrixOneIn_C11", model.EasyCalcModel.MatrixOneIn.C11);
                p.Add("@MatrixOneIn_C12", model.EasyCalcModel.MatrixOneIn.C12);
                p.Add("@MatrixOneIn_C13", model.EasyCalcModel.MatrixOneIn.C13);
                p.Add("@MatrixOneIn_C14", model.EasyCalcModel.MatrixOneIn.C14);
                p.Add("@MatrixOneIn_C21", model.EasyCalcModel.MatrixOneIn.C21);
                p.Add("@MatrixOneIn_C22", model.EasyCalcModel.MatrixOneIn.C22);
                p.Add("@MatrixOneIn_C23", model.EasyCalcModel.MatrixOneIn.C23);
                p.Add("@MatrixOneIn_C24", model.EasyCalcModel.MatrixOneIn.C24);
                p.Add("@MatrixOneIn_D21", model.EasyCalcModel.MatrixOneIn.D21);
                p.Add("@MatrixOneIn_D22", model.EasyCalcModel.MatrixOneIn.D22);
                p.Add("@MatrixOneIn_D23", model.EasyCalcModel.MatrixOneIn.D23);
                p.Add("@MatrixOneIn_D24", model.EasyCalcModel.MatrixOneIn.D24);
                p.Add("@MatrixOneIn_E11", model.EasyCalcModel.MatrixOneIn.E11);
                p.Add("@MatrixOneIn_E12", model.EasyCalcModel.MatrixOneIn.E12);
                p.Add("@MatrixOneIn_E13", model.EasyCalcModel.MatrixOneIn.E13);
                p.Add("@MatrixOneIn_E14", model.EasyCalcModel.MatrixOneIn.E14);
                p.Add("@MatrixOneIn_E21", model.EasyCalcModel.MatrixOneIn.E21);
                p.Add("@MatrixOneIn_E22", model.EasyCalcModel.MatrixOneIn.E22);
                p.Add("@MatrixOneIn_E23", model.EasyCalcModel.MatrixOneIn.E23);
                p.Add("@MatrixOneIn_E24", model.EasyCalcModel.MatrixOneIn.E24);
                p.Add("@MatrixOneIn_F11", model.EasyCalcModel.MatrixOneIn.F11);
                p.Add("@MatrixOneIn_F12", model.EasyCalcModel.MatrixOneIn.F12);
                p.Add("@MatrixOneIn_F13", model.EasyCalcModel.MatrixOneIn.F13);
                p.Add("@MatrixOneIn_F14", model.EasyCalcModel.MatrixOneIn.F14);
                p.Add("@MatrixOneIn_F21", model.EasyCalcModel.MatrixOneIn.F21);
                p.Add("@MatrixOneIn_F22", model.EasyCalcModel.MatrixOneIn.F22);
                p.Add("@MatrixOneIn_F23", model.EasyCalcModel.MatrixOneIn.F23);
                p.Add("@MatrixOneIn_F24", model.EasyCalcModel.MatrixOneIn.F24);
                p.Add("@MatrixOneIn_G11", model.EasyCalcModel.MatrixOneIn.G11);
                p.Add("@MatrixOneIn_G12", model.EasyCalcModel.MatrixOneIn.G12);
                p.Add("@MatrixOneIn_G13", model.EasyCalcModel.MatrixOneIn.G13);
                p.Add("@MatrixOneIn_G14", model.EasyCalcModel.MatrixOneIn.G14);
                p.Add("@MatrixOneIn_G21", model.EasyCalcModel.MatrixOneIn.G21);
                p.Add("@MatrixOneIn_G22", model.EasyCalcModel.MatrixOneIn.G22);
                p.Add("@MatrixOneIn_G23", model.EasyCalcModel.MatrixOneIn.G23);
                p.Add("@MatrixOneIn_G24", model.EasyCalcModel.MatrixOneIn.G24);
                p.Add("@MatrixOneIn_H11", model.EasyCalcModel.MatrixOneIn.H11);
                p.Add("@MatrixOneIn_H12", model.EasyCalcModel.MatrixOneIn.H12);
                p.Add("@MatrixOneIn_H13", model.EasyCalcModel.MatrixOneIn.H13);
                p.Add("@MatrixOneIn_H14", model.EasyCalcModel.MatrixOneIn.H14);
                p.Add("@MatrixOneIn_H21", model.EasyCalcModel.MatrixOneIn.H21);
                p.Add("@MatrixOneIn_H22", model.EasyCalcModel.MatrixOneIn.H22);
                p.Add("@MatrixOneIn_H23", model.EasyCalcModel.MatrixOneIn.H23);
                p.Add("@MatrixOneIn_H24", model.EasyCalcModel.MatrixOneIn.H24);

                p.Add("@MatrixTwoIn_SelectedOption", model.EasyCalcModel.MatrixTwoIn.SelectedOption);
                p.Add("@MatrixTwoIn_D21", model.EasyCalcModel.MatrixTwoIn.D21);
                p.Add("@MatrixTwoIn_D22", model.EasyCalcModel.MatrixTwoIn.D22);
                p.Add("@MatrixTwoIn_D23", model.EasyCalcModel.MatrixTwoIn.D23);
                p.Add("@MatrixTwoIn_D24", model.EasyCalcModel.MatrixTwoIn.D24);
                p.Add("@MatrixTwoIn_E11", model.EasyCalcModel.MatrixTwoIn.E11);
                p.Add("@MatrixTwoIn_E12", model.EasyCalcModel.MatrixTwoIn.E12);
                p.Add("@MatrixTwoIn_E13", model.EasyCalcModel.MatrixTwoIn.E13);
                p.Add("@MatrixTwoIn_E14", model.EasyCalcModel.MatrixTwoIn.E14);
                p.Add("@MatrixTwoIn_E21", model.EasyCalcModel.MatrixTwoIn.E21);
                p.Add("@MatrixTwoIn_E22", model.EasyCalcModel.MatrixTwoIn.E22);
                p.Add("@MatrixTwoIn_E23", model.EasyCalcModel.MatrixTwoIn.E23);
                p.Add("@MatrixTwoIn_E24", model.EasyCalcModel.MatrixTwoIn.E24);
                p.Add("@MatrixTwoIn_F11", model.EasyCalcModel.MatrixTwoIn.F11);
                p.Add("@MatrixTwoIn_F12", model.EasyCalcModel.MatrixTwoIn.F12);
                p.Add("@MatrixTwoIn_F13", model.EasyCalcModel.MatrixTwoIn.F13);
                p.Add("@MatrixTwoIn_F14", model.EasyCalcModel.MatrixTwoIn.F14);
                p.Add("@MatrixTwoIn_F21", model.EasyCalcModel.MatrixTwoIn.F21);
                p.Add("@MatrixTwoIn_F22", model.EasyCalcModel.MatrixTwoIn.F22);
                p.Add("@MatrixTwoIn_F23", model.EasyCalcModel.MatrixTwoIn.F23);
                p.Add("@MatrixTwoIn_F24", model.EasyCalcModel.MatrixTwoIn.F24);
                p.Add("@MatrixTwoIn_G11", model.EasyCalcModel.MatrixTwoIn.G11);
                p.Add("@MatrixTwoIn_G12", model.EasyCalcModel.MatrixTwoIn.G12);
                p.Add("@MatrixTwoIn_G13", model.EasyCalcModel.MatrixTwoIn.G13);
                p.Add("@MatrixTwoIn_G14", model.EasyCalcModel.MatrixTwoIn.G14);
                p.Add("@MatrixTwoIn_G21", model.EasyCalcModel.MatrixTwoIn.G21);
                p.Add("@MatrixTwoIn_G22", model.EasyCalcModel.MatrixTwoIn.G22);
                p.Add("@MatrixTwoIn_G23", model.EasyCalcModel.MatrixTwoIn.G23);
                p.Add("@MatrixTwoIn_G24", model.EasyCalcModel.MatrixTwoIn.G24);
                p.Add("@MatrixTwoIn_H11", model.EasyCalcModel.MatrixTwoIn.H11);
                p.Add("@MatrixTwoIn_H12", model.EasyCalcModel.MatrixTwoIn.H12);
                p.Add("@MatrixTwoIn_H13", model.EasyCalcModel.MatrixTwoIn.H13);
                p.Add("@MatrixTwoIn_H14", model.EasyCalcModel.MatrixTwoIn.H14);
                p.Add("@MatrixTwoIn_H21", model.EasyCalcModel.MatrixTwoIn.H21);
                p.Add("@MatrixTwoIn_H22", model.EasyCalcModel.MatrixTwoIn.H22);
                p.Add("@MatrixTwoIn_H23", model.EasyCalcModel.MatrixTwoIn.H23);
                p.Add("@MatrixTwoIn_H24", model.EasyCalcModel.MatrixTwoIn.H24);

                // output
                p.Add("@SystemInputVolume_B19", model.EasyCalcModel.WaterBalancePeriod.SystemInputVolume_B19);
                p.Add("@SystemInputVolumeErrorMargin_B21", model.EasyCalcModel.WaterBalancePeriod.SystemInputVolumeErrorMargin_B21);
                p.Add("@AuthorizedConsumption_K12", model.EasyCalcModel.WaterBalancePeriod.AuthorizedConsumption_K12);
                p.Add("@AuthorizedConsumptionErrorMargin_K15", model.EasyCalcModel.WaterBalancePeriod.AuthorizedConsumptionErrorMargin_K15);
                p.Add("@WaterLosses_K29", model.EasyCalcModel.WaterBalancePeriod.WaterLosses_K29);
                p.Add("@WaterLossesErrorMargin_K31", model.EasyCalcModel.WaterBalancePeriod.WaterLossesErrorMargin_K31);
                p.Add("@BilledAuthorizedConsumption_T8", model.EasyCalcModel.WaterBalancePeriod.BilledAuthorizedConsumption_T8);
                p.Add("@UnbilledAuthorizedConsumption_T16", model.EasyCalcModel.WaterBalancePeriod.UnbilledAuthorizedConsumption_T16);
                p.Add("@UnbilledAuthorizedConsumptionErrorMargin_T20", model.EasyCalcModel.WaterBalancePeriod.UnbilledAuthorizedConsumptionErrorMargin_T20);
                p.Add("@CommercialLosses_T26", model.EasyCalcModel.WaterBalancePeriod.CommercialLosses_T26);
                p.Add("@CommercialLossesErrorMargin_T29", model.EasyCalcModel.WaterBalancePeriod.CommercialLossesErrorMargin_T29);
                p.Add("@PhysicalLossesM3_T34", model.EasyCalcModel.WaterBalancePeriod.PhysicalLossesM3_T34);
                p.Add("@PhyscialLossesErrorMargin_AH35", model.EasyCalcModel.WaterBalancePeriod.PhyscialLossesErrorMargin_AH35);
                p.Add("@BilledMeteredConsumption_AC4", model.EasyCalcModel.WaterBalancePeriod.BilledMeteredConsumption_AC4);
                p.Add("@BilledUnmeteredConsumption_AC9", model.EasyCalcModel.WaterBalancePeriod.BilledUnmeteredConsumption_AC9);
                p.Add("@UnbilledMeteredConsumption_AC14", model.EasyCalcModel.WaterBalancePeriod.UnbilledMeteredConsumption_AC14);
                p.Add("@UnbilledUnmeteredConsumption_AC19", model.EasyCalcModel.WaterBalancePeriod.UnbilledUnmeteredConsumption_AC19);
                p.Add("@UnbilledUnmeteredConsumptionErrorMargin_AO20", model.EasyCalcModel.WaterBalancePeriod.UnbilledUnmeteredConsumptionErrorMargin_AO20);
                p.Add("@UnauthorizedConsumption_AC24", model.EasyCalcModel.WaterBalancePeriod.UnauthorizedConsumption_AC24);
                p.Add("@UnauthorizedConsumptionErrorMargin_AO25", model.EasyCalcModel.WaterBalancePeriod.UnauthorizedConsumptionErrorMargin_AO25);
                p.Add("@CustomerMeterInaccuraciesAndErrorsM3_AC29", model.EasyCalcModel.WaterBalancePeriod.CustomerMeterInaccuraciesAndErrorsM3_AC29);
                p.Add("@CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30", model.EasyCalcModel.WaterBalancePeriod.CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30);
                p.Add("@RevenueWaterM3_AY8", model.EasyCalcModel.WaterBalancePeriod.RevenueWaterM3_AY8);
                p.Add("@NonRevenueWaterM3_AY24", model.EasyCalcModel.WaterBalancePeriod.NonRevenueWaterM3_AY24);
                p.Add("@NonRevenueWaterErrorMargin_AY26", model.EasyCalcModel.WaterBalancePeriod.NonRevenueWaterErrorMargin_AY26);

                connection.Execute("dbo.spWbEasyCalcDataSave", p, commandType: CommandType.StoredProcedure);

                model.WbEasyCalcDataId = p.Get<int>("@id");

                var waterConsumptionRepo = new WaterConsumption.ListRepository(_cnnString);
                var oldList = waterConsumptionRepo.GetList().Where(x => x.WbEasyCalcDataId == model.WbEasyCalcDataId).ToList();
                // Delete
                foreach (var item in oldList)
                {
                    if (!model.WaterConsumptionModelList.Any(x => x.WaterConsumptionId == item.WaterConsumptionId))
                    {
                        waterConsumptionRepo.DeleteItem(item.WaterConsumptionId);
                    }
                }
                // Add and updaate
                foreach (var orderHeaderModel in model.WaterConsumptionModelList)
                {
                    if (!oldList.Any(x => x.WaterConsumptionId == orderHeaderModel.WaterConsumptionId))
                    {
                        orderHeaderModel.WaterConsumptionId = 0;
                        orderHeaderModel.WbEasyCalcDataId = model.WbEasyCalcDataId;
                    }
                    waterConsumptionRepo.SaveItem(orderHeaderModel);
                }

                return model;
            }
        }

        public bool DeleteItem(int id)
        {
            using (IDbConnection connection = new SqlConnection(_cnnString))
            {
                var p = new DynamicParameters();
                p.Add("@id", id);
                connection.Execute("dbo.spWbEasyCalcDataDelete", p, commandType: CommandType.StoredProcedure);
            }

            return true;
        }

        public bool DeleteItem(List<int> idList)
        {
            throw new NotImplementedException();
        }

        public int Clone(int id)
        {

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            using (IDbConnection connection = new SqlConnection(_cnnString))
            {
                var p = new DynamicParameters();
                p.Add("@id", id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                p.Add("@UserName", userName);

                connection.Execute("dbo.spWbEasyCalcDataClone", p, commandType: CommandType.StoredProcedure);

                var wbEasyCalcDataId = p.Get<int>("@id");

                return wbEasyCalcDataId;
            }
        }

        public int CreateAll()
        {
            using (IDbConnection connection = new SqlConnection(_cnnString))
            {
                var p = new DynamicParameters();
                p.Add("@RecordQty", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                connection.Execute("dbo.spWbEasyCalcDataCreateAll", p, commandType: CommandType.StoredProcedure);

                var recordQty = p.Get<int>("@RecordQty");

                return recordQty;
            }
        }


        private readonly string _cnnString;
        public ListRepository(string cnnString)
        {
            _cnnString = cnnString;
        }
    }
}
