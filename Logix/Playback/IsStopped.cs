using BaseX;

namespace FrooxEngine.LogiX.Playback
{
	[Category("LogiX/Playback")]
	[NodeName("IsStopped")]
	public class IsStopped : LogixOperator<bool>
	{
		public readonly Input<IPlayable> Playable;
		public override bool Content
		{
			get
			{
				IPlayable target = Playable.EvaluateRaw();
				bool flag = target != null && target.IsPlaying;
				bool flag2 = MathX.Approximately((target != null) ? target.NormalizedPosition : 0f, 0f, 9.403955E-38f);
				return !flag && flag2;
			}
		}

		// Token: 0x060086F0 RID: 34544 RVA: 0x0005D48C File Offset: 0x0005B68C
		protected override void NotifyOutputsOfChange()
		{
			((IOutputElement)this).NotifyChange();
		}
	}
}
