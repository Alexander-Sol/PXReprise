using System.Diagnostics;
using PXReprise.SchemaGen;

// Usage, from the repository root or anywhere inside it:
//   dotnet run --project src/PXReprise.SchemaGen                    write Tables.g.cs, SchemaDocs.g.cs and the schema
//                                                                   reference pages docs/data/schema/*.md
//   dotnet run --project src/PXReprise.SchemaGen -- --check          exit 1 if any of them is stale
//   dotnet run --project src/PXReprise.SchemaGen -- --cli            build the datarepo CLI, then write docs/data/cli.md
//                                                                   from its own -h output
//   dotnet run --project src/PXReprise.SchemaGen -- --cli --check    exit 1 if docs/data/cli.md is stale
//   ... --cli --exe <datarepo.dll or datarepo executable>           use an already-built CLI instead
var root = FindRoot(Directory.GetCurrentDirectory())
    ?? FindRoot(AppContext.BaseDirectory)
    ?? throw new DirectoryNotFoundException("no schemas/datarepo/datarepo.yaml above the working directory");
var cli = args.Contains("--cli");
var regenerate = "dotnet run --project src/PXReprise.SchemaGen" + (cli ? " -- --cli" : "");
var outputs = new List<(string Target, string Text)>();
if (cli)
{
    var exe = Array.IndexOf(args, "--exe");
    string? built = null;
    var program = exe >= 0 && exe + 1 < args.Length ? Path.GetFullPath(args[exe + 1]) : (built = BuildDataCli(root));
    try
    {
        outputs.Add((Path.Combine(root, "docs", "data", "cli.md"), CliDocs.Render(CliDocs.FromProgram(program))));
    }
    finally
    {
        if (built is not null) Directory.Delete(Path.GetDirectoryName(built)!, recursive: true);
    }
}
else
{
    var schemaPath = Path.Combine(root, "schemas", "datarepo", "datarepo.yaml");
    var studyDir = Path.Combine(root, "schemas", "datarepo", "study");
    var generated = Path.Combine(root, "src", "PXReprise.Bundle", "Generated");
    outputs.Add((Path.Combine(generated, "Tables.g.cs"), SchemaGenerator.Render(schemaPath, studyDir)));
    outputs.Add((Path.Combine(generated, "SchemaDocs.g.cs"), SchemaGenerator.RenderDocs(schemaPath, studyDir)));
    outputs.AddRange(SchemaGenerator.ReferencePages(root));
}

if (args.Contains("--check"))
{
    var stale = 0;
    foreach (var (target, text) in outputs)
    {
        // Line endings are not content: git may check the file out with CRLF on Windows.
        var current = File.Exists(target) ? File.ReadAllText(target).Replace("\r\n", "\n") : "";
        if (current == text) continue;
        Console.Error.WriteLine($"{Path.GetRelativePath(root, target)} is out of date; run {regenerate}");
        stale++;
    }
    if (stale > 0) return 1;
    Console.WriteLine($"up to date: {string.Join(", ", outputs.Select(o => Path.GetFileName(o.Target)))}");
    return 0;
}

foreach (var (target, text) in outputs)
{
    Directory.CreateDirectory(Path.GetDirectoryName(target)!);
    File.WriteAllText(target, text, new System.Text.UTF8Encoding(false));
    Console.WriteLine($"wrote {Path.GetRelativePath(root, target)}");
}
return 0;

static string? FindRoot(string start)
{
    for (var dir = new DirectoryInfo(start); dir is not null; dir = dir.Parent)
        if (File.Exists(Path.Combine(dir.FullName, "schemas", "datarepo", "datarepo.yaml")))
            return dir.FullName;
    return null;
}

// Build the datarepo CLI once into a fresh temporary folder and return its datarepo.dll (run as `dotnet datarepo.dll`,
// which works on every platform).
static string BuildDataCli(string root)
{
    var outDir = Path.Combine(Path.GetTempPath(), "pxreprise-cli-docs-" + Guid.NewGuid().ToString("N")[..8]);
    var psi = new ProcessStartInfo("dotnet") { UseShellExecute = false, RedirectStandardOutput = true };
    foreach (var a in new[] { "build", Path.Combine(root, "src", "PXReprise.DataCli"), "-c", "Release", "-o", outDir, "-v", "quiet", "-nologo" })
        psi.ArgumentList.Add(a);
    using var p = Process.Start(psi) ?? throw new InvalidOperationException("could not start dotnet");
    var log = p.StandardOutput.ReadToEnd();
    p.WaitForExit();
    if (p.ExitCode != 0)
        throw new InvalidOperationException($"dotnet build src/PXReprise.DataCli exited {p.ExitCode}:\n{log}");
    var dll = Path.Combine(outDir, "datarepo.dll");
    return File.Exists(dll) ? dll : throw new FileNotFoundException("dotnet build wrote no datarepo.dll", dll);
}
