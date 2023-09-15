using BaseX;

namespace FrooxEngine.LogiX.Math
{
    [Category("LogiX/NeosPlus/Math/Constants")]
    [NodeName("Epsilon Float")]
    public class EpsilonFloat : LogixOperator<float>
    {
        public override float Content => MathX.FLOAT_EPSILON;
    }
}