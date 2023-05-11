using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
//this is experimental not tested using a normal user.
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
            EyeSide side = EyeSide.Left; // or EyeSide.Right, depending on which eye you want to track
            bool eyeTracking = eyeTrackingStreamManager.GetIsTracking(side);
            EyeTracking.Value = eyeTracking;
        }
        else
        {
            EyeTracking.Value = false;
        }
    }
}
