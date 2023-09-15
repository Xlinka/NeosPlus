using System;
using BaseX;

namespace FrooxEngine.LogiX.Math.Physics;

[NodeName("Centripetal Force Calculation")]
[Category(new string[] { "LogiX/NeosPlus/Math/Physics" })]
internal class CentripetalForceNode : LogixNode
{
    public readonly Input<float> Mass;           // Mass of the object
    public readonly Input<float> Velocity;       // Tangential velocity of the object
    public readonly Input<float> Radius;         // Radius of the circular path

    public readonly Output<float> CentripetalForce;  // Centripetal force acting on the object

    protected override void OnEvaluate()
    {
        float m = Mass.EvaluateRaw();
        float v = Velocity.EvaluateRaw();
        float r = Radius.EvaluateRaw();

        float force = (m * v * v) / r;
        
        CentripetalForce.Value = force;
    }
}

