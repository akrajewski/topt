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

        NodePort localNodePort = null;

        int nodeId = 0;

        int startTime_global = 0;
        int endTime_global = 0;

        int generationLatency = 0;
            //specifies how many time units should generator wait between generating a single packet

        int timeUnitilNextGeneration = 0;

        public TrafficGenerator(NodePort localNodePort, Node node, double generationRate)
        {
            this.localNodePort = localNodePort;
            this.nodeId = node.Id;

            generationLatency = (int)((1.0 / generationRate) - 1.0);

            Console.WriteLine("generationLatency=" + generationLatency + " for rate=" + generationRate); 
                //for debug purposes
        }

        public void PerformSimulationStep()
        {
            if (timeUnitilNextGeneration == 0)
            {
                localNodePort.ReceivePacket(packetGenerator.GenerateRandomPacket(nodeId));
                timeUnitilNextGeneration = generationLatency;
            }
            else
            {
                timeUnitilNextGeneration--;
            }
        }
    }
}
