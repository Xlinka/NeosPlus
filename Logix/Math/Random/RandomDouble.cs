using BaseX;
using FrooxEngine.LogiX;
using CodeX;
using System;

namespace FrooxEngine.Logix.Math
{
    [Category("LogiX/Math/Random")]
    [NodeName("Random Double")]
    public class RandomDouble : LogixNode
    {
        public readonly Input<double> Min;
        public readonly Input<double> Max;
        public readonly Output<double> Value;

        protected override void OnEvaluate()
        {
            var rand = new Random();
            var min = Min.EvaluateRaw();
            var max = Max.EvaluateRaw();
            Value.Value = min + rand.NextDouble() * (max - min);
        }
    }
}
