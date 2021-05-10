using Database.DataRepository;

namespace Database.DataRepository.WbEasyCalcData
{
    public interface IListRepository : IMultiDeleteListRepository<Database.DataModel.WbEasyCalcData>
    {
        int Clone(int id);
        int CreateAll();
    }
}
