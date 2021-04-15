using GeometryModel;
using GeometryReader;
using GeometryReader.ObjectReaders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GeometryReader.Test
{
    [TestClass]
    public class UnitTest1
    {
        private string _sqliteFile = @"K:\temp\sandbox\Nowy model testowy\testOPC.wtg.sqlite";  //@"K:\temp\sandbox\Nowy model testowy\_archiw\2019-11-19\testOPC.wtg.sqlite";
        private string _binFile = @"K:\temp\sandbox\Nowy model testowy\WgData.bin";


        [TestMethod]
        public void TestMethod1()
        {
            List<DomainObjectData> domainObjects;
            using (var dataSetProvider = new DomainDataSetProxy(_sqliteFile))
            {
                domainObjects = GetWgObjects(dataSetProvider);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(_sqliteFile, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, domainObjects);
            stream.Close();
        }

        [TestMethod]
        public void TestMethod3()
        {
            Dictionary<int, string> zoneDict;
            Dictionary<int, string> objTypeDict;
            List<DomainObjectData> domainObjects;
            using (var dataSetProvider = new DomainDataSetProxy(_sqliteFile))
            {
                var reader = new GenericObjectReader(dataSetProvider);

                zoneDict = reader.ReadZoneList();
                objTypeDict = reader.ReadOjectTypeList();
                domainObjects = GetWgObjects(dataSetProvider);
            }
            WaterGemsData waterGemsData = new WaterGemsData()
            {
                ObjTypeDict = objTypeDict,
                ZoneDict = zoneDict,
                DomainObjectDataList = domainObjects,
            };

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(_binFile, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, waterGemsData);
            stream.Close();

            //Dictionary<ObjectTypes, List<DomainObjectData>> domainGrouppedObjects = domainObjects
            //    .GroupBy(x => x.ObjectType)
            //    .ToDictionary(x => x.Key, x => x.ToList());
        }
        private static Dictionary<int, string> GetWgZoneDict(DomainDataSetProxy dataSetProvider)
        {
            var reader = new GenericObjectReader(dataSetProvider);
            return reader.ReadZoneList();
        }

        private static Dictionary<int, string> GetWgObjTypeDict(DomainDataSetProxy dataSetProvider)
        {
            var reader = new GenericObjectReader(dataSetProvider);
            return reader.ReadOjectTypeList();
        }



        [TestMethod]
        public void TestMethod2()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            List<DomainObjectData> domainObjects = (List<DomainObjectData>)formatter.Deserialize(stream);
            stream.Close();

            Dictionary<ObjectTypes, List<DomainObjectData>> domainGrouppedObjects = domainObjects
                .GroupBy(x => x.ObjectType)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        [TestMethod]
        public void TestMethod4()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(_binFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            WaterGemsData domainObjects = (WaterGemsData)formatter.Deserialize(stream);
            stream.Close();

            Dictionary<ObjectTypes, List<DomainObjectData>> domainGrouppedObjects = domainObjects.DomainObjectDataList
                .GroupBy(x => x.ObjectType)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        [TestMethod]
        public void TestMethod5()
        {
            Dictionary<int, string> zoneDict;
            Dictionary<int, string> objTypeDict;
            List<DomainObjectData> domainObjects;
            using (var dataSetProvider = new DomainDataSetProxy(_sqliteFile))
            {
                var reader = new GenericObjectReader(dataSetProvider);

                zoneDict = reader.ReadZoneList();
                objTypeDict = reader.ReadOjectTypeList();
                domainObjects = GetWgObjects2(dataSetProvider, reader.ReadOjectTypeList());
            }
            WaterGemsData waterGemsData = new WaterGemsData()
            {
                ObjTypeDict = objTypeDict,
                ZoneDict = zoneDict,
                DomainObjectDataList = domainObjects,
            };

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(_binFile, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, waterGemsData);
            stream.Close();

            //Dictionary<ObjectTypes, List<DomainObjectData>> domainGrouppedObjects = domainObjects
            //    .GroupBy(x => x.ObjectType)
            //    .ToDictionary(x => x.Key, x => x.ToList());
        }

        private static List<DomainObjectData> GetWgObjects2(DomainDataSetProxy dataSetProvider, Dictionary<int, string> typeDict)
        {
            var junctionReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.Pipe);
            List<DomainObjectData> domainObjectDataList = new List<DomainObjectData>();
            
            foreach(var type in typeDict)
            {
                var junctions = junctionReader.ReadObjects(type.Key);
                domainObjectDataList.AddRange(junctions);
            }

            return domainObjectDataList;
        }
        private static List<DomainObjectData> GetWgObjects(DomainDataSetProxy dataSetProvider)
        {
            //log.Info("Reading WaterGEMS objects.");

            var pipeReader = new PipeReader(dataSetProvider);
            var pipes = pipeReader.ReadObjects(new List<string>());

            var junctionReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.Junction);
            var junctions = junctionReader.ReadObjects(new List<string>());

            var hydrantReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.IdahoHydrant);
            var hydrants = hydrantReader.ReadObjects(new List<string>());

            var reservoirReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.Reservoir);
            var reservoirs = reservoirReader.ReadObjects(new List<string>());

            var tankReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.Tank);
            var tanks = tankReader.ReadObjects(new List<string>());

            var pumpReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.StandardPump);
            var pumps = pumpReader.ReadObjects(new List<string>());

            var pumpBatteryReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.VariableSpeedPumpBattery);
            var batteries = pumpBatteryReader.ReadObjects(new List<string>());

            var pbvReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.PBV);
            var pbvs = pbvReader.ReadObjects(new List<string>());

            var tcvReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.TCV);
            var tcvs = tcvReader.ReadObjects(new List<string>());

            var prvReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.PRV);
            var prvs = prvReader.ReadObjects(new List<string>());

            var psvReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.PSV);
            var psvs = psvReader.ReadObjects(new List<string>());

            var fcvReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.FCV);
            var fcvs = fcvReader.ReadObjects(new List<string>());

            var gpvReader = new GenericObjectReader(dataSetProvider, GeometryReader.Constants.ObjectTypes.GPV);
            var gpvs = gpvReader.ReadObjects(new List<string>());

            var custReader = new CustomerMeterReader(dataSetProvider);
            var cust = custReader.ReadObjects(new List<string>());

            var allObjects = 
                 pipes
                .Union(junctions)
                .Union(hydrants)
                .Union(reservoirs)
                .Union(tanks)
                .Union(pumps)
                .Union(batteries)
                .Union(pbvs)
                .Union(tcvs)
                .Union(prvs)
                .Union(psvs)
                .Union(fcvs)
                .Union(gpvs)
                .Union(cust)

                .ToList();
            //log.Info("{0} WaterGEMS objects were read.", allObjects.Count);
            return allObjects;
        }

        //private static DomainObjectDataSerialized MapToserialized(DomainObjectData domainObjectData)
        //{
        //    var domainObjectDataSerialized = new DomainObjectDataSerialized()
        //    {
        //        ID = domainObjectData.ID,
        //        Label = domainObjectData.Label,
        //        Zone = domainObjectData.Zone,
        //        IsActive = domainObjectData.IsActive,
        //    };

        //    return domainObjectDataSerialized;
        //}


        //private string Serialize(DomainObjectData value)
        //{
        //    if (value == null)
        //    {
        //        return string.Empty;
        //    }
        //    try
        //    {
        //        var xmlserializer = new XmlSerializer(typeof(DomainObjectData));
        //        var stringWriter = new StringWriter();
        //        using (var writer = XmlWriter.Create(stringWriter))
        //        {
        //            xmlserializer.Serialize(writer, value);
        //            return stringWriter.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("An error occurred", ex);
        //    }
        //}

    }
}
