// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.SleepEvent
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class SleepEvent : UserEvent
  {
    public SleepEvent() => this.SleepTimeline = (IList<SleepTypeClassification>) new List<SleepTypeClassification>();

    [DataMember]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan AwakeTime { get; set; }

    [DataMember]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan SleepTime { get; set; }

    [DataMember]
    public int NumberOfWakeups { get; set; }

    [DataMember]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan TimeToFallAsleep { get; set; }

    [DataMember]
    public double SleepEfficiencyPercentage { get; set; }

    [DataMember]
    public IList<SleepEventSequence> Sequences { get; set; }

    [DataMember]
    public int RestingHeartRate { get; set; }

    [DataMember]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan TotalRestfulSleep { get; set; }

    [DataMember]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan TotalRestlessSleep { get; set; }

    public IList<SleepTypeClassification> SleepTimeline { get; private set; }

    [DataMember]
    public DateTimeOffset? FallAsleepTime { get; set; }

    [DataMember]
    public DateTimeOffset? WakeUpTime { get; set; }

    [DataMember]
    public string SleepRestorationMsg { get; set; }

    [DataMember]
    public bool IsAutoDetected { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.DateTimePropertyIsSet("DayId", this.DayId);
      JsonAssert.PropertyIsGreaterOrEqualToZero("AwakeTime", this.AwakeTime);
      JsonAssert.PropertyIsGreaterOrEqualToZero("SleepTime", this.SleepTime);
      JsonAssert.PropertyIsGreaterOrEqualToZero("TotalRestfulSleep", this.TotalRestfulSleep);
      JsonAssert.PropertyIsGreaterOrEqualToZero("TotalRestlessSleep", this.TotalRestlessSleep);
      JsonAssert.PropertyIsGreaterOrEqualToZero("NumberOfWakeups", this.NumberOfWakeups);
      JsonAssert.PropertyIsGreaterOrEqualToZero("TimeToFallAsleep", this.TimeToFallAsleep);
      JsonAssert.PropertyIsWithinRange("SleepEfficiencyPercentage", this.SleepEfficiencyPercentage, 0.0, 100.0);
    }
  }
}
