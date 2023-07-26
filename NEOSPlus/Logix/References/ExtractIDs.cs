using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

[Category("LogiX/References")]
[NodeName("Extract IDs")]
public class ExtractIDs : LogixNode
{
	public readonly Input<RefID> RefID;
	public readonly Output<ulong> Position;
	public readonly Output<byte> User;

	protected override void OnEvaluate()
	{
		RefID refId = RefID.Evaluate();
		if (refId != null)
		{
			refId.ExtractIDs(out ulong position, out byte user);
			Position.Value = position;
			User.Value = user;
		}
		else
		{
			Position.Value = 0;
			User.Value = 0;
		}
	}
}