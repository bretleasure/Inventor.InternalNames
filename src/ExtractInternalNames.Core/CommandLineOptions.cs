namespace ExtractInternalNames.Core;

public sealed class CommandLineOptions
{
    public bool Help { get; private set; }
    public bool List { get; private set; }
    public bool FromCatalog { get; private set; }
    public bool DryRun { get; private set; }
    public bool Check { get; private set; }
    public bool Build { get; private set; }
    public string Output { get; private set; }
    public string Catalog { get; private set; }
    public string Connect { get; private set; } = "auto";
    public int TimeoutSeconds { get; private set; } = 120;
    public HashSet<string> Only { get; private set; }
    public Dictionary<string, string> Templates { get; } = new(StringComparer.OrdinalIgnoreCase);

    public static CommandLineOptions Parse(string[] args)
    {
        var options = new CommandLineOptions();
        for (var i = 0; i < args.Length; i++)
        {
            string Value()
            {
                if (++i >= args.Length || args[i].StartsWith("--", StringComparison.Ordinal))
                    throw new ArgumentException($"Missing value for {args[i - 1]}.");
                return args[i];
            }
            switch (args[i])
            {
                case "--help": case "-h": options.Help = true; break;
                case "--list": options.List = true; break;
                case "--from-catalog": options.FromCatalog = true; break;
                case "--dry-run": options.DryRun = true; break;
                case "--check": options.Check = true; break;
                case "--build": options.Build = true; break;
                case "--output": options.Output = Path.GetFullPath(Value()); break;
                case "--catalog": options.Catalog = Path.GetFullPath(Value()); break;
                case "--connect":
                    options.Connect = Value();
                    if (!new[] { "auto", "existing", "new" }.Contains(options.Connect))
                        throw new ArgumentException("--connect must be auto, existing, or new.");
                    break;
                case "--timeout":
                    if (!int.TryParse(Value(), out var seconds) || seconds < 1 || seconds > 3600)
                        throw new ArgumentException("--timeout must be between 1 and 3600 seconds.");
                    options.TimeoutSeconds = seconds;
                    break;
                case "--only":
                    options.Only = new HashSet<string>(Value().Split(',').Select(s => s.Trim()), StringComparer.Ordinal);
                    if (options.Only.Any(string.IsNullOrEmpty)) throw new ArgumentException("--only requires category IDs separated by commas.");
                    break;
                case "--template":
                    var pair = Value().Split(new[] { '=' }, 2);
                    if (pair.Length != 2 || !new[] { "part", "assembly", "drawing", "presentation" }.Contains(pair[0]))
                        throw new ArgumentException("--template must be part|assembly|drawing|presentation=path.");
                    var path = Path.GetFullPath(pair[1]);
                    if (!File.Exists(path)) throw new ArgumentException($"Template does not exist: {path}");
                    options.Templates[pair[0]] = path;
                    break;
                default: throw new ArgumentException($"Unknown option: {args[i]}. Use --help.");
            }
        }
        if (options.Build && (options.DryRun || options.Check))
            throw new ArgumentException("--build cannot be combined with --dry-run or --check.");
        return options;
    }

    public static string FindRepository(string start)
    {
        for (var directory = new DirectoryInfo(start); directory != null; directory = directory.Parent)
            if (File.Exists(Path.Combine(directory.FullName, "Inventor.InternalNames.sln"))) return directory.FullName;
        return null;
    }
}
