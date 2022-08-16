using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Get Array Item")]
    [Category("LogiX/Json")]
    public class JSONGetArrayItem : LogixOperator<JObject>
    {
        public readonly Input<JArray> Input;
        public readonly Input<int> Index;
        public override JObject Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                var index = Index.EvaluateRaw();
                if (input == null || index < 0 || input.Count < index) return null;
                try
                {
                    return (JObject) input[index];
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}