using BaseX;

namespace FrooxEngine.LogiX.Math
{
	[Category("LogiX/Math/Random")]
	[NodeName("Random Letter")]
	public class RandomLetter : LogixNode
	{
		public readonly Input<int> StartOfAlphabet;
		public readonly Input<int> EndOfAlphabet;
		public readonly Input<bool> UseUppercase;
		public readonly Output<char> Value;

		protected override void OnEvaluate()
		{
			var start = MathX.Clamp(StartOfAlphabet.EvaluateRaw(), 0, 26);
			var end = MathX.Clamp(EndOfAlphabet.EvaluateRaw(26), start, 26);
			Value.Value = UseUppercase.EvaluateRaw() ?
				RandomX.UPPERCASE_ALPHABET[RandomX.Range(start, end)] :
				RandomX.LOWERCASE_ALPHABET[RandomX.Range(start, end)];
		}

		protected override void OnCommonUpdate()
		{
			base.OnCommonUpdate();
			MarkChangeDirty();
		}
	}
}
