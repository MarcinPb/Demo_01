using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Database.DataModel;
using Database.DataRepository.Options;

namespace Database.DataRepository
{
    public class MainRepo : IMainRepo
    {
        private readonly string _cnnString;

        public ItemRepository Option { get; private set; }

        public WbEasyCalcData.IListRepository WbEasyCalcDataListRepository { get; private set; }
        public WaterConsumption.IListRepository WaterConsumptionListRepository { get; private set; }
        public WaterConsumption.IListRepository WaterConsumptionListRepositoryTemp { get; set; }

        private List<ZoneItem> _zoneList;
        public List<ZoneItem> ZoneList
        {
            get
            {
                if (_zoneList == null)
                {
                    using (IDbConnection cnn = new SqlConnection(_cnnString))
                    {
                        _zoneList = cnn.Query<ZoneItem>("dbo.spZoneList", commandType: CommandType.StoredProcedure).ToList();
                        return _zoneList;
                    }
                }
                return _zoneList;
            }
            //set => throw new System.NotImplementedException();
        }

        private List<IdNamePair> _yearList;
        public List<IdNamePair> YearList
        {
            get
            {
                if (_yearList == null)
                {
                    using (IDbConnection cnn = new SqlConnection(_cnnString))
                    {
                        _yearList = cnn.Query<IdNamePair>("dbo.spYearList", commandType: CommandType.StoredProcedure).ToList();
                        return _yearList;
                    }
                }
                return _yearList;
            }
            set => throw new System.NotImplementedException();
        }

        private List<IdNamePair> _montList;
        public List<IdNamePair> MonthList
        {
            get
            {
                if (_montList == null)
                {
                    using (IDbConnection cnn = new SqlConnection(_cnnString))
                    {
                        _montList = cnn.Query<IdNamePair>("dbo.spMonthList", commandType: CommandType.StoredProcedure).ToList();
                        return _montList;
                    }
                }
                return _montList;
            }
            set => throw new System.NotImplementedException();
        }

        private List<IdNamePair> _waterConsumptionCategoryList;
        public List<IdNamePair> WaterConsumptionCategoryList
        {
            get
            {
                if (_waterConsumptionCategoryList == null)
                {
                    using (IDbConnection cnn = new SqlConnection(_cnnString))
                    {
                        _waterConsumptionCategoryList = cnn.Query<IdNamePair>("dbo.spWaterConsumptionCategoryList", commandType: CommandType.StoredProcedure).ToList();
                        return _waterConsumptionCategoryList;
                    }
                }
                return _waterConsumptionCategoryList;
            }
            set => throw new System.NotImplementedException();
        }

        private List<IdNamePair> _waterConsumptionStatusList;
        public List<IdNamePair> WaterConsumptionStatusList
        {
            get
            {
                if (_waterConsumptionStatusList == null)
                {
                    using (IDbConnection cnn = new SqlConnection(_cnnString))
                    {
                        _waterConsumptionStatusList = cnn.Query<IdNamePair>("dbo.spWaterConsumptionStatusList", commandType: CommandType.StoredProcedure).ToList();
                        return _waterConsumptionStatusList;
                    }
                }
                return _waterConsumptionStatusList;
            }
            set => throw new System.NotImplementedException();
        }

        private List<WaterConsumptionCategoryStatusExcel> _waterConsumptionCategoryStatusExcelList;
        public List<WaterConsumptionCategoryStatusExcel> WaterConsumptionCategoryStatusExcelList
        {
            get
            {
                if (_waterConsumptionCategoryStatusExcelList == null)
                {
                    using (IDbConnection cnn = new SqlConnection(_cnnString))
                    {
                        _waterConsumptionCategoryStatusExcelList = cnn.Query<WaterConsumptionCategoryStatusExcel>("dbo.spWaterConsumptionCategoryStatusExcelList", commandType: CommandType.StoredProcedure).ToList();
                        return _waterConsumptionCategoryStatusExcelList;
                    }
                }
                return _waterConsumptionCategoryStatusExcelList;
            }
            set => throw new System.NotImplementedException();
        }

