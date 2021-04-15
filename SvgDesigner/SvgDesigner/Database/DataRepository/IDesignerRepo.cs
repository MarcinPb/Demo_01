using GeometryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataRepository
{
    public interface IDesignerRepo
    {
        DomainObjectData GetItem(int id);
        List<DomainObjectData> GetJunctionList();
        List<DomainObjectData> GetPipeList();
        List<DomainObjectData> GetCustomerNodeList();

    }
}
