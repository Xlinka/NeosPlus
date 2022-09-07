using System;
using System.Text;

namespace FrooxEngine.LogiX.String
{
	[Category("LogiX/String")]
	[NodeName("Decode Base64")]
	public class DecodeBase64 : LogixOperator<string>
	{
		public readonly Input<string> Input;
		public override string Content
		{
			get
			{
				var input = Input.EvaluateRaw();
				if (input == null) return null;
				try
				{
					return Encoding.UTF8.GetString(Convert.FromBase64String(input));
				}
				catch
				{
					return null;
				}
			}
		}
		protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
	}
}