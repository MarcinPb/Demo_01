using System;
using System.Collections.Generic;
using System.Linq;
using Database.DataModel.Infra;
using Haestad.Domain;
using Haestad.Domain.ModelingObjects;
using Haestad.Domain.ModelingObjects.Water;
using Haestad.Support.Support;
using NLog;

namespace GeometryReader
{
    public class Importer
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public event EventHandler<ProgressEventArgs> OuterProgressChanged;
        public event EventHandler<ProgressEventArgs> InnerProgressChanged;


        public ImportedBaseOutputLists ImportBase(string fileName)
        {
            _logger.Info("ImportBase 1");

            List<InfraObjType> objectTypeDict;
            List<ImportedField> supportedFieldDict;

            using (DomainDataSetProxy domainDataSetProxy = new DomainDataSetProxy(fileName))
            {
                IDomainDataSet domainDataSet = domainDataSetProxy.OpenDomainDataSet();

                // ObjectType list
                //IDictionary<int, string> objectTypeDict = domainDataSet.DomainDataSetType().DomainElementTypes().Cast<IDomainElementType>().ToDictionary(x => x.Id, x => x.Label);
                objectTypeDict = domainDataSet.DomainDataSetType().DomainElementTypes().Cast<IDomainElementType>().Select(x => new InfraObjType() { ObjTypeId = x.Id, Name = x.Label }).ToList();
                //ImportRepo.InsertToInfraObjType(objectTypeDict);

                // SuportedField list
                supportedFieldDict = new List<ImportedField>();
                //foreach (int objectTypeId in objectTypeDict.Keys)
                foreach (var objectType in objectTypeDict)
                {
                    var manager = domainDataSet.DomainElementManager(objectType.ObjTypeId);
                    var manager1 = domainDataSet.FieldManager;
                    // Get list suported fields for a particular objectTypeId. 
                    IEnumerable<IField> supportedFields = manager.SupportedFields().Cast<IField>();
                    supportedFieldDict.AddRange(supportedFields.Select(x => new ImportedField()
                    {
                        Id = x.Id,
                        ObjTypeId = objectType.ObjTypeId,
                        Name = x.Name,
                        Label = x.Label,
                        Notes = x.Notes,
                        Category = x.Category,
                        DataTypeId = (int)x.FieldDataType,
                        FieldTypeId = 1,    // not result field
                    }));
                }
                //ImportRepo.InsertToInfraField(supportedFieldDict);
            }

            return new ImportedBaseOutputLists() { InfraObjTypeList = objectTypeDict, ImportedFieldList = supportedFieldDict };
        }

        public InfraChangeableDataLists ImportData(string fileName, InfraConstantDataLists importedDataInputLists)
        {
            using (DomainDataSetProxy domainDataSetProxy = new DomainDataSetProxy(fileName))
            {
                List<InfraObjType> infraObjTypeList = importedDataInputLists.InfraObjTypeList;
                List<InfraObjTypeField> infraObjTypeFieldList = importedDataInputLists.InfraObjTypeFieldList;
                List<InfraField> infraFieldList = importedDataInputLists.InfraFieldList;

                IDomainDataSet domainDataSet = domainDataSetProxy.OpenDomainDataSet();

                // Zone and DemandPattern lists
                IdahoDomainDataSet idahoDomainDataSet = (IdahoDomainDataSet)domainDataSet;
                List<InfraZone> zoneDict = idahoDomainDataSet.ZoneElementManager.Elements().Cast<ModelingElementBase>().Select(x => new InfraZone { ZoneId = x.Id, Name = x.Label }).ToList();
                List<InfraDemandPattern> demandPatternDict = idahoDomainDataSet.IdahoPatternElementManager.Elements().Cast<ModelingElementBase>().Select(x => new InfraDemandPattern { DemandPatternId = x.Id, Name = x.Label}).ToList();
                demandPatternDict.Add(new InfraDemandPattern { DemandPatternId = -1, Name="Fixed" });
                List<InfraDemandPatternCurve> demandPatternCurveList = GetDemandPatternCurveList(idahoDomainDataSet.IdahoPatternElementManager, demandPatternDict);

                // InfraObj list
                List<InfraObj> infraObjList = new List<InfraObj>();
                List<InfraValue> infraValueList = new List<InfraValue>();
                List<InfraGeometry> infraGeometryList = new List<InfraGeometry>();
                List<InfraDemandBase> infraInfraDemandBaseList = new List<InfraDemandBase>();

                int counter = 0;
                foreach (var objType in infraObjTypeList)
                {
                    OnOuterProgressChanged((double)counter++ / infraObjTypeList.Count, objType.Name);

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
                            var supportedField = supportedFields.FirstOrDefault(x => x.Id == field.FieldId);
                            //if (supportedField==null)
                            //{
                            //    continue;
                            //}

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
                                    infraInfraDemandBaseList.AddRange(GetDemandCollection(supportedField, objId, infraValue));
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
                OnInnerProgressChanged(1);
                OnOuterProgressChanged(1, $"Successfully imported {infraObjList.Count} objects, {infraValueList.Count} fields, {infraGeometryList.Count} geometries, {zoneDict.Count} zones and {demandPatternDict.Count} demand patterns.");

                InfraChangeableDataLists importedDataOutputLists = new InfraChangeableDataLists
                {
                    ZoneDict = zoneDict,
                    DemandPatternDict = demandPatternDict,
                    DemandPatternCurveList = demandPatternCurveList,
                    InfraObjList = infraObjList,
                    InfraValueList = infraValueList,
                    InfraGeometryList = infraGeometryList,
                    DemandBaseList = infraInfraDemandBaseList,
                };

                return importedDataOutputLists;
            }
        }
        private List<InfraDemandPatternCurve> GetDemandPatternCurveList(IdahoPatternElementManager patternManager, List<InfraDemandPattern> demandPatternDict)
        {
            //int supportElementId = 19098;
            var idahoPatternPatternCurveList = new List<InfraDemandPatternCurve>();

            //ISupportElementManager patternManager =
            //    idahoPatternElementManager.SupportElementManager((int)SupportElementType.IdahoPatternElementManager);

            IEditField transientValveCurveField =
                patternManager.SupportElementField(StandardFieldName.PatternCurve) as IEditField;
            int id = 1;
            foreach (var idahoPattern in demandPatternDict)
            {
                ICollectionFieldListManager cflm = (ICollectionFieldListManager)transientValveCurveField.GetValue(idahoPattern.DemandPatternId);

                IUnitizedField timeFromStart = cflm.Field(StandardFieldName.PatternCurve_TimeFromStart) as IUnitizedField;
                IUnitizedField multiplier = cflm.Field(StandardFieldName.PatternCurve_Multiplier) as IUnitizedField;

                for (int i = 0; i < cflm.Count; ++i)
                {
                    idahoPatternPatternCurveList.Add(new InfraDemandPatternCurve()
                    {
                        DemandPatternCurveId = id++,
                        DemandPatternId = idahoPattern.DemandPatternId,
                        TimeFromStart = timeFromStart.GetDoubleValue(i),
                        Multiplier = multiplier.GetDoubleValue(i),
                    }); ;
                }
            }

            return idahoPatternPatternCurveList;
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
        private List<InfraDemandBase> GetDemandCollection(IField supportedField, int objId, InfraValue infraValue)
        {
            try
            {
                if (supportedField.Name != "DemandCollection") { return new List<InfraDemandBase>(); }

                //_collectionList.Add($"{objId}-{supportedField.Name}");
                List<InfraDemandBase> list = new List<InfraDemandBase>();

                var val = supportedField.GetValue(objId);
                var domainElementCollectionFieldListManager = (DomainElementCollectionFieldListManager)val;
                var dataTable = domainElementCollectionFieldListManager.DataTable;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    list.Add(new InfraDemandBase
                    {
                        ValueId = infraValue.ValueId,
                        DemandBase = (double)dataTable.Rows[i][2],
                        DemandPatternId = (int)(string.IsNullOrEmpty(dataTable.Rows[i][3].ToString()) ? -1 : dataTable.Rows[i][3]),
                    });
                }

                return list;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return null;
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
        protected virtual void OnOuterProgressChanged(double ratio, string message = "")
        {
            this.OuterProgressChanged?.Invoke(this, new ProgressEventArgs(ratio, message));
        }

        protected virtual void OnInnerProgressChanged(double ratio, string message = "")
        {
            this.InnerProgressChanged?.Invoke(this, new ProgressEventArgs(ratio, message));
        }

    }
}
