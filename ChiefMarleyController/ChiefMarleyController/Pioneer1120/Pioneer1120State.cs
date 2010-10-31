using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChiefMarleyController.Pioneer1120
{
    public class Pioneer1120State
    {
        public Pioneer1120State()
        {
        }

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
                return PioneerProtocol.Response.VOL.VolumeLvlToDb(VolumeLvl);
            }
        }

        /// <summary>
        /// current input selected
        /// </summary>
        public PioneerProtocol.InputType InputState { get; set; }

        /// <summary>
        /// keeps track of last known state of display
        /// </summary>
        public string LastKnownFLStatus { get; set; }

        /// <summary>
        /// If device is on or off
        /// </summary>
        public PioneerProtocol.Response.PWR.PowerState PowerState { get; set; }
    }
}
