using System;
using System.Security.Cryptography;
using System.Text;

namespace FrooxEngine.LogiX.String
{
    [Category("LogiX/NeosPlus/String")]
    [NodeName("Encode Sha256")]
    public class EncodeSha256 : LogixOperator<string>
    {
        private static readonly SHA256 SHA = SHA256.Create();
        public readonly Input<string> Input;

        public override string Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                return input == null
                    ? null
                    : BitConverter.ToString(SHA.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "");
            }
        }
    }
}