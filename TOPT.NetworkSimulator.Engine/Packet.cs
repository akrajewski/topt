using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    class Packet
    {
        public int destinationId;

        String traceroute = "";

        public void AddToTraceroute(int id, bool isNode)
        {
            if (isNode) //thats a node
            {
                traceroute += "n" + id + ";";
            }
            else //thats a link
            {
                traceroute += "l" + id + ";";
            }
        }
    }
}
