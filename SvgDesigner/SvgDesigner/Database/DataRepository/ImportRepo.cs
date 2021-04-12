using Dapper;
using Database.DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataRepository
{
    public class ImportRepo
    {
        public static void InsertToInfraObjType(IDictionary<int, string> dict)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    DELETE FROM dbo.tbInfraObjType;
                ";
                cnn.Execute(sql);

                sql = $@"
                    INSERT INTO dbo.tbInfraObjType (
                        ObjTypeId,  Name
                    ) VALUES (
                        @ObjTypeId, @Name
                    );
                ";
                cnn.Execute(sql, dict.Select(x => new { ObjTypeId = x.Key, Name = x.Value }));
            }
        }
        public static void InsertToInfraField(List<ImportedField> list)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    DELETE FROM dbo.tbInfraFieldTemp;
                    DELETE FROM dbo.tbInfraObjTypeField;
                    DELETE FROM dbo.tbInfraField;
                    DELETE FROM dbo.tbInfraCategory;
                ";
                cnn.Execute(sql);

                sql = $@"    
                    INSERT INTO dbo.tbInfraFieldTemp (
                    --INSERT INTO @TbInfraFieldTemp (
                        ObjTypeId,  FieldId,  Name,  DataTypeId,  Description,  Label,  Category,  FieldTypeId
                    ) VALUES (
                        @ObjTypeId, @FieldId, @Name, @DataTypeId, @Description, @Label, @Category, @FieldTypeId
                    );
                ";
                cnn.Execute(sql, list.Select(x => new {
                    ObjTypeId = x.ObjTypeId,
                    FieldId = x.Id, 
                    Name = x.Name, 
                    DataTypeId = x.DataTypeId, 
                    Description = x.Notes,
                    Label = x.Label,
                    Category = x.Category,
                    FieldTypeId = x.FieldTypeId
                }));

                sql = $@"    
                    WITH t01 AS 
                    (
	                    SELECT DISTINCT
		                    [Category]
	                    FROM 
		                    [dbo].[tbInfraFieldTemp]
                    )
                    INSERT INTO [dbo].[tbInfraCategory](
                            [CategoryId]
                        ,[Name]
                    )
                    SELECT
	                    ROW_NUMBER() OVER (Order by [Category]) AS CategoryId,
	                    [Category] as [Name]
                    FROM
	                    t01;

                    INSERT INTO [dbo].[tbInfraField](
	                     [FieldId]
                        ,[CategoryId]
	                    ,[DataTypeId]
	                    ,[Name]
						,[Label]
	                    ,[Description]
                    )
                    SELECT DISTINCT 
                         [FieldId]
                        ,[CategoryId]
                        ,[DataTypeId]
                        ,tTemp.[Name]
						,[Label]
                        ,[Description]
                    FROM 
	                    dbo.tbInfraFieldTemp tTemp
						INNER JOIN dbo.tbInfraCategory tCat ON tTemp.Category = tCat.[Name];
	                    --@TbInfraFieldTemp; 

                    INSERT INTO [dbo].[tbInfraObjTypeField](
                         [FieldId]
	                    ,[ObjTypeId]
                    )
                    SELECT 
                         [FieldId]
	                    ,[ObjTypeId]
                    FROM 
	                    dbo.tbInfraFieldTemp;
	                    --@TbInfraFieldTemp;
                ";
                cnn.Execute(sql);

                //sql = $@"    
                //";
                //cnn.Execute(sql);

                //sql = $@"    
                //";
                //cnn.Execute(sql);
            }
        }

        public static void InsertToInfraZone(IDictionary<int, string> dict)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    DELETE FROM dbo.tbInfraZone;
                ";
                cnn.Execute(sql);

                sql = $@"
                    INSERT INTO dbo.tbInfraZone (
                        ZoneId,  Name
                    ) VALUES (
                        @ZoneId, @Name
                    );
                ";
                cnn.Execute(sql, dict.Select(x => new { ZoneId = x.Key, Name = x.Value }));
            }
        }

        public static void InsertToInfraValue(List<InfraValue> infraValueList)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    DELETE FROM dbo.tbInfraValue;
                ";
                cnn.Execute(sql);

                sql = $@"
                    INSERT INTO dbo.tbInfraValue (
                        ValueId,
                        FieldId,
	                    ObjId,
	                    IntValue,
	                    FloatValue,
	                    StringValue,
	                    BooleanValue,
	                    DateTimeValue
                    ) VALUES (
                        @ValueId,
                        @FieldId,
	                    @ObjId,
	                    @IntValue,
	                    @FloatValue,
	                    @StringValue,
	                    @BooleanValue,
	                    @DateTimeValue
                    );
                ";
                cnn.Execute(sql, infraValueList);
            }
        }








        public static IEnumerable<InfraObjType> GetObjTypeList()
        {
            IEnumerable<InfraObjType> list;

            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    SELECT * FROM dbo.tbInfraObjType;
                ";
                list = cnn.Query<InfraObjType>(sql);
            }
            return list;
        }

        public static IEnumerable<InfraObjTypeField> GetObjTypeFieldList()
        {
            IEnumerable<InfraObjTypeField> list;
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    SELECT * FROM dbo.tbInfraObjTypeField;
                ";
                list = cnn.Query<InfraObjTypeField>(sql);
            }

            return list;
        }
        public static List<InfraField> GetFieldList()
        {
            List<InfraField> list;
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    SELECT * FROM dbo.tbInfraField;
                ";
                list = cnn.Query<InfraField>(sql).ToList();
            }

            return list;
        }

        public static void InsertToInfraGeometry(List<InfraGeometry> infraGeometryList)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    DELETE FROM dbo.tbInfraGeometry;
                ";
                cnn.Execute(sql);

                sql = $@"
                    INSERT INTO dbo.tbInfraGeometry (
                        ValueId,  OrderNo,  Xp,  Yp
                    ) VALUES (
                        @ValueId, @OrderNo, @Xp, @Yp
                    );
                ";
                cnn.Execute(sql, infraGeometryList);
            }
        }

        public static void InsertToInfraObj(List<InfraObj> infraObjList)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    DELETE FROM dbo.tbInfraObj;
                ";
                cnn.Execute(sql);

                sql = $@"
                    INSERT INTO dbo.tbInfraObj (
                        ObjId,  ObjTypeId
                    ) VALUES (
                        @ObjId, @ObjTypeId
                    );
                ";
                cnn.Execute(sql, infraObjList);
            }
        }



        private static string GetConnectionString(string name = "WaterInfra_5_ConnStr")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
