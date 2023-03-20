using System;
using System.Linq;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using NEOSPlus;
using System.Collections.Generic;

[Category("LogiX/Network/ART-NET")]
public class ArtNetChannelDataExtractor : LogixNode
{
    public readonly Input<byte[]> DMXData;
    public readonly Input<int> StartIndex;
    public readonly SyncList<Output<int>> ChannelData;
    public readonly Impulse Trigger;

    public readonly SyncList<Sync<int>> channelData = new SyncList<Sync<int>>();

    protected override void OnAwake()
    {
        ChannelData.ElementsAdded += ChannelData_Changed;
        ChannelData.ElementsRemoved += ChannelData_Changed;
    }

    private void ChannelData_Changed(SyncElementList<Output<int>> list, int startIndex, int count)
    {
        channelData.EnsureExactCount(list.Count);
    }

    protected override void OnAttach()
    {
        base.OnAttach();
        for (int i = 0; i < 2; i++)
        {
            ChannelData.Add().Value = 0;
        }
    }

    [ImpulseTarget]
    public void OnExtractChannelData()
    {
        byte[] dmxData = DMXData.Evaluate();
        int startIndex = StartIndex.Evaluate() - 1;

        if (dmxData == null)
        {
            UniLog.Log("DMX data is null");
            return;
        }

        if (startIndex < 0 || startIndex >= dmxData.Length)
        {
            UniLog.Log($"Start index {startIndex} is out of range");
            return;
        }

        for (int i = 0; i < ChannelData.Count; i++)
        {
            int channelIndex = startIndex + i;
            if (channelIndex < dmxData.Length)
            {
                int channelValue = dmxData[channelIndex];
                channelData[i].Value = channelValue;
            }
        }

        Trigger.Trigger();
    }

    protected override void OnEvaluate()
    {
        // Copy data from internal channelData list to the output ChannelData list
        for (int i = 0; i < ChannelData.Count; i++)
        {
            ChannelData[i].Value = channelData[i];
        }
    }

    protected override void OnGenerateVisual(Slot root) =>
            GenerateUI(root).GenerateListButtons(Add, Remove);

    [SyncMethod]
    private void Add(IButton button, ButtonEventData eventData)
    {
        ChannelData.Add().Value = 0;
        RefreshLogixBox();
    }

    [SyncMethod]
    private void Remove(IButton button, ButtonEventData eventData)
    {
        if (ChannelData.Count <= 2)
        {
            return;
        }
        ChannelData.RemoveAt(ChannelData.Count - 1);

        RefreshLogixBox();
    }
}
