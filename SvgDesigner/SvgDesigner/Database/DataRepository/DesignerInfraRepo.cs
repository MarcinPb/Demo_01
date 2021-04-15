using GeometryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataRepository
{
    public class DesignerInfraRepo : IDesignerRepo
    {




        public DomainObjectData GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public List<DomainObjectData> GetJunctionList()
        {
            throw new NotImplementedException();
        }
        public List<DomainObjectData> GetPipeList()
        {
            return new List<DomainObjectData>();
        }
        public List<DomainObjectData> GetCustomerNodeList()
        {
            return new List<DomainObjectData>();
        }

        public Point2D GetPointBottomRight()
        {
            throw new NotImplementedException();
        }

        public Point2D GetPointTopLeft()
        {
            throw new NotImplementedException();
        }
    }
}
