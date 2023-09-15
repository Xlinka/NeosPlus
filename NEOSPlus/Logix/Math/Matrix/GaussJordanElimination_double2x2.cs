using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using Mehroz;

namespace FrooxEngine.LogiX.Math
{
    [NodeName("Reduced Row Echelon Form")]
    [NodeOverload("Reduced-Row-Echelon-Form")]
    [Category(new string[] {"LogiX/NeosPlus/Math/Matrix"})]

    // GaussJordanElimination
    public sealed class GaussJordanElimination_d2x2 : LogixOperator<double2>
    {
        public readonly Input<double2x2> LinearEquationMatrix;
        public readonly Input<double2> LinearSolutionMatrix;

        public override double2 Content
        {
            get
            {
                Matrix m1 = new Matrix(LinearEquationMatrix.EvaluateRaw().To2DArray());
                Matrix m2 = new Matrix(2, 1);
                m2[0, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().x);
                m2[1, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().y);
                Matrix m3 = Matrix.Concatenate(m1, m2);
                m3 = m3.ReducedEchelonForm();
                return new double2(m3[0, 2].ToDouble(), m3[1, 2].ToDouble());
            }
        }
    }
}