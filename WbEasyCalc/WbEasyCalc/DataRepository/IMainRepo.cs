using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using DataRepository.WaterConsumption;
using DataRepository.WbEasyCalcData;

namespace DataRepository
{
    public interface IMainRepo
    {
        WbEasyCalcData.IListRepository WbEasyCalcDataListRepository { get; }
        WaterConsumption.IListRepository WaterConsumptionListRepository { get; }
        WaterConsumption.IListRepository WaterConsumptionListRepositoryTemp { get; set; }

        List<IdNamePair> YearList { get; }
        List<IdNamePair> MonthList { get; }
        List<IdNamePair> WaterConsumptionCategoryList { get; }
        List<IdNamePair> WaterConsumptionStatusList { get; }
        List<ZoneItem> ZoneList { get; }

        DataModel.WbEasyCalcData GetAutomaticData(int yearNo, int monthNo, int zoneId);
    }
}
