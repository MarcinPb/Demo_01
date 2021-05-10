using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataModel
{
    public class ConfigMatrix
    {
        public int ConfigId { get; set; }
        public int TypeId { get; set; }
        public string CategoryName { get; set; }
        public double? Ili { get; set; }
        public double? Lcd10 { get; set; }
        public double? Lcd20 { get; set; }
        public double? Lcd30 { get; set; }
        public double? Lcd40 { get; set; }
        public double? Lcd50 { get; set; }
    }
}
