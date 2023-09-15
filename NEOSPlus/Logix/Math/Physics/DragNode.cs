using System;
using BaseX;

namespace FrooxEngine.LogiX.Math;

[NodeName("Drag Calculation")]
[Category(new string[] { "LogiX/NeosPlus/Math/Physics" })]
internal class DragNode : LogixNode
{
    public readonly Input<float> FluidDensity;        // rho
    public readonly Input<float3> ObjectVelocity;     // v
    public readonly Input<float> DragCoefficient;     // Cd
    public readonly Input<float> CrossSectionalArea;  // A

    public readonly Output<float3> DragForce;

    protected override void OnEvaluate()
    {
        float rho = FluidDensity.EvaluateRaw();
        float3 v = ObjectVelocity.EvaluateRaw();
        float Cd = DragCoefficient.EvaluateRaw();
        float A = CrossSectionalArea.EvaluateRaw();

        float3 dragForce = 0.5f * rho * v * v * Cd * A; // Calculate the drag force
        DragForce.Value = -dragForce;  // Drag acts in the opposite direction of motion
    }
}
