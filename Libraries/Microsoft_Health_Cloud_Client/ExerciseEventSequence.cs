// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.ExerciseEventSequence
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class ExerciseEventSequence : UserEventSequence
  {
    private HRZones hrZones;

    [DataMember]
    public double CaloriesFromCarbs { get; set; }

    [DataMember]
    public double CaloriesFromFat { get; set; }

    [DataMember]
    public double RelaxationPercentage { get; set; }

    [DataMember]
    public double RelaxationTime { get; set; }

    [DataMember]
    public double StressPercentage { get; set; }

    [DataMember]
    public double StressTime { get; set; }

    [DataMember]
    public double AverageVO2 { get; set; }

    [DataMember]
    public double TrainingEffect { get; set; }

    [DataMember]
    public double HealthIndex { get; set; }

    [DataMember]
    private string HRZoneClassification { get; set; }

    public HRZones HRZones
    {
      get => this.hrZones;
      set
      {
        this.hrZones = value;
        this.HRZoneClassification = value != null ? JsonUtilities.SerializeObject((object) value) : (string) null;
      }
    }

    protected override void OnDeserialized(StreamingContext context)
    {
      this.hrZones = this.HRZoneClassification != null ? JsonUtilities.DeserializeObject<HRZones>(this.HRZoneClassification) : (HRZones) null;
      base.OnDeserialized(context);
    }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsGreaterOrEqualToZero("CaloriesFromCarbs", this.CaloriesFromCarbs);
      JsonAssert.PropertyIsGreaterOrEqualToZero("CaloriesFromFat", this.CaloriesFromFat);
      JsonAssert.PropertyIsGreaterOrEqualToZero("AverageVO2", this.AverageVO2);
      JsonAssert.PropertyIsWithinRange("HealthIndex", this.HealthIndex, 0.0, 100.0);
      JsonAssert.PropertyIsWithinRange("RelaxationPercentage", this.RelaxationPercentage, 0.0, 100.0);
      JsonAssert.PropertyIsGreaterOrEqualToZero("RelaxationTime", this.RelaxationTime);
      JsonAssert.PropertyIsWithinRange("StressPercentage", this.StressPercentage, 0.0, 100.0);
      JsonAssert.PropertyIsGreaterOrEqualToZero("StressTime", this.StressTime);
      JsonAssert.PropertyIsWithinRange("TrainingEffect", this.TrainingEffect, 0.0, 5.0);
    }
  }
}
