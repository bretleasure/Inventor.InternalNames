using System;
using System.Collections.Generic;
using System.Text;

namespace Inventor.InternalNames.AssetLibraries
{
    /// <summary>
    /// Utility class for discovering Asset Library IDs at runtime
    /// </summary>
    public static class AssetLibraryDiscovery
    {
        /// <summary>
        /// Generates C# constants for all available asset libraries.
        /// Call this method from within an Inventor add-in to discover all asset library IDs.
        /// 
        /// Example usage in VB.NET:
        /// For Each lib As AssetLibrary In ThisApplication.AssetLibraries
        ///     Debug.Print("public const string " + lib.DisplayName.Replace(" ", "") + " = """ + lib.InternalName + """;")
        /// Next
        /// 
        /// Example usage in C#:
        /// foreach (AssetLibrary lib in ThisApplication.AssetLibraries)
        /// {
        ///     Console.WriteLine($"public const string {lib.DisplayName.Replace(" ", "")} = \"{lib.InternalName}\";");
        /// }
        /// </summary>
        /// <param name="assetLibrariesCollection">The AssetLibraries collection from Inventor Application</param>
        /// <returns>C# code with constants for all asset libraries</returns>
        public static string GenerateAssetLibraryConstants(object assetLibrariesCollection)
        {
            var sb = new StringBuilder();
            sb.AppendLine("// Auto-generated Asset Library IDs");
            sb.AppendLine("// Add these constants to AssetLibraryIds.cs");
            sb.AppendLine();

            try
            {
                // Use reflection to iterate through the collection
                var enumerator = assetLibrariesCollection.GetType().GetMethod("GetEnumerator")?.Invoke(assetLibrariesCollection, null);
                if (enumerator != null)
                {
                    var moveNext = enumerator.GetType().GetMethod("MoveNext");
                    var current = enumerator.GetType().GetProperty("Current");

                    while ((bool)moveNext!.Invoke(enumerator, null)!)
                    {
                        var lib = current!.GetValue(enumerator, null);
                        if (lib != null)
                        {
                            var displayNameProp = lib.GetType().GetProperty("DisplayName");
                            var internalNameProp = lib.GetType().GetProperty("InternalName");

                            var displayName = displayNameProp?.GetValue(lib, null)?.ToString() ?? "Unknown";
                            var internalName = internalNameProp?.GetValue(lib, null)?.ToString() ?? "";

                            if (!string.IsNullOrEmpty(internalName))
                            {
                                // Clean up the display name for use as a C# identifier
                                string constantName = displayName.Replace(" ", "").Replace("-", "");

                                sb.AppendLine($"/// <summary>");
                                sb.AppendLine($"/// Asset Library ID for {displayName}");
                                sb.AppendLine($"/// </summary>");
                                sb.AppendLine($"public const string {constantName} = \"{internalName}\";");
                                sb.AppendLine();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"// Error discovering asset libraries: {ex.Message}");
                sb.AppendLine("// Make sure to pass ThisApplication.AssetLibraries to this method");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets a list of all available asset library display names and their IDs.
        /// Call this method from within an Inventor add-in.
        /// </summary>
        /// <param name="assetLibrariesCollection">The AssetLibraries collection from Inventor Application</param>
        /// <returns>Dictionary mapping display names to internal IDs</returns>
        public static Dictionary<string, string> GetAssetLibraryIds(object assetLibrariesCollection)
        {
            var result = new Dictionary<string, string>();

            try
            {
                // Use reflection to iterate through the collection
                var enumerator = assetLibrariesCollection.GetType().GetMethod("GetEnumerator")?.Invoke(assetLibrariesCollection, null);
                if (enumerator != null)
                {
                    var moveNext = enumerator.GetType().GetMethod("MoveNext");
                    var current = enumerator.GetType().GetProperty("Current");

                    while ((bool)moveNext!.Invoke(enumerator, null)!)
                    {
                        var lib = current!.GetValue(enumerator, null);
                        if (lib != null)
                        {
                            var displayNameProp = lib.GetType().GetProperty("DisplayName");
                            var internalNameProp = lib.GetType().GetProperty("InternalName");

                            var displayName = displayNameProp?.GetValue(lib, null)?.ToString() ?? "Unknown";
                            var internalName = internalNameProp?.GetValue(lib, null)?.ToString() ?? "";

                            if (!string.IsNullOrEmpty(internalName))
                            {
                                result[displayName] = internalName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result["Error"] = ex.Message;
            }

            return result;
        }
    }
}