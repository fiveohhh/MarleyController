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
        public Pioneer1120Engine eng;
        comms comms;
        
        public MainWindow()
        {
            bool ConnectionSuccesful = false;
            InitializeComponent();
            
            GetSettings();
            sw.NewSettingsSaved += new EventHandler(sw_NewSettingsSaved);
            while (!ConnectionSuccesful)
            {
                
                try
                {
                    comms = new comms(Settings);
                    ConnectionSuccesful = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error connecting to device\r\n" + e.Message);
                    sw.ShowDialog();
                }
            }
            eng = new Pioneer1120Engine(comms);
            eng.UpdateStatus();
            System.Threading.Thread.Sleep(1000);
            tabController1.SetDeviceState(eng.DevState);
            tabController1.MessagesReadyToSend += new EventHandler(tabController1_MessagesReadyToSend);
            eng.DeviceStateUpdateFromDevice += new EventHandler(eng_DeviceStateUpdateFromDevice);
            
            eng.SetDevChangedDel(UpdateDevState);
            
        }

        void sw_NewSettingsSaved(object sender, EventArgs e)
        {
            Settings = sw.settingsOnClose;
            comms = new comms(Settings);
            
        }

        /// <summary>
        /// same as tabController1.SetDeviceState(eng.DevState) was having threading issues
        /// </summary>
        /// <param name="st"></param>
        public void UpdateDevState(Pioneer1120.Pioneer1120State st)
        {
            eng_DeviceStateUpdateFromDevice(null, null);
        }

        /// <summary>
        /// Device state was updated, lets make sure the gui is in sync with the device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void eng_DeviceStateUpdateFromDevice(object sender, EventArgs e)
        {
            if (tabController1.Dispatcher.CheckAccess())
            {
                tabController1.SetDeviceState(eng.DevState);
            }
            else
            {
                eng.bw.ReportProgress(0,eng.DevState);
            }
        }

        void tabController1_MessagesReadyToSend(object sender, EventArgs e)
        {
            while (tabController1.MessagesToSend.Count > 0)
            {
                eng.SendData(tabController1.MessagesToSend.Dequeue());
            }
        }

        /// <summary>
        /// Get settings from persisted storrage
        /// </summary>
        private void GetSettings()
        {
            Settings = new ChiefMarleyControllerSettings("192.168.1.101", 23);
            sw = new SettingsWindow(Settings);
        }



        SettingsWindow sw;
        /// <summary>
        /// server settings button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            var t = sw.ShowDialog();
            
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