        public DataModel.WbEasyCalcData GetWbGisModelScadaData(int yearNo, int monthNo, int zoneId)
        {
            using (IDbConnection connection = new SqlConnection(_cnnString))
            {
                var p = new DynamicParameters();

                p.Add("@YearNo", yearNo);
                p.Add("@MonthNo", monthNo);
                p.Add("@ZoneId", zoneId);

                p.Add("@SysInput_SystemInputVolumeM3_D6", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@BilledCons_UnbMetConsM3_D8", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@Network_DistributionAndTransmissionMains_D7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_NoCustomers_H7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_NoOfConnOfRegCustomers_H10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Network_NoOfInactAccountsWSvcConns_H18", dbType: DbType.Double, direction: ParameterDirection.Output);

                p.Add("@Prs_DailyAvgPrsM_F7", dbType: DbType.Double, direction: ParameterDirection.Output);

                connection.Execute("dbo.spWbGetGisModelScadaData", p, commandType: CommandType.StoredProcedure, commandTimeout: 600);

                return new Database.DataModel.WbEasyCalcData()
                {
                    EasyCalcModel= new WbEasyCalcModel.EasyCalcModel
                    { 
                        SysInputModel = new WbEasyCalcModel.WbEasyCalc.SysInputModel
                        {
                            SysInput_SystemInputVolumeM3_D6 = p.Get<double>("@SysInput_SystemInputVolumeM3_D6"),
                        },
                        BilledConsModel = new WbEasyCalcModel.WbEasyCalc.BilledConsModel
                        {
                            BilledCons_UnbMetConsM3_D8 = p.Get<double>("@BilledCons_UnbMetConsM3_D8"),
                        },
                        NetworkModel = new WbEasyCalcModel.WbEasyCalc.NetworkModel
                        {
                            Network_DistributionAndTransmissionMains_D7 = p.Get<double>("@Network_DistributionAndTransmissionMains_D7"),
                            Network_NoOfConnOfRegCustomers_H10 = p.Get<double>("@Network_NoOfConnOfRegCustomers_H10"),
                            Network_NoOfInactAccountsWSvcConns_H18 = p.Get<double>("@Network_NoOfInactAccountsWSvcConns_H18"),
                            Network_NoCustomers_H7 = p.Get<double>("@Network_NoCustomers_H7"),
                        },
                        PressureModel = new WbEasyCalcModel.WbEasyCalc.PressureModel
                        {
                            Prs_DailyAvgPrsM_F7 = p.Get<double>("@Prs_DailyAvgPrsM_F7"),
                        },
                    },
                };
            }
        }

