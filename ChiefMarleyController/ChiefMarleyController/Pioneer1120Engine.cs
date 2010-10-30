using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ChiefMarleyController
{
    public class Pioneer1120Engine
    {
        /// <summary>
        /// last known volume state
        /// </summary>
        int Volume = 0;
        bool Listening = false;
        private string splitSequence = "\r\n";
        comms Comms;

        BackgroundWorker bw = new BackgroundWorker();

        // going to use this to put received strings in, will figure out what to do with them later
        Queue<string> Received;
        
        
        public Pioneer1120Engine(comms comms)
        {
            Received = new Queue<string>();
            Comms = comms;
            SendData("VU");
            
            // create thread to listen and receive responses
            bw.DoWork += RX;
            bw.RunWorkerAsync();
            
        }

        public void SendData(string s)
        {
            Comms.Write(s + "\r");
        }

        /// <summary>
        /// get received messages
        /// </summary>
        private void RX(object sender, DoWorkEventArgs e)
        {
            string rxBuffer = string.Empty;

            Listening = true;
            while (Comms.IsConneted)
            {
                rxBuffer += Comms.Read();
                if (rxBuffer.Contains(splitSequence))
                {
                    ProcessRawRX(rxBuffer);
                    rxBuffer = string.Empty;
                }
            }
            Listening = false;
        }

        /// <summary>
        /// processes the raw rx and puts them on the queue
        /// </summary>
        private void ProcessRawRX(string rx)
        {
            string[] lines = Regex.Split(rx, splitSequence);
            foreach (string s in lines)
            {
                if (s.StartsWith("FL") || s.Length == 0)
                {
                    // don't need blank lines or FL stuff now, throw it away
                }
                else
                {
                    Received.Enqueue(s);
                }
            }
        }
        
    }
}
