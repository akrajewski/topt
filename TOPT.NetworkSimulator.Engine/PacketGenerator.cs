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
        int randomIdTopBoundry = 0;

        public PacketGenerator(int numberOfNodes)
        {
            rnd = new Random();
            this.randomIdTopBoundry = numberOfNodes + 1;
        }

        public Packet GenerateRandomPacket(int sourceId)
        {
            int destinationId = -1;

            do
            {
                destinationId = rnd.Next(0, randomIdTopBoundry);
            } while (destinationId == sourceId);

            return new Packet(sourceId, destinationId);
        }
    }
}
