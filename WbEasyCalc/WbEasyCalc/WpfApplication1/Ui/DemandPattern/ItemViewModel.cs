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

        #endregion

        public DemandPatternCurve.ListViewModel WaterConsumptionListViewModel { get; set; }


        public Database.DataModel.Infra.InfraDemandPattern Model
        {
            get
            {
                return new Database.DataModel.Infra.InfraDemandPattern()
                {
                    DemandPatternId = this.Id,

                    Name = Name,

                    //WaterConsumptionModelList = WaterConsumptionListViewModel.List.Select(x => x.Model).ToList(),
                };
            }
        }

        public ItemViewModel(Database.DataModel.Infra.InfraDemandPattern model)
        {
            Id = model.DemandPatternId;

            Name = model.Name;

            WaterConsumptionListViewModel = new DemandPatternCurve.ListViewModel(Id);

        }
        public void Dispose()
        {
            //WaterConsumptionListViewModel.Dispose();
        }
    }
}
