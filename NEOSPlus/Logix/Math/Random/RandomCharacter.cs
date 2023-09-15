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
            var str = String.IsConnected ? String.EvaluateRaw() : null;
            if (!string.IsNullOrEmpty(str))
            {
                var start = MathX.Clamp(Start.EvaluateRaw(), 0, str.Length);
                var end = MathX.Clamp(End.EvaluateRaw(str.Length), start, str.Length);
                Value.Value = str[RandomX.Range(start, end)];
            }
            else
            {
                //fix for https://github.com/Xlinka/NeosPlus/issues/164
                Value.Value = null;
            }
        }

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            MarkChangeDirty();
        }
    }
}