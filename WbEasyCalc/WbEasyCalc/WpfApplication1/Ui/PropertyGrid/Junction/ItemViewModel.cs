using Database.DataModel.Infra;
using Database.DataRepository.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Ui.PropertyGrid.Junction.Model;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.PropertyGrid.Junction
{
    public class ItemViewModel : Ui.PropertyGrid.ItemXyViewModel
    {
        private List<InfraDemandBaseExtended> _demmandList;
        [Category("Demand")]
        [DisplayName("Demand Collection")]
        public List<InfraDemandBaseExtended> DemmandList
        {
            get { return _demmandList; }
            set { _demmandList = value; RaisePropertyChanged("Path"); }
        }


        public ItemViewModel(int id) : base(id)
        {
            var infraChangeableData = InfraRepo.GetInfraData().InfraChangeableData;
            var valueId = infraChangeableData.InfraValueList.FirstOrDefault(f => f.ObjId == id && f.FieldId == InfraRepo.GetInfraData().InfraSpecialFieldId.DemandCollection)?.ValueId;
            if(valueId.HasValue)
            {
                var demandBaseListFiltered = infraChangeableData.DemandBaseList.Where(f => f.ValueId == valueId);
                //DemmandList = infraChangeableData.DemandPatternDict.Where(f => demandBaseListFiltered.Any(x => f.DemandPatternId==x.DemandPatternId)).ToList();
                DemmandList = demandBaseListFiltered.Join(
                    infraChangeableData.DemandPatternDict,
                    l => l.DemandPatternId,
                    r => r.DemandPatternId,
                    (l, r) => new InfraDemandBaseExtended() {
                        DemandPatternId = l.DemandPatternId,
                        DemandBase = l.DemandBase,
                        Name= r.Name
                    })
                    .ToList();
            }
        }
    }
}
