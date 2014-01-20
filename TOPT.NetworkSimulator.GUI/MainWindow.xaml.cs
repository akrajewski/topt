using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using TOPT.NetworkSimulator.Engine;
using TOPT.NetworkSimulator.Routing;

namespace TOPT.NetworkSimulator.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitData();
        }

        private Dictionary<TextBox, string> defaults = new Dictionary<TextBox,string>();
        private Dictionary<PCE.RoutingAlgorithm, List<Network>> routedNetworks = new Dictionary<PCE.RoutingAlgorithm, List<Network>>();

        public void InitData()
        {

            this.routingAlgorithms.ItemsSource = new List<string>()
            {
                "ShortestPaths",
                "LongestPaths",
                "RandomPaths"
            };
            this.routingAlgorithms.SelectedIndex = 0;

            this.defaults.Add(this.networkSize, "2");
            this.defaults.Add(this.packetCount, "400");
            this.defaults.Add(this.packetsPerSecond, "1.0");
            this.defaults.Add(this.packetTimeout, "10");
            this.defaults.Add(this.queueSize, "1000");
            foreach (var textBox in defaults.Keys)
            {
                textBox.Text = defaults[textBox];
            }

            routedNetworks.Add(PCE.RoutingAlgorithm.ShortestPaths, new List<Network>());
            routedNetworks.Add(PCE.RoutingAlgorithm.LongestPaths, new List<Network>());
            routedNetworks.Add(PCE.RoutingAlgorithm.RandomPaths, new List<Network>());

        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            var networkSize = int.Parse(this.networkSize.Text);
            var queueSize = int.Parse(this.queueSize.Text);

            var packetsPerSecText = this.packetsPerSecond.Text.Replace(".", ",") ;

            var packetsPerSec = double.Parse(packetsPerSecText);
            var packetCount = int.Parse(this.packetCount.Text);
            var packetTimeout = int.Parse(this.packetTimeout.Text);
            var routingAlgorithm = (PCE.RoutingAlgorithm) Enum.Parse(typeof(PCE.RoutingAlgorithm), this.routingAlgorithms.SelectedItem as string);

            var network = routedNetworks[routingAlgorithm].FirstOrDefault(n => n.Routers.Count == networkSize * networkSize);
            if (network == null)
            {
                network = new Network(networkSize, queueSize, packetsPerSec, packetCount, packetTimeout);
                PCE.Compute(network, routingAlgorithm);
                routedNetworks[routingAlgorithm].Add(network);
            }
            else
            {
                network.SetupPacketGeneration(networkSize, queueSize, packetsPerSec, packetCount, packetTimeout);
            }


            //var network = new Network(networkSize, queueSize, packetsPerSec, packetCount, packetTimeout);
            //PCE.Compute(network, routingAlgorithm);
            var scheduler = new Scheduler(network);
            var statistics = new StatisticsManager(network);
            Node.statistics = statistics;
            NodePort.statistics = statistics;
            NodeQueue.statistics = statistics;
            scheduler.PerformSimulation();

    
            this.output.Text += "----------------------------";
            this.output.Text += "\nNetwork size: " + networkSize;
            this.output.Text += "\nQueue size: " + queueSize;
            this.output.Text += "\nRouting algorithm: " + routingAlgorithm;
            this.output.Text += "\nPacket count: " + packetCount;
            this.output.Text += "\nPackets per second: " + packetsPerSec;
            this.output.Text += "\nPacket timeout: " + packetTimeout + "\n";

            this.output.Text += statistics.ToString();

            this.output.Text += "----------------------------\n";
        }

        private void int_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text[0])) 
            {
                e.Handled = true;
            }
        }

        private void double_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var curr = (sender as TextBox).Text;

            if (!(Char.IsDigit(e.Text[0]) || e.Text[0] == '.'))
            {
                e.Handled = true;
            }
            
        }

        private void SetDefaultValue(TextBox textBox)
        {
            textBox.Text = defaults[textBox];
        }

        private new void LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            try
            {
                textBox.Text = textBox.Text.Replace(" ", string.Empty);
                Double.Parse(textBox.Text);
                Int32.Parse(textBox.Text);
            }
            catch (FormatException ex)
            {
                SetDefaultValue(textBox);
            }

        }

    }
}
