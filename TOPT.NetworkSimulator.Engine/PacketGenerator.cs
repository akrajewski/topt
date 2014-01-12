using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    public class PacketGenerator
    {
        Random rnd = null;
        int numberOfNodes = 0;

        Network network = null;

        public PacketGenerator(Network network)
        {
            rnd = new Random();
            this.numberOfNodes = network.networkNodes.Count;
            this.network = network;
        }

        public Packet GenerateRandomPacket(int sourceId)
        {
            int destinationId = -1;

            do
            {
                destinationId = rnd.Next(0, numberOfNodes); //random number from 0 to numberOfNodes-1
            } while (destinationId == sourceId);

            network.packetsInNetwork++;

            return new Packet(sourceId, destinationId);
        }
    }
}
