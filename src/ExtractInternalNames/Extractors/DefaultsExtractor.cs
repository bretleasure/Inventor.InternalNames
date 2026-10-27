using ExtractInternalNames.Core;
using Inventor;

namespace ExtractInternalNames.Extractors;

internal sealed class DefaultsExtractor : INameExtractor
{
    public string Id => "defaults";
    public string Description => "Origin work features and primary/default model representations; retain curated iLogic names";

    public IEnumerable<NameGroup> Extract(ExtractionContext context)
    {
        var document = (PartDocument)context.Document(DocumentTypeEnum.kPartDocumentObject);
        var component = document.ComponentDefinition;
        var planes = context.Group(Id, "WorkPlanes.cs", "WorkPlanes");
        var axes = context.Group(Id, "WorkAxes.cs", "WorkAxes");
        var points = context.Group(Id, "WorkPoints.cs", "WorkPoints");
        foreach (WorkPlane plane in component.WorkPlanes)
        {
            if (!plane.IsCoordinateSystemElement)
            {
                continue;
            }

            var normal = plane.Plane.Normal;
            var label = Math.Abs(normal.X) > 0.999999 ? "YZ" : Math.Abs(normal.Y) > 0.999999 ? "XZ" :
                Math.Abs(normal.Z) > 0.999999 ? "XY" : null;
            if (label != null)
            {
                ExtractionContext.Add(planes, label, label, plane.Name);
            }
        }
        foreach (WorkAxis axis in component.WorkAxes)
        {
            if (!axis.IsCoordinateSystemElement)
            {
                continue;
            }

            var direction = axis.Line.Direction;
            var label = Math.Abs(direction.X) > 0.999999 ? "X" : Math.Abs(direction.Y) > 0.999999 ? "Y" :
                Math.Abs(direction.Z) > 0.999999 ? "Z" : null;
            if (label != null)
            {
                ExtractionContext.Add(axes, label, label, axis.Name);
            }
        }
        foreach (WorkPoint point in component.WorkPoints)
            if (point.IsCoordinateSystemElement)
            {
                ExtractionContext.Add(points, "origin", "CenterPoint", point.Name);
            }

        if (planes.Names.Count != 3 || axes.Names.Count != 3 || points.Names.Count != 1)
        {
            throw new InvalidOperationException("The Part template did not expose the expected origin planes, axes, and point.");
        }

        var states = context.Group(Id, "ModelStates.cs", "ModelStates");
        // Model states were introduced in Inventor 2022 (API major 26).
        if (context.Application.SoftwareVersion.Major >= 26)
        {
            foreach (ModelState state in component.ModelStates)
                if (state.ModelStateType == ModelStateTypeEnum.kPrimaryModelStateType)
                {
                    ExtractionContext.Add(states, "primary", "Primary", state.Name);
                }
        }

        var views = context.Group(Id, "ViewRepresentations.cs", "ViewRepresentations");
        ReadViews(component.RepresentationsManager.DesignViewRepresentations, views);
        var assembly = (AssemblyDocument)context.Document(DocumentTypeEnum.kAssemblyDocumentObject);
        ReadViews(assembly.ComponentDefinition.RepresentationsManager.DesignViewRepresentations, views);
        // iLogic attribute keys are definitions rather than an enumerable Inventor registry.
        return new[] { planes, axes, points, states, views };
    }

    private static void ReadViews(DesignViewRepresentations representations, NameGroup group)
    {
        foreach (DesignViewRepresentation view in representations)
        {
            if (view.DesignViewType == DesignViewTypeEnum.kPrimaryDesignViewType)
            {
                ExtractionContext.Add(group, "primary", "Primary", view.Name);
            }
            else if (view.Name == "Default")
            {
                ExtractionContext.Add(group, "default", "Default", view.Name);
            }
        }
    }
}
