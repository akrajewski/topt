using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    public class Scheduler
    {
        Network network = null;


        public Scheduler(Network network)
        {
            this.network = network;
        }

        public void PerformSimulation(/*int simulationPeriod*/)
        {
          
            //for (currentSimulationTime = 0; currentSimulationTime < simulationPeriod; ++currentSimulationTime)
            //{

            do
            {
                foreach (TrafficGenerator tg in network.trafficGenerators)
                {
                    tg.PerformSimulationStep();
                }

                foreach (Node n in network.networkNodes)
                {
                    n.PerformSimulationStep();
                }
                
                foreach (Link l in network.networkLinks)
                {
                    l.PerformSimulationStep();
                }

          

                
            } while (network.packetsInNetwork != 0);
        }
    }
}
