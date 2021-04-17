using GeometryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataRepository
{
    public class DesignerInfraRepo : IDesignerRepo
    {
        private static readonly List<DomainObjectData> _domainObjectDataList = GetDomainObjectDataList();

        public DomainObjectData GetItem(int id)
        {
            return _domainObjectDataList.FirstOrDefault(x => x.ID == id);
        }

        public List<DomainObjectData> GetJunctionList()
        {
            //return _domainObjectDataList.Where(f => f.ObjectType == (ObjectTypes)55).ToList();
            return _domainObjectDataList.Where(f => f.ObjectType != (ObjectTypes)69 && f.ObjectType != (ObjectTypes)73).ToList();
        }
        public List<DomainObjectData> GetPipeList()
        {
            return _domainObjectDataList.Where(f => f.ObjectType == (ObjectTypes)69).ToList();
        }
        public List<DomainObjectData> GetCustomerNodeList()
        {
            return _domainObjectDataList.Where(f => f.ObjectType == (ObjectTypes)73).ToList();
        }

        public Point2D GetPointTopLeft()
        {
            var junctionList = GetJunctionList();
            var xMin = junctionList.Min(x => x.Geometry[0].X);
            var yMin = junctionList.Min(x => x.Geometry[0].Y);
            return new Point2D(xMin, yMin);
        }
        public Point2D GetPointBottomRight()
        {
            var junctionList = GetJunctionList();
            var xMax = junctionList.Max(x => x.Geometry[0].X);
            var yMax = junctionList.Max(x => x.Geometry[0].Y);
            return new Point2D(xMax, yMax);
        }

        private static List<DomainObjectData> GetDomainObjectDataList()
        {
            InfraData infraData = InfraRepo.GetInfraData();

            var infraValueLabelList = infraData.InfraChangeableData.InfraValueList.Where(f => f.FieldId == 2).ToList();                 // Label
            var infraValueIsActiveList = infraData.InfraChangeableData.InfraValueList.Where(f => f.FieldId == 612).ToList();            // HMIActiveTopologyIsActive
            var infraValueZoneIdList = infraData.InfraChangeableData.InfraValueList.Where(f => f.FieldId == 614).ToList();              // Physical_Zone
            var infraValueAssociatedList = infraData.InfraChangeableData.InfraValueList.Where(f => f.FieldId == 735).ToList();          // Demand_AssociatedElement (for Customer Meter)
            var infraValueTargerList = infraData.InfraChangeableData.InfraValueList.Where(f => f.FieldId == 1002).ToList();             // Scada_TargetElement (for SCADA Element)

            var infraValueGeometryList = infraData.InfraChangeableData.InfraValueList                                                   // Geometry
                .Join(
                    infraData.InfraChangeableData.InfraGeometryList,
                    l => l.ValueId,
                    r => r.ValueId,
                    (l, r) => new { l.ObjId, r.OrderNo, r.Xp, r.Yp }
                )
                .ToList();

            var domainObjects = infraData.InfraChangeableData.InfraObjList
                .Join(
                    infraValueLabelList,
                    l => l.ObjId,
                    r => r.ObjId,
                    (l, r) => new { l.ObjTypeId, l.ObjId, Label = r.StringValue }
                )
                .Join(
                    infraValueIsActiveList,
                    l => l.ObjId,
                    r => r.ObjId,
                    (l, r) => new { l.ObjTypeId, l.ObjId, l.Label, IsActive = r.BooleanValue }
                )
                .GroupJoin(
                    infraValueZoneIdList,
                    l => l.ObjId,
                    r => r.ObjId,
                    (f, bs) => new { f.ObjTypeId, f.ObjId, f.Label, f.IsActive, ZoneId = bs?.SingleOrDefault()?.IntValue }
                )
                .GroupJoin(
                    infraValueAssociatedList,
                    l => l.ObjId,
                    r => r.ObjId,
                    (f, bs) => new { f.ObjTypeId, f.ObjId, f.Label, f.IsActive, f.ZoneId, AssociatedId = bs?.SingleOrDefault()?.IntValue }
                )
                .GroupJoin(
                    infraValueTargerList,
                    l => l.ObjId,
                    r => r.ObjId,
                    (f, bs) => new { f.ObjTypeId, f.ObjId, f.Label, f.IsActive, f.ZoneId, f.AssociatedId, TargetId = bs?.SingleOrDefault()?.IntValue }
                )
                .Select(x => new DomainObjectData()
                {
                    ID = x.ObjId,
                    ObjTypeId = x.ObjTypeId,
                    Label = x.Label,
                    IsActive = (bool)x.IsActive,
                    ZoneId = x.ZoneId,
                    AssociatedId = x.AssociatedId,
                    TargetId = x.TargetId,

                    Geometry = infraValueGeometryList.Where(g => g.ObjId == x.ObjId).OrderBy(g => g.OrderNo).Select(g => new Point2D((double)g.Xp, (double)g.Yp)).ToList(),
                                 
                    ObjectType = (ObjectTypes)x.ObjTypeId,
                    Zone = x.ZoneId.ToString(),
                    Fields = GetFieldDict(x.AssociatedId),
                })
                .ToList();

            domainObjects.ForEach(x => { x.Xp = x.Geometry[0].X; x.Yp = x.Geometry[0].Y; });
            return domainObjects;
        }


        private static Dictionary<string, object> GetFieldDict(int? associatedObjId)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Demand_AssociatedElement", associatedObjId);
            return dict;
        }

    }
}
