using ExtractInternalNames.Core;
using Xunit;

namespace ExtractInternalNames.Core.Tests;

public sealed class CommandLineTests
{
    [Fact]
    public void ParsesCategorySelectionAndConnectionOptions()
    {
        var options = CommandLineOptions.Parse(new[] { "--only", "commands,ribbons", "--connect", "new", "--timeout", "30", "--dry-run" });
        Assert.Equal(new[] { "commands", "ribbons" }, options.Only.OrderBy(x => x).ToArray());
        Assert.Equal("new", options.Connect);
        Assert.Equal(30, options.TimeoutSeconds);
        Assert.True(options.DryRun);
    }

    [Theory]
    [InlineData("--output")]
    [InlineData("--connect", "arbitrary")]
    [InlineData("--timeout", "0")]
    [InlineData("--timeout", "abc")]
    [InlineData("--only", "commands,")]
    [InlineData("--build", "--check")]
    [InlineData("--build", "--dry-run")]
    [InlineData("--unknown")]
    public void RejectsInvalidOptions(params string[] args) =>
        Assert.Throws<ArgumentException>(() => CommandLineOptions.Parse(args));

    [Fact]
    public void FindsRepositoryFromANestedDirectory()
    {
        var root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        try
        {
            var nested = Path.Combine(root, "src", "tool");
            Directory.CreateDirectory(nested);
            File.WriteAllText(Path.Combine(root, "Inventor.InternalNames.sln"), "");
            Assert.Equal(root, CommandLineOptions.FindRepository(nested));
        }
        finally { Directory.Delete(root, true); }
    }
}
