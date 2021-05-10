using System.Collections.Generic;

namespace Database.DataRepository
{
    public interface IBaseItemRepository<T>
    {
        T GetItem(int id);
        T SaveItem(T model);
        bool DeleteItem(int id);
    }
}
