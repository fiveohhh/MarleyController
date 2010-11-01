using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChiefMarleyController
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        public ChiefMarleyControllerSettings ConnectionSettings { get; private set; }

        /// <summary>
        /// will want to catch exception that occur in here and let someone know
        /// </summary>
        /// <param name="settings"></param>
        public void SetSettings(ChiefMarleyControllerSettings settings)
        {
            // set addressing
            System.Net.IPAddress ipAddr;
            ipAddr = System.Net.IPAddress.Parse(settings.IpAddress);
            System.Net.IPEndPoint endPoint = new System.Net.IPEndPoint(ipAddr, settings.Port);
            addressing1.SetConnection(endPoint);
        }

        // when save is clicked
        // probably going to fire an event and update everything we need to update
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
        }


    }

    
}
