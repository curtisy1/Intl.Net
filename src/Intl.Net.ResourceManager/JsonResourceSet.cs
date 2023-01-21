namespace Intl.Net.ResourceManager {
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
    
    private void GetNameFromProperty(JsonProperty property, string parentName = "") {
      if (property.Value.ValueKind is JsonValueKind.Array or JsonValueKind.Object) {
        GetNameFromElement(property.Value, parentName + property.Name + ".");
      } else {
        this.resources.TryAdd(parentName + property.Name, property.Value.GetString());
      }
    }
    
    private void GetNameFromElement(JsonElement element, string parentName = "") {
      if (element.ValueKind is JsonValueKind.Array) {
        element.EnumerateArray().ToList().ForEach(el => GetNameFromElement(el, parentName));
      } else if (element.ValueKind is JsonValueKind.Object) {
        element.EnumerateObject().ToList().ForEach(el => GetNameFromProperty(el, parentName));
      } else {
        this.resources.TryAdd(parentName, element.GetString());
      }
    }

    private void FillResourceCache(Stream stream) {
      this.GetNameFromElement(JsonSerializer.Deserialize<JsonElement>(stream));
    }
  }
}