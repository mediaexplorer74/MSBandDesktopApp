// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.WeeklyActivityProgressSummary
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class WeeklyActivityProgressSummary
  {
    public WeeklyActivityProgressSummary() => this.DailyProgressSummaries = (IList<DailyActivityProgressSummary>) new List<DailyActivityProgressSummary>();

    [DataMember]
    public DateTimeOffset TimeOfWeek { get; set; }

    [DataMember]
    public DateTimeOffset NextTimeOfWeek { get; set; }

    [DataMember]
    public IList<DailyActivityProgressSummary> DailyProgressSummaries { get; private set; }

    [DataMember]
    public int? CardioScore { get; set; }

    [DataMember]
    public int? AverageDailySteps { get; set; }

    [DataMember]
    public int? StepsTarget { get; set; }

    [DataMember]
    public int? CardioScoreTarget { get; set; }

    [DataMember]
    public int? StrengthWorkout { get; set; }

    [DataMember]
    public int? StrengthWorkoutTarget { get; set; }

    [DataMember]
    public int? DailyStepGoalMet { get; set; }

    [DataMember]
    public int? LowCardio { get; set; }

    [DataMember]
    public int? MediumCardio { get; set; }

    [DataMember]
    public int? IntenseCardio { get; set; }

    [DataMember]
    public int? StrengthWorkoutsCalories { get; set; }
  }
}
