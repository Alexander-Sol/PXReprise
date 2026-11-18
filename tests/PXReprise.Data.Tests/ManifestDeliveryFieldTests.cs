using PXReprise.Bundle;
using PXReprise.Ingest;

namespace PXReprise.Data.Tests;

/// <summary>
/// The engine's <c>delivery</c> and <c>delivered_utc</c> on a manifest entry (the external structure review's R14): the
/// ingester reads such an entry exactly as one without them, and neither field reaches a bundle's identity.
/// </summary>
public class ManifestDeliveryFieldTests
{
    private const string Head = "manifest_version: 1\nwork_root: .\nstore: store\ndatasets:\n  - accession: PXD999999\n    status: include\n";
    private const string Tail = "    run: runs/PXD999999\n    title: \"a deposit\"\n    files: 2\n    organism: NCBITaxon:9606\n";

    private static Manifest LoadText(string text)
    {
        string dir = Path.Combine(Path.GetTempPath(), "datarepo-delivery-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        string path = Path.Combine(dir, "manifest.yaml");
        File.WriteAllText(path, text);
        return Manifest.Load(path);
    }

    [TestCase("")]
    [TestCase("    delivery: pending\n")]
    [TestCase("    delivery: delivered\n    delivered_utc: 2026-11-18T20:15:23Z\n")]
    public void AnEntryReadsTheSameWithOrWithoutItsDeliveryRecord(string delivery)
    {
        var plain = LoadText(Head + Tail).Datasets["PXD999999"];
        var marked = LoadText(Head + delivery + Tail).Datasets["PXD999999"];
        Assert.Multiple(() =>
        {
            Assert.That(marked.Ingestable, Is.True);
            Assert.That(PyFormat.Json(marked.ContentDeclaration()), Is.EqualTo(PyFormat.Json(plain.ContentDeclaration())),
                "the bundle id is the same with or without the delivery record");
        });
    }

    [Test]
    public void TheDeliveryFieldsAreClassifiedAsNotContent()
    {
        Assert.That(DatasetEntry.NonContentFields.Keys, Does.Contain("delivery").And.Contain("delivered_utc"));
        Assert.That(DatasetEntry.ContentFields, Does.Not.Contain("delivery").And.Not.Contain("delivered_utc"));
    }
}
