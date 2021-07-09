using Database.DataRepository.Infra;
using ExcelNpoi.ExcelNpoi.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ExcelNpoi.ExcelNpoi.Test
{
    [TestClass]
    public class PostCalcTest
    {
        private string _excelFile = @"K:\temp\PostCalcExcel\GeneratedSettings.xlsx";

        [TestMethod]
        public void CreateExcel()
        {
            InfraData infraData = InfraRepo.GetInfraData();


            PostCalcExcelWriter.Write(
                _excelFile,
                infraData.InfraChangeableData,
                infraData.InfraSpecialFieldId
                );
        }
    }
}
