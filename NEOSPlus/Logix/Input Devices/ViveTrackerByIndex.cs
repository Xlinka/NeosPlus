using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace NEOSPlus.Logix.Input_Devices
{
    [Category("LogiX/Input Devices")]
    public class ViveTrackerByIndex : TrackerBatteryBase
    {
        public readonly Input<int> TrackerIndex;

        internal override ViveTracker GetViveTracker()
        {
            var idx = TrackerIndex.Evaluate();
            if (idx < 0) return null;
            var trackers = InputInterface.GetDevices<ViveTracker>();
            if (idx >= trackers.Count) return null;
            return trackers[idx];
        }
    }
}
