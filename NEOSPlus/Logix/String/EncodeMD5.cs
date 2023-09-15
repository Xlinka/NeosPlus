using System;
using System.Security.Cryptography;
using System.Text;

namespace FrooxEngine.LogiX.String
{
    [Category("LogiX/NeosPlus/String")]
    [NodeName("Encode MD5")]
    public class EncodeMD5 : LogixOperator<string>
    {
        private static readonly MD5CryptoServiceProvider MD5 = new();
        public readonly Input<string> Input;

        public override string Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                return input == null
                    ? null
                    : BitConverter.ToString(MD5.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "");
            }
        }
    }
}