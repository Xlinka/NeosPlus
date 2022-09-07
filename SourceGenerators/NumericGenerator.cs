using System.Linq;
using Microsoft.CodeAnalysis;

namespace SourceGenerators
{
	[Generator]
	public class NumericGenerator : ISourceGenerator
	{
		public void Initialize(GeneratorInitializationContext context)
		{

		}
		public void Execute(GeneratorExecutionContext context)
		{
			var types = new[]
			{
				"byte",
				"sbyte",
				"short",
				"ushort",
				"int",
				"int2",
				"int3",
				"int4",
				"long",
				"long2",
				"long3",
				"long4",
				"uint",
				"uint2",
				"uint3",
				"uint4",
				"ulong",
				"ulong2",
				"ulong3",
				"ulong4",
				"float",
				"float2",
				"float3",
				"float4",
				"floatQ",
				"float2x2",
				"float3x3",
				"float4x4",
				"double",
				"double2",
				"double3",
				"double4",
				"doubleQ",
				"double2x2",
				"double3x3",
				"double4x4",
			};
			foreach (var type in types)
			{
				var name = type.First().ToString().ToUpper() + type.Substring(1); ;
				var hidden = type != "float" ? "[HiddenNode]" : "";
				var gen = $@"//generated
                using BaseX;

                namespace FrooxEngine.LogiX.Operators
                {{
                    {hidden}
                    [Category(""LogiX/Operators"")]
                    [NodeOverload(""DoubleInput"")]
                    [NodeName(""*2"")]
                    public class DoubleInput_{name} : LogixOperator<{type}>
                    {{
                        public readonly Input<{type}> A;
                        public override {type} Content => ({type})(A.EvaluateRaw() * 2);
                    }}
                    {hidden}
                    [Category(""LogiX/Operators"")]
                    [NodeOverload(""HalfInput"")]
                    [NodeName(""÷2"")]
                    public class HalfInput_{name} : LogixOperator<{type}>
                    {{
                        public readonly Input<{type}> A;
                        public override {type} Content => ({type})(A.EvaluateRaw() / 2);
                    }}
                }}
                ";
				context.AddSource($"Numerics_{name}.g.cs", gen);

			}
		}
	}
}