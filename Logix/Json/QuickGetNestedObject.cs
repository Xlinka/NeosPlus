using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Quick Get Nested Object")]
    [Category("LogiX/Json")]
    public class QuickGetNestedObject : LogixOperator<JObject>
    {
        public readonly Input<string> Input;
        public readonly Input<string> Tag;
        public override JObject Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                var tag = Tag.EvaluateRaw();
                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(tag)) return null;
                try
                {
                    var input2 = JObject.Parse(input);
                    return (JObject) input2[tag];
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}