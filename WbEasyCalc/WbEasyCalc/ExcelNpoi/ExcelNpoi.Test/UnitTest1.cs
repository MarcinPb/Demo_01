using ExcelNpoi.ExcelNpoi.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ExcelNpoi.ExcelNpoi.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var excelReader = new ExcelReader(@"..\..\Data\TestFile.xlsx");

            string a1 = excelReader.ReadCell<string>("Ark1", "A1");
            string actual = a1;
            string expected = "Asia";
            Assert.AreEqual(expected, actual, $"actual = {actual}, expected = {expected}");

            int a2 = excelReader.ReadCell<int>("Ark1", "B1");
            int actualInt = a2;
            int expectedInt = 35435;
            Assert.AreEqual(expected, actual, $"actual = {actualInt}, expected = {expectedInt}");

            double a3 = excelReader.ReadCell<int>("Ark1", "C1");
            double actualDouble = a3;
            double expectedDouble = 33.99;
            Assert.AreEqual(expected, actual, $"actual = {actualDouble}, expected = {expectedDouble}");
        }

        [TestMethod]
        public void TestMethod2()
        {
            var excelReader = new ExcelReader(@"..\..\Data\TestFile.xlsx");
            string fileName = @"..\..\Data\TestFileCreated.xlsx";
            List<CellByRowColumnNo> cellList = new List<CellByRowColumnNo>()
            {
                new CellByRowColumnNo("Ark1", 0, 0, "Asica", typeof(string)),
                new CellByRowColumnNo("Ark1", 1, 0, 333, typeof(int)),
                new CellByRowColumnNo("Ark1", 2, 0, 0.11111, typeof(double)),
            };

            excelReader.WriteCellList(fileName, cellList);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var excelReader = new ExcelReader(@"..\..\Data\TestFile.xlsx");
            string fileName = @"..\..\Data\TestFileCreated.xlsx";

            excelReader.WriteToCell<string>("Ark1", 0, 0, "Asicaaaaaa");
            excelReader.WriteToCell<int>("Ark1", 1, 0, 22222);
            excelReader.WriteToCell<double>("Ark1", 2, 0, 0.222222);

            excelReader.WriteToFile(fileName);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var excelReader = new ExcelReader(@"..\..\Data\TestFile.xlsx");
            string fileName = @"..\..\Data\TestFileCreated.xlsx";

            excelReader.WriteToCell<string>("Ark1", "A1", "Asicaaaaaa");
            excelReader.WriteToCell<int>("Ark1", "B1", 22222);
            excelReader.WriteToCell<double>("Ark1", "C1", 0.222222);

            excelReader.WriteToFile(fileName);
        }

    }
}
