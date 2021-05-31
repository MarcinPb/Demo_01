using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.DataRepository.Infra;
using GlobalRepository;

namespace WpfApplication1.Ui.WaterConsumptionList
{
    public class RowViewModel
    {
        public Database.DataModel.WaterConsumption Model { get; }
        public string WaterConsumptionCategoryName => GlobalConfig.DataRepository.WaterConsumptionCategoryList.FirstOrDefault(x => x.Id == Model.WaterConsumptionCategoryId)?.Name;
        public string WaterConsumptionStatusName => GlobalConfig.DataRepository.WaterConsumptionStatusList.FirstOrDefault(x => x.Id == Model.WaterConsumptionStatusId)?.Name;

        public string ObjectName { get; }

        public RowViewModel(Database.DataModel.WaterConsumption model)
        {
            Model = model;
            ObjectName = GetObjectName(model);
        }

        private string GetObjectName(Database.DataModel.WaterConsumption model)
        {
            const int LabelFieldId = 2;
            var objId = model.RelatedId;
            var objName = InfraRepo.GetInfraData().InfraChangeableData.InfraValueList.FirstOrDefault(f => f.FieldId == LabelFieldId && f.ObjId == objId)?.StringValue;

            return objName;
        }
    }
}
