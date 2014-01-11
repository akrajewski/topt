using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing
{
    public class DirectedEdge
    {
        private const int WEIGHT = 1;

        public int From { get; private set; }
        public int To { get; private set; }
        public int Weight { get; private set; }

        public DirectedEdge(int from, int to)
        {
            this.From = from;
            this.To = to;
            this.Weight = WEIGHT;
        }

        public DirectedEdge(int from, int to, int weight)
            : this(from, to)
        {
            this.Weight = weight;
        }
    }
}
