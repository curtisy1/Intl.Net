namespace Localizati18n.ResourceGenerator.Plugins {
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.CodeAnalysis;

  public abstract class PluginBase : IPlugin {
    public virtual Task<IEnumerable<string>> FetchResources(GeneratorExecutionContext context) {
      return Task.FromResult(Enumerable.Empty<string>());
    }
  }
}