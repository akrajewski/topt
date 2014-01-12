using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing.Algorithms
{
    public class DirectedEdge
    {
        private const int WEIGHT = 1;

        public int From { get; private set; }
        public int To { get; private set; }
        public double Weight { get; private set; }

        public DirectedEdge(int from, int to)
        {
            this.From = from;
            this.To = to;
            this.Weight = WEIGHT;
        }

        public DirectedEdge(int from, int to, double weight)
            : this(from, to)
        {
            this.Weight = weight;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var other = obj as DirectedEdge;
            return (other != null) ? (this.From == other.From && this.To == other.To) : false;
        }
    }
}
