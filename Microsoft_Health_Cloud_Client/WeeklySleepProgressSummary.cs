// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.WeeklySleepProgressSummary
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class WeeklySleepProgressSummary : DeserializableObjectBase
  {
    public WeeklySleepProgressSummary() => this.DailyProgressSummaries = (IList<DailySleepProgressSummary>) new List<DailySleepProgressSummary>();

    [DataMember]
    public DateTimeOffset TimeOfWeek { get; set; }

    [DataMember]
    public DateTimeOffset NextTimeOfWeek { get; set; }

    [DataMember]
    public IList<DailySleepProgressSummary> DailyProgressSummaries { get; private set; }

    [DataMember]
    public int? AverageActualSleepMinutes { get; set; }

    [DataMember]
    public int? ActualSleepDailyTarget { get; set; }

    [DataMember]
    public int? AverageSleepRestoration { get; set; }

    [DataMember]
    public int? AverageSleepEfficiencyPercentage { get; set; }

    [DataMember]
    public int? ActualSleepHoursGoalMet { get; set; }

    [DataMember]
    public MostEfficientSleeps MostEfficientSleeps { get; set; }

    [DataMember]
    public int? AverageBedTime { get; set; }

    [DataMember]
    public int? BedTimeGoalMet { get; set; }

    [DataMember]
    public int? AverageWakeupTime { get; set; }

    [DataMember]
    public int? WakeupTimeGoalMet { get; set; }

    [DataMember]
    public int? AverageOptimalSleepEfficiencyPercentage { get; set; }

    [DataMember]
    public int? AverageSleepConsistencyPercentage { get; set; }
  }
}
