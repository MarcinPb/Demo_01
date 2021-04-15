using Database.DataRepository;
using GeometryModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using WpfApplication1.ShapeModel;
using WpfApplication1.Utility;

namespace WpfApplication1.Ui.Designer
{
    public class DesignerViewModel : ViewModelBase
    {
        public DateTime StartDate { get; set; }
        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }

        public ObservableCollection<Shp> ObjList { get; set; }

        private int _selectedItem;
        public int SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; RaisePropertyChanged(nameof(SelectedItem)); }
        }

        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new RelayCommand(OnAddExecute, () => true)); }
        }
        private void OnAddExecute()
        {
            ObjList.Add(new ObjMy() { Id = 9, X = 210, Y = 30, Width = 10, Height = 10, TypeId = 2 });
        }

        private ICommand _moveCommand;
        public ICommand MoveCommand
        {
            get { return _moveCommand ?? (_moveCommand = new RelayCommand(OnMoveExecute, () => true)); }
        }
        private void OnMoveExecute()
        {
            var obj = ObjList.LastOrDefault();
            if (obj != null) obj.X += 20;
        }

        public RelayCommand<object> OnMouseDoubleClickCmd { get; }
        private void OnMouseDoubleClickCmdExecute(object obj)
        {
            int id = 0;
            MouseEventArgs e = (MouseEventArgs)obj;
            var position = e.GetPosition(e.Device.Target);
            if (e.Device.Target is Line)
            {
                var line = (Line)e.Device.Target;
                id = Convert.ToInt32(((Line)e.Device.Target).Tag);
            }
            if (e.Device.Target is Ellipse)
            {
                id = Convert.ToInt32(((Ellipse)e.Device.Target).Tag);
            }
            if (e.Device.Target is Rectangle)
            {
                id = Convert.ToInt32(((Rectangle)e.Device.Target).Tag);

            }
            SelectedItem = id;
            var shp = ObjList.FirstOrDefault(x => x.Id == id);
            Messenger.Default.Send(shp);
        }

        public DesignerViewModel()
        {
            //ObjList = _objTempList;
            StartDate = Convert.ToDateTime("2021-03-09 11:30");

            OnMouseDoubleClickCmd = new RelayCommand<object>(OnMouseDoubleClickCmdExecute);

            double svgWidth = 800;
            double svgHeight = 800;
            double margin = 20;

            double dotR = 0.2;

            CanvasWidth = svgWidth + 2 * margin;
            CanvasHeight = svgHeight + 2 * margin;

            var pointTopLeft = DesignerBinFileRepo.GetPointTopLeft();
            var pointBottomRight = DesignerBinFileRepo.GetPointBottomRight();
            var xFactor = svgWidth / (pointBottomRight.X - pointTopLeft.X);
            var yFactor = svgHeight / (pointBottomRight.Y - pointTopLeft.Y);


            var pipeList = DesignerBinFileRepo.GetPipeList();
            pipeList.ForEach(t => t.Geometry.ForEach(p => { p.X = (p.X - pointTopLeft.X) * xFactor + margin; p.Y = (pointBottomRight.Y - p.Y) * yFactor + margin; }));
            //var linkMyList = pipeList.Select(o => new LinkMy
            //{
            //    Id = o.ID,
            //    Name = o.Label,
            //    X = o.Geometry[0].X,
            //    Y = o.Geometry[0].Y,

            //    X2 = o.Geometry.Last().X - o.Geometry[0].X,
            //    Y2 = o.Geometry.Last().Y - o.Geometry[0].Y,

            //    Path = o.Geometry,
            //    TypeId = 6,
            //});
            var linkList = pipeList
                .SelectMany(x => x.Geometry,  (p, c) => new 
                    {
                        Id = p.ID,
                        Name = p.Label,
                        X = c.X,
                        Y = c.Y,
                    })
                .Select((g, idx) => new 
                    {
                        Id = g.Id,
                        Name = g.Name,
                        X = g.X,
                        Y = g.Y,
                        Idx = idx
                    })
                ;
            var linkMyList = linkList.Join(
                        linkList,
                        l => l.Idx,
                        r => r.Idx + 1,
                        (l, r) => new { l, r }
                    )
                    .Where(x => x.l.Id == x.r.Id)
                    .Select(o => new LinkMy
                    {
                        Id = o.l.Id,
                        Name = o.l.Name,
                        X = o.l.X,
                        Y = o.l.Y,

                        X2 = o.r.X - o.l.X,
                        Y2 = o.r.Y - o.l.Y,

                        TypeId = 6,
                    })
                    .ToList()
                    ;

            var junctionList = DesignerBinFileRepo.GetJunctionList();
 
            //junctionList.ForEach(p => { p.Geometry[0].X = (p.Geometry[0].X - pointTopLeft.X) * xFactor + margin; p.Geometry[0].Y = (pointBottomRight.Y - p.Geometry[0].Y) * yFactor + margin; });
            junctionList.ForEach(t => t.Geometry.ForEach(p => { p.X = (p.X - pointTopLeft.X) * xFactor + margin; p.Y = (pointBottomRight.Y - p.Y) * yFactor + margin; }));
            var objMyList = junctionList.Select(j => new ObjMy
            {
                Id = j.ID,
                Name = j.Label,
                X = j.Geometry[0].X - dotR,
                Y = j.Geometry[0].Y - dotR,
                Width = 2 * dotR,
                Height = 2 * dotR,
                TypeId = 2
            });

            var customerNodeList = DesignerBinFileRepo.GetCustomerNodeList();

            //customerNodeList.ForEach(p => { p.Geometry[0].X = (p.Geometry[0].X - pointTopLeft.X) * xFactor + margin; p.Geometry[0].Y = (pointBottomRight.Y - p.Geometry[0].Y) * yFactor + margin; });
            customerNodeList.ForEach(t => t.Geometry.ForEach(p => { p.X = (p.X - pointTopLeft.X) * xFactor + margin; p.Y = (pointBottomRight.Y - p.Y) * yFactor + margin; }));
            var cnShpList = customerNodeList.Select(p => new CnShp
            {
                Id = p.ID,
                X = p.Geometry[0].X - dotR,
                Y = p.Geometry[0].Y - dotR,
                Width = 2 * dotR,
                Height = 2 * dotR,

                //X1 = 0,
                //Y1 = 0,
                //X2 = objMyList.FirstOrDefault(x => x.Name == (string)p.Fields["Demand_AssociatedElement"]).X - (p.Geometry[0].X - 2*dotR),
                //Y2 = objMyList.FirstOrDefault(x => x.Name == (string)p.Fields["Demand_AssociatedElement"]).Y - (p.Geometry[0].Y - 2*dotR),

                TypeId = 7
            }); ;


            var custNodeLineList = customerNodeList.Select(p => new CnLineShp
            {
                //Id = o.ID,
                //Name = o.Label,
                X = p.Geometry[0].X,
                Y = p.Geometry[0].Y,

                //X2 = o.Geometry.Last().X - o.Geometry[0].X,
                //Y2 = o.Geometry.Last().Y - o.Geometry[0].Y,
                X2 = junctionList.FirstOrDefault(x => x.Label == (string)p.Fields["Demand_AssociatedElement"]).Geometry[0].X - p.Geometry[0].X,
                Y2 = junctionList.FirstOrDefault(x => x.Label == (string)p.Fields["Demand_AssociatedElement"]).Geometry[0].Y - p.Geometry[0].Y,

                Path = new List<Point2D>(),
                TypeId = 0,
            });




            ObjList = new ObservableCollection<Shp>(
                custNodeLineList.Select(cl => (Shp)cl)
                    .Union(linkMyList.Select(l => (Shp)l))
                    .Union(objMyList.Select(o => (Shp)o))
                    .Union(cnShpList.Select(c => (Shp)c))
                );
        }

        private int GetIdBasedOnField(DomainObjectData obj) 
        {
            var junctionName = (string)obj.Fields["Demand_AssociatedElement"];

            return 1;
        }

        #region Waste
        private readonly ObservableCollection<Shp> _objTempList = new ObservableCollection<Shp>()
        {
            //new ObjMy() {Id=1, X=10,  Y=30,  Width=15,  Height=15, ConnTypeId=2 },
            //new ObjMy() {Id=2, X=70,  Y=40,  Width=15,  Height=15, ConnTypeId=2 },
            //new ObjMy() {Id=3, X=20,  Y=80,  Width=15,  Height=15, ConnTypeId=2 },
            //new ObjMy() {Id=4, X=80,  Y=100, Width=15,  Height=15, ConnTypeId=2 },
            //new ObjMy() {Id=5, X=110, Y=30,  Width=15,  Height=15, ConnTypeId=2 },
            //new ObjMy() {Id=6, X=170, Y=40,  Width=15,  Height=15, ConnTypeId=2 },
            //new ObjMy() {Id=7, X=120, Y=80,  Width=15,  Height=15, ConnTypeId=2 },
            //new ObjMy() {Id=8, X=180, Y=100, Width=15,  Height=15, ConnTypeId=2 },

            new ObjMy() {Id=1, TypeId=2, X=150,  Y=50,   Width=15,  Height=15 },
            new ObjMy() {Id=2, TypeId=2, X=100,  Y=150,  Width=15,  Height=15 },

            new LinkMy() {Id=11, TypeId=6, X=210,  Y=30,  X2=10,   Y2=50  },
            new LinkMy() {Id=12, TypeId=6, X=150,  Y=50,  X2=-50,  Y2=100 },
        };
        #endregion
    }
}
