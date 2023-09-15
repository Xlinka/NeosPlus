using BaseX;

namespace FrooxEngine.LogiX.Math
{
    [NodeName("IsPrimeNumber")]
    [Category("LogiX/NeosPlus/Math")]
    public class IsPrimeNumber : LogixOperator<bool>
    {
        public readonly Input<int> input;

        public override bool Content
        {
            get
            {
                var num = input.EvaluateRaw();
                switch (num)
                {
                    case < 2:
                        return false;
                    case 2:
                        return true;
                    default:
                    {
                        if (num % 2 == 0) return false;
                        break;
                    }
                }
                double sqrtNum = MathX.Sqrt(num);
                for (var i = 3; i <= sqrtNum; i += 2)
                    if (num % i == 0)
                        return false;
                return true;
            }
        }
    }
}