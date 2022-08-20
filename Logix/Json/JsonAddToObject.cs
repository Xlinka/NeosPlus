using System;
using BaseX;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Add To Object")]
    [Category("LogiX/Json")]
    [GenericTypes(typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long),
        typeof(ulong), typeof(float), typeof(double), typeof(string), typeof(Uri), typeof(JToken), typeof(JObject),
        typeof(JArray))]
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
                var in2 = (JObject)input.DeepClone();
                in2[tag] = obj as JToken;
                return in2;
            }
        }
    }
}