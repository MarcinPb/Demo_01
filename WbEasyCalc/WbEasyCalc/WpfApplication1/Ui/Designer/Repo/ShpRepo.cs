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
        //private static List<DesignerObj> _designerObjList = DesignerRepoTwo.DesignerObjList;
        //public static List<Shp> ShpObjList { get; } = GetShpList();

        //public ShpRepo(double svgWidth, double svgHeight, double margin)
        //{
        //}

        public static List<Shp> GetShpList(double svgWidth, double svgHeight, double margin, int zoneId)
        {
            const double dotR = 0.2;

            var designerObjList = DesignerRepoTwo.DesignerObjList.Where(f => f.ZoneId == zoneId).Select(x => (DesignerObj)x.Clone()).ToList();

            var pointTopLeft = GetPointTopLeft(designerObjList);
            var pointBottomRight = GetPointBottomRight(designerObjList);
            var xFactor = svgWidth / (pointBottomRight.X - pointTopLeft.X);
            var yFactor = svgHeight / (pointBottomRight.Y - pointTopLeft.Y);

            //int designerObjQty = designerObjList.Count;

            foreach (var o in designerObjList)
            {
                //var gem = o.Geometry.Count;
                for (int i = 0; i < o.Geometry.Count; i++)
                {
                    var p = o.Geometry[i];
                    p.X = (p.X - pointTopLeft.X) * xFactor + margin;
                    p.Y = (pointBottomRight.Y - p.Y) * yFactor + margin;
                    o.Geometry[i] = p;
                }
            }

            var pathList = designerObjList
                .Where(f => f.ObjTypeId== 69)
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

            var objMyList = designerObjList
                .Where(f => f.ObjTypeId != 69 && f.ObjTypeId != 73)
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

            var cnShpList = designerObjList
                .Where(f => f.ObjTypeId== 73)
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

            var result = custNodeLineList
                .Select(cl => (Shp)cl)
                .Union(pathList.Select(l => (Shp)l))
                .Union(objMyList.Select(o => (Shp)o))
                .Union(cnShpList.Select(c => (Shp)c))
                .ToList();

            return result;
        }


        private static PathGeometry GetPathGeometry(DesignerObj designerObj)
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

        private static Point GetPointTopLeft(IEnumerable<DesignerObj> junctionList)
        {
            var xMin = junctionList.Min(x => x.Geometry[0].X);
            var yMin = junctionList.Min(x => x.Geometry[0].Y);
            return new Point(xMin, yMin);
        }
        private static Point GetPointBottomRight(IEnumerable<DesignerObj> junctionList)
        {
            var xMax = junctionList.Max(x => x.Geometry[0].X);
            var yMax = junctionList.Max(x => x.Geometry[0].Y);
            return new Point(xMax, yMax);
        }
    }
}
