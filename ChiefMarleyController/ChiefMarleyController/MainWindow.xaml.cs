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
using System.Net.Sockets;
using System.Net;

namespace ChiefMarleyController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ChiefMarleyControllerSettings Settings;
        Pioneer1120Engine eng;
        comms comms;
        
        public MainWindow()
        {
            InitializeComponent();
            GetSettings();
            comms = new comms(Settings);
            eng = new Pioneer1120Engine(comms);
            
        }

        private void GetSettings()
        {
            Settings = new ChiefMarleyControllerSettings("192.168.1.107", 23);
            
        }
    }

    /// <summary>
    /// Contains data we would like to keep, we want this serializable so settings can persist
    /// </summary>
    public class ChiefMarleyControllerSettings
    {
        public string IpAddress;
        public Int32 Port;

        // parameterless required for serialization
        ChiefMarleyControllerSettings()
        {
        }

        public ChiefMarleyControllerSettings(string ipAddress, Int32 port)
        {
            IpAddress = ipAddress;
            Port = port;
        }
    }

   
}


