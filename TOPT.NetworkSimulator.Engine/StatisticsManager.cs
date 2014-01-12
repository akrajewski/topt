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

        public void AddPacketToStatistics(Packet packet, PacketState state) 
        {
            packetList.Add(new PacketListElement(packet, state));
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
            return sb.ToString();
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
        DELIVERED_SUCCESSFULLY,
        DROPPED
    }
}
