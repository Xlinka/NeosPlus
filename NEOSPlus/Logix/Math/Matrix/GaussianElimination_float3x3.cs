using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using Mehroz;

namespace FrooxEngine.LogiX.Math
{
    [HiddenNode] // Hide overload from node browser
    [NodeName("Reduced Echelon Form")]
    [NodeOverload("Reduced-Echelon-Form")]
    [Category(new string[] {"LogiX/NeosPlus/Math/Matrix"})]
    public sealed class GaussianElimination : LogixOperator<float3>
    {
        public readonly Input<float3x3> LinearEquationMatrix;
        public readonly Input<float3> LinearSolutionMatrix;

        public override float3 Content
        {
            get
            {
                Matrix m1 = new Matrix(((double3x3) LinearEquationMatrix.EvaluateRaw()).To2DArray());
                Matrix m2 = new Matrix(3, 1);
                m2[0, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().x);
                m2[1, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().y);
                m2[2, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().z);
                Matrix m3 = Matrix.Concatenate(m1, m2);
                m3 = m3.EchelonForm();
                // m3.Rows should be 2
                return new float3((float) m3[0, m3.Rows].ToDouble(), (float) m3[1, m3.Rows].ToDouble(),
                    (float) m3[2, m3.Rows].ToDouble());
            }
        }
    }
}