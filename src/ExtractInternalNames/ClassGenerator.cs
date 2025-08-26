using System.Text;

namespace ExtractInternalNames;

public static class ClassGenerator
{
    public static string GenerateCode(string className, Dictionary<string, string> constants)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"public struct {className}");
        sb.AppendLine("{");

        foreach (var kvp in constants.OrderBy(c => c.Key))
        {
            sb.AppendLine($"\tpublic const string {ToValidIdentifier(kvp.Key)} = \"{kvp.Value}\";");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }
    
    private static string ToValidIdentifier(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "Empty";

        var sb = new StringBuilder();

        foreach (char c in input)
        {
            if (char.IsLetterOrDigit(c) || c == '_')
                sb.Append(c);
            // else skip invalid chars
        }

        string result = sb.ToString();

        if (string.IsNullOrEmpty(result))
            result = "Empty";

        // If starts with digit, add 'e'
        if (char.IsDigit(result[0]))
            result = "e" + result;

        // Optionally, make first letter uppercase for style
        // result = char.ToUpper(result[0]) + result.Substring(1);

        return result;
    }
}