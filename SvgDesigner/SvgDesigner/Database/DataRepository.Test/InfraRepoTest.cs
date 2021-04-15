using System;
using System.Collections.Generic;
using System.Linq;
using Database.DataModel;
using Database.DataRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Database.DataRepository.Test
{
    [TestClass]
    public class InfraRepoTest
    {
        [TestMethod]
        public void GetInfraDataTest()
        {
            InfraData infraData1 = InfraRepo.GetInfraData();
            InfraData infraData2 = InfraRepo.GetInfraData();
        }
    }
}
