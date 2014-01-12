using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Routing.Algorithms
{
    public class PathSearch
    {
        private DirectedGraph graph;
        private List<int> traversed;
        private List<Path> paths;

        public PathSearch(DirectedGraph graph)
        {
            this.graph = graph;
            this.paths = new List<Path>();
            for (var i = 0; i < graph.VertexCount; i++)
            {
                this.traversed = new List<int>();
                DFS(i);
            }
        }

        public List<Path> Paths()
        {
            return this.paths;
        }

        public List<Path> Paths(int from, int to)
        {
            return this.paths.Where(path => path.From == from && path.To == to).ToList();
        }

        public Path ShortestPath(int from, int to)
        {
            var paths = this.Paths(from, to);
            var shortestDist = paths.Min(path => path.Edges.Count);
            return paths.First(path => path.Edges.Count == shortestDist);
        }

        public List<Path> ShortestPaths()
        {
            return this.SelectPaths((from, to) => this.ShortestPath(from, to));
        }

        public Path LongestPath(int from, int to)
        {
            var paths = this.Paths(from, to);
            var longestDist = paths.Max(path => path.Edges.Count);
            return paths.First(path => path.Edges.Count == longestDist);
        }

        public List<Path> LongestPaths()
        {
            return this.SelectPaths((from, to) => this.LongestPath(from, to));
        }

        public Path RandomPath(int from, int to)
        {
            var paths = this.Paths(from, to);
            return paths[new Random().Next(paths.Count)];
        }

        public List<Path> RandomPaths()
        {
            return this.SelectPaths((from, to) => this.RandomPath(from, to));
        }

        private List<Path> SelectPaths(Func<int, int, Path> selector)
        {
            var selected = new List<Path>();
            for (var from = 0; from < graph.VertexCount; from++)
            {
                for (var to = 0; to < graph.VertexCount; to++)
                {
                    if (from != to)
                    {
                        selected.Add(selector(from, to));
                    }
                }
            }
            return selected;
        }

        private void DFS(int v)
        {
            traversed.Add(v);
            foreach (var edge in graph.Edges(v))
            {
                var to = edge.To;
                //Console.WriteLine("Visiting vertex: {0}", to);
                //Console.WriteLine("Traversed vertices: " + string.Join(" ", traversedVertices.Select(ve => ve.ToString()).ToArray()));
                if (!traversed.Contains(to))
                {
                    AddPath(to);
                    DFS(to);
                }
            }
            traversed.Remove(v);
        }

        private void AddPath(int to)
        {
            var verticesSoFar = new List<int>(traversed);
            verticesSoFar.Add(to);
            //Console.WriteLine("Adding path: " + new Path(verticesSoFar));
            var newPath = new Path(verticesSoFar);
            if (!paths.Contains(newPath))
            {
                paths.Add(new Path(verticesSoFar));
            }
            for (var i = 1; i <= verticesSoFar.Count - 2; i++)
            {
                //Console.WriteLine(new Path(verticesSoFar.Skip(i).ToList()));
                newPath = new Path(verticesSoFar.Skip(i).ToList());
                if (!paths.Contains(newPath))
                {
                    paths.Add(newPath);
                }
            }
        }
    }
}

