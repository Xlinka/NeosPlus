using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

[Category("LogiX/NeosPlus/Math/Random")]
[NodeName("Random Euler Angles")]
public class RandomEulerAngles : LogixNode
{
    public readonly Input<float> minPitch;
    public readonly Input<float> maxPitch;
    public readonly Input<float> minYaw;
    public readonly Input<float> maxYaw;
    public readonly Input<float> minRoll;
    public readonly Input<float> maxRoll;
    public readonly Output<float3> euler;

    protected override void OnEvaluate()
    {
        float pitch = RandomX.Range(minPitch.Evaluate(), maxPitch.Evaluate());
        float yaw = RandomX.Range(minYaw.Evaluate(), maxYaw.Evaluate());
        float roll = RandomX.Range(minRoll.Evaluate(), maxRoll.Evaluate());

        euler.Value = new float3(pitch, yaw, roll);
    }
}
