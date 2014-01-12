using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    public class Packet
    {
        public int destinationId { get; set; }
        public int sourceId { get; set; }

        int hops = 0;
        int latency = 0;

        String traceroute = "";

        public Packet(int sourceId, int destinationId)
        {
            this.sourceId = sourceId;
            this.destinationId = destinationId;
        }

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

        public void IncreaseHopCounter()
        {
            hops++;
        }

        public void IncreaseLatencyCounter()
        {
            latency++;
        }
    }
}
