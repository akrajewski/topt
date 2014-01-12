using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing.Algorithms
{
    public class DirectedGraph
    {
        private List<DirectedEdge>[] adj;

        public int VertexCount 
        { 
            get { return adj.Length; }
            
        }

        public DirectedGraph(int v)
        {
            adj = new List<DirectedEdge>[v];
            for(var i = 0; i < v; i++)
            {
                adj[i] = new List<DirectedEdge>();
            }
        }

        public void AddEdge(int from, int to)
        {
            adj[from].Add(new DirectedEdge(from, to));
        }

        public void AddEdge(DirectedEdge de)
        {
            adj[de.From].Add(de);
        }

        public void RemoveEdge(DirectedEdge de)
        {
            adj[de.From].Remove(de);
        }

        public List<DirectedEdge> Edges(int v)
        {
            return adj[v];
        }

        public DirectedGraph Without(List<DirectedEdge> ommitedEdges)
        {
            var clone = this.Clone();
            ommitedEdges.ForEach(edge => clone.RemoveEdge(edge));
            return clone;
        }

        public DirectedGraph Clone()
        {
            var clone = new DirectedGraph(this.VertexCount);
            foreach (var adjList in adj)
            {
                adjList.ForEach(edge => clone.AddEdge(edge.From, edge.To));
            }
            return clone;
        }
    }
}
