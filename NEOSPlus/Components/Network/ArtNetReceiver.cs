using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BaseX;
using FrooxEngine;

[Category("Network")]
public class ArtNetReceiver : Component
{
    public readonly Sync<Uri> URL;
    public readonly UserRef HandlingUser;
    public readonly Sync<string> AccessReason;
    public readonly Sync<float> ConnectRetryInterval;
    public readonly Sync<bool> IsConnected;

    private Uri _currentURL;
    private UdpClient _udpClient;

    public event Action<ArtNetReceiver> Connected;
    public event Action<ArtNetReceiver> Closed;
    public event Action<ArtNetReceiver, string> Error;
    public event Action<ArtNetReceiver, byte[]> PacketReceived;

    protected override void OnAwake()
    {
        base.OnAwake();
        ConnectRetryInterval.Value = 10f;
    }

    protected override void OnChanges()
    {
        Uri uri = (Enabled ? URL.Value : null);
        if (HandlingUser.Target != LocalUser)
        {
            uri = null;
        }
        if (uri != _currentURL)
        {
            _currentURL = uri;
            CloseCurrent();
            IsConnected.Value = false;
            if (_currentURL != null)
            {
                StartTask(async () =>
                {
                    await ConnectTo(_currentURL);
                });
            }
        }
    }

    private async Task ConnectTo(Uri target)
    {
        if (target.Scheme != "artnet")
        {
            throw new ArgumentException("Invalid URL scheme. Expected 'artnet://'.");
        }

        if (await Engine.Security.RequestAccessPermission(target.Host, target.Port, AccessReason.Value ?? "Art-Net Receiver") == HostAccessPermission.Allowed && target == _currentURL && !IsRemoved)
        {
            _udpClient = new UdpClient(target.Port);
            IsConnected.Value = true;
            Connected?.Invoke(this);
            StartTask(ReceiveLoop);
        }
    }

    private async Task ReceiveLoop()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        while (IsConnected.Value)
        {
            UdpReceiveResult result = await _udpClient.ReceiveAsync();
            byte[] receivedData = result.Buffer;

            PacketReceived?.Invoke(this, receivedData);
        }
    }

    protected override void OnDispose()
    {
        CloseCurrent();
        base.OnDispose();
    }

    private void CloseCurrent()
    {
        if (_udpClient != null)
        {
            UdpClient udpClient = _udpClient;
            _udpClient = null;
            try
            {
                Closed?.Invoke(this);
            }
            catch (Exception ex)
            {
                UniLog.Error($"Exception in running Closed event on ArtNetReceiver:\n{ex}");
            }
            udpClient.Close();
        }
    }
}