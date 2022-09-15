// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.UserDailySummaryExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class UserDailySummaryExtensions
  {
    public static UserDailySummaryGroup ToUserDailySummaryGroup(
      this IList<UserDailySummary> userDailySummaries,
      Range<DateTimeOffset> range)
    {
      UserDailySummaryGroup dailySummaryGroup = new UserDailySummaryGroup()
      {
        UserDailySummaries = userDailySummaries,
        StepsSamples = (IList<Sample>) new List<Sample>(),
        CaloriesSamples = (IList<Sample>) new List<Sample>(),
        HeartRateSamples = (IList<Sample>) new List<Sample>(),
        TotalActiveHours = TimeSpan.Zero,
        TotalUvExposure = TimeSpan.Zero,
        RequestRange = range
      };
      if (userDailySummaries != null && userDailySummaries.Count > 0)
      {
        int num = 0;
        foreach (UserDailySummary userDailySummary in (IEnumerable<UserDailySummary>) userDailySummaries)
        {
          dailySummaryGroup.TotalDistance += userDailySummary.TotalDistance;
          dailySummaryGroup.TotalDistanceOnFoot += userDailySummary.TotalDistanceOnFoot;
          dailySummaryGroup.TotalStepsTaken += userDailySummary.StepsTaken;
          dailySummaryGroup.TotalCaloriesBurned += userDailySummary.CaloriesBurned;
          dailySummaryGroup.TotalActiveHours += TimeSpan.FromHours((double) userDailySummary.NumberOfActiveHours);
          num += userDailySummary.AverageHeartRate;
          dailySummaryGroup.StepsSamples.Add(new Sample(userDailySummary.TimeOfDay, (double) userDailySummary.StepsTaken));
          dailySummaryGroup.CaloriesSamples.Add(new Sample(userDailySummary.TimeOfDay, (double) userDailySummary.CaloriesBurned));
          dailySummaryGroup.HeartRateSamples.Add(new Sample(userDailySummary.TimeOfDay, (double) userDailySummary.AverageHeartRate));
          dailySummaryGroup.TotalFlightsClimbed += userDailySummary.FloorsClimbed;
          dailySummaryGroup.TotalUvExposure += TimeSpan.FromMinutes((double) userDailySummary.UvExposure);
          dailySummaryGroup.TotalCardioScore += userDailySummary.CardioScore;
          dailySummaryGroup.TotalCardioBonus += userDailySummary.IntenseCardioSeconds;
        }
        dailySummaryGroup.AverageHeartRate = num / userDailySummaries.Count;
      }
      return dailySummaryGroup;
    }
  }
}
