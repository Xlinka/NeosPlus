using System.Linq;
using Microsoft.CodeAnalysis;

namespace SourceGenerators
{
    [Generator]
    public class DMGGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }
        public void Execute(GeneratorExecutionContext context)
        {
            var letterOrder = new[]
            {
                "B",
                "C",
                "D",
                "E",
                "H",
                "L",
                "F",
                "A"
            };
            var methods = new string[255];
            for (var i = 0; i < letterOrder.Length; i++)
            {
                var letter = letterOrder[i];
                if (letter == "F") continue;
                for (var j = 0; j < letterOrder.Length; j++)
                {
                    var letter2 = letterOrder[j];
                    if (letter == letter2) continue;
                    if (letter2 == "F") letter2 = "Memory.ReadByte(HL)";
                    var order = i * 8 + j;
                    methods[0x40 + order] = $"LD(out {letter}, {letter2});";
                }
            }
            for (var i = 0; i < letterOrder.Length; i++)
            {
                var letter = letterOrder[i];
                if (letter == "F") letter = "Memory.ReadByte(HL)";
                methods[0x80 + i] = $"ADD({letter});";
                methods[0x88 + i] = $"ADC({letter});";
                methods[0x90 + i] = $"SUB({letter});";
                methods[0x98 + i] = $"SBC({letter});";
                methods[0xA0 + i] = $"AND({letter});";
                methods[0xA8 + i] = $"XOR({letter});";
                methods[0xB0 + i] = $"OR({letter});";
                methods[0xB8 + i] = $"CP({letter});";
                if (letter == "Memory.ReadByte(HL)") continue;
                methods[0x04 + i * 8] = $"INC(ref {letter});";
                methods[0x05 + i * 8] = $"DEC(ref {letter});";
                methods[0x06 + i * 8] = $"LD(out {letter}, PC);";
            }

            methods[0x01] = "LD(DMGRegisters.BC, PC);";
            
            methods[0x07] = "RotateFeedCarry(0x80);";
            methods[0x0F] = "RotateFeedCarry(0x1);";
            methods[0x17] = "Rotate(0x80, 0x1);";
            methods[0x1F] = "Rotate(0x1, 0x80);";
            var insert = "";
            for (var i = 0; i < methods.Length; i++) insert += $"case {i}: {methods[i]} break;\n";
            var gen = $@"//generated
using System;
namespace NEOSPlus.Components.Emulation.DMG.Emulator;

public partial class DMGCore
{{
    partial void DoOpcode(byte op)
    {{
        switch (op)
        {{
{insert}
        }}
    }}
}}

";
            context.AddSource($"DMG.g.cs", gen);
        }
    }
}