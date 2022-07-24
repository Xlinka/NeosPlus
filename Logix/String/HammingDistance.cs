using FrooxEngine.LogiX;
using System;

namespace FrooxEngine.Logix.String
{
    [Category("LogiX/String")]
    [NodeName("Hamming Distance")]
    public class HammingDistance : LogixOperator<int?>
    {
        public readonly Input<string> str1;
        public readonly Input<string> str2;
        
        public override int? Content
        {
            get
            {
                if (str1.EvaluateRaw() is null || str2.EvaluateRaw() is null)
                {
                    return null;
                }

                if (str1.EvaluateRaw().Length != str2.EvaluateRaw().Length)
                {
                    return null;
                }

                int i = 0, count = 0;
                while (i < str1.EvaluateRaw().Length)
                {
                    // Is it more efficient to cache an evaluated copy of the string, rather than index it every time? 
                    if (str1.EvaluateRaw()[i] != str2.EvaluateRaw()[i])
                    {
                        count++;
                    }
                    i++;
                }
                return count;
            }
        }

        protected override void NotifyOutputsOfChange() => ((IOutputElement)this).NotifyChange();
    }
}
