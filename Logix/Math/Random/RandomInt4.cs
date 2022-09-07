using BaseX;
using FrooxEngine.LogiX;

namespace FrooxEngine.Logix.Math
{
	[Category("LogiX/Math/Random")]
	[NodeName("Random Int4")]
	public class RandomInt4 : LogixNode
	{
		public readonly Input<int4> Min;
		public readonly Input<int4> Max;
		public readonly Output<int4> Value;

		protected override void OnEvaluate()
		{
			var min = Min.EvaluateRaw(int4.Zero);
			var max = Max.EvaluateRaw(int4.One);
			Value.Value = new int4(RandomX.Range(min.x, max.x),
								   RandomX.Range(min.y, max.y));
		}

		protected override void OnCommonUpdate()
		{
			base.OnCommonUpdate();
			MarkChangeDirty();
		}
	}
}
