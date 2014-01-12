using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing
{
    public class PCE
    {
        private RoutingAlgorithm algorithm;

        public PCE(RoutingAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        public void Compute()
        {
            switch (algorithm)
            {
                case RoutingAlgorithm.DijkstraShortestPath:
                    //Compute with Dijkstra

                    break;
                default:
                    break;
            }
        }

        public enum RoutingAlgorithm
        {
            DijkstraShortestPath
        }

    }
}
