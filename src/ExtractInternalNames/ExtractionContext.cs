using ExtractInternalNames.Core;
using Inventor;
using Application = Inventor.Application;

namespace ExtractInternalNames;

internal sealed class ExtractionContext : IDisposable
{
    public Application Application { get; }
    public NameCatalog Catalog { get; }
    private readonly IDictionary<string, string> templates;
    private readonly Dictionary<DocumentTypeEnum, Document> documents = new();
    private readonly Dictionary<string, NameGroup> groups = new(StringComparer.Ordinal);
    private readonly Document originalDocument;
    private readonly bool originalSilentOperation;

    public ExtractionContext(Application application, NameCatalog catalog, IDictionary<string, string> templates)
    {
        Application = application;
        Catalog = catalog;
        this.templates = templates;
        originalDocument = application.ActiveDocument;
        originalSilentOperation = application.SilentOperation;
        application.SilentOperation = true;
    }

    public Document Document(DocumentTypeEnum type)
    {
        if (!documents.TryGetValue(type, out var document))
        {
            var kind = type switch
            {
                DocumentTypeEnum.kPartDocumentObject => "part", DocumentTypeEnum.kAssemblyDocumentObject => "assembly",
                DocumentTypeEnum.kDrawingDocumentObject => "drawing", DocumentTypeEnum.kPresentationDocumentObject => "presentation",
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
            var template = templates.TryGetValue(kind, out var supplied) ? supplied : Application.FileManager.GetTemplateFile(type);
            document = Application.Documents.Add(type, template, true);
            documents.Add(type, document);
        }
        document.Activate();
        return document;
    }

    public void PrepareDocuments(bool includePresentation = false)
    {
        Document(DocumentTypeEnum.kPartDocumentObject);
        Document(DocumentTypeEnum.kAssemblyDocumentObject);
        Document(DocumentTypeEnum.kDrawingDocumentObject);
        if (includePresentation)
        {
            Document(DocumentTypeEnum.kPresentationDocumentObject);
        }
    }

    public NameGroup Group(string category, string file, string classPath, string key = null,
        string namespaceName = "Inventor.InternalNames")
    {
        key ??= classPath;
        var identity = category + "|" + key;
        if (groups.TryGetValue(identity, out var existing))
        {
            return existing;
        }

        var known = Catalog.Groups.SingleOrDefault(g => g.Category == category && g.Key == key);
        if (known != null)
        {
            file = known.File;
            classPath = known.Class;
            namespaceName = known.Namespace;
        }
        else if (Catalog.Groups.Concat(groups.Values).Any(g => g.Namespace == namespaceName && g.Class == classPath))
        {
            classPath += "_" + CatalogMerger.StableSuffix(key);
        }

        var group = new NameGroup { Category = category, File = file, Namespace = namespaceName, Class = classPath, Key = key };
        groups.Add(identity, group);
        return group;
    }

    public static void Add(NameGroup group, string key, string suggestedName, string value)
    {
        var existing = group.Names.SingleOrDefault(n => n.Key == key);
        if (existing != null)
        {
            if (existing.Value != value)
            {
                throw new InvalidDataException($"Conflicting observations for {group.Class}: {key}");
            }

            return;
        }
        group.Names.Add(new NameEntry { Key = key, Name = suggestedName, Value = value });
    }

    public void Dispose()
    {
        var errors = new List<Exception>();
        foreach (var document in documents.Values.Reverse())
            try { document.Close(true); } catch (Exception error) { errors.Add(error); }
        try { originalDocument?.Activate(); } catch (Exception error) { errors.Add(error); }
        try { Application.SilentOperation = originalSilentOperation; } catch (Exception error) { errors.Add(error); }
        if (errors.Count != 0)
        {
            throw new AggregateException("Could not restore the Inventor session after extraction.", errors);
        }
    }
}
