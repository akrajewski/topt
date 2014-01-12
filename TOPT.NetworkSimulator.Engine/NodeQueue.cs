using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    class NodeQueue : Queue<Packet>
    {
        public static int size { get; set; } //NEED TO BE SET GLOBALY FOR ALL NODEQUEUES

        public void AddToQueue (Packet packet)
        {
            if (this.Count < size) {
                base.Enqueue(packet);
            }
            else {
                //REPORT PACKET DROPPED ON QUEUE
            }
        }
    }
}
