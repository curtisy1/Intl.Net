namespace Intl.Net.Benchmark;

using System.Resources;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ResourceManager;

public class ResxVsJson {
  private readonly ResourceManager resxManager;

  private readonly JsonResourceManager jsonManager;

  public ResxVsJson() {
    this.resxManager = new ResourceManager("Intl.Net.Benchmark.bench", typeof(ResxVsJson).Assembly);
    this.jsonManager = new JsonResourceManager("bench", typeof(ResxVsJson).Assembly, "bench");
  }

  [Benchmark]
  public string FirstEntryJson() => jsonManager.GetString("calculatinggroupwarenetworkssystemsturquoise");

  [Benchmark]
  public string FirstEntryResx() => resxManager.GetString("calculatinggroupwarenetworkssystemsturquoise");

  [Benchmark]
  public string LastEntryJson() => jsonManager.GetString("FrozenErgonomicRubberPizzaglobalfirewall");

  [Benchmark]
  public string LastEntryResx() => resxManager.GetString("FrozenErgonomicRubberPizzaglobalfirewall");
  
  [Benchmark]
  public string MiddleEntryJson() => jsonManager.GetString("orchidcircuitbusAgentAutoLoanAccount");

  [Benchmark]
  public string MiddleEntryResx() => resxManager.GetString("orchidcircuitbusAgentAutoLoanAccount");
  
  [Benchmark]
  public string NonExistingEntryJson() => jsonManager.GetString("abcdefghijklmnopqrstuvwxyz");

  [Benchmark]
  public string NonExistingEntryResx() => resxManager.GetString("abcdefghijklmnopqrstuvwxyz");
  
  [Benchmark]
  public (string, string) SameEntryTwiceJson() => (jsonManager.GetString("calculatinggroupwarenetworkssystemsturquoise"), jsonManager.GetString("calculatinggroupwarenetworkssystemsturquoise"));

  [Benchmark]
  public (string, string) SameEntryTwiceResx() => (resxManager.GetString("calculatinggroupwarenetworkssystemsturquoise"), resxManager.GetString("calculatinggroupwarenetworkssystemsturquoise"));
}

public class Program {
  public static void Main(string[] args) {
    var summary = BenchmarkRunner.Run<ResxVsJson>();
  }
}