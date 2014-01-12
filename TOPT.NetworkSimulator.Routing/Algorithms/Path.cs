using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing.Algorithms
{
    public class Path : IComparable<Path>
    {
        public int From { get; private set; }
        public int To { get; private set; }
        public List<int> Vertices { get; private set; }
        public List<DirectedEdge> Edges { get; private set; }

        public Path(List<DirectedEdge> edges)
        {
            this.Edges = edges;
            this.From = edges.First().From;
            this.To = edges.Last().To;
            
            var vertices = new List<int>();
            foreach (var edge in edges)
            {
                if (!vertices.Contains(edge.From))
                {
                    vertices.Add(edge.From);
                }

                if (!vertices.Contains(edge.To))
                {
                    vertices.Add(edge.To);
                }
            }
        }

        public Path(List<int> vertices)
        {
            this.From = vertices.First();
            this.To = vertices.Last();
            this.Vertices = vertices;
            this.Edges = new List<DirectedEdge>();
            for (var i = 0; i < vertices.Count - 1; i++)
            {
                this.Edges.Add(new DirectedEdge(vertices[i], vertices[i + 1]));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = obj as Path;

            if (other.Vertices.Count != this.Vertices.Count)
            {
                return false;
            }

            for (var i = 0; i < this.Vertices.Count; i++)
            {
                if (this.Vertices[i] != other.Vertices[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int CompareTo(Path other)
        {
            return (this.From == other.From) 
                ? ((this.To == other.To) ? 0 : ((this.To < other.To) ? -1 : 1)) 
                : ((this.From < other.From) ? -1 : 1);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(string.Format("From {0} to {1} ", this.From, this.To));
            foreach (var edge in this.Edges)
            {
                builder.Append(string.Format(" {0}->{1} ", edge.From, edge.To));
            }
            return builder.ToString();
        }
    }
}
