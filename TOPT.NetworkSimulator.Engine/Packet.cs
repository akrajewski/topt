using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    public class Packet
    {
        private static int ttlDefault = 0;
        public static void SetTimeToLive (int ttl)
        {
            ttlDefault = ttl;
        }

        public int destinationId { get; set; }
        public int sourceId { get; set; }

        public int hops { get; set; }
        public int latency { get; set; }

        public int ttl { get; set; }

        String traceroute = "";

        public Packet(int sourceId, int destinationId)
        {
            this.sourceId = sourceId;
            this.destinationId = destinationId;

            ttl = ttlDefault;
        }

        public override string ToString()
        {
            return "[PACKET " + sourceId + " -> " + destinationId + " trace: " + traceroute + " hops: " + hops + " latency: " + latency + "]";
        }

        public void AddToTraceroute(int id, bool isNode)
        {
            if (isNode) //thats a node
            {
                traceroute += "N" + id + ">";
            }
            else //thats a link
            {
                traceroute += "L" + id + ">";
            }
        }

        public void IncreaseHopCounter()
        {
            hops++;
        }

        public void IncreaseLatencyCounter()
        {
            latency++;
            ttl--;
        }

        public bool IsOutdated()
        {
            if (ttl <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
