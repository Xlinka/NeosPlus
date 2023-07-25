using FrooxEngine.LogiX;
using FrooxEngine;
using Buttplug;

namespace NEOSPlus.Logix.NSFW
{
    [NodeName("Stop Vibration")]
    [Category("LogiX/NSFW")]
    public class StopVibration : LogixNode
    {
        public readonly Impulse Impulse;
        public readonly Input<ButtplugClientDevice> Device;

        [ImpulseTarget]
        public void Trigger()
        {
            var device = Device.EvaluateRaw();

            if (device != null)
            {
                device.SendStopDeviceCmd();
            }
            Impulse.Trigger();
        }
    }
}
