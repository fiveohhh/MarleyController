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

        Pioneer1120.Pioneer1120State DevState;
        bool Listening = false;
        private string splitSequence = "\r\n";
        comms Comms;

        List<string> RspTypes;



        BackgroundWorker bw = new BackgroundWorker();

        // going to use this to put received strings in, will figure out what to do with them later
        Queue<string> Received;
        
        
        public Pioneer1120Engine(comms comms)
        {
            DevState = new Pioneer1120.Pioneer1120State();
            DevState.LastKnownFLStatus = string.Empty;
            RspTypes = Enum.GetNames(typeof(PioneerProtocol.ResponseMsgType)).ToList();
            Received = new Queue<string>();
            Comms = comms;
            SendData("VU");
            
            // create thread to listen and receive responses
            bw.DoWork += RX;
            bw.RunWorkerAsync();
            
        }


        public PioneerProtocol.Response.RSPMsg SendMessage(PioneerProtocol.Request.REQMsg msg)
        {
            SendData(msg.GetMsgAsString());
            return null;
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
                if (s.StartsWith("FL"))
                {
                    DevState.LastKnownFLStatus = s;
                }
                if (s.Length == 0)
                {
                    // don't need blank lines throw it away
                }
                else
                {
                    Received.Enqueue(s);
                }
            }

            ProcessQueue();
        }

        /// <summary>
        /// Update devState with messages received.
        /// </summary>
        private void ProcessQueue()
        {
            
        }

        public PioneerProtocol.Response.RSPMsg GetRSPMessageType(string msg)
        {
            PioneerProtocol.Response.RSPMsg rspMsgType;
            PioneerProtocol.ResponseMsgType RspEnumType = PioneerProtocol.ResponseMsgType.UNKNOWN;
            foreach (string s in RspTypes)
            {
                
                if (msg.StartsWith(s))
                {
                    RspEnumType = (PioneerProtocol.ResponseMsgType)Enum.Parse(typeof(PioneerProtocol.ResponseMsgType), s);
                }
               
            }

            rspMsgType = EnumToRSPMsg(RspEnumType);

            return rspMsgType;
        }

        private PioneerProtocol.Response.RSPMsg EnumToRSPMsg(PioneerProtocol.ResponseMsgType enumType)
        {
            PioneerProtocol.Response.RSPMsg msgType;
            switch (enumType)
            {
                case PioneerProtocol.ResponseMsgType.VOL:
                    msgType = new PioneerProtocol.Response.VOL();
                    break;
                case PioneerProtocol.ResponseMsgType.PWR:
                    msgType = new PioneerProtocol.Response.PWR();
                    break;
                case PioneerProtocol.ResponseMsgType.FN:
                    msgType = new PioneerProtocol.Response.FN();
                    break;
                case PioneerProtocol.ResponseMsgType.MUT:
                    msgType = new PioneerProtocol.Response.MUT();
                    break;
                default:
                    msgType = null;
                    break;
            }
            return msgType;
        }
        
    }
}
