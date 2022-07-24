using FrooxEngine;
using FrooxEngine.LogiX;
using BaseX;
using Mehroz;

namespace FrooxEngine.LogiX.Math
{
    [HiddenNode] // Hide overload from node browser
    [NodeName("Reduced Row Echelon Form")]
    [NodeOverload("Reduced-Row-Echelon-Form")]
    [Category(new string[] { "LogiX/Math/Matrix" })]

    public sealed class GaussJordanElimination_double4x4 : LogixOperator<double4>
    {
        public readonly Input<double4x4> LinearEquationMatrix;
        public readonly Input<double4> LinearSolutionMatrix;

        public override double4 Content
        {
            get 
            {
                Matrix m1 = new Matrix(LinearEquationMatrix.EvaluateRaw().To2DArray());
                Matrix m2 = new Matrix(4, 1);
                m2[0, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().x);
                m2[1, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().y);
                m2[2, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().z);
                m2[3, 0] = new Fraction(LinearSolutionMatrix.EvaluateRaw().w);
                Matrix m3 = Matrix.Concatenate(m1, m2);
                m3 = m3.ReducedEchelonForm();
                // m3.Rows should be 2
                return new double4(m3[0, m3.Rows].ToDouble(), m3[1, m3.Rows].ToDouble(), m3[2, m3.Rows].ToDouble(), m3[3, m3.Rows].ToDouble());
            }
        }
           
    }
}

