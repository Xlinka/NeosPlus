using System;
using System.Linq;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using NEOSPlus;

[Category("LogiX/Network/ART-NET")]
public class DMXChannelDataExtractor : LogixNode
{
    public readonly Input<string> DMXData;
    public readonly Input<int> StartIndex;
    public readonly Impulse ExtractChannelData;
    public readonly SyncList<Output<int>> ChannelOutputs;
    public readonly Impulse Trigger;

    protected override void OnAttach()
    {
        base.OnAttach();
        for (var i = 0; i < 2; i++)
        {
            ChannelOutputs.Add();
        }
    }

    [ImpulseTarget]
    public void OnExtractChannelData()
    {
        Trigger.Trigger();
    }

    protected override void OnEvaluate()
    {
        string dmxData = DMXData.Evaluate();
        int startIndex = StartIndex.Evaluate() - 1;

        if (!string.IsNullOrEmpty(dmxData) && startIndex >= 0)
        {
            byte[] dmxBytes = StringToByteArray(dmxData);

            for (var i = 0; i < ChannelOutputs.Count; i++)
            {
                int channelIndex = startIndex + i;
                if (channelIndex < dmxBytes.Length)
                {
                    ChannelOutputs[i].Value = dmxBytes[channelIndex];
                }
            }
        }
    }

    protected override void OnGenerateVisual(Slot root) =>
            GenerateUI(root).GenerateListButtons(Add, Remove);

    public static byte[] StringToByteArray(string hex)
    {
        int numberOfChars = hex.Length;
        byte[] bytes = new byte[numberOfChars / 2];
        for (int i = 0; i < numberOfChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }

    [SyncMethod]
    private void Add(IButton button, ButtonEventData eventData)
    {
        ChannelOutputs.Add();
        RefreshLogixBox();
    }

    [SyncMethod]
    private void Remove(IButton button, ButtonEventData eventData)
    {
        if (ChannelOutputs.Count <= 2) return;
        ChannelOutputs.RemoveAt(ChannelOutputs.Count - 1);
        RefreshLogixBox();
    }
}