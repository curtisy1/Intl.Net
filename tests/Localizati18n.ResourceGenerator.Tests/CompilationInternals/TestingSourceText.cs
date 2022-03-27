namespace Localizati18n.ResourceGenerator.Tests.CompilationInternals;

using System.Text;
using Microsoft.CodeAnalysis.Text;

public class TestingSourceText : SourceText {
  public override void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) {
    throw new System.NotImplementedException();
  }

  public override Encoding? Encoding => Encoding.UTF8;
  public override int Length => 0;

  public override char this[int position] => throw new System.NotImplementedException();
}