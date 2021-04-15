using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel
{
    public class InfraConstantDataLists
    {
        public List<InfraObjType> InfraObjTypeList { get; set; }
        public List<InfraObjTypeField> InfraObjTypeFieldList { get; set; }
        public List<InfraField> InfraFieldList { get; set; }
        public List<InfraUnitCorrection> InfraUnitCorrectionList { get; set; }
    }
}
