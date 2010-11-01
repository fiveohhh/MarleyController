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

        public Pioneer1120.Pioneer1120State DevState;
        bool Listening = false;
        private string splitSequence = "\r\n";
        comms Comms;

        public event EventHandler DeviceStateUpdateFromDevice;
        List<string> RspTypes;

        public delegate void DevStateChangedDelegate(Pioneer1120.Pioneer1120State state);

        private DevStateChangedDelegate DevChangedDel;    

        public BackgroundWorker bw = new BackgroundWorker();

        // going to use this to put received strings in, will figure out what to do with them later
        Queue<string> Received;
        
        
        public Pioneer1120Engine(comms comms)
        {
            DevState = new Pioneer1120.Pioneer1120State();
            DevState.LastKnownFLStatus = string.Empty;
            RspTypes = Enum.GetNames(typeof(PioneerProtocol.ResponseMsgType)).ToList();
            Received = new Queue<string>();
            Comms = comms;
            //SendData("VU");
            
            // create thread to listen and receive responses
            bw.DoWork += RX;
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync();
            
        }

        /// <summary>
        /// set a delegate if you want it to execute when device state is changed
        /// </summary>
        /// <param name="d"></param>
        public void SetDevChangedDel(DevStateChangedDelegate d)
        {
            DevChangedDel = d;
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DevChangedDel(DevState);
        }


        /// <summary>
        /// Get updated State from device
        /// </summary>
        public void UpdateStatus()
        {
            SendData("?V");
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

           // Listening = true;
            while (true)
            {
                rxBuffer += Comms.Read();
                if (rxBuffer.Contains(splitSequence))
                {
                    ProcessRawRX(rxBuffer);
                    rxBuffer = string.Empty;
                }
            }
           // Listening = false;
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
                else if (s.Length == 0)
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
            while (Received.Count != 0)
            {
                string msg = Received.Dequeue();
                PioneerProtocol.Response.RSPMsg Msg = GetRSPMessage(msg);
                UpdateDevState(Msg);
            }

        }

        public PioneerProtocol.Response.RSPMsg GetRSPMessage(string msg)
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

            rspMsgType.Decode(msg);
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
                    msgType = new PioneerProtocol.Response.UNKNOWN();
                    break;
            }
            return msgType;
        }

        private void UpdateDevState(PioneerProtocol.Response.RSPMsg msg)
        {
            if (msg.GetType() == typeof(PioneerProtocol.Response.VOL))
            {
                DevState.CurrentVolumeState = ((PioneerProtocol.Response.VOL)msg).Volume;
            }
            else if (msg.GetType() == typeof(PioneerProtocol.Response.PWR))
            {
                DevState.PowerState = ((PioneerProtocol.Response.PWR)msg).pwrState;
            }
            else if (msg.GetType() == typeof(PioneerProtocol.Response.MUT))
            {
                DevState.MuteState = ((PioneerProtocol.Response.MUT)msg).MuteState;
            }
            else if (msg.GetType() == typeof(PioneerProtocol.Response.FN))
            {
                DevState.InputState = ((PioneerProtocol.Response.FN)msg).SelectedInput;
            }

            if (DeviceStateUpdateFromDevice != null)
            {
                DeviceStateUpdateFromDevice(DevState, null);
            }
        }
        
    }
}
