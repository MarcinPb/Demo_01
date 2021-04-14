using System;
using System.Collections.Generic;
using Database.DataModel;
using Database.DataRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Database.DataRepository.Test
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            ImportedDataOutputLists importedDataOutputLists = ImportRepo.GetInfraDataLists();
        }

    }
}
