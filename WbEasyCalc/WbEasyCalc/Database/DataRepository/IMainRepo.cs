using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.DataModel;
using Database.DataRepository.WaterConsumption;
using Database.DataRepository.WbEasyCalcData;

namespace Database.DataRepository
{
    public interface IMainRepo
    {
        Options.ItemRepository Option { get; }

        WbEasyCalcData.IListRepository WbEasyCalcDataListRepository { get; }
        WaterConsumption.IListRepository WaterConsumptionListRepository { get; }
        WaterConsumption.IListRepository WaterConsumptionListRepositoryTemp { get; set; }

        List<IdNamePair> YearList { get; }
        List<IdNamePair> MonthList { get; }
        List<IdNamePair> WaterConsumptionCategoryList { get; }
        List<IdNamePair> WaterConsumptionStatusList { get; }
        List<WaterConsumptionCategoryStatusExcel> WaterConsumptionCategoryStatusExcelList { get; }
        List<ZoneItem> ZoneList { get; }

        DataModel.WbEasyCalcData GetWbGisModelScadaData(int yearNo, int monthNo, int zoneId);
        DataModel.WbEasyCalcData GetWbYearData(int yearNo, int zoneId);
    }
}
