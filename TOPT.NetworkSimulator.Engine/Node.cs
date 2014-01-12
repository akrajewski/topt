using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPT.NetworkSimulator.Routing;

namespace TOPT.NetworkSimulator.Engine
{
    public class Node : ISimulationObject, IReceivable, IRouter
    {
        public static StatisticsManager statistics { get; set; }
        private static int nodeIdGenerator = 0;

        public int Id { get; private set; }

        public Link ingressHorizontalLink { get; set; }
        public Link ingressVerticalLink { get; set; }

        public Link egressHorizontalLink { get; set; }
        public Link egressVerticalLink { get; set; }

        public NodePort localPort { get; set; }

        NodeQueue queue = null;

        public Node()
        {
            this.Id = nodeIdGenerator++; //generating new Id
            this.RoutingTable = new RoutingTable();
        }

        public void ReceivePacket(Packet packet)
        {
            if (packet.sourceId != Id) //increase hop counter unless packet originates from this node
            {
                packet.IncreaseHopCounter();
            }

            //receive a packet on one of the ingress vertical links or on a local port
            //put it into the queue
            packet = queue.AddToQueue(packet);
            if (packet != null)
            {
                statistics.AddPacketToStatistics(packet, PacketState.DROPPED);
                    //packet was dropped in queue 
            }
        }

        public void PerformSimulationStep()
        {
            queue.IncreasePacketsLatency();

            //take last packet from queue
            Packet packet = queue.Dequeue();

            if (packet != null)
            {
                //check if packet's destination id is equal to this node's id
                if (packet.destinationId == Id) //if yes hand it over to the local port
                {
                    localPort.ReceivePacket(packet);
                }
                else //if not
                {
                    //map it on forwarding table &
                    //send it using ReceivePacket method on a proper egress link
                    this.OutgoingLink(packet.destinationId, packet.sourceId).ReceivePacket(packet);
                }
            }
        }

        public override string ToString()
        {
            return "[NODE " + Id + ":\n\thorizontal links " +
                "\n\t\tingress " + ingressHorizontalLink +
                "\n\t\tegress " + egressHorizontalLink +
                "\n\tvertical links" +
                "\n\t\tingress " + ingressVerticalLink +
                "\n\t\tegress " + egressVerticalLink + 
                "\n\troutingTable\n" + RoutingTable + "\n]";
        }

        public RoutingTable RoutingTable { get; set; }

        private Link OutgoingLink(int from, int to)
        {
            var nextHop = this.RoutingTable.NextHop(from, to);
            return (egressHorizontalLink.linkDestinationNode.Id == nextHop) ? egressHorizontalLink : egressVerticalLink;
        }

    }
}
