using System;
using System.Collections.Generic;
using System.Linq;
using GeometryModel;
using GeometryReader.Constants;
using GeometryReader.Extensions;
using Haestad.Domain;
using Haestad.Domain.ModelingObjects.Water;
using Haestad.Support.Support;

namespace GeometryReader.ObjectReaders
{
    public class GenericObjectReader
    {
        private readonly DomainDataSetProxy provider;
        private IDomainDataSet domainDataSet;

        public GenericObjectReader(DomainDataSetProxy provider, Constants.ObjectTypes objectType)
        {
            this.provider = provider;
            this.ObjectType = objectType;
        }
        public GenericObjectReader(DomainDataSetProxy provider)
        {
            this.provider = provider;
        }

        public event EventHandler<ProgressEventArgs> ProgressChanged;

        public Constants.ObjectTypes ObjectType { get; private set; }

        public virtual IDomainDataSet DomainDataSet
        {
            get
            {
                return this.domainDataSet ?? (this.domainDataSet = this.provider.OpenDomainDataSet());
            }
        }


        public Dictionary<int, string> ReadZoneList()
        {
            var idahoDomainDataSet = (IdahoDomainDataSet)this.DomainDataSet;
            var zones = idahoDomainDataSet.ZoneElementManager.Elements().Cast<Haestad.Domain.ModelingObjects.ModelingElementBase>().ToDictionary(x => x.Id, x => x.Label);

            return zones;
        }

        public Dictionary<int, string> ReadOjectTypeList()
        {
            return this.DomainDataSet.DomainDataSetType().DomainElementTypes().Cast<IDomainElementType>().ToDictionary(x => x.Id, x => x.Label); 
        }

        public virtual IList<DomainObjectData> ReadObjects(IList<string> fields)
        {
            //var list1 = this.DomainDataSet.DomainElementTypeIDs();
            //var list2 = this.DomainDataSet.DomainDataSetType().DomainElementTypes().Cast<IDomainElementType>().ToDictionary(x => x.Id, x => x.Label);
            //var list3 = this.DomainDataSet.DomainDataSetType().SupportElementTypes().SyncRoot;


            var manager = this.DomainDataSet.DomainElementManager((int)this.ObjectType);
            var supportedFields = manager.SupportedFields().Cast<IField>().ToDictionary(x => x.Name, x => x);
            var labelField = supportedFields[FieldNames.Label];
            var isActiveField = supportedFields[FieldNames.IsActive];
            var ids = manager.ElementIDs();
            var items = new List<DomainObjectData>();

            var idahoDomainDataSet = (IdahoDomainDataSet)this.DomainDataSet;
            var zones = idahoDomainDataSet.ZoneElementManager.Elements().Cast<Haestad.Domain.ModelingObjects.ModelingElementBase>().ToDictionary(x => x.Id, x => x);
            int counter = 0;
            foreach (var id in ids)
            {
                var geometry = this.GetGeometry(supportedFields, id);
                string zoneLabel = this.GetZone(supportedFields, zones, id);
                var item = new DomainObjectData
                {
                    ID = id,
                    ObjectType = (GeometryModel.ObjectTypes)(int)this.ObjectType,
                    Label = (string)labelField.GetValue(id),
                    Zone = zoneLabel,
                    IsActive = (bool)isActiveField.GetValue(id),
                    Geometry = geometry.Select(x => new Point2D(x.X.FeetToMeters(), x.Y.FeetToMeters())).ToList(),
                    Fields = this.GetFields(fields, supportedFields, id)
                };

                items.Add(item);
                this.OnProgressChanged((double)++counter / ids.Count);
            }

            return items;
        }
        public virtual IList<DomainObjectData> ReadObjects(int objectType)
        {
            // Get list of zones
            var idahoDomainDataSet = (IdahoDomainDataSet)this.DomainDataSet;
            var zones = idahoDomainDataSet.ZoneElementManager.Elements().Cast<Haestad.Domain.ModelingObjects.ModelingElementBase>().ToDictionary(x => x.Id, x => x);

            var manager = this.DomainDataSet.DomainElementManager(objectType);

            // Get list suported fields for a particular element type. 
            var supportedFields = manager.SupportedFields().Cast<IField>().ToDictionary(x => x.Name, x => x);

            // Get list of elements of a particular element type. 
            var ids = manager.ElementIDs();

            var labelField = supportedFields[FieldNames.Label];
            //var isActiveField = supportedFields[FieldNames.IsActive];
            var items = new List<DomainObjectData>();

            int counter = 0;
            foreach (var id in ids)
            {
                var geometry = this.GetGeometry(supportedFields, id);
                string zoneLabel = this.GetZone(supportedFields, zones, id);

                var item = new DomainObjectData
                {
                    ID = id,
                    ObjectType = (GeometryModel.ObjectTypes)(int)this.ObjectType,
                    Label = (string)labelField.GetValue(id),
                    Zone = zoneLabel,
                    //IsActive = (bool)isActiveField.GetValue(id),
                    Geometry = geometry.Select(x => new Point2D(x.X.FeetToMeters(), x.Y.FeetToMeters())).ToList(),
                    Fields = this.GetFields(supportedFields, id)
                };

                items.Add(item);
                this.OnProgressChanged((double)++counter / ids.Count);
            }

            return items;
        }

        protected virtual void OnProgressChanged(double ratio, string message = "")
        {
            this.ProgressChanged?.Invoke(this, new ProgressEventArgs(ratio, message));
        }

        protected virtual string GetZone(Dictionary<string, IField> supportedFields, Dictionary<int, Haestad.Domain.ModelingObjects.ModelingElementBase> zones, int id)
        {
            if (!supportedFields.TryGetValue(FieldNames.Zone, out IField zoneField))
            {
                return "";
            }

            var zoneID = zoneField.GetValue(id);
            string zoneLabel = (zoneID is int) ? zones[(int)zoneField.GetValue(id)].Label : "";
            return zoneLabel;
        }

        protected virtual GeometryPoint[] GetGeometry(Dictionary<string, IField> supportedFields, int objectID)
        {
            var geometryField = supportedFields[FieldNames.Geometry];
            var geometry = geometryField.GetValue(objectID);
            if (geometry is GeometryPoint)
            {
                return new GeometryPoint[] { (GeometryPoint)geometry };
            }
            else if (geometry is GeometryPoint[])
            {
                return (GeometryPoint[])geometry;
            }
            else
            {
                throw new NotSupportedException("Unknown geometry type: " + geometry.GetType().ToString());
            }
        }

        protected virtual Dictionary<string, object> GetFields(IList<string> fields, Dictionary<string, IField> supportedFields, int objectID)
        {
            return fields.ToDictionary(x => x, x => supportedFields[x].GetValue(objectID));
        }

        protected virtual Dictionary<string, object> GetFields(Dictionary<string, IField> supportedFields, int objectID)
        {
            Dictionary<string, object> fieldDict = new Dictionary<string, object>();
            foreach (var f in supportedFields)
            { 
                fieldDict.Add(f.Key, supportedFields[f.Key].GetValue(objectID));

                /*
                switch(f.Value.FieldDataType)
                {
                    case FieldDataType.Integer:
                        fieldDict.Add(f.Key, supportedFields[f.Key].GetValue(objectID));
                        break;
                    case FieldDataType.Integer:

                    default:
                }
                */
            }

            return fieldDict;

            //return fields.ToDictionary(x => x, x => supportedFields[x].GetValue(objectID));
        }
    }
}