        public DataModel.WbEasyCalcData GetWbYearData(int yearNo, int zoneId)
        {
            using (IDbConnection connection = new SqlConnection(_cnnString))
            {
                var p = new DynamicParameters();

                p.Add("@YearNo", yearNo);
                //p.Add("@MonthNo", monthNo);
                p.Add("@ZoneId", zoneId);

                // input
                p.Add("@Start_PeriodDays_M21", dbType: DbType.Int32, direction: ParameterDirection.Output);

                p.Add("@SysInput_SystemInputVolumeM3_D6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeError_F6", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeM3_D7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeError_F7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeM3_D8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeError_F8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeM3_D9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@SysInput_SystemInputVolumeError_F9", dbType: DbType.Double, direction: ParameterDirection.Output);

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

                //p.Add("@Prs_ApproxNoOfConn_D7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_DailyAvgPrsM_F7", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_ApproxNoOfConn_D8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_DailyAvgPrsM_F8", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_ApproxNoOfConn_D9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_DailyAvgPrsM_F9", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_ApproxNoOfConn_D10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_DailyAvgPrsM_F10", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@Prs_ErrorMarg_F26", dbType: DbType.Double, direction: ParameterDirection.Output);

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

                p.Add("@FinancData_D26", dbType: DbType.Double, direction: ParameterDirection.Output);
                p.Add("@FinancData_G35", dbType: DbType.Double, direction: ParameterDirection.Output);

                var list = connection.Query<DataModel.WaterConsumption>("dbo.spWbGetYearData", p, commandType: CommandType.StoredProcedure).ToList();

                return new Database.DataModel.WbEasyCalcData()
                {
                    EasyCalcModel = new WbEasyCalcModel.EasyCalcModel
                    {
                        StartModel = new WbEasyCalcModel.WbEasyCalc.StartModel
                        {
                            Start_PeriodDays_M21 = p.Get<int>("@Start_PeriodDays_M21"),                                                           
                        },
                        SysInputModel = new WbEasyCalcModel.WbEasyCalc.SysInputModel
                        {
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
                            Network_DistributionAndTransmissionMains_D7 = p.Get<double>("@Network_DistributionAndTransmissionMains_D7"),
                            Network_NoOfConnOfRegCustomers_H10 = p.Get<double>("@Network_NoOfConnOfRegCustomers_H10"),
                            Network_NoOfInactAccountsWSvcConns_H18 = p.Get<double>("@Network_NoOfInactAccountsWSvcConns_H18"),
                            Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 = p.Get<double>("@Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32"),
                            Network_PossibleUnd_D30 = p.Get<double>("@Network_PossibleUnd_D30"),
                            Network_NoCustomers_H7 = p.Get<double>("@Network_NoCustomers_H7"),
                            Network_ErrorMargin_J7 = p.Get<double>("@Network_ErrorMargin_J7"),
                            Network_ErrorMargin_J10 = p.Get<double>("@Network_ErrorMargin_J10"),
                            Network_ErrorMargin_J18 = p.Get<double>("@Network_ErrorMargin_J18"),
                            Network_ErrorMargin_J32 = p.Get<double>("@Network_ErrorMargin_J32"),
                        },
                        PressureModel = new WbEasyCalcModel.WbEasyCalc.PressureModel
                        {
                            //Prs_ApproxNoOfConn_D7 = p.Get<double>("@Prs_ApproxNoOfConn_D7"),
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
                            Interm_Conn_D7 = p.Get<double>("Interm_Conn_D7"),
                            Interm_Conn_D8 = p.Get<double>("Interm_Conn_D8"),
                            Interm_Conn_D9 = p.Get<double>("Interm_Conn_D9"),
                            Interm_Conn_D10 = p.Get<double>("Interm_Conn_D10"),
                            Interm_Days_F7 = p.Get<double>("Interm_Days_F7"),
                            Interm_Days_F8 = p.Get<double>("Interm_Days_F8"),
                            Interm_Days_F9 = p.Get<double>("Interm_Days_F9"),
                            Interm_Days_F10 = p.Get<double>("Interm_Days_F10"),
                            Interm_Hour_H7 = p.Get<double>("Interm_Hour_H7"),
                            Interm_Hour_H8 = p.Get<double>("Interm_Hour_H8"),
                            Interm_Hour_H9 = p.Get<double>("Interm_Hour_H9"),
                            Interm_Hour_H10 = p.Get<double>("Interm_Hour_H10"),
                        },
                        FinancDataModel = new WbEasyCalcModel.WbEasyCalc.FinancDataModel
                        {
                            FinancData_D26 = p.Get<double>("FinancData_D26"),
                            FinancData_G35 = p.Get<double>("FinancData_G35"),
                        },
                    },
                    WaterConsumptionModelList = list,
                };
            }
        }



        public MainRepo(string cnnString)
        {
            _cnnString = cnnString;

            Option = new ItemRepository(_cnnString);

            WbEasyCalcDataListRepository = new WbEasyCalcData.ListRepository(_cnnString);
            WaterConsumptionListRepository = new WaterConsumption.ListRepository(_cnnString);

        }
    }
}
