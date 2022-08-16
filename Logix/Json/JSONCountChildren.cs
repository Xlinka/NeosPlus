using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Count Children")]
    [Category("LogiX/Json")]
    public class JSONCountChildren : LogixOperator<int>
    {
        public readonly Input<JObject> Input;
        public override int Content => Input.EvaluateRaw()?.Count ?? -1;
    }
}