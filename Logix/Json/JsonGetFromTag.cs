using System;
using BaseX;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Get From Tag")]
    [Category("LogiX/Json")]
    [GenericTypes(typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long),
        typeof(ulong), typeof(float), typeof(double), typeof(string), typeof(Uri), typeof(JToken), typeof(JObject),
        typeof(JArray))]
    [OldTypeSpecialization("FrooxEngine.LogiX.Json.GetFromTag", typeof(string))]
    [OldTypeSpecialization("FrooxEngine.LogiX.Json.GetArrayFromTag", typeof(JArray))]
    public class JsonGetFromTag<T> : LogixOperator<T>
    {
        public readonly Input<JObject> Input;
        public readonly Input<string> Tag;
        protected override string Label => $"Get {typeof(T).GetNiceName()} From Tag";
        public override T Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                var tag = Tag.EvaluateRaw();
                if (input == null || string.IsNullOrEmpty(tag)) return default;
                try
                {
                    return input[tag].Value<T>();
                }
                catch
                {
                    return default;
                }
            }
        }
    }
}