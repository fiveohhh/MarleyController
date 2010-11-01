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

        public ChiefMarleyController.PioneerProtocol.VolumeState CurrentVolumeState { get; set; }

        public ChiefMarleyController.PioneerProtocol.MuteState MuteState { get; set; }

        /// <summary>
        /// current input selected
        /// </summary>
        public ChiefMarleyController.PioneerProtocol.InputType InputState { get; set; }

        /// <summary>
        /// keeps track of last known state of display
        /// </summary>
        public string LastKnownFLStatus { get; set; }

        /// <summary>
        /// If device is on or off
        /// </summary>
        public ChiefMarleyController.PioneerProtocol.Response.PWR.PowerState PowerState { get; set; }

    }

    public class ZoneState
    {
        public ChiefMarleyController.PioneerProtocol.MuteState MuteState { get; set; }
        public ChiefMarleyController.PioneerProtocol.VolumeState VolumeState { get; set; }
    }
}
