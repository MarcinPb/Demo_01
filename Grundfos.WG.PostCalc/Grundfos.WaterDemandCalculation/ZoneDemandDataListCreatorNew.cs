using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grundfos.OPC;
using Grundfos.WG.Model;
using Grundfos.Workbooks;
using Haestad.Support.OOP.Logging;
using Grundfos.WaterDemandCalculation.ExtensionMethods;

namespace Grundfos.WaterDemandCalculation
{
    public class ZoneDemandDataListCreatorNew
    {
        private readonly DataContext _dataContext;
        private readonly ActionLogger _logger;

        public double GetMinutesFromMonday { get; set; }

        public ZoneDemandDataListCreatorNew(DataContext dataContext, ActionLogger logger = null)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public List<ZoneDemandData> Create(DateTime calcTime)
        {
            try
            {
                _logger?.WriteMessage(OutputLevel.Info, $"Run: ZoneDemandDataListCreatorNew.Create({calcTime})");


                DataSet dataSet = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(_dataContext.WaterInfraConnString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter
                    {
                        SelectCommand = new SqlCommand("spPostCalcGetObjDemand", sqlConn)
                        {
                            CommandType = CommandType.StoredProcedure
                        }
                    };
                    adapter.SelectCommand.Parameters.Add("@CalcTime", SqlDbType.DateTime).Value = calcTime;
                    adapter.Fill(dataSet);
                }

                List<ZoneDemandData> list = CreateList(dataSet);

                return list;
            }
            catch (Exception e)
            {
                _logger?.WriteMessage(OutputLevel.Errors, $"Getting MSSQL data.\n{e.Message}");
                throw;
            }
        }

        private List<ZoneDemandData> CreateList(DataSet dataSet)
        {
            var list = dataSet.Tables[0].AsEnumerable().Select(x => new 
            {
                ZoneId = x.Field<int>("ZoneId"),
                ZoneName = x.Field<string>("ZoneName"),
                WgDemand = x.Field<double>("WgDemand"),
                ScadaDemand = x.Field<double>("ScadaDemand"),
                DemandAdjustmentRatio = x.Field<double>("DemandAdjustmentRatio"),
                DemandAdjustmentRatioDb = x.Field<double>("DemandAdjustmentRatio"),
            })
            .Distinct()
            .Select(y => new ZoneDemandData { 
                ZoneId = y.ZoneId, 
                ZoneName = y.ZoneName,
                WgDemand = y.WgDemand,
                ScadaDemand = y.ScadaDemand,
                DemandAdjustmentRatio = y.DemandAdjustmentRatio,
                DemandAdjustmentRatioDb = y.DemandAdjustmentRatioDb,
            })
            .ToList();

            list.ForEach(x => x.Demands = CreateDemands(x.ZoneId, dataSet));

            return list;            
        }

        private List<WaterDemandData> CreateDemands(int zoneId, DataSet dataSet)
        {
            var list = dataSet.Tables[0].AsEnumerable()
                .Where(f => f.Field<int>("ZoneId") == zoneId)
                .Select(x => new WaterDemandData
                {
                    ZoneID = x.Field<int>("ZoneId"),
                    ZoneName = x.Field<string>("ZoneName"),

                    ObjectID = x.Field<int>("ObjectId"),
                    ObjectTypeID = x.Field<int>("ObjectTypeId"),
                    ObjectIsExcluded = x.Field<bool>("ObjectIsExcluded"),

                    DemandPatternID = x.Field<int>("DemandPatternId"),
                    DemandPatternName = x.Field<string>("DemandPatternName"),
                    DemandIsExcluded = x.Field<bool>("DemandIsExcluded"),

                    BaseDemandValue = x.Field<double>("BaseDemandValue"),
                    DemandFactorValue = x.Field<double>("DemandFactorValue"),
                    ActualDemandValue = x.Field<double>("ActualDemandValue"),
                    DemandCalculatedValue = x.Field<double>("DemandCalculatedValue"),
                    DemandCalculatedValueDb = x.Field<double>("DemandCalculatedValue"),
                })
                .ToList();

            return list;
        }

        public class DataContext
        {
            public string WaterInfraConnString { get; set; }
        }





        public List<int> GetExcludedObjectId(List<ZoneDemandData> zoneDemandDataList)
        {
            var list = zoneDemandDataList
                .SelectMany(x => x.Demands)
                .Where(f => f.ObjectIsExcluded == true)
                .Select(y => y.ObjectID)
                .Distinct()
                .ToList();
            
            return list;
            
            /*
            try
            {
                _logger?.WriteMessage(OutputLevel.Info, $"Run: ZoneDemandDataListCreatorNew.GetExcludedObjectId()");


                DataSet dataSet = new DataSet();
                using (SqlConnection sqlConn = new SqlConnection(_dataContext.WaterInfraConnString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter
                    {
                        SelectCommand = new SqlCommand("spGetExcludedObjectId", sqlConn)
                        {
                            CommandType = CommandType.StoredProcedure
                        }
                    };
                    adapter.Fill(dataSet);
                }

                var list = dataSet.Tables[0].AsEnumerable().Select(x => x.Field<int>("Id")).ToList();

                return list;
            }
            catch (Exception e)
            {
                _logger?.WriteMessage(OutputLevel.Errors, $"Getting MSSQL data: GetExcludedObjectId.\n{e.Message}");
                throw;
            }
            */
        }

        public List<int> GetExcludedDemandPatternId(List<ZoneDemandData> zoneDemandDataList)
        {
            var list = zoneDemandDataList
                .SelectMany(x => x.Demands)
                .Where(f => f.DemandIsExcluded==true)
                .Select(y => y.DemandPatternID)
                .Distinct()
                .ToList();

            return list;
        }


    }
}
