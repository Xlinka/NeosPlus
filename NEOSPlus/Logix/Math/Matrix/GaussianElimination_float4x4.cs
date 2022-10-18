using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using Mehroz;

namespace FrooxEngine.LogiX.Math
{
    [HiddenNode] // Hide overload from node browser
    [NodeName("Reduced Echelon Form")]
    [NodeOverload("Reduced-Echelon-Form")]
    [Category(new string[] {"LogiX/Math/Matrix"})]
    public sealed class GaussianElimination_float4x4 : LogixOperator<float4>
    {
        public readonly Input<float4x4> LinearEquationMatrix;
        public readonly Input<float4> LinearSolutionMatrix;

        public override float4 Content
        {
            get
            {
                Matrix m1 = new Matrix(((double4x4) LinearEquationMatrix.EvaluateRaw()).To2DArray());
                Matrix m2 = new Matrix(4, 1);
                m2[0, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().x);
                m2[1, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().y);
                m2[2, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().z);
                m2[3, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().w);
                Matrix m3 = Matrix.Concatenate(m1, m2);
                m3 = m3.EchelonForm();
                // m3.Rows should be 2
                return new float4((float) m3[0, m3.Rows].ToDouble(), (float) m3[1, m3.Rows].ToDouble(),
                    (float) m3[2, m3.Rows].ToDouble(), (float) m3[3, m3.Rows].ToDouble());
            }
        }
    }
}