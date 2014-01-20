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

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.IsEnabled = false;
            mainGrid.Opacity = 0.75;
            progressPanel.Visibility = Visibility.Visible;

            var networkSize = int.Parse(this.networkSize.Text);
            var queueSize = int.Parse(this.queueSize.Text);
            var packetsPerSec = double.Parse(this.packetsPerSecond.Text);
            var packetCount = int.Parse(this.packetCount.Text);
            var packetTimeout = int.Parse(this.packetTimeout.Text);
            var routingAlgorithm = (PCE.RoutingAlgorithm) Enum.Parse(typeof(PCE.RoutingAlgorithm), this.routingAlgorithms.SelectedItem as string);

            var network = new Network(networkSize, queueSize, packetsPerSec, packetCount);
            PCE.Compute(network, routingAlgorithm);
            var scheduler = new Scheduler(network);
            var statistics = new StatisticsManager(network);
            Node.statistics = statistics;
            NodePort.statistics = statistics;
            scheduler.PerformSimulation();
            
            this.output.Text += statistics.ToString();

            progressPanel.Visibility = Visibility.Hidden;
            mainGrid.Opacity = 1;
            mainGrid.IsEnabled = true;

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
