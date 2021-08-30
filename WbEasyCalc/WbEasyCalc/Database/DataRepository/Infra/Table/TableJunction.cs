using Dapper;
using Database.DataModel.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataRepository.Infra.Table
{
    public class TableJunction
    {
        private string _connectionString;

        public TableJunction(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<DemandSettingObj> GetList()
        {
            using (IDbConnection cnn = new SqlConnection(_connectionString))
            {
                string sql;

                sql = $@"
                    SELECT 
	                    ObjectId AS ObjId,
	                    BaseDemandValue AS DemandBaseValue,
	                    DemandPatternId,
                        CAST(CASE WHEN tbExcelExcludedObj.Id IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsExcluded
                    FROM 
	                    tbExcelObjectData
                        LEFT OUTER JOIN tbExcelExcludedObj ON tbExcelObjectData.ObjectId = tbExcelExcludedObj.Id
                        ;
                ";
                return cnn.Query<DemandSettingObj>(sql).ToList();
            }
        }

        public int SaveItem(DemandSettingObj model)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@id", model.ObjId, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                // input
                p.Add("@DemandBaseValue", model.DemandBaseValue);
                p.Add("@DemandPatternId", model.DemandPatternId);
                p.Add("@IsExcluded", model.IsExcluded);

                connection.Execute("dbo.spPostCalcDemandSettingsSave", p, commandType: CommandType.StoredProcedure);

                model.ObjId = p.Get<int>("@id");

                return model.ObjId;
            }
        }
    }
}
