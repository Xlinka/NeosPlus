using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Quick Get From Tag")]
    [Category("LogiX/Json")]
    public class QuickGetFromTag : LogixOperator<string>
    {
        public readonly Input<string> Input;
        public readonly Input<string> Tag;
        public override string Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                var tag = Tag.EvaluateRaw();
                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(tag)) return null;
                try
                {
                    var input2 = JObject.Parse(input);
                    return input2[tag].ToString();
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}