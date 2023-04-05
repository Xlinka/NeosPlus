using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json;

[NodeName("To String")]
[Category("LogiX/Json")]
[GenericTypes(typeof(JToken), typeof(JObject), typeof(JArray))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.ToJSONString", typeof(JObject))]
public class JsonToString<T> : LogixOperator<string>
{
    public readonly Input<T> Input;
    public override string Content => Input.EvaluateRaw()?.ToString();
}