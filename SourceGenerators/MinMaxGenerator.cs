using System.Linq;
using Microsoft.CodeAnalysis;

namespace SourceGenerators
{
	[Generator]
	public class MixMaxGenerator : ISourceGenerator
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
			};
			foreach (var type in types)
			{
				var name = type.First().ToString().ToUpper() + type.Substring(1); ;
				var hidden = type != "float" ? "[HiddenNode]" : "";
				var gen = $@"//generated
                using BaseX;
                using NEOSPlus;
                using System.Linq;

                namespace FrooxEngine.LogiX.Math
                {{
                    {hidden}
                    [Category(""LogiX/Math"")]
                    [NodeOverload(""MinMax"")]
                    [NodeName(""MinMax"")]
                    public class MinMax_{name} : LogixNode
                    {{
                        public readonly SyncList<Input<{type}>> ValueInputs;
                        public readonly Output<{type}> Min;
                        public readonly Output<{type}> Max;

                        protected override void OnAttach()
                        {{
                            base.OnAttach();
                            for (var i = 0; i < 2; i++)
                            {{
                                ValueInputs.Add();
                            }}
                        }}

                        protected override void OnEvaluate()
                        {{
                            var inputs = ValueInputs.Select(t => t.EvaluateRaw()).ToArray();
                            var min = inputs[0];
                            var max = min;
                            foreach (var num in inputs)
                            {{
                                min = MathX.Min(min, num);
                                max = MathX.Max(max, num);
                            }}
                            Min.Value = min;
                            Max.Value = max;
                        }}

                        protected override void OnGenerateVisual(Slot root) => GenerateUI(root).GenerateListButtons(Add, Remove);

                        [SyncMethod]
                        private void Add(IButton button, ButtonEventData eventData)
                        {{
                            ValueInputs.Add();
                            RefreshLogixBox();
                        }}
                        [SyncMethod]
                        private void Remove(IButton button, ButtonEventData eventData)
                        {{
                            if (ValueInputs.Count <= 2) return;
                            ValueInputs.RemoveAt(ValueInputs.Count - 1);
                            RefreshLogixBox();
                        }}
                    }}
                }}
                ";
				context.AddSource($"MinMax_{name}.g.cs", gen);
			}
		}
	}
}