using Database.DataModel.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataRepository.Infra
{
    public class InfraData
    {
        public InfraConstantDataLists InfraConstantData { get; set; } 
        public InfraChangeableDataLists InfraChangeableData { get; set; }
        public InfraSpecialFieldId InfraSpecialFieldId { get; set; }

        public bool IsRecalculated { get; set; } = false;
        public void Recalculate() 
        {
            if (IsRecalculated) { return; }

            InfraChangeableData.InfraValueList.ForEach(x => RecalculateRealValue(x));
            RecalculateGeometryValue();
            CalculateInfraSpecialFieldId();
            IsRecalculated = true;
        }
        private void CalculateInfraSpecialFieldId()
        {
            InfraSpecialFieldId = new InfraSpecialFieldId
            {
                Label = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "Label").FieldId,
                HMIActiveTopologyIsActive = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "HMIActiveTopologyIsActive").FieldId,
                Physical_Zone = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "Physical_Zone").FieldId,
                Demand_AssociatedElement = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "Demand_AssociatedElement").FieldId,
                Scada_TargetElement = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "Scada_TargetElement").FieldId,
            };
        }

        private void RecalculateGeometryValue()
        {
            var value = InfraConstantData.InfraUnitCorrectionList.FirstOrDefault(x => x.UnitCorrectionId == 1).Value;
            InfraChangeableData.InfraGeometryList.ForEach(x => { x.Xp = x.Xp * value; x.Yp = x.Yp * value; });
        }

        private void RecalculateRealValue(InfraValue infraValue)
        {
            var field = InfraConstantData.InfraFieldList.FirstOrDefault(x => x.FieldId == infraValue.FieldId);
            if (field.UnitCorrectionId == null) { return; }
            var value = InfraConstantData.InfraUnitCorrectionList.FirstOrDefault(x => x.UnitCorrectionId == field.UnitCorrectionId).Value;
            infraValue.FloatValue = infraValue.FloatValue * value;
        }
    }
}

