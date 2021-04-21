using Database.DataModel;
using Database.DataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Ui.Designer.Model;

namespace WpfApplication1.Ui.Designer.Repo
{
    public class DesignerRepoTwo
    {
        public static List<DesignerObj> DesignerObjList { get; }  = GetInitialDesignerObjList();

        public static List<DesignerObj> GetListByZone(int zoneId)
        {
            var junctionAndPipeList = DesignerObjList.Where(x => x.ObjTypeId != 23 && x.ObjTypeId != 73 && x.ZoneId == zoneId);
            var customerMeterList = DesignerObjList.Where(x => x.ObjTypeId == 73 && junctionAndPipeList.Any(y => y.ObjId == x.AssociatedId));
            var designerObjList = junctionAndPipeList.Union(customerMeterList).ToList();

            return designerObjList;
        }

        private static List<DesignerObj> GetInitialDesignerObjList()
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
                .Select(x => new DesignerObj()
                {
                    ObjId = x.ObjId,
                    ObjTypeId = x.ObjTypeId,
                    Label = x.Label,
                    IsActive = (bool)x.IsActive,
                    ZoneId = x.ZoneId,
                    AssociatedId = x.AssociatedId,
                    TargetId = x.TargetId,

                    Geometry = infraValueGeometryList.Where(g => g.ObjId == x.ObjId).OrderBy(g => g.OrderNo).Select(g => new Point2D((double)g.Xp, (double)g.Yp)).ToList(),
                })
                .ToList();

            domainObjects.ForEach(x => { x.Xp = x.Geometry[0].X; x.Yp = x.Geometry[0].Y; });
            return domainObjects;
        }
    }
}
