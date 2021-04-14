using Dapper;
using Database.DataModel;
using GeometryModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataRepository
{
    public class MainRepo
    {
        private static List<DomainObjectData> _domainObjectDataList = GetDomainObjectDataList();
        private static Dictionary<ObjectTypes, List<DomainObjectData>> _domainGrouppedObjects = GetWgObjectTypeList();

        public static List<DomainObjectData> GetDomainObjectDataList()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("Files\\Wg\\MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            List<DomainObjectData> domainObjects = (List<DomainObjectData>)formatter.Deserialize(stream);
            stream.Close();

            return domainObjects;
        }
        public static Dictionary<ObjectTypes, List<DomainObjectData>> GetWgObjectTypeList()
        {
            Dictionary<ObjectTypes, List<DomainObjectData>> domainGrouppedObjects = _domainObjectDataList
                .GroupBy(x => x.ObjectType)
                .ToDictionary(x => x.Key, x => x.ToList());

            return domainGrouppedObjects;
        }

        public static DomainObjectData GetItem(int id)
        {
            return _domainObjectDataList.FirstOrDefault(x => x.ID==id);
        }

        public static List<DomainObjectData> GetJunctionList()
        {
            return _domainGrouppedObjects[ObjectTypes.Junction];
        }

        public static Point2D GetPointTopLeft()
        {
            var junctionList = GetJunctionList();
            var xMin = junctionList.Min(x => x.Geometry[0].X);
            var yMin = junctionList.Min(x => x.Geometry[0].Y);
            return new Point2D(xMin, yMin);
        }
        public static Point2D GetPointBottomRight()
        {
            var junctionList = GetJunctionList();
            var xMax = junctionList.Max(x => x.Geometry[0].X);
            var yMax = junctionList.Max(x => x.Geometry[0].Y);
            return new Point2D(xMax, yMax);
        }

        public static List<DomainObjectData> GetCustomerNodeList()
        {
            return _domainGrouppedObjects[ObjectTypes.CustomerNode];
        }

        public static List<DomainObjectData> GetPipeList()
        {
            return _domainGrouppedObjects[ObjectTypes.Pipe];
        }



        #region Database WaterInfra_5
        private static string GetConnectionString(string name = "DapperDB")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
        #endregion

        #region Database WaterInfra_4

        // Import data from \Wg\MyFile.bin file to WaterInfra_4 database.
        public static void ImportInfraToDatabase()
        {
            //var list = GetPipeList().Where(x => (int)x.Fields["HMITopologyStartNodeID"] == (int)x.Fields["HMITopologyStopNodeID"]).ToList();

            //var list = GetCustomerNodeList().Select(x => new InfraObj() 
            var infraObjectList = GetDomainObjectDataList().Select(x => new InfraObj() 
            { 
                ObjId = x.ID,
                ObjTypeId = (int)x.ObjectType,
            })
            //.Take(10)
            .ToList();

            var infraConnectionCustomerNodeList = GetCustomerNodeList()
                .Select(x => new  
                    {
                        ParentObjId = GetJunctionList().FirstOrDefault(j => j.Label==(string)x.Fields["Demand_AssociatedElement"])?.ID,
                        ChildObjId = x.ID,
                    })
                .Where(y => y.ParentObjId!=null)
                .Select(z => new InfraConn 
                    {
                        ParentObjId = (int)z.ParentObjId,
                        ChildObjId = z.ChildObjId
                    })
                //.ToList()
                ;

            var infraConnectionPipeStartList = GetPipeList()
                .Select(x => new  
                    {
                        ParentObjId = (int?)x.Fields["HMITopologyStartNodeID"],
                        ChildObjId = x.ID,
                    })
                .Where(y => y.ParentObjId!=null)
                .Select(z => new InfraConn 
                    {
                        ParentObjId = (int)z.ParentObjId,
                        ChildObjId = z.ChildObjId,
                        ConnTypeId = 1,
                    })
                //.ToList()
                ;

            var infraConnectionPipeStopList = GetPipeList()
                .Select(x => new  
                    {
                        ParentObjId = (int?)x.Fields["HMITopologyStopNodeID"],
                        ChildObjId = x.ID,
                    })
                .Where(y => y.ParentObjId!=null)
                .Select(z => new InfraConn 
                    {
                        ParentObjId = (int)z.ParentObjId,
                        ChildObjId = z.ChildObjId,
                        ConnTypeId = 2,
                    })
                //.ToList()
                ;

            var infraConnectionList = infraConnectionCustomerNodeList                
                .Union(infraConnectionPipeStartList)
                .Union(infraConnectionPipeStopList)
                .Where(x => _domainObjectDataList.Any(y => x.ChildObjId==y.ID) && _domainObjectDataList.Any(y => x.ParentObjId==y.ID))
                .ToList();

            List<InfraField> infraFieldList = MainRepo.GetInfraFieldList();

            var infraObjectFieldList = infraObjectList
                .Join(
                    _domainObjectDataList,
                    l => l.ObjId,
                    r => r.ID,
                    (l, r) => new { l.ObjId, l.ObjTypeId, r.Fields }
                )
                .SelectMany(x => x.Fields, (x, lst) => new { 
                    ObjId = x.ObjId, 
                    ObjTypeId = x.ObjTypeId, 
                    FieldName = lst.Key, 
                    FieldValue = lst.Value 
                })
                .Join(
                    infraFieldList,
                    l => l.ObjTypeId.ToString() + l.FieldName,
                    r => r.ObjTypeId.ToString() + r.Name,
                    (l, r) => new InfraValue { 
                        FieldId = r.FieldId, 
                        ObjId = l.ObjId, 
                        FloatValue = r.DataTypeId==1 ? (double?)Convert.ToDouble(l.FieldValue) : null,
                        StringValue = r.DataTypeId==2 ? (string)l.FieldValue : null,
                    }
                )
                .ToList();

            WriteSet(infraObjectList, infraConnectionList, infraObjectFieldList);
        }

        private static void WriteSet(List<InfraObj> infraObjectList, List<InfraConn> infraConnectionList, List<InfraValue> infraObjectFieldValueList)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"
                    DELETE FROM dbo.tbInfraObj;
                ";
                cnn.Execute(sql, infraObjectList);

                sql = $@"
                    INSERT INTO dbo.tbInfraObj (
                        ObjId,  ObjTypeId,  Name,  Description,  IsActive,  Xx,  Yy
                    ) VALUES (
                        @ObjId, @ObjTypeId, @Name, @Description, @IsActive, @Xx, @Yy
                    );
                ";
                cnn.Execute(sql, infraObjectList);

                sql = $@"
                    INSERT INTO dbo.tbInfraConn (
                        ParentObjId,  ChildObjId, ConnTypeId
                    ) VALUES (
                        @ParentObjId, @ChildObjId, @ConnTypeId
                    )
                ";
                cnn.Execute(sql, infraConnectionList);

                sql = $@"
                    INSERT INTO dbo.tbInfraValue (
                        FieldId,  ObjId, FloatValue, StringValue
                    ) VALUES (
                        @FieldId, @ObjId, @FloatValue, @StringValue
                    )
                ";
                cnn.Execute(sql, infraObjectFieldValueList);

            }
        }

        // Get data from database
        public static List<InfraField> GetInfraFieldList()
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                string sql;

                sql = $@"SELECT * FROM dbo.tbInfraField;";
                var list = cnn.Query<InfraField>(sql);
                return list.ToList();
            }
        }


        #endregion


        #region Database WaterInfra_5

        public static void InsertToInfraObjType(IDictionary<int,string> dict)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString("WaterInfra_5_ConnStr")))
            {
                string sql;

                sql = $@"
                    DELETE FROM dbo.tbInfraObjType;
                ";
                cnn.Execute(sql, dict.Select(x => new { ObjTypeId = x.Key, Name = x.Value}));

                sql = $@"
                    INSERT INTO dbo.tbInfraObjType (
                        ObjTypeId,  Name
                    ) VALUES (
                        @ObjTypeId, @Name
                    );
                ";
            }
        }

        #endregion


    }
}
