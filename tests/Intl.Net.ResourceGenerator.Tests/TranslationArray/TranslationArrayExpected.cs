namespace Intl.Net.ResourceGenerator.Tests.NestedTranslations; 

using System.Globalization;
using Intl.Net.ResourceManager;

public static class TranslationArrayExpected {
  private static JsonResourceManager? s_ResourceManager;
  public static JsonResourceManager ResourceManager => s_ResourceManager ??= new JsonResourceManager("Intl.Net.Testing.TranslationArray", typeof(TranslationArrayExpected).Assembly, "TranslationArray");
  public static CultureInfo? ResourceCulture { get; set; }

  public static string A => ResourceManager.GetString("A", ResourceCulture)!;
  public static string B => ResourceManager.GetString("B", ResourceCulture)!;
  public static string C => ResourceManager.GetString("C", ResourceCulture)!;
  public static string D_E => ResourceManager.GetString("D.E", ResourceCulture)!;
  public static string F => ResourceManager.GetString("F", ResourceCulture)!;
  public static string G_H => ResourceManager.GetString("G.H", ResourceCulture)!;
}