using System.Collections.Generic;

namespace Grundfos.WG.OPC.Publisher.Configuration
{
    public class OpcMapping
    {
        public string FieldName { get; set; }   // PipeStatus, TankPercentFull, ZoneAveragePressure
        public ICollection<OpcMappingEntry> Mappings { get; set; }
    }
}
