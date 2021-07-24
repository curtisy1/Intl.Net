namespace Localizati18n.ResourceGenerator.Tests {
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using FluentAssertions;
  using Xunit;

  public class GeneratorTests {
    [Fact]
    public void GetCompilationUnit() {
      var text = @"{ ""CreateDate"": ""Oldest"", ""CreateDateDescending"": ""Newest"" }";
      var replaceValues = new[] {
        "\r",
        "\n",
        " "
      };
      var expected = ReplaceAll(replaceValues, "", @"namespace Resources 
{
    using System.Globalization;
    using Localizati18n.ResourceManager;

    public static class SampleResourceStrings
    {
        private static JsonResourceManager? s_ResourceManager;
        public static JsonResourceManager ResourceManager => s_ResourceManager ??= new JsonResourceManager(""Resources.SampleResourceStrings"", typeof(SampleResourceStrings).Assembly);
        public static CultureInfo? CultureInfo { get; set; }

        public static string CreateDate => ResourceManager.GetString(nameof(CreateDate), CultureInfo)!;
        public static string CreateDateDescending => ResourceManager.GetString(nameof(CreateDateDescending), CultureInfo)!;
    }
}");
      using var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
      using var generator = new Generator(stream, new GeneratorOptions("Resources", "SampleResourceStrings"));
      ReplaceAll(replaceValues, "", generator.Generate().ToFullString()).Should().Be(expected);
    }

    private static string ReplaceAll(IEnumerable<string> oldValues, string newValue, string value) {
      return oldValues.Aggregate(value, (current, oldValue) => current.Replace(oldValue, newValue));
    }
  }
}