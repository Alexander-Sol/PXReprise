using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PXReprise.Data.Tests;

/// <summary>
/// The released schemas under <c>schemas/datarepo/releases/</c> are frozen: each folder is the schema text a released version
/// carried, and it is never edited (external review R1). A bundle records only its <c>schema_version</c>, so this folder is
/// where its schema text is read; these tests keep it what was released.
/// </summary>
public class FrozenSchemaTests
{
    /// <summary>Every released core schema version (see the folder's README for how the list was decided).</summary>
    internal static readonly string[] Released =
    [
        "0.0.2", "0.0.3", "0.0.4", "0.0.5", "0.0.6", "0.0.7", "0.0.8", "0.0.9", "0.0.10",
        "0.0.11", "0.0.12", "0.0.13", "0.0.14", "0.0.15", "0.0.16",
    ];

    private static string Releases => Path.Combine(DataPaths.Root, "schemas", "datarepo", "releases");

    [Test]
    public void EveryReleasedVersionHasAFolderAndNoOtherFolderExists()
    {
        var folders = Directory.GetDirectories(Releases).Select(Path.GetFileName).OrderBy(n => n, StringComparer.Ordinal);
        Assert.That(folders, Is.EquivalentTo(Released));
    }

    [TestCaseSource(nameof(Released))]
    public void AFrozenFolderMatchesItsChecksums(string version)
    {
        string dir = Path.Combine(Releases, version);
        var listed = File.ReadAllLines(Path.Combine(dir, "SHA256SUMS"))
            .Where(l => l.Length > 0)
            .Select(l => Regex.Match(l, @"^([0-9a-f]{64})  (\S.*)$"))
            .ToList();
        Assert.That(listed.All(m => m.Success), "SHA256SUMS lines are '<sha256>  <path>'");
        var expected = listed.ToDictionary(m => m.Groups[2].Value, m => m.Groups[1].Value, StringComparer.Ordinal);

        var present = Directory.GetFiles(dir, "*", SearchOption.AllDirectories)
            .Select(f => Path.GetRelativePath(dir, f).Replace('\\', '/'))
            .Where(f => f != "SHA256SUMS")
            .OrderBy(f => f, StringComparer.Ordinal)
            .ToList();
        Assert.That(present, Is.EquivalentTo(expected.Keys), $"{version}: a file was added to or removed from a frozen folder");

        foreach (var (path, sha) in expected)
        {
            string actual = Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(Path.Combine(dir, path))));
            Assert.That(actual, Is.EqualTo(sha), $"{version}/{path} changed: a frozen schema is never edited");
        }
    }

    [TestCaseSource(nameof(Released))]
    public void AFrozenCoreSchemaCarriesItsFoldersVersion(string version)
    {
        string text = File.ReadAllText(Path.Combine(Releases, version, "datarepo.yaml"));
        var m = Regex.Match(text, @"^version:\s*(\S+)\s*$", RegexOptions.Multiline);
        Assert.That(m.Success, $"{version}: no version line");
        Assert.That(m.Groups[1].Value, Is.EqualTo(version));
    }

    [Test]
    public void TheWorkingSchemaIsNotAReleasedVersion()
    {
        string text = File.ReadAllText(DataPaths.Schema);
        string working = Regex.Match(text, @"^version:\s*(\S+)\s*$", RegexOptions.Multiline).Groups[1].Value;
        Assert.That(Released, Does.Not.Contain(working),
            "the working schema carries a released version: bump it before changing it, or freeze it here when it is released");
    }
}
