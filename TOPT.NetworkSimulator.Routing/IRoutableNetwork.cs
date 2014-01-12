using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPT.NetworkSimulator.Routing.Algorithms;

namespace TOPT.NetworkSimulator.Routing
{
    public interface IRoutableNetwork
    {
        List<IRouter> Routers { get; }
        DirectedGraph ToGraph();
    }
}
