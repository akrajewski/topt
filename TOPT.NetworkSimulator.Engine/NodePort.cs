using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    class NodePort : IReceivable
    {
        Node node = null;

        public void ReceivePacket(Packet packet)
        {
            //if the destinationId of the packet is equal to id of the node of the nodePort,
            if (packet.destinationId == node.Id)
            {
                //packet should be handed over to statistics (client).
            }
            else
            {
                //otherwise packet should be handed over to the node
                node.ReceivePacket(packet);
            }

            throw new NotImplementedException();
        }
    }
}
