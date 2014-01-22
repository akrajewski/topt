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

        public static void ResetNodeIdGenerator()
        {
            nodeIdGenerator = 0;
        }

        public int Id { get; private set; }

        public Link ingressHorizontalLink { get; set; }
        public Link ingressVerticalLink { get; set; }

        public Link egressHorizontalLink 
        {
            get { return ehz; }
            set
            {
                ehz = value;
                queues.Add(ehz, new NodeQueue());
            }
        }
        private Link ehz;


        public Link egressVerticalLink 
        {
            get { return evl; }
            set
            {
                evl = value;
                queues.Add(evl, new NodeQueue());
            }
        }
        private Link evl;

        public NodePort localPort { get; set; }


        private List<Packet> receivedPackets = new List<Packet>();
        private Dictionary<Link, NodeQueue> queues = new Dictionary<Link, NodeQueue>();

        NodeQueue queue = new NodeQueue();

        public Node()
        {
            this.Id = nodeIdGenerator++; //generating new Id
            this.RoutingTable = new RoutingTable();

        }

        public void ReceivePacket(Packet packet)
        {
            if (packet.sourceId != Id) //increase hop counter unless element originates from this node
            {
                packet.IncreaseHopCounter();
            }

            packet.AddToTraceroute(Id, true);

            //if (packet.IsOutdated())
            //{
            //    statistics.AddPacketToStatistics(packet, PacketState.DROPPED_OUTDATED);
            //    return;
            //}

            //receive a element on one of the ingress vertical links or on a local port
            //put it into the queue

            receivedPackets.Add(packet);
        }

        private Action<Packet, NodeQueue> addToQueue = (p, q) =>
            {
                if (q.AddToQueue(p) != null)
                {
                    statistics.AddPacketToStatistics(p, PacketState.DROPPED_ON_QUEUE);
                }
            };


        public void HandleTransmissionVia(Link link)
        {
            var queue = queues[link];

            var packetsVia = receivedPackets.Where(p => OutgoingLink(p.sourceId, p.destinationId) == link).ToList();
            var fromQueue = queue.GetPacketFromQueue();
            if (fromQueue != null)
            {
                link.ReceivePacket(fromQueue);
                packetsVia.ForEach(p => addToQueue(p, queue));
            }
            else
            {
                if (packetsVia.Count > 0)
                {
                    var first = packetsVia.First();
                    link.ReceivePacket(first);
                    packetsVia.Remove(first);
                    packetsVia.ForEach(p => addToQueue(p, queue));
                }
            }
        }

        public void PerformSimulationStep()
        {
            if (receivedPackets.Count > 3)
            {
                throw new InvalidOperationException("Ingrss packets - more than 3. Weird.");
            }

            
            var localPackets = receivedPackets.Where(p => p.destinationId == this.Id).ToList();
            localPackets.ForEach(lp => { this.localPort.ReceivePacket(lp); receivedPackets.Remove(lp); });

            HandleTransmissionVia(egressHorizontalLink);
            HandleTransmissionVia(egressVerticalLink);

            receivedPackets.Clear();

            foreach (var queue in queues.Values)
            {
                queue.IncreasePacketsLatency();
                queue.DropOutdatedPackets();
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
            if (this.RoutingTable.Contains(from, to))
            {
                var nextHop = this.RoutingTable.NextHop(from, to);
                return (egressHorizontalLink.linkDestinationNode.Id == nextHop) ? egressHorizontalLink : egressVerticalLink;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        
    }
}
