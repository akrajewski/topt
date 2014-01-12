using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    class Node : ISimulationObject, IReceivable
    {
        private static int nodeIdGenerator = 0;

        public int nodeId { get; private set; }

        public Link ingressHorizontalLink { get; set; }
        public Link ingressVerticalLink { get; set; }

        public Link egressHorizontalLink { get; set; }
        public Link egressVerticalLink { get; set; }

        NodePort localPort = null;

        NodeQueue queue = null;

        public Node()
        {
            this.nodeId = ++nodeIdGenerator; //generating new Id
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
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "[NODE " + nodeId + ":\n\thorizontal links " +
                "\n\t\tingress " + ingressHorizontalLink +
                "\n\t\tegress " + egressHorizontalLink +
                "\n\tvertical links" +
                "\n\t\tingress " + ingressVerticalLink +
                "\n\t\tegress " + egressVerticalLink + "\n]";
        }
    }
}
