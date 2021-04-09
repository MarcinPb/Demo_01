using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel
{
    public class ImportedField
    {
        public int Id { get; set; }
        public int ObjTypeId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Notes { get; set; }
        public string Category { get; set; }
        public int DataTypeId { get; set; }
        public int FieldTypeId { get; set; }
    }
}
