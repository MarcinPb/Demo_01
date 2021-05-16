using Database.DataModel.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfApplication1.Ui.Designer.Model;
using WpfApplication1.Ui.Designer.Model.ShapeModel;

namespace WpfApplication1.Ui.Designer.Repo
{
    public class ShpRepo
    {
        private List<Shp> _shpList;
        
        private double _margin;

        private Point _pointTopLeft;
        private Point _pointBottomRight;
        private double _xFactor;
        private double _yFactor;

        public ShpRepo(double svgWidth, double svgHeight, double margin, List<DesignerObj> designerObjList)
        {
            const double dotR = 0.2;

            _margin = margin;

            _pointTopLeft = GetPointTopLeft(designerObjList);
            _pointBottomRight = GetPointBottomRight(designerObjList);
            _xFactor = svgWidth / (_pointBottomRight.X - _pointTopLeft.X);
            _yFactor = svgHeight / (_pointBottomRight.Y - _pointTopLeft.Y);

            // Geometry
            foreach (var o in designerObjList)
            {
                for (int i = 0; i < o.Geometry.Count; i++)
                {
                    var p = o.Geometry[i];
                    p.X = (p.X - _pointTopLeft.X) * _xFactor + margin;                // x = (pX - margin) / xFactor + pointTopLeft.X
                    p.Y = (_pointBottomRight.Y - p.Y) * _yFactor + margin;            // y = pointBottomRight.Y - (pY - margin) / yFactor 
                    o.Geometry[i] = p;
                }
            }

            // PushPin (1000)
            var pushPinList = designerObjList
                .Where(f => f.ObjTypeId == 1000)
                .Select(j => new PushPinShp
                {
                    Id = j.ObjId,
                    Name = j.Label,
                    X = j.Geometry[0].X,
                    Y = j.Geometry[0].Y,
                    TypeId = 2,

                    ZoneId = j.ZoneId,
                }).ToList();

            // Pipes (69)
            var pathList = designerObjList
                .Where(f => f.ObjTypeId == 69)
                .Select(o => new PathShp
                {
                    Id = o.ObjId,
                    Name = o.Label,
                    X = o.Geometry[0].X,
                    Y = o.Geometry[0].Y,

                    Geometry = GetPathGeometry(o),
                    TypeId = 6,
                    ZoneId = o.ZoneId,
                })
                .ToList();

            // All shapes except Pipes, CustomerMeters and PushPin
            var objMyList = designerObjList
                .Where(f => f.ObjTypeId != 69 && f.ObjTypeId != 73 && f.ObjTypeId != 1000)
                .Select(j => new EllipseShp
                {
                    Id = j.ObjId,
                    Name = j.Label,
                    X = j.Geometry[0].X - dotR,
                    Y = j.Geometry[0].Y - dotR,
                    Width = 2 * dotR,
                    Height = 2 * dotR,
                    TypeId = 2,

                    ZoneId = j.ZoneId,
                }).ToList();


            // CustomerMeters (73)
            var cnShpList = designerObjList
                .Where(f => f.ObjTypeId == 73)
                .Select(p => new RectangleShp
                {
                    Id = p.ObjId,
                    X = p.Geometry[0].X - dotR,
                    Y = p.Geometry[0].Y - dotR,
                    Width = 2 * dotR,
                    Height = 2 * dotR,

                    TypeId = 7,
                    ZoneId = p.ZoneId,
                }).ToList();

            // Connections between CustomerMeter and its attached Junction
            var custNodeLineList = designerObjList
                .Where(f => f.ObjTypeId == 73 && f.AssociatedId != null)
                .Select(p => new ConnectionShp
                {
                    X = p.Geometry[0].X,
                    Y = p.Geometry[0].Y,

                    X2 = designerObjList.FirstOrDefault(x => x.ObjId == p.AssociatedId).Geometry[0].X - p.Geometry[0].X,
                    Y2 = designerObjList.FirstOrDefault(x => x.ObjId == p.AssociatedId).Geometry[0].Y - p.Geometry[0].Y,

                    TypeId = 0,
                    ZoneId = p.ZoneId,
                }).ToList();

            // All shaps together
            var result = custNodeLineList
                .Select(cl => (Shp)cl)
                .Union(pathList.Select(l => (Shp)l))
                .Union(objMyList.Select(o => (Shp)o))
                .Union(cnShpList.Select(c => (Shp)c))
                .Union(pushPinList.Select(c => (Shp)c))
                .ToList();

            _shpList = result;

        }

        public List<Shp> GetShpList()
        {
            return _shpList;
        }

        internal Point ShpPointToDesignerPoint(Point point)
        {

            //p.X = (x - pointTopLeft.X) * xFactor + margin;                // x = (pX - margin) / xFactor + pointTopLeft.X
            //p.Y = (pointBottomRight.Y - y) * yFactor + margin;            // y = pointBottomRight.Y - (pY - margin) / yFactor 
            var xx = (point.X - _margin) / _xFactor + _pointTopLeft.X;
            var yy = _pointBottomRight.Y - (point.Y - _margin) / _yFactor;

            return new Point(xx, yy);
        }

        private PathGeometry GetPathGeometry(DesignerObj designerObj)
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(0, 0);

            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
            var geometryListWithoutFirst = designerObj.Geometry.Where(f => f != designerObj.Geometry[0]);
            foreach (var pt in geometryListWithoutFirst)
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

        private Point GetPointTopLeft(IEnumerable<DesignerObj> junctionList)
        {
            var xMin = junctionList.Min(x => x.Geometry[0].X);
            var yMin = junctionList.Min(x => x.Geometry[0].Y);
            return new Point(xMin, yMin);
        }
        private Point GetPointBottomRight(IEnumerable<DesignerObj> junctionList)
        {
            var xMax = junctionList.Max(x => x.Geometry[0].X);
            var yMax = junctionList.Max(x => x.Geometry[0].Y);
            return new Point(xMax, yMax);
        }
    }
}
