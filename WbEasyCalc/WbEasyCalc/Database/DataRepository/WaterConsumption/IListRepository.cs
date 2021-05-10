using Database.DataRepository;

namespace Database.DataRepository.WaterConsumption
{
    public interface IListRepository : IMultiDeleteListRepository<Database.DataModel.WaterConsumption>
    {
        int Clone(int id);
    }
}
