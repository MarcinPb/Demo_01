using System.Collections.Generic;

namespace Database.DataRepository
{
    public interface IBaseListRepository<T>
    {
        List<T> GetList();

        //T GetItem(int id);
        //T SaveItem(T model);
        //bool DeleteItem(int id);
    }
}
