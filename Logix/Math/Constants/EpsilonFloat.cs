using BaseX;

namespace FrooxEngine.LogiX.Math
{
    [Category("LogiX/Math/Constants")]
    [NodeName("Epsilon")]
    public class EpsilonFloat : LogixOperator<float>
    {
        public override float Content => MathX.FLOAT_EPSILON;
    }
}
