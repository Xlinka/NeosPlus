using System;
using BaseX;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Remove From Array")]
    [Category("LogiX/Json")]
    public class JsonRemoveFromArray<T> : LogixOperator<JArray>
    {
        public readonly Input<JArray> Array;
        public readonly Input<int> Index;

        public override JArray Content
        {
            get
            {
                var array = Array.EvaluateRaw();
                var index = Index.EvaluateRaw();
                if (array == null || index < 0 || index >= array.Count) return null;
                try
                {
                    var output = (JArray) array.DeepClone();
                    output.RemoveAt(index);
                    return output;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}