using System;
using System.Security.Cryptography;
using System.Text;

namespace FrooxEngine.LogiX.String
{
    [Category("LogiX/String")]
    [NodeName("Encode MD5")]
    public class EncodeMD5 : LogixOperator<string>
    {
        private static MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();
        public readonly Input<string> Input;

        public override string Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                return input == null
                    ? null
                    : BitConverter.ToString(_md5.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "");
            }
        }
    }
}