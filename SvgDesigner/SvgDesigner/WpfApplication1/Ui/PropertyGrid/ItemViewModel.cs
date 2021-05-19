using Database.DataModel.Infra;
using Database.DataRepository.Infra;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using WpfApplication1.Ui.Designer.Model;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.PropertyGrid
{
    public class ItemViewModel : ViewModelBase
    {
        protected DesignerObj _model;


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
        public string Zone { get; set; }

        public ItemViewModel(int objId)
        {
            InfraData infraData = InfraRepo.GetInfraData();

            var geometry = infraData.InfraChangeableData.InfraValueList.Where(f => f.ObjId == objId).Join(
                    infraData.InfraChangeableData.InfraGeometryList,
                    l => l.ValueId,
                    r => r.ValueId,
                    (l, r) => new Point(r.Xp, r.Yp)
                )
                .ToList();

            _model = new DesignerObj()
            {
                ObjId = objId,
                Label = infraData.InfraChangeableData.InfraValueList.FirstOrDefault(f => f.ObjId == objId && f.FieldId == infraData.InfraSpecialFieldId.Label).StringValue,
                IsActive = infraData.InfraChangeableData.InfraValueList.FirstOrDefault(f => f.ObjId == objId && f.FieldId == infraData.InfraSpecialFieldId.HMIActiveTopologyIsActive).BooleanValue ?? false,
                ZoneId = infraData.InfraChangeableData.InfraValueList.FirstOrDefault(f => f.ObjId == objId && f.FieldId == infraData.InfraSpecialFieldId.Physical_Zone)?.IntValue,

                Fields = GetObjFieldValueList(objId),
                Geometry = geometry,
            };

            Id = _model.ObjId;
            Name = _model.Label;
            IsActive = _model.IsActive;

            //Zone = _model.ZoneId.ToString();
            if(_model.ZoneId != null)
            {
                var zoneList = infraData.InfraChangeableData.ZoneDict;
                Zone = zoneList.FirstOrDefault(x => x.ZoneId == _model.ZoneId)?.Name;
            }
        }

        [Category("Physical")]
        [DisplayName("ZoneItemsSource")]
        [Browsable(false)]
        public ObservableCollection<string> ZoneItemsSource
        {
            get
            {
                ObservableCollection<string> list = new ObservableCollection<string>(InfraRepo.GetInfraData().InfraChangeableData.ZoneDict.Select(x => x.Name));
                return list;
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
                case 8:
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
