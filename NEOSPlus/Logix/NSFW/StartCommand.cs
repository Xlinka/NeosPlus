using Buttplug;
using FrooxEngine;
using FrooxEngine.LogiX;
using System.Threading.Tasks;

namespace NEOSPlus.Logix.NSFW
{
    [NodeName("Start Vibration")]
    [Category("LogiX/NSFW")]
    public class StartVibration : LogixNode
    {
        public readonly Impulse Impulse;
        public readonly Input<ButtplugClientDevice> Device;
        public readonly Input<float> Intensity;

        [ImpulseTarget]
        public void Trigger()
        {
            var device = Device.EvaluateRaw();
            var intensity = Intensity.EvaluateRaw();

            if (device != null)
            {
                Task.Run(() => device.SendVibrateCmd(intensity));
            }
            Impulse.Trigger();
        }
    }
}
