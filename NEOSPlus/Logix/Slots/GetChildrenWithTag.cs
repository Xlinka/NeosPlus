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
		string tag = Tag.Evaluate();
		if (slot != null && tag != null)
		{
			FoundChildren.Value = slot.GetChildrenWithTag(tag);
		}
		else
		{
			FoundChildren.Value = null;
		}
	}
}