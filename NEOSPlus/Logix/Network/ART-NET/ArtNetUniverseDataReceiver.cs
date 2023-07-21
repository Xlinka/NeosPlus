using FrooxEngine;
using FrooxEngine.LogiX;
using System;
using BaseX;

[Category(new string[] { "LogiX/Network/ART-NET" })]

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
       // UniLog.Log("Packet received. Data length: " + data.Length);

        if (IsValidArtNetPacket(data))
        {
           // UniLog.Log("Data is a valid Art-Net packet");

            int receivedUniverseID = ParseUniverseID(data);
           // UniLog.Log("Parsed Universe ID: " + receivedUniverseID);

            if (receivedUniverseID == UniverseID.Evaluate())
            {
                RunSynchronously(delegate
                {
                    byte[] dmxData = ExtractDMXData(data);
                    Data.Value = dmxData;
                    Received.Trigger();
                });
            }
        }
        else if (IsValidDMXPacket(data))
        {
            UniLog.Log("Data is a valid DMX packet");

            RunSynchronously(delegate
            {
                byte[] dmxData = ExtractDMXData(data);
                Data.Value = dmxData;
                Received.Trigger();
            });
        }
        else
        {
            UniLog.Log("Received data is not a valid Art-Net or DMX packet.");
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
        // According to the Art-Net protocol, the Universe ID is stored as a 16-bit integer (2 bytes) with the Low Byte at offset 14 and High Byte at offset 15.
        int universeIDOffsetLowByte = 14;
        int universeIDOffsetHighByte = 15;

        int universeID = (data[universeIDOffsetHighByte] << 8) | data[universeIDOffsetLowByte];

        // Additional logging details
        byte subuni = data[14];
        string subnet = Convert.ToString(subuni >> 4);
        string universe = Convert.ToString(subuni & 0x0F);

        //UniLog.Log("Subnet: " + subnet);
        //UniLog.Log("Universe: " + universe);

        return universeID;
    }

    private byte[] ExtractDMXData(byte[] data)
    {
        int dmxDataOffset = 18;
        int dmxDataLength = 512; // fixed size of DMX data

        byte[] dmxData = new byte[dmxDataLength];
        Array.Copy(data, dmxDataOffset, dmxData, 0, dmxDataLength);

        string dmx = BitConverter.ToString(dmxData);
        UniLog.Log("DMX Data: " + dmx);

        return dmxData;
    }
}
