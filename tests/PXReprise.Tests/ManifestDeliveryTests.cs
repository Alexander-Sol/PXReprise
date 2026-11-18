using System.Text.Json.Nodes;
using PXReprise.Batch;
using PXReprise.Config;

namespace PXReprise.Tests;

/// <summary>
/// The external structure review's R14: a manifest entry says whether its ingest succeeded. The batch writes the entry
/// before it ingests (<c>delivery: pending</c>) and marks it <c>delivered</c>, with <c>delivered_utc</c>, after.
/// </summary>
public class ManifestDeliveryTests
{
    private static readonly IReadOnlyDictionary<string, Profile> Profiles = ProfileLoader.LoadDirectory(TestSupport.ProfilesDir);
    private static readonly DateTime When = new(2026, 11, 18, 20, 15, 23, DateTimeKind.Utc);

    /// <summary>A searched run folder (QC report and search provenance) and an empty manifest beside it.</summary>
    private static (string Dir, string Run, string Manifest) Searched(string acc)
    {
        string dir = TestSupport.TempDir(), run = Path.Combine(dir, "runs", acc);
        Directory.CreateDirectory(Path.Combine(run, "02b_qc"));
        Directory.CreateDirectory(Path.Combine(run, "04_search"));
        File.WriteAllText(Path.Combine(run, "02b_qc", "qc_report.json"),
            new JsonObject { ["a.raw"] = new JsonObject { ["pass"] = true, ["fail_reasons"] = new JsonArray() } }.ToJsonString());
        File.WriteAllText(Path.Combine(run, "04_search", "provenance.json"), """{"tools":{"MetaMorpheus":{"release":"1.1.14"}}}""");
        return (dir, run, TestSupport.WriteFile(dir, "manifest.yaml", "manifest_version: 1\ndatasets:\n"));
    }

    private static string EntryOf(string manifest, string acc)
    {
        string text = ManifestEntry.ReadLf(manifest);
        var (start, end) = ManifestEntry.Block(text, acc);
        return text[start..end];
    }

    [Test]
    public void ANewEntryIsPendingAndBecomesDeliveredWithItsTime()
    {
        var (dir, run, manifest) = Searched("PXD000301");
        ManifestEntry.Append(manifest, new QueueEntry("PXD000301", "t", "human"), Profiles["label-free-dda@1"], run, new JsonObject(), dir);
        Assert.That(EntryOf(manifest, "PXD000301"), Does.Contain("    status: include\n    delivery: pending\n"));

        ManifestEntry.MarkDelivered(manifest, "PXD000301", When);
        string entry = EntryOf(manifest, "PXD000301");
        Assert.That(entry, Does.Contain("    status: include\n    delivery: delivered\n    delivered_utc: 2026-11-18T20:15:23Z\n"));
        Assert.That(entry, Does.Not.Contain("pending"));
    }

    [Test]
    public void MarkingTwiceKeepsOneRecordWithTheLatestTime()
    {
        var (dir, run, manifest) = Searched("PXD000302");
        ManifestEntry.Append(manifest, new QueueEntry("PXD000302", "t", "human"), Profiles["label-free-dda@1"], run, new JsonObject(), dir);
        ManifestEntry.MarkDelivered(manifest, "PXD000302", When);
        ManifestEntry.MarkDelivered(manifest, "PXD000302", When.AddHours(1));
        string entry = EntryOf(manifest, "PXD000302");
        Assert.That(entry.Split("delivery:").Length - 1, Is.EqualTo(1));
        Assert.That(entry.Split("delivered_utc:").Length - 1, Is.EqualTo(1));
        Assert.That(entry, Does.Contain("delivered_utc: 2026-11-18T21:15:23Z"));
    }

    [Test]
    public void AnEntryWrittenBeforeTheFieldGainsItAfterItsStatusAndNothingElseMoves()
    {
        string dir = TestSupport.TempDir();
        string before = "manifest_version: 1\r\ndatasets:\r\n\r\n  - accession: PXD000303\r\n    status: include\r\n    run: runs/PXD000303\r\n    notes: >\r\n      by hand\r\n\r\n  - accession: PXD000304\r\n    status: include\r\n    run: runs/PXD000304\r\n";
        string manifest = TestSupport.WriteFile(dir, "manifest.yaml", before);
        ManifestEntry.MarkDelivered(manifest, "PXD000303", When);
        string after = File.ReadAllText(manifest);
        Assert.That(after, Is.EqualTo(before.Replace("    status: include\r\n    run: runs/PXD000303",
            "    status: include\r\n    delivery: delivered\r\n    delivered_utc: 2026-11-18T20:15:23Z\r\n    run: runs/PXD000303")),
            "CRLF kept, the other entry untouched");
    }

    [Test]
    public void AnOlderSplitEntryGainsTheRecordAfterItsSplitReasonWhereAppendWritesIt()
    {
        string dir = TestSupport.TempDir();
        string manifest = TestSupport.WriteFile(dir, "manifest.yaml",
            "manifest_version: 1\ndatasets:\n\n  - accession: PXD000307-mouse\n    status: include\n    split_reason: species\n    title: \"t\"\n");
        ManifestEntry.MarkDelivered(manifest, "PXD000307-mouse", When);
        Assert.That(File.ReadAllText(manifest), Does.Contain(
            "    status: include\n    split_reason: species\n    delivery: delivered\n    delivered_utc: 2026-11-18T20:15:23Z\n    title: \"t\"\n"));
    }

    [Test]
    public void AReSearchStartsTheRecordAfreshInsteadOfCarryingItOver()
    {
        var (dir, run, manifest) = Searched("PXD000305");
        var e = new QueueEntry("PXD000305", "t", "human");
        ManifestEntry.Append(manifest, e, Profiles["label-free-dda@1"], run, new JsonObject(), dir);
        ManifestEntry.MarkDelivered(manifest, "PXD000305", When);
        var state = new JsonObject
        {
            ["researched"] = new JsonArray(new JsonObject { ["utc"] = "2026-11-19T00:00:00Z", ["previous_profile"] = "label-free-dda@1", ["moved_to"] = "x", ["reason"] = "a test" }),
        };
        ManifestEntry.Append(manifest, e, Profiles["label-free-dda@1"], run, state, dir);
        string entry = EntryOf(manifest, "PXD000305");
        Assert.That(entry, Does.Contain("delivery: pending"));
        Assert.That(entry, Does.Not.Contain("delivered"));
        Assert.That(entry.Split("delivery:").Length - 1, Is.EqualTo(1));
    }

    [Test]
    public void MarkingAnEntryTheManifestDoesNotHaveIsAnError()
    {
        var (_, _, manifest) = Searched("PXD000306");
        Assert.Throws<InvalidOperationException>(() => ManifestEntry.MarkDelivered(manifest, "PXD000306", When));
    }
}
