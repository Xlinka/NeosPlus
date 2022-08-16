using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Get Array From Tag")]
    [Category("LogiX/Json")]
    public class GetArrayFromTag : LogixOperator<JArray>
    {
        public readonly Input<JObject> Input;
        public readonly Input<string> Tag;
        public override JArray Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                var tag = Tag.EvaluateRaw();
                if (input == null || string.IsNullOrEmpty(tag)) return null;
                try
                {
                    return (JArray)input[tag];
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}