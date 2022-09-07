using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FrooxEngine.LogiX;

namespace FrooxEngine.LogiX.String
{
	[Category("LogiX/String")]
	[NodeName("Count Substring")]
	public class CountSubstring : LogixOperator<int>
	{
		public readonly Input<string> String;
		public readonly Input<string> Pattern;

		public override int Content
		{
			get
			{
				var str = String.EvaluateRaw();
				var pattern = Pattern.EvaluateRaw();
				return str is null || pattern is null || str == "" || pattern == "" ? 0 :
					(str.Length - str.Replace(pattern, "").Length) / pattern.Length;
			}
		}
		protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
	}
}
