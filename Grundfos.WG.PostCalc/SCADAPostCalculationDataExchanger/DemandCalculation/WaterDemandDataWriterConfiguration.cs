using System.Collections.Generic;

namespace Grundfos.WG.PostCalc.DemandCalculation
{
    public class WaterDemandDataWriterConfiguration
    {
        public int[] ExcludedObjectIDs { get; set; }
        public int[] ExcludedDemandPatterns { get; set; }
        //public bool IsCalculationOnDb { get; set; }
    }
}
