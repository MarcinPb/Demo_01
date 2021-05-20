using Database.DataRepository.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.PropertyGrid.CustomerNode
{
    public class ItemViewModel : Ui.PropertyGrid.ItemXyViewModel
    {
        [Category("Demand")]
        [DisplayName("Associated Element")]
        public string Demand_AssociatedElement { get; set; }

        [Category("Demand")]
        [DisplayName("Base Flow")]
        public double Demand_BaseFlow { get; set; }

        [Category("Demand")]
        [DisplayName("Demand Pattern")]
        public string Demand_DemandPattern { get; set; }

        public ItemViewModel(int id) : base(id)
        {
            var infraValueList = InfraRepo.GetInfraData().InfraChangeableData.InfraValueList;
            var infraDemandPatternList = InfraRepo.GetInfraData().InfraChangeableData.DemandPatternDict;

            if (_model.Fields["Demand_AssociatedElement"] != null)
            {
                int relatedId = (int)_model.Fields["Demand_AssociatedElement"];
                Demand_AssociatedElement = infraValueList.FirstOrDefault(x => x.ObjId == relatedId && x.FieldId == InfraRepo.GetInfraData().InfraSpecialFieldId.Label).StringValue;
            }

            Demand_BaseFlow = (double)_model.Fields["Demand_BaseFlow"];

            object fieldValue = _model.Fields["Demand_DemandPattern"];
            if (fieldValue != null)
            {
                int relatedId = (int)fieldValue;
                Demand_DemandPattern = infraDemandPatternList.FirstOrDefault(x => x.DemandPatternId == relatedId)?.Name;
            }
        }
    }
}
