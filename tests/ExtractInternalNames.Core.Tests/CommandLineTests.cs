using System.CommandLine;
using ExtractInternalNames.Core;
using Xunit;

namespace ExtractInternalNames.Core.Tests;

public sealed class CommandLineTests
{
    [Fact]
    public void ParsesCategorySelectionAndConnectionOptions()
    {
        CommandLineOptions options = null;
        var handlerThread = 0;
        var command = Create(parsed => { options = parsed; handlerThread = Environment.CurrentManagedThreadId; return 37; });
        var thread = Environment.CurrentManagedThreadId;
        Assert.Equal(37, command.Parse(new[] { "--only", "commands,ribbons", "--connect", "new", "--timeout", "30", "--dry-run" }).Invoke());
        Assert.Equal(thread, handlerThread);
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
    [InlineData("--timeout")]
    [InlineData("--only")]
    [InlineData("--template")]
    [InlineData("--check=invalid")]
    [InlineData("--dry-run=invalid")]
    [InlineData("--build=invalid")]
    [InlineData("--catalog", "missing-catalog.xml")]
    [InlineData("--only", "commands,")]
    [InlineData("--build", "--check")]
    [InlineData("--build", "--dry-run")]
    [InlineData("--unknown")]
    [InlineData("--only", "unknown")]
    [InlineData("--template", "invalid=path")]
    [InlineData("--template", "part=missing-template.ipt")]
    [InlineData("--timeout", "3601")]
    public void RejectsInvalidOptions(params string[] args)
    {
        var invoked = false;
        var command = Create(_ => { invoked = true; return 0; });
        var result = command.Parse(args);
        Assert.NotEmpty(result.Errors);
        using var output = new StringWriter();
        using var error = new StringWriter();
        Assert.Equal(1, result.Invoke(new InvocationConfiguration { Output = output, Error = error }));
        Assert.False(invoked);
        Assert.NotEmpty(error.ToString());
    }

    [Fact]
    public void DefaultsAreBoundBySystemCommandLine()
    {
        CommandLineOptions options = null;
        var command = Create(parsed => { options = parsed; return 0; });
        Assert.Equal(0, command.Parse(Array.Empty<string>()).Invoke());
        Assert.Equal("auto", options.Connect);
        Assert.Equal(120, options.TimeoutSeconds);
        Assert.Null(options.Only);
        Assert.False(options.DryRun);
    }

    [Theory]
    [InlineData("--help")]
    [InlineData("-h")]
    [InlineData("--version")]
    public void BuiltInHelpAndVersionDoNotInvokeExtraction(string argument)
    {
        var invoked = false;
        var command = Create(_ => { invoked = true; return 0; });
        using var output = new StringWriter();
        Assert.Equal(0, command.Parse(new[] { argument }).Invoke(new InvocationConfiguration { Output = output }));
        Assert.False(invoked);
        Assert.NotEmpty(output.ToString());
        if (argument != "--version")
        {
            Assert.Contains("--template", output.ToString());
            Assert.Contains("--timeout", output.ToString());
        }
    }

    [Fact]
    public void SupportsRepeatedTemplatesAndSpaceSeparatedCategories()
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            var part = Path.Combine(directory, "part template.ipt");
            var drawing = Path.Combine(directory, "drawing=template.idw");
            File.WriteAllText(part, "");
            File.WriteAllText(drawing, "");
            CommandLineOptions options = null;
            var command = Create(parsed => { options = parsed; return 0; });
            Assert.Equal(0, command.Parse(new[] { "--template", "part=" + part, "--template", "drawing=" + drawing,
                "--only", "commands", "ribbons", "--output", directory }).Invoke());
            Assert.Equal(part, options.Templates["part"]);
            Assert.Equal(drawing, options.Templates["drawing"]);
            Assert.Equal(directory, options.Output);
            Assert.Equal(2, options.Only.Count);
        }
        finally { Directory.Delete(directory, true); }
    }

    private static RootCommand Create(Func<CommandLineOptions, int> execute) =>
        ExtractorCommand.Create(execute, new[] { "commands", "ribbons" });

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
