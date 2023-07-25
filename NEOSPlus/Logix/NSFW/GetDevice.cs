using Buttplug;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace NEOSPlus.Logix.NSFW
{
    [NodeName("GetDevice")]
    [Category("LogiX/NSFW")]
    public class GetDevice : LogixNode
    {
        public readonly Input<ButtplugClient> Client;
        public readonly Input<int> Index;
        public readonly Output<ButtplugClientDevice> Device;

        protected override void OnEvaluate()
        {
            var client = Client.EvaluateRaw();

            if (client == null || Index.EvaluateRaw() < 0 || Index.EvaluateRaw() >= client.Devices.Length)
            {
                Device.Value = null;
            }
            else
            {
                Device.Value = client.Devices[Index.EvaluateRaw()];
            }
        }
    }
}
