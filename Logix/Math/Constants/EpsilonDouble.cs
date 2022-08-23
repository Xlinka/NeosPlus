using BaseX;

namespace FrooxEngine.LogiX.Math
{
    [Category("LogiX/Math/Constants")]
    [NodeName("Epsilon")]
    public class EpsilonDouble : LogixOperator<double>
    {
        public override double Content => MathX.DOUBLE_EPSILON;
    }
}
