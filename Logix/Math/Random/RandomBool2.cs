using BaseX;
using FrooxEngine.LogiX;

namespace FrooxEngine.Logix.Math
{
    [Category("LogiX/Math/Random")]
    [NodeName("Random Bool2")]
    public class RandomBool2 : LogixNode
    {
        public readonly Input<float2> Chance;
        public readonly Output<bool2> Value;

        protected override void OnEvaluate()
        {
            var chance = Chance.EvaluateRaw(new float2(0.5f, 0.5f));
            Value.Value = new bool2(RandomX.Chance(chance.x),
                                    RandomX.Chance(chance.y));
        }

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            MarkChangeDirty();
        }
    }
}
