// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.UserDailySummary
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class UserDailySummary : DeserializableObjectBase
  {
    [DataMember]
    public DateTimeOffset TimeOfDay { get; set; }

    [DataMember]
    public DayClassification DayClassification { get; set; }

    [DataMember]
    public ActivityLevel ActivityLevel { get; set; }

    [DataMember]
    public int StepsTaken { get; set; }

    [DataMember]
    public int CaloriesBurned { get; set; }

    [DataMember]
    public int UvExposure { get; set; }

    [DataMember]
    public string Location { get; set; }

    [DataMember]
    public int PeakHeartRate { get; set; }

    [DataMember]
    public int LowestHeartRate { get; set; }

    [DataMember]
    public int AverageHeartRate { get; set; }

    [DataMember]
    public HRZones DailyHeartRateZones { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length TotalDistance { get; set; }

    [DataMember]
    [JsonConverter(typeof (CentimetersToLengthConverter))]
    public Length TotalDistanceOnFoot { get; set; }

    [DataMember]
    public int NumberOfActiveHours { get; set; }

    [DataMember]
    public int FloorsClimbed { get; set; }

    [DataMember]
    public int CardioScore { get; set; }

    [DataMember]
    public int IntenseCardioSeconds { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsGreaterOrEqualToZero("AverageHeartRate", this.AverageHeartRate);
      JsonAssert.PropertyIsGreaterOrEqualToZero("CaloriesBurned", this.CaloriesBurned);
      JsonAssert.PropertyIsGreaterOrEqualToZero("LowestHeartRate", this.LowestHeartRate);
      JsonAssert.PropertyIsGreaterOrEqualToZero("PeakHeartRate", this.PeakHeartRate);
      JsonAssert.PropertyIsGreaterOrEqualToZero("StepsTaken", this.StepsTaken);
      JsonAssert.PropertyIsGreaterOrEqualToZero("TotalDistance", this.TotalDistance);
      JsonAssert.PropertyIsGreaterOrEqualToZero("UvExposure", this.UvExposure);
      JsonAssert.PropertyIsGreaterOrEqualToZero("FloorsClimbed", this.FloorsClimbed);
      JsonAssert.DateTimePropertyIsSet("TimeOfDay", this.TimeOfDay);
      JsonAssert.PropertyIsGreaterOrEqualToZero("IntenseCardioSeconds", this.IntenseCardioSeconds);
      JsonAssert.PropertyIsGreaterOrEqualToZero("CardioScore", this.CardioScore);
    }
  }
}
