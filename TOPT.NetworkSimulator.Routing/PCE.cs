using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TOPT.NetworkSimulator.Routing.Algorithms;

namespace TOPT.NetworkSimulator.Routing
{
    public class PCE
    {
        private RoutingAlgorithm algorithm;
        private IRoutableNetwork network;

        public PCE(IRoutableNetwork network, RoutingAlgorithm algorithm)
        {
            this.algorithm = algorithm;
            this.network = network;
        }

        public void Compute()
        {
            var search = new PathSearch(network.ToGraph());
            List<Path> paths = null;
            if (algorithm == RoutingAlgorithm.ShortestPaths)
            {
                paths = search.ShortestPaths();
            }
            else if (algorithm == RoutingAlgorithm.LongestPaths)
            {
                paths = search.LongestPaths();
            }
            else if (algorithm == RoutingAlgorithm.RandomPaths)
            {
                paths = search.RandomPaths();
            }
            FillRoutingTables(paths);
        }

        public enum RoutingAlgorithm
        {
            ShortestPaths,
            LongestPaths,
            RandomPaths
        }

        private void FillRoutingTables(List<Path> paths)
        {
            foreach (var path in paths)
            {
                var to = path.To;
                var from = path.From;
                for (var i = 0; i < path.Vertices.Count - 1; i++)
                {
                    var curr = path.Vertices[i];
                    var nextHop = path.Vertices[i + 1];
                    var router = network.Routers.First(r => r.Id == curr);
                    router.RoutingTable.AddEntry(from, to, nextHop);
                }
            }
        }
    }
}
