using System;
using BaseX;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
	[NodeName("Get From Array")]
	[Category("LogiX/Json")]
	[GenericTypes(typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long),
		typeof(ulong), typeof(float), typeof(double), typeof(string), typeof(Uri), typeof(JToken), typeof(JObject),
		typeof(JArray))]
	[OldTypeSpecialization("FrooxEngine.LogiX.Json.JSONGetArrayItem", typeof(JObject))]
	public class JsonGetFromArray<T> : LogixOperator<T>
	{
		public readonly Input<JArray> Input;
		public readonly Input<int> Index;
		protected override string Label => $"Get {typeof(T).GetNiceName()} From Array";
		public override T Content
		{
			get
			{
				var input = Input.EvaluateRaw();
				var index = Index.EvaluateRaw();
				if (input == null || index < 0 || input.Count < index) return default;
				try
				{
					return input[index].Value<T>();
				}
				catch
				{
					return default;
				}
			}
		}
	}
}