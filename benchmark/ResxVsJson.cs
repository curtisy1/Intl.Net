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
  public string FirstEntryJson() => jsonManager.GetString("ivory");

  [Benchmark]
  public string FirstEntryResx() => resxManager.GetString("ivory");

  [Benchmark]
  public string LastEntryJson() => jsonManager.GetString("gold");

  [Benchmark]
  public string LastEntryResx() => resxManager.GetString("gold");
  
  [Benchmark]
  public string MiddleEntryJson() => jsonManager.GetString("withdrawal");

  [Benchmark]
  public string MiddleEntryResx() => resxManager.GetString("withdrawal");
  
  [Benchmark]
  public (string, string) SameEntryTwiceJson() => (jsonManager.GetString("ivory"), jsonManager.GetString("ivory"));

  [Benchmark]
  public (string, string) SameEntryTwiceResx() => (resxManager.GetString("ivory"), resxManager.GetString("ivory"));
}

public class Program {
  public static void Main(string[] args) {
    var summary = BenchmarkRunner.Run<ResxVsJson>();
  }
}