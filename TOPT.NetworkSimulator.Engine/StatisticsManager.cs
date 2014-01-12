using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    class StatisticsManager
    {
        List<PacketListElement> packetList = new List<PacketListElement>();

        public void AddPacketToStatistics(Packet packet, PacketState state) 
        {
            packetList.Add(new PacketListElement(packet, state));
        }
    }

    private class PacketListElement
    {
        public Packet packet { get; set; }
        public PacketState state { get; set; }

        public PacketListElement(Packet packet, PacketState state)
        {
            this.packet = packet;
            this.state = state;
        }
    }

    enum PacketState //nie mam pomyslu na lepsza nazwe
    {
        DELIVERED_SUCCESSFULLY,
        DROPPED
    }
}
