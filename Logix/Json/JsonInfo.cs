using System;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
	public static class JsonInfo
	{
		public static readonly Type[] JsonTypes = {
			typeof(byte),
			typeof(sbyte),
			typeof(short),
			typeof(ushort),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(string),
			typeof(Uri),
			typeof(JToken),
			typeof(JObject),
			typeof(JArray),
		};
	}
}