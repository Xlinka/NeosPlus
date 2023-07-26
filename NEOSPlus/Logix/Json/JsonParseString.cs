using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json;

[NodeName("Parse Object From String")]
[Category("LogiX/Json")]
[OldTypeName("FrooxEngine.LogiX.Json.ParseJSONString")]
public class JsonParseString : LogixOperator<JObject>
{
    public readonly Input<string> Input;

    public override JObject Content
    {
        get
        {
            var input = Input.EvaluateRaw();
            if (string.IsNullOrEmpty(input)) return null;
            try
            {
                var output = JObject.Parse(input);
                return output;
            }
            catch
            {
                return null;
            }
        }
    }
}