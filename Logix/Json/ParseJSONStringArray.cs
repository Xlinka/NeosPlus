using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Parse Array From String")]
    [Category("LogiX/Json")]
    public class ParseJSONStringArray : LogixOperator<JArray>
    {
        public readonly Input<string> Input;
        public override JArray Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                if (string.IsNullOrEmpty(input)) return null;
                try
                {
                    var output = JArray.Parse(input);
                    return output;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}