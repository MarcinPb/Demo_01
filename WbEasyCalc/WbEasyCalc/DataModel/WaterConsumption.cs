using System;
using WbEasyCalcModel;

namespace DataModel
{
    public class WaterConsumption : ICloneable
    {
        public int WaterConsumptionId { get; set; }
        public int WbEasyCalcDataId { get; set; }

        public string CreateLogin { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyLogin { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Description { get; set; } = string.Empty;
        public bool IsArchive { get; set; }
        public bool IsAccepted { get; set; }

        public int WaterConsumptionCategoryId { get; set; }
        public int WaterConsumptionStatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Latitude { get; set; }
        public double Lontitude { get; set; }
        public int ZoneId { get; set; }
        public double Value { get; set; }

        public object Clone()
        {
            return new WaterConsumption()
            {
                WaterConsumptionId = WaterConsumptionId,
                WbEasyCalcDataId = WbEasyCalcDataId,
                CreateLogin = CreateLogin,
                CreateDate = CreateDate,
                ModifyLogin = ModifyLogin,
                ModifyDate = ModifyDate,
                Description = Description,
                IsArchive = IsArchive,
                IsAccepted = IsAccepted,

                WaterConsumptionCategoryId = WaterConsumptionCategoryId,
                WaterConsumptionStatusId = WaterConsumptionStatusId,
                StartDate = StartDate,
                EndDate = EndDate,
                Latitude = Latitude,
                Lontitude = Lontitude,
                ZoneId = ZoneId,
                Value = Value,
            };
        }
    }
}
