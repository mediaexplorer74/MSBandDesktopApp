// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.WorkoutSearchOptions
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.Cloud.Client.Bing.HealthAndFitness
{
  public sealed class WorkoutSearchOptions
  {
    private readonly IList<KeyValuePair<string, string>> filters;

    public WorkoutSearchOptions(IEnumerable<KeyValuePair<string, string>> filters = null)
    {
      this.filters = (IList<KeyValuePair<string, string>>) new List<KeyValuePair<string, string>>(filters ?? Enumerable.Empty<KeyValuePair<string, string>>());
      this.Cluster = "fitness";
      this.Types = "Strength";
      this.Scenario = "workout";
    }

    public string Cluster { get; set; }

    public string Goals { get; set; }

    public string Types { get; set; }

    public ICollection<KeyValuePair<string, string>> Filters => (ICollection<KeyValuePair<string, string>>) this.filters;

    public string Scenario { get; set; }

    public string Market { get; set; }

    public int? Count { get; set; }

    public string Version { get; set; }

    public string ClientVersion { get; set; }

    public string Query { get; set; }

    public bool? IsHealthClient { get; set; }

    public WorkoutPublisher? PublishedBy { get; set; }
  }
}
