using Database.DataModel;
using Database.DataRepository;
using GeometryModel;
using GeometryReader;
using Haestad.Domain;
using Haestad.Domain.ModelingObjects;
using Haestad.Domain.ModelingObjects.Water;
using Haestad.Support.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryReader
{
    public class Reader
    {
        public static void ReadBase(string fileName)
        {
            using (DomainDataSetProxy domainDataSetProxy = new DomainDataSetProxy(fileName))
            {
                IDomainDataSet domainDataSet = domainDataSetProxy.OpenDomainDataSet();

                // ObjectType list
                IDictionary<int, string> objectTypeDict = domainDataSet.DomainDataSetType().DomainElementTypes().Cast<IDomainElementType>().ToDictionary(x => x.Id, x => x.Label);
                ImportRepo.InsertToInfraObjType(objectTypeDict);

                // SuportedField list
                List<ImportedField> supportedFieldDict = new List<ImportedField>();
                foreach (int objectTypeId in objectTypeDict.Keys)
                {
                    var manager = domainDataSet.DomainElementManager(objectTypeId);
                    var manager1 = domainDataSet.FieldManager;
                    // Get list suported fields for a particular objectTypeId. 
                    IEnumerable<IField> supportedFields = manager.SupportedFields().Cast<IField>();
                    supportedFieldDict.AddRange(supportedFields.Select(x => new ImportedField() {
                        Id = x.Id,
                        ObjTypeId = objectTypeId,
                        Name = x.Name,
                        Label = x.Label,
                        Notes = x.Notes,
                        Category = x.Category,
                        DataTypeId = (int)x.FieldDataType,
                        FieldTypeId = 1,    // not result field
                    }));

                    //var supportedResultFields = manager.SupportedResultFields().Cast<IField>();;
                    //supportedFieldDict.AddRange(supportedResultFields.Select(x => new ImportedField()
                    //{
                    //    Id = x.Id,
                    //    ObjTypeId = objectTypeId,
                    //    Name = x.Name,
                    //    Label = x.Label,
                    //    Notes = x.Notes,
                    //    Category = x.Category,
                    //    DataTypeId = (int)x.FieldDataType,
                    //    FieldTypeId = 2,    // result field
                    //}));
                }
                ImportRepo.InsertToInfraField(supportedFieldDict);

                // Zone list
                IdahoDomainDataSet idahoDomainDataSet = (IdahoDomainDataSet)domainDataSet;
                IDictionary<int, string> zoneDict = idahoDomainDataSet.ZoneElementManager.Elements().Cast<ModelingElementBase>().ToDictionary(x => x.Id, x => x.Label);
                ImportRepo.InsertToInfraZone(zoneDict);
            }
        }

        public static void ReadData(string fileName)
        {
            using (DomainDataSetProxy domainDataSetProxy = new DomainDataSetProxy(fileName))
            {
                IDomainDataSet domainDataSet = domainDataSetProxy.OpenDomainDataSet();


                // Zone list
                IdahoDomainDataSet idahoDomainDataSet = (IdahoDomainDataSet)domainDataSet;
                IDictionary<int, string> zoneDict = idahoDomainDataSet.ZoneElementManager.Elements().Cast<ModelingElementBase>().ToDictionary(x => x.Id, x => x.Label);
                ImportRepo.InsertToInfraZone(zoneDict);


                // InfraObj list
                List<InfraObj> infraObjList = new List<InfraObj>();
                List<InfraValue> infraValueList = new List<InfraValue>();
                List<InfraGeometry> infraGeometryList = new List<InfraGeometry>();

                List<int> infraObjTypeList = ImportRepo.GetObjTypeList().Select(x => x.ObjTypeId).ToList();
                List<InfraObjTypeField> infraObjTypeFieldList = ImportRepo.GetObjTypeFieldList().ToList();
                List<InfraField> infraFieldList = ImportRepo.GetFieldList();
                
                foreach (int objTypeId in infraObjTypeList
                    //.Where(x => x==69)                
                    )
                { 
                    // InfraObj
                    var manager = domainDataSet.DomainElementManager(objTypeId);
                    var supportedFields = manager.SupportedFields().Cast<IField>();
                    var objIdList = manager.ElementIDs();
                    foreach (var objId in objIdList)
                    {
                        var infraObj = new InfraObj
                        {
                            ObjId = objId,
                            ObjTypeId = objTypeId,
                        };
                        infraObjList.Add(infraObj);

                        // InfraValue
                        var fieldList = infraFieldList.Where(x => infraObjTypeFieldList.Where(y => y.ObjTypeId == objTypeId).Any(y => x.FieldId == y.FieldId));
                        foreach (var field in fieldList)
                        {
                            var supportedField = supportedFields.FirstOrDefault(x => x.Id==field.FieldId);
                            var infraValue = new InfraValue
                            {
                                ValueId = infraValueList.Count + 1,
                                FieldId = field.FieldId,
                                ObjId = objId,
                            };
                            switch (field.DataTypeId)
                            {
                                case 1:     // Int
                                    infraValue.IntValue = (int)supportedField.GetValue(objId);
                                    break;
                                case 2:     // Real
                                    //infraValue.FloatValue = CheckAndGetDouble((double)supportedField.GetValue(objId));
                                    ManageReal(supportedField, objId, infraValue);
                                    break;
                                case 3:     // Text 
                                case 4:     // LongText 
                                    infraValue.StringValue = (string)supportedField.GetValue(objId);
                                    break;
                                case 5:     // DateTime 
                                    infraValue.DateTimeValue = (DateTime)supportedField.GetValue(objId);
                                    break;
                                case 6:     // Boolean  
                                    infraValue.BooleanValue = (bool)supportedField.GetValue(objId);
                                    break;
                                case 7:     // LongBinary   
                                    infraGeometryList.AddRange(ManageLongBinary(supportedField, objId, infraValue));
                                    break;
                                case 8:     // Referenced   
                                    ManageReferenced(supportedField, objId, infraValue);
                                    break;
                                case 9:     // Collection   
                                    ManageCollection(supportedField, objId, infraValue);
                                    break;
                                case 10:    // Enumerated   
                                    ManageEnumerated(supportedField, objId, infraValue);
                                    break;
                                default:
                                    break;
                            }
                            infraValueList.Add(infraValue);
                        }
                        //break;
                    }
                }
                ImportRepo.InsertToInfraObj(infraObjList);
                ImportRepo.InsertToInfraValue(infraValueList);
                ImportRepo.InsertToInfraGeometry(infraGeometryList);
            }
        }

        private static void ManageEnumerated(IField supportedField, int objId, InfraValue infraValue)
        {
            try
            {
                infraValue.IntValue = (int?)supportedField.GetValue(objId);
                return;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return;
            }
        }

        //private static List<string> _collectionList = new List<string>();
        private static void ManageCollection(IField supportedField, int objId, InfraValue infraValue)
        {
            try
            {
                //_collectionList.Add($"{objId}-{supportedField.Name}");
                var val = supportedField.GetValue(objId);
                return;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return;
            }
        }
        private static void ManageReferenced(IField supportedField, int objId, InfraValue infraValue)
        {
            try
            {
                infraValue.IntValue = (int?)supportedField.GetValue(objId);
                return;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return;
            }
        }

        private static List<InfraGeometry> ManageLongBinary(IField supportedField, int objId, InfraValue infraValue)
        {
            if (supportedField.Name != "HMIGeometry")
            {
                return null;
            }
            GeometryPoint[] geomArr;

            var geometryField = supportedField;
            var geometry = geometryField.GetValue(objId);
            if (geometry is GeometryPoint)
            {
                geomArr = new GeometryPoint[] { (GeometryPoint)geometry };
            }
            else if (geometry is GeometryPoint[])
            {
                geomArr = (GeometryPoint[])geometry;
            }
            else
            {
                throw new NotSupportedException("Unknown geometry type: " + geometry.GetType().ToString());
            }

            return geomArr.Select(x => new InfraGeometry()
            {
                ValueId = infraValue.ValueId,
                Xp = x.X,
                Yp = x.Y,
            }).ToList();

        }

        private static void ManageReal(IField supportedField, int objId, InfraValue infraValue)
        {
            double v = (double)supportedField.GetValue(objId);
            if (double.IsNaN(v))
            {
                infraValue.StringValue = "NaN";
            }
            else
            {
                infraValue.FloatValue = v;
            }
        }

    }
}
