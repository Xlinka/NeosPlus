using FrooxEngine;
using FrooxEngine.LogiX;
using System.Collections.Generic;

[Category("LogiX/Slots")]
[NodeName("Get Children With Tag")]
public class GetChildrenWithTag : LogixNode
{
    public readonly Input<Slot> Instance;

    public readonly Input<string> Tag;

    public readonly Output<List<Slot>> FoundChildren;

    protected override void OnEvaluate()
    {
        Slot slot = Instance.Evaluate();
        if (slot != null)
        {
            string tag = Tag.Evaluate();
            FoundChildren.Value = slot.GetChildrenWithTag(tag);
        }
        else
        {
            FoundChildren.Value = null;
        }
    }
}