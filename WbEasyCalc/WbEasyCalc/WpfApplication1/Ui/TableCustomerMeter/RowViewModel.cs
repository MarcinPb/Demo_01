using Database.DataModel.Infra;
using System;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.TableCustomerMeter
{
    public class RowViewModel : ViewModelBase, IDisposable
    {
        public InfraObj ObjModel { get; }
        public string Label { get; }
        public string AssociatedElementName { get; }
        public double? DemandBase { get; }
        public InfraDemandPattern DemandPatternModel { get; }
        public bool IsActive { get; }
        public InfraZone Zone { get; }
        

        private double? _demandBaseDmSet;
        public double? DemandBaseDmSet
        {
            get => _demandBaseDmSet;
            set { _demandBaseDmSet = value; RaisePropertyChanged(nameof(DemandBaseDmSet)); }
        }

        //public InfraDemandPattern DemandPatternModelDmSet { get; }

        private int? _demandPatternIdDmSet;
        public int? DemandPatternIdDmSet
        {
            get => _demandPatternIdDmSet;
            set { _demandPatternIdDmSet = value; RaisePropertyChanged(nameof(DemandPatternIdDmSet)); }
        }

        private string _demandPatternNameDmSet;
        public string DemandPatternNameDmSet
        {
            get => _demandPatternNameDmSet;
            set { _demandPatternNameDmSet = value; RaisePropertyChanged(nameof(DemandPatternNameDmSet)); }
        }

        private bool _isExcluded { get; set; }
        public bool IsExcluded
        {
            get => _isExcluded;
            set { _isExcluded = value; RaisePropertyChanged(nameof(IsExcluded)); }
        }


        public RowViewModel(InfraObj objModel, string label, string associatedElementName, double? demandBase, InfraDemandPattern demandPatternModel, bool isActive, InfraZone zone, double? demandBaseDmSet, InfraDemandPattern demandPatternModelDmSet, bool isExcluded)
        {
            ObjModel = objModel;
            Label = label;
            AssociatedElementName = associatedElementName;
            DemandBase = demandBase;
            DemandPatternModel = demandPatternModel;
            IsActive = isActive;
            Zone = zone;

            DemandBaseDmSet = demandBaseDmSet;
            //DemandPatternModelDmSet = demandPatternModelDmSet;
            DemandPatternIdDmSet = demandPatternModelDmSet?.DemandPatternId;
            DemandPatternNameDmSet = demandPatternModelDmSet?.Name;
            IsExcluded = isExcluded;
       }
        public void Dispose()
        {
            //DemandPatternCurveListViewModel.Dispose();
        }
    }
}
