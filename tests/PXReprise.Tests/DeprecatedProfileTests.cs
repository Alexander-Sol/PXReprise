using System.Text.Json;
using PXReprise.Config;
using PXReprise.Discovery;

namespace PXReprise.Tests;

/// <summary>
/// A profile's <c>deprecated</c> status (the external structure review's R9): the profile still loads, so the results made
/// under it read as before, but it takes no new search.
/// </summary>
public class DeprecatedProfileTests
{
    private static readonly IReadOnlyDictionary<string, Profile> Shipped = ProfileLoader.LoadDirectory(TestSupport.ProfilesDir);

    /// <summary>The shipped profiles, with the named ones marked deprecated.</summary>
    private static Dictionary<string, Profile> WithDeprecated(params string[] keys) =>
        Shipped.ToDictionary(kv => kv.Key, kv => keys.Contains(kv.Key) ? kv.Value with { Status = ProfileStatus.Deprecated } : kv.Value);

    private static Question QuestionNaming(params string[] profiles) =>
        QuestionLoader.Load(TestSupport.WriteFile(TestSupport.TempDir(), "q.toml", TestSupport.MinimalQuestion.Replace(
            "profiles = [\"label-free-dda@1\", \"tmt-dda@1\"]",
            $"profiles = [{string.Join(", ", profiles.Select(p => $"\"{p}\""))}]")));

    private static Route RouteLabelFree(Question q, IReadOnlyDictionary<string, Profile> profiles)
    {
        var r = TestSupport.Record("PXD000010", "Muscle proteome in type 2 diabetes");
        return Router.Assign(r.Accession, AcquisitionClassifier.Classify(r), Relevance.Evaluate(r, q.Relevance), q, profiles);
    }

    [Test]
    public void ADeprecatedProfileStillLoads()
    {
        string text = File.ReadAllText(Path.Combine(TestSupport.ProfilesDir, "label-free-dda-1.toml"))
            .Replace("status = \"available\"", "status = \"deprecated\"");
        var p = ProfileLoader.Load(TestSupport.WriteFile(TestSupport.TempDir(), "label-free-dda-1.toml", text));
        Assert.That((p.Key, p.Status), Is.EqualTo(("label-free-dda@1", ProfileStatus.Deprecated)));
    }

    [Test]
    public void ADeprecatedProfileIsPassedOverForTheNextProfileTheQuestionNames()
    {
        var q = QuestionNaming("label-free-dda@1", "label-free-dda@2");
        Assert.That(RouteLabelFree(q, Shipped), Is.EqualTo(new Route(RouteKind.Search, "label-free-dda@1", "accepted")), "the control");
        Assert.That(RouteLabelFree(q, WithDeprecated("label-free-dda@1")), Is.EqualTo(new Route(RouteKind.Search, "label-free-dda@2", "accepted")));
    }

    [Test]
    public void ADepositOnlyADeprecatedProfileAcceptsWaitsAndNamesIt()
    {
        var route = RouteLabelFree(QuestionNaming("label-free-dda@1", "tmt-dda@2"), WithDeprecated("label-free-dda@1"));
        Assert.Multiple(() =>
        {
            Assert.That(route.Kind, Is.EqualTo(RouteKind.WaitingOnCapability));
            Assert.That(route.Profile, Is.EqualTo("label-free-dda@1"));
            Assert.That(route.Reason, Does.Contain("label-free-dda@1 is deprecated"));
        });
    }

    [Test]
    public void TheDeprecatedProfilesAQuestionNamesAreListedInItsOrder()
    {
        var q = QuestionNaming("label-free-dda@2", "label-free-dda@1", "tmt-dda@2");
        Assert.That(Router.DeprecatedProfiles(q, WithDeprecated("label-free-dda@1", "label-free-dda@2")),
            Is.EqualTo(new[] { "label-free-dda@2", "label-free-dda@1" }));
        Assert.That(Router.DeprecatedProfiles(q, Shipped), Is.Empty);
    }

    [Test]
    public async Task ValidateListsTheDeprecatedProfilesTheQuestionNames()
    {
        string profiles = Path.Combine(TestSupport.TempDir(), "profiles");
        CopyDirectory(TestSupport.ProfilesDir, profiles);
        string lf1 = Path.Combine(profiles, "label-free-dda-1.toml");
        File.WriteAllText(lf1, File.ReadAllText(lf1).Replace("status = \"available\"", "status = \"deprecated\""));
        string q = TestSupport.WriteFile(TestSupport.TempDir(), "question.toml", TestSupport.MinimalQuestion);

        var (exit, data) = await Run("validate", q, "--profiles", profiles);
        Assert.That(exit, Is.EqualTo(0));
        Assert.That(data.GetProperty("deprecated_profiles").EnumerateArray().Select(x => x.GetString()), Is.EqualTo(new[] { "label-free-dda@1" }));

        var (_, shipped) = await Run("validate", q, "--profiles", TestSupport.ProfilesDir);
        Assert.That(shipped.TryGetProperty("deprecated_profiles", out var none) && none.ValueKind != JsonValueKind.Null, Is.False,
            "none deprecated: none reported");
    }

    private static async Task<(int Exit, JsonElement Data)> Run(params string[] argv)
    {
        var sw = new StringWriter();
        int exit = await Program.RunAsync(argv, sw, CancellationToken.None, () => new FakeSearch());
        var doc = JsonDocument.Parse(sw.ToString());
        return (exit, doc.RootElement.TryGetProperty("data", out var d) ? d.Clone() : doc.RootElement.Clone());
    }

    private static void CopyDirectory(string from, string to)
    {
        Directory.CreateDirectory(to);
        foreach (string f in Directory.EnumerateFiles(from)) File.Copy(f, Path.Combine(to, Path.GetFileName(f)));
        foreach (string d in Directory.EnumerateDirectories(from)) CopyDirectory(d, Path.Combine(to, Path.GetFileName(d)));
    }
}
