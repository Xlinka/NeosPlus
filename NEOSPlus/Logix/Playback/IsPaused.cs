using BaseX;

namespace FrooxEngine.LogiX.Playback
{
    [Category("LogiX/NeosPlus/Playback")]
    [NodeName("IsPaused")]
    public class IsPaused : LogixOperator<bool>
    {
        public readonly Input<IPlayable> Playable;

        public override bool Content
        {
            get
            {
                var target = Playable.EvaluateRaw();
                var flag = target != null && target.IsPlaying;
                var flag2 = MathX.Approximately(target?.NormalizedPosition ?? 0f, 0f);
                return !flag && !flag2;
            }
        }
    }
}