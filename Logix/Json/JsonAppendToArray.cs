using System;
using BaseX;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Append To Array")]
    [Category("LogiX/Json")]
    [GenericTypes(typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long),
        typeof(ulong), typeof(float), typeof(double), typeof(string), typeof(Uri), typeof(JToken), typeof(JObject),
        typeof(JArray))]
    public class JsonAppendToArray<T> : LogixOperator<JArray>
    {
        public readonly Input<JArray> Array;
        public readonly Input<T> Object;
        protected override string Label => $"Append {typeof(T).GetNiceName()} To Array";

        public override JArray Content
        {
            get
            {
                var array = Array.EvaluateRaw();
                var obj = Object.EvaluateRaw();
                if (array == null || obj == null) return null;
                try
                {
                    var output = (JArray) array.DeepClone();
                    output.Add(obj);
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