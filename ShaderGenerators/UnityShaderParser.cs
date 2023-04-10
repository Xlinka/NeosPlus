using System.Text.RegularExpressions;

namespace SourceGenerators.ShaderGen;

internal static class UnityShaderParser
{
    private static readonly Regex shaderNameRegex = new Regex(@"^Shader\s*""(\S+\s*\b)+", RegexOptions.Multiline);
    private static readonly Regex propertyRegex = new Regex(@"(?<=Properties\s*\{)(?:.*\n?)*?(?=\s*\})", RegexOptions.Multiline);

    public static ShaderInfo Parse(string path)
    {
        var shaderInfo = new ShaderInfo();
        string text = File.ReadAllText(path);

        // Get the shader name(s)
        var match = shaderNameRegex.Match(text);
        if (match.Success)
        {
            string propertyName;
            if (match.Value.Contains(" "))
            {
                propertyName = match.Groups[0].Value.Replace(" ", "");
            }
            else
            {
                propertyName = match.Groups[1].Value;
            }

            shaderInfo.FullName = propertyName;
            var nameSplit = propertyName.Split("/");
            if (nameSplit.Length > 1)
            {
                shaderInfo.Name = nameSplit[1];
            }
        }

        // Get the shader properties
        // First, get the properties block as a substring
        var propertyMatches = propertyRegex.Match(text);
        if (propertyMatches.Success)
        {
            string matchedSubstring = text.Substring(propertyMatches.Index, propertyMatches.Length);
            string[] propertySplit = matchedSubstring.
                Split(new string[] { "\r\n" }, StringSplitOptions.None).
                Where(s => !string.IsNullOrEmpty(s)).
                Select(s => s.Replace("[HDR]", "")).
                Select(s => s.Trim()).
                ToArray();
            
            foreach (var property in propertySplit)
            {
                var propertyInfo = new ShaderPropertyInfo();

                // Get the first substring from the start of the string to the first space.
                // This will be the property name
                propertyInfo.PropertyName = property.Substring(
                        0, 
                        Math.Min(property.IndexOf(" "), property.IndexOf("("))).
                    Replace(" ", "");

                // Get the first substring contained by the last pair of parentheses. This will be the HumanName and Type,
                // Seperated by a comma and will always contain 2 elements
                var start = property.IndexOf("(\"");
                var end = property.IndexOf(") = ");
                var humanNameAndType = property.Substring(start, end - start); // This is awful
                var humanNameAndTypeSplit = humanNameAndType.Split(",");
                if (humanNameAndTypeSplit.Length == 2)
                {
                    propertyInfo.HumanName = humanNameAndTypeSplit[0].Trim();
                    propertyInfo.Type = humanNameAndTypeSplit[1].Trim();
                }
                if (humanNameAndTypeSplit.Length == 3) // Handle weird inline range attribute for exponents
                {
                    propertyInfo.HumanName = humanNameAndTypeSplit[0].Trim();
                    propertyInfo.PropertyName = "_" + propertyInfo.HumanName.
                        Replace(")", "").
                        Replace("(", "").
                        Replace("\"", "").
                        Replace(" ", "");
                    propertyInfo.Type = "Float";
                }

                // Get the default value. This will be the substring of the only "="
                // To the end of the string after the HumanName and Type above
                var defaultValue = property.Substring(property.IndexOf("=") + 1);
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    propertyInfo.DefaultValue = defaultValue.Trim();
                }

                shaderInfo.Properties.Add(propertyInfo);
            }
        }
        
        return shaderInfo;
    }

    public class ShaderInfo
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public List<ShaderPropertyInfo> Properties { get; } = new List<ShaderPropertyInfo>();
    }

    public class ShaderPropertyInfo
    {
        public string PropertyName { get; set; }
        public string HumanName { get; set; }
        public string Type { get; set; }
        public object? DefaultValue { get; set; }
        public RangeAttribute? Range { get; set; }
    }

    public class RangeAttribute
    {
        public float Min { get; set; }
        public float Max { get; set; }
    }
}
