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
    public readonly Input<BodyNode> BodyNode;
    public readonly Output<float> BatteryLevel;
    public readonly Output<bool> IsBatteryCharging;

    private ViveTracker _lastTracker;
    protected readonly SyncRef<ValueStream<float>> _batteryLevelStream;
    protected readonly SyncRef<ValueStream<bool>> _batteryChargingStream;

    protected override void OnCommonUpdate()
    {
        base.OnCommonUpdate();
        User user = User.Evaluate();
        var bodyNode = BodyNode.Evaluate();
        if (user == base.LocalUser)
        {
            ViveTracker device = InputInterface.GetDevices<ViveTracker>().Find((t) => t.CorrespondingBodyNode == bodyNode);
            if (device != _lastTracker)
            {
                _batteryLevelStream.Target = device?.BatteryLevel.GetStream(base.World);
                _batteryChargingStream.Target = device?.BatteryCharging.GetStream(base.World);
                _lastTracker = device;
            }
        }
        else
        {
            _lastTracker = null;
            if (base.World.IsAuthority && user == null)
            {
                _batteryLevelStream.Target = null;
                _batteryChargingStream.Target = null;
            }
        }
    }

    protected override void OnEvaluate()
    {
        BatteryLevel.Value = _batteryLevelStream.Target?.Value ?? (-1f);
        IsBatteryCharging.Value = _batteryChargingStream.Target?.Value ?? false;
    }
}
