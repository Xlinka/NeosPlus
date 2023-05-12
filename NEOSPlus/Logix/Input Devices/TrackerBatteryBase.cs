using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace NEOSPlus.Logix.Input_Devices
{
    public abstract class TrackerBatteryBase : LogixNode
    {
        public readonly Input<User> User;
        public readonly Output<float> BatteryLevel;
        public readonly Output<bool> IsBatteryCharging;

        internal ViveTracker _lastTracker;
        protected readonly SyncRef<ValueStream<float>> _batteryLevelStream;
        protected readonly SyncRef<ValueStream<bool>> _batteryChargingStream;

        internal abstract ViveTracker GetViveTracker();

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            User user = User.Evaluate();
            if (user == base.LocalUser)
            {
                ViveTracker device = GetViveTracker();
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
}
