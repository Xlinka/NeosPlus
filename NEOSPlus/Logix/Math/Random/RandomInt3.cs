using BaseX;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Math
{
    [Category("LogiX/NeosPlus/Math/Random")]
    [NodeName("Random Int3")]
    public class RandomInt3 : LogixNode
    {
        public readonly Input<int3> Min;
        public readonly Input<int3> Max;
        public readonly Output<int3> Value;

        protected override void OnEvaluate()
        {
            var min = Min.EvaluateRaw(int3.Zero);
            var max = Max.EvaluateRaw(int3.One);
            Value.Value = new int3(RandomX.Range(min.x, max.x),
                RandomX.Range(min.y, max.y));
        }

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            MarkChangeDirty();
        }
    }
}