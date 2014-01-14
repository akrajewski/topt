using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using TOPT.NetworkSimulator.Routing.Algorithms;
using TOPT.NetworkSimulator.Routing;
using TOPT.NetworkSimulator.Engine;

using NDesk.Options;

namespace TOPT.NetworkSimulator.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var routing = "ShortestPaths";
            var networkSize = 2;
            var queueSize = 100;
            var verbose = false;
            var packetsPerSec = 1.0;
            var help = false;

            var optionSet = new OptionSet()
            {
                { 
                    "r|routing=",
                    "The routing algorithm to use. Available algorithms: LongestPaths, ShortestPaths, RandomPaths. Default is ShortestPaths.",
                    v => routing = v 
                },
                {
                    "n|networkSize=",
                    "The size of the simulated network. Should be higher than 2. Default is 3.",
                    v => networkSize = int.Parse(v)
                },
                {
                    "q|queueLength=",
                    "The length of queue in a single network node. Default is 100.",
                    v => queueSize = int.Parse(v)
                },
                {
                    "p|packetsPerSec=",
                    "The number of packets to generate per second. Should be real higher than 0. Default is 1.0",
                    v => packetsPerSec = double.Parse(v)
                },
                {
                    "v|verbose",
                    "Enables verbosity, which prints additional info about simulated network",
                    v => verbose = (v != null)
                },
                {
                    "h|help",
                    "Prints help.",
                    v => help = (v != null)
                }
            };

            List<string> extra = null;
            try
            {
                extra = optionSet.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine("tns:");
                Console.WriteLine(e);
                Console.WriteLine("Try --help for more information");
                return;
            }

            if (extra.Count > 0 || help)
            {
                printHelp(optionSet);
                return;
            }

            PCE.RoutingAlgorithm routingAlgorithm = PCE.RoutingAlgorithm.ShortestPaths;
            try
            {
                routingAlgorithm = (PCE.RoutingAlgorithm) Enum.Parse(typeof(PCE.RoutingAlgorithm), routing);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Unknown routing algorithm. Using default.");
                Console.WriteLine("Try --help for more information.\n");
            }


            StartSimulation(routing, networkSize, queueSize, packetsPerSec, verbose);

        }

        private static void StartSimulation(string routing, int networkSize, int queueSize, double packetsPerSec, bool verbose)
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("TOPT Network Simulator v0.1");
            Console.WriteLine("Authors: Adam Krajewski, Bartosz Lipiński");
            Console.WriteLine("Date: Jan 2014");
            Console.WriteLine("-----------------------------------------");

            Console.WriteLine("Generating a {0}x{0} Manhattan nework...\n", networkSize);
            var network = new Network(networkSize, queueSize, packetsPerSec, 1000);

            Console.WriteLine("Routing algorithm used: " + routing);
            Console.WriteLine("Computing paths... " + ((networkSize > 3) ? "This may take a moment...\n" : "\n"));
            PCE.Compute(network, PCE.RoutingAlgorithm.ShortestPaths);

            if (verbose)
            {
                Console.WriteLine("Generated network and computed paths details:");
                Console.WriteLine(network.ToString());
            }

            var scheduler = new Scheduler(network);
            var statistics = new StatisticsManager(network);
            Node.statistics = statistics;
            NodePort.statistics = statistics;

            Console.WriteLine("Ready to start the simulation. Press any key to continue...\n");
            Console.ReadKey();

            scheduler.PerformSimulation();

            Console.WriteLine(statistics);
        }

        private static void printHelp(OptionSet os)
        {
            Console.WriteLine("Usage: tns [OPTIONS]");
            Console.WriteLine("Simulates packet transport in a NxN Manhattan network");
            os.WriteOptionDescriptions(Console.Out);
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

            var network = new Network(4, 2000, 1.0, 100);
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
