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
            //List<DomainObjectData> domainObjects = new List<DomainObjectData>();

            InfraData infraData = InfraRepo.GetInfraData();
            //var valueWithTypeList = infraData.InfraChangeableData.InfraObjList
            //    .Join(
            //        infraData.InfraChangeableData.InfraValueList,
            //        l => l.ObjId,
            //        r => r.ObjId,
            //        (l, r) => new { l.ObjTypeId, InfraValue = r }
            //    );

            var infraValueLabelList = infraData.InfraChangeableData.InfraValueList.Where(f => f.FieldId == 2).ToList();         // Label
            //var infraValueXxList = infraData.InfraChangeableData.InfraValueList.Where(f => f.FieldId == 192086302).ToList();    // X
            //var infraValueYyList = infraData.InfraChangeableData.InfraValueList.Where(f => f.FieldId == -334626530).ToList();   // Y
            var infraValueAssociatedList = infraData.InfraChangeableData.InfraValueList.Where(f => f.FieldId == 735).ToList();  // Demand_AssociatedElement

            var infraValueGeometryList = infraData.InfraChangeableData.InfraValueList                                           // Geometry
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
                    (l, r) => new { ObjTypeId = l.ObjTypeId, ObjId = l.ObjId, Label = r.StringValue }
                )
                //.Join(
                //    infraValueXxList,
                //    l => l.ObjId,
                //    r => r.ObjId,
                //    (l, r) => new { ObjTypeId = l.ObjTypeId, ObjId = l.ObjId, Label = l.Label, X = r.FloatValue }
                //)
                //.Join(
                //    infraValueYyList,
                //    l => l.ObjId,
                //    r => r.ObjId,
                //    (l, r) => new { ObjTypeId = l.ObjTypeId, ObjId = l.ObjId, Label = l.Label,X = l.X, Y = r.FloatValue }
                //)
                .GroupJoin(
                    infraValueAssociatedList,
                    l => l.ObjId,
                    r => r.ObjId,
                    //(f, bs) => new { ObjTypeId = f.ObjTypeId, ObjId = f.ObjId, Label = f.Label, X = f.X, Y = f.Y, AssociatedObjId = bs?.SingleOrDefault()?.IntValue }
                    (f, bs) => new { ObjTypeId = f.ObjTypeId, ObjId = f.ObjId, Label = f.Label, AssociatedObjId = bs?.SingleOrDefault()?.IntValue }
                )
                //.Join(
                //    infraValueAssociatedList,
                //    l => l.ObjId,
                //    r => r.ObjId,
                //    (l, r) => new { ObjTypeId = l.ObjTypeId, ObjId = l.ObjId, Label = l.Label, X = l.X, Y = l.Y, AssociatedObjId = r.IntValue }
                //)
                .Select(x => new DomainObjectData()
                {
                    ID = x.ObjId,
                    ObjectType = (ObjectTypes)x.ObjTypeId,
                    Label = x.Label,
                    Geometry = infraValueGeometryList.Where(g => g.ObjId == x.ObjId).OrderBy(g => g.OrderNo).Select(g => new Point2D((double)g.Xp, (double)g.Yp)).ToList(),
                    Fields = GetFieldDict(x.AssociatedObjId),
                })
                .ToList()
                ;

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
