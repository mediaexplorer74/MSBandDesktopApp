// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.WorkoutPlanWeek
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Bing.HealthAndFitness
{
  [DataContract]
  public class WorkoutPlanWeek : DeserializableObjectBase
  {
    [DataMember(Name = "daylist")]
    public IList<WorkoutPlanDay> Days { get; set; }

    [DataMember(Name = "id")]
    public string Id { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsNotNull("id", (object) this.Id);
      JsonAssert.PropertyIsNotNull("daylist", (object) this.Days);
    }
  }
}
