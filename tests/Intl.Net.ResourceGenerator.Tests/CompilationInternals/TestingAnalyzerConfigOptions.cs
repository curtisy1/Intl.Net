namespace Intl.Net.ResourceGenerator.Tests.CompilationInternals;

using Microsoft.CodeAnalysis.Diagnostics;

public class TestingAnalyzerConfigOptions : AnalyzerConfigOptions {
  public override bool TryGetValue(string key, out string? value) {
    value = "Intl.Net.Testing";
    return !key.Contains("ResourceLocation");
  }
}