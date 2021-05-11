using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WbEasyCalcModel.WbEasyCalc
{
    public class StartModel : ICloneable
    {
        public int Start_PeriodDays_M21 { get; set; }

        public object Clone()
        {
            return new StartModel()
            {
                Start_PeriodDays_M21 = Start_PeriodDays_M21,
            };
        }
    }
}
