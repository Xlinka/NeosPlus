using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using System.Linq;
using System.Collections.Generic;
using NEOSPlus.Logix.Input_Devices;

[Category("LogiX/Input Devices")]
public class ViveTrackerByBodyNode : TrackerBatteryBase
{
    public readonly Input<BodyNode> BodyNode;

    internal override ViveTracker GetViveTracker() => InputInterface.GetDevices<ViveTracker>().Find((t) => t.CorrespondingBodyNode == BodyNode.Evaluate());
}
