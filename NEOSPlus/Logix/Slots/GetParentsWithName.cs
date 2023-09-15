using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.WorldModel;
using System.Collections.Generic;

namespace FrooxEngine.LogiX.Slots;

[Category("LogiX/NeosPlus/Slots")]
[NodeName("Get Parents With Name")]
public class GetParentsWithName : LogixNode
{
    public readonly Input<Slot> Instance;

    public readonly Input<string> Name;

    public readonly Output<List<Slot>> FoundParents;

    private void InternalGetParentsWithName(Slot s, List<Slot> slots, string name)
    {
        if (s.Name == name)
        {
            slots.Add(s);
        }
        if (s.Parent == null)
        {
            return;
        }
        InternalGetParentsWithName(s.Parent, slots, name);
    }

    protected override void OnEvaluate()
    {
        Slot slot = Instance.Evaluate();
        string name = Name.Evaluate();
        if (slot != null && name != null && slot.Parent != null)
        {
            List<Slot> list = new List<Slot>();
            InternalGetParentsWithName(slot.Parent, list, name);
            FoundParents.Value = list;
        }
        else
        {
            FoundParents.Value = null;
        }
    }
}