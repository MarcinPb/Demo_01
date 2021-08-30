using Database.DataModel.Infra;

namespace WpfApplication1.Ui.DemandPattern
{
    public class RowViewModel
    {
        public InfraDemandPattern Model { get; }
        public bool IsExcluded { get; set; }

        public RowViewModel(InfraDemandPattern model, bool isExcluded)
        {
            Model = model;
            IsExcluded = isExcluded;
        }
    }
}
