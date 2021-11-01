namespace Localizati18n.ResourceGenerator {
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Globalization;
  using System.IO;

  internal static class StringExtensions {
    public static string GetBaseName(this string filePath) {
      var name = Path.GetFileNameWithoutExtension(filePath);
      var innerExtension = Path.GetExtension(name);
      var languageName = innerExtension.TrimStart('.');

      return IsValidLanguageName(languageName) ? Path.GetFileNameWithoutExtension(name) : name;
    }

    public static string ReplaceAll(this string str, IDictionary<string, string> stringValueMap) {
      var newStr = str;
      foreach (var (key, value) in stringValueMap) {
        newStr = newStr.Replace(key, value);
      }

      return newStr;
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