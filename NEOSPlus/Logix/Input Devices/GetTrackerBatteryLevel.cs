using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using System.Linq;
using System.Collections.Generic;

[Category("LogiX/Imput Devices")]
[NodeName("GetTrackerBatteryLevel")]
public class GetTrackerBatteryLevel : LogixNode
{
    public readonly Input<User> User;
    public readonly Input<BodyNode> TrackerIndex;
    public readonly Output<float> BatteryLevel;

    protected override void OnEvaluate()
    {
        User user = User.Evaluate();
        if (user != null)
        {
            var trackerIndex = TrackerIndex.Evaluate();
            ViveTracker tracker = user.InputInterface.GetDevices<ViveTracker>().Find((t) => t.CorrespondingBodyNode == trackerIndex);
            if (tracker != null)
            {
                BatteryLevel.Value = tracker.BatteryLevel.Value;
                return;
            }
        }

        BatteryLevel.Value = 0f;
    }
}
