using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TOPT.NetworkSimulator.Routing;
using TOPT.NetworkSimulator.Engine;
using System.Text.RegularExpressions;

using TOPT.NetworkSimulator.Routing.Algorithms;

namespace TOPT.NetworkSimulator.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            RoutingTests(args);
            //EngineTests();
        }

        private static void EngineTests()
        {
            Network net = new Network(3, 0, 1.0, 10);
            Console.Write(net.ToString());
            Console.ReadKey();
        }

        private static void RoutingAlgorithmsTests(string[] args)
        {
            var filename = args[0];
            var lines = File.ReadAllLines(filename);
            var vertexCount = int.Parse(lines.First());
            var graph = new DirectedGraph(vertexCount);
            foreach (var line in lines.Skip(1))
            {
                var singleSpaced = Regex.Replace(line, @"\s+", " ");
                var splitted = singleSpaced.Trim().Split(' ');
                var from = int.Parse(splitted[0]);
                var to = int.Parse(splitted[1]);
                graph.AddEdge(new DirectedEdge(from, to));
            }
            var pathSearch = new PathSearch(graph);

            Console.WriteLine("All paths:");
            var paths = pathSearch.Paths();
            paths.Sort();
            paths.ForEach(path => Console.WriteLine(path.ToString()));
            Console.WriteLine("=====\n");

            Console.WriteLine("Shortest paths:");
            var shortestPaths = pathSearch.ShortestPaths();
            shortestPaths.Sort();
            shortestPaths.ForEach(path => Console.WriteLine(path.ToString()));
            Console.WriteLine("=====\n");

            Console.WriteLine("Longest paths:");
            var longestPaths = pathSearch.LongestPaths();
            longestPaths.Sort();
            longestPaths.ForEach(path => Console.WriteLine(path.ToString()));
            Console.WriteLine("=====\n");

            Console.WriteLine("Random paths:");
            var randomPaths = pathSearch.RandomPaths();
            randomPaths.Sort();
            randomPaths.ForEach(path => Console.WriteLine(path.ToString()));
            Console.WriteLine("=====\n");
        }

        public static void RoutingTests(string[] args)
        {

            var network = new Network(4, 10, 1.0, 100);
            var pce = new PCE(network, PCE.RoutingAlgorithm.ShortestPaths);
            pce.Compute();
            var scheduler = new Scheduler(network);

            var statistics = new StatisticsManager(network);
            Node.statistics = statistics;
            NodePort.statistics = statistics;

            Console.WriteLine(network.ToString());
            Console.ReadKey();

            scheduler.PerformSimulation();

            Console.WriteLine(statistics);
            Console.ReadKey();
        }
    }
}
