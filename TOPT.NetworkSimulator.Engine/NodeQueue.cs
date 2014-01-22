using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    public class NodeQueue
    {
        public static StatisticsManager statistics { get; set; }
        public static int size { get; set; } //NEED TO BE SET GLOBALY FOR ALL NODEQUEUES

        private List<Packet> queue = null;

        public NodeQueue()
        {
            queue = new List<Packet>();
        }

        private void Enqueue(Packet packet)
        {
            queue.Add(packet);
        }

        private Packet Dequeue()
        {
            return RemovePacketAt(0);
        }

        public Packet AddToQueue (Packet packet)
        {
            if (queue.Count < size)
            {
                Enqueue(packet);
                return null;
            }
            else {
                //REPORT PACKET DROPPED_ON_QUEUE ON QUEUE
                return packet;
            }
        }

        public Packet GetPacketFromQueue()
        {
            if (queue.Count != 0)
            {
                return Dequeue();
            }
            else
            {
                return null;
            }
        }

        public void IncreasePacketsLatency()
        {
            foreach (Packet p in queue)
            {
                p.IncreaseLatencyCounter();
            }
        }

        public void DropOutdatedPackets()
        {
            Packet p = null;
            Packet outdated = null;

            for (int i = 0; i < queue.Count; ++i)
            {
                p = queue.ElementAt(i);
                if (p.IsOutdated())
                {
                    outdated = RemovePacketAt(i--);
                    statistics.AddPacketToStatistics(outdated, PacketState.DROPPED_OUTDATED);
                }
            }
        }

        private Packet RemovePacketAt(int index)
        {
            Packet result = queue.ElementAt(index);
            queue.RemoveAt(index);

            return result;
        }

        public bool IsEmpty()
        {
            return queue.Count == 0;
        }
        
    }
}
