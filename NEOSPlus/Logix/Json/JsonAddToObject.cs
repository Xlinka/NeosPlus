using System;
using BaseX;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json;

[NodeName("Add To Object")]
[Category("LogiX/NeosPlus/Json")]
[GenericTypes(typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long),
    typeof(ulong), typeof(float), typeof(double), typeof(string), typeof(Uri), typeof(JToken), typeof(JObject),
    typeof(JArray))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddString", typeof(string))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddBool", typeof(bool))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddNestedObject", typeof(JObject))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddNumber_Byte", typeof(byte))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddNumber_Sbyte", typeof(sbyte))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddNumber_Short", typeof(short))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddNumber_Ushort", typeof(ushort))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddNumber_Int", typeof(int))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddNumber_Uint", typeof(uint))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddNumber_Long", typeof(long))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddNumber_Ulong", typeof(ulong))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddNumber_Float", typeof(float))]
[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONAddNumber_Double", typeof(double))]
public class JsonAddToObject<T> : LogixOperator<JObject>
{
    public readonly Input<JObject> Input;
    public readonly Input<string> Tag;
    public readonly Input<T> Object;
    protected override string Label => $"Add {typeof(T).GetNiceName()} To Object";

    public override JObject Content
    {
        get
        {
            var input = Input.EvaluateRaw();
            if (input == null) return null;
            var tag = Tag.EvaluateRaw();
            var obj = Object.EvaluateRaw();
            if (string.IsNullOrEmpty(tag) || obj == null) return input;
            var in2 = (JObject) input.DeepClone();
            in2[tag] = obj switch
            {
                JArray jArray => jArray,
                JObject jObject => jObject,
                JToken token => token,
                _ => new JValue(obj) 
            };
            return in2;
        }
    }
}