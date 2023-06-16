using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;

[Category("LogiX/Users")]
[NodeName("IsUserEyeTracking")]
public class IsUserEyeTracking : LogixNode
{
    public readonly Input<User> User;
    public readonly Input<EyeSide> Side;
    public readonly Output<bool> EyeTracking;

    private EyeTrackingStreamManager eyeTrackingStreamManager;

    protected override void OnAttach()
    {
        eyeTrackingStreamManager = base.LocalUserRoot.GetRegisteredComponent<EyeTrackingStreamManager>();
    }

    protected override void OnEvaluate()
    {
        User user = User.Evaluate();
        if (user != null)
        {
            EyeSide side = Side.Evaluate(); // Get the EyeSide value from the input
            bool eyeTracking = eyeTrackingStreamManager.GetIsTracking(side);
            EyeTracking.Value = eyeTracking;
        }
        else
        {
            EyeTracking.Value = false;
        }
    }
}
