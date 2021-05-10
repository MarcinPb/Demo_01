using Database.DataRepository;
using System.Collections.Generic;

namespace Database.DataRepository
{
    public interface IMultiDeleteListRepository<T> : IBaseItemRepository<T>, IBaseListRepository<T>
    {
        bool DeleteItem(List<int> idList);
    }
}
