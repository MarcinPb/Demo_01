using Database.DataRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GeometryReader.Test
{
    [TestClass]
    public class ImportDataTest
    {
        private readonly string _sqliteFile = @"K:\temp\sandbox\Nowy model testowy\testOPC.wtg.sqlite";

        [TestMethod]
        public void ReadBaseTest()
        {
            //new Importer().ImportBase(_sqliteFile);

            var importer = new Importer();
            var importedBaseOutputLists = importer.ImportBase(_sqliteFile);
            ImportRepo.InsertToInfraObjType(importedBaseOutputLists.InfraObjTypeList);
            ImportRepo.InsertToInfraField(importedBaseOutputLists.ImportedFieldList);
        }



        [TestMethod]
        public void ReadDataTest()
        {
            var importer = new Importer();
            importer.ProgressChanged += OnProgressChanged;
            importer.ImportData(_sqliteFile);
        }

        private void OnProgressChanged(object sender, ProgressEventArgs e)
        {
            var ratio = e.ProgressRatio;
            var message = e.Message;
        }
    }
}
