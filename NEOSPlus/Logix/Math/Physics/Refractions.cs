using System;
using BaseX;

namespace FrooxEngine.LogiX.Math;

[NodeName("Refraction Calculation")]
[Category(new string[] { "LogiX/Math/Physics" })]
internal class RefractionNode : LogixNode
{
    public readonly Input<float> RefractiveIndex1;  // Refractive index of medium 1
    public readonly Input<float> RefractiveIndex2;  // Refractive index of medium 2
    public readonly Input<float> AngleOfIncidence;  // Angle of incidence in degrees

    public readonly Output<float> AngleOfRefraction;  // Angle of refraction in degrees

    protected override void OnEvaluate()
    {
        float n1 = RefractiveIndex1.EvaluateRaw();
        float n2 = RefractiveIndex2.EvaluateRaw();
        float theta1Rad = AngleOfIncidence.EvaluateRaw() * (float)MathX.PI / 180.0f;  // Convert angle to radians 
        // for the love of god why does mathx not have a toradians function

        // Calculate using Snell's Law
        float sinTheta2 = n1 * (float)MathX.Sin(theta1Rad) / n2;

        // Ensure value is within [-1, 1] due to numerical inaccuracies
        sinTheta2 = MathX.Min(MathX.Max(sinTheta2, -1.0f), 1.0f);

        float theta2Rad = (float)MathX.Asin(sinTheta2);
        AngleOfRefraction.Value = theta2Rad * 180.0f / (float)MathX.PI;  // Convert angle back to degrees
    }
}
