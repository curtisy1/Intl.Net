namespace Localizati18n.ResourceManager {
  using System;
  using System.Collections.Concurrent;
  using System.Globalization;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Resources;

  public class JsonResourceManager : ResourceManager {
    private const string jsonExtension = ".json";
    private readonly ConcurrentDictionary<string, JsonResourceSet> resourceSetCache = new();

    public string FileName { get; }
    
    private string? ResourceDirectory => this.MainAssembly?.Location.Split("/").SkipLast(1).Aggregate((a, b) => $"{a}/{b}");

    public JsonResourceManager(string baseName, Assembly assembly, string fileName)
      : base(baseName, assembly) {
      this.FileName = fileName;
    }

    public override Type ResourceSetType => typeof(JsonResourceSet);

    public override string? GetString(string name) => this.GetString(name, null);

    public override string? GetString(string name, CultureInfo? culture) {
      var currentCulture = culture ?? CultureInfo.CurrentUICulture;
      if (this.resourceSetCache.TryGetValue(currentCulture.Name, out var resourceSet)) {
        return resourceSet.GetString(name);
      }

      resourceSet = new JsonResourceSet(this.GetResourceFileName(currentCulture));
      this.resourceSetCache.TryAdd(currentCulture.Name, resourceSet);

      return resourceSet.GetString(name);
    }

    protected override string GetResourceFileName(CultureInfo culture) {
      var parentCulture = culture;

      while (!string.IsNullOrEmpty(parentCulture.Name)) {
        var possibleFileName = $"{this.ResourceDirectory}/{this.FileName}.{parentCulture.Name}{jsonExtension}";
        if (File.Exists(possibleFileName)) {
          return possibleFileName;
        }
        
        parentCulture = parentCulture.Parent;
      }

      return $"{this.ResourceDirectory}/{this.FileName}{jsonExtension}";
    }
  }
}