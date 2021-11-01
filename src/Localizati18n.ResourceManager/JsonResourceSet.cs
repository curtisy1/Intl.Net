namespace Localizati18n.ResourceManager {
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Resources;
  using System.Text.Json;

  public class JsonResourceSet : ResourceSet {
    private readonly List<string> stringValueMap = new() {
      ".",
      "-",
      "[",
      "]",
    };
    private readonly ConcurrentDictionary<string, string> resources = new();

    public JsonResourceSet(Stream stream) {
      this.FillResourceCache(stream);
    }

    public JsonResourceSet(string fileName) {
      if (File.Exists(fileName)) {
        this.FillResourceCache(File.OpenRead(fileName));
      }
    }

    public override Type GetDefaultReader() {
      return null;
    }

    public override Type GetDefaultWriter() {
      return null;
    }

    public override string? GetString(string name) {
      if (!this.resources.TryGetValue(name, out var outValue) && name.Contains('_')) {
        if (this.stringValueMap.Any(stringValue => this.resources.TryGetValue(name.Replace("_", stringValue), out outValue))) {
          return outValue;
        }
      }

      return string.IsNullOrEmpty(outValue) ? name : outValue;
    }

    public IEnumerable<KeyValuePair<string, string>> GetStrings() {
      return this.resources.ToArray();
    }

    protected override void ReadResources() { }

    private void FillResourceCache(Stream stream) {
      var resourceContent = JsonSerializer.Deserialize<Dictionary<string, string>>(new StreamReader(stream).ReadToEnd());
      foreach (var (k, v) in resourceContent) {
        this.resources.TryAdd(k, v);
      }
    }
  }
}