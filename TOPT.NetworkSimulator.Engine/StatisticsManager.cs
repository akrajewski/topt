using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPT.NetworkSimulator.Engine
{
    public class StatisticsManager
    {
        List<PacketListElement> packetList = new List<PacketListElement>();
        Network network = null;

        public StatisticsManager(Network network)
        {
            this.network = network;
        }

        public void AddPacketToStatistics(Packet packet, PacketState state) 
        {
            packetList.Add(new PacketListElement(packet, state));
            network.packetsInNetwork--;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            //foreach (PacketListElement p in packetList)
            //{
            //    sb.Append(p.packet.ToString());
            //    sb.Append(" ");
            //    sb.AppendLine(Enum.GetName(typeof(PacketState), p.state));
            //}

            sb.AppendLine(GetStatistics());
            sb.AppendLine();
            //sb.AppendLine(GetPacketDestinationStatistics());

            return sb.ToString();
        }

        public String GetStatistics()
        {
            double avg_hops = 0;
            double avg_latency = 0;
            double latency_variance = 0;

            int deliveredCounter = 0;
            int droppedOnQueueCounter = 0;
            int droppedOutdatedCounter = 0;

            foreach (PacketListElement p in packetList)
            {
                if (p.state == PacketState.DELIVERED)
                {
                    deliveredCounter++;

                    avg_hops += p.packet.hops;
                    avg_latency += p.packet.latency;
                }
                else if (p.state == PacketState.DROPPED_ON_QUEUE)
                {
                    droppedOnQueueCounter++;
                }
                else if (p.state == PacketState.DROPPED_OUTDATED)
                {
                    droppedOutdatedCounter++;
                }
            }
            avg_latency /= (double)deliveredCounter;
            avg_hops /= (double)deliveredCounter;


            foreach (PacketListElement p in packetList)
            {
                if (p.state == PacketState.DELIVERED)
                {
                    latency_variance += Math.Pow(p.packet.latency - avg_latency, 2.0);
                }
            }
            latency_variance /= (double)deliveredCounter;

            return "Dropped on queue packets:\t" + droppedOnQueueCounter +
                        "\nDropped (outdated) packets:\t" + droppedOutdatedCounter +
                            "\nDelivered packets:\t\t" + deliveredCounter +
                            " \n\tavg_hops=" + Math.Round(avg_hops, 3) + ";   avg_latency=" + Math.Round(avg_latency, 3) + ";   latency variance=" + Math.Round(latency_variance, 3);
        }

        public String GetPacketDestinationStatistics ()
        {
            PacketDestinationStatistics destinationStatistics = new PacketDestinationStatistics(network.networkNodes.Count);
            destinationStatistics.CalculatePacketPercentages(packetList);

            return destinationStatistics.ToString();
        }
    }

    public class PacketListElement
    {
        public Packet packet { get; set; }
        public PacketState state { get; set; }

        public PacketListElement(Packet packet, PacketState state)
        {
            this.packet = packet;
            this.state = state;
        }
    }

    public class PacketDestinationStatistics
    {
        List<PacketDestinationListElement> packets = null;
        int maxNumberOfDigits = 0;

        public PacketDestinationStatistics(int numberOfNodes)
        {
            packets = new List<PacketDestinationListElement>();

            for (int i = 0; i < numberOfNodes; i++)
            {
                for (int j = 0; j < numberOfNodes; j++)
                {
                    if (i != j) //its not the same node number
                    {
                        packets.Add(new PacketDestinationListElement(i, j));
                    }
                }
            }

            maxNumberOfDigits = CalculateNumberOfDigits(numberOfNodes - 1);
        }

        private int CalculateNumberOfDigits(int number)
        {
            if (number == 0)
                return 1;

            double temp = (double)number;
            int counter = 0;

            while (temp >= 1.0)
            {
                temp /= 10.0;
                counter++;
            }

            return counter;
        }


        public void CalculatePacketPercentages(List<PacketListElement> packetList)
        {
            foreach (PacketListElement p in packetList)
            {
                foreach (PacketDestinationListElement destination in packets)
                {
                    if (destination.IsTheSamePacketAs(p))
                    {
                        destination.percentage++;
                        break;
                    }
                }
            }

            foreach (PacketDestinationListElement destination in packets)
            {
                destination.percentage /= packetList.Count;
                destination.percentage *= 100.0;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            double sum = 0.0;

            String strSrc = null;
            int srcNumOfDigitsDiff = 0;

            String strDst = null;
            int dstNumOfDigitsDiff = 0;


            foreach (PacketDestinationListElement p in packets)
            {

                srcNumOfDigitsDiff = maxNumberOfDigits - CalculateNumberOfDigits(p.sourceId);
                strSrc = p.sourceId.ToString();
                while (srcNumOfDigitsDiff-- > 0)
                {
                    strSrc = " " + strSrc;
                }

                dstNumOfDigitsDiff = maxNumberOfDigits - CalculateNumberOfDigits(p.destinationId);
                strDst = p.destinationId.ToString();
                while (dstNumOfDigitsDiff-- > 0)
                {
                    strDst = " " + strDst;
                }


                sb.AppendLine("[" + strSrc + " -> " + strDst + "]\t" + p.percentage + "%");
                sum += p.percentage;
            }
            sb.AppendLine("Sum of percentages= " + sum);
            return sb.ToString();
        }
    }


    public class PacketDestinationListElement
    {
        public int sourceId { get; set; }
        public int destinationId { get; set; }
        public double percentage { get; set; }

        public PacketDestinationListElement (int sourceId, int destinationId)
        {
            this.sourceId = sourceId;
            this.destinationId = destinationId;
            this.percentage = 0;
        }

        public bool IsTheSamePacketAs(PacketListElement element)
        {
            if (element.packet.sourceId == this.sourceId && element.packet.destinationId == this.destinationId)
                return true;
            else
                return false;
        }
    }

    

    public enum PacketState //nie mam pomyslu na lepsza nazwe
    {
        DELIVERED,
        DROPPED_ON_QUEUE,
        DROPPED_OUTDATED
    }
}
