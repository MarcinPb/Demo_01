using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grundfos.WG.Model;
using Grundfos.WaterDemandCalculation.Tests.TestData;
using Grundfos.WG.PostCalc;
using Haestad.Support.OOP.FileSystem;
using Haestad.Support.OOP.Logging;
using NUnit.Framework;

namespace Grundfos.WaterDemandCalculation.Tests
{
    [TestFixture]
    public class ZoneDemandDataListCreatorNewTest
    {
        private string _testedZoneName = "1 - Przybków";
        private string _logFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"ZoneDemandDataListCreatorTest.log");
        private string dateFormat = "yyyy-MM-dd_HH-mm-ss_fffffff";
        //private string _conStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=WG;Integrated Security=True";
        private string _conStr = @"Server=.;Database=WaterInfra;User Id=sa;Password=Gfosln123.;";
        private DateTime calcTime = new DateTime(2021, 07, 23, 13, 20, 0);

        [TestCase("2019-09-10 12:24:30", "2019-09-10 12:20:00")]
        public void Create_Tests(DateTime input, DateTime output)
        {
            var logger = new ActionLogger();
            logger.InitializeLogger(new FileLogger(new FilePath(_logFolder), 10000), OutputLevel.Info);

            ZoneDemandDataListCreatorNew.DataContext dataContext = new ZoneDemandDataListCreatorNew.DataContext()
            {
                WaterInfraConnString = _conStr
            };

            ZoneDemandDataListCreatorNew zoneDemandDataListCreatorNew = new ZoneDemandDataListCreatorNew(dataContext, logger);
            List<ZoneDemandData> zoneDemandDataList = zoneDemandDataListCreatorNew.Create(calcTime);

            Helper.DumpToFile(zoneDemandDataList.FirstOrDefault(x => x.ZoneName == "1 - Przybków"), Path.Combine(TestContext.CurrentContext.TestDirectory, $"Dump_{DateTime.Now.ToString(dateFormat)}_ZoneDemandData.xml"));
            Helper.DumpToFile(zoneDemandDataList.FirstOrDefault(x => x.ZoneName == "6 - ZPW"), Path.Combine(TestContext.CurrentContext.TestDirectory, $"Dump_{DateTime.Now.ToString(dateFormat)}_ZoneDemandData.xml"));
            Helper.DumpToFile(zoneDemandDataList.FirstOrDefault(x => x.ZoneName == "7 - Tranzyt"), Path.Combine(TestContext.CurrentContext.TestDirectory, $"Dump_{DateTime.Now.ToString(dateFormat)}_ZoneDemandData.xml"));

            //string ratioFormula = "IIF(DemandWg-DemandExcluded<0.000001, 0, (DemandScada-DemandExcluded)/(DemandWg-DemandExcluded))";
            //zoneDemandDataListCreator.SaveToDatabase(zoneDemandDataList, _conStr, ratioFormula);
            //zoneDemandDataListCreator.UpdateAndLoadFromDatabase(zoneDemandDataList, _conStr);

            Helper.DumpToFile(zoneDemandDataList.FirstOrDefault(x => x.ZoneName == _testedZoneName), Path.Combine(TestContext.CurrentContext.TestDirectory, $"Dump_{DateTime.Now.ToString(dateFormat)}_ZoneDemandData.xml"));
        }

        [TestCase("2019-09-10 12:24:30", "2019-09-10 12:20:00")]
        public void GetExcludedObjectIdTests(DateTime input, DateTime output)
        {
            var logger = new ActionLogger();
            logger.InitializeLogger(new FileLogger(new FilePath(_logFolder), 10000), OutputLevel.Info);

            ZoneDemandDataListCreatorNew.DataContext dataContext = new ZoneDemandDataListCreatorNew.DataContext()
            {
                WaterInfraConnString = _conStr
            };

            ZoneDemandDataListCreatorNew zoneDemandDataListCreatorNew = new ZoneDemandDataListCreatorNew(dataContext, logger);
            List<ZoneDemandData> zoneDemandDataList = zoneDemandDataListCreatorNew.Create(calcTime);


            var excludedObjectIdList = zoneDemandDataListCreatorNew.GetExcludedObjectId(zoneDemandDataList);
            var excludedDemandPatternIdList = zoneDemandDataListCreatorNew.GetExcludedDemandPatternId(zoneDemandDataList);
        }
    }
}
