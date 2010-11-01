using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChiefMarleyController
{
    public class PioneerProtocol
    {
        public enum ResponseMsgType
        {
            /// <summary>Power</summary>
            PWR,
           /// <summary>Volume</summary>
            VOL,
            /// <summary>Mute</summary>
            MUT,
            /// <summary>Input Source</summary>
            FN,
            /// <summary>Listening Mode Set</summary>
            SR,
            /// <summary>Listening Mode</summary>
            LM,
            /// <summary>Speakers</summary>
            SPK,
            /// <summary>HDMI Output Select</summary>
            HO,
            /// <summary>SBch Processing</summary>
            EX,
            /// <summary>MCACC Memory</summary>
            MC,
            /// <summary>Phase Control</summary>
            IS,
            /// <summary>Tone</summary>
            TO,
            /// <summary>Bass</summary>
            BA,
            /// <summary>Treble</summary>
            TA,
            /// <summary>HDMI Audio</summary>
            HA,
            /// <summary>Tuner Preset</summary>
            PR,
            /// <summary>Tuner Frequency</summary>
            FR,
            /// <summary>XM Channel</summary>
            XM,
            /// <summary>Sirius Channel</summary>
            SIR,
            /// <summary>Zone 2 Power</summary>
            APR,
            /// <summary>Zone 3 Power</summary>
            BPR,
            /// <summary>Zone 2 Volume</summary>
            ZV,
            /// <summary>Zone 3 Volume</summary>
            YV,
            /// <summary>Zone 2 Mute</summary>
            Z2MUT,
            /// <summary>Zone 3 Mute</summary>
            Z3MUT,
            /// <summary>Zone 3 Input</summary>
            Z2F,
            /// <summary>Zone 3 Input</summary>
            Z3F,
            /// <summary>PQLS</summary>
            PQ,
            /// <summary>CH Level</summary>
            CLV,
            /// <summary>Virtual SB</summary>
            VSB,
            /// <summary>Virtual Height</summary>
            VHT,
            /// <summary>FL Display Information</summary>
            FL,
            /// <summary>Input Name Information</summary>
            RGB,
            /// <summary>Unknown</summary>
            UNKNOWN
        }

        public enum InputType
        {
            PHONO = 0,
            CD = 1,
            TUNER = 2,
            CDR_TAPE = 3,
            DVD = 4,
            TV_SAT = 5,
            VIDEO_1 = 10,
            VIDEO_2 = 14,
            DVR_BDR = 15,
            IPOD_USB = 17,
            XM_RADIO = 18,
            HDMI_1 = 19,
            HDMI_2 = 20,
            HDMI_3 = 21,
            HDMI_4 = 22,
            HDMI_5 = 23,
            BD = 25,
            HOME_MEDIA_GALLERY = 26,
            SIRIUS = 27,
            HDMI_CYCLIC = 31,
            ADAPTER_PORT = 33
        }

        public enum MuteState
        {
            MUTED = 0,
            UNMUTED
        }

        public class VolumeState
        {
            public const byte MIN_VOL = 0;
            public const byte MAX_VOL = 185;
            /// <summary>
            /// Volume 0-185
            /// </summary>
            public byte VolumeLvl { get; set; }
            /// <summary>
            /// volume in dB
            /// </summary>
            public double VolDB
            {
                get
                {
                    return VolumeLvlToDb(VolumeLvl);
                }
            }

            /// <summary>
            /// returns values in .5dB increments
            /// </summary>
            /// <param name="VolLvl">0-185</param>
            /// <returns></returns>
            public static double VolumeLvlToDb(byte VolLvl)
            {
                if (VolLvl > MAX_VOL || VolLvl < MIN_VOL)
                {
                    throw new Exception("Attempted to set volume outside of accepted range: " + MIN_VOL + " <-> " + MAX_VOL);
                }
                double FromMinimum = (double)(VolLvl) / 2;
                return -80 + FromMinimum;
            }
        }

        
        public class Request
        {

            public abstract class REQMsg
            {
                /// <summary>
                /// String Received
                /// </summary>
                protected string Msg;

                protected ResponseMsgType ResponseType;

                public string GetMsgAsString()
                {
                    return Msg;
                }
            }

            public class Power : REQMsg
            {

                /// <summary>
                /// Power On
                /// </summary>
                public class PowerOn : Power
                {
                    public PowerOn()
                    {
                        Msg = "PO";
                        ResponseType = ResponseMsgType.PWR;
                    }
                }

                /// <summary>
                /// Power Off
                /// </summary>
                public class PowerOff : Power
                {
                    public PowerOff()
                    {
                        Msg = "PF";
                    }
                }

                /// <summary>
                /// Request Power Status
                /// </summary>
                public class RequestPowerStatus : Power
                {
                    public RequestPowerStatus()
                    {
                        Msg = "?P";
                    }
                }
            }

            public class Volume : REQMsg
            {
                public class VolumeUp : Volume
                {
                    public VolumeUp()
                    {
                        Msg = "VU";
                    }
                }

                public class VolumeDown : Volume
                {
                    public VolumeDown()
                    {
                        Msg = "VD";
                    }
                }

                public class VolumeSet : Volume
                {
                    const byte MAX_VOL_LVL = 185;
                    public VolumeSet(byte volume)
                    {
                        if (volume > MAX_VOL_LVL)
                        {
                            throw new Exception("Volume must be between 0 and " + MAX_VOL_LVL);
                        }
                        else
                        {
                            Msg = volume.ToString().PadLeft(3,'0') + "VL";
                        }
                    }
                }

                public class RequestVolumeLvl : Volume
                {
                    public RequestVolumeLvl()
                    {
                        Msg = "?V";
                    }
                }
            }

            public class Mute : REQMsg
            {

                /// <summary>
                /// Mute On
                /// </summary>
                public class MuteOn : Mute
                {
                    public MuteOn()
                    {
                        Msg = "MO";
                        ResponseType = ResponseMsgType.MUT;
                    }
                }

                /// <summary>
                /// Mute Off
                /// </summary>
                public class MuteOff : Mute
                {
                    public MuteOff()
                    {
                        Msg = "MF";
                    }
                }

                /// <summary>
                /// Request Mute Status
                /// </summary>
                public class RequestMuteStatus : Mute
                {
                    public RequestMuteStatus()
                    {
                        Msg = "?M";
                    }
                }
            }

            public class Input : REQMsg
            {
               

                /// <summary>
                /// Change Input
                /// </summary>
                public class ChangeInput : Input
                {
                    public ChangeInput(InputType type)
                    {
                        
                        int inputType = (int)type;
                        Msg = inputType.ToString().PadLeft(2, '0') + "FN";
                    }
                }

                public class ChangeInputCyclic :Input
                {
                    public ChangeInputCyclic()
                    {
                        Msg = "FU";
                    }
                }

                public class ChangeInputReverse : Input
                {
                    public ChangeInputReverse()
                    {
                        Msg = "FD";
                    }
                }

                public class RequestInputChange : Input
                {
                    public RequestInputChange()
                    {
                        Msg = "?F";
                    }
                }
            }

            /// tone control, amp function, tuner

        }

        public class Response
        {
            public abstract class RSPMsg
            {
               
                protected string Identifier;
                protected string Message;

                public abstract void Decode(string msg);

            }

            public class PWR : RSPMsg
            {

                public enum PowerState
                {
                    ON = 0,
                    OFF
                }
                public PowerState pwrState{get;private set;}
                public PWR(string msg)
                    :this()
                {
                   
                    Message = msg;
                    Decode(Message);
                }

                public PWR()
                {
                    Identifier = "PWR";
                }



                public override void Decode(string msg)
                {
                    Message = msg;
                    if (Message.StartsWith(Identifier))
                    {
                        string s = Message.Replace(Identifier, "");
                        pwrState = (PowerState)int.Parse(s);
                    }
                    else
                    {
                        throw new Exception("Invalid message format, expected " + Identifier + " Received " + Message);
                    }
                }
                
            }

            public class VOL : RSPMsg
            {
                public VolumeState Volume { get; private set; }

                public VOL(string msg)
                    :this()
                {
                    
                    Message = msg;
                    Decode(Message);

                }

           

                public VOL()
                {
                    Identifier = "VOL";
                    Volume = new VolumeState();
                }

                public override void Decode(string msg)
                {
                    Message = msg;
                    if (Message.StartsWith(Identifier))
                    {
                        // remove identifier, rest should be volume
                        string s = Message.Replace(Identifier, "");
                        Volume.VolumeLvl = byte.Parse(s);
                    }
                    else
                    {
                        throw new Exception("Invalid message format, expected " + Identifier + " Received " + Message);
                    }
                }

               
            }

            public class MUT : RSPMsg
            {

                
                public MuteState MuteState { get; private set; }

                public MUT(string msg)
                    :this()
                {
                    
                    Message = msg;
                    Decode(Message);
                }

                public MUT()
                {
                    Identifier = "MUT";
                }

                public override void Decode(string msg)
                {
                    Message = msg;
                    if (Message.StartsWith(Identifier))
                    {
                        string s = Message.Replace(Identifier, "");
                        MuteState = (MuteState)int.Parse(s);
                    }
                    else
                    {
                        throw new Exception("Invalid message format, expected " + Identifier + " Received " + Message);
                    }
                }

            }

            public class UNKNOWN : RSPMsg
            {
                public UNKNOWN(string msg)
                    :this()
                {

                }
                public UNKNOWN()
                {
                }
                public override void Decode(string msg)
                {
                    Message = msg;
                }

            }

            public class FN : RSPMsg
            {

                public enum MuteState
                {
                    ON = 0,
                    OFF
                }
                public InputType SelectedInput { get; private set; }

                public FN(string msg)
                    :this()
                {
                    
                    Message = msg;
                    Decode(Message);
                }

                public FN()
                {
                    Identifier = "FN";
                }

                public override void Decode(string msg)
                {
                    Message = msg;
                    if (Message.StartsWith(Identifier))
                    {
                        string s = Message.Replace(Identifier, "");
                        SelectedInput = (InputType)int.Parse(s);
                    }
                    else
                    {
                        throw new Exception("Invalid message format, expected " + Identifier + " Received " + Message);
                    }
                }

            }
        }
    }
}
