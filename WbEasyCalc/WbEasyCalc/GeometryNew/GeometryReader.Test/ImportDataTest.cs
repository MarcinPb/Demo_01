using Database.DataModel.Infra;
using Database.DataRepository.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeometryReader.Test
{
    [TestClass]
    public class ImportDataTest
    {
        //private readonly string _sqliteFile = 
        //@"K:\temp\sandbox\Nowy model testowy\testOPC.wtg.sqlite";
        //@"K:\temp\sandbox\Nowy model testowy\nowy.wtg.sqlite";

        [TestMethod]
        public void ImportInfraConstantDataTest()
        {
            //new Importer().ImportBase(_sqliteFile);
            var sqliteFile = GetSqliteFile();
            var importer = new Importer();
            var importedBaseOutputLists = importer.ImportBase(sqliteFile);
            InfraRepo.InsertToInfraObjType(importedBaseOutputLists.InfraObjTypeList);
            InfraRepo.InsertToInfraField(importedBaseOutputLists.ImportedFieldList);
        }



        [TestMethod]
        public void ImportInfraChangableDataTest()
        {
            var sqliteFile = GetSqliteFile();
            InfraConstantDataLists importedDataInputLists = InfraRepo.GetInfraConstantData();

            var importer = new Importer();
            importer.OuterProgressChanged += OnProgressChanged;
            InfraChangeableDataLists importedDataOutputLists = importer.ImportData(sqliteFile, importedDataInputLists);

            InfraRepo.InsertToInfraZone(importedDataOutputLists.ZoneDict);
            InfraRepo.InsertToInfraDemandPattern(importedDataOutputLists.DemandPatternDict);
            InfraRepo.InsertToInfraDemandPatternCurve(importedDataOutputLists.DemandPatternCurveList);

            InfraRepo.InsertToInfraObj(importedDataOutputLists.InfraObjList);
            InfraRepo.InsertToInfraValue(importedDataOutputLists.InfraValueList);
            InfraRepo.InsertToInfraGeometry(importedDataOutputLists.InfraGeometryList);
            InfraRepo.InsertToInfraDemandBase(importedDataOutputLists.DemandBaseList);
        }

        private void OnProgressChanged(object sender, ProgressEventArgs e)
        {
            var ratio = e.ProgressRatio;
            var message = e.Message;
        }

        private string GetSqliteFile()
        {
            return System.Configuration.ConfigurationManager.AppSettings["SqliteFile"]; ;
        }
    }
}
