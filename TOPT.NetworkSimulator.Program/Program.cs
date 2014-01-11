using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using TOPT.NetworkSimulator.Routing;

namespace TOPT.NetworkSimulator.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            RoutingTests(args);
        }

        private static void RoutingTests(string[] args)
        {
            var filename = "tinyEWD.txt";
            var lines = File.ReadAllLines(filename);
            var vertexCount = int.Parse(lines.First());
            var graph = new Digraph(vertexCount);
            foreach (var line in lines.Skip(1))
            {
                var splitted = line.Split(' ');
                var from = int.Parse(splitted[0]);
                var to = int.Parse(splitted[1]);
                graph.addEdge(new DirectedEdge(from, to));
            }
            var dijkstra = new DijkstraSP(graph, 0);
            for (var i = 1; i < vertexCount; i++)
            {
                Console.Write("0 to {0}: ", i);
                foreach (var edge in dijkstra.pathTo(i))
                {
                    Console.Write("{0}->{1}", edge.From, edge.To);
                }
                Console.Write("\n");
            }
        }

    }
}
