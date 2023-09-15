using BaseX;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Math
{
    [Category("LogiX/NeosPlus/Math/Random")]
    [NodeName("Random Int2")]
    public class RandomInt2 : LogixNode
    {
        public readonly Input<int2> Min;
        public readonly Input<int2> Max;
        public readonly Output<int2> Value;

        protected override void OnEvaluate()
        {
            var min = Min.EvaluateRaw(int2.Zero);
            var max = Max.EvaluateRaw(int2.One);
            Value.Value = new int2(RandomX.Range(min.x, max.x),
                RandomX.Range(min.y, max.y));
        }

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            MarkChangeDirty();
        }
    }
}