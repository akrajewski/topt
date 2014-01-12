using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing.Collections
{
    public class PriorityQueue
    {
        private class Item : IComparable<Item>
        {
            public int V { get; set; }
            public double Priority { get; set; }

            public int CompareTo(Item other)
            {
                var op = other.Priority;
                var tp = this.Priority;
                return (op == tp) ? 0 : ((tp < op) ? -1 : 1);
            }
        }

        private HashSet<Item> set;

        public PriorityQueue()
        {
            this.set = new HashSet<Item>();
        }

        public void Enqueue(int v, double priority)
        {
            set.Add(new Item { V = v, Priority = priority });
        }

        public int Dequeue()
        {
            var min = set.Min();
            set.Remove(min);
            return min.V;
        }

        public bool isEmpty()
        {
            return set.Count == 0;
        }

        public void Update(int v, double priority)
        {
            set.First(i => i.V == v).Priority = priority;
        }

        public bool Contains(int v)
        {
            return set.Count(i => i.V == v) == 1;
        }
    }
}
