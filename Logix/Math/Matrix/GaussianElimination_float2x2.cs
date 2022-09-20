using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using Mehroz;

namespace FrooxEngine.LogiX.Math
{
    [NodeName("Reduced Echelon Form")]
    [NodeOverload("Reduced-Echelon-Form")]
    [Category(new string[] {"LogiX/Math/Matrix"})]

    // GaussJordanElimination
    public sealed class GaussianElimination_float2x2 : LogixOperator<float2>
    {
        public readonly Input<float2x2> LinearEquationMatrix;
        public readonly Input<float2> LinearSolutionMatrix;

        public override float2 Content
        {
            get
            {
                Matrix m1 = new Matrix(((double2x2) LinearEquationMatrix.EvaluateRaw()).To2DArray());
                Matrix m2 = new Matrix(2, 1);
                m2[0, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().x);
                m2[1, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().y);
                Matrix m3 = Matrix.Concatenate(m1, m2);
                m3 = m3.EchelonForm();
                // m3.Rows should be 2
                return new float2((float) m3[0, m3.Rows].ToDouble(), (float) m3[1, m3.Rows].ToDouble());
            }
        }
    }
}