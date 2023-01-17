namespace Localizati18n.ResourceGenerator.Plugins {

  using System.Collections.Generic;
  using System.Threading.Tasks;
  using Microsoft.CodeAnalysis;

  public interface IPlugin {
    Task<IEnumerable<string>> FetchResources(GeneratorExecutionContext context);
  }
}