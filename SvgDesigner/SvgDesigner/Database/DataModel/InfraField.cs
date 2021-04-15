using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel
{
    public class InfraField
    {
        public int FieldId { get; set; }
        public int ObjTypeId { get; set; }
        public int DataTypeId { get; set; }
        public int? UnitCorrectionId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }

    }
}
