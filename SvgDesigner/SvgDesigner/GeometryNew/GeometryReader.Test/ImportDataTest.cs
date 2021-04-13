using Database.DataModel;
using Database.DataRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

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
            ImportedDataInputLists importedDataInputLists = new ImportedDataInputLists()
            {
                InfraObjTypeList = ImportRepo.GetObjTypeList().ToList(),
                InfraObjTypeFieldList = ImportRepo.GetObjTypeFieldList().ToList(),
                InfraFieldList = ImportRepo.GetFieldList(),
            };

            var importer = new Importer();
            importer.ProgressChanged += OnProgressChanged;
            ImportedDataOutputLists importedDataOutputLists = importer.ImportData(_sqliteFile, importedDataInputLists);

            ImportRepo.InsertToInfraZone(importedDataOutputLists.ZoneDict);
            ImportRepo.InsertToInfraObj(importedDataOutputLists.InfraObjList);
            ImportRepo.InsertToInfraValue(importedDataOutputLists.InfraValueList);
            ImportRepo.InsertToInfraGeometry(importedDataOutputLists.InfraGeometryList);
        }

        private void OnProgressChanged(object sender, ProgressEventArgs e)
        {
            var ratio = e.ProgressRatio;
            var message = e.Message;
        }
    }
}
