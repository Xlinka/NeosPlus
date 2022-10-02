using BaseX;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Math
{
    [Category("LogiX/Math/Random")]
    [NodeName("Random Bool4")]
    public class RandomBool4 : LogixNode
    {
        public readonly Input<float4> Chance;
        public readonly Output<bool4> Value;

        protected override void OnEvaluate()
        {
            var chance = Chance.EvaluateRaw(new float4(0.5f, 0.5f, 0.5f, 0.5f));
            Value.Value = new bool4(RandomX.Chance(chance.x),
                RandomX.Chance(chance.y),
                RandomX.Chance(chance.z),
                RandomX.Chance(chance.w));
        }

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            MarkChangeDirty();
        }
    }
}