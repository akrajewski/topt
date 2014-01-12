using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    public class NodePort : IReceivable
    {
        public static StatisticsManager statistics { get; set; }
        Node node = null;

        public NodePort(Node node)
        {
            this.node = node;
        }

        public void ReceivePacket(Packet packet)
        {
            //if the destinationId of the packet is equal to id of the node of the nodePort,
            if (packet.destinationId == node.Id)
            {
                statistics.AddPacketToStatistics(packet, PacketState.DELIVERED_SUCCESSFULLY);
            }
            else
            {
                //otherwise packet should be handed over to the node
                node.ReceivePacket(packet);
            }
        }
    }
}
