namespace Localizati18n.ResourceGenerator.Tests.NestedTranslations; 

using System.Globalization;
using Localizati18n.ResourceManager;

public static class NestedTranslationsExpected {
  private static JsonResourceManager? s_ResourceManager;
  public static JsonResourceManager ResourceManager => s_ResourceManager ??= new JsonResourceManager("Localizati18n.Testing.NestedTranslations", typeof(NestedTranslationsExpected).Assembly, "NestedTranslations");
  public static CultureInfo? ResourceCulture { get; set; }

  public static string A => ResourceManager.GetString("A", ResourceCulture)!;
  public static string B_1 => ResourceManager.GetString("B.1", ResourceCulture)!;
  public static string B_2 => ResourceManager.GetString("B.2", ResourceCulture)!;
  public static string C_1_a => ResourceManager.GetString("C.1.a", ResourceCulture)!;
  public static string C_1_b => ResourceManager.GetString("C.1.b", ResourceCulture)!;
  public static string C_2_a => ResourceManager.GetString("C.2.a", ResourceCulture)!;
  public static string C_2_b => ResourceManager.GetString("C.2.b", ResourceCulture)!;
  public static string D_E_F_G_H_I_J_K_L => ResourceManager.GetString("D.E.F.G.H.I.J.K.L", ResourceCulture)!;
}