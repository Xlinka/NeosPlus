using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.WorldModel;
using System.Collections.Generic;

namespace FrooxEngine.LogiX.Slots;

[Category("LogiX/NeosPlus/Slots")]
[NodeName("Get Parents With Tag")]
public class GetParentsWithTag : LogixNode
{
    public readonly Input<Slot> Instance;

    public readonly Input<string> Tag;

    public readonly Output<List<Slot>> FoundParents;

    private void InternalGetParentsWithTag(Slot s, List<Slot> slots, string tag)
    {
        if (s.Tag == tag)
        {
            slots.Add(s);
        }
        if (s.Parent == null)
        {
            return;
        }
        InternalGetParentsWithTag(s.Parent, slots, tag);
    }

    protected override void OnEvaluate()
    {
        Slot slot = Instance.Evaluate();
        string tag = Tag.Evaluate();
        if (slot != null && tag != null && slot.Parent != null)
        {
            List<Slot> list = new List<Slot>();
            InternalGetParentsWithTag(slot.Parent, list, tag);
            FoundParents.Value = list;
        }
        else
        {
            FoundParents.Value = null;
        }
    }
}