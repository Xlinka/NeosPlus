using System;
using System.Text;
using NEOSPlus;

namespace FrooxEngine.LogiX.String;

[Category("LogiX/NeosPlus/String")]
[NodeName("Decode Morse")]
public class DecodeMorse : LogixOperator<string>
{
    public readonly Input<string> Input;
    public override string Content
    {
        get
        {
            var a = Input.EvaluateRaw();
            if (string.IsNullOrWhiteSpace(a)) return null;
            var words = a.Split('/');
            var result = new StringBuilder();
            foreach (var word in words)
            {
                var characters = word.Split(' ');
                if (string.IsNullOrWhiteSpace(word)) continue;
                foreach (var c in characters)
                {
                    if (string.IsNullOrWhiteSpace(c)) continue;
                    if (NodeExtensions.MorseToChar.TryGetValue(c, out var ch))
                        result.Append(ch);
                }
                result.Append(' ');
            }
            return result.ToString();
        }
    }
}