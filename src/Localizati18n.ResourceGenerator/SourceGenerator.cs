namespace Localizati18n.ResourceGenerator {
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.Diagnostics;
  using Plugins;

  internal static class AnalyzerConfigOptionsExtensions {
    public static string? GetValueOrDefault(this AnalyzerConfigOptions options, string key) => options.TryGetValue(key, out var value) ? value : null;
  }

  [Generator]
  public class SourceGenerator : ISourceGenerator {
    private readonly List<IPlugin> plugins  = new List<IPlugin> {
      new TolgeePlugin(),
    };
    
    private static string GetLocalNamespace(string? resourcePath, string? projectPath, string? rootNamespace) {
      try {
        if (resourcePath is null) {
          return string.Empty;
        }

        var resourceFolder = Path.GetDirectoryName(resourcePath);
        var projectFolder = Path.GetDirectoryName(projectPath);
        rootNamespace ??= string.Empty;

        if (resourceFolder is null || projectFolder is null) {
          return string.Empty;
        }

        var localNamespace = rootNamespace;
        if (resourceFolder.StartsWith(projectFolder, StringComparison.OrdinalIgnoreCase)) {
          localNamespace += resourceFolder[projectFolder.Length..]
            .Replace(Path.DirectorySeparatorChar, '.')
            .Replace(Path.AltDirectorySeparatorChar, '.');
        }

        return localNamespace;
      } catch (Exception) {
        return string.Empty;
      }
    }

    public void Execute(GeneratorExecutionContext context) {
      context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Info", "Starting resource generation", DiagnosticSeverity.Info, DiagnosticSeverity.Info, false, 2));

      if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.MSBuildProjectFullPath", out var projectFullPath)) {
        context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Error", "No project path to check", DiagnosticSeverity.Error, DiagnosticSeverity.Error, true, 0));
        return;
      }

      if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace)) {
        context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Error", "No root namespace specified for source generation", DiagnosticSeverity.Error, DiagnosticSeverity.Error, true, 0));
        return;
      }

      var resourceFilePaths = new List<string>();
      foreach (var plugin in this.plugins) {
        // source generators do not support async. This is by-design according to the Roslyn team
        // see https://github.com/dotnet/roslyn/issues/44045 for details
        resourceFilePaths.AddRange(plugin.FetchResources(context).GetAwaiter().GetResult());
      }
      
      resourceFilePaths.AddRange(context.AdditionalFiles
        .Where(af => af.Path.EndsWith(".json"))
        .Where(af => Path.GetFileNameWithoutExtension(af.Path) == af.Path.GetBaseName())
        .Select(af => af.Path)
        .ToList());

      if (!resourceFilePaths.Any()) {
        context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Warning", "No files found to generate resources for!", DiagnosticSeverity.Warning, DiagnosticSeverity.Warning, true, 1));
      }

      var customToolNamespace = context.AnalyzerConfigOptions.GlobalOptions.GetValueOrDefault("build_property.CustomToolNamespace");
      foreach (var resourceFile in resourceFilePaths) {
        context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Info", $"Generating resource from file {resourceFile}", DiagnosticSeverity.Info, DiagnosticSeverity.Info, false, 2));
        using var stream = File.OpenRead(resourceFile);
        var className = Path.GetFileNameWithoutExtension(resourceFile);
        var generatedNamespace = customToolNamespace ?? GetLocalNamespace(resourceFile, projectFullPath, rootNamespace);

        context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Info", $"Generating resource using namespace {generatedNamespace} and classname {className}", DiagnosticSeverity.Info, DiagnosticSeverity.Info, false, 2));

        using var generator = new Generator(stream, new GeneratorOptions(generatedNamespace, className));
        var unit = generator.Generate();
        context.AddSource($"{className}.cs", unit.ToFullString());
      }
    }

    public void Initialize(GeneratorInitializationContext context) { }
  }
}