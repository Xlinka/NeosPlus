using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Count Array Children")]
    [Category("LogiX/Json")]
    [OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONCountArrayChildren")]
    public class JsonCountArrayChildren : LogixOperator<int>
    {
        public readonly Input<JArray> Input;
        public override int Content => Input.EvaluateRaw()?.Count ?? -1;
    }
}