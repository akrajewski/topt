using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    public class TrafficGenerator : ISimulationObject
    {
        public static PacketGenerator packetGenerator { get; set; }
        public static int numberOfPacketsToGenerate { get; set; }

        NodePort localNodePort = null;

        int nodeId = 0;

        //int startTime_global = 0;
        //int endTime_global = 0;

        int generationLatency = 0;
            //specifies how many time units should generator wait between generating a single packet

        int timeUnitilNextGeneration = 0;

        int packetsStillToGenerate = 0;

        public TrafficGenerator(NodePort localNodePort, Node node, double generationRate)
        {
            this.localNodePort = localNodePort;
            this.nodeId = node.Id;

            generationLatency = (int)((1.0 / generationRate) - 1.0);
            packetsStillToGenerate = numberOfPacketsToGenerate;
        }

        public void PerformSimulationStep()
        {
            if (timeUnitilNextGeneration == 0)
            {
                if (packetsStillToGenerate-- > 0)
                {
                    Packet packet = packetGenerator.GenerateRandomPacket(nodeId);
                    localNodePort.ReceivePacket(packet);
                    timeUnitilNextGeneration = generationLatency;
                }
            }
            else
            {
                timeUnitilNextGeneration--;
            }
        }
    }
}
