using Database.DataModel.Infra;

namespace WpfApplication1.Ui.DemandPattern
{
    public class RowViewModel
    {
        public InfraDemandPattern Model { get; }

        public RowViewModel(InfraDemandPattern model)
        {
            Model = model;
        }
    }
}
