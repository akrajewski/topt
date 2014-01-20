using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    public class Link : ISimulationObject, IReceivable
    {
        private static int linkIdGenerator = 0;
        public static void ResetLinkIdGenerator()
        {
            linkIdGenerator = 0;
        }

        private int linkId = 0;

        Packet lastStepPacket = null;
        Packet currentStepPacket = null;

        public Node linkDestinationNode { get; set; }

        public Link()
        {
            this.linkId = linkIdGenerator++; //generating new Id
        }

        public void ReceivePacket(Packet packet)
        {
            //after last simulation step currentStepPacket should be null
            if (currentStepPacket == null)
            {
                currentStepPacket = packet;
            }
            else
            {
                throw new SystemException(
                    "CurrentStepPacket is not null in link destined for "
                    + linkDestinationNode.Id);
            }
        }

        public void PerformSimulationStep()
        {
            if (lastStepPacket != null) 
            {
                lastStepPacket.IncreaseLatencyCounter(); //currentStepPacket has already had its latency increased
                lastStepPacket.AddToTraceroute(linkId, false);

                linkDestinationNode.ReceivePacket(lastStepPacket);
            }

            lastStepPacket = currentStepPacket;

            currentStepPacket = null;
        }

        public void SetDestinationNode(Node node)
        {
            this.linkDestinationNode = node;
        }

        public override string ToString()
        {
            return string.Format("[LINK " + linkId + ": destined for node {0}]", linkDestinationNode.Id);
        }
    }
}
