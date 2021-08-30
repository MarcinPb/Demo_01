using Dapper;
using Database.DataModel.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataRepository.Infra.Demand
{
    public class ExcludedDemmandPattern
    {
        private string _connectionString;

        public ExcludedDemmandPattern(string connectionString)
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
	                    DemandPatternId
                    FROM 
	                    tbExcelExcludedDemPatt
                        ;
                ";
                return cnn.Query<DemandSettingObj>(sql).ToList();
            }
        }

        public int SaveItem(int id, bool isExcluded)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@id", id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                p.Add("@IsExcluded", isExcluded);

                connection.Execute("dbo.spPostCalcDemandPatternSave", p, commandType: CommandType.StoredProcedure);

                return id;
            }
        }
    }
}
