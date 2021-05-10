using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataRepository.WaterConsumption
{
    public class ListRepositoryTemp : IListRepository
    {
        private List<Database.DataModel.WaterConsumption> _list;

        public List<Database.DataModel.WaterConsumption> GetList()
        {
            return _list;
        }

        public Database.DataModel.WaterConsumption GetItem(int id)
        {
            if (id != 0)
            {
                var customer = _list.Single(f => f.WaterConsumptionId == id);
                return (Database.DataModel.WaterConsumption)customer?.Clone();
            }
            else
            {
                return new Database.DataModel.WaterConsumption();
            }
        }

        public Database.DataModel.WaterConsumption SaveItem(Database.DataModel.WaterConsumption model)
        {
            if (model.WaterConsumptionId == 0)
            {
                var id = _list.Any() ? _list.Max(x => x.WaterConsumptionId) + 1 : 1;
                model.WaterConsumptionId = id;
                _list.Add(model);
            }
            else
            {
                var ind = _list.FindIndex(x => x.WaterConsumptionId == model.WaterConsumptionId);
                _list[ind] = model;
            }
            return model;
        }

        public bool DeleteItem(int id)
        {
            var modelTemp = _list.FirstOrDefault(x => x.WaterConsumptionId == id);
            _list.Remove(modelTemp);

            return true;
        }

        public bool DeleteItem(List<int> idList)
        {
            throw new NotImplementedException();
        }

        public int Clone(int id)
        {
            throw new NotImplementedException();
        }

        public ListRepositoryTemp(List<Database.DataModel.WaterConsumption> list)
        {
            _list = list;
        }
    }
}
