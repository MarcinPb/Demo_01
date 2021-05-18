using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel
{
    public class BaseSheetViewModel : ViewModelBase
    {
        private protected void Calculate()
        {
            Messenger.Default.Send<BaseSheetViewModel>(this);
        }

    }
}
