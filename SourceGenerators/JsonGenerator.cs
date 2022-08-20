using System.Linq;
using Microsoft.CodeAnalysis;

namespace SourceGenerators
{
    [Generator]
    public class JsonGenerator : ISourceGenerator
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
                "long",
                "uint",
                "ulong",
                "float",
                "double",
            };
            foreach (var type in types)
            {
                var name = type.First().ToString().ToUpper() + type.Substring(1);;
                //var hidden = type != "float" ? "[HiddenNode]" : "";
                var gen = $@"//generated
using Newtonsoft.Json.Linq;
namespace FrooxEngine.LogiX.Json
{{
    [HiddenNode]
    [NodeName(""Add Number"")]
    [NodeOverload(""JSONAddObject"")]
    [Category(""LogiX/Json"")]
    public class JSONAddNumber_{name} : LogixOperator<JObject>
    {{
        public readonly Input<JObject> Input;
        public readonly Input<string> Tag;
        public readonly Input<{type}> Object;
        public override JObject Content
        {{
            get
            {{
                var input = Input.EvaluateRaw();
                if (input == null) return null;
                var tag = Tag.EvaluateRaw();
                if (string.IsNullOrEmpty(tag)) return input;
                var in2 = (JObject)input.DeepClone();
                in2[tag] = Object.EvaluateRaw();
                return in2;
            }}
        }}
    }}
}}

";
                
                context.AddSource($"JSONAddNumber_{name}.g.cs", gen);
            }
        }
    }
}