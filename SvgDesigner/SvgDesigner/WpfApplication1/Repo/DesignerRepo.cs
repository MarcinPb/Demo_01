using Database.DataModel;
using Database.DataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfApplication1.ShapeModel;

namespace WpfApplication1.Repo
{
    public class DesignerRepo
    {
        private readonly List<DesignerObj> _designerObjList;


        public DesignerRepo(int? zoneId = null) 
        {
            _designerObjList = GetDesignerObjList(zoneId);
        }


        public List<Shp> GetShpList(double svgWidth, double svgHeight, double margin)
        {
            const double dotR = 0.2;

            var pointTopLeft = GetPointTopLeft();
            var pointBottomRight = GetPointBottomRight();
            var xFactor = svgWidth / (pointBottomRight.X - pointTopLeft.X);
            var yFactor = svgHeight / (pointBottomRight.Y - pointTopLeft.Y);


            var pipeList = GetPipeList();
            pipeList.ForEach(t => t.Geometry.ForEach(p => { p.X = (p.X - pointTopLeft.X) * xFactor + margin; p.Y = (pointBottomRight.Y - p.Y) * yFactor + margin; }));
            var pathList = pipeList
                .Select(o => new PathShp
                {
                    Id = o.ObjId,
                    Name = o.Label,
                    X = o.Geometry[0].X,
                    Y = o.Geometry[0].Y,

                    Geometry = GetPathGeometry(o),  
                    TypeId = 6,
                })
                .ToList();

            var junctionList = GetJunctionList();
            junctionList.ForEach(t => t.Geometry.ForEach(p => { p.X = (p.X - pointTopLeft.X) * xFactor + margin; p.Y = (pointBottomRight.Y - p.Y) * yFactor + margin; }));
            var objMyList = junctionList.Select(j => new EllipseShp
            {
                Id = j.ObjId,
                Name = j.Label,
                X = j.Geometry[0].X - dotR,
                Y = j.Geometry[0].Y - dotR,
                Width = 2 * dotR,
                Height = 2 * dotR,
                TypeId = 2
            }).ToList();

            var customerNodeList = GetCustomerNodeList();
            customerNodeList.ForEach(t => t.Geometry.ForEach(p => { p.X = (p.X - pointTopLeft.X) * xFactor + margin; p.Y = (pointBottomRight.Y - p.Y) * yFactor + margin; }));
            var cnShpList = customerNodeList.Select(p => new RectangleShp
            {
                Id = p.ObjId,
                X = p.Geometry[0].X - dotR,
                Y = p.Geometry[0].Y - dotR,
                Width = 2 * dotR,
                Height = 2 * dotR,

                TypeId = 7
            }).ToList();

            var custNodeLineList = customerNodeList.Select(p => new ConnectionShp
            {
                X = p.Geometry[0].X,
                Y = p.Geometry[0].Y,

                X2 = junctionList.FirstOrDefault(x => x.ObjId == p.AssociatedId).Geometry[0].X - p.Geometry[0].X,
                Y2 = junctionList.FirstOrDefault(x => x.ObjId == p.AssociatedId).Geometry[0].Y - p.Geometry[0].Y,

                TypeId = 0,
            }).ToList();


            var result = custNodeLineList.Select(cl => (Shp)cl)
                .Union(pathList.Select(l => (Shp)l))
                .Union(objMyList.Select(o => (Shp)o))
                .Union(cnShpList.Select(c => (Shp)c))
                .ToList();

            return result;
        }


        internal DesignerObj GetItem(int objId)
        {
            var item = _designerObjList.FirstOrDefault(x => x.ObjId == objId);
            item.Fields = GetObjFieldValueList(objId);

            return item;
        }

        private PathGeometry GetPathGeometry(DesignerObj designerObj)
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(0, 0);


            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
            var geometryListWithoutFirst = designerObj.Geometry.Where(f => f != designerObj.Geometry[0]);
            foreach(var pt in geometryListWithoutFirst)
            {
                LineSegment myLineSegment = new LineSegment();
                myLineSegment.Point = new Point(pt.X - designerObj.Geometry[0].X, pt.Y - designerObj.Geometry[0].Y);

                myPathSegmentCollection.Add(myLineSegment);
            }

            myPathFigure.Segments = myPathSegmentCollection;

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            myPathFigureCollection.Add(myPathFigure);

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;

            return myPathGeometry;
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

        private List<DesignerObj> GetJunctionList()
        {
            return _designerObjList.Where(f => f.ObjTypeId != 69 && f.ObjTypeId != 73).ToList();
        }
        private List<DesignerObj> GetPipeList()
        {
            return _designerObjList.Where(f => f.ObjTypeId == 69).ToList();
        }
        private List<DesignerObj> GetCustomerNodeList()
        {
            return _designerObjList.Where(f => f.ObjTypeId == 73).ToList();
        }

        private Point2D GetPointTopLeft()
        {
            var junctionList = GetJunctionList();
            var xMin = junctionList.Min(x => x.Geometry[0].X);
            var yMin = junctionList.Min(x => x.Geometry[0].Y);
            return new Point2D(xMin, yMin);
        }
        private Point2D GetPointBottomRight()
        {
            var junctionList = GetJunctionList();
            var xMax = junctionList.Max(x => x.Geometry[0].X);
            var yMax = junctionList.Max(x => x.Geometry[0].Y);
            return new Point2D(xMax, yMax);
        }

        private static List<DesignerObj> GetDesignerObjList(int? zoneId = null)
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

            if (zoneId != null)
            {
                var junctionList = domainObjects.Where(x => x.ObjTypeId != 23 && x.ObjTypeId != 73 && x.ZoneId == zoneId);
                var customerMeterList = domainObjects.Where(x => x.ObjTypeId == 73 && junctionList.Any(y => y.ObjId == x.AssociatedId));
                domainObjects = junctionList.Union(customerMeterList).ToList();
            }

            domainObjects.ForEach(x => { x.Xp = x.Geometry[0].X; x.Yp = x.Geometry[0].Y; });
            return domainObjects;
        }
    }
}
