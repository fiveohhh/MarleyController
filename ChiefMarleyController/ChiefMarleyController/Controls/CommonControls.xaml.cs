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
    /// Interaction logic for CommonControls.xaml
    /// </summary>
    public partial class CommonControls : UserControl
    {
        public delegate void SendData(string s);

        SendData sendData;

        public Pioneer1120.Pioneer1120State DeviceState
        {
            get
            {
                return DeviceState;
            }
            set
            {
                volume1.VolumeState = DeviceState.CurrentVolumeState;
            }
        }
        public CommonControls()
        {
            InitializeComponent();
            volume1.VolumeChanged += new EventHandler(volume1_VolumeChanged);
        }

        public void SetSendDataFunction(SendData sd)
        {
            sendData = sd;
        }

        void volume1_VolumeChanged(object sender, EventArgs e)
        {
            PioneerProtocol.VolumeState vol = ((SubControls.Volume)sender).VolumeState;
            PioneerProtocol.Request.Volume.VolumeSet vMsg = new PioneerProtocol.Request.Volume.VolumeSet(vol.VolumeLvl);
            sendData(vMsg.GetMsgAsString());
        }
    }
}
