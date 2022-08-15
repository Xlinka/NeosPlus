using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Get Nested Object")]
    [Category("LogiX/Json")]
    public class GetNestedObject : LogixOperator<JObject>
    {
        public readonly Input<JObject> Input;
        public readonly Input<string> Tag;
        public override JObject Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                var tag = Tag.EvaluateRaw();
                if (input == null || string.IsNullOrEmpty(tag)) return null;
                try
                {
                    return (JObject) input[tag];
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}