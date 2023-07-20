using FrooxEngine;
using FrooxEngine.LogiX;
using System;

public class ArtNetChannelDataExtractor : LogixNode
{
    public readonly Input<byte[]> Data;
    public readonly Input<int> Channel;
    public readonly Input<int> StartIndex;
    public readonly Output<byte> Output;

    protected override void OnChanges()
    {
        base.OnChanges();

        byte[] data = Data.Evaluate();
        int channel = Channel.Evaluate();
        int startIndex = StartIndex.Evaluate();

        if (data != null && data.Length > startIndex + channel - 1)
        {
            Output.Value = data[startIndex + channel - 1];
        }
        else
        {
            Output.Value = 0;
        }
    }
}
