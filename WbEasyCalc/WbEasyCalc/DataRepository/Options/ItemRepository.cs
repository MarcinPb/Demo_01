using Dapper;
using DataModel;
using DataRepository.WbEasyCalcData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Options
{
    public class ItemRepository : IBaseItemRepository<Option>
    {
        public bool DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public Option GetItem(int id)
        {
            using (IDbConnection cnn = new SqlConnection(_cnnString))
            {
                string sql = "dbo.spSettingGet";
                var list = cnn.Query<WbEasyCalcDb>(sql, commandType: CommandType.StoredProcedure).ToList();
                var _list2 = list.Select(x => x.GetWbEasyCalcData()).ToList();
                return new Option()
                {
                    FinancDataModel = _list2[0].EasyCalcModel.FinancDataModel,
                    MatrixOneInModel = _list2[0].EasyCalcModel.MatrixOneIn,
                    MatrixTwoInModel = _list2[0].EasyCalcModel.MatrixTwoIn,
                };
            }
        }

        public Option SaveItem(Option model)
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            using (IDbConnection connection = new SqlConnection(_cnnString))
            {
                var p = new DynamicParameters();

                p.Add("@FinancData_G6", model.FinancDataModel.FinancData_G6);
                p.Add("@FinancData_K6", model.FinancDataModel.FinancData_K6);
                p.Add("@FinancData_G8", model.FinancDataModel.FinancData_G8);

                p.Add("@MatrixOneIn_SelectedOption", model.MatrixOneInModel.SelectedOption);
                p.Add("@MatrixOneIn_C11", model.MatrixOneInModel.C11);
                p.Add("@MatrixOneIn_C12", model.MatrixOneInModel.C12);
                p.Add("@MatrixOneIn_C13", model.MatrixOneInModel.C13);
                p.Add("@MatrixOneIn_C14", model.MatrixOneInModel.C14);
                p.Add("@MatrixOneIn_C21", model.MatrixOneInModel.C21);
                p.Add("@MatrixOneIn_C22", model.MatrixOneInModel.C22);
                p.Add("@MatrixOneIn_C23", model.MatrixOneInModel.C23);
                p.Add("@MatrixOneIn_C24", model.MatrixOneInModel.C24);
                p.Add("@MatrixOneIn_D21", model.MatrixOneInModel.D21);
                p.Add("@MatrixOneIn_D22", model.MatrixOneInModel.D22);
                p.Add("@MatrixOneIn_D23", model.MatrixOneInModel.D23);
                p.Add("@MatrixOneIn_D24", model.MatrixOneInModel.D24);
                p.Add("@MatrixOneIn_E11", model.MatrixOneInModel.E11);
                p.Add("@MatrixOneIn_E12", model.MatrixOneInModel.E12);
                p.Add("@MatrixOneIn_E13", model.MatrixOneInModel.E13);
                p.Add("@MatrixOneIn_E14", model.MatrixOneInModel.E14);
                p.Add("@MatrixOneIn_E21", model.MatrixOneInModel.E21);
                p.Add("@MatrixOneIn_E22", model.MatrixOneInModel.E22);
                p.Add("@MatrixOneIn_E23", model.MatrixOneInModel.E23);
                p.Add("@MatrixOneIn_E24", model.MatrixOneInModel.E24);
                p.Add("@MatrixOneIn_F11", model.MatrixOneInModel.F11);
                p.Add("@MatrixOneIn_F12", model.MatrixOneInModel.F12);
                p.Add("@MatrixOneIn_F13", model.MatrixOneInModel.F13);
                p.Add("@MatrixOneIn_F14", model.MatrixOneInModel.F14);
                p.Add("@MatrixOneIn_F21", model.MatrixOneInModel.F21);
                p.Add("@MatrixOneIn_F22", model.MatrixOneInModel.F22);
                p.Add("@MatrixOneIn_F23", model.MatrixOneInModel.F23);
                p.Add("@MatrixOneIn_F24", model.MatrixOneInModel.F24);
                p.Add("@MatrixOneIn_G11", model.MatrixOneInModel.G11);
                p.Add("@MatrixOneIn_G12", model.MatrixOneInModel.G12);
                p.Add("@MatrixOneIn_G13", model.MatrixOneInModel.G13);
                p.Add("@MatrixOneIn_G14", model.MatrixOneInModel.G14);
                p.Add("@MatrixOneIn_G21", model.MatrixOneInModel.G21);
                p.Add("@MatrixOneIn_G22", model.MatrixOneInModel.G22);
                p.Add("@MatrixOneIn_G23", model.MatrixOneInModel.G23);
                p.Add("@MatrixOneIn_G24", model.MatrixOneInModel.G24);
                p.Add("@MatrixOneIn_H11", model.MatrixOneInModel.H11);
                p.Add("@MatrixOneIn_H12", model.MatrixOneInModel.H12);
                p.Add("@MatrixOneIn_H13", model.MatrixOneInModel.H13);
                p.Add("@MatrixOneIn_H14", model.MatrixOneInModel.H14);
                p.Add("@MatrixOneIn_H21", model.MatrixOneInModel.H21);
                p.Add("@MatrixOneIn_H22", model.MatrixOneInModel.H22);
                p.Add("@MatrixOneIn_H23", model.MatrixOneInModel.H23);
                p.Add("@MatrixOneIn_H24", model.MatrixOneInModel.H24);

                connection.Execute("dbo.spSettingSave", p, commandType: CommandType.StoredProcedure);

                return model;
            }
        }

        private readonly string _cnnString;
        public ItemRepository(string cnnString)
        {
            _cnnString = cnnString;
        }

    }
}
