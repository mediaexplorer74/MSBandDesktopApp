// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Coaching.SleepDurationRecommendation
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Providers.Coaching
{
  public class SleepDurationRecommendation
  {
    public SleepDurationRecommendation(
      TimeSpan currentSleepDuration,
      TimeSpan recommendedSleepDuration,
      DateTimeOffset currentWakeUpTime,
      string planId,
      string sleepDurationGoalId,
      string wakeUpTimeGoalId,
      string sleepDurationGoalTemplateName,
      string wakeUpTimeGoalTemplateName)
    {
      this.CurrentSleepDuration = currentSleepDuration;
      this.RecommendedSleepDuration = recommendedSleepDuration;
      this.CurrentWakeUpTime = currentWakeUpTime;
      this.PlanId = planId;
      this.SleepDurationGoalId = sleepDurationGoalId;
      this.WakeUpTimeGoalId = wakeUpTimeGoalId;
      this.SleepDurationGoalTemplateName = sleepDurationGoalTemplateName;
      this.WakeUpTimeGoalTemplateName = wakeUpTimeGoalTemplateName;
    }

    public TimeSpan CurrentSleepDuration { get; }

    public TimeSpan RecommendedSleepDuration { get; }

    public DateTimeOffset CurrentWakeUpTime { get; }

    public string PlanId { get; }

    public string SleepDurationGoalId { get; }

    public string WakeUpTimeGoalId { get; }

    public string SleepDurationGoalTemplateName { get; }

    public string WakeUpTimeGoalTemplateName { get; }
  }
}
