using System.Text;

namespace SourceGenerators.ShaderGen;

internal class SingleShaderMaterialGenerator
{
    private static readonly string[] Namespaces = new string[]
    {
        "System",
        "BaseX",
        "FrooxEngine",
        "NEOSPlus.Shaders"
    };

    private const string ShaderInputPath = @"shaders"; // TODO Use me!
    private const string ShaderOutputPath = @"parsedShaders";

    public static void Main(string[] args)
    {
        Console.WriteLine("Starting...");

        string path = args.Length == 0 ? ShaderOutputPath : args[0];
        Directory.CreateDirectory(ShaderOutputPath);

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

            ParseShader(file);
            counter++;
        }

        Console.WriteLine($"Parsed {counter} files");
    }

    private static void ParseShader(string path)
    {
        var parsedShader = UnityShaderParser.Parse(path);

        StringBuilder builder = new StringBuilder();

        foreach (var @namespace in Namespaces)
        {
            builder.AppendLine($"using {@namespace};");
        }

        builder.AppendLine();
        builder.AppendLine($"[Category(new string[] {{ \"Assets/Materials/NeosPlus/{parsedShader.Name}\"}})]");
        builder.AppendLine($"public class {parsedShader.Name}Material : SingleShaderMaterialProvider");
        builder.AppendLine("{");
        builder.AppendLine("    protected override Uri ShaderURL => ShaderInjection.UnlitDisplacement;");

        // Define Sync Members
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

        // Define Material Properties
        foreach (var item in parsedShader.Properties)
        {
            builder.AppendLine($"    private static MaterialProperty {item.PropertyName} = new MaterialProperty(\"{item.PropertyName}\");");
        }
        builder.AppendLine();

        // Define UpdateMaterial
        builder.AppendLine("    protected override void UpdateMaterial(Material material)");
        builder.AppendLine("    {");
        foreach (var item in parsedShader.Properties)
        {
            builder.AppendLine($"       material.UpdateTexture({item.PropertyName}, {item.PropertyName.TrimStart('_')});");
        }
        builder.AppendLine("    }");
        builder.AppendLine();

        builder.AppendLine("    protected override void UpdateKeywords(ShaderKeywords keywords) { }");
        builder.AppendLine("}");

        Console.WriteLine($"Parsed {parsedShader.Name}, saving as {parsedShader.Name}Material.cs");
        File.WriteAllText(Path.Combine(ShaderOutputPath, $"{parsedShader.Name}Material.cs"), builder.ToString());
    }
}
