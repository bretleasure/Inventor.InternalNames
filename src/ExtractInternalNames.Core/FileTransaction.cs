using System.Text;

namespace ExtractInternalNames.Core;

/// <summary>Stages every change before replacing files and restores originals if applying fails.</summary>
public static class FileTransaction
{
    public static IReadOnlyList<string> ChangedFiles(IReadOnlyDictionary<string, string> files) => files
        .Where(f => !File.Exists(f.Key) || File.ReadAllText(f.Key) != f.Value)
        .Select(f => f.Key).OrderBy(p => p, StringComparer.Ordinal).ToArray();

    public static void Apply(IReadOnlyDictionary<string, string> files)
    {
        var changed = ChangedFiles(files);
        var staged = new Dictionary<string, string>();
        var backups = new Dictionary<string, byte[]>();
        var applied = new List<string>();
        var createdDirectories = new List<string>();
        try
        {
            foreach (var path in changed)
            {
                var directory = Path.GetDirectoryName(Path.GetFullPath(path));
                var missing = new Stack<string>();
                for (var d = directory; !Directory.Exists(d); d = Path.GetDirectoryName(d)) missing.Push(d);
                while (missing.Count > 0)
                {
                    var d = missing.Pop();
                    Directory.CreateDirectory(d);
                    createdDirectories.Add(d);
                }
                backups.Add(path, File.Exists(path) ? File.ReadAllBytes(path) : null);
                var temp = Path.Combine(directory, ".names-" + Guid.NewGuid().ToString("N") + ".tmp");
                staged.Add(path, temp);
                File.WriteAllText(temp, files[path], new UTF8Encoding(false));
            }
            foreach (var path in changed)
            {
                if (backups[path] != null) File.Replace(staged[path], path, null);
                else File.Move(staged[path], path);
                applied.Add(path);
            }
        }
        catch (Exception original)
        {
            var failures = new List<Exception> { original };
            foreach (var path in applied.AsEnumerable().Reverse())
            {
                try
                {
                    if (backups[path] == null) File.Delete(path);
                    else File.WriteAllBytes(path, backups[path]);
                }
                catch (Exception rollback) { failures.Add(rollback); }
            }
            if (failures.Count > 1) throw new AggregateException("File update and rollback failed.", failures);
            throw;
        }
        finally
        {
            foreach (var temp in staged.Values)
                if (File.Exists(temp)) File.Delete(temp);
            foreach (var directory in createdDirectories.AsEnumerable().Reverse())
                if (Directory.Exists(directory) && !Directory.EnumerateFileSystemEntries(directory).Any()) Directory.Delete(directory);
        }
    }
}
