// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.WorkoutPlanDetail
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Bing.HealthAndFitness
{
  [DataContract]
  public class WorkoutPlanDetail : DeserializableObjectBase
  {
    [DataMember(Name = "bdyprts")]
    public IList<string> BodyParts { get; set; }

    [DataMember(Name = "desc")]
    public string Description { get; set; }

    [DataMember(Name = "displaytype")]
    public string DisplayType { get; set; }

    [DataMember(Name = "dtls")]
    public IList<WorkoutDetailItem> Details { get; set; }

    [DataMember(Name = "dur")]
    public int Duration { get; set; }

    [DataMember(Name = "focus")]
    public IList<string> Focus { get; set; }

    [DataMember(Name = "gender")]
    public string Gender { get; set; }

    [DataMember(Name = "goal")]
    public IList<string> Goals { get; set; }

    [DataMember(Name = "how")]
    public string HowTo { get; set; }

    [DataMember(Name = "id")]
    public string WorkoutPlanId { get; set; }

    [DataMember(Name = "imgeattrib")]
    public string ImageAttributes { get; set; }

    [DataMember(Name = "lvl")]
    public string Level { get; set; }

    [DataMember(Name = "mkt")]
    public string Market { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "path")]
    public string Image { get; set; }

    [DataMember(Name = "rltd")]
    public IList<RelatedWorkoutPlan> Related { get; set; }

    [DataMember(Name = "type")]
    public string Type { get; set; }

    [DataMember(Name = "wklist")]
    public IList<WorkoutPlanWeek> Weeks { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsNotNull("id", (object) this.WorkoutPlanId);
      JsonAssert.PropertyIsNotNull("WorkoutDetails:name", (object) this.Name);
      JsonAssert.PropertyIsNotNull("wklist", (object) this.Weeks);
      JsonAssert.PropertyIsNotNull("dtls", (object) this.Details);
    }
  }
}
