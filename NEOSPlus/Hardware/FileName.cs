using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using System.Linq;
using System.Collections.Generic;

[Category("LogiX/Users")]
[NodeName("GetTrackerBatteryLevel")]
public class GetTrackerBatteryLevel : LogixNode
{
    public readonly Input<User> User;
    public readonly Input<int> TrackerIndex;
    public readonly Output<float> BatteryLevel;

    protected override void OnEvaluate()
    {
        User user = User.Evaluate();
        int trackerIndex = TrackerIndex.Evaluate();

        if (user != null)
        {
            List<ViveTracker> trackers = user.InputInterface.GetDevices<ViveTracker>().ToList();
            if (trackerIndex >= 0 && trackerIndex < trackers.Count)
            {
                ViveTracker tracker = trackers[trackerIndex];
                BatteryLevel.Value = tracker.BatteryLevel.Value;
                return;
            }
        }

        BatteryLevel.Value = 0f;
    }
}
