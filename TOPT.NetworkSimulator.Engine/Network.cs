using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPT.NetworkSimulator.Routing;

namespace TOPT.NetworkSimulator.Engine
{
    public class Network : IRoutableNetwork
    {
        public List<Node> networkNodes { get; set; }
        public List<Link> networkLinks { get; set; }
        public List<TrafficGenerator> trafficGenerators { get; set; }

        public int packetsInNetwork { get; set; }

        public Network(int networkSize, int queueSize, double packetGenerationRate, int numberOfPacketsToGenerate)
        {
            NodeQueue.size = queueSize;
            packetsInNetwork = 0;

            //network generation
            networkNodes = new List<Node>();
            networkLinks = new List<Link>();

            trafficGenerators = new List<TrafficGenerator>();

            GenerateNetwork(networkSize);

            TrafficGenerator.packetGenerator = new PacketGenerator(this);
            TrafficGenerator.numberOfPacketsToGenerate = numberOfPacketsToGenerate;

            ConnectTrafficGenerators(packetGenerationRate);
        }

        private void ConnectTrafficGenerators(double packetGenerationRate)
        {
            NodePort localPort = null;
            foreach (Node n in networkNodes)
            {
                localPort = new NodePort(n); //create node port

                n.localPort = localPort; //connect node port to node

                trafficGenerators.Add(new TrafficGenerator(localPort, n, packetGenerationRate));
                    //connect traffic generator to node port
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Node n in networkNodes)
            {
                sb.AppendLine(n.ToString());
            }
            return sb.ToString();
        }

        private void GenerateNetwork(int networkSize)
        {
            Node firstNodeOfTheRow = null;

            bool currentHorizontalDirectionIsRight = true;
            bool currentVerticalDirectionIsBottom = true;

            Node generatedNode = null;

            Link horizontalLink = null;
            Link verticalLink = null;

            Link lastHorizontalLink = null; //needed when assigning proper destination nodes to links
            Link lastVerticalLink = null;   //(just like above)

            for (int row = 0; row < networkSize; ++row)
            {
                currentHorizontalDirectionIsRight = SetDirection(row); //just setting the current row direction (left or right)

                for (int column = 0; column < networkSize; ++column)
                {
                    currentVerticalDirectionIsBottom = SetDirection(column); //just setting the current column direction (top or bottom)

                    networkNodes.Add(new Node());
                    generatedNode = networkNodes.ElementAt(networkNodes.Count - 1);

                    if (column == 0)
                    {
                        firstNodeOfTheRow = generatedNode;
                    }

                    networkLinks.Add(new Link());
                    networkLinks.Add(new Link());

                    horizontalLink = networkLinks.ElementAt(networkLinks.Count - 2);
                    verticalLink = networkLinks.ElementAt(networkLinks.Count - 1);

                    //----------assigning links to right fields in node objects
                    if (currentHorizontalDirectionIsRight)
                    {
                        generatedNode.egressHorizontalLink = horizontalLink;
                        if (column != 0)
                        {
                            lastHorizontalLink = networkLinks.ElementAt(networkLinks.Count - 4);
                                //we add horizontal and then vertical link to the list, so we are interested in getting next to last element

                            generatedNode.ingressHorizontalLink = lastHorizontalLink;
                            lastHorizontalLink.SetDestinationNode(generatedNode);
                        }
                    }
                    else
                    {
                        generatedNode.ingressHorizontalLink = horizontalLink;
                        horizontalLink.SetDestinationNode(generatedNode); //this horizontal link ends in generated node
                        if (column != 0)
                        {
                            lastHorizontalLink = networkLinks.ElementAt(networkLinks.Count - 4);
                                //we add horizontal and then vertical link to the list, so we are interested in getting next to last element
                            generatedNode.egressHorizontalLink = lastHorizontalLink;
                        }
                    }

                    if (currentVerticalDirectionIsBottom)
                    {
                        generatedNode.egressVerticalLink = verticalLink;
                        if (row != 0)
                        {
                            lastVerticalLink = networkLinks.ElementAt(networkLinks.Count - 1 - (2*networkSize));
                                //this will return the link that is above currently added node
                            generatedNode.ingressVerticalLink = lastVerticalLink;
                            lastVerticalLink.SetDestinationNode(generatedNode);
                        }
                    }
                    else
                    {
                        generatedNode.ingressVerticalLink = verticalLink;
                        verticalLink.SetDestinationNode(generatedNode); //this vertical link ends in generated node
                        if (row != 0)
                        {
                            lastVerticalLink = networkLinks.ElementAt(networkLinks.Count - 1 - (2 * networkSize));
                                //this will return the link that is above currently added node
                            generatedNode.egressVerticalLink = lastVerticalLink;
                        }
                    }

                    if (column == networkSize - 1) //creating "the loop" of the network
                    {
                        if (currentHorizontalDirectionIsRight)
                        {
                            horizontalLink.SetDestinationNode(firstNodeOfTheRow);
                            firstNodeOfTheRow.ingressHorizontalLink = horizontalLink;
                        }
                        else
                        {
                            firstNodeOfTheRow.egressHorizontalLink = horizontalLink;
                        }
                    }

                    if (row == networkSize - 1)
                    {
                        Node topRowNode = networkNodes.ElementAt(networkNodes.Count - 1 - (networkSize * (networkSize - 1)));
                            //points to the node in the same column but in the first row

                        if (currentVerticalDirectionIsBottom)
                        {
                            verticalLink.SetDestinationNode(topRowNode);
                            topRowNode.ingressVerticalLink = verticalLink;
                        }
                        else
                        {
                            topRowNode.egressVerticalLink = verticalLink;
                        }
                    }
                    //----------
                }
            }

        }

        private bool SetDirection(int parameter) //function created just so the GenerateNetwork method looks a bit neater
        {
            if (parameter % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<IRouter> Routers
        {
            get { return networkNodes.Select(node => node as IRouter).ToList(); }
        }

        public Routing.Algorithms.DirectedGraph ToGraph()
        {
            var graph = new Routing.Algorithms.DirectedGraph(this.networkNodes.Count);
            foreach (var node in this.networkNodes)
            {
                var from = node.Id;
                graph.AddEdge(from, node.egressHorizontalLink.linkDestinationNode.Id);
                graph.AddEdge(from, node.egressVerticalLink.linkDestinationNode.Id);
            }
            return graph;
        }
    }
}
