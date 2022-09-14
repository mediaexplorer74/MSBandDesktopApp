// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Coaching.ICoachingProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers.Coaching
{
  public interface ICoachingProvider
  {
    Task<Timeline> GetCoachingTimelineAsync(
      DateTimeOffset startDay,
      DateTimeOffset endDay,
      CancellationToken token);

    Task<IList<CoachingProgressSection>> GetProgressSectionsAsync(
      CancellationToken token);

    Task<bool> IsOnPlanAsync(CancellationToken token);

    Task<Reminder> GetReminderAsync(
      string habitId,
      int habitIdDuplicateIndex,
      DateTimeOffset day,
      Range<DateTimeOffset> timelineDayRange,
      CancellationToken token);

    Task SaveReminderAsync(
      string habitId,
      int habitIdDuplicateIndex,
      DateTimeOffset day,
      DateTimeOffset newStartTime,
      bool isReminderEnabled,
      Range<DateTimeOffset> timelineDayRange,
      CancellationToken token);

    Task SkipReminderAsync(
      string habitId,
      int habitIdDuplicateIndex,
      DateTimeOffset day,
      Range<DateTimeOffset> timelineDayRange,
      CancellationToken token);

    Task<SleepDurationRecommendation> GetSleepDurationRecommendationAsync(
      CancellationToken token);

    Task UpdateSleepPlanDurationGoalAsync(
      SleepDurationRecommendation recommendation,
      SleepPlanRecommendationOption chosenOption,
      string insightId,
      CancellationToken token);
  }
}
