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
        public Random rand = new Random();

        protected override void OnEvaluate()
        {
            var min = Min.EvaluateRaw();
            var max = Max.EvaluateRaw();
            if (min > max)
            {
                var num1 = max;
                var num2 = min;
                min = num1;
                max = num2;
            }
            Value.Value = min + rand.NextDouble() * (max - min);
        }
    }
}
