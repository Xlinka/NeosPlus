using Buttplug;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace NEOSPlus.Logix.NSFW
{
    [NodeName("Device Info")]
    [Category("LogiX/NSFW")]
    public class DeviceInfo : LogixNode
    {
        public readonly Input<ButtplugClientDevice> Device;

        public readonly Output<string> Name;
        public readonly Output<uint> Index; // Updated to uint
        public readonly Output<float> Battery;

        protected override void OnEvaluate()
        {
            var device = Device.EvaluateRaw();
            if (device == null)
            {
                Name.Value = "Unknown Device";
                Index.Value = 0; // Default value for uint is 0
                Battery.Value = -1;
            }
            else
            {
                Name.Value = device.Name;
                Index.Value = device.Index; // Updated to uint
                Battery.Value = device.SendBatteryLevelCmd().IsFaulted ? -1 : (float)device.SendBatteryLevelCmd().Result;
            }
        }
    }
}
