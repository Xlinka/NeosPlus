using System;
using BaseX;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json;

[NodeName("Remove From Object")]
[Category("LogiX/NeosPlus/Json")]
public class JsonRemoveFromObject : LogixOperator<JObject>
{
    public readonly Input<JObject> Input;
    public readonly Input<string> Tag;

    public override JObject Content
    {
        get
        {
            var input = Input.EvaluateRaw();
            if (input == null) return null;
            var tag = Tag.EvaluateRaw();
            if (string.IsNullOrEmpty(tag)) return input;
            var in2 = (JObject) input.DeepClone();
            in2.Remove(tag);
            return in2;
        }
    }
}