using Database.DataModel;
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
        private static List<DesignerObj> _designerObjList = DesignerRepoTwo.DesignerObjList;
        public static List<Shp> ShpObjList { get; } = GetShpList();

        //public ShpRepo(double svgWidth, double svgHeight, double margin)
        //{                   
        //}

        private static List<Shp> GetShpList()
        {
            double svgWidth = 800;
            double svgHeight = 800;
            double margin = 20;

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
                    ZoneId = o.ZoneId,
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
                TypeId = 2,

                ZoneId = j.ZoneId,
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

                TypeId = 7,
                ZoneId = p.ZoneId,
            }).ToList();

            var custNodeLineList = customerNodeList.Select(p => new ConnectionShp
            {
                X = p.Geometry[0].X,
                Y = p.Geometry[0].Y,

                X2 = junctionList.FirstOrDefault(x => x.ObjId == p.AssociatedId).Geometry[0].X - p.Geometry[0].X,
                Y2 = junctionList.FirstOrDefault(x => x.ObjId == p.AssociatedId).Geometry[0].Y - p.Geometry[0].Y,

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

        private static List<DesignerObj> GetJunctionList()
        {
            return _designerObjList.Where(f => f.ObjTypeId != 69 && f.ObjTypeId != 73).ToList();
        }
        private static List<DesignerObj> GetPipeList()
        {
            return _designerObjList.Where(f => f.ObjTypeId == 69).ToList();
        }
        private static List<DesignerObj> GetCustomerNodeList()
        {
            return _designerObjList.Where(f => f.ObjTypeId == 73).ToList();
        }

        private static Point2D GetPointTopLeft()
        {
            var junctionList = GetJunctionList();
            var xMin = junctionList.Min(x => x.Geometry[0].X);
            var yMin = junctionList.Min(x => x.Geometry[0].Y);
            return new Point2D(xMin, yMin);
        }
        private static Point2D GetPointBottomRight()
        {
            var junctionList = GetJunctionList();
            var xMax = junctionList.Max(x => x.Geometry[0].X);
            var yMax = junctionList.Max(x => x.Geometry[0].Y);
            return new Point2D(xMax, yMax);
        }
    }
}
