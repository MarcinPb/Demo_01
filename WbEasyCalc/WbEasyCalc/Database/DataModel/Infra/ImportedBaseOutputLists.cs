using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel.Infra
{
    public class ImportedBaseOutputLists
    {
        public List<InfraObjType> InfraObjTypeList { get; set; } = new List<InfraObjType>();
        public List<ImportedField> ImportedFieldList { get; set; } = new List<ImportedField>();
    }
}
