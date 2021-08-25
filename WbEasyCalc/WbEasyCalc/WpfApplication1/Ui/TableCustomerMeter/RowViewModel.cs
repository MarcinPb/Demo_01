using Database.DataModel.Infra;

namespace WpfApplication1.Ui.TableCustomerMeter
{
    public class RowViewModel
    {
        public InfraObj ObjModel { get; }
        public string Label { get; }
        public bool IsActive { get; set; }
        public InfraZone Zone { get; }
        public double? DemandBase { get; }
        public InfraDemandPattern DemandPatternModel { get; }

        public RowViewModel(InfraObj objModel, string label, bool isActive, InfraZone zone, double? demandBase, InfraDemandPattern demandPatternModel)
        {
            ObjModel = objModel;
            Label = label;
            IsActive = isActive;
            Zone = zone;
            DemandBase = demandBase;
            DemandPatternModel = demandPatternModel;
        }
    }
}
