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

namespace ChiefMarleyController.Controls.SubControls
{
    /// <summary>
    /// Interaction logic for Volume.xaml
    /// </summary>
    public partial class Volume : UserControl
    {

        private PioneerProtocol.VolumeState volState;
        public PioneerProtocol.VolumeState VolumeState
        {
            get
            {
                return volState;
            }
            set
            {
                if (value != null)
                {
                    volState = value;
                    slider1.Value = volState.VolumeLvl;
                }
            }
        }
        public event EventHandler VolumeChanged;
        public Volume()
        {
            volState = new PioneerProtocol.VolumeState();
            InitializeComponent();
            slider1.Minimum = PioneerProtocol.VolumeState.MIN_VOL;
            slider1.Maximum = PioneerProtocol.VolumeState.MAX_VOL;
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            volState.VolumeLvl = (byte)slider1.Value;
            if (VolumeChanged != null)
            {
                VolumeChanged(this, e);
            }
        }


    }
}
