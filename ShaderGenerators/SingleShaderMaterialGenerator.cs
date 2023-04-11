using System.Text;

namespace ShaderGenerators;

internal class SingleShaderMaterialGenerator
{


    public static void Main(string[] args)
    {
        Console.WriteLine("Starting...");

        string path = args.Length == 0 ? ShaderUitls.ShaderOutputPath : args[0];
        Directory.CreateDirectory(ShaderUitls.ShaderOutputPath);

        Console.WriteLine($"Got {Path.GetFullPath(path)} as target directory.");

        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"Directory {path} not found.");
        }

        int counter = 0;
        foreach (var file in Directory.EnumerateFiles(path))
        {
            if (Path.GetExtension(file) != ".shader")
            {
                continue;
            }

            GenerateMaterialClass(file);
            counter++;
        }

        Console.WriteLine($"Parsed {counter} files");
    }

    private static void GenerateMaterialClass(string path)
    {
        var parsedShader = UnityShaderParser.Parse(path);
        StringBuilder builder = new StringBuilder();

        ExtractMaterialHeader(ref parsedShader, ref builder);
        ExtractSyncMembers(ref parsedShader, ref builder);
        ExtractMaterialProperties(ref parsedShader, ref builder);
        ExtractUpdateMaterial(ref parsedShader, ref builder);
        ExtractMaterialFooter(ref parsedShader, ref builder);

        GenerateAttachClass(ref parsedShader, ref builder);
    }

    private static void ExtractMaterialFooter(ref UnityShaderParser.ShaderInfo parsedShader, ref StringBuilder builder)
    {
        builder.AppendLine("    protected override void UpdateKeywords(ShaderKeywords keywords) { }");
        builder.AppendLine("}");
        Console.WriteLine($"Parsed {parsedShader.Name}, saving as {parsedShader.Name}Material.cs and {parsedShader.Name}Attach.cs");
        File.WriteAllText(Path.Combine(ShaderUitls.ShaderOutputPath, $"{parsedShader.Name}Material.cs"), builder.ToString());
    }

    private static void ExtractMaterialHeader(ref UnityShaderParser.ShaderInfo parsedShader, ref StringBuilder builder)
    {
        foreach (var @namespace in ShaderUitls.Namespaces)
        {
            builder.AppendLine($"using {@namespace};");
        }

        builder.AppendLine();
        builder.AppendLine($"[Category(new string[] {{ \"Assets/Materials/NeosPlus/{parsedShader.Name}\"}})]");
        builder.AppendLine($"public partial class {parsedShader.Name}Material : SingleShaderMaterialProvider");
        builder.AppendLine("{");
        builder.AppendLine("    protected override Uri ShaderURL => ShaderInjection.UnlitDisplacement;");
    }

    private static void ExtractSyncMembers(ref UnityShaderParser.ShaderInfo parsedShader, ref StringBuilder builder)
    {
        foreach (var item in parsedShader.Properties)
        {
            if (item.DefaultValue is not null &&
                item.Type != "2D" &&
                item.Type != "Color")
            {
                builder.AppendLine($"    [DefaultValue({item.DefaultValue}f)]");
            }

            if (item.Range is not null)
            {
                builder.AppendLine($"    [Range({item.Range.Min}, {item.Range.Max})]");
            }

            // Unity to Neos types
            var lowerName = item.PropertyName.TrimStart('_');
            switch (item.Type)
            {
                case "2D":
                    builder.AppendLine($"    public readonly AssetRef<ITexture2D> {lowerName};");
                    break;
                case "Float":
                    builder.AppendLine($"    public readonly Sync<float> {lowerName};");
                    break;
                case "Color":
                    builder.AppendLine($"    public readonly Sync<color> {lowerName};");
                    break;
                case "Int":
                    builder.AppendLine($"    public readonly Sync<int> {lowerName};");
                    break;
            }
        }
        builder.AppendLine();
    }

    private static void ExtractUpdateMaterial(ref UnityShaderParser.ShaderInfo parsedShader, ref StringBuilder builder)
    {
        builder.AppendLine("    protected override void UpdateMaterial(Material material)");
        builder.AppendLine("    {");
        foreach (var item in parsedShader.Properties)
        {
            builder.AppendLine($"       material.UpdateTexture({item.PropertyName}, {item.PropertyName.TrimStart('_')});");
        }
        builder.AppendLine("    }");
        builder.AppendLine();
    }

    private static void ExtractMaterialProperties(ref UnityShaderParser.ShaderInfo parsedShader, ref StringBuilder builder)
    {
        foreach (var item in parsedShader.Properties)
        {
            builder.AppendLine($"    private static MaterialProperty {item.PropertyName} = new MaterialProperty(\"{item.PropertyName}\");");
        }
        builder.AppendLine();
    }

    private static void GenerateAttachClass(ref UnityShaderParser.ShaderInfo parsedShader, ref StringBuilder builder)
    {
        builder.Clear();
        foreach (var @namespace in ShaderUitls.Namespaces)
        {
            builder.AppendLine($"using {@namespace};");
        }

        builder.AppendLine();
        builder.AppendLine($"[Category(new string[] {{ \"Assets/Materials/NeosPlus/{parsedShader.Name}\"}})]");
        builder.AppendLine($"public partial class {parsedShader.Name}Material : SingleShaderMaterialProvider");
        builder.AppendLine("{");
        builder.AppendLine("    protected override void OnAttach()");
        builder.AppendLine("    {");
        builder.AppendLine("        base.OnAttach();");
        builder.AppendLine("    }");
        builder.AppendLine("}");

        File.WriteAllText(Path.Combine(ShaderUitls.ShaderOutputPath, $"{parsedShader.Name}Attach.cs"), builder.ToString());
        builder.Clear();
    }
}
