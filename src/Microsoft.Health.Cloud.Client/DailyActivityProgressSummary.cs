// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.DailyActivityProgressSummary
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class DailyActivityProgressSummary
  {
    [DataMember]
    public DateTimeOffset TimeOfDay { get; set; }

    [DataMember]
    public DateTimeOffset NextTimeOfDay { get; set; }

    [DataMember]
    public int? StepsTaken { get; set; }

    [DataMember]
    public int? StepsTarget { get; set; }

    [DataMember]
    public bool? IsStepGoalMet { get; set; }

    [DataMember]
    public int? CardioScore { get; set; }

    [DataMember]
    public int? LowCardioMinutes { get; set; }

    [DataMember]
    public int? MediumCardioMinutes { get; set; }

    [DataMember]
    public int? IntenseCardioMinutes { get; set; }

    [DataMember]
    public int? StrengthWorkouts { get; set; }

    [DataMember]
    public int? StrengthWorkoutsCalories { get; set; }
  }
}
