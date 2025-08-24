# Inventor.InternalNames

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

Inventor.InternalNames is a .NET Standard 2.0 class library that provides internal names as static string constants for Autodesk Inventor API components. This library contains no runtime behavior - only constants organized into namespaces for Ribbon components, Commands, PropertySets, and iProperties.

## Working Effectively

- Build and validate the library:
  - `dotnet restore` -- takes 8 seconds. NEVER CANCEL. Set timeout to 60+ seconds.
  - `dotnet build -c Release` -- takes 5 seconds. NEVER CANCEL. Set timeout to 60+ seconds.
  - `dotnet build -c Debug` -- takes 2 seconds. NEVER CANCEL. Set timeout to 60+ seconds.
  - `dotnet pack --output artifacts --configuration Release` -- takes 2 seconds. NEVER CANCEL. Set timeout to 60+ seconds.
- There are NO TESTS in this repository. The library contains only static constants, so no unit tests are needed.
- Always validate changes by creating a test consumer project that references the library and exercises the constants you've modified.

## Validation

- Always validate any changes by creating a simple test application:
  ```bash
  mkdir /tmp/test-consumer && cd /tmp/test-consumer
  dotnet new console
  dotnet add reference /path/to/Inventor.InternalNames.csproj
  # Create Program.cs that uses the constants you modified
  # Example validation code:
  # using Inventor.InternalNames;
  # using Inventor.InternalNames.Ribbon;
  # Console.WriteLine($"Testing: {InventorRibbons.Drawing}");
  # Console.WriteLine($"Command: {CommandNames.AssemblyBillOfMaterialsCmd}");
  # Console.WriteLine($"Property: {Inventor.InternalNames.iProperties.Part.Title}");
  dotnet build && dotnet run
  ```
- ALWAYS run through at least one complete validation scenario after making changes to constants.
- The library builds successfully on both Windows and Linux with .NET Standard 2.0.
- No linting, formatting, or code quality tools are configured. Follow the existing code style: public const string with descriptive names.

## Build Pipeline

The CI/CD process uses GitHub Actions (.github/workflows/build-deploy.yml):
- Builds on Windows using dotnet build
- Uses GitVersion for semantic versioning
- Publishes to both NuGet.org and GitHub Packages
- No tests are run because none exist
- Build takes approximately 1-2 minutes total in CI

## Common Tasks

### Repository Structure
```
Commands/                    - Command names for Inventor CommandManager
  CommandNames.cs           - Large file with 2200+ command constants
iProperties/                - Property names for Inventor iProperties
  Assembly.cs              - Assembly document iProperty names
  Drawing.cs               - Drawing document iProperty names  
  Part.cs                  - Part document iProperty names
PropertySets/               - Property set names for Inventor documents
  Assembly.cs              - Assembly property set constants
  Drawing.cs               - Drawing property set constants
  Part.cs                  - Part property set constants
Ribbon/                     - UI ribbon component internal names
  InventorRibbons.cs       - Main ribbon names (Part, Assembly, Drawing, etc.)
  *RibbonTabs.cs           - Tab names for each document type
  *RibbonPanels.cs         - Panel names organized by tabs and document type
```

### Key Files to Understand
- `README.md` - Documents the purpose and usage examples
- `Inventor.InternalNames.csproj` - .NET Standard 2.0 project targeting Autodesk Inventor API
- `.github/workflows/build-deploy.yml` - GitHub Actions build and deployment pipeline
- `Commands/CommandNames.cs` - Largest file with 2200+ lines of command constants

### Example Usage
The library is consumed like this:
```csharp
using Inventor.InternalNames;
using Inventor.InternalNames.Ribbon;

// Access ribbon names
var drawingRibbon = inventorApp.UserInterfaceManager.Ribbons[InventorRibbons.Drawing];
var placeViewsTab = drawingRibbon.RibbonTabs[DrawingRibbonTabs.PlaceViews];

// Access command names
commandManager.ExecuteCommand(CommandNames.AssemblyBillOfMaterialsCmd);

// Access iProperty names (note: fully qualified names required due to namespace conflicts)
part.PropertySets[Inventor.InternalNames.PropertySets.Part.SummaryInformation][Inventor.InternalNames.iProperties.Part.Title] = "New Title";
```

### Adding New Constants
When adding new constants:
1. Follow the existing naming pattern: descriptive PascalCase names
2. Use const string declarations in public structs
3. Organize by namespace (Commands, Ribbon, PropertySets, iProperties)
4. Always validate with a test consumer application
5. Build and pack to ensure no compilation errors
6. The constants should match exact internal names from Autodesk Inventor API

### Build Artifacts
- `bin/Release/netstandard2.0/Inventor.InternalNames.dll` - Compiled library
- `artifacts/Inventor.InternalNames.*.nupkg` - NuGet package for distribution
- No test outputs (no tests exist)

This is a simple constants library with fast build times. All operations complete in under 10 seconds locally.