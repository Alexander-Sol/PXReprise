using System.Diagnostics;
using System.Text;

namespace PXReprise.SchemaGen;

/// <summary>
/// Generates <c>docs/data/cli.md</c>, the command reference, from the <c>datarepo</c> program's own help: <c>datarepo -h</c>
/// and <c>datarepo &lt;command&gt; -h</c>, run for every command, so the page cannot describe a flag that does not exist.
/// </summary>
/// <remarks>
/// The C# twin of dataRepo's <c>tools/build_cli_docs.py</c>, with the same page. The help text lives in
/// <c>src/PXReprise.DataCli</c> (<c>Cli.Summaries</c> and each command's <c>Args.Spec</c>), with <c>mcp</c>'s and
/// <c>site</c>'s in <c>PXReprise.Mcp.McpCommand</c> and <c>PXReprise.Site.SiteCommand</c>. A command's <c>-h</c> prints and
/// then exits the process, so the help is read from a separate <c>datarepo</c> process, never in this one.
/// </remarks>
public static class CliDocs
{
    /// <summary>The order a reader meets the commands in, grouped by who runs them. Every command <c>datarepo -h</c> lists
    /// must appear here exactly once; <see cref="Render"/> refuses otherwise, so a new command cannot go undocumented.</summary>
    internal static readonly (string Title, string Lede, string[] Commands)[] Groups =
    [
        ("Check the machine", "Run first on a new machine.", ["doctor"]),
        ("Ingest: a producer's run becomes a bundle",
            "An instance operator runs these once per dataset, after the search. See [ingest.md](ingest.md) and [study.md](study.md).",
            ["manifest", "ingest", "inspect", "verify", "study", "run", "pride"]),
        ("Build and publish: bundles become a catalog and a site",
            "`publish` is `build` then `site` in one step. See [build.md](build.md), [site.md](site.md) and [operating.md](operating.md).",
            ["publish", "build", "site", "catalog"]),
        ("Share: bundles between machines",
            "Ingest and build work on a local folder; these copy whole bundles to and from a shared store (a folder or S3). See [storage.md](storage.md).",
            ["push", "pull"]),
        ("Ask: read a catalog", "See [querying.md](querying.md) and [mcp.md](mcp.md).", ["query", "mcp"]),
    ];

    /// <summary>What the help text cannot say in one line: behaviour a calling script depends on.</summary>
    internal static readonly IReadOnlyDictionary<string, string> Notes = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["manifest"] = "A dataset with a `run_enrichment` map is checked the way `ingest` would check it, against the run folder when it is reachable. A map `ingest` would refuse is printed as `REFUSED` and the command exits `1`, so a wrong map is found before a re-ingest.",
        ["ingest"] = "With `--json`, standard output carries exactly one JSON object and nothing else; the human report goes to standard error. Its keys: `datarepo` (the version), `ok`, `exit_code`, `reasons`, and `datasets`, one per accession, each with `accession`, `status` (`ingested`, `unchanged`, `excluded`, `refused` or `error`) and `reasons`, plus for a written bundle `bundle_id`, `bundle_path`, `tables` (row counts), `checks`, `mismatches`, `unresolved_modifications`, `unmatched_runs` and `findings` (the open warning codes).",
        ["run"] = "A development build (version `0.0.0-dev`, or no recorded commit) is refused: see [operating.md](operating.md#1-install-one-released-version).",
        ["pride"] = "The only command that reaches PRIDE: `build` reads the records it kept and fetches nothing. Each record is kept under `<store>/_companion/pride/<accession>/<record id>/` (`record.json`, `fetch.json`), never in a bundle, and a record unchanged since the last fetch writes nothing. Every value PRIDE publishes is kept, e-mail included. A deposit PRIDE has no record of is kept as `found = false`; a failed fetch keeps nothing, is printed as `failed`, and makes the command exit `1` after the rest are fetched.",
    };

    /// <summary>Render the page.</summary>
    /// <param name="help">The help text for a command, or for <c>datarepo -h</c> itself when given null; line breaks
    /// as <c>\n</c>, no trailing newline.</param>
    /// <exception cref="InvalidDataException"><see cref="Groups"/> is out of step with the commands <c>datarepo -h</c>
    /// lists.</exception>
    public static string Render(Func<string?, string> help)
    {
        var summaries = Summaries(help(null));
        var listed = Groups.SelectMany(g => g.Commands).ToList();
        var missing = summaries.Keys.Except(listed).OrderBy(n => n, StringComparer.Ordinal).ToList();
        var extra = listed.Except(summaries.Keys).OrderBy(n => n, StringComparer.Ordinal).ToList();
        if (missing.Count > 0 || extra.Count > 0 || listed.Count != listed.Distinct().Count())
            throw new InvalidDataException(
                $"CliDocs.Groups is out of step with `datarepo -h`: undocumented [{string.Join(", ", missing)}], unknown [{string.Join(", ", extra)}]");

        var sb = new StringBuilder();
        sb.Append("<!-- GENERATED by PXReprise.SchemaGen --cli from `datarepo <command> -h`. Do not edit by hand. -->\n\n");
        sb.Append("# Command reference\n\n");
        sb.Append("Every `datarepo` command, with every argument it takes. This page is generated from the program's own help, so it lists exactly what the executable accepts; `datarepo <command> -h` prints the same text.\n\n");
        sb.Append("New here? Start with [getting-started.md](getting-started.md), which runs the main commands end to end on the example instance shipped in this repository.\n\n");
        sb.Append("| Command | What it does |\n|---|---|\n");
        foreach (var name in listed)
            sb.Append($"| [`{name}`](#datarepo-{name}) | {Cell(summaries[name])} |\n");
        sb.Append("\n`datarepo --version` prints the version. A release build says its version (`datarepo X.Y.Z`); a build from source says `0.0.0-dev`.\n\n");
        sb.Append("**Exit status.** `0` when the command did what it was asked, including a no-op it reports (an unchanged bundle or catalog, a delivery already written). `1` when it refused: a dataset the manifest excludes, a check that failed, a file that does not match its record. The reason is on standard error. `2` for a command line it cannot parse.\n");
        foreach (var (title, lede, names) in Groups)
        {
            sb.Append($"\n## {title}\n\n{lede}\n");
            foreach (var name in names)
            {
                var summary = summaries[name];
                sb.Append($"\n### `datarepo {name}`\n\n{summary[..1].ToUpperInvariant()}{summary[1..]}.\n\n");
                sb.Append($"```text\n{help(name)}\n```\n");
                if (Notes.TryGetValue(name, out var note)) sb.Append($"\n{note}\n");
            }
        }
        return sb.ToString();
    }

    /// <summary>The help of a built <c>datarepo</c>: run as <c>dotnet &lt;datarepo.dll&gt;</c> when given a .dll (the
    /// <c>dotnet</c> that runs this process when the SDK says which, <c>DOTNET_HOST_PATH</c>), otherwise as the executable
    /// itself.</summary>
    public static Func<string?, string> FromProgram(string program) => command =>
    {
        var host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") is { Length: > 0 } h && File.Exists(h) ? h : "dotnet";
        var psi = new ProcessStartInfo(program.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? host : program)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            StandardOutputEncoding = Encoding.UTF8,
        };
        if (program.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)) psi.ArgumentList.Add(program);
        if (command is not null) psi.ArgumentList.Add(command);
        psi.ArgumentList.Add("-h");
        using var p = Process.Start(psi) ?? throw new InvalidOperationException($"could not start {program}");
        var stdout = p.StandardOutput.ReadToEndAsync();
        var stderr = p.StandardError.ReadToEnd();
        p.WaitForExit();
        if (p.ExitCode != 0)
            throw new InvalidOperationException($"`datarepo {command} -h` exited {p.ExitCode}: {stderr}");
        return stdout.Result.Replace("\r\n", "\n").TrimEnd('\n');
    };

    /// <summary>The <c>commands:</c> block of <c>datarepo -h</c>: command to its one line.</summary>
    private static Dictionary<string, string> Summaries(string top)
    {
        var lines = top.Split('\n');
        var found = new Dictionary<string, string>(StringComparer.Ordinal);
        for (var i = Array.IndexOf(lines, "commands:") + 1; i > 0 && i < lines.Length && lines[i].StartsWith("  ", StringComparison.Ordinal); i++)
        {
            var line = lines[i].Trim();
            var space = line.IndexOf(' ');
            found[space < 0 ? line : line[..space]] = space < 0 ? "" : line[(space + 1)..].Trim();
        }
        return found;
    }

    /// <summary>A table cell: whitespace folded, <c>|</c> escaped, angle brackets as entities (build_cli_docs.py's <c>_cell</c>).</summary>
    private static string Cell(string text) =>
        string.Join(" ", text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
            .Replace("|", "\\|").Replace("<", "&lt;").Replace(">", "&gt;");
}
