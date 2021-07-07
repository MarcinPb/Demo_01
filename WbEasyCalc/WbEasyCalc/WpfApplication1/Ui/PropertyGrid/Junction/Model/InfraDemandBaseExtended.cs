using Database.DataModel.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Ui.PropertyGrid.Junction.Model
{
    public class InfraDemandBaseExtended
    {
        public int DemandPatternId { get; set; }
        public string Name { get; set; }
        public double DemandBase { get; set; }

        public override string ToString()
        {
            return $"{DemandBase} - {Name}"; 
        }
    }
}
