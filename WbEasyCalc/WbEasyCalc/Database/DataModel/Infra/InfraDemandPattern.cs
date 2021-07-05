using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel.Infra
{
    public class InfraDemandPattern
    {
        public int DemandPatternId { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{DemandPatternId} - {Name}";
        }
    }
}
