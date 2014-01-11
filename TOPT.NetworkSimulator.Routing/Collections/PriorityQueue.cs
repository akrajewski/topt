using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing.Collections
{
    public class PriorityQueue
    {
        private struct Item
        {
            public int V { get; set; }
            public double Priority { get; set; }

            public override int GetHashCode()
            {
                return V.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return false;
            }
            
        }

        private class ByPriority : IComparer<Item>
        {
            public int Compare(Item x, Item y)
            {
                Console.Write("here");
                var xp = x.Priority;
                var yp = y.Priority;
                var result = (xp == yp) ? 0 : ((xp < yp) ? -1 : 1);
                Console.WriteLine(result);
                return result;
            }
        }

        private SortedSet<Item> sortedSet;
        private HashSet<Item> set;

        public PriorityQueue()
        {
            this.set = new HashSet<Item>();
            this.sortedSet = new SortedSet<Item>(new ByPriority());
        }

        public void Enqueue(int v, double priority)
        {
            var item = new Item { V = v, Priority = priority };
            Console.Write(sortedSet.Add(item));
            Console.WriteLine(set.Add(item));
        }

        public int Dequeue()
        {
            var min = sortedSet.Min;
            sortedSet.Remove(min);
            return min.V;
        }

        public bool isEmpty()
        {
            return sortedSet.Count == 0;
        }
    }
}
