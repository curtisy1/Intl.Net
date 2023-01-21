namespace Intl.Net.ResourceGenerator.Tests.NestedTranslations; 

using System.Globalization;
using Intl.Net.ResourceManager;

public static class SingleTranslationExpected {
  private static JsonResourceManager? s_ResourceManager;
  public static JsonResourceManager ResourceManager => s_ResourceManager ??= new JsonResourceManager("Intl.Net.Testing.SingleTranslation", typeof(SingleTranslationExpected).Assembly, "SingleTranslation");
  public static CultureInfo? ResourceCulture { get; set; }

  public static string A => ResourceManager.GetString("A", ResourceCulture)!;
}