using Database.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DataRepository
{
    public interface IDesignerRepo
    {
        DesignerObj GetItem(int id);
        List<DesignerObj> GetJunctionList();
        List<DesignerObj> GetPipeList();
        List<DesignerObj> GetCustomerNodeList();
        Point2D GetPointBottomRight();
        Point2D GetPointTopLeft();
    }
}
