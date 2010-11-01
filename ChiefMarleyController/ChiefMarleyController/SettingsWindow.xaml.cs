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
using System.Windows.Shapes;

namespace ChiefMarleyController
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {

        public ChiefMarleyControllerSettings settingsOnClose;

        public event EventHandler NewSettingsSaved;

        public SettingsWindow(ChiefMarleyControllerSettings settings)
        {

            InitializeComponent();
            if (settings != null)
            {
                settings1.addressing1.txtBox_IPAddress.Text = settings.IpAddress;
                settings1.addressing1.txtBox_Port.Text = settings.Port.ToString();
            }

            settings1.btn_SaveSettings.Click += new RoutedEventHandler(btn_SaveSettings_Click);

        }

        void btn_SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            settingsOnClose = new ChiefMarleyControllerSettings(
               settings1.addressing1.txtBox_IPAddress.Text,
               int.Parse(settings1.addressing1.txtBox_Port.Text));
            if (NewSettingsSaved != null)
            {
                NewSettingsSaved(settingsOnClose, null);
            }
            this.Hide();
        }

        private void settings1_Loaded(object sender, RoutedEventArgs e)
        {
            settingsOnClose = new ChiefMarleyControllerSettings(
                settings1.addressing1.txtBox_IPAddress.Text, 
                int.Parse(settings1.addressing1.txtBox_Port.Text));
            //if (NewSettingsSaved != null)
            //{
            //    NewSettingsSaved(settingsOnClose, null);
            //}
        }
    }
}
