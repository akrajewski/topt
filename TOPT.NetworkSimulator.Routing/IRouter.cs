using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing
{
    interface IRouter
    {
        RoutingTable RoutingTable { get; set; }
    }
}
