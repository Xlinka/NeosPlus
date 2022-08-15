using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("To JSON String")]
    [Category("LogiX/Json")]
    public class ToJSONString : LogixOperator<string>
    {
        public readonly Input<JObject> Input;
        public override string Content => Input.EvaluateRaw()?.ToString();
    }
}