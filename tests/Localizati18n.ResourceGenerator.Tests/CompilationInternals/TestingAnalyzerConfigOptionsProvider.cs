namespace Localizati18n.ResourceGenerator.Tests.CompilationInternals;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

public class TestingAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider {
  public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => new TestingAnalyzerConfigOptions();

  public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => new TestingAnalyzerConfigOptions();

  public override AnalyzerConfigOptions GlobalOptions => new TestingAnalyzerConfigOptions();
}