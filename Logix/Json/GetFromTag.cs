using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Get From Tag")]
    [Category("LogiX/Json")]
    public class GetFromTag : LogixOperator<string>
    {
        public readonly Input<JObject> Input;
        public readonly Input<string> Tag;
        public override string Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                var tag = Tag.EvaluateRaw();
                if (input == null || string.IsNullOrEmpty(tag)) return null;
                try
                {
                    return input[tag].ToString();
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}