using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;
using WbEasyCalcRepository.Model;

namespace WbEasyCalcRepository.Test
{
    [TestClass]
    public class UnitTest2
    {
        #region Base test

        private EasyCalcModel _easyCalcModel_01 = new EasyCalcModel
        {
            StartModel = new StartModel
            {
                Start_PeriodDays_M21 = 30,
            },

            SysInputModel = new SysInputModel
            {
                SysInput_SystemInputVolumeM3_D6 = 6593339,
                SysInput_SystemInputVolumeError_F6 = 0.05,
                //SysInput_SystemInputVolumeM3_D7 = 10,
                //SysInput_SystemInputVolumeError_F7 = 0.10,
            },

            BilledConsModel = new BilledConsModel
            {
                BilledCons_BilledMetConsBulkWatSupExpM3_D6 = 5332026,
                BilledCons_BilledUnmetConsBulkWatSupExpM3_H6 = 1000,
            },


            UnbilledConsModel = new UnbilledConsModel
            {
                UnbilledCons_MetConsBulkWatSupExpM3_D6 = 309349,
                UnbilledCons_UnbMetConsM3_D8 = 0,
                UnbilledCons_UnbUnmetConsM3_H6 = 0,
                UnbilledCons_UnbUnmetConsError_J6 = 0,
            },

            UnauthConsModel = new UnauthConsModel
            {
                UnauthCons_IllegalConnDomEstNo_D6 = 100,
                UnauthCons_IllegalConnDomErrorMargin_F6 = 0.05,
                UnauthCons_IllegalConnDomPersPerHouse_H6 = 3,
                UnauthCons_IllegalConnDomConsLitPerPersDay_J6 = 120,
                UnauthCons_IllegalConnOthersErrorMargin_F10 = 0.0,
                IllegalConnectionsOthersEstimatedNumber_D10 = 0,
                IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = 0,
                UnauthCons_MeterTampBypEtcEstNo_D14 = 1000,
                UnauthCons_MeterTampBypEtcErrorMargin_F14 = 0.10,
                UnauthCons_MeterTampBypEtcConsLitPerCustDay_J14 = 160,
            },

            MetErrorsModel = new MetErrorsModel
            {
                MetErrors_DetailedManualSpec_J6 = 1,
                MetErrors_BilledMetConsWoBulkSupMetUndrreg_H8 = 0.03,
                MetErrors_BilledMetConsWoBulkSupErrorMargin_N8 = 0.02,
                MeteredBulkSupplyExportErrorMargin_N32 = 0.0,
                UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = 0.0,
                CorruptMeterReadingPracticessErrorMargin_N38 = 0.0,
                DataHandlingErrorsOffice_L40 = 0.0,
                DataHandlingErrorsOfficeErrorMargin_N40 = 0.0,
                MetErrors_MetBulkSupExpMetUnderreg_H32 = 0.03,
                MetErrors_UnbillMetConsWoBulkSupplMetUndrreg_H34 = 0.03,
                MetErrors_CorruptMetReadPractMetUndrreg_H38 = 0.03,
            },

            NetworkModel = new NetworkModel
            {
                Network_DistributionAndTransmissionMains_D7 = 260.0,
                Network_NoOfConnOfRegCustomers_H10 = 8000,
                Network_NoOfInactAccountsWSvcConns_H18 = 1500,
                Network_AvgLenOfSvcConnFromBoundaryToMeterM_H32 = 7,
            },

            PressureModel = new PressureModel
            {
                Prs_ApproxNoOfConn_D7 = 9500,
                Prs_DailyAvgPrsM_F7 = 30
            },

            //IntermModel = new IntermModel(),

            //FinancDataModel = new FinancDataModel(),
        };

        private Dictionary<string, double> _expectedDictDay = new Dictionary<string, double>()
        {
            {"SystemInputVolume_B19", 219778},
            {"SystemInputVolumeErrorMargin_B21", 0.05},

            {"AuthorizedConsumption_K12", 188413},                             // 5642375
            {"AuthorizedConsumptionErrorMargin_K15", 0.0},
            {"WaterLosses_K29", 31365},                                        // 950964
            {"WaterLossesErrorMargin_K31", 0.35},

            {"BilledAuthorizedConsumption_T8", 177934},
            {"UnbilledAuthorizedConsumption_T16", 10478},                      // 309349
            {"UnbilledAuthorizedConsumptionErrorMargin_T20", 0.0},
            {"CommercialLosses_T26", 11624},
            {"CommercialLossesErrorMargin_T29", 0.014},
            {"PhysicalLossesM3_T34", 19741},                                    // 605669.536
            {"PhyscialLossesErrorMargin_AH35", 0.557},                          // 0.544

            {"BilledMeteredConsumption_AC4", 177834},
            {"BilledUnmeteredConsumption_AC9", 100},
            {"UnbilledMeteredConsumption_AC14", 10412},

            {"UnbilledUnmeteredConsumption_AC19", 67},                           // 0
            {"UnbilledUnmeteredConsumptionErrorMargin_AO20", 0.05},              // 0.0
            {"UnauthorizedConsumption_AC24", 296},
            {"UnauthorizedConsumptionErrorMargin_AO25", 0.057},
            {"CustomerMeterInaccuraciesAndErrorsM3_AC29", 11328},
            {"CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30", 0.014},

            {"RevenueWaterM3_AY8", 177934},
            {"NonRevenueWaterM3_AY24", 41844},
            {"NonRevenueWaterErrorMargin_AY26", 0.263},
        };

        private Dictionary<string, double> _expectedDictPeriod = new Dictionary<string, double>()
        {
            {"SystemInputVolume_B19", 6593339},
            {"SystemInputVolumeErrorMargin_B21", 0.05},

            {"AuthorizedConsumption_K12", 5642375},                             // 5642375
            {"AuthorizedConsumptionErrorMargin_K15", 0.0},
            {"WaterLosses_K29", 950964},                                        // 950964
            {"WaterLossesErrorMargin_K31", 0.347},

            {"BilledAuthorizedConsumption_T8", 5333026},
            {"UnbilledAuthorizedConsumption_T16", 309349},                      // 309349
            {"UnbilledAuthorizedConsumptionErrorMargin_T20", 0.0},
            {"CommercialLosses_T26", 345294},
            {"CommercialLossesErrorMargin_T29", 0.001},
            {"PhysicalLossesM3_T34", 605670},                                   // 605669.536
            {"PhyscialLossesErrorMargin_AH35", 0.544},                          // 0.544

            {"BilledMeteredConsumption_AC4", 5332026},
            {"BilledUnmeteredConsumption_AC9", 1000},
            {"UnbilledMeteredConsumption_AC14", 309349},                        

            {"UnbilledUnmeteredConsumption_AC19", 0},                           // 0
            {"UnbilledUnmeteredConsumptionErrorMargin_AO20", 0.0},              // 0.0
            {"UnauthorizedConsumption_AC24", 5880},
            {"UnauthorizedConsumptionErrorMargin_AO25", 0.082},
            {"CustomerMeterInaccuraciesAndErrorsM3_AC29", 339414},              // 339414.464
            {"CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30", 0.0},

            {"RevenueWaterM3_AY8", 5333026},
            {"NonRevenueWaterM3_AY24", 1260313},
            {"NonRevenueWaterErrorMargin_AY26", 0.262},

        };

        #endregion

        [TestMethod]
        public void ReadSheetData_Test_01()
        {
            ReadSheetData_Test(_easyCalcModel_01, _expectedDictPeriod);
        }

        [TestMethod]
        public void ReadSheetData_Test_02()
        {
            EasyCalcModel easyCalcModel = (EasyCalcModel)_easyCalcModel_01.Clone();

            easyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D8 = 3000;
            easyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H8 = 2000;
            easyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D8 = 3000;
            easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H6 = 2000;
            easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J6 = 0.05;


            var dictTemp = CloneDictionaryCloningValues<string, double>(_expectedDictPeriod);

            //dictTemp["SystemInputVolume_B19"] = 6593339;                         
            //dictTemp["SystemInputVolumeErrorMargin_B21"] = 0.05;                                    

            dictTemp["AuthorizedConsumption_K12"] = 5652375;                         
            //dictTemp["AuthorizedConsumptionErrorMargin_K15"] = 0.0;                         
            dictTemp["WaterLosses_K29"] = 940964;                                    
            dictTemp["WaterLossesErrorMargin_K31"] = 0.35; 
            
            dictTemp["BilledAuthorizedConsumption_T8"] = 5338026;                  
            dictTemp["UnbilledAuthorizedConsumption_T16"] = 314349;                  
            //dictTemp["UnbilledAuthorizedConsumptionErrorMargin_T20"] = 0.0;            
            dictTemp["CommercialLosses_T26"] = 345635;                              
            //dictTemp["CommercialLossesErrorMargin_T29"] = 0.001;                       
            dictTemp["PhysicalLossesM3_T34"] = 595329;                           
            dictTemp["PhyscialLossesErrorMargin_AH35"] = 0.554;
        
            dictTemp["BilledMeteredConsumption_AC4"] = 5335026;            
            dictTemp["BilledUnmeteredConsumption_AC9"] = 3000;            
            dictTemp["UnbilledMeteredConsumption_AC14"] = 312349;

            dictTemp["UnbilledUnmeteredConsumption_AC19"] = 2000;                    
            dictTemp["UnbilledUnmeteredConsumptionErrorMargin_AO20"] = 0.05;
            //dictTemp["UnauthorizedConsumption_AC24"] = 5880;         
            //dictTemp["UnauthorizedConsumptionErrorMargin_AO25"] = 0.082;         
            dictTemp["CustomerMeterInaccuraciesAndErrorsM3_AC29"] = 339755;         
            dictTemp["CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30"] = 0.0;

            dictTemp["RevenueWaterM3_AY8"] = 5338026;         
            dictTemp["NonRevenueWaterM3_AY24"] = 1255313;
            dictTemp["NonRevenueWaterErrorMargin_AY26"] = 0.263;         

            ReadSheetData_Test(easyCalcModel, dictTemp);
        }



        [TestMethod]
        public void ReadSheetData_Test_03()
        {
            EasyCalcModel easyCalcModel = (EasyCalcModel)_easyCalcModel_01.Clone();

            easyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D8 = 3000;
            easyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H8 = 2000;
            easyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D8 = 3000;
            easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H6 = 2000;
            easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J6 = 0.05;

            easyCalcModel.UnauthConsModel.IllegalConnectionsOthersEstimatedNumber_D10 = 500;
            easyCalcModel.UnauthConsModel.UnauthCons_IllegalConnOthersErrorMargin_F10 = 0.05;
            easyCalcModel.UnauthConsModel.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = 200;

            var dictTemp = CloneDictionaryCloningValues<string, double>(_expectedDictPeriod);

            //dictTemp["SystemInputVolume_B19"] = 6593339;                         
            //dictTemp["SystemInputVolumeErrorMargin_B21"] = 0.05;                                    

            dictTemp["AuthorizedConsumption_K12"] = 5652375;
            //dictTemp["AuthorizedConsumptionErrorMargin_K15"] = 0.0;                         
            dictTemp["WaterLosses_K29"] = 940964;
            dictTemp["WaterLossesErrorMargin_K31"] = 0.35;

            dictTemp["BilledAuthorizedConsumption_T8"] = 5338026;                  
            dictTemp["UnbilledAuthorizedConsumption_T16"] = 314349;
            //dictTemp["UnbilledAuthorizedConsumptionErrorMargin_T20"] = 0.0;            
            dictTemp["CommercialLosses_T26"] = 348635;
            //dictTemp["CommercialLossesErrorMargin_T29"] = 0.001;                       
            dictTemp["PhysicalLossesM3_T34"] = 592329;
            dictTemp["PhyscialLossesErrorMargin_AH35"] = 0.557;

            dictTemp["BilledMeteredConsumption_AC4"] = 5335026;            
            dictTemp["BilledUnmeteredConsumption_AC9"] = 3000;            
            dictTemp["UnbilledMeteredConsumption_AC14"] = 312349;

            dictTemp["UnbilledUnmeteredConsumption_AC19"] = 2000;
            dictTemp["UnbilledUnmeteredConsumptionErrorMargin_AO20"] = 0.05;
            dictTemp["UnauthorizedConsumption_AC24"] = 8880;
            dictTemp["UnauthorizedConsumptionErrorMargin_AO25"] = 0.057;         
            dictTemp["CustomerMeterInaccuraciesAndErrorsM3_AC29"] = 339755;
            dictTemp["CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30"] = 0.0;

            dictTemp["RevenueWaterM3_AY8"] = 5338026;         
            dictTemp["NonRevenueWaterM3_AY24"] = 1255313;         
            dictTemp["NonRevenueWaterErrorMargin_AY26"] = 0.263;         

            ReadSheetData_Test(easyCalcModel, dictTemp);
        }

        [TestMethod]
        public void ReadSheetData_Test_04_1()
        {
            EasyCalcModel easyCalcModel = (EasyCalcModel)_easyCalcModel_01.Clone();

            easyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D8 = 3000;
            easyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H8 = 2000;
            easyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D8 = 3000;
            easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H6 = 2000;
            easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J6 = 0.05;

            easyCalcModel.UnauthConsModel.IllegalConnectionsOthersEstimatedNumber_D10 = 500;
            easyCalcModel.UnauthConsModel.UnauthCons_IllegalConnOthersErrorMargin_F10 = 0.05;
            easyCalcModel.UnauthConsModel.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = 200;

            easyCalcModel.MetErrorsModel.MeteredBulkSupplyExportErrorMargin_N32 = 0.02;
            easyCalcModel.MetErrorsModel.UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = 0.02;
            easyCalcModel.MetErrorsModel.CorruptMeterReadingPracticessErrorMargin_N38 = 0.02;
            easyCalcModel.MetErrorsModel.DataHandlingErrorsOffice_L40 = 100;
            easyCalcModel.MetErrorsModel.DataHandlingErrorsOfficeErrorMargin_N40 = 0.02;

            var dictTemp0 = CloneDictionaryCloningValues<string, double>(_expectedDictDay);

            var dictTemp = CloneDictionaryCloningValues<string, double>(_expectedDictPeriod);
            //dictTemp["SystemInputVolume_B19"] = 6593339;                         
            //dictTemp["SystemInputVolumeErrorMargin_B21"] = 0.05;                                    

            dictTemp["AuthorizedConsumption_K12"] = 5652375;
            //dictTemp["AuthorizedConsumptionErrorMargin_K15"] = 0.0;                         
            dictTemp["WaterLosses_K29"] = 940964;
            dictTemp["WaterLossesErrorMargin_K31"] = 0.35;

            dictTemp["BilledAuthorizedConsumption_T8"] = 5338026;                  
            dictTemp["UnbilledAuthorizedConsumption_T16"] = 314349;
            //dictTemp["UnbilledAuthorizedConsumptionErrorMargin_T20"] = 0.0;            
            dictTemp["CommercialLosses_T26"] = 348735;
            dictTemp["CommercialLossesErrorMargin_T29"] = 0.014;                       
            dictTemp["PhysicalLossesM3_T34"] = 592229;
            dictTemp["PhyscialLossesErrorMargin_AH35"] = 0.557;

            dictTemp["BilledMeteredConsumption_AC4"] = 5335026;
            dictTemp["BilledUnmeteredConsumption_AC9"] = 3000;            
            dictTemp["UnbilledMeteredConsumption_AC14"] = 312349;

            dictTemp["UnbilledUnmeteredConsumption_AC19"] = 2000;
            dictTemp["UnbilledUnmeteredConsumptionErrorMargin_AO20"] = 0.05;
            dictTemp["UnauthorizedConsumption_AC24"] = 8880;
            dictTemp["UnauthorizedConsumptionErrorMargin_AO25"] = 0.057;         
            dictTemp["CustomerMeterInaccuraciesAndErrorsM3_AC29"] = 339855;
            dictTemp["CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30"] = 0.014;

            dictTemp["RevenueWaterM3_AY8"] = 5338026;         
            dictTemp["NonRevenueWaterM3_AY24"] = 1255313;         
            dictTemp["NonRevenueWaterErrorMargin_AY26"] = 0.263;         

            ReadSheetData_Test(easyCalcModel, dictTemp0, dictTemp);
        }

        [TestMethod]
        public void ReadSheetData_Test_04_2()
        {
            EasyCalcModel easyCalcModel = (EasyCalcModel)_easyCalcModel_01.Clone();

            easyCalcModel.BilledConsModel.BilledCons_UnbMetConsM3_D8 = 3000;
            easyCalcModel.BilledConsModel.BilledCons_UnbUnmetConsM3_H8 = 2000;
            easyCalcModel.UnbilledConsModel.UnbilledCons_UnbMetConsM3_D8 = 3000;
            easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsM3_H6 = 2000;
            easyCalcModel.UnbilledConsModel.UnbilledCons_UnbUnmetConsError_J6 = 0.05;

            easyCalcModel.UnauthConsModel.IllegalConnectionsOthersEstimatedNumber_D10 = 500;
            easyCalcModel.UnauthConsModel.UnauthCons_IllegalConnOthersErrorMargin_F10 = 0.05;
            easyCalcModel.UnauthConsModel.IllegalConnectionsOthersConsumptionLitersPerConnectionPerDay_J10 = 200;

            easyCalcModel.MetErrorsModel.MetErrors_DetailedManualSpec_J6 = 2;
            easyCalcModel.MetErrorsModel.MeteredBulkSupplyExportErrorMargin_N32 = 0.02;
            easyCalcModel.MetErrorsModel.UnbilledMeteredConsumptionWithoutBulkSupplyErrorMargin_N34 = 0.02;
            easyCalcModel.MetErrorsModel.CorruptMeterReadingPracticessErrorMargin_N38 = 0.02;
            easyCalcModel.MetErrorsModel.DataHandlingErrorsOffice_L40 = 100;
            easyCalcModel.MetErrorsModel.DataHandlingErrorsOfficeErrorMargin_N40 = 0.02;

            var dictTemp = CloneDictionaryCloningValues<string, double>(_expectedDictPeriod);

            //dictTemp["SystemInputVolume_B19"] = 6593339;                         
            //dictTemp["SystemInputVolumeErrorMargin_B21"] = 0.05;                                    

            dictTemp["AuthorizedConsumption_K12"] = 5652375;
            //dictTemp["AuthorizedConsumptionErrorMargin_K15"] = 0.0;                         
            dictTemp["WaterLosses_K29"] = 940964;
            dictTemp["WaterLossesErrorMargin_K31"] = 0.35;

            dictTemp["BilledAuthorizedConsumption_T8"] = 5338026;                  
            dictTemp["UnbilledAuthorizedConsumption_T16"] = 314349;
            //dictTemp["UnbilledAuthorizedConsumptionErrorMargin_T20"] = 0.0;            
            dictTemp["CommercialLosses_T26"] = 348642;
            dictTemp["CommercialLossesErrorMargin_T29"] = 0.014;                       
            dictTemp["PhysicalLossesM3_T34"] = 592322;
            dictTemp["PhyscialLossesErrorMargin_AH35"] = 0.557;

            dictTemp["BilledMeteredConsumption_AC4"] = 5335026;
            dictTemp["BilledUnmeteredConsumption_AC9"] = 3000;            
            dictTemp["UnbilledMeteredConsumption_AC14"] = 312349;

            dictTemp["UnbilledUnmeteredConsumption_AC19"] = 2000;
            dictTemp["UnbilledUnmeteredConsumptionErrorMargin_AO20"] = 0.05;
            dictTemp["UnauthorizedConsumption_AC24"] = 8880;
            dictTemp["UnauthorizedConsumptionErrorMargin_AO25"] = 0.057;         
            dictTemp["CustomerMeterInaccuraciesAndErrorsM3_AC29"] = 339762;
            dictTemp["CustomerMeterInaccuraciesAndErrorsErrorMargin_AO30"] = 0.014;

            dictTemp["RevenueWaterM3_AY8"] = 5338026;         
            dictTemp["NonRevenueWaterM3_AY24"] = 1255313;         
            dictTemp["NonRevenueWaterErrorMargin_AY26"] = 0.263;         

            ReadSheetData_Test(easyCalcModel, dictTemp);
        }


        private void ReadSheetData_Test(EasyCalcModel easyCalcModel, Dictionary<string, double> expectedDict)
        {
            //WbEasyCalcRepository.WbEasyCalc easyCalcDataReaderMoq = new WbEasyCalcRepository.WbEasyCalc();
            //easyCalcDataReaderMoq.CalculateNew(easyCalcModel);
            WbEasyCalc.CalculateNew(easyCalcModel);

            foreach (var keyValuePair in expectedDict)
            {
                int roundFactor = keyValuePair.Key.Contains("ErrorMargin") ? 3 : 0;
                var actual = Math.Round((double)easyCalcModel.WaterBalancePeriod.GetType().GetProperty(keyValuePair.Key).GetValue(easyCalcModel.WaterBalancePeriod, null), roundFactor, MidpointRounding.AwayFromZero);
                var expected = keyValuePair.Value;
                Assert.AreEqual(expected, actual, $"{keyValuePair.Key}: actual = {actual}, expected = {expected}");
            }
        }

        private void ReadSheetData_Test(EasyCalcModel easyCalcModel, Dictionary<string, double> expectedWaterBalanceDayDict, Dictionary<string, double> expectedWaterBalancePeriodDict)
        {
            //WbEasyCalcRepository.WbEasyCalc easyCalcDataReaderMoq = new WbEasyCalcRepository.WbEasyCalc();
            //easyCalcDataReaderMoq.CalculateNew(easyCalcModel);
            WbEasyCalc.CalculateNew(easyCalcModel);

            WaterBalanceModel waterBalanceDay = easyCalcModel.WaterBalanceDay;
            WaterBalanceModel waterBalancePeriod = easyCalcModel.WaterBalancePeriod;


            foreach (var keyValuePair in expectedWaterBalanceDayDict)
            {
                int roundFactor = keyValuePair.Key.Contains("ErrorMargin") ? 3 : 0;
                var actual = Math.Round((double)easyCalcModel.WaterBalanceDay.GetType().GetProperty(keyValuePair.Key).GetValue(easyCalcModel.WaterBalanceDay, null), roundFactor, MidpointRounding.AwayFromZero);
                var expected = keyValuePair.Value;
                Assert.AreEqual(expected, actual, $"{keyValuePair.Key}: actual = {actual}, expected = {expected}");
            }
            foreach (var keyValuePair in expectedWaterBalancePeriodDict)
            {
                int roundFactor = keyValuePair.Key.Contains("ErrorMargin") ? 3 : 0;
                var actual = Math.Round((double)easyCalcModel.WaterBalancePeriod.GetType().GetProperty(keyValuePair.Key).GetValue(easyCalcModel.WaterBalancePeriod, null), roundFactor, MidpointRounding.AwayFromZero);
                var expected = keyValuePair.Value;
                Assert.AreEqual(expected, actual, $"{keyValuePair.Key}: actual = {actual}, expected = {expected}");
            }
        }

        private static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>(Dictionary<TKey, TValue> original)
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count, original.Comparer);
            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue)entry.Value);
            }
            return ret;
        }

    }
}
