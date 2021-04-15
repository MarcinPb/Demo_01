using System;
using Database.DataRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Database.DataRepository.Test
{
    [TestClass]
    public class MainRepoTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var pipeList = DesignerBinFileRepo.GetPipeList();
        }

        [TestMethod]
        public void TestMethod2()
        {
            var topLeft = DesignerBinFileRepo.GetPointTopLeft();
            var bottomRight = DesignerBinFileRepo.GetPointBottomRight();
        }
    }
}
