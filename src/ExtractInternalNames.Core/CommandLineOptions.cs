namespace ExtractInternalNames.Core;

// Values bound by System.CommandLine before invoking the extraction workflow.
public sealed class CommandLineOptions
{
    public bool List { get; internal set; }
    public bool FromCatalog { get; internal set; }
    public bool DryRun { get; internal set; }
    public bool Check { get; internal set; }
    public bool Build { get; internal set; }
    public string Output { get; internal set; }
    public string Catalog { get; internal set; }
    public string Connect { get; internal set; } = "auto";
    public int TimeoutSeconds { get; internal set; } = 120;
    public HashSet<string> Only { get; internal set; }
    public Dictionary<string, string> Templates { get; } = new(StringComparer.OrdinalIgnoreCase);

    public static string FindRepository(string start)
    {
        for (var directory = new DirectoryInfo(start); directory != null; directory = directory.Parent)
            if (File.Exists(Path.Combine(directory.FullName, "Inventor.InternalNames.sln")))
            {
                return directory.FullName;
            }

        return null;
    }
}
