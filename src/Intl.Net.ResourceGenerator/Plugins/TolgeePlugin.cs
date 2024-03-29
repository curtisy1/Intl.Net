namespace Intl.Net.ResourceGenerator.Plugins {

  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.IO.Compression;
  using System.Net.Http;
  using System.Threading.Tasks;
  using Microsoft.CodeAnalysis;

  public class TolgeePlugin : PluginBase {
    public TolgeePlugin() {
      this.PluginName = "Tolgee";
    }
    
    public override async Task<IEnumerable<string>> FetchResources(GeneratorExecutionContext context) {
      var files = new List<string>();
      var (enabled, uri, downloadPath) = this.GetConfiguredVariables(context);
      
      if (!enabled) {
        context.ReportDiagnostic(Diagnostic.Create("SourceGenerator.Plugin.Tolgee", "Info", "Tolgee plugin not enabled",
          DiagnosticSeverity.Info, DiagnosticSeverity.Info, false, 2));
        return files;
      }
      
      if (string.IsNullOrEmpty(uri)) {
        context.ReportDiagnostic(Diagnostic.Create("ResourceGenerator.Plugin.Tolgee", "Warning", "No uri found",
          DiagnosticSeverity.Warning, DiagnosticSeverity.Warning, true, 1));
        return files;
      }

      try {
        using var client = new HttpClient();
        await using var response = await client.GetStreamAsync(uri);
        using var archive = new ZipArchive(response);
        foreach (var entry in archive.Entries) {
          await using var stream = entry.Open();
          var destination = Path.GetFullPath(Path.Combine(downloadPath, entry.FullName));

          var directory = Path.GetDirectoryName(destination);
          if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
          }

          await using var file = new FileStream(destination, FileMode.Create, FileAccess.Write);
          await stream.CopyToAsync(file);
          files.Add(destination);
        }
      } catch (Exception ex) {
        context.ReportDiagnostic(Diagnostic.Create("ResourceGenerator.Plugin.Tolgee", "Error", ex.Message,
          DiagnosticSeverity.Error, DiagnosticSeverity.Error, true, 0));
      }

      return files;
    }
  }
}