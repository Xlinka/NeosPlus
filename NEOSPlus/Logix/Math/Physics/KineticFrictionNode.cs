using System;
using BaseX;

namespace FrooxEngine.LogiX.Math;

[NodeName("Kinetic Friction Calculation")]
[Category(new string[] { "LogiX/NeosPlus/Math/Physics" })]
internal class KineticFrictionNode : LogixNode
{
    public readonly Input<float3> NormalForce; // Assuming it's a 3D force, change as needed
    public readonly Input<float> KineticFrictionCoefficient;

    public readonly Output<float3> KineticFrictionalForce;

    protected override void OnEvaluate()
    {
        float3 normal = NormalForce.EvaluateRaw();
        float coefficient = KineticFrictionCoefficient.EvaluateRaw(0f); // Default to 0 if not provided

        // Kinetic friction formula: f_kinetic = mu_kinetic * N
        KineticFrictionalForce.Value = coefficient * normal;
    }
}
