// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.ExerciseEventBase
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
  public class ExerciseEventBase : UserEvent
  {
    [DataMember]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan PausedTime { get; set; }

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
    public string FitnessBenefitMsg { get; set; }

    [DataMember]
    public double HealthIndex { get; set; }

    [DataMember]
    public HRZones HeartRateZones { get; set; }

    [DataMember]
    public int FinishHeartRate { get; set; }

    [DataMember]
    public int RecoveryHeartRate1Minute { get; set; }

    [DataMember]
    public int RecoveryHeartRate2Minute { get; set; }

    [DataMember]
    [JsonConverter(typeof (SecondsToTimeSpanConverter))]
    public TimeSpan RecoveryTime { get; set; }

    public override bool IsComplete() => this.RecoveryTime != TimeSpan.Zero && this.RecoveryHeartRate1Minute != 0 && (uint) this.RecoveryHeartRate2Minute > 0U;
  }
}
