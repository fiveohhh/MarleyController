using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ChiefMarleyController
{
    /// <summary>
    /// Handles all sending and receiving
    /// </summary>
    public class comms
    {
        TcpClient tcpSocket;
        const Int32 DEFAULT_TIMEOUT = 100; //ms



        public comms(ChiefMarleyControllerSettings settings)
        {
            IPEndPoint ipe = null;
            IPAddress addr;
            if (IPAddress.TryParse(settings.IpAddress, out addr))
            {
                ipe = new IPEndPoint(addr, settings.Port);
            }
            else
            {
                throw new Exception("Error Parsing IpAddress");
            }
            try
            {
                tcpSocket = new TcpClient(addr.ToString(), settings.Port);
            }
            catch(Exception e)
            {
                tcpSocket.Close();
                throw e;

            }
            
        }

        public bool IsConneted
        {
            get
            {
                return tcpSocket.Connected;
            }
        }

        public string Read()
        {
            int bufferSize = 255;
            if (!tcpSocket.Connected)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            byte[] buffer = new byte[bufferSize];
            while (tcpSocket.Available > 0)
            {
                tcpSocket.GetStream().Read(buffer, 0, bufferSize);
                sb.Append(System.Text.ASCIIEncoding.ASCII.GetString(buffer));
                buffer = new byte[bufferSize];
            }
            string rx = sb.ToString();
            int endOfString = rx.IndexOf('\0');

            string cleanrx = string.Empty;
            if (endOfString > 0)
            {
                cleanrx= rx.Remove(endOfString);
            }
            return cleanrx;
        }

        public void Write(string s)
        {
            if (!tcpSocket.Connected) return;
            byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(s.Replace("\0xFF", "\0xFF\0xFF"));
            tcpSocket.GetStream().Write(buf, 0, buf.Length);
        }

        
        
    }
}
