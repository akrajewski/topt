using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TOPT.NetworkSimulator.Routing.Collections;

namespace TOPT.NetworkSimulator.Routing
{
    public class DijkstraSP
    {
        private Digraph digraph;
        private int s;
        private DirectedEdge[] edgeTo;
        private double[] distTo;

        public DijkstraSP(Digraph digraph, int designated)
        {
            this.digraph = digraph;
            this.s = designated;

            var vertexCount = digraph.VertexCount;
            this.edgeTo = new DirectedEdge[vertexCount];
            this.distTo = new double[vertexCount];
            var queue = new PriorityQueue();
            
            for (var i = 0; i < vertexCount; i++)
            {
                distTo[i] = (i == s) ? 0.0 : Double.MaxValue;
                queue.Enqueue(i, distTo[i]);
            }

            while (!queue.isEmpty())
            {
                var vertex = queue.Dequeue();
                relaxVertex(vertex);
            }
        }

        private void relaxEdge(DirectedEdge e)
        {
            var v = e.From;
            var w = e.To;
            if (distTo[w] > distTo[v] + e.Weight)
            {
                distTo[w] = distTo[v] + e.Weight;
                edgeTo[w] = e;
            }
        }

        private void relaxVertex(int v)
        {
            var edges = digraph.edges(v);
            foreach (var edge in edges)
            {
                relaxEdge(edge);
            }
        }

        public List<DirectedEdge> pathTo(int v)
        {
            var path = new Stack<DirectedEdge>();
            for (var e = edgeTo[v]; e != null; e = edgeTo[e.From])
            {
                path.Push(e);
            }
            return path.ToList();
        }
    }
}
