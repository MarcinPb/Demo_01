using Database.DataModel.Infra;

namespace WpfApplication1.Ui.DemandPatternCurve
{
    public class RowViewModel
    {
        public InfraDemandPatternCurve Model { get; }

        public RowViewModel(InfraDemandPatternCurve model)
        {
            Model = model;
        }
    }
}
