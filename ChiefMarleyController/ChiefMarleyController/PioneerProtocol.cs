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
            RGB
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

        
        public class Request
        {

            public abstract class REQMsg
            {
                /// <summary>
                /// String Received
                /// </summary>
                protected string Msg;

                protected ResponseMsgType ResponseType;
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
                            Msg = volume.ToString().PadLeft(3) + "VL";
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

                public abstract void Decode(); 
            }

            public class PWR : RSPMsg
            {

                public enum PowerState
                {
                    ON = 0,
                    OFF
                }
                private PowerState pwrState;
                public PWR(string msg)
                {
                    Identifier = "PWR";
                    Message = msg;
                    Decode();
                }

                public override void Decode()
                {
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
                byte VolumeLvl;
                double VolDB
                {
                    get
                    {
                        return VolumeLvlToDb(VolumeLvl);
                    }
                }

                public VOL(string msg)
                {
                    Identifier = "VOL";
                    Message = msg;
                    Decode();

                }

                public override void Decode()
                {
                    if (Message.StartsWith(Identifier))
                    {
                        // remove identifier, rest should be volume
                        string s = Message.Replace(Identifier, "");
                        VolumeLvl = byte.Parse(s);
                    }
                    else
                    {
                        throw new Exception("Invalid message format, expected " + Identifier + " Received " + Message);
                    }
                }

                /// <summary>
                /// returns values in .5dB increments
                /// </summary>
                /// <param name="VolLvl">0-185</param>
                /// <returns></returns>
                public double VolumeLvlToDb(byte VolLvl)
                {
                    double FromMinimum = (double)(VolLvl) / 2;
                    return -80 + FromMinimum;
                }
            }

            public class MUT : RSPMsg
            {

                public enum MuteState
                {
                    ON = 0,
                    OFF
                }
                public MuteState PwrState { get; private set; }
                public MUT(string msg)
                {
                    Identifier = "MUT";
                    Message = msg;
                    Decode();
                }

                public override void Decode()
                {
                    if (Message.StartsWith(Identifier))
                    {
                        string s = Message.Replace(Identifier, "");
                        PwrState = (MuteState)int.Parse(s);
                    }
                    else
                    {
                        throw new Exception("Invalid message format, expected " + Identifier + " Received " + Message);
                    }
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
                {
                    Identifier = "FN";
                    Message = msg;
                    Decode();
                }

                public override void Decode()
                {
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
