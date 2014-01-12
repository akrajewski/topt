using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    class TrafficGenerator : ISimulationObject
    {
        NodePort localNodePort = null;

        int startTime_global = 0;
        int endTime_global = 0;

        int generationRate = 1; 
            //specifies how many time units should generator wait between generating a single packet

        int timeUnitilNextGeneration = 0;


        public void PerformSimulationStep()
        {
            if (timeUnitilNextGeneration == 0)
            {
                Packet packet = null; //TO DO: GENERATE A RANDOM PACKET
                localNodePort.ReceivePacket(packet); 
                timeUnitilNextGeneration = generationRate;
            }
            else
            {
                timeUnitilNextGeneration--;
            }
        }
    }
}
