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

            CalculateInfraSpecialFieldId();

            InfraChangeableData.InfraValueList.ForEach(x => RecalculateRealValue(x));
            RecalculateGeometryValue();
            RecalculateDemandBaseValue();
            IsRecalculated = true;
        }
        private void CalculateInfraSpecialFieldId()
        {
            InfraSpecialFieldId = new InfraSpecialFieldId
            {
                Demand_AssociatedElement = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "Demand_AssociatedElement").FieldId,
                Demand_BaseFlow = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "Demand_BaseFlow").FieldId,
                Demand_DemandPattern = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "Demand_DemandPattern").FieldId,
                DemandCollection = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "DemandCollection").FieldId,
                HMIActiveTopologyIsActive = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "HMIActiveTopologyIsActive").FieldId,
                Label = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "Label").FieldId,
                Physical_Zone = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "Physical_Zone").FieldId,
                Scada_TargetElement = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "Scada_TargetElement").FieldId,
                Physical_NodeElevation = InfraConstantData.InfraFieldList.FirstOrDefault(f => f.Name == "Physical_NodeElevation").FieldId,
            };
        }

        private void RecalculateGeometryValue()
        {
            var value = InfraConstantData.InfraUnitCorrectionList.FirstOrDefault(x => x.UnitCorrectionId == 1).Value;
            InfraChangeableData.InfraGeometryList.ForEach(x => { x.Xp = x.Xp * value; x.Yp = x.Yp * value; });
        }
        private void RecalculateDemandBaseValue()
        {
            var value = InfraConstantData.InfraUnitCorrectionList.FirstOrDefault(x => x.UnitCorrectionId == 2).Value;
            InfraChangeableData.DemandBaseList.ForEach(x => { x.DemandBase = x.DemandBase * value; });
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

