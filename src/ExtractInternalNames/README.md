# ExtractInternalNames

A Windows x64 console application that extracts names through Inventor's COM API and updates the `Inventor.InternalNames` source files. No add-in installation or copying generated text is required.

The CLI uses `System.CommandLine` for option parsing, generated help, and validation. Run `--help` (or `-h`) to see available options and defaults.

## Build and run

Build with the .NET 10 SDK, then run on an Inventor workstation from the repository root:

```powershell
dotnet build src/ExtractInternalNames/ExtractInternalNames.csproj -c Release
.\src\ExtractInternalNames\bin\Release\net10.0-windows\ExtractInternalNames.exe --dry-run
.\src\ExtractInternalNames\bin\Release\net10.0-windows\ExtractInternalNames.exe
```

The project also builds a .NET Framework 4.7.2 executable under `bin/Release/net472`. The modern executable requires the .NET 10 x64 runtime. These are out-of-process applications; their runtime does not need to match Inventor's in-process add-in runtime. Compatibility with each Inventor release must still be verified on that release. The bundled interop reference is the one already used by this repository.

To move the utility to another computer, copy the **entire target output directory**, including dependency DLLs, configuration/runtime files, and `Names.xml`. Running it does not require an SDK; `--build` additionally requires a .NET SDK.

The default destination is `src/Inventor.InternalNames`, discovered by walking up from the working directory or executable directory to the solution. The repository's `src/ExtractInternalNames/Names.xml` is the default catalog. Outside a checkout, specify a destination; the bundled catalog becomes the default:

```powershell
.\ExtractInternalNames.exe --output C:\Repos\Inventor.InternalNames\src\Inventor.InternalNames --catalog C:\Repos\Inventor.InternalNames\src\ExtractInternalNames\Names.xml
```

Use an explicit `--catalog` when maintaining another checkout or output directory so its public-name history stays with those files. Commit the updated catalog together with the generated source.

## Options

| Option | Behavior |
|---|---|
| No options | Attach to running Inventor, or start it; extract all categories and update classes and catalog |
| `--dry-run` | Extract and validate, then report proposed changes without writing |
| `--check` | Write nothing; return exit code 2 when updates are needed |
| `--from-catalog` | Regenerate without Inventor; also works on non-Windows systems using the modern DLL |
| `--only commands,ribbons` | Run selected categories and generate their files; space-separated IDs are also accepted |
| `--list` | List category IDs without connecting to Inventor |
| `--connect auto`, `existing`, or `new` | Attach or start, require a running instance, or start a dedicated instance |
| `--timeout 120` | Readiness and rejected-call retry timeout in seconds; does not bound a hung COM activation or method call |
| `--template drawing=C:\Templates\Special.idw` | Use a supplied template; repeat for `part`, `assembly`, `drawing`, or `presentation` |
| `--output PATH` | Override the source destination |
| `--catalog PATH` | Override the persistent catalog |
| `--build` | Compile the destination library after writing; requires its `.csproj` and a .NET SDK |
| `--help` | Show usage |
| `--version` | Show version information without connecting to Inventor |

Exit codes: **0** success, **1** failure, **2** differences under `--check`. Preview modes still create temporary documents during live extraction. `--from-catalog` avoids Inventor entirely.

If multiple Inventor processes are running, attach mode stops instead of choosing an arbitrary session. Close extra instances or use `--connect new`. The tool restores the previous active document and silent-operation setting, closes only documents it created without saving, and quits Inventor only if it started the instance.

If `--build` fails, generated files remain written and the command reports failure. Extraction, catalog conflicts, and generated-code validation failures occur before writing. File updates stage all contents first and restore previous bytes if applying a replacement fails. A process crash or power loss during replacement is not a transaction across all files.

## Extraction coverage

| Category | Source |
|---|---|
| `commands` | Control definitions, after loading document contexts |
| `environments` | Available application environments, after loading document contexts |
| `addins` | Registered application add-ins; does not activate every add-in |
| `asset-libraries` | Available material and appearance libraries |
| `asset-properties` | Property keys across library appearance, material, and physical assets |
| `ribbons` | Ribbons, tabs, and panels observed initially and with temporary Part, Assembly, Drawing, and Presentation documents active |
| `properties` | Property-set and iProperty names from Part, Assembly, and Drawing templates |
| `defaults` | Coordinate-system work features, primary model states when supported, and primary/default design views; retain curated iLogic attribute keys |

Results reflect the installed Inventor version, libraries, add-ins, templates, and loaded environments. Specialized contexts such as routed systems or sheet metal may expose additional names. A blank drawing does not contain every property of every subtype. Supply representative templates and retain observations from other runs rather than treating one run as exhaustive.

Existing property/default constants contain English names, so `properties` and `defaults` require English (`en-US`) Inventor. Other categories use invariant identities where available, although new public-member labels can come from the current language. Existing public names remain fixed.

## Catalog and generation

`Names.xml` records each category, output path, namespace, class hierarchy, and constant. Constants have a public `name`, string `value`, and optional discovery `key`. Groups also have stable keys. Some property identities bind on first discovery because the original files did not record their FMTIDs and property IDs.

- Keep existing public names, aliases, values, and nesting.
- Add new observations using internal identities. Sanitize new C# identifiers and add a stable identity suffix for collisions.
- Retain entries absent from this installation, including unselected categories.
- Fail on a changed value for a known identity so it can be reviewed explicitly.
- Keep property-set values as historical readable names; FMTIDs identify observations rather than replace those values.

Generation emits complete static classes, sorts ordinally, escapes string literals, validates code with Roslyn, and writes only changed files. It includes no changing timestamps. The migration preserves all 4,156 existing constants.

Edit the catalog for intentional public-name changes, aliases, or curated constants. Avoid editing generated `.cs` files directly. Removing a catalog entry is an explicit API change; a live run re-adds it if discovered again.

## Add a new category

1. Add an `INameExtractor` implementation under `Extractors` with a unique `Id` and description.
2. Return groups through `context.Group`, supplying a stable group key, relative `.cs` path, class path, and namespace. A class path such as `FeatureNames.Special` generates a nested static class.
3. Add observations through `ExtractionContext.Add(group, stableKey, suggestedPublicName, stringValue)`. Prefer internal names or GUIDs as keys. Scope keys to the group; use composite identities such as property-set FMTID plus property ID when needed.
4. Register it in `ExtractorRegistry.Create`. It becomes available to `--list`, `--only`, and default runs without changing the CLI, renderer, or writer.
5. Add offline tests for new generation/merge rules and live verification for new Inventor queries.

Extractors can create multiple files/groups. New groups do not need pre-existing catalog entries. Use the context's temporary documents instead of modifying the user's active document, and keep COM calls on the entry point's STA thread.

## Offline verification

```sh
dotnet build Inventor.InternalNames.sln -c Release
dotnet test tests/ExtractInternalNames.Core.Tests -c Release --no-build
dotnet src/ExtractInternalNames/bin/Release/net10.0-windows/ExtractInternalNames.dll --from-catalog --check
```

Tests cover preservation of existing constants, compilable output, aliases, missing entries, collisions, escaped/control characters, repeatability, CLI parsing, paths, and rollback after a failed update.

## Live acceptance checks

Live extraction has not been exercised on the implementation machine because Inventor is not installed. On each supported Inventor release:

1. Remove installed copies of the old extractor add-in so its custom ribbon does not enter the results.
2. Run `--connect new --dry-run`. Confirm the version, template availability, results, and cleanup of the started process.
3. Extract into a temporary output directory using a **copy** of `Names.xml`. Build the generated library and review additions and retained entries. Review known-identity value conflicts before changing the catalog.
4. Repeat the run and confirm no file changes.
5. Use `--connect existing` with a document containing unsaved edits. Confirm it stays open, edits remain, and the active document and application settings are restored.
6. Check missing templates, unavailable libraries, busy/modal Inventor, multiple instances, and a machine without Inventor. Failures should report the relevant category and leave source files unchanged.
