namespace Intl.Net.ResourceManager {
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Resources;

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
      var resourceContentPlaceholder = string.Empty;
      var previousPlaceholder = string.Empty;
      var depth = 0;
      using var reader = new StreamReader(stream);
      var line = reader.ReadLine();
      while (!string.IsNullOrEmpty(line)) {
        var kvp = line.Split(":");
        var key = kvp[0].Trim().Replace("\"", "");

        switch (key) {
          // first line is the JSON initializer, skip this one
          case "{":
            line = reader.ReadLine();
            continue;
          case "}":
            line = reader.ReadLine();
            depth--;
            continue;
        }
        
        // first character of key was a colon, in this case, add it back and append the second entry
        if (string.IsNullOrEmpty(key)) {
          key = ":" + kvp[1].Trim().Replace("\"", "");
        }
        
        // we have a composite object. Combine it
        if (line.EndsWith('{')) {
          if (depth > 0) {
            previousPlaceholder = resourceContentPlaceholder;
          }
          
          depth++;
          resourceContentPlaceholder += key + ".";
        } else if (!line.EndsWith("},")) {
          key = resourceContentPlaceholder + key;
          var value = string.Join("", kvp.Skip(1)).Trim().Replace("\"", "");
          this.resources.TryAdd(key, value[..^1]);
        } else {
          depth = depth != 0 ? depth - 1 : 0;
          resourceContentPlaceholder = depth < 1 ? string.Empty : previousPlaceholder;
        }
        
        line = reader.ReadLine();
      }
    }
  }
}