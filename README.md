# Inventor.InternalNames

This repository contains a list of internal names for the Autodesk Inventor User Interface Components. The internal names are used to identify the user interface components in the Inventor API. The internal names are used in the `Ribbon`, `RibbonTab`, and `RibbonPanel` classes to identify the user interface components.

## Ribbon Internal Names

Ribbon internal names are found in `InventorRibbons`. 

## RibbonTab Internal Names

RibbonTab internal names are found in `PartRibbonTabs`, `AssemblyRibbonTabs`, and `DrawingRibbonTabs`.

## RibbonPanel Internal Names

RibbonPanel internal names are found in `PartRibbonPanels`, `AssemblyRibbonPanels`, and `DrawingRibbonPanels`. Panel names are broken down into their respective tabs.

## Example:
```csharp
var drawingRibbon = inventorApplication.UserInterfaceManager.Ribbons[InventorRibbons.Drawing];

var placeViewsTab = drawingRibbon.RibbonTabs[DrawingRibbonTabs.PlaceViews];

var createPlaceViewsPanel = placeViewsTab.RibbonPanels[DrawingRibbonPanels.PlaceViews.Create];
```