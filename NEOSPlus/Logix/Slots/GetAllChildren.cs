using FrooxEngine;
using FrooxEngine.LogiX;
using System.Collections.Generic;

namespace FrooxEngine.LogiX.Slots;

[Category("LogiX/NeosPlus/Slots")]
[NodeName("Get All Children")]
public class GetAllChildren : LogixNode
{
    public readonly Input<Slot> Instance;

    public readonly Output<List<Slot>> FoundChildren;

    protected override void OnEvaluate()
    {
        Slot slot = Instance.Evaluate();
        if (slot != null)
        {
            FoundChildren.Value = slot.GetAllChildren();
        }
        else
        {
            FoundChildren.Value = null;
        }
    }
}