using Database.DataModel;
using Database.DataRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Utility;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace WpfApplication1.Ui.PropertyGrid.Pipe
{
    public class ItemViewModel : Ui.PropertyGrid.ItemViewModel
    {
        private List<Point2D> _path;
        [Category("<Geometry>")]
        [DisplayName("Geometry")]
        public List<Point2D> Path
        {
            get { return _path; }
            set { _path = value; RaisePropertyChanged("Path"); }
        }

        //public double HMITopologyStartNodeID { get; set; }

        [Category("Active Topology")]
        [DisplayName("Start Node")]
        public string HMITopologyStartNodeLabel { get; set; }

        //public double HMITopologyStopNodeID { get; set; }

        [Category("Active Topology")]
        [DisplayName("Stop Node")]
        public string HMITopologyStopNodeLabel { get; set; }

        [Category("Physical")]
        [DisplayName("User Defined Length")]
        public bool Physical_IsUserDefinedLength { get; set; }

        [Category("Physical")]
        [DisplayName("Pipe Status")]
        public int PipeStatus { get; set; }

        [Category("Physical")]
        [DisplayName("Pipe Material")]
        public string Physical_PipeMaterial { get; set; }

        [Category("Physical")]
        [DisplayName("Installation Year")]
        public int Physical_InstallationYear { get; set; }

        [Category("Physical")]
        [DisplayName("HMI Geometry Scaled Length")]
        public double HMIGeometryScaledLength { get; set; }

        [Category("Physical")]
        [DisplayName("Pipe Diameter")]
        public double Physical_PipeDiameter { get; set; }


        public ItemViewModel(int id) : base(id)
        {
            Path = _model.Geometry.ToList();

            //HMITopologyStartNodeLabel = (string)_model.Fields["HMITopologyStartNodeLabel"];
            //HMITopologyStopNodeLabel = (string)_model.Fields["HMITopologyStopNodeLabel"];
            Physical_IsUserDefinedLength = (bool)_model.Fields["Physical_IsUserDefinedLength"];
            PipeStatus = (int)(_model.Fields["PipeStatus"] ?? 0);
            Physical_PipeMaterial = (string)_model.Fields["Physical_PipeMaterial"];
            Physical_InstallationYear = (int)_model.Fields["Physical_InstallationYear"];
            HMIGeometryScaledLength = (double)_model.Fields["HMIGeometryScaledLength"];
            Physical_PipeDiameter = (double)_model.Fields["Physical_PipeDiameter"];
        }
    }
}
