// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.UserActivityExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.Cloud.Client;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class UserActivityExtensions
  {
    public static UserActivityGroup ToUserActivityGroup(
      this IList<UserActivity> userActivities,
      ActivityPeriod activityPeriod)
    {
      UserActivityGroup userActivityGroup = new UserActivityGroup()
      {
        UserActivities = userActivities,
        ActivityPeriod = activityPeriod,
        StepsSamples = (IList<Sample>) new List<Sample>(),
        CaloriesSamples = (IList<Sample>) new List<Sample>(),
        HeartRateSamples = (IList<Sample>) new List<Sample>(),
        UvExposureSamples = (IList<Sample>) new List<Sample>()
      };
      foreach (UserActivity userActivity in (IEnumerable<UserActivity>) userActivities)
      {
        userActivityGroup.StepsSamples.Add(new Sample(userActivity.TimeOfDay, (double) userActivity.StepsTaken));
        userActivityGroup.CaloriesSamples.Add(new Sample(userActivity.TimeOfDay, (double) userActivity.CaloriesBurned));
        userActivityGroup.HeartRateSamples.Add(new Sample(userActivity.TimeOfDay, (double) userActivity.AverageHeartRate));
        userActivityGroup.UvExposureSamples.Add(new Sample(userActivity.TimeOfDay, (double) userActivity.UvExposure));
      }
      return userActivityGroup;
    }
  }
}
