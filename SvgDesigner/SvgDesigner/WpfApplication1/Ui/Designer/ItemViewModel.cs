using Database.DataModel;
using Database.DataRepository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Repo;
using WpfApplication1.Utility;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace WpfApplication1.Ui.Designer
{
    public class ItemViewModel : ViewModelBase
    {

        private int _id;
        [Category("<General>")]
        [DisplayName("ID")]
        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        private string _name;
        [Category("<General>")]
        [DisplayName("Label")]
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged("Name"); }
        }

        [Category("Active Topology")]
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [Category("Physical")]
        [DisplayName("Zone")]
        [Description("This property uses the DoubleUpDown as the default editor.")] 
        //[ItemsSource(typeof(FontSizeItemsSource))]
        public string Zone { get; set; }

        protected DesignerObj _model;
        public ItemViewModel(int objId)
        {
            //_model = new DesignerRepo().GetItem(objId);
            InfraData infraData = InfraRepo.GetInfraData();

            var geometry = infraData.InfraChangeableData.InfraValueList.Where(f => f.ObjId == objId).Join(
                    infraData.InfraChangeableData.InfraGeometryList,
                    l => l.ValueId,
                    r => r.ValueId,
                    (l, r) => new Point2D(r.Xp, r.Yp)
                )
                .ToList();

            _model = new DesignerObj()
            {
                ObjId = objId,
                Label = infraData.InfraChangeableData.InfraValueList.FirstOrDefault(f => f.ObjId == objId && f.FieldId == 2).StringValue,
                IsActive = infraData.InfraChangeableData.InfraValueList.FirstOrDefault(f => f.ObjId == objId && f.FieldId == 612).BooleanValue ?? false,
                ZoneId = infraData.InfraChangeableData.InfraValueList.FirstOrDefault(f => f.ObjId == objId && f.FieldId == 614)?.IntValue,

                Fields = GetObjFieldValueList(objId),
                Geometry = geometry,
            };

            Id = _model.ObjId;
            Name = _model.Label;
            IsActive = _model.IsActive;
            Zone = _model.ZoneId.ToString();

        }


        [Category("Physical")]
        [DisplayName("FontSizeItemsSource")]
        [Browsable(false)]
        public ObservableCollection<string> FontSizeItemsSource
        {
            get
            {
                ObservableCollection<string> sizes = new ObservableCollection<string>();

                // Items generation could be made here
                sizes.Add("1 - Przybków");
                sizes.Add("2 - Stare Miasto");
                sizes.Add("3 - Kopernik");
                sizes.Add("4 - Piekary");
                sizes.Add("5 - Północ");
                sizes.Add("6 - ZPW");
                return sizes;
            }

        }
        private Dictionary<string, object> GetObjFieldValueList(int objId)
        {
            InfraData infraData = InfraRepo.GetInfraData();

            var infraFieldList = infraData.InfraConstantData.InfraFieldList;
            var infraValueList = infraData.InfraChangeableData.InfraValueList.Where(f => f.ObjId == objId);

            Dictionary<string, object> dict = infraFieldList
                .Join(
                    infraValueList,
                    l => l.FieldId,
                    r => r.FieldId,
                    (l, r) => new { Key = l.Name, Value = GetFieldValue(l, r) }
                    )
                .ToDictionary(x => x.Key, x => (object)x.Value);
            return dict;
        }
        private object GetFieldValue(InfraField infraField, InfraValue infraValue)
        {
            object result = null;
            switch (infraField.DataTypeId)
            {
                case 1:
                    result = infraValue.IntValue;
                    break;
                case 2:
                    result = infraValue.FloatValue;
                    break;
                case 3:
                case 4:
                    result = infraValue.StringValue;
                    break;
                case 5:
                    result = infraValue.DateTimeValue;
                    break;
                case 6:
                    result = infraValue.BooleanValue;
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }
    }


}
