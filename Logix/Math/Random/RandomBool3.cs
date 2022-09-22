using BaseX;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.Math
{
	[Category("LogiX/Math/Random")]
	[NodeName("Random Bool3")]
	public class RandomBool3 : LogixNode
	{
		public readonly Input<float3> Chance;
		public readonly Output<bool3> Value;

		protected override void OnEvaluate()
		{
			var chance = Chance.EvaluateRaw(new float3(0.5f, 0.5f, 0.5f));
			Value.Value = new bool3(RandomX.Chance(chance.x),
									RandomX.Chance(chance.y),
									RandomX.Chance(chance.z));
		}

		protected override void OnCommonUpdate()
		{
			base.OnCommonUpdate();
			MarkChangeDirty();
		}
	}
}
