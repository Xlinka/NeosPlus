using BaseX;
using FrooxEngine.LogiX;
using CodeX;
using System;

namespace FrooxEngine.Logix.Math
{
    [Category("LogiX/Math/Random")]
    [NodeName("Random Long")]
    public class RandomLong : LogixNode
    {
        public readonly Input<long> Min;
        public readonly Input<long> Max;
        public readonly Output<long> Value;

        protected override void OnEvaluate()
        {
            var rand = new Random();
            var min = Min.EvaluateRaw();
            var max = Max.EvaluateRaw();
            Value.Value = min + (long)(rand.Next() * (max - min));
        }
    }
}
