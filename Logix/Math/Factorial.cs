using BaseX;

namespace FrooxEngine.LogiX.Math
{
    [NodeName("Factorial")]
    [Category("LogiX/Math")]
    public class Factorial : LogixOperator<int>
    {
        public readonly Input<int> Input;

        public override int Content
        {
            get
            {
                var fact = 1;
                var loop = MathX.Clamp(Input.EvaluateRaw(), 0, 16);
                for (var i = 1; i <= loop; i++) fact *= i;
                return fact;
            }
        }

        protected override void NotifyOutputsOfChange() => ((IOutputElement) this).NotifyChange();
    }
}