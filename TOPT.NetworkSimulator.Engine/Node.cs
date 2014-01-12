using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPT.NetworkSimulator.Routing;

namespace TOPT.NetworkSimulator.Engine
{
    class Node : ISimulationObject, IReceivable, IRouter
    {
        private static int nodeIdGenerator = 0;

        public int Id { get; private set; }

        public Link ingressHorizontalLink { get; set; }
        public Link ingressVerticalLink { get; set; }

        public Link egressHorizontalLink { get; set; }
        public Link egressVerticalLink { get; set; }

        NodePort localPort = null;

        NodeQueue queue = null;

        public Node()
        {
            this.Id = nodeIdGenerator++; //generating new Id
            this.RoutingTable = new RoutingTable();
        }

        public void ReceivePacket(Packet packet)
        {
            //receive a packet on one of the ingress vertical links or on a local port
            //put it into the queue
            queue.AddToQueue(packet);
        }

        public void PerformSimulationStep()
        {
            //take last packet from queue
            Packet packet = queue.Dequeue();

            //check if packet's destination id is equal to this node's id
            //if yes hand it over to the local port
         

            //map it on forwarding table
            //send it using ReceivePacket method on a proper egress link
            this.OutgoingLink(packet.destinationId, packet.sourceId).ReceivePacket(packet);

            throw new NotImplementedException();
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
