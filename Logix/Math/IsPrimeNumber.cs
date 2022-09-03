using BaseX;

namespace FrooxEngine.LogiX.Math
{
    [NodeName("IsPrimeNumber")]
    [Category("LogiX/Math")]

    public class IsPrimeNumber : LogixOperator<bool>
    {
        public readonly Input<int> input;

        public override bool Content
        {
            get
            {
                var num = input.EvaluateRaw();
                if (num < 2) return false;
                else if (num == 2) return true;
                else if (num % 2 == 0) return false;

                double sqrtNum = MathX.Sqrt(num);
                for (int i = 3; i <= sqrtNum; i += 2)
                {
                    if (num % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}
