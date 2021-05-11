using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Windows;
using WpfApplication1.Ui.Designer.Repo;

namespace WpfApplication1.Test
{
    [TestClass]
    public class InfraTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var designerObjList1 = DesignerRepoTwo.DesignerObjList;
            var designerObjList2 = DesignerRepoTwo.DesignerObjList;
            //var designerObjList3 = DesignerRepoTwo.GetListByZone(6773);


            double svgWidth = 800;
            double svgHeight = 800;
            double margin = 20;
            //int zoneId = 6773;

            //CanvasWidth = svgWidth + 2 * margin;
            //CanvasHeight = svgHeight + 2 * margin;

            var shpObjList1 = ShpRepo.GetShpList(svgWidth, svgHeight, margin, 6773);
            var shpObjList2 = ShpRepo.GetShpList(svgWidth, svgHeight, margin, 6774);

            //var shpObjList = ShpRepo.ShpObjList;
            //var shpObjListZone1 = ShpRepo.ShpObjList.Where(f => f.ZoneId == 6773).ToList();
            //var shpObjListZone2 = ShpRepo.ShpObjList.Where(f => f.ZoneId == 6774).ToList();
        }

        [TestMethod]
        public void TestMethod2()
        {
            var p = new Point(1, 2);
            p.X = 2;

        }
    }
}
