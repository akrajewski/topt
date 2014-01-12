using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TOPT.NetworkSimulator.Routing.Collections;

namespace TOPT.NetworkSimulator.Routing.Algorithms
{
    public class DijkstraSP
    {
        private DirectedGraph graph;
        private int s;
        private DirectedEdge[] edgeTo;
        private double[] distTo;
        private PriorityQueue queue;

        public DijkstraSP(DirectedGraph digraph, int designated)
        {
            this.graph = digraph;
            this.s = designated;

            var vertexCount = digraph.VertexCount;
            this.edgeTo = new DirectedEdge[vertexCount];
            this.distTo = new double[vertexCount];
            queue = new PriorityQueue();
            
            for (var i = 0; i < vertexCount; i++)
            {
                distTo[i] = (i == s) ? 0.0 : Double.MaxValue;
            }

            queue = new PriorityQueue();
            queue.Enqueue(s, distTo[s]);
            while (!queue.isEmpty())
            {
                var vertex = queue.Dequeue();
                RelaxVertex(vertex);
            }
        }

        private void RelaxEdge(DirectedEdge e)
        {
            var v = e.From;
            var w = e.To;
            if (distTo[w] > distTo[v] + e.Weight)
            {
                distTo[w] = distTo[v] + e.Weight;
                edgeTo[w] = e;
                if (queue.Contains(w))
                {
                    queue.Update(w, distTo[w]);
                }
                else
                {
                    queue.Enqueue(w, distTo[w]);
                }
            }
        }

        private void RelaxVertex(int v)
        {
            var edges = graph.Edges(v);
            edges.ForEach(e => RelaxEdge(e));
        }

        public Path PathTo(int v)
        {
            var path = new Stack<DirectedEdge>();
            for (var e = edgeTo[v]; e != null; e = edgeTo[e.From])
            {
                path.Push(e);
            }
            return (path.Count != 0) ? new Path(path.ToList()) : null;
        }
    }
}
