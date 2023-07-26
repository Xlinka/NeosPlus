using System;
using BaseX;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json;

[NodeName("Quick Get From Object")]
[Category("LogiX/Json")]
[GenericTypes(typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long),
    typeof(ulong), typeof(float), typeof(double), typeof(string), typeof(Uri), typeof(JToken), typeof(JObject),
    typeof(JArray))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.QuickGetNestedObject", typeof(JObject))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.QuickGetFromTag", typeof(string))]
[OldTypeName("FrooxEngine.LogiX.Json.JsonQuickGetFromTag")]
public class JsonQuickGetFromObject<T> : LogixOperator<T>
{
    public readonly Input<string> Input;
    public readonly Input<string> Tag;
    protected override string Label => $"Quick Get {typeof(T).GetNiceName()} From Object";

    public override T Content
    {
        get
        {
            var input = Input.EvaluateRaw();
            var tag = Tag.EvaluateRaw();
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(tag)) return default;
            try
            {
                var input2 = JObject.Parse(input);
                return input2[tag].Value<T>();
            }
            catch
            {
                return default;
            }
        }
    }
}