using Database.DataModel.Infra;
using Database.DataRepository.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.PropertyGrid.Junction
{
    public class ItemViewModel : Ui.PropertyGrid.ItemXyViewModel
    {
        private List<InfraDemandPattern> _demmandList;
        [Category("Demand")]
        [DisplayName("Demand Collection")]
        public List<InfraDemandPattern> DemmandList
        {
            get { return _demmandList; }
            set { _demmandList = value; RaisePropertyChanged("Path"); }
        }


        public ItemViewModel(int id) : base(id)
        {
            var infraChangeableData = InfraRepo.GetInfraData().InfraChangeableData;
            //var valueId = infraChangeableData.InfraValueList.FirstOrDefault(f => f.ObjId == id && f.FieldId == InfraRepo.GetInfraData().InfraSpecialFieldId.DemandCollection)?.IntValue;
            var valueId = infraChangeableData.InfraValueList.FirstOrDefault(f => f.ObjId == id && f.FieldId == 757)?.ValueId;
            if(valueId.HasValue)
            {
                var demmandPatternIdList1 = infraChangeableData.DemandBaseList;           
                var demmandPatternIdList = infraChangeableData.DemandBaseList.Where(f => f.ValueId == valueId);           
                DemmandList = infraChangeableData.DemandPatternDict.Where(f => demmandPatternIdList.Any(x => f.DemandPatternId==x.DemandPatternId)).ToList();
            }
        }
    }
}
