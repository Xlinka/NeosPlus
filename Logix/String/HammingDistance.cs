using FrooxEngine.LogiX;
using System;

namespace FrooxEngine.Logix.String
{
    [Category("LogiX/String")]
    [NodeName("Hamming Distance")]
    public class HammingDistance : LogixOperator<int?>
    {
        public readonly Input<string> String1;
        public readonly Input<string> String2;
        
        public override int? Content
        {
            get
            {
                if (String1.EvaluateRaw() is null || String2.EvaluateRaw() is null)
                {
                    return null;
                }

                if (String1.EvaluateRaw().Length != String2.EvaluateRaw().Length)
                {
                    return null;
                }

                int i = 0, count = 0;
                while (i < String1.EvaluateRaw().Length)
                {
                    // Is it more efficient to cache an evaluated copy of the string, rather than index it every time? 
                    if (String1.EvaluateRaw()[i] != String2.EvaluateRaw()[i])
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
