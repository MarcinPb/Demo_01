using System;
using WbEasyCalcModel;

namespace Database.DataModel
{
    public class WaterConsumption : ICloneable
    {
        public int WaterConsumptionId { get; set; }
        public int WbEasyCalcDataId { get; set; }

        public string Description { get; set; } = string.Empty;

        public int WaterConsumptionCategoryId { get; set; }
        public int WaterConsumptionStatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Latitude { get; set; }
        public double Lontitude { get; set; }
        public int RelatedId { get; set; }
        public double Value { get; set; }

        public object Clone()
        {
            return new WaterConsumption()
            {
                WaterConsumptionId = WaterConsumptionId,
                WbEasyCalcDataId = WbEasyCalcDataId,

                Description = Description,

                WaterConsumptionCategoryId = WaterConsumptionCategoryId,
                WaterConsumptionStatusId = WaterConsumptionStatusId,
                StartDate = StartDate,
                EndDate = EndDate,
                Latitude = Latitude,
                Lontitude = Lontitude,
                RelatedId = RelatedId,
                Value = Value,
            };
        }
    }
}
