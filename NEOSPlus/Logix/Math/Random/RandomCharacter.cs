using BaseX;

namespace FrooxEngine.LogiX.Math
{
    [Category("LogiX/NeosPlus/Math/Random")]
    [NodeName("Random Character")]
    public class RandomCharacter : LogixNode
    {
        public readonly Input<int> Start;
        public readonly Input<int> End;
        public readonly Input<string> String;
        public readonly Output<char> Value;

        protected override void OnEvaluate()
        {
            var str = String.EvaluateRaw();
            var start = MathX.Clamp(Start.EvaluateRaw(), 0, str.Length);
            var end = MathX.Clamp(End.EvaluateRaw(str.Length), start, str.Length);
            Value.Value = str[RandomX.Range(start, end)];
        }

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            MarkChangeDirty();
        }
    }
}