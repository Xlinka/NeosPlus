using System;
using System.Text;

namespace FrooxEngine.LogiX.String
{
    [Category("LogiX/NeosPlus/String")]
    [NodeName("Encode Base64")]
    public class EncodeBase64 : LogixOperator<string>
    {
        public readonly Input<string> Input;

        public override string Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                return input == null ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
            }
        }
    }
}