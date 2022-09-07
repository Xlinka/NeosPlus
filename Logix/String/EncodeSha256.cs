using System;
using System.Security.Cryptography;
using System.Text;

namespace FrooxEngine.LogiX.String
{
	[Category("LogiX/String")]
	[NodeName("Encode Sha256")]
	public class EncodeSha256 : LogixOperator<string>
	{
		private static SHA256 _sha = SHA256.Create();
		public readonly Input<string> Input;
		public override string Content
		{
			get
			{
				var input = Input.EvaluateRaw();
				return input == null ? null : BitConverter.ToString(_sha.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "");
			}
		}
		protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
	}
}