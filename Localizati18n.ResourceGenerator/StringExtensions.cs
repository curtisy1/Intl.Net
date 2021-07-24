namespace Localizati18n.ResourceGenerator {
  using System;
  using System.Globalization;
  using System.IO;

  internal static class StringExtensions {
    public static string GetBaseName(this string filePath) {
      var name = Path.GetFileNameWithoutExtension(filePath);
      var innerExtension = Path.GetExtension(name);
      var languageName = innerExtension.TrimStart('.');

      return IsValidLanguageName(languageName) ? Path.GetFileNameWithoutExtension(name) : name;
    }

    private static bool IsValidLanguageName(this string? languageName) {
      try {
        if (string.IsNullOrEmpty(languageName)) {
          return false;
        }

        if (languageName.StartsWith("qps-", StringComparison.Ordinal)) {
          return true;
        }

        var culture = new CultureInfo(languageName);

        while (!culture.IsNeutralCulture) {
          culture = culture.Parent;
        }

        return culture.LCID != 4096;
      } catch {
        return false;
      }
    }
  }
}