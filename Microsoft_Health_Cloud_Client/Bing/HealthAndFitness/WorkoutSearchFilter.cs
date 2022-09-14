// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.WorkoutSearchFilter
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Bing.HealthAndFitness
{
  [DataContract]
  public class WorkoutSearchFilter : DeserializableObjectBase
  {
    [DataMember(Name = "count")]
    public int Count { get; set; }

    [DataMember(Name = "id")]
    public string Id { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember]
    public string FilterName { get; set; }

    [DataMember]
    public string Image { get; set; }

    [DataMember]
    public string BackgroundImage { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsNotNull("id", (object) this.Id);
      JsonAssert.PropertyIsNotNull("WorkoutSearchFilter:name", (object) this.Name);
    }
  }
}
