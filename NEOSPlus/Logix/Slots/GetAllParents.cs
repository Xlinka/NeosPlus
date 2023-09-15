using FrooxEngine;
using FrooxEngine.LogiX;
using System.Collections.Generic;

namespace FrooxEngine.LogiX.Slots;

[Category("LogiX/NeosPlus/Slots")]
[NodeName("Get All Parents")]
public class GetAllParents : LogixNode
{
    public readonly Input<Slot> Instance;

    public readonly Output<List<Slot>> FoundParents;

    protected override void OnEvaluate()
    {
        Slot slot = Instance.Evaluate();
        if (slot != null)
        {
            List<Slot> list = new List<Slot>();
            slot.GetAllParents(list);
            FoundParents.Value = list;
        }
        else
        {
            FoundParents.Value = null;
        }
    }
}