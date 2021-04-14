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
            var pipeList = MainRepo.GetPipeList();
        }

        [TestMethod]
        public void TestMethod2()
        {
            var topLeft = MainRepo.GetPointTopLeft();
            var bottomRight = MainRepo.GetPointBottomRight();
        }
    }
}
