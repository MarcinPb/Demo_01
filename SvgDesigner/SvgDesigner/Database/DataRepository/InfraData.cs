using Database.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataRepository
{
    public class InfraData
    {
        public InfraConstantDataLists InfraConstantData { get; set; } 
        public InfraChangeableDataLists InfraChangeableData { get; set; }

        public bool IsRecalculated { get; set; } = false;
        public void Recalculate() 
        {
            if (IsRecalculated) { return; }

            InfraChangeableData.InfraValueList.ForEach(x => RecalculateRealValue(x));
            RecalculateGeometryValue();
            IsRecalculated = true;
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

