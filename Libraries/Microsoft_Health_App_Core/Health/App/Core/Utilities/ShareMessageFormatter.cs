// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.ShareMessageFormatter
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Providers.Golf.Rounds;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using Microsoft.Health.Cloud.Client.Events.Golf;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class ShareMessageFormatter
  {
    public static string FormatBikeMessage(BikeEvent model, IFormattingService formattingService)
    {
      int num = model.GpsState == GpsState.AtLeastOnePoint ? 1 : 0;
      return string.Format(AppResources.ShareBikeMessage, new object[2]
      {
        (object) Formatter.GetMonthDayString(model.StartTime),
        (object) (num == 0 ? (string) Formatter.FormatTimeSpan(model.Duration, Formatter.TimeSpanFormat.Abbreviated, false) : (string) formattingService.FormatDistance(new Length?(model.TotalDistance), appendUnit: true))
      });
    }

    public static string FormatRunMessage(RunEvent model, IFormattingService formattingService)
    {
      string str = (string) formattingService.FormatDistance(new Length?(model.TotalDistance), appendUnit: true);
      return string.Format(AppResources.ShareRunMessage, new object[2]
      {
        (object) Formatter.GetMonthDayString(model.StartTime),
        (object) str
      });
    }

    public static string FormatExerciseMessage(ExerciseEvent model)
    {
      string str1 = (string) Formatter.FormatCalories(new int?(model.CaloriesBurned), true);
      string str2 = (string) Formatter.FormatTimeSpan(model.Duration, Formatter.TimeSpanFormat.Abbreviated, false);
      return string.Format(AppResources.ShareExerciseMessage, new object[3]
      {
        (object) Formatter.GetMonthDayString(model.StartTime),
        (object) str1,
        (object) str2
      });
    }

    public static string FormatGolfMessage(
      GolfEvent model,
      GolfRound round,
      IFormattingService formattingService)
    {
      return string.Format(AppResources.ShareGolfMessage, new object[3]
      {
        (object) Formatter.GetMonthDayString(model.StartTime),
        (object) formattingService.FormatGolfScore(round.TotalScore, round.TotalDifferenceFromPar),
        (object) round.CourseName
      });
    }

    public static string FormatWorkoutMessage(WorkoutEvent model, WorkoutPlanDetail workoutPlan)
    {
      string str1 = (string) Formatter.FormatCalories(new int?(model.CaloriesBurned), true);
      string str2 = (string) Formatter.FormatTimeSpan(model.Duration, Formatter.TimeSpanFormat.Abbreviated, false);
      return string.Format(AppResources.ShareGuidedWorkoutMessage, (object) Formatter.GetMonthDayString(model.StartTime), (object) str1, (object) str2, (object) workoutPlan.Name);
    }

    public static string FormatSleepMessage(SleepEvent model, IFormattingService formattingService) => string.Format(AppResources.ShareSleepMessage, new object[2]
    {
      (object) Formatter.GetMonthDayString(model.StartTime),
      (object) model.SleepEfficiencyPercentage
    });

    public static string FormatHikeMessage(HikeEvent model, IFormattingService formattingService) => string.Format(AppResources.ShareHikeMessage, new object[2]
    {
      (object) Formatter.GetMonthDayString(model.StartTime),
      (object) formattingService.FormatCaloriesValue(model.CaloriesBurned)
    });
  }
}
