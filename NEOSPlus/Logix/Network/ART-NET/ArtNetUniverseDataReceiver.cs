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
        if (IsValidArtNetPacket(data))
        {
            int receivedUniverseID = ParseUniverseID(data);
            if (receivedUniverseID == UniverseID.Evaluate())
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
        else if (IsValidDMXPacket(data))
        {
            RunSynchronously(delegate
            {
                byte[] dmxData = ExtractDMXData(data);
                Data.Value = dmxData;
                Received.Trigger();
                Data.Value = null;
            });
        }
        else
        {
            UniLog.Log("Received data is not a valid Art-Net or DMX packet.");
        }
    }

    private bool IsValidArtNetPacket(byte[] data)
    {
        // Check if the data starts with the Art-Net packet header (which is "Art-Net" followed by null-termination).
        return data.Length >= 8 && System.Text.Encoding.ASCII.GetString(data, 0, 7) == "Art-Net";
    }

    private bool IsValidDMXPacket(byte[] data)
    {
        // According to the DMX protocol, a valid DMX packet must have a start code of 0.
        return data.Length >= 1 && data[0] == 0;
    }

    private int ParseUniverseID(byte[] data)
    {
        // According to the Art-Net protocol, the Universe ID is stored as a 16-bit integer (2 bytes) with the Low Byte at offset 14 and High Byte at offset 15.
        int universeIDOffsetLowByte = 14;
        int universeIDOffsetHighByte = 15;

        int universeID = (data[universeIDOffsetHighByte] << 8) | data[universeIDOffsetLowByte];
        return universeID;
    }

    private byte[] ExtractDMXData(byte[] data)
    {
        // According to the Art-Net and DMX protocols, the DMX data starts at offset 18.
        int dmxDataOffset = 18;
        int dmxDataLength = data.Length - dmxDataOffset;

        byte[] dmxData = new byte[dmxDataLength];
        Array.Copy(data, dmxDataOffset, dmxData, 0, dmxDataLength);

        return dmxData;
    }
}
