using FrooxEngine;
using FrooxEngine.LogiX;
using BaseX;

[Category(new string[] { "LogiX/References" })]
public class ExtractIDs : LogixNode
{
    public readonly Input<RefID> RefID;

    public readonly Output<ulong> Position;

    public readonly Output<byte> User;

    protected override void OnEvaluate()
    {
        RefID refId = RefID.EvaluateRaw();
        if (refId == null)
        {
            Position.Value = 0;
            User.Value = 0;
            return;
        }
        refId.ExtractIDs(out var position, out var user);
        Position.Value = position;
        User.Value = user;
    }
}