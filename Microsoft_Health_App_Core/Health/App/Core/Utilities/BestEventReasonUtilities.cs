// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.BestEventReasonUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.ViewModels;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class BestEventReasonUtilities
  {
    private static Dictionary<string, BestEventReason> cloudKeyMap = new Dictionary<string, BestEventReason>();
    private static Dictionary<BestEventReason, BestEventReasonInformation> map = new Dictionary<BestEventReason, BestEventReasonInformation>();

    static BestEventReasonUtilities()
    {
      BestEventReasonUtilities.AddEventReason(BestEventReason.FastestPaceRun, "fastest run pace", AppResources.BestGoalPace, AppResources.AveragePace);
      BestEventReasonUtilities.AddEventReason(BestEventReason.FurthestRun, "furthest run", AppResources.BestGoalFurthestRun, AppResources.BestGoalFurthestRun);
      BestEventReasonUtilities.AddEventReason(BestEventReason.FastestSplitRun, "fastest run split", AppResources.BestGoalSplit, AppResources.Split);
      BestEventReasonUtilities.AddEventReason(BestEventReason.MostCaloriesRun, "most calories burned run", AppResources.BestGoalBurn, AppResources.CaloriesBurned);
      BestEventReasonUtilities.AddEventReason(BestEventReason.LargestElevationGainRide, "largest elevation gain ride", AppResources.BestGoalElevationGain, AppResources.ElevationGain);
      BestEventReasonUtilities.AddEventReason(BestEventReason.FurthestRide, "furthest ride", AppResources.BestGoalFurthestRide, AppResources.BestGoalFurthestRide);
      BestEventReasonUtilities.AddEventReason(BestEventReason.MostCaloriesRide, "most calories burned ride", AppResources.BestGoalBurn, AppResources.CaloriesBurned);
      BestEventReasonUtilities.AddEventReason(BestEventReason.FastestSpeedRide, "fastest speed ride", AppResources.BestGoalAverageSpeed, AppResources.Speed);
      BestEventReasonUtilities.AddEventReason(BestEventReason.LongestDurationWorkout, "longest duration exercise", AppResources.BestGoalDuration, AppResources.BestGoalDuration);
      BestEventReasonUtilities.AddEventReason(BestEventReason.MostCaloriesWorkout, "most calories burned exercise", AppResources.BestGoalBurn, AppResources.CaloriesBurned);
      BestEventReasonUtilities.AddEventReason(BestEventReason.FurthestHike, "furthest hike", AppResources.BestGoalFurthestHike, AppResources.BestGoalFurthestHike);
      BestEventReasonUtilities.AddEventReason(BestEventReason.MostCaloriesHike, "most calories burned hike", AppResources.BestGoalBurn, AppResources.CaloriesBurned);
      BestEventReasonUtilities.AddEventReason(BestEventReason.LargestElevationGainHike, "largest elevation gain hike", AppResources.BestGoalElevationGain, AppResources.ElevationGain);
      BestEventReasonUtilities.AddEventReason(BestEventReason.LongestDurationHike, "longest duration hike", AppResources.BestGoalDuration, AppResources.BestGoalDuration);
    }

    public static string GetGlyph(BestEventReason reason)
    {
      switch (reason)
      {
        case BestEventReason.FastestPaceRun:
          return "\uE029";
        case BestEventReason.FurthestRun:
        case BestEventReason.FurthestRide:
        case BestEventReason.FurthestHike:
          return "\uE030";
        case BestEventReason.MostCaloriesRun:
        case BestEventReason.MostCaloriesRide:
        case BestEventReason.MostCaloriesWorkout:
        case BestEventReason.MostCaloriesHike:
          return "\uE009";
        case BestEventReason.FastestSplitRun:
          return "\uE048";
        case BestEventReason.LargestElevationGainRide:
        case BestEventReason.LargestElevationGainHike:
          return "\uE103";
        case BestEventReason.FastestSpeedRide:
          return "\uE137";
        case BestEventReason.LongestDurationWorkout:
        case BestEventReason.LongestDurationHike:
          return "\uE024";
        default:
          throw new NotImplementedException(string.Format("No glyph found for reason: {0}", new object[1]
          {
            (object) reason
          }));
      }
    }

    public static string GetFullText(BestEventReason reason)
    {
      BestEventReasonUtilities.CheckReason(reason);
      return BestEventReasonUtilities.map[reason].FullText;
    }

    public static string GetPhraseText(BestEventReason reason)
    {
      BestEventReasonUtilities.CheckReason(reason);
      return BestEventReasonUtilities.map[reason].PhraseText;
    }

    public static BestEventReason GetReason(string cloudKey)
    {
      cloudKey = cloudKey.ToLowerInvariant();
      return !BestEventReasonUtilities.cloudKeyMap.ContainsKey(cloudKey) ? BestEventReason.Unknown : BestEventReasonUtilities.cloudKeyMap[cloudKey];
    }

    private static void CheckReason(BestEventReason reason)
    {
      if (!BestEventReasonUtilities.map.ContainsKey(reason))
        throw new ArgumentException("reason is invalid: " + (object) reason, nameof (reason));
    }

    private static void AddEventReason(
      BestEventReason reason,
      string cloudKey,
      string fullText,
      string phraseText)
    {
      BestEventReasonInformation reasonInformation = new BestEventReasonInformation()
      {
        CloudKey = cloudKey,
        FullText = fullText,
        PhraseText = phraseText,
        Reason = reason
      };
      BestEventReasonUtilities.cloudKeyMap.Add(cloudKey, reason);
      BestEventReasonUtilities.map.Add(reason, reasonInformation);
    }
  }
}
