namespace Localizati18n.ResourceGenerator {
  using System;
  using System.IO;
  using System.Linq;
  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.Diagnostics;

  internal static class AnalyzerConfigOptionsExtensions {
    public static string? GetValueOrDefault(this AnalyzerConfigOptions options, string key) => options.TryGetValue(key, out var value) ? value : null;
  }

  [Generator]
  public class SourceGenerator : ISourceGenerator {
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
      context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Info", "Starting resource generation", DiagnosticSeverity.Info, DiagnosticSeverity.Info, true, 1));

      if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.MSBuildProjectFullPath", out var projectFullPath)) {
        context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Error", "No project path to check", DiagnosticSeverity.Error, DiagnosticSeverity.Error, true, 0));
        return;
      }

      if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace)) {
        context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Error", "No root namespace specified for source generation", DiagnosticSeverity.Error, DiagnosticSeverity.Error, true, 0));
        return;
      }

      var resourceFiles = context.AdditionalFiles
        .Where(af => af.Path.EndsWith(".json"))
        .Where(af => Path.GetFileNameWithoutExtension(af.Path) == af.Path.GetBaseName())
        .ToList();

      if (!resourceFiles.Any()) {
        context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Warning", "No files found to generate resources for!", DiagnosticSeverity.Warning, DiagnosticSeverity.Warning, true, 1));
      }
      
      foreach (var resourceFile in resourceFiles) {
        context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Info", $"Generating resource from file {resourceFile.Path}", DiagnosticSeverity.Warning, DiagnosticSeverity.Warning, true, 1));
        using var stream = File.OpenRead(resourceFile.Path);
        var customToolNamespace = context.AnalyzerConfigOptions.GetOptions(resourceFile).GetValueOrDefault("build_metadata.EmbeddedResource.CustomToolNamespace");
        var className = Path.GetFileNameWithoutExtension(resourceFile.Path);
        var generatedNamespace = customToolNamespace ?? GetLocalNamespace(resourceFile.Path, projectFullPath, rootNamespace);
        
        context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Warning", $"Generating resource using namespace {generatedNamespace} and classname {className}", DiagnosticSeverity.Warning, DiagnosticSeverity.Warning, true, 1));

        using var generator = new Generator(stream, new GeneratorOptions(generatedNamespace, className));
        var unit = generator.Generate();
        context.AddSource($"{generatedNamespace}.{className}.cs", unit.ToFullString());
        context.ReportDiagnostic(Diagnostic.Create("SourceGenerator", "Warning", $"Generated resource {unit.ToFullString().Replace("\r", "").Replace("\n", "")}", DiagnosticSeverity.Warning, DiagnosticSeverity.Warning, true, 1));
      }
    }

    public void Initialize(GeneratorInitializationContext context) { }
  }
}