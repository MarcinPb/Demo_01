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
        public void ImportInfraBaseTest()
        {
            //new Importer().ImportBase(_sqliteFile);

            var importer = new Importer();
            var importedBaseOutputLists = importer.ImportBase(_sqliteFile);
            InfraRepo.InsertToInfraObjType(importedBaseOutputLists.InfraObjTypeList);
            InfraRepo.InsertToInfraField(importedBaseOutputLists.ImportedFieldList);
        }



        [TestMethod]
        public void ImportInfraDataTest()
        {
            //InfraConstantDataLists importedDataInputLists = new InfraConstantDataLists()
            //{
            //    InfraObjTypeList = InfraRepo.GetObjTypeList().ToList(),
            //    InfraObjTypeFieldList = InfraRepo.GetObjTypeFieldList().ToList(),
            //    InfraFieldList = InfraRepo.GetFieldList(),
            //};
            InfraConstantDataLists importedDataInputLists = InfraRepo.GetInfraConstantData();

            var importer = new Importer();
            importer.ProgressChanged += OnProgressChanged;
            InfraChangeableDataLists importedDataOutputLists = importer.ImportData(_sqliteFile, importedDataInputLists);

            InfraRepo.InsertToInfraZone(importedDataOutputLists.ZoneDict);
            InfraRepo.InsertToInfraDemandPattern(importedDataOutputLists.DemandPatternDict);
            InfraRepo.InsertToInfraObj(importedDataOutputLists.InfraObjList);
            InfraRepo.InsertToInfraValue(importedDataOutputLists.InfraValueList);
            InfraRepo.InsertToInfraGeometry(importedDataOutputLists.InfraGeometryList);
        }

        private void OnProgressChanged(object sender, ProgressEventArgs e)
        {
            var ratio = e.ProgressRatio;
            var message = e.Message;
        }
    }
}
