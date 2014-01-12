using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing
{
    public class RoutingTable
    {
        private List<RoutingTableEntry> entries = new List<RoutingTableEntry>();

        public void AddEntry(int from, int to, int nextHop)
        {
            entries.Add(new RoutingTableEntry { From = from, To = to, NextHop = nextHop}); 
        }

        public int NextHop(int from, int to)
        {
            return entries.First(entry => entry.From == from && entry.To == to).NextHop;
        }

        public bool Contains(int from, int to)
        {
            return entries.Count(entry => entry.From == from && entry.To == to) > 0;
        }
        
        public override string ToString()
        {
            var builder = new StringBuilder();
            entries.ForEach(entry => builder.AppendLine("\t\t" + entry));
            return (builder.Length != 0) ? builder.ToString() : "\t\tempty";
        }

        private class RoutingTableEntry
        {
            public int From { get; set; }
            public int To { get; set; }
            public int NextHop { get; set; }

            public override string ToString()
            {
                return string.Format("From {0} To {1} Via {2}", this.From, this.To, this.NextHop);
            }
        }
    }
}
