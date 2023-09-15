using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json;

[NodeName("Empty Object")]
[Category("LogiX/NeosPlus/Json")]
public class JsonEmptyObject : LogixOperator<JObject>
{
    public override JObject Content => new();
}

[NodeName("Empty Array")]
[Category("LogiX/NeosPlus/Json")]
public class JsonEmptyArray : LogixOperator<JArray>
{
    public override JArray Content => new();
}

[NodeName("Null Value")]
[Category("LogiX/NeosPlus/Json")]
public class JsonNullValue : LogixOperator<JToken>
{
    public override JToken Content => JValue.CreateNull();
}