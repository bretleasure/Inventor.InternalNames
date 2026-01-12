# Inventor.InternalNames

![GitHub Release](https://img.shields.io/github/v/release/bretleasure/inventor.internalnames?logo=github)
![GitHub Release](https://img.shields.io/github/v/release/bretleasure/inventor.internalnames?include_prereleases&logo=github&label=latest%20build)
![NuGet Downloads](https://img.shields.io/nuget/dt/inventor.internalnames?logo=nuget&color=9932CC&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FInventor.InternalNames)
![GitHub License](https://img.shields.io/github/license/bretleasure/inventor.internalnames?color=salmon)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/bretleasure/inventor.internalnames/build-deploy.yml?logo=github%20actions&logoColor=white&label=Build%20and%20Deploy)

This repository contains a comprehensive collection of internal names for Autodesk Inventor. These internal names are essential for accessing various Inventor API components programmatically. Each category is organized into its own namespace (e.g., Ribbon, PropertySets, iProperties, Commands, etc.) for easy navigation and usage.

## 📋 Table of Contents

- [Installation](#-installation)
- [Quick Start](#-quick-start)
- [Ribbon Components](#-ribbon-components)
  - [Available Ribbons](#available-ribbons)
  - [Ribbon Tabs](#ribbon-tabs)
  - [Ribbon Panels](#ribbon-panels)
  - [Ribbon Examples](#ribbon-examples)
- [Property Sets](#-property-sets)
  - [Available Property Sets](#available-property-sets)
  - [Property Set Examples](#property-set-examples)
- [iProperties](#-iproperties)
  - [Available iProperties](#available-iproperties)
  - [iProperty Examples](#iproperty-examples)
- [Command Names](#-command-names)
  - [Available Commands](#available-commands)
  - [Command Examples](#command-examples)
- [Asset Library Names](#-asset-library-names)
  - [Available Asset Libraries](#available-asset-libraries)
  - [Asset Library Examples](#asset-library-examples)
- [Asset Property Names](#-asset-property-names)
  - [Available Asset Property Names](#available-asset-property-names)
  - [Asset Property Examples](#asset-property-examples)
- [Environment Names](#-environment-names)
  - [Available Environments](#available-environments)
  - [Environment Examples](#environment-examples)
- [Application Add-in IDs](#-application-add-in-ids)
  - [Available Add-in IDs](#available-add-in-ids)
  - [Add-in ID Examples](#add-in-id-examples)
- [Complete Reference](#-complete-reference)

## 📦 Installation

Install the package via NuGet Package Manager:

```bash
dotnet add package Inventor.InternalNames
```

Or via Package Manager Console in Visual Studio:

```powershell
Install-Package Inventor.InternalNames
```

## 🚀 Quick Start

Add the using statement to access all internal names:

```csharp
using Inventor.InternalNames;
using Inventor.InternalNames.Ribbon;
using Inventor.InternalNames.PropertySets;
using Inventor.InternalNames.iProperties;
```

Basic usage example:

```csharp
// Access a specific ribbon
var partRibbon = inventorApplication.UserInterfaceManager.Ribbons[InventorRibbons.Part];

// Get a part property value
var partNumber = partDocument.PropertySets[PropertySets.Part.DesignTracking]
    .ItemByPropId[(int)PropertiesForDesignTrackingPropertiesEnum.kPartNumberDesignTrackingProperties].Value;

// Execute a command
inventorApplication.CommandManager.ControlDefinitions[CommandNames.AssemblyCreateComponentCmd].Execute();
```

## 🎀 Ribbon Components

User Interface Components. The internal names are used to identify the user interface components in the Inventor API. The internal names are used in the `Ribbon`, `RibbonTab`, and `RibbonPanel` classes to identify the user interface components.

### Available Ribbons

The [`InventorRibbons`](src/Inventor.InternalNames/Ribbon/InventorRibbons.cs) struct provides access to all main ribbon types for different Inventor environments:

- **Part** - Part modeling environment
- **Assembly** - Assembly environment  
- **Drawing** - Drawing environment
- **ZeroDoc** - Zero document state
- **Presentation** - Presentation environment
- **iFeatures** - iFeature environment
- **UnknownDocument** - Unknown document type

📁 **Source:** [src/Inventor.InternalNames/Ribbon/InventorRibbons.cs](src/Inventor.InternalNames/Ribbon/InventorRibbons.cs)

### Ribbon Tabs

Each document type has its own set of ribbon tabs:

#### Part Document Tabs
📁 **Source:** [`PartRibbonTabs`](src/Inventor.InternalNames/Ribbon/PartRibbonTabs.cs) - Contains 46+ ribbon tabs including:
- `SheetMetal`, `FlatPattern`, `k3DModel`, `Sketch`, `Annotate`, `Inspect`, `Tools`, `Manage`, `View`, `Environments`, and many more specialized tabs.

#### Assembly Document Tabs  
📁 **Source:** [`AssemblyRibbonTabs`](src/Inventor.InternalNames/Ribbon/AssemblyRibbonTabs.cs) - Contains tabs for:
- `Assemble`, `Design`, `k3DModel`, `Weld`, `Electromechanical`, `TubeAndPipe`, `CableAndHarness`, and more.

#### Drawing Document Tabs
📁 **Source:** [`DrawingRibbonTabs`](src/Inventor.InternalNames/Ribbon/DrawingRibbonTabs.cs) - Contains tabs for:
- `PlaceViews`, `Annotate`, and more drawing-specific functionality.
- `Sketch` - Sketching in drawings
- `Tools` - Drawing tools
- And more...

### Ribbon Panels

Each tab contains multiple panels organized as nested structures within each document type:

📁 **Sources:**
- [`PartRibbonPanels`](src/Inventor.InternalNames/Ribbon/PartRibbonPanels.cs) - All Part document ribbon panels
- [`AssemblyRibbonPanels`](src/Inventor.InternalNames/Ribbon/AssemblyRibbonPanels.cs) - All Assembly document ribbon panels  
- [`DrawingRibbonPanels`](src/Inventor.InternalNames/Ribbon/DrawingRibbonPanels.cs) - All Drawing document ribbon panels

### Ribbon Examples

#### Example 1: Accessing Drawing Ribbon Components
```csharp
// Get the drawing ribbon
var drawingRibbon = inventorApplication.UserInterfaceManager.Ribbons[InventorRibbons.Drawing];

// Access the Place Views tab
var placeViewsTab = drawingRibbon.RibbonTabs[DrawingRibbonTabs.PlaceViews];

// Access the Create panel within Place Views tab
var createPanel = placeViewsTab.RibbonPanels[DrawingRibbonPanels.PlaceViews.Create];
```

#### Example 2: Working with Part Environment
```csharp
// Access Part ribbon
var partRibbon = inventorApplication.UserInterfaceManager.Ribbons[InventorRibbons.Part];

// Get 3D Model tab
var modelTab = partRibbon.RibbonTabs[PartRibbonTabs.k3DModel];

// Access the Create panel for 3D features
var createPanel = modelTab.RibbonPanels[PartRibbonPanels.k3DModelTab.Create];
```

#### Example 3: Assembly Environment Navigation
```csharp
// Access Assembly ribbon
var assemblyRibbon = inventorApplication.UserInterfaceManager.Ribbons[InventorRibbons.Assembly];

// Get Assemble tab
var assembleTab = assemblyRibbon.RibbonTabs[AssemblyRibbonTabs.Assemble];

// Access Component panel
var componentPanel = assembleTab.RibbonPanels[AssemblyRibbonPanels.AssembleTab.Component];
```

## 📄 Property Sets

Property sets contain groups of related properties in Inventor documents. Each document type (Part, Assembly, Drawing) has access to different property sets.

### Available Property Sets

Each document type has access to different property sets for managing document metadata and properties:

📁 **Sources:**
- [`PropertySets.Part`](src/Inventor.InternalNames/PropertySets/Part.cs) - Part document property sets
- [`PropertySets.Assembly`](src/Inventor.InternalNames/PropertySets/Assembly.cs) - Assembly document property sets
- [`PropertySets.Drawing`](src/Inventor.InternalNames/PropertySets/Drawing.cs) - Drawing document property sets

**Common Property Sets:**
- `SummaryInformation` - Basic document information
- `DocumentSummaryInformation` - Extended document details
- `DesignTracking` - Design tracking and workflow properties
- `UserDefined` - Custom user-defined properties

### Property Set Examples

#### Example 1: Accessing Design Tracking Properties
```csharp
// Get part document
PartDocument partDoc = (PartDocument)inventorApplication.ActiveDocument;

// Access Design Tracking property set
PropertySet designTrackingProps = partDoc.PropertySets[PropertySets.Part.DesignTracking];

// Read specific properties
string partNumber = designTrackingProps["Part Number"].Value;
string description = designTrackingProps["Description"].Value;
```

#### Example 2: Working with Summary Information
```csharp
// Access Summary Information property set
PropertySet summaryInfo = partDoc.PropertySets[PropertySets.Part.SummaryInformation];

// Get document properties
string title = summaryInfo["Title"].Value;
string author = summaryInfo["Author"].Value;
string company = summaryInfo["Company"].Value;
```

#### Example 3: User Defined Properties
```csharp
// Access User Defined property set
PropertySet userDefinedProps = partDoc.PropertySets[PropertySets.Part.UserDefined];

// Add custom property
userDefinedProps.Add("MyCustomProperty", "Custom Value");

// Read custom property
string customValue = userDefinedProps["MyCustomProperty"].Value;
```

## 🏷️ iProperties

iProperties are individual property names within property sets. Each document type has an extensive collection of available property names.

### Available iProperties

iProperties are individual property names within property sets. Each document type has an extensive collection of available property names:

📁 **Sources:**
- [`iProperties.Part`](src/Inventor.InternalNames/iProperties/Part.cs) - 100+ Part document property constants
- [`iProperties.Assembly`](src/Inventor.InternalNames/iProperties/Assembly.cs) - Assembly document property constants  
- [`iProperties.Drawing`](src/Inventor.InternalNames/iProperties/Drawing.cs) - Drawing document property constants

**Property Categories Include:**
- **Basic Properties:** Title, Subject, Author, Keywords, Comments, Part Number, Description, Material
- **Design Tracking:** Project, Cost Center, Designer, Engineer, Checked By, Approved By
- **Physical Properties:** Mass, Surface Area, Volume, Density  
- **Sheet Metal Properties:** Flat Pattern dimensions, Sheet Metal Rule
- **Drawing-Specific:** Pipe Type, Route Preview, Fitting Material, Diameter, Schedule

### iProperty Examples

#### Example 1: Reading Standard iProperties
```csharp
// Access Design Tracking property set
PropertySet designProps = partDoc.PropertySets[PropertySets.Part.DesignTracking];

// Read properties using iProperty constants
string partNumber = designProps[iProperties.Part.PartNumber].Value;
string description = designProps[iProperties.Part.Description].Value;
string designer = designProps[iProperties.Part.Designer].Value;
string material = designProps[iProperties.Part.Material].Value;
```

#### Example 2: Setting iProperties
```csharp
// Set various part properties
designProps[iProperties.Part.PartNumber].Value = "ABC-123";
designProps[iProperties.Part.Description].Value = "Main Housing Component";
designProps[iProperties.Part.Designer].Value = "John Smith";
designProps[iProperties.Part.Engineer].Value = "Jane Doe";
```

#### Example 3: Working with Physical Properties
```csharp
// Access Summary Information for physical properties
PropertySet summaryInfo = partDoc.PropertySets[PropertySets.Part.SummaryInformation];

// Read physical properties
double mass = summaryInfo[iProperties.Part.Mass].Value;
double volume = summaryInfo[iProperties.Part.Volume].Value;
double density = summaryInfo[iProperties.Part.Density].Value;
```

#### Example 4: Sheet Metal Specific Properties
```csharp
// For sheet metal parts
if (partDoc.SubType == PartDocumentSubTypeEnum.kSheetMetalDocumentSubType)
{
    double flatWidth = summaryInfo[iProperties.Part.FlatPatternWidth].Value;
    double flatLength = summaryInfo[iProperties.Part.FlatPatternLength].Value;
    double flatArea = summaryInfo[iProperties.Part.FlatPatternArea].Value;
}
```

## ⚡ Command Names

Command names are internal identifiers for Inventor commands that can be executed programmatically using the CommandManager. The `CommandNames` struct contains hundreds of command constants.

### Available Commands

📁 **Source:** [`CommandNames`](src/Inventor.InternalNames/Commands/CommandNames.cs) - Contains hundreds of Inventor command constants

**Command Categories Include:**
- **General Commands:** Update, Continue, Save, Export operations
- **Assembly Commands:** Create/Place/Move components, Constraints, Patterns, Mirroring  
- **Part Modeling Commands:** Extrude, Revolve, Sweep, Loft, Hole, Fillet, Chamfer
- **Drawing Commands:** Base View, Projected View, Section View, Detail View, Dimensions
- **Sketch Commands:** Line, Circle, Rectangle, Arc, Spline and geometric operations

### Command Examples

#### Example 1: Executing Basic Commands
```csharp
// Get the command manager
CommandManager cmdMgr = inventorApplication.CommandManager;

// Execute update command
cmdMgr.ControlDefinitions[CommandNames.Update].Execute();

// Start a new sketch
cmdMgr.ControlDefinitions[CommandNames.Part2DSketchCmd].Execute();
```

#### Example 2: Assembly Operations
```csharp
// Place a new component
cmdMgr.ControlDefinitions[CommandNames.AssemblyPlaceComponentCmd].Execute();

// Create assembly constraints
cmdMgr.ControlDefinitions[CommandNames.AssemblyConstraintCmd].Execute();

// Pattern components
cmdMgr.ControlDefinitions[CommandNames.AssemblyPatternComponentCmd].Execute();
```

#### Example 3: Part Modeling Operations
```csharp
// Create extrude feature
cmdMgr.ControlDefinitions[CommandNames.PartExtrudeCmd].Execute();

// Add fillet
cmdMgr.ControlDefinitions[CommandNames.PartFilletCmd].Execute();

// Create hole feature
cmdMgr.ControlDefinitions[CommandNames.PartHoleCmd].Execute();
```

#### Example 4: Drawing Commands
```csharp
// Create base view
cmdMgr.ControlDefinitions[CommandNames.DrawingBaseViewCmd].Execute();

// Add dimensions
cmdMgr.ControlDefinitions[CommandNames.DrawingGeneralDimensionCmd].Execute();

// Create section view
cmdMgr.ControlDefinitions[CommandNames.DrawingSectionViewCmd].Execute();
```

#### Example 5: Checking Command Availability
```csharp
// Check if a command is available before executing
ControlDefinition controlDef = cmdMgr.ControlDefinitions[CommandNames.AssemblyCreateComponentCmd];

if (controlDef.Enabled)
{
    controlDef.Execute();
}
```

## 🎨 Asset Library Names

Asset Library Names provide internal identifiers for Autodesk's built-in asset libraries in Inventor. These identifiers are used to programmatically access material libraries, appearance libraries, and other asset collections.

### Available Asset Libraries

📁 **Source:** [`AssetLibraryNames`](src/Inventor.InternalNames/AssetLibraryNames.cs) - Contains asset library GUID constants

**Available Libraries:**
- **Autodesk Material Library** - Standard Autodesk material library
- **Autodesk Appearance Library** - Standard Autodesk appearance library  
- **Favorites** - User's favorites collection
- **Inventor Material Library** - Inventor-specific material library

### Asset Library Examples

#### Example 1: Accessing Material Libraries
```csharp
// Get the asset library manager
AssetLibraries assetLibraries = inventorApplication.AssetLibraries;

// Access the Autodesk Material Library
AssetLibrary materialLibrary = assetLibraries[AssetLibraryNames.AutodeskMaterialLibrary];

// Access the Inventor Material Library
AssetLibrary inventorMaterialLibrary = assetLibraries[AssetLibraryNames.InventorMaterialLibrary];
```

#### Example 2: Working with Appearance Libraries
```csharp
// Access the Autodesk Appearance Library
AssetLibrary appearanceLibrary = assetLibraries[AssetLibraryNames.AutodeskAppearanceLibrary];

// Browse available appearances
foreach (Asset appearance in appearanceLibrary.AppearanceAssets)
{
    Console.WriteLine($"Appearance: {appearance.DisplayName}");
}
```

## 🎭 Asset Property Names

Asset Property Names provide internal identifiers for properties within Autodesk's asset system in Inventor. These property names are used to programmatically access and modify material and appearance properties, such as colors, textures, reflectivity, transparency, and other rendering attributes.

### Available Asset Property Names

📁 **Source:** [`AssetPropertyNames`](src/Inventor.InternalNames/AssetPropertyNames.cs) - Contains asset property name constants

**Available Property Categories:**
- **Asset Identification:** AssetLibID, ExchangeGUID, common_Shared_Asset
- **Color & Tint Properties:** color_by_object, common_Tint_color, common_Tint_toggle, common_Tint_color_colorspace
- **Diffuse Properties:** generic_diffuse, generic_diffuse_colorspace, generic_diffuse_image_fade
- **Reflection Properties:** generic_reflectivity_at_0deg, generic_reflectivity_at_90deg, generic_reflection_glossy_samples, generic_refl_depth
- **Refraction Properties:** generic_refraction_index, generic_refraction_translucency_weight, generic_refraction_glossy_samples, generic_refr_depth
- **Surface Properties:** generic_glossiness, generic_is_metal, generic_bump_map, generic_bump_amount
- **Transparency & Opacity:** generic_transparency, generic_transparency_image_fade, generic_cutout_opacity
- **Self-Illumination:** generic_self_illum_luminance, generic_self_illum_color_temperature, generic_self_illum_filter_map
- **Ambient Occlusion:** generic_ao_on, generic_ao_samples, generic_ao_distance, generic_ao_details
- **Advanced Properties:** generic_roundcorners_radius, generic_backface_cull, mode

### Asset Property Examples

#### Example 1: Accessing Material Properties
```csharp
// Get a material asset from the library
AssetLibrary materialLibrary = inventorApplication.AssetLibraries[AssetLibraryNames.AutodeskMaterialLibrary];
Asset materialAsset = materialLibrary.MaterialAssets["Steel"];

// Read asset properties
if (materialAsset.HasProperty(AssetPropertyNames.generic_diffuse))
{
    AssetValue diffuseColor = materialAsset[AssetPropertyNames.generic_diffuse];
    Console.WriteLine($"Diffuse Color: {diffuseColor}");
}

// Check metallic property
if (materialAsset.HasProperty(AssetPropertyNames.generic_is_metal))
{
    bool isMetal = (bool)materialAsset[AssetPropertyNames.generic_is_metal].Value;
    Console.WriteLine($"Is Metal: {isMetal}");
}
```

#### Example 2: Modifying Appearance Properties
```csharp
// Get an appearance asset
AssetLibrary appearanceLibrary = inventorApplication.AssetLibraries[AssetLibraryNames.AutodeskAppearanceLibrary];
Asset appearanceAsset = appearanceLibrary.AppearanceAssets["Polished"];

// Modify glossiness
if (appearanceAsset.HasProperty(AssetPropertyNames.generic_glossiness))
{
    appearanceAsset[AssetPropertyNames.generic_glossiness].Value = 0.9;
}

// Set transparency
if (appearanceAsset.HasProperty(AssetPropertyNames.generic_transparency))
{
    appearanceAsset[AssetPropertyNames.generic_transparency].Value = 0.5;
}
```

#### Example 3: Working with Reflectivity
```csharp
// Configure reflection properties for a material
Asset asset = materialLibrary.MaterialAssets["CustomMaterial"];

// Set reflectivity at different angles
if (asset.HasProperty(AssetPropertyNames.generic_reflectivity_at_0deg))
{
    asset[AssetPropertyNames.generic_reflectivity_at_0deg].Value = 0.3;
}

if (asset.HasProperty(AssetPropertyNames.generic_reflectivity_at_90deg))
{
    asset[AssetPropertyNames.generic_reflectivity_at_90deg].Value = 1.0;
}

// Configure reflection quality
if (asset.HasProperty(AssetPropertyNames.generic_reflection_glossy_samples))
{
    asset[AssetPropertyNames.generic_reflection_glossy_samples].Value = 16;
}
```

#### Example 4: Configuring Self-Illumination
```csharp
// Create a self-illuminating material
Asset glowingAsset = appearanceLibrary.AppearanceAssets["Glowing"];

// Set luminance
if (glowingAsset.HasProperty(AssetPropertyNames.generic_self_illum_luminance))
{
    glowingAsset[AssetPropertyNames.generic_self_illum_luminance].Value = 1500.0; // cd/m²
}

// Set color temperature
if (glowingAsset.HasProperty(AssetPropertyNames.generic_self_illum_color_temperature))
{
    glowingAsset[AssetPropertyNames.generic_self_illum_color_temperature].Value = 5000.0; // Kelvin
}
```

## 🌍 Environment Names

Environment Names provide internal identifiers for Inventor's various working environments. These identifiers are used to programmatically detect, switch between, or interact with different Inventor environments such as Part modeling, Assembly, Drawing, Sheet Metal, Weld, Cable & Harness, and many specialized environments.

### Available Environments

📁 **Source:** [`Environments`](src/Inventor.InternalNames/Environments.cs) - Contains environment identifier constants

**Available Environments Include:**
- **Part Environments:** PMxPartEnvironment, PMxPartSketchEnvironment, PMxPartSketch3dEnvironment, PMxPartSolidEditEnvironment
- **Sheet Metal:** MBxSheetMetalEnvironment, PMxPartFlatPatternEditEnvironment
- **Assembly Environments:** AMxAssemblyEnvironment, AMxAssemblySketchEnvironment, AMxWeldmentEnvironment
- **Drawing Environments:** DLxDrawingEnvironment, DLxDrawingSketchEnvironment, DLxDrawingModelSpaceEnvironment, DLxDrawingCustomViewEnvironment
- **Specialized Environments:**
  - Cable & Harness: HSLCableAssemblyEnvironment, HSLCablePartEnvironment, HSLCableNailboardDrawingEnvironment
  - Piping & Tubing: PipingRouteEnvironment, PipingRunEnvironment, PipingRunsEnvironment, PipingHoseEnvironment
  - Frame Analysis: FrameAnalysisEnvironment, FWxMainFrameEnvironment
  - Simulation: DynamicSimulationEnvironmentInternalName, FEAEnvironmentInternalName
- **Other Environments:** DXxPresentationEnvironment, PMxiFeatureEnvironment, MoldEnvironment, PMx3DPrintEnvironment

### Environment Examples

#### Example 1: Detecting Current Environment
```csharp
// Get the active environment
if (inventorApplication.ActiveDocument != null)
{
    Environment activeEnv = inventorApplication.ActiveDocument.ActiveEnvironment;
    string envName = activeEnv.InternalName;
    
    // Check which environment is active
    if (envName == Environments.PMxPartEnvironment)
    {
        Console.WriteLine("Currently in Part modeling environment");
    }
    else if (envName == Environments.PMxPartSketchEnvironment)
    {
        Console.WriteLine("Currently in Part sketch environment");
    }
    else if (envName == Environments.MBxSheetMetalEnvironment)
    {
        Console.WriteLine("Currently in Sheet Metal environment");
    }
}
```

#### Example 2: Activating a Specific Environment
```csharp
// Switch to a specific environment in a part document
PartDocument partDoc = (PartDocument)inventorApplication.ActiveDocument;

// Access available environments
Environments environments = partDoc.EnvironmentManager.Environments;

// Activate the sketch environment
Environment sketchEnv = environments[Environments.PMxPartSketchEnvironment];
if (sketchEnv != null)
{
    sketchEnv.Activate();
    Console.WriteLine("Sketch environment activated");
}
```

#### Example 3: Environment-Specific Operations
```csharp
// Perform different operations based on current environment
string currentEnv = inventorApplication.ActiveDocument.ActiveEnvironment.InternalName;

switch (currentEnv)
{
    case Environments.PMxPartEnvironment:
        // Part modeling operations
        Console.WriteLine("Perform part modeling operations");
        break;
        
    case Environments.MBxSheetMetalEnvironment:
        // Sheet metal operations
        Console.WriteLine("Perform sheet metal operations");
        break;
        
    case Environments.AMxAssemblyEnvironment:
        // Assembly operations
        Console.WriteLine("Perform assembly operations");
        break;
        
    case Environments.DLxDrawingEnvironment:
        // Drawing operations
        Console.WriteLine("Perform drawing operations");
        break;
        
    default:
        Console.WriteLine($"Unknown environment: {currentEnv}");
        break;
}
```

#### Example 4: Checking Environment Availability
```csharp
// Check if a specific environment is available in the document
PartDocument partDoc = (PartDocument)inventorApplication.ActiveDocument;

// List all available environments
foreach (Environment env in partDoc.EnvironmentManager.Environments)
{
    Console.WriteLine($"Available Environment: {env.InternalName}");
    
    // Check for specific environments
    if (env.InternalName == Environments.MBxSheetMetalEnvironment)
    {
        Console.WriteLine("  -> Sheet Metal environment is available");
    }
}
```

## 🔌 Application Add-in IDs

Application Add-in IDs provide internal identifiers for Autodesk Inventor add-ins and translators. These GUID constants allow developers to programmatically identify and interact with specific add-ins installed in Inventor.

### Available Add-in IDs

📁 **Source:** [`ApplicationAddinIds`](src/Inventor.InternalNames/ApplicationAddinIds.cs) - Contains 70+ application add-in GUID constants

**Add-in Categories Include:**
- **Core Inventor Add-ins:** Content Center, Design Accelerator, Frame Generator, iLogic, iCopy
- **Simulation Add-ins:** Stress Analysis, Frame Analysis, Dynamic Simulation  
- **Manufacturing Add-ins:** Additive Manufacturing, Mold Design, Presentations
- **Translators:** STEP, IGES, CATIA V5, SolidWorks, Fusion 360, DWG/DXF, STL, PDF, and many more
- **Specialized Tools:** BIM Content, Assembly Bonus Tools, Shared Views, Interactive Tutorial

### Add-in ID Examples

#### Example 1: Checking If Add-ins Are Available
```csharp
// Get the application add-ins collection
ApplicationAddIns addIns = inventorApplication.ApplicationAddIns;

// Check if iLogic add-in is available
if (addIns.ItemById[ApplicationAddinIds.iLogic] != null)
{
    Console.WriteLine("iLogic add-in is available");
    
    // Get the iLogic add-in
    ApplicationAddIn iLogicAddin = addIns.ItemById[ApplicationAddinIds.iLogic];
}
```

#### Example 2: Working with Translator Add-ins
```csharp
// Check available translators
var translatorIds = new string[]
{
    ApplicationAddinIds.TranslatorSTEP,
    ApplicationAddinIds.TranslatorIGES,
    ApplicationAddinIds.TranslatorCATIAV5Import,
    ApplicationAddinIds.TranslatorSolidWorks,
    ApplicationAddinIds.TranslatorFusion
};

foreach (string translatorId in translatorIds)
{
    var translator = addIns.ItemById[translatorId];
    if (translator != null)
    {
        Console.WriteLine($"Translator Available: {translator.DisplayName}");
    }
}
```

#### Example 3: Loading Add-ins Programmatically
```csharp
// Load the Content Center add-in if available
ApplicationAddIn contentCenter = addIns.ItemById[ApplicationAddinIds.ContentCenter];
if (contentCenter != null)
{
    try
    {
        contentCenter.Activate();
        Console.WriteLine("Content Center add-in loaded successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to load Content Center: {ex.Message}");
    }
}
```

#### Example 4: Enumerating All Available Add-ins
```csharp
// Get all add-ins and their information
foreach (ApplicationAddIn addIn in addIns)
{
    Console.WriteLine($"Add-in: {addIn.DisplayName}");
    Console.WriteLine($"  ID: {addIn.ClassIdString}");
    Console.WriteLine($"  Description: {addIn.Description}");
    Console.WriteLine();
}
```

## 📚 Complete Reference

### Namespace Organization

The library is organized into the following namespaces:

```csharp
Inventor.InternalNames                  // Base namespace: CommandNames, AssetLibraryNames,
                                        // AssetPropertyNames, Environments, ApplicationAddinIds
├── Ribbon                             // Ribbon-related constants
│   ├── InventorRibbons               // Main ribbon types
│   ├── PartRibbonTabs                // Part document ribbon tabs
│   ├── PartRibbonPanels              // Part document ribbon panels
│   ├── AssemblyRibbonTabs            // Assembly document ribbon tabs
│   ├── AssemblyRibbonPanels          // Assembly document ribbon panels
│   ├── DrawingRibbonTabs             // Drawing document ribbon tabs
│   ├── DrawingRibbonPanels           // Drawing document ribbon panels
│   ├── PresentationRibbonTabs        // Presentation ribbon tabs
│   ├── PresentationRibbonPanels      // Presentation ribbon panels
│   ├── ZeroDocRibbonTabs             // Zero document ribbon tabs
│   ├── ZeroDocRibbonPanels           // Zero document ribbon panels
│   ├── iFeatureRibbonTabs            // iFeature ribbon tabs
│   ├── iFeatureRibbonPanels          // iFeature ribbon panels
│   ├── UnknownDocumentRibbonTabs     // Unknown document ribbon tabs
│   └── UnknownDocumentRibbonPanels   // Unknown document ribbon panels
├── PropertySets                       // Property set names
│   ├── Part                          // Part document property sets
│   ├── Assembly                      // Assembly document property sets
│   └── Drawing                       // Drawing document property sets
└── iProperties                        // Individual property names
    ├── Part                          // Part document property names
    ├── Assembly                      // Assembly document property names
    └── Drawing                       // Drawing document property names
```

### Document Type Support

| Document Type | Ribbons | Tabs | Panels | Property Sets | iProperties | Commands | Asset Libraries | Asset Properties | Environments | Add-in IDs |
|---------------|---------|------|--------|---------------|-------------|----------|-----------------|------------------|--------------|------------|
| Part          | ✅      | ✅   | ✅     | ✅            | ✅          | ✅       | ✅              | ✅               | ✅           | ✅         |
| Assembly      | ✅      | ✅   | ✅     | ✅            | ✅          | ✅       | ✅              | ✅               | ✅           | ✅         |
| Drawing       | ✅      | ✅   | ✅     | ✅            | ✅          | ✅       | ✅              | ✅               | ✅           | ✅         |
| Presentation  | ✅      | ✅   | ✅     | ❌            | ❌          | ✅       | ✅              | ✅               | ✅           | ✅         |
| iFeature      | ✅      | ✅   | ✅     | ❌            | ❌          | ✅       | ✅              | ✅               | ✅           | ✅         |
| ZeroDoc       | ✅      | ✅   | ✅     | ❌            | ❌          | ✅       | ✅              | ✅               | ❌           | ✅         |
| Unknown       | ✅      | ✅   | ✅     | ❌            | ❌          | ✅       | ✅              | ✅               | ❌           | ✅         |

### Usage Best Practices

1. **Always check availability**: Verify that UI elements exist before accessing them
2. **Use constants**: Leverage the provided constants instead of hardcoding strings
3. **Handle exceptions**: Wrap API calls in try-catch blocks for robustness
4. **Document context**: Consider which document type is active when accessing UI elements

### Example: Complete Integration
```csharp
using Inventor;
using Inventor.InternalNames;
using Inventor.InternalNames.Ribbon;
using Inventor.InternalNames.PropertySets;
using Inventor.InternalNames.iProperties;

public class InventorIntegrationExample
{
    private Application _inventorApp;
    
    public void CompleteExample()
    {
        // 1. Access UI components
        var partRibbon = _inventorApp.UserInterfaceManager.Ribbons[InventorRibbons.Part];
        var modelTab = partRibbon.RibbonTabs[PartRibbonTabs.k3DModel];
        
        // 2. Work with properties
        if (_inventorApp.ActiveDocument is PartDocument partDoc)
        {
            var designProps = partDoc.PropertySets[PropertySets.Part.DesignTracking];
            string partNumber = designProps[iProperties.Part.PartNumber].Value;
            
            // 3. Execute commands
            var cmdMgr = _inventorApp.CommandManager;
            cmdMgr.ControlDefinitions[CommandNames.PartExtrudeCmd].Execute();
            
            // 4. Work with asset libraries
            var assetLibraries = _inventorApp.AssetLibraries;
            var materialLibrary = assetLibraries[AssetLibraryNames.AutodeskMaterialLibrary];
            
            // 5. Work with asset properties
            Asset materialAsset = materialLibrary.MaterialAssets["Steel"];
            if (materialAsset.HasProperty(AssetPropertyNames.generic_diffuse))
            {
                AssetValue diffuseColor = materialAsset[AssetPropertyNames.generic_diffuse];
            }
            
            // 6. Check current environment
            string currentEnv = partDoc.ActiveEnvironment.InternalName;
            if (currentEnv == Environments.PMxPartEnvironment)
            {
                Console.WriteLine("In Part modeling environment");
            }
            
            // 7. Check and manage add-ins
            var addIns = _inventorApp.ApplicationAddIns;
            var iLogicAddin = addIns.ItemById[ApplicationAddinIds.iLogic];
            if (iLogicAddin != null)
            {
                iLogicAddin.Activate();
            }
        }
    }
}
```

---

## 🤝 Contributing

Contributions are welcome! If you find missing internal names or have suggestions for improvements, please:

1. Open an issue describing the missing functionality
2. Submit a pull request with the additions
3. Include examples in your contributions

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 Related Resources

- [Autodesk Inventor API Documentation](https://help.autodesk.com/view/INVNTOR/2026/ENU/?guid=UserManualIndex)
- [NuGet Package](https://www.nuget.org/packages/Inventor.InternalNames)
