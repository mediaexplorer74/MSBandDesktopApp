// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.WorkoutSearchResults
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Bing.HealthAndFitness
{
  [DataContract]
  public class WorkoutSearchResults : DeserializableObjectBase
  {
    public WorkoutSearchResults() => this.Results = (IList<WorkoutSearchResult>) new List<WorkoutSearchResult>();

    [DataMember(Name = "count")]
    public int Count { get; set; }

    [DataMember(Name = "results")]
    public IList<WorkoutSearchResult> Results { get; private set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsNotNull("results", (object) this.Results);
    }
  }
}
