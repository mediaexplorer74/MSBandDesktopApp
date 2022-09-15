// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.WorkoutSearchResult
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Bing.HealthAndFitness
{
  [DataContract]
  public class WorkoutSearchResult : DeserializableObjectBase
  {
    [DataMember(Name = "kprtnrlogourl")]
    public string PartnerLogo { get; set; }

    [DataMember(Name = "kprtnrname")]
    public string PartnerName { get; set; }

    [DataMember(Name = "bdyparts")]
    public string BodyParts { get; set; }

    [DataMember(Name = "bingimageid")]
    public string BingImageId { get; set; }

    [DataMember(Name = "duration")]
    public int Duration { get; set; }

    [DataMember(Name = "fttype")]
    public string FitnessType { get; set; }

    [DataMember(Name = "gender")]
    public string Gender { get; set; }

    [DataMember(Name = "id")]
    public string Id { get; set; }

    [DataMember(Name = "image")]
    public string Image { get; set; }

    [DataMember(IsRequired = false, Name = "iscustomworkout")]
    public bool IsCustomWorkout { get; set; }

    [DataMember(Name = "level")]
    public string Level { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(IsRequired = false, Name = "publishdate")]
    public DateTimeOffset PublishDate { get; set; }

    [DataMember(Name = "scenario")]
    public string Scenario { get; set; }

    [DataMember(Name = "type")]
    public string Type { get; set; }

    [DataMember(Name = "kdisplaysubtype")]
    public DisplaySubType DisplaySubType { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsNotNull("id", (object) this.Id);
      JsonAssert.PropertyIsNotNull("WorkoutSearchResult:name", (object) this.Name);
      JsonAssert.PropertyIsNotNull("duration", (object) this.Duration);
      JsonAssert.PropertyIsNotNull("level", (object) this.Level);
    }
  }
}
