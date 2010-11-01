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

namespace ChiefMarleyController.Controls
{
    /// <summary>
    /// Interaction logic for TabController.xaml
    /// </summary>
    public partial class TabController : UserControl
    {
        public Queue<string> MessagesToSend { get; private set; }

        public event EventHandler MessagesReadyToSend;

        public TabController()
        {
            MessagesToSend = new Queue<string>();
            InitializeComponent();
            commonControls1.SetSendDataFunction(SendMessage);
            
        }

        /// <summary>
        /// seperate messages with '\r'
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(string msg)
        {
            string[] lines = msg.Split('\r');
            foreach (string s in lines)
            {
                MessagesToSend.Enqueue(s);
            }
            if (MessagesReadyToSend != null)
            {
                MessagesReadyToSend(null, null);
            }

        }

        /// <summary>
        /// Change the gui to match an updated state
        /// </summary>
        /// <param name="devState"></param>
        public void SetDeviceState(Pioneer1120.Pioneer1120State devState)
        {
            //try
            //{
                commonControls1.volume1.VolumeState = devState.CurrentVolumeState;
            //}
            //catch(Exception e)
            //{
            //    MessageBox.Show("Error setting Volume state");
            //}
        }

       // public delegate void UpdateDeviceState(Pioneer1120.Pioneer1120State DevState);
    }
}
