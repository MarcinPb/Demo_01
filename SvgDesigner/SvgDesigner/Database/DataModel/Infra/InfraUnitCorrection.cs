using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel.Infra
{
    public class InfraUnitCorrection
    {
        public int UnitCorrectionId { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }
    }
}
