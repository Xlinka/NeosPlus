using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using System.Linq;
using System.Collections.Generic;
using NEOSPlus.Logix.Input_Devices;

[Category("LogiX/Imput Devices")]
[NodeName("GetTrackerBatteryLevel")]
public class ViveTrackerByBodyNode : TrackerBatteryBase
{
    public readonly Input<BodyNode> BodyNode;

    internal override ViveTracker GetViveTracker()
    {
        var bodyNode = BodyNode.Evaluate();
        return InputInterface.GetDevices<ViveTracker>().Find((t) => t.CorrespondingBodyNode == bodyNode);
    }
}
