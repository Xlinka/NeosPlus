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
                var str = String.EvaluateRaw();
                var pattern = Pattern.EvaluateRaw();
                if (str is null || pattern is null || str == "" || pattern == "")
                    return 0;
                return (str.Length - str.Replace(pattern, "").Length) / pattern.Length;
            }
        }
        protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}
