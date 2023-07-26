using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json;

[NodeName("Count Object Children")]
[Category("LogiX/Json")]
[OldTypeName("FrooxEngine.LogiX.Json.JSONCountChildren")]
public class JsonCountObjectChildren : LogixOperator<int>
{
    public readonly Input<JObject> Input;
    public override int Content => Input.EvaluateRaw()?.Count ?? -1;
}