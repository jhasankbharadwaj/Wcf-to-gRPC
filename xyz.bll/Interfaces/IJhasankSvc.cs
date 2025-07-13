using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyz.Data;

namespace xyz.bll.Interfaces
{
    public interface IJhasankSvc
    {
        void hello();
        Person Get(string name);

        Order  SaveOrder(Order order);

        Person Search(string query);

        Order SearOrder(string ordername);

    }
}
