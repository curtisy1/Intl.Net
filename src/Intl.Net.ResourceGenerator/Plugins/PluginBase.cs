namespace Intl.Net.ResourceGenerator.Plugins {
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.CodeAnalysis;

  public abstract class PluginBase : IPlugin {
    protected string PluginName = "";
    public virtual Task<IEnumerable<string>> FetchResources(GeneratorExecutionContext context) {
      return Task.FromResult(Enumerable.Empty<string>());
    }

    protected virtual ConfigurableVariables GetConfiguredVariables(GeneratorExecutionContext context) {
      var hasEnabledValue = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue($"build_property.Enable{this.PluginName}Provider", out var enabled);
      var hasUri = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ResourceUri", out var uri);
      var hasPath = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.ProjectDir", out var downloadPath);

      return new ConfigurableVariables(
        hasEnabledValue && bool.Parse(enabled) && hasUri && hasPath,
        uri, 
        downloadPath);
    }
  }
}