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
    public sealed class GaussianElimination_double3x3 : LogixOperator<double3>
    {
        public readonly Input<double3x3> LinearEquationMatrix;
        public readonly Input<double3> LinearSolutionMatrix;

        public override double3 Content
        {
            get
            {
                Matrix m1 = new Matrix(LinearEquationMatrix.EvaluateRaw().To2DArray());
                Matrix m2 = new Matrix(3, 1);
                m2[0, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().x);
                m2[1, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().y);
                m2[2, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().z);
                Matrix m3 = Matrix.Concatenate(m1, m2);
                m3 = m3.EchelonForm();
                // m3.Rows should be 2
                return new double3(m3[0, m3.Rows].ToDouble(), m3[1, m3.Rows].ToDouble(), m3[2, m3.Rows].ToDouble());
            }
        }
    }
}