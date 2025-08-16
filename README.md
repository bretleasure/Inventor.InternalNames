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

The `InventorRibbons` struct provides access to all main ribbon types:

```csharp
public struct InventorRibbons
{
    public const string Part = "Part";                        // Part modeling environment
    public const string Assembly = "Assembly";                // Assembly environment  
    public const string Drawing = "Drawing";                  // Drawing environment
    public const string ZeroDoc = "ZeroDoc";                 // Zero document state
    public const string Presentation = "Presentation";        // Presentation environment
    public const string iFeatures = "iFeatures";             // iFeature environment
    public const string UnknownDocument = "UnknownDocument"; // Unknown document type
}
```

### Ribbon Tabs

Each document type has its own set of ribbon tabs:

#### Part Document Tabs (`PartRibbonTabs`)
- `SheetMetal` - Sheet metal modeling
- `FlatPattern` - Flat pattern operations  
- `k3DModel` - 3D modeling tools
- `Sketch` - 2D sketching
- `Annotate` - Annotations and dimensions
- `Inspect` - Inspection tools
- `Tools` - General tools
- `Manage` - File and project management
- `View` - View controls
- `Environments` - Environment switching
- And many more specialized tabs...

#### Assembly Document Tabs (`AssemblyRibbonTabs`)
- `Assemble` - Assembly tools
- `Design` - Design features
- `k3DModel` - 3D modeling
- `Weld` - Welding features
- `Electromechanical` - Electrical components
- `TubeAndPipe` - Tube and pipe routing
- `CableAndHarness` - Cable and harness design
- And many more...

#### Drawing Document Tabs (`DrawingRibbonTabs`)
- `PlaceViews` - View placement
- `Annotate` - Drawing annotations
- `Sketch` - Sketching in drawings
- `Tools` - Drawing tools
- And more...

### Ribbon Panels

Each tab contains multiple panels. Panels are organized as nested structures within each document type.

#### Part Ribbon Panels Example (`PartRibbonPanels`)

```csharp
// 3D Model tab panels
public struct k3DModelTab
{
    public const string Sketch = "id_PanelP_Model2DSketchCreate";
    public const string Primitives = "id_PanelP_ModelPrimitives";
    public const string Create = "id_PanelP_ModelCreate";
    public const string Modify = "id_PanelP_ModelModify";
    public const string WorkFeatures = "id_PanelA_ModelWorkFeatures";
    public const string Pattern = "id_PanelP_ModelPattern";
    // ... and more
}
```

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

#### Part Document Property Sets (`PropertySets.Part`)
```csharp
public struct Part
{
    public const string SummaryInformation = "Inventor Summary Information";
    public const string DocumentSummaryInformation = "Inventor Document Summary Information";
    public const string DesignTracking = "Design Tracking Properties";
    public const string UserDefined = "Inventor User Defined Properties";
}
```

#### Assembly Document Property Sets (`PropertySets.Assembly`)
```csharp
public struct Assembly
{
    public const string SummaryInformation = "Inventor Summary Information";
    public const string DocumentSummaryInformation = "Inventor Document Summary Information";
    public const string DesignTracking = "Design Tracking Properties";
    public const string UserDefined = "Inventor User Defined Properties";
}
```

#### Drawing Document Property Sets (`PropertySets.Drawing`)
```csharp
public struct Drawing
{
    public const string SummaryInformation = "Inventor Summary Information";
    public const string DocumentSummaryInformation = "Inventor Document Summary Information";
    public const string DesignTracking = "Design Tracking Properties";
    public const string UserDefined = "Inventor User Defined Properties";
    public const string PipingStyle = "Piping Style";
}
```

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

#### Part Document iProperties (`iProperties.Part`)

The Part struct contains over 100 property name constants, including:

**Basic Properties:**
```csharp
public const string Title = "Title";
public const string Subject = "Subject";
public const string Author = "Author";
public const string Keywords = "Keywords";
public const string Comments = "Comments";
public const string PartNumber = "Part Number";
public const string Description = "Description";
public const string Material = "Material";
```

**Design Tracking:**
```csharp
public const string Project = "Project";
public const string CostCenter = "Cost Center";
public const string CheckedBy = "Checked By";
public const string DateChecked = "Date Checked";
public const string EngrApprovedBy = "Engr Approved By";
public const string EngrDateApproved = "Engr Date Approved";
public const string Designer = "Designer";
public const string Engineer = "Engineer";
```

**Physical Properties:**
```csharp
public const string Mass = "Mass";
public const string SurfaceArea = "SurfaceArea";
public const string Volume = "Volume";
public const string Density = "Density";
```

**Sheet Metal Properties:**
```csharp
public const string FlatPatternWidth = "Flat Pattern Width";
public const string FlatPatternLength = "Flat Pattern Length";
public const string FlatPatternArea = "Flat Pattern Area";
public const string SheetMetalRule = "Sheet Metal Rule";
```

#### Assembly Document iProperties (`iProperties.Assembly`)

Assembly documents have similar properties to parts, with additional assembly-specific properties.

#### Drawing Document iProperties (`iProperties.Drawing`)

Drawing documents include all standard properties plus drawing-specific properties like piping and routing information:

```csharp
public const string PipeType = "PipeType";
public const string RoutePreview = "RoutePreview";
public const string FittingMaterial = "FittingMaterial";
public const string Diameter = "Diameter";
public const string Schedule = "Schedule";
```

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

The `CommandNames` struct provides access to all Inventor commands, including:

#### General Commands
```csharp
public const string Update = "Update";
public const string Continue = "Continue";
```

#### Assembly Commands
```csharp
public const string AssemblyCreateComponentCmd = "AssemblyCreateComponentCmd";
public const string AssemblyPlaceComponentCmd = "AssemblyPlaceComponentCmd";
public const string AssemblyConstraintCmd = "AssemblyConstraintCmd";
public const string AssemblyMoveComponentCmd = "AssemblyMoveComponentCmd";
public const string AssemblyReplaceCmd = "AssemblyReplaceCmd";
public const string AssemblyPatternComponentCmd = "AssemblyPatternComponentCmd";
public const string AssemblyMirrorComponentCmd = "AssemblyMirrorComponentCmd";
```

#### Part Modeling Commands
```csharp
public const string PartExtrudeCmd = "PartExtrudeCmd";
public const string PartRevolveCmd = "PartRevolveCmd";
public const string PartSweepCmd = "PartSweepCmd";
public const string PartLoftCmd = "PartLoftCmd";
public const string PartHoleCmd = "PartHoleCmd";
public const string PartFilletCmd = "PartFilletCmd";
public const string PartChamferCmd = "PartChamferCmd";
```

#### Drawing Commands
```csharp
public const string DrawingBaseViewCmd = "DrawingBaseViewCmd";
public const string DrawingProjectedViewCmd = "DrawingProjectedViewCmd";
public const string DrawingSectionViewCmd = "DrawingSectionViewCmd";
public const string DrawingDetailViewCmd = "DrawingDetailViewCmd";
public const string DrawingGeneralDimensionCmd = "DrawingGeneralDimensionCmd";
```

#### Sketch Commands
```csharp
public const string SketchLineCmd = "SketchLineCmd";
public const string SketchCircleCmd = "SketchCircleCmd";
public const string SketchRectangleCmd = "SketchRectangleCmd";
public const string SketchArcCmd = "SketchArcCmd";
public const string SketchSplineCmd = "SketchSplineCmd";
```

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

## 📚 Complete Reference

### Namespace Organization

The library is organized into the following namespaces:

```csharp
Inventor.InternalNames                  // Base namespace with CommandNames
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

| Document Type | Ribbons | Tabs | Panels | Property Sets | iProperties | Commands |
|---------------|---------|------|--------|---------------|-------------|----------|
| Part          | ✅      | ✅   | ✅     | ✅            | ✅          | ✅       |
| Assembly      | ✅      | ✅   | ✅     | ✅            | ✅          | ✅       |
| Drawing       | ✅      | ✅   | ✅     | ✅            | ✅          | ✅       |
| Presentation  | ✅      | ✅   | ✅     | ❌            | ❌          | ✅       |
| iFeature      | ✅      | ✅   | ✅     | ❌            | ❌          | ✅       |
| ZeroDoc       | ✅      | ✅   | ✅     | ❌            | ❌          | ✅       |
| Unknown       | ✅      | ✅   | ✅     | ❌            | ❌          | ✅       |

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

- [Autodesk Inventor API Documentation](https://help.autodesk.com/view/INVNTOR/2024/ENU/?guid=GUID-C3F3C736-1AB0-4956-B7B5-FF61A0A5E0AC)
- [Inventor Developer Center](https://www.autodesk.com/developer-network/platform-technologies/inventor)
- [NuGet Package](https://www.nuget.org/packages/Inventor.InternalNames)