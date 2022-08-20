using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Count Array Children")]
    [Category("LogiX/Json")]
    public class JSONCountArrayChildren : LogixOperator<int>
    {
        public readonly Input<JArray> Input;
        public override int Content => Input.EvaluateRaw()?.Count ?? -1;
    }
}