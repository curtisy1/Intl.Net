namespace Intl.Net.ResourceGenerator.Tests.CompilationInternals;

using System.IO;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

public class TestingAdditionalText : AdditionalText {
  private readonly string testFile;

  public TestingAdditionalText(string testFile) {
    this.testFile = testFile;
  }
  
  public override SourceText? GetText(CancellationToken cancellationToken = new CancellationToken()) {
    return new TestingSourceText();
  }

  public override string Path => $"{Directory.GetCurrentDirectory()}/{this.testFile}";
}