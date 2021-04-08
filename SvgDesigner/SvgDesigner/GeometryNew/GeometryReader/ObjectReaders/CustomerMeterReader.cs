using System.Collections.Generic;
using System.Linq;
using GeometryReader.Constants;
using Haestad.Domain.ModelingObjects;
using Haestad.Domain.ModelingObjects.Water;
using Haestad.Support.Support;

namespace GeometryReader.ObjectReaders
{
    public class CustomerMeterReader : GenericObjectReader
    {
        public CustomerMeterReader(DomainDataSetProxy provider) : base(provider, ObjectTypes.CustomerNode)
        {
        }

        protected override Dictionary<string, object> GetFields(IList<string> fields, Dictionary<string, IField> supportedFields, int objectID)
        {
            var baseFields = base.GetFields(fields, supportedFields, objectID);

            var associatedElement = this.GetCustomerMeterFields(supportedFields, objectID);
            associatedElement.ToList().ForEach(x => baseFields[x.Key] = x.Value);

            return baseFields;
        }

        private Dictionary<string, object> GetCustomerMeterFields(Dictionary<string, IField> supportedFields, int objectID)
        {
            var fields = new Dictionary<string, object>();

            var nodeManager = this.DomainDataSet.DomainElementManager((int)ObjectTypes.Junction);
            var associatedNodeID = (int)supportedFields[FieldNames.DemandAssociatedElement].GetValue(objectID);
            fields[FieldNames.DemandAssociatedElement] = nodeManager.Element(associatedNodeID).Label;

            var baseFlow = supportedFields[FieldNames.DemandBaseFlow].GetValue(objectID);
            fields[FieldNames.DemandBaseFlow] = baseFlow;

            var pattern = supportedFields[FieldNames.DemandPattern].GetValue(objectID);
            if (pattern != null)
            {
                var patternID = (int)pattern;
                var patternManager = ((IdahoDomainDataSet)this.DomainDataSet).IdahoPatternElementManager;
                fields[FieldNames.DemandPattern] = patternManager.Element(patternID).Label;
            }

            return fields;
        }

        protected override string GetZone(Dictionary<string, IField> supportedFields, Dictionary<int, ModelingElementBase> zones, int id)
        {
            var associatedNodeID = (int)supportedFields[FieldNames.DemandAssociatedElement].GetValue(id);
            return this.GetAssociatedZone(associatedNodeID);
        }

        private string GetAssociatedZone(int associatedNodeID)
        {
            var idahoDomainDataSet = (IdahoDomainDataSet)this.DomainDataSet;
            var node = idahoDomainDataSet.IdahoJunctionElementManager.Element(associatedNodeID);
            if (node == null)
            {
                return "";
            }

            var supportedFields = idahoDomainDataSet.IdahoJunctionElementManager.SupportedFields().Cast<IField>().ToDictionary(x => x.Name, x => x);
            if (!supportedFields.TryGetValue(FieldNames.Zone, out IField zoneField))
            {
                return "";
            }

            var zoneID = (int)zoneField.GetValue(node.Id);
            var nodeZone = idahoDomainDataSet.ZoneElementManager.Element(zoneID);
            if (nodeZone == null)
            {
                return "";
            }

            return nodeZone.Label;
        }
    }
}
