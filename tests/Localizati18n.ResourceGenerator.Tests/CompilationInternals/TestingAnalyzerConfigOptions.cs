namespace Localizati18n.ResourceGenerator.Tests.CompilationInternals;

using Microsoft.CodeAnalysis.Diagnostics;

public class TestingAnalyzerConfigOptions : AnalyzerConfigOptions {
  public override bool TryGetValue(string key, out string? value) {
    value = "Localizati18n.Testing";
    return !key.Contains("ResourceLocation");
  }
}