namespace Localizati18n.ResourceGenerator.Tests {
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Threading.Tasks;
  using FluentAssertions;
  using Localizati18n.ResourceGenerator.Tests.CompilationInternals;
  using Localizati18n.ResourceGenerator.Tests.SpecialCharacters;
  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.CSharp;
  using Microsoft.CodeAnalysis.CSharp.Syntax;
  using Xunit;

  public class GeneratorTests {
    [Fact]
    public async Task Generator_HandlesSpecialCharacters() {
      var assemblyLocation = typeof(SpecialCharactersExpected).Assembly.Location;
      var expectedClassString = await File.ReadAllTextAsync(assemblyLocation + "/../../../../SpecialCharacters/SpecialCharactersExpected.cs");
      
      // Create the 'input' compilation that the generator will act on
      var inputCompilation = CreateCompilation(expectedClassString);

      var expectedSyntaxTree = inputCompilation.SyntaxTrees.First();

      var additionalTexts = new List<AdditionalText> { new TestingAdditionalText("SpecialCharacters/SpecialCharacters.json") };

      // directly create an instance of the generator
      // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
      var sourceGenerator = new SourceGenerator();
      // Create the driver that will control the generation, passing in our generator
      GeneratorDriver driver = CSharpGeneratorDriver.Create(new[] { sourceGenerator }, additionalTexts, CSharpParseOptions.Default, new TestingAnalyzerConfigOptionsProvider());

      // Run the generation pass
      // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
      driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

      var runResult = driver.GetRunResult();

      var generatedSyntaxTree = runResult.Results[0].GeneratedSources[0].SyntaxTree;

      var generatedProperties = GetProperties(generatedSyntaxTree.GetCompilationUnitRoot());
      var expectedProperties = GetProperties(expectedSyntaxTree.GetCompilationUnitRoot());

      // TODO: Also test equality of property access
      var generatedIdentifiers = generatedProperties.Select(p => p.Identifier.Text);
      var expectedIdentifiers = expectedProperties.Select(p => p.Identifier.Text);

      generatedIdentifiers.Should().Equal(expectedIdentifiers);
    }

    private static Compilation CreateCompilation(string source) =>
      CSharpCompilation.Create("Localizati18n.Testing",
                               new[] { CSharpSyntaxTree.ParseText(source) },
                               new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                               new CSharpCompilationOptions(OutputKind.ConsoleApplication));

    private static IEnumerable<PropertyDeclarationSyntax> GetProperties(SyntaxNode compilationUnit) =>
      compilationUnit.DescendantNodes()
        .Where(n => n is PropertyDeclarationSyntax { Type: PredefinedTypeSyntax })
        .Select(n => (PropertyDeclarationSyntax)n)
        .ToList();
  }
}