using System.Linq;

namespace Intl.Net.ResourceGenerator.Tests.CompilationInternals;

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Diagnostics;

public class TestingAnalyzerConfigOptions : AnalyzerConfigOptions {
  private readonly List<string> falsyKeys = new List<string> {
    "ResourceUri",
    "EnableTolgeeProvider",
  };
  
  public override bool TryGetValue(string key, out string? value) {
    value = "Intl.Net.Testing";
    return !falsyKeys.Any(key.Contains);
  }
}