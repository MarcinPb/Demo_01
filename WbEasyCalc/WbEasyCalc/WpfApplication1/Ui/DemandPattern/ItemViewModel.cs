using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using GlobalRepository;
using WbEasyCalcModel;
using WbEasyCalcRepository;
using WpfApplication1.Ui.WaterBalanceList.Excel;
using WpfApplication1.Ui.WaterConsumptionMap;
//using WpfApplication1.Ui.WbEasyCalcData.ViewModel.Tabs;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.DemandPattern
{

    public class ItemViewModel : ViewModelBase, IDisposable
    {

        #region Props ViewModel: Id, Name

        private int _id;
        public int Id
        {
            get => _id;
            set { _id = value; RaisePropertyChanged(nameof(Id)); }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value; RaisePropertyChanged(nameof(Name));
            }
        }

        private bool _isExcluded;
        public bool IsExcluded
        {
            get
            {
                return _isExcluded;
            }
            set
            {
                _isExcluded = value; RaisePropertyChanged(nameof(IsExcluded));
            }
        }
        #endregion

        public DemandPatternCurve.ListViewModel DemandPatternCurveListViewModel { get; set; }


        public Database.DataModel.Infra.InfraDemandPattern Model
        {
            get
            {
                return new Database.DataModel.Infra.InfraDemandPattern()
                {
                    DemandPatternId = this.Id,
                    Name = Name,
                };
            }
        }

        public ItemViewModel(Database.DataModel.Infra.InfraDemandPattern model, bool isExcluded)
        {
            Id = model.DemandPatternId;
            Name = model.Name;
            IsExcluded = isExcluded;

            DemandPatternCurveListViewModel = new DemandPatternCurve.ListViewModel(Id);
        }
        public void Dispose()
        {
            //DemandPatternCurveListViewModel.Dispose();
        }
    }
}
