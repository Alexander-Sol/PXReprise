using System.Globalization;
using System.Text.RegularExpressions;

namespace PXReprise.Tests;

/// <summary>The docs site is built by GitHub Pages (Jekyll) only after a merge to master, so a page that breaks the build is
/// found by no pull request's CI. Jekyll reads a page's front-matter <c>date</c> as a date, and one value that is not a date
/// stops the WHOLE site from building (2026-11-14: <c>date: YYYY-MM-DD</c> in the decision-record template failed the build
/// of #5's merge). These checks catch that class of mistake before the merge.</summary>
public class SiteFrontMatterTests
{
    private static string RepoRoot()
    {
        for (var d = new DirectoryInfo(TestContext.CurrentContext.TestDirectory); d is not null; d = d.Parent)
            if (File.Exists(Path.Combine(d.FullName, "PXReprise.slnx"))) return d.FullName;
        throw new InvalidOperationException("repository root not found above the test directory");
    }

    private static readonly Regex DateLine = new(@"^date:\s*(?<value>.*?)\s*(#.*)?$", RegexOptions.Multiline);

    private static IEnumerable<string> Pages()
    {
        var docs = Path.Combine(RepoRoot(), "docs");
        foreach (var page in Directory.EnumerateFiles(docs, "*.md", SearchOption.AllDirectories).Order(StringComparer.Ordinal))
        {
            var relative = Path.GetRelativePath(docs, page).Replace('\\', '/');
            if (!relative.StartsWith('_')) yield return relative;
        }
    }

    /// <summary>The front matter between the first two <c>---</c> lines, or null when the page has none.</summary>
    private static string? FrontMatter(string text)
    {
        var lines = text.ReplaceLineEndings("\n").Split('\n');
        if (lines.Length == 0 || lines[0].Trim() != "---") return null;
        var end = Array.FindIndex(lines, 1, l => l.Trim() == "---");
        return end < 0 ? null : string.Join('\n', lines[1..end]);
    }

    [TestCaseSource(nameof(Pages))]
    public void AFrontMatterDateIsARealDate(string relative)
    {
        var front = FrontMatter(File.ReadAllText(Path.Combine(RepoRoot(), "docs", relative)));
        if (front is null) return;
        foreach (Match m in DateLine.Matches(front))
        {
            var value = m.Groups["value"].Value.Trim('"', '\'');
            Assert.That(DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _),
                $"docs/{relative}: front-matter date '{value}' is not a yyyy-MM-dd date; Jekyll cannot build the site with it");
        }
    }

    [Test]
    public void EveryDecisionRecordAgreesWithItsFileNumber()
    {
        var dir = Path.Combine(RepoRoot(), "docs", "decisions");
        var bad = new List<string>();
        foreach (var file in Directory.EnumerateFiles(dir, "0*.md").Order(StringComparer.Ordinal))
        {
            var name = Path.GetFileName(file);
            var number = int.Parse(name[..4], CultureInfo.InvariantCulture);
            var front = FrontMatter(File.ReadAllText(file)) ?? "";
            if (!Regex.IsMatch(front, @"^parent:\s*Decisions\s*$", RegexOptions.Multiline)) bad.Add($"{name}: no 'parent: Decisions'");
            if (!Regex.IsMatch(front, $@"^nav_order:\s*{number}\s*$", RegexOptions.Multiline)) bad.Add($"{name}: nav_order is not {number}");
            if (!Regex.IsMatch(front, @"^status:\s*\S", RegexOptions.Multiline)) bad.Add($"{name}: no status");
        }
        Assert.That(bad, Is.Empty, string.Join("; ", bad));
    }
}
