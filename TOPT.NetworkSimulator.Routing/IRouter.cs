using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing
{
    public interface IRouter
    {
        RoutingTable RoutingTable { get; set; }
        int Id { get; }
    }
}
