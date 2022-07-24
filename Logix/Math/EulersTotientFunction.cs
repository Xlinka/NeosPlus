using FrooxEngine;
using FrooxEngine.LogiX;
using BaseX;

namespace FrooxEngine.LogiX.Math
{
    [NodeName("Euler's Totient Function")]
    [Category(new string[] { "LogiX/Operators" })]

    public sealed class EulersTotientFunction : LogixNode
    {
        public readonly Input<int> input;
        public readonly Output<int> output;

        protected override void OnEvaluate()
        {
            int result = input.EvaluateRaw();
            int inputCopy = input.EvaluateRaw();

            for (int p = 2; p * p <= inputCopy; ++p)
            {
                if (inputCopy % p == 0)
                {
                    while (inputCopy % p == 0)
                    {
                        inputCopy /= p;
                    }
                    result -= result / p;
                }
            }

            if (inputCopy > 1)
            {
                result -= result / inputCopy;
            }

            output.Value = result;
        }
    }
}

