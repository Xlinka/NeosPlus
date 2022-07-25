using FrooxEngine.LogiX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FrooxEngine.Logix.String
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
                if (String.EvaluateRaw() is null || Pattern.EvaluateRaw() is null)
                    return 0;
                else
                    return Regex.Matches(String.EvaluateRaw(), Pattern.EvaluateRaw()).Count; // Simple but slow, might want to replace this
            }
        }
        protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}
