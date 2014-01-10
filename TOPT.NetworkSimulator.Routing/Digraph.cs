using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing
{
    public class Digraph
    {
        private List<DirectedEdge>[] adj;

        public int VertexCount 
        { 
            get { return adj.Length; }
            
        }

        public Digraph(int v)
        {
            adj = new List<DirectedEdge>[v];
            for(var i = 0; i < v; i++)
            {
                adj[i] = new List<DirectedEdge>();
            }
        }

        public void addEdge(int from, int to)
        {
            adj[from].Add(new DirectedEdge(from, to));
        }

        public void addEdge(DirectedEdge de)
        {
            adj[de.From].Add(de);
        }

        public List<DirectedEdge> edges(int v)
        {
            return adj[v];
        }
    }
}
