using Database.DataRepository.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Database.DataRepository.Test
{
    [TestClass]
    public class InfraTest
    {
        [TestMethod]
        public void GetInfraDataTest()
        {
            InfraData infraData1 = InfraRepo.GetInfraData();
            InfraData infraData2 = InfraRepo.GetInfraData();
        }
    }
}
