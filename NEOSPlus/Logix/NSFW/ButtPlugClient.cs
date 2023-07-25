using FrooxEngine;
using FrooxEngine.LogiX;
using Buttplug;
using BaseX;
using System;
using NEOSPlus.Hardware;

namespace NEOSPlus.Logix.NSFW
{
    [NodeName("ButtPlug Client")]
    [Category("LogiX/NSFW")]
    public class ButtPlugClient : LogixNode
    {
        public readonly Impulse OnConnect;
        public readonly Impulse OnDisconnect;
        public readonly Output<ButtplugClient> ButtClient;
        public readonly Output<bool> Connected;
        public readonly Output<int> DeviceCount;

        private ButtplugClient _client;

        [ImpulseTarget]
        public void Initialize()
        {
            try
            {
                if (_client != null) return;

                _client = new ButtplugClient("ButtPlugClient");
                _client.ConnectAsync(ButtplugIntegration.connector);
                _client.StartScanningAsync();

                _client.DeviceAdded += HandleDeviceAdded;
                _client.DeviceRemoved += HandleDeviceRemoved;
                _client.ErrorReceived += Client_ErrorReceived;

                ButtClient.Value = _client;
                Connected.Value = _client.Connected;

                OnConnect.Trigger();
            }
            catch (Exception ex)
            {
                UniLog.Log($"Error occurred: {ex}");
                StopAllServices();
                OnDisconnect.Trigger();
                return;
            }
        }

        [ImpulseTarget]
        public void Disconnect()
        {
            StopAllServices();
            OnDisconnect.Trigger();
        }

        protected override void OnEvaluate()
        {
            base.OnEvaluate();
            ButtClient.Value = _client ?? null;
            Connected.Value = _client?.Connected ?? false;
            DeviceCount.Value = _client?.Devices.Length ?? 0;

            MarkChangeDirty();
        }

        public void StopAllServices()
        {
            // Dispose all devices
            if (_client != null)
            {
                // Stop devices
                if (_client.Devices.Length > 0) _client.StopAllDevicesAsync();

                // Stop the scanning
                if (_client.IsScanning) _client.StopScanningAsync();

                _client.DisconnectAsync();
                _client.Dispose();
            }
            _client = null;
            ButtClient.Value = null;
            Connected.Value = false;
        }

        private void HandleDeviceAdded(object sender, DeviceAddedEventArgs e)
        {
            UniLog.Log($"Device added: {e.Device.Name}");
        }

        private void HandleDeviceRemoved(object sender, DeviceRemovedEventArgs e)
        {
            UniLog.Log($"Device removed: {e.Device.Name}");
        }

        private void Client_ErrorReceived(object sender, ButtplugExceptionEventArgs e)
        {
            UniLog.Log($"Error Received: {e}");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StopAllServices();
        }
    }
}
