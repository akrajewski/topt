using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    public class StatisticsManager
    {
        List<PacketListElement> packetList = new List<PacketListElement>();
        Network network = null;

        public StatisticsManager(Network network)
        {
            this.network = network;
        }

        public void AddPacketToStatistics(Packet packet, PacketState state) 
        {
            packetList.Add(new PacketListElement(packet, state));
            network.packetsInNetwork--;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (PacketListElement p in packetList)
            {
                sb.Append(p.packet.ToString());
                sb.Append(" ");
                sb.AppendLine(Enum.GetName(typeof(PacketState), p.state));
            }

            sb.AppendLine(GetStatistics());

            return sb.ToString();
        }

        public String GetStatistics()
        {
            double avg_hops = 0;
            double avg_latency = 0;
            int deliveredCounter = 0;
            int droppedCounter = 0;

            foreach (PacketListElement p in packetList)
            {
                if (p.state == PacketState.DELIVERED)
                {
                    deliveredCounter++;

                    avg_hops += p.packet.hops;
                    avg_latency += p.packet.latency;
                }
                else
                {
                    droppedCounter++;
                }
            }
            avg_latency /= (double)deliveredCounter;
            avg_hops /= (double)deliveredCounter;

            return "Dropped packets: " + droppedCounter + "\nDelivered packets: " + deliveredCounter + " with avg_hops=" + avg_hops + " and avg_latency=" + avg_latency;
        }
    }

    public class PacketListElement
    {
        public Packet packet { get; set; }
        public PacketState state { get; set; }

        public PacketListElement(Packet packet, PacketState state)
        {
            this.packet = packet;
            this.state = state;
        }
    }

    

    public enum PacketState //nie mam pomyslu na lepsza nazwe
    {
        DELIVERED,
        DROPPED
    }
}
