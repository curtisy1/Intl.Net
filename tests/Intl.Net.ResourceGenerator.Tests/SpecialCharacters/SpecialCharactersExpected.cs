namespace Intl.Net.ResourceGenerator.Tests.SpecialCharacters;

using System.Globalization;
using Intl.Net.ResourceManager;

public static class SpecialCharactersExpected {
  private static JsonResourceManager? s_ResourceManager;
  public static JsonResourceManager ResourceManager => s_ResourceManager ??= new JsonResourceManager("Intl.Net.Testing.SpecialCharacters", typeof(SpecialCharactersExpected).Assembly, "SpecialCharacters");
  public static CultureInfo? ResourceCulture { get; set; }

  public static string TestString => ResourceManager.GetString("TestString", ResourceCulture)!;
  public static string ß => ResourceManager.GetString("ß", ResourceCulture)!;
  public static string _ => ResourceManager.GetString("2", ResourceCulture)!;
  public static string ö => ResourceManager.GetString("ö", ResourceCulture)!;
  public static string _A => ResourceManager.GetString(">A", ResourceCulture)!;
  public static string _B => ResourceManager.GetString("<B", ResourceCulture)!;
  public static string _C => ResourceManager.GetString("=C", ResourceCulture)!;
  public static string _D => ResourceManager.GetString("!D", ResourceCulture)!;
  public static string _E => ResourceManager.GetString("?E", ResourceCulture)!;
  public static string _F => ResourceManager.GetString(":F", ResourceCulture)!;
  public static string _G => ResourceManager.GetString("#G", ResourceCulture)!;
  public static string _H => ResourceManager.GetString("~H", ResourceCulture)!;
  public static string _I => ResourceManager.GetString("¿I", ResourceCulture)!;
  public static string _J => ResourceManager.GetString("&J", ResourceCulture)!;
  public static string _K => ResourceManager.GetString("%K", ResourceCulture)!;
  public static string _L => ResourceManager.GetString("²L", ResourceCulture)!;
  public static string _M => ResourceManager.GetString("|M", ResourceCulture)!;
  public static string _N => ResourceManager.GetString("\\N", ResourceCulture)!;
  public static string _O => ResourceManager.GetString("/O", ResourceCulture)!;
  public static string _P_ => ResourceManager.GetString("{P}", ResourceCulture)!;
  public static string _Q => ResourceManager.GetString("[Q", ResourceCulture)!;
  public static string _R_ => ResourceManager.GetString("´R`", ResourceCulture)!;
  public static string _S => ResourceManager.GetString("\"S", ResourceCulture)!;
  public static string _T => ResourceManager.GetString(";T", ResourceCulture)!;
  public static string _U => ResourceManager.GetString("⅛U", ResourceCulture)!;
  public static string _V => ResourceManager.GetString("¤V", ResourceCulture)!;
  public static string W_ => ResourceManager.GetString("W™", ResourceCulture)!;
  public static string _X => ResourceManager.GetString("°X", ResourceCulture)!;
  public static string ΩY => ResourceManager.GetString("ΩY", ResourceCulture)!;
  public static string _Z => ResourceManager.GetString("_Z", ResourceCulture)!;
  public static string __ => ResourceManager.GetString("💩", ResourceCulture)!;
  public static string __ZZ => ResourceManager.GetString("😀ZZ", ResourceCulture)!;
  public static string Ω___Ŧ__ıØÞẞÐªŊŊĦŁ_______º___ => ResourceManager.GetString("Ω§€®Ŧ¥↑ıØÞẞÐªŊŊĦŁ›‹©‚‚‘’º×÷—", ResourceCulture)!;
}