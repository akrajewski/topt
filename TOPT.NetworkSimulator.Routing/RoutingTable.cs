using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing
{
    public class RoutingTable
    {
        private Dictionary<int, int> entries = new Dictionary<int, int>();

        public void AddEntry(int to, int outputPort)
        {
            entries.Add(to, outputPort);
        }

        public int OutputPort(int to)
        {
            return entries[to];
        }
    }
}
