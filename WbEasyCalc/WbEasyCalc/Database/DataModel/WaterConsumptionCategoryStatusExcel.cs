using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel
{
    public class WaterConsumptionCategoryStatusExcel
    {
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public int ExcelCellId { get; set; }
        public string ExcelCellName { get; set; }
    }
}
