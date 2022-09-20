using System;
using BaseX;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
	[NodeName("Insert To Array")]
	[Category("LogiX/Json")]
	[GenericTypes(typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long),
		typeof(ulong), typeof(float), typeof(double), typeof(string), typeof(Uri), typeof(JToken), typeof(JObject),
		typeof(JArray))]
	public class JsonInsertToArray<T> : LogixOperator<JArray>
	{
		public readonly Input<JArray> Array;
		public readonly Input<T> Object;
		public readonly Input<int> Index;
		protected override string Label => $"Insert {typeof(T).GetNiceName()} To Array";
		public override JArray Content
		{
			get
			{
				var array = Array.EvaluateRaw();
				var obj = Object.EvaluateRaw();
				var index = Index.EvaluateRaw();
				if (array == null || obj == null || index < 0 || index > array.Count) return null;
				try
				{
					var output = (JArray)array.DeepClone();
					output.Insert(index, obj as JToken);
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