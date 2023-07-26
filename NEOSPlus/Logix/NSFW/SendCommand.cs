using Buttplug;
using FrooxEngine;
using FrooxEngine.LogiX;
using System;
using System.Threading.Tasks;
using BaseX;

namespace NEOSPlus.Logix.NSFW
{
    [NodeName("Send Command")]
    [Category("LogiX/NSFW")]
    public class SendCommand : LogixNode
    {
        public readonly Impulse Impulse;
        public readonly Input<ButtplugClientDevice> Device;
        public readonly Input<ButtplugDeviceCommand> Command;
        public readonly Input<float> Intensity;
        public readonly Input<uint> Duration;
        public readonly Input<bool> Clockwise;

        [ImpulseTarget]
        public void Trigger()
        {
            var device = Device.EvaluateRaw();
            var command = Command.EvaluateRaw();
            var intensity = Intensity.EvaluateRaw();
            var duration = Duration.EvaluateRaw();
            var clockwise = Clockwise.EvaluateRaw();

            if (device == null)
            {
                UniLog.Log("Device is null. Unable to send command.");
                return;
            }

            Task.Run(async () =>
            {
                try
                {
                    switch (command)
                    {
                        case ButtplugDeviceCommand.Vibrate:
                            await device.SendVibrateCmd(intensity);
                            UniLog.Log($"Sent Vibrate command with intensity: {intensity}");
                            break;

                        case ButtplugDeviceCommand.Stop:
                            await device.SendStopDeviceCmd();
                            UniLog.Log("Sent Stop command");
                            break;

                        case ButtplugDeviceCommand.Rotate:
                            await device.SendRotateCmd(intensity, clockwise);
                            UniLog.Log($"Sent Rotate command with intensity: {intensity}, clockwise: {clockwise}");
                            break;

                        case ButtplugDeviceCommand.Linear:
                            await device.SendLinearCmd(duration, intensity);
                            UniLog.Log($"Sent Linear command with duration: {duration}, position: {intensity}");
                            break;

                        default:
                            throw new ArgumentException("Invalid command");
                    }
                }
                catch (Exception ex)
                {
                    UniLog.Log($"Error occurred while sending command: {ex.Message}");
                    UniLog.Log(ex.ToString());  // More detailed logging
                }
            });

            Impulse.Trigger();
        }
    }

    public enum ButtplugDeviceCommand
    {
        Vibrate,
        Stop,
        Rotate,
        Linear,
        // Add other commands based on Buttplug.io pecification
    }
}
