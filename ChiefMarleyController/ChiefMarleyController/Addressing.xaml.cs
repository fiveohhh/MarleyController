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
using System.Net;

namespace ChiefMarleyController
{
    /// <summary>
    /// Interaction logic for Addressing.xaml
    /// </summary>
    public partial class Addressing : UserControl
    {
        public Addressing()
        {
            InitializeComponent();
        }


        public IPEndPoint GetConnection()
        {
            IPEndPoint ipe = null;
            IPAddress addr;
            int port;
            if (IPAddress.TryParse(txtBox_IPAddress.Text, out addr) && int.TryParse(txtBox_Port.Text,out port))
            {
                ipe = new IPEndPoint(addr, port);
            }

            return ipe;
        }

        public void SetConnection(IPEndPoint endPoint)
        {
            txtBox_IPAddress.Text = endPoint.Address.ToString();
            txtBox_Port.Text = endPoint.Port.ToString();
        }
    }


}
