using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WbEasyCalcModel.WbEasyCalc;
using WpfApplication1.Ui.WbEasyCalcData.Excel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.WbEasyCalcData.Excel.Start
{
    public class ViewModel : BaseSheetViewModel
    {
        #region Input props

        private int _startPeriodDaysM21;
        public int Start_PeriodDays_M21
        {
            get => _startPeriodDaysM21;
            set { _startPeriodDaysM21 = value; RaisePropertyChanged(nameof(Start_PeriodDays_M21)); Calculate(); }
        }

        #endregion

        #region Output props
        #endregion

        public StartModel Model => new StartModel()
        {
            Start_PeriodDays_M21 = Start_PeriodDays_M21,
        };

        public ViewModel(StartModel model)
        {
            if (model == null) return;

            // Input
            Start_PeriodDays_M21 = model.Start_PeriodDays_M21;
        }
    }
}
