using Database.DataRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.Designer.CustomerNode
{
    public class ItemViewModel : Ui.Designer.ItemXyViewModel
    {
        [Category("Demand")]
        [DisplayName("Associated Element")]
        public string Demand_AssociatedElement { get; set; }

        [Category("Demand")]
        [DisplayName("Base Flow")]
        public double Demand_BaseFlow { get; set; }

        [Category("Demand")]
        [DisplayName("Demand Pattern")]
        public string Demand_DemandPattern { get; set; }

        public ItemViewModel(int id) : base(id)
        {
            Demand_AssociatedElement = (string)_model.Fields["Demand_AssociatedElement"];
            Demand_BaseFlow = (double)_model.Fields["Demand_BaseFlow"];
            Demand_DemandPattern = (string)_model.Fields["Demand_DemandPattern"];
        }
    }
}
