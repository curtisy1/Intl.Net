namespace Localizati18n.ResourceGenerator {
  using System;
  using System.Globalization;
  using System.IO;
  using System.Linq;

  internal static class StringExtensions {
    public static string GetBaseName(this string filePath) {
      var name = Path.GetFileNameWithoutExtension(filePath);
      var innerExtension = Path.GetExtension(name);
      var languageName = innerExtension.TrimStart('.');

      return IsValidLanguageName(languageName) ? Path.GetFileNameWithoutExtension(name) : name;
    }

    public static string SubstituteInvalidCharacters(this string str) {
      var charArr = str.ToCharArray();
      
      // Edge case: A declaration cannot contain only numbers
      if (charArr.All(char.IsDigit)) {
        return new string(charArr.Select(c => '_').ToArray());
      }
      
      return new string(charArr.Select(ch => char.IsLetterOrDigit(ch) ? ch : '_').ToArray());
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