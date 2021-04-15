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
    public class DesignerBinFileRepo : IDesignerRepo
    {
        const string binFile = "Files\\Wg\\MyFile.bin";

        private static readonly List<DomainObjectData> _domainObjectDataList = GetDomainObjectDataList();
        private static readonly Dictionary<ObjectTypes, List<DomainObjectData>> _domainGrouppedObjects = GetWgObjectTypeList();


        public DomainObjectData GetItem(int id)
        {
            return _domainObjectDataList.FirstOrDefault(x => x.ID == id);
        }

        public List<DomainObjectData> GetJunctionList()
        {
            return _domainGrouppedObjects[ObjectTypes.Junction];
        }
        public List<DomainObjectData> GetPipeList()
        {
            return _domainGrouppedObjects[ObjectTypes.Pipe];
        }
        public List<DomainObjectData> GetCustomerNodeList()
        {
            return _domainGrouppedObjects[ObjectTypes.CustomerNode];
        }

        public Point2D GetPointTopLeft()
        {
            var junctionList = GetJunctionList();
            var xMin = junctionList.Min(x => x.Geometry[0].X);
            var yMin = junctionList.Min(x => x.Geometry[0].Y);
            return new Point2D(xMin, yMin);
        }
        public Point2D GetPointBottomRight()
        {
            var junctionList = GetJunctionList();
            var xMax = junctionList.Max(x => x.Geometry[0].X);
            var yMax = junctionList.Max(x => x.Geometry[0].Y);
            return new Point2D(xMax, yMax);
        }


        private static List<DomainObjectData> GetDomainObjectDataList()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(binFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            List<DomainObjectData> domainObjects = (List<DomainObjectData>)formatter.Deserialize(stream);
            stream.Close();

            return domainObjects;
        }
        private static Dictionary<ObjectTypes, List<DomainObjectData>> GetWgObjectTypeList()
        {
            Dictionary<ObjectTypes, List<DomainObjectData>> domainGrouppedObjects = _domainObjectDataList
                .GroupBy(x => x.ObjectType)
                .ToDictionary(x => x.Key, x => x.ToList());

            return domainGrouppedObjects;
        }
    }
}
