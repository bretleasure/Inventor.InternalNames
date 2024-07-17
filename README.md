# Inventor.InternalNames

[![NuGet version (Inventor.InternalNames)](https://buildstats.info/nuget/Inventor.InternalNames)](https://www.nuget.org/packages/Inventor.InternalNames)

This repository contains a list of internal names for Autodesk Inventor. Each category is broken down into its own namesspace (e.g. Ribbon, PropertySets, iProperties, etc.)

## Ribbon Components

 User Interface Components. The internal names are used to identify the user interface components in the Inventor API. The internal names are used in the `Ribbon`, `RibbonTab`, and `RibbonPanel` classes to identify the user interface components.

### Ribbon Internal Names

Ribbon internal names are found in `InventorRibbons`. 

### RibbonTab Internal Names

RibbonTab internal names are found in `PartRibbonTabs`, `AssemblyRibbonTabs`, and `DrawingRibbonTabs`.

### RibbonPanel Internal Names

RibbonPanel internal names are found in `PartRibbonPanels`, `AssemblyRibbonPanels`, and `DrawingRibbonPanels`. Panel names are broken down into their respective tabs.

### Example:
```csharp
var drawingRibbon = inventorApplication.UserInterfaceManager.Ribbons[InventorRibbons.Drawing];

var placeViewsTab = drawingRibbon.RibbonTabs[DrawingRibbonTabs.PlaceViews];

var createPlaceViewsPanel = placeViewsTab.RibbonPanels[DrawingRibbonPanels.PlaceViews.Create];
```

## Other Internal Names

* Property Sets
  * PropertySets.Part
  * PropertySets.Assembly
  * PropertySets.Drawing
* iProperties
  * iProperties.Part
  * iProperties.Assembly
  * iProperties.Drawing
* Command Names
  * Internal names of `ControlDefinitions` used to Execute commands with `CommandManager`