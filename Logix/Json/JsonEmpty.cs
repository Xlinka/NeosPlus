using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Empty Object")]
    [Category("LogiX/Json")]
    public class JsonEmptyObject : LogixOperator<JObject>
    {
        public override JObject Content => new JObject();
    }
    [NodeName("Empty Array")]
    [Category("LogiX/Json")]
    public class JsonEmptyArray : LogixOperator<JArray>
    {
        public override JArray Content => new JArray();
    }
}