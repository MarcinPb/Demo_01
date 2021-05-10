using System;
using System.Collections.Generic;
using System.Linq;
using WbEasyCalcModel;
using WbEasyCalcModel.WbEasyCalc;

namespace Database.DataModel
{
    public class WbEasyCalcData : ICloneable
    {

        public int WbEasyCalcDataId { get; set; }

        public string CreateLogin { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyLogin { get; set; }
        public DateTime ModifyDate { get; set; }

        public int YearNo { get; set; }
        public int MonthNo { get; set; }
        public int ZoneId { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsArchive { get; set; }
        public bool IsAccepted { get; set; }


        //private EasyCalcModel easyCalcModel;
        //public EasyCalcModel EasyCalcModel 
        //{ 
        //    get => easyCalcModel; 
        //    set => easyCalcModel = value; 
        //}
        public EasyCalcModel EasyCalcModel { get; set; } = new EasyCalcModel();

        public List<WaterConsumption> WaterConsumptionModelList { get; set; } = new List<WaterConsumption>();

        public object Clone()
        {
            return new WbEasyCalcData()
            {
                WbEasyCalcDataId = WbEasyCalcDataId,

                CreateLogin = CreateLogin,
                CreateDate = CreateDate,
                ModifyLogin = ModifyLogin,
                ModifyDate = ModifyDate,
                Description = Description,
                IsArchive = IsArchive,
                IsAccepted = IsAccepted,

                ZoneId = ZoneId,
                YearNo = YearNo,
                MonthNo = MonthNo,

                EasyCalcModel = (EasyCalcModel)EasyCalcModel.Clone(),
                WaterConsumptionModelList = WaterConsumptionModelList.Select(x => (WaterConsumption)x.Clone()).ToList(),
                //WaterConsumptionModelList = WaterConsumptionModelList.Cast<WaterConsumption>().ToList(),
            };
        }
    }
}
