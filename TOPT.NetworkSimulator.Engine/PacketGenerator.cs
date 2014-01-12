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

        public PacketGenerator(int numberOfNodes)
        {
            rnd = new Random();
            this.numberOfNodes = numberOfNodes;
        }

        public Packet GenerateRandomPacket(int sourceId)
        {
            int destinationId = -1;

            do
            {
                destinationId = rnd.Next(0, numberOfNodes); //random number from 0 to numberOfNodes-1
            } while (destinationId == sourceId);

            return new Packet(sourceId, destinationId);
        }
    }
}
