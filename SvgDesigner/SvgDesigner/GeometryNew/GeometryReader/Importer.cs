using System;
using System.Collections.Generic;
using System.Linq;
using Haestad.Domain;
using Haestad.Domain.ModelingObjects;
using Haestad.Domain.ModelingObjects.Water;
using Haestad.Support.Support;
using Database.DataModel;
using Database.DataRepository;

namespace GeometryReader
{
    public class Importer
    {
        public event EventHandler<ProgressEventArgs> ProgressChanged;
        public event EventHandler<ProgressEventArgs> InnerProgressChanged;


        public void ImportBase(string fileName)
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
            }
        }

        public void ImportData(string fileName)
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

                List<InfraObjType> infraObjTypeList = ImportRepo.GetObjTypeList().ToList();
                List<InfraObjTypeField> infraObjTypeFieldList = ImportRepo.GetObjTypeFieldList().ToList();
                List<InfraField> infraFieldList = ImportRepo.GetFieldList();

                int counter = 0;
                foreach (var objType in infraObjTypeList)
                {
                    OnProgressChanged((double) counter++ / infraObjTypeList.Count, objType.Name);

                    // InfraObj
                    var manager = domainDataSet.DomainElementManager(objType.ObjTypeId);
                    var supportedFields = manager.SupportedFields().Cast<IField>();
                    var objIdList = manager.ElementIDs();

                    int innerCounter = 0;
                    int objQty = objIdList.Count;
                    foreach (var objId in objIdList)
                    {
                        var infraObj = new InfraObj
                        {
                            ObjId = objId,
                            ObjTypeId = objType.ObjTypeId,
                        };
                        infraObjList.Add(infraObj);

                        // InfraValue
                        var fieldList = infraFieldList.Where(x => infraObjTypeFieldList.Where(y => y.ObjTypeId == objType.ObjTypeId).Any(y => x.FieldId == y.FieldId));
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
                                    infraGeometryList.AddRange(GetLongBinary(supportedField, objId, infraValue));
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
                        OnInnerProgressChanged((double)++innerCounter / objQty);
                        //break;
                    }
                }
                OnProgressChanged(1, "Saving data to database.");

                ImportRepo.InsertToInfraObj(infraObjList);
                ImportRepo.InsertToInfraValue(infraValueList);
                ImportRepo.InsertToInfraGeometry(infraGeometryList);
                
                OnInnerProgressChanged(0);
                OnProgressChanged(0, $"Successfully imported {infraObjList.Count} objects, {infraValueList.Count} fields and {infraGeometryList.Count} geometries.");
            }
        }

        private void ManageEnumerated(IField supportedField, int objId, InfraValue infraValue)
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
        private void ManageCollection(IField supportedField, int objId, InfraValue infraValue)
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
        private void ManageReferenced(IField supportedField, int objId, InfraValue infraValue)
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

        private List<InfraGeometry> GetLongBinary(IField supportedField, int objId, InfraValue infraValue)
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

            return geomArr.Select((x, idx) => new InfraGeometry()
            {
                ValueId = infraValue.ValueId,
                OrderNo = idx,
                Xp = x.X,
                Yp = x.Y,
            }).ToList();

        }

        private void ManageReal(IField supportedField, int objId, InfraValue infraValue)
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
        protected virtual void OnProgressChanged(double ratio, string message = "")
        {
            this.ProgressChanged?.Invoke(this, new ProgressEventArgs(ratio, message));
        }

        protected virtual void OnInnerProgressChanged(double ratio, string message = "")
        {
            this.InnerProgressChanged?.Invoke(this, new ProgressEventArgs(ratio, message));
        }

    }
}
