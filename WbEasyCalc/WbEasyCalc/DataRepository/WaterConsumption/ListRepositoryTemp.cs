using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.WaterConsumption
{
    public class ListRepositoryTemp : IListRepository
    {
        private List<DataModel.WaterConsumption> _list;

        public List<DataModel.WaterConsumption> GetList()
        {
            return _list;
        }

        public DataModel.WaterConsumption GetItem(int id)
        {
            if (id != 0)
            {
                var customer = _list.Single(f => f.WaterConsumptionId == id);
                return (DataModel.WaterConsumption)customer?.Clone();
            }
            else
            {
                return new DataModel.WaterConsumption();
            }
        }

        public DataModel.WaterConsumption SaveItem(DataModel.WaterConsumption model)
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

        public ListRepositoryTemp(List<DataModel.WaterConsumption> list)
        {
            _list = list;
        }
    }
}
