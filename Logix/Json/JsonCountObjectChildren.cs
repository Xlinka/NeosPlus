using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Count Object Children")]
    [Category("LogiX/Json")]
    public class JsonCountObjectChildren : LogixOperator<int>
    {
        public readonly Input<JObject> Input;
        public override int Content => Input.EvaluateRaw()?.Count ?? -1;
    }
}