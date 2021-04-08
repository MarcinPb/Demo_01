using System.Collections.Generic;
using System.Linq;
using GeometryReader.Constants;
using GeometryReader.Extensions;
using Haestad.Support.Support;

namespace GeometryReader.ObjectReaders
{
    public class PipeReader : GenericObjectReader
    {
        public PipeReader(DomainDataSetProxy provider) : base(provider, ObjectTypes.Pipe)
        {
        }

        protected override Dictionary<string, object> GetFields(IList<string> fields, Dictionary<string, IField> supportedFields, int objectID)
        {
            var baseFields = base.GetFields(fields, supportedFields, objectID);

            var nodeFields = this.GetNodeFields(supportedFields, objectID);
            nodeFields.ToList().ForEach(x => baseFields[x.Key] = x.Value);

            var pipeFields = new List<string>
            {
                FieldNames.IsUserDefinedLength,
                FieldNames.StatusInitial,
                FieldNames.PipeMaterial,
                FieldNames.InstallationYear,
            };
            var pipeFieldValues = base.GetFields(pipeFields, supportedFields, objectID);
            pipeFieldValues.ToList().ForEach(x => baseFields[x.Key] = x.Value);

            var lengthFields = this.GetLengthFields(supportedFields, objectID);
            lengthFields.ToList().ForEach(x => baseFields[x.Key] = x.Value);

            return baseFields;
        }

        private Dictionary<string, object> GetLengthFields(Dictionary<string, IField> supportedFields, int objectID)
        {
            var lengthFields = new List<string>
            {
                FieldNames.UserDefinedLength,
                FieldNames.LengthScaled,
                FieldNames.PipeDiameter,
            };

            var lengthFieldValues = base.GetFields(lengthFields, supportedFields, objectID)
                .ToDictionary(x => x.Key, x => (object)((double)x.Value).FeetToMeters());
            return lengthFieldValues;
        }

        private Dictionary<string, object> GetNodeFields(Dictionary<string, IField> supportedFields, int objectID)
        {
            var fields = new Dictionary<string, object>();
            var nodeManager = this.DomainDataSet.DomainElementManager((int)ObjectTypes.Junction);

            var startNodeID = (int)supportedFields[FieldNames.StartNodeID].GetValue(objectID);
            var startNode = nodeManager.Element(startNodeID);
            fields[FieldNames.StartNodeID] = startNode.Id;
            fields[FieldNames.StartNodeLabel] = startNode.Label;

            var stopNodeID = (int)supportedFields[FieldNames.StopNodeID].GetValue(objectID);
            var stopNode = nodeManager.Element(stopNodeID);
            fields[FieldNames.StopNodeID] = stopNode.Id;
            fields[FieldNames.StopNodeLabel] = stopNode.Label;

            return fields;
        }
    }
}
