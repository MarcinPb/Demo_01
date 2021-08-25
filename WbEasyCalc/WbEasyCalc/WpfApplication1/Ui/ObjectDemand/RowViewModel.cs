using Database.DataModel.Infra;

namespace WpfApplication1.Ui.ObjectDemand
{
    public class RowViewModel
    {
        public InfraObj ObjModel { get; }
        public string Label { get; }
        public InfraZone Zone { get; }
        public InfraDemandBase DemandBaseModel { get; }
        public InfraDemandPattern DemandPatternModel { get; }

        public RowViewModel(InfraObj objModel, string label, InfraZone zone, InfraDemandBase demandBaseModel, InfraDemandPattern demandPatternModel)
        {
            ObjModel = objModel;
            Label = label;
            Zone = zone;
            DemandBaseModel = demandBaseModel;
            DemandPatternModel = demandPatternModel;
        }
    }
}
