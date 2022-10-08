using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Math
{
    [NodeName("Euler's Totient Function")]
    [Category("LogiX/Operators")]
    public class EulersTotientFunction : LogixNode
    {
        public readonly Input<int> Input;
        public readonly Output<int> Output;

        protected override void OnEvaluate()
        {
            var result = Input.EvaluateRaw();
            var inputCopy = result;
            for (var p = 2; p * p <= inputCopy; ++p)
            {
                if (inputCopy % p != 0) continue;
                while (inputCopy % p == 0) inputCopy /= p;
                result -= result / p;
            }
            if (inputCopy > 1) result -= result / inputCopy;
            Output.Value = result;
        }
    }
}