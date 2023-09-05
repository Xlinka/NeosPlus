using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.WorldModel;
using System.Collections.Generic;

namespace FrooxEngine.LogiX.Slots;

[Category("LogiX/Slots")]
[NodeName("Get Children With Name")]
public class GetChildrenWithName : LogixNode
{
    public readonly Input<Slot> Instance;

    public readonly Input<string> Name;

    public readonly Output<List<Slot>> FoundChildren;

    private void InternalGetChildrenWithName(Slot s, List<Slot> slots, string name)
    {
        if (s.Name == name)
        {
            slots.Add(s);
        }
        if (s.ChildrenCount <= 0)
        {
            return;
        }
        foreach (Slot child in s.Children)
        {
            InternalGetChildrenWithName(child, slots, name);
        }
        
    }

    protected override void OnEvaluate()
    {
        Slot slot = Instance.Evaluate();
        string name = Name.Evaluate();
        if (slot != null && name != null)
        {
            List<Slot> list = new List<Slot>();
            foreach (Slot child in slot.Children)
            {
                InternalGetChildrenWithName(child, list, name);
            }
            FoundChildren.Value = list;
        }
        else
        {
            FoundChildren.Value = null;
        }
    }
}