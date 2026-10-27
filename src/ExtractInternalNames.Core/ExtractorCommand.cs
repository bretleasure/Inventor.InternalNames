using System.CommandLine;

namespace ExtractInternalNames.Core;

public static class ExtractorCommand
{
    public static RootCommand Create(Func<CommandLineOptions, int> execute, IEnumerable<string> categoryIds)
    {
        var categories = new HashSet<string>(categoryIds, StringComparer.Ordinal);
        var output = new Option<DirectoryInfo>("--output") { Description = "Destination for generated .cs files (default: repository library)." };
        var catalog = new Option<FileInfo>("--catalog") { Description = "Persistent catalog (default: repository or bundled Names.xml)." };
        catalog.AcceptExistingOnly();
        var only = new Option<string[]>("--only")
        {
            Description = "Category IDs, separated by commas or spaces: " + string.Join(", ", categories.OrderBy(c => c, StringComparer.Ordinal)),
            Arity = ArgumentArity.OneOrMore,
            AllowMultipleArgumentsPerToken = true
        };
        only.Validators.Add(result =>
        {
            if (result.Errors.Any()) return;
            foreach (var id in SplitCategories(result.GetValue(only)))
            {
                if (!categories.Contains(id))
                {
                    result.AddError(string.IsNullOrEmpty(id) ? "--only requires nonempty category IDs." : $"Unknown category: {id}");
                }
            }
        });
        var connect = new Option<string>("--connect")
        {
            Description = "Attach or start Inventor, require an existing instance, or start a dedicated instance.",
            DefaultValueFactory = _ => "auto"
        };
        connect.AcceptOnlyFromAmong("auto", "existing", "new");
        var timeout = new Option<int>("--timeout")
        {
            Description = "Readiness and rejected-call retry timeout in seconds (1–3600).",
            DefaultValueFactory = _ => 120
        };
        timeout.Validators.Add(result =>
        {
            // Leave type/arity failures to System.CommandLine; validate only the numeric range.
            int seconds;
            try { seconds = result.GetValue(timeout); }
            catch (InvalidOperationException) { return; }
            if (seconds < 1 || seconds > 3600)
            {
                result.AddError("--timeout must be between 1 and 3600 seconds.");
            }
        });
        var templates = new Option<string[]>("--template")
        {
            Description = "Template as part|assembly|drawing|presentation=path; repeat for multiple document types.",
            Arity = ArgumentArity.OneOrMore,
            AllowMultipleArgumentsPerToken = false
        };
        templates.Validators.Add(result =>
        {
            if (result.Errors.Any()) return;
            foreach (var value in result.GetValue(templates) ?? Array.Empty<string>())
            {
                var pair = value.Split(new[] { '=' }, 2);
                if (pair.Length != 2 || !new[] { "part", "assembly", "drawing", "presentation" }.Contains(pair[0]))
                {
                    result.AddError("--template must be part|assembly|drawing|presentation=path.");
                }
                else if (!File.Exists(pair[1]))
                {
                    result.AddError($"Template does not exist: {pair[1]}");
                }
            }
        });
        var fromCatalog = new Option<bool>("--from-catalog") { Description = "Regenerate without connecting to Inventor." };
        var dryRun = new Option<bool>("--dry-run") { Description = "Report changes without writing." };
        var check = new Option<bool>("--check") { Description = "Write nothing; return 2 when updates are needed." };
        var build = new Option<bool>("--build") { Description = "Run dotnet build on the destination library after writing." };
        var list = new Option<bool>("--list") { Description = "List extractors without connecting to Inventor." };
        var command = new RootCommand("Extract Inventor internal names and update constant classes.\n" +
            "Live extraction requires Windows x64 and Inventor. Exit codes: 0 success, 1 failure, 2 updates needed (--check).")
        {
            output, catalog, only, connect, timeout, templates, fromCatalog, dryRun, check, build, list
        };
        command.Validators.Add(result =>
        {
            if (new Option[] { build, dryRun, check }.Any(option => result.GetResult(option)?.Errors.Any() == true)) return;
            if (result.GetValue(build) && (result.GetValue(dryRun) || result.GetValue(check)))
            {
                result.AddError("--build cannot be combined with --dry-run or --check.");
            }
        });
        // Synchronous invocation keeps all Inventor work on the entry point's STA thread.
        command.SetAction(result =>
        {
            var selected = SplitCategories(result.GetValue(only)).ToArray();
            var options = new CommandLineOptions
            {
                List = result.GetValue(list), FromCatalog = result.GetValue(fromCatalog),
                DryRun = result.GetValue(dryRun), Check = result.GetValue(check), Build = result.GetValue(build),
                Output = result.GetValue(output)?.FullName, Catalog = result.GetValue(catalog)?.FullName,
                Connect = result.GetValue(connect), TimeoutSeconds = result.GetValue(timeout),
                Only = selected.Length == 0 ? null : new HashSet<string>(selected, StringComparer.Ordinal)
            };
            foreach (var value in result.GetValue(templates) ?? Array.Empty<string>())
            {
                var pair = value.Split(new[] { '=' }, 2);
                options.Templates[pair[0]] = Path.GetFullPath(pair[1]);
            }
            return execute(options);
        });
        return command;
    }

    private static IEnumerable<string> SplitCategories(IEnumerable<string> values) =>
        (values ?? Array.Empty<string>()).SelectMany(value => value.Split(',')).Select(id => id.Trim());
}
