using System.Text.RegularExpressions;
using PXReprise.SchemaGen;

namespace PXReprise.Data.Tests;

/// <summary>
/// The generated reference pages say what the schema and the program say: <c>docs/data/schema/*.md</c> is the schema
/// rendered by <see cref="SchemaGenerator.RenderReference"/>, and <c>docs/data/cli.md</c> is the <c>datarepo</c> program's
/// own <c>-h</c> output, rendered by <see cref="CliDocs.Render"/>. Each fails when its page is stale, as
/// <c>BundleTests.TheGeneratedTablesAreCurrent</c> does for <c>Tables.g.cs</c> (external review R6).
/// </summary>
public class GeneratedPagesTests
{
    /// <summary>The datarepo CLI this test project was built with (a project reference copies it beside the tests).</summary>
    private static string DataRepoDll => Path.Combine(AppContext.BaseDirectory, "datarepo.dll");

    private static string Read(string path) => File.ReadAllText(path).Replace("\r\n", "\n");

    [Test]
    public void TheSchemaReferencePagesAreCurrent()
    {
        var pages = SchemaGenerator.ReferencePages(DataPaths.Root).ToList();
        Assert.That(pages.Select(p => Path.GetFileName(p.Target)), Is.EquivalentTo(new[] { "core.md", "study-aging.md" }));
        Assert.Multiple(() =>
        {
            foreach (var (target, text) in pages)
                Assert.That(File.Exists(target) ? Read(target) : "", Is.EqualTo(text),
                    $"{Path.GetFileName(target)} is stale: run dotnet run --project src/PXReprise.SchemaGen");
        });
    }

    [Test]
    public void TheCommandReferenceIsCurrent()
    {
        var rendered = CliDocs.Render(CliDocs.FromProgram(DataRepoDll));
        Assert.That(Read(Path.Combine(DataPaths.Docs, "cli.md")), Is.EqualTo(rendered),
            "docs/data/cli.md is stale: run dotnet run --project src/PXReprise.SchemaGen -- --cli");
    }

    [Test]
    public void ACommandTheGroupsDoNotPlaceIsRefused()
    {
        var real = CliDocs.FromProgram(DataRepoDll);
        string WithExtra(string? command) => command is null
            ? real(null).Replace("commands:\n", "commands:\n  frobnicate do something new\n")
            : real(command);
        var e = Assert.Throws<InvalidDataException>(() => CliDocs.Render(WithExtra));
        Assert.That(e!.Message, Does.Contain("undocumented [frobnicate]"));
    }

    [Test]
    public void AGroupedCommandTheProgramLacksIsRefused()
    {
        var real = CliDocs.FromProgram(DataRepoDll);
        string WithoutDoctor(string? command) => command is null
            ? Regex.Replace(real(null), @"^  doctor .*\n", "", RegexOptions.Multiline)
            : real(command);
        var e = Assert.Throws<InvalidDataException>(() => CliDocs.Render(WithoutDoctor));
        Assert.That(e!.Message, Does.Contain("unknown [doctor]"));
    }

    [Test]
    public void AStudyLayersPageLinksCoreRangesToTheCorePage()
    {
        var core = Path.Combine(DataPaths.Root, "schemas", "datarepo", "datarepo.yaml");
        var study = Path.Combine(DataPaths.StudySchemas, "aging.yaml");
        var page = SchemaGenerator.RenderReference(study, "schemas/datarepo/study/aging.yaml", core);
        Assert.That(page, Does.Contain("- **Imports:** the core schema ([core.md](core.md))"));
        Assert.That(page, Does.Contain("](core.md#"), "a core class or enum used by the layer links to core.md");
        Assert.That(page, Does.Not.Contain("\n## Bundle\n"), "Bundle is the core's table list, not a table");
    }
}
