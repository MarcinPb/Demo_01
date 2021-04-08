using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeometryReader.Test
{
    [TestClass]
    public class ImportDataTest
    {
        private readonly string _sqliteFile = @"K:\temp\sandbox\Nowy model testowy\testOPC.wtg.sqlite";

        [TestMethod]
        public void ReadBaseTest()
        {
            Reader.ReadBase(_sqliteFile);
        }
        [TestMethod]
        public void ReadDataTest()
        {
            Reader.ReadData(_sqliteFile);
        }
    }
}
