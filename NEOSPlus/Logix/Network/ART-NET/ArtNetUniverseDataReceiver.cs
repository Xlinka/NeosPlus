using FrooxEngine;
using FrooxEngine.LogiX;
using System;
using BaseX;

public class ArtNetUniverseDataReceiver : ArtNetReceiverBaseNode
{
    public readonly Input<int> UniverseID;
    public readonly Impulse Received;
    public readonly Output<byte[]> Data;

    protected override void Register(ArtNetReceiver receiver)
    {
        receiver.PacketReceived += OnArtNetPacketReceived;
    }

    protected override void Unregister(ArtNetReceiver receiver)
    {
        receiver.PacketReceived -= OnArtNetPacketReceived;
    }

    private void OnArtNetPacketReceived(ArtNetReceiver receiver, byte[] data)
    {
        int receivedUniverseID = -1;
        bool validPacket = false;

        if (IsValidArtNetPacket(data))
        {
            receivedUniverseID = ParseUniverseID(data);
            validPacket = receivedUniverseID == UniverseID.Evaluate();
        }
        else if (IsValidDMXPacket(data))
        {
            receivedUniverseID = UniverseID.Evaluate();
            validPacket = true;
        }
        else
        {
            UniLog.Log("Received data is not a valid Art-Net or DMX packet.");
        }

        if (validPacket)
        {
            RunSynchronously(delegate
            {
                byte[] dmxData = ExtractDMXData(data);
                Data.Value = dmxData;
                Received.Trigger();
                Data.Value = null;
            });
        }
    }

    private bool IsValidArtNetPacket(byte[] data)
    {
        return data.Length >= 8 && System.Text.Encoding.ASCII.GetString(data, 0, 7) == "Art-Net";
    }

    private bool IsValidDMXPacket(byte[] data)
    {
        return data.Length >= 1 && data[0] == 0;
    }

    private int ParseUniverseID(byte[] data)
    {
        int universeIDOffsetLowByte = 14;
        int universeIDOffsetHighByte = 15;

        int universeID = (data[universeIDOffsetHighByte] << 8) | data[universeIDOffsetLowByte];
        return universeID;
    }

    private byte[] ExtractDMXData(byte[] data)
    {
        int dmxDataOffset = 18;
        int dmxDataLength = data.Length - dmxDataOffset;

        byte[] dmxData = new byte[dmxDataLength];
        Array.Copy(data, dmxDataOffset, dmxData, 0, dmxDataLength);

        return dmxData;
    }
}
