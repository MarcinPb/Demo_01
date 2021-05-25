using Database.DataModel;
using Database.DataRepository;
using System;
using System.Configuration;

namespace GlobalRepository
{
    public static class GlobalConfig
    {
        // ex: GlobalConfig.Opc.RunOpcPublish(zoneRomanNo, easyCalcDataInput, easyCalcDataOutput); 
        public static IOpcServer OpcServer { get; set; }

        public static IWbEasyCalcExcel WbEasyCalcExcel { get; set; }

        public static IMainRepo DataRepository { get; private set; }


        public static void InitializeConnection(DatabaseType db)
        {
            OpcServer = new OpcServer(ConfigurationManager.AppSettings["opcAddress"]);
            WbEasyCalcExcel = new WbEasyCalcExcel(ConfigurationManager.AppSettings["ExcelTemplateFileName"]);

            if (db == DatabaseType.Sql)
            {
                DataRepository = new MainRepo(CnnString("WaterUtility_ConnStr"));
            }
            else if (db == DatabaseType.TextFile)
            {
                // todo - Set up the text Connector properly
                //TextConnector text = new TextConnector();
                DataRepository = null;
            }
        }

        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
