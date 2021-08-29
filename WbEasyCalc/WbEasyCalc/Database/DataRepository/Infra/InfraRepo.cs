using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.DataModel.Infra;
using Database.DataRepository.Infra.Table;

namespace Database.DataRepository.Infra
{
    public class InfraRepo
    {
        public static TableCustomerMeter TableCustomerMeter { get; } = new TableCustomerMeter(GetConnectionString());


        #region Fill constant data: tbInfraObjType, tbInfraObjTypeField, tbInfraField, tbInfraCategory, tbInfraUnitCorrection, (tbInfraFieldTemp) 

        public static void InsertToInfraObjType(List<InfraObjType> list)
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
                cnn.Execute(sql, list);
            }
        }

        public static void InsertToInfraField(List<ImportedField> list)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    DELETE FROM dbo.tbInfraFieldTemp;
                    DELETE FROM dbo.tbInfraField;
                    --DELETE FROM dbo.tbInfraObjTypeField;
                    DELETE FROM dbo.tbInfraCategory;
                    DELETE FROM dbo.tbInfraUnitCorrection;
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

                sql = $@" 
                    INSERT INTO [dbo].[tbInfraUnitCorrection](
                        [UnitCorrectionId]
                       ,[Value]
                       ,[Description]
                    ) VALUES (
                        1   ,0.304800000001219     ,N'[ft] -> [m]'
                    );
                    INSERT INTO [dbo].[tbInfraUnitCorrection](
                        [UnitCorrectionId]
                       ,[Value]
                       ,[Description]
                    ) VALUES (
                        2   ,101.940646927238     ,N'[ft3/s] -> [m3/h]'
                    );
                    INSERT INTO [dbo].[tbInfraUnitCorrection](
                        [UnitCorrectionId]
                       ,[Value]
                       ,[Description]
                    ) VALUES (
                        3   ,304.800000001219     ,N'[ft] -> [mm]'
                    );
                    
                    UPDATE [dbo].[tbInfraField] SET 
                        [UnitCorrectionId] = 1
                    WHERE 
                        --[FieldId] IN (-334626530, 586, 588, 690, 192086302);
                        [Name] IN ('HMIGeometryYCoordinate', 'HMIGeometry', 'HMIGeometryScaledLength', 'Physical_HydrantLateralLength', 'HMIGeometryXCoordinate');

                    UPDATE [dbo].[tbInfraField] SET 
                        [UnitCorrectionId] = 2
                    WHERE 
                        --[FieldId] IN (767, 757);
                        [Name] IN ('Demand_BaseFlow', 'DemandCollection');

                    UPDATE [dbo].[tbInfraField] SET 
                        [UnitCorrectionId] = 3
                    WHERE 
                        --[FieldId] IN (659);
                        [Name] IN ('Physical_PipeDiameter');
                ";
                cnn.Execute(sql);
            }
        }

        #endregion

        #region Fill changeable data: tbInfraZone, tbInfraDemandPattern, tbInfraObj, tbInfraValue, tbInfraGeometry 

        public static void InsertToInfraZone(List<InfraZone> list)
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
                cnn.Execute(sql, list);
            }
        }

        public static void InsertToInfraDemandPattern(List<InfraDemandPattern> list)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    DELETE FROM dbo.tbInfraDemandPattern;
                ";
                cnn.Execute(sql);

                sql = $@"
                    INSERT INTO dbo.tbInfraDemandPattern (
                        DemandPatternId,  Name
                    ) VALUES (
                        @DemandPatternId, @Name
                    );
                ";
                cnn.Execute(sql, list);
            }
        }
        public static void InsertToInfraDemandPatternCurve(List<InfraDemandPatternCurve> list)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    DELETE FROM dbo.tbInfraDemandPatternCurve;
                ";
                cnn.Execute(sql);

                sql = $@"
                    INSERT INTO dbo.tbInfraDemandPatternCurve (
                        DemandPatternCurveId,  DemandPatternId,  TimeFromStart,  Multiplier
                    ) VALUES (
                        @DemandPatternCurveId, @DemandPatternId, @TimeFromStart, @Multiplier
                    );
                ";
                cnn.Execute(sql, list);
            }
        }

        public static void InsertToInfraDemandBase(List<InfraDemandBase> list)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    DELETE FROM dbo.tbInfraDemandBase;
                ";
                cnn.Execute(sql);

                sql = $@"
                    INSERT INTO dbo.tbInfraDemandBase (
                        ValueId,  DemandBase,  DemandPatternId
                    ) VALUES (
                        @ValueId, @DemandBase, @DemandPatternId
                    );
                ";
                cnn.Execute(sql, list);
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

        #endregion

        #region Get data: GetInfraData()

        private static InfraData _infraData;
        public static InfraData GetInfraData()
        {
            if (_infraData == null)
            {
                InfraData infraData = new InfraData();
                infraData.InfraConstantData = GetInfraConstantData();
                infraData.InfraChangeableData = GetInfraChangableData();
                if (infraData.InfraChangeableData.InfraObjList.Count > 0) 
                { 
                    infraData.Recalculate();
                }

                _infraData = infraData;
            }
            return _infraData;
        }

        public static InfraConstantDataLists GetInfraConstantData()
        {
            List<InfraObjType> infraObjTypeList;
            List<InfraObjTypeField> infraObjTypeFieldList;
            List<InfraField> infraFieldList;
            List<InfraUnitCorrection> infraUnitCorrectionList;

            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"SELECT * FROM dbo.tbInfraObjType;";
                infraObjTypeList = cnn.Query<InfraObjType>(sql).ToList();

                sql = $@"SELECT * FROM dbo.tbInfraObjTypeField;";
                infraObjTypeFieldList = cnn.Query<InfraObjTypeField>(sql).ToList();

                sql = $@"SELECT * FROM dbo.tbInfraField;";
                infraFieldList = cnn.Query<InfraField>(sql).ToList();

                sql = $@"SELECT * FROM dbo.tbInfraUnitCorrection;";
                infraUnitCorrectionList = cnn.Query<InfraUnitCorrection>(sql).ToList();
            }

            InfraConstantDataLists result = new InfraConstantDataLists
            {
                InfraObjTypeList = infraObjTypeList,
                InfraObjTypeFieldList = infraObjTypeFieldList,
                InfraFieldList = infraFieldList,
                InfraUnitCorrectionList = infraUnitCorrectionList
            };

            return result;
        }

        private static InfraChangeableDataLists GetInfraChangableData()
        {
            List<InfraObj> infraObjList;
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    SELECT * FROM dbo.tbInfraObj;
                ";
                infraObjList = cnn.Query<InfraObj>(sql).ToList();
            }

            List<InfraValue> infraValueList;
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    SELECT * FROM dbo.tbInfraValue;
                ";
                infraValueList = cnn.Query<InfraValue>(sql).ToList();
            }

            List<InfraGeometry> infraGeometryList;
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    SELECT * FROM dbo.tbInfraGeometry;
                ";
                infraGeometryList = cnn.Query<InfraGeometry>(sql).ToList();
            }

            List<InfraZone> zoneDict;
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    SELECT * FROM dbo.tbInfraZone;
                ";
                zoneDict = cnn.Query<InfraZone>(sql).ToList();
            }

            List<InfraDemandPattern> demandPatternDict;
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    SELECT * FROM dbo.tbInfraDemandPattern;
                ";
                demandPatternDict = cnn.Query<InfraDemandPattern>(sql).ToList();
            }

            List<InfraDemandPatternCurve> demandPatternCurveList;
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    SELECT * FROM dbo.tbInfraDemandPatternCurve;
                ";
                demandPatternCurveList = cnn.Query<InfraDemandPatternCurve>(sql).ToList();
            }

            List<InfraDemandBase> demandBaseList;
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    SELECT * FROM dbo.tbInfraDemandBase;
                ";
                demandBaseList = cnn.Query<InfraDemandBase>(sql).ToList();
            }

            List<DemandSettingObj> demandSettingObjList;
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    SELECT ObjectId AS ObjId, DemandPatternId, BaseDemandValue AS DemandBaseValue FROM dbo.tbExcelObjectData;
                ";
                demandSettingObjList = cnn.Query<DemandSettingObj>(sql).ToList();
            }

            InfraChangeableDataLists importedDataOutputLists = new InfraChangeableDataLists
            {
                InfraObjList = infraObjList,
                InfraValueList = infraValueList,
                InfraGeometryList = infraGeometryList,
                ZoneDict = zoneDict,
                DemandPatternDict = demandPatternDict,
                DemandPatternCurveList = demandPatternCurveList,
                DemandBaseList = demandBaseList,
                DemandSettingObjList = demandSettingObjList,
            };

            return importedDataOutputLists;
        }

        #endregion

        private static string GetConnectionString(string name = "WaterInfra_ConnStr")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
