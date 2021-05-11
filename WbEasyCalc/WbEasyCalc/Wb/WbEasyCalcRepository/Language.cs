using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel;

namespace WbEasyCalc
{
    public class Language
    {
        public List<LanguageItem> GetList()
        {
            return new List<LanguageItem>()
                {
                    new LanguageItem(){Id = 1, Order = 1, Desc = "The volumes used this water balance are for a period of [days]: ", Us = "The volumes used this water balance are for a period of [days]...: ", Pl = "Ilość dni: "},
                    new LanguageItem(){Id = 2, Order = 2, Desc = "Sys. Input", Us = "Sys. Input...", Pl = "Syy. Iyyyy"},
                };
        }

        public void SaveList()
        {
        }
    }
}
