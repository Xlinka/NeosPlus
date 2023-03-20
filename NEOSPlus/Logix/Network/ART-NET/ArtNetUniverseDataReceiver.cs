using System;
using FrooxEngine;
using FrooxEngine.LogiX;

public class ArtNetUniverseDataReceiver : ArtNetReceiverBaseNode
{
    public readonly Input<int> UniverseID;
    public readonly Impulse Received;
    public readonly Output<string> Data; // Changed the output type to string

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
        int receivedUniverseID = ParseUniverseID(data);
        if (receivedUniverseID == UniverseID.Evaluate())
        {
            RunSynchronously(delegate
            {
                byte[] dmxData = ExtractDMXData(data);
                Data.Value = BitConverter.ToString(dmxData).Replace("-", string.Empty); // Changed to assign the hex data string
                Received.Trigger();
                Data.Value = null;
            });
        }
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
        // According to the Art-Net protocol, the DMX data starts at offset 18.
        int dmxDataOffset = 18;
        int dmxDataLength = data.Length - dmxDataOffset;

        byte[] dmxData = new byte[dmxDataLength];
        Array.Copy(data, dmxDataOffset, dmxData, 0, dmxDataLength);

        return dmxData;
    }
}
