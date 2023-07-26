using System;
using System.Text;
using NEOSPlus;

namespace FrooxEngine.LogiX.String;

[Category("LogiX/String")]
[NodeName("Encode Morse")]
public class EncodeMorse : LogixOperator<string>
{
    public readonly Input<string> Input;
    public override string Content
    {
        get
        {
            var a = Input.EvaluateRaw();
            if (string.IsNullOrWhiteSpace(a)) return null;
            a = a.ToUpperInvariant();
            var result = new StringBuilder();
            foreach (var c in a)
            {
                //is it better to explicitly check for space or is it better to use char.IsWhitespace(c)?
                //we might miss valid whitespace characters but IsWhitespace might pick up characters we dont want
                if (c == ' ')
                {
                    result.Append("/ ");
                    continue;
                }
                if (NodeExtensions.CharToMorse.TryGetValue(c, out var o)) result.Append(o + " ");
            }
            return result.ToString();
        }
    }
}