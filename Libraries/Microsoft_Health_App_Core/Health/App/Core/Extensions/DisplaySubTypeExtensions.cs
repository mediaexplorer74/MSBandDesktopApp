// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.DisplaySubTypeExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.Cloud.Client;
using System.Diagnostics;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class DisplaySubTypeExtensions
  {
    public static string ToFriendlyString(this DisplaySubType type)
    {
      switch (type)
      {
        case DisplaySubType.Time:
          return AppResources.BrowseWorkoutFilterTypeTime;
        case DisplaySubType.Task:
          return AppResources.BrowseWorkoutFilterTypeTask;
        case DisplaySubType.Running:
          return AppResources.BrowseWorkoutFilterTypeRunning;
        case DisplaySubType.Bodyweight:
          return AppResources.BrowseWorkoutFilterTypeBodyweight;
        case DisplaySubType.Strength:
          return AppResources.BrowseWorkoutFilterTypeStrength;
        case DisplaySubType.Running_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeRunningOneOff;
        case DisplaySubType.Running_MultiWeek:
          return AppResources.BrowseWorkoutFilterTypeRunningMultiWeek;
        case DisplaySubType.Bodyweight_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeBodyweightOneOff;
        case DisplaySubType.Bodyweight_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeBodyweightMultiWeek;
        case DisplaySubType.Strength_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeStrengthOneOff;
        case DisplaySubType.Strength_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeStrengthMultiWeek;
        case DisplaySubType.Biking:
          return AppResources.BrowseWorkoutFilterTypeBiking;
        case DisplaySubType.Biking_OneOff:
          return AppResources.BrowseWorkoutFilterTypeBikingOneOff;
        case DisplaySubType.Biking_MultiWeek:
          return AppResources.BrowseWorkoutFilterTypeBikingMultiWeek;
        case DisplaySubType.Hiking:
          return AppResources.BrowseWorkoutFilterTypeHiking;
        case DisplaySubType.Hiking_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeHikingOneOff;
        case DisplaySubType.Hiking_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeHikingMultiWeek;
        case DisplaySubType.Swimming:
          return AppResources.BrowseWorkoutFilterTypeSwimming;
        case DisplaySubType.Swimming_OneOff:
          return AppResources.BrowseWorkoutFilterTypeSwimmingOneOff;
        case DisplaySubType.Swimming_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeSwimmingMultiWeek;
        case DisplaySubType.Golfing:
          return AppResources.BrowseWorkoutFilterTypeGolfing;
        case DisplaySubType.Golfing_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeGolfingOneOff;
        case DisplaySubType.Golfing_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeGolfingMultiWeek;
        case DisplaySubType.Skiing:
          return AppResources.BrowseWorkoutFilterTypeSkiing;
        case DisplaySubType.Skiing_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeSkiingOneOff;
        case DisplaySubType.Skiing_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeSkiingMultiWeek;
        case DisplaySubType.Sleeping:
          return AppResources.BrowseWorkoutFilterTypeSleeping;
        case DisplaySubType.Sleeping_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeSleepingOneOff;
        case DisplaySubType.Sleeping_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeSleepingMultiWeek;
        case DisplaySubType.Yoga:
          return AppResources.BrowseWorkoutFilterTypeYoga;
        case DisplaySubType.Yoga_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeYogaOneOff;
        case DisplaySubType.Yoga_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeYogaMultiWeek;
        case DisplaySubType.Pilates:
          return AppResources.BrowseWorkoutFilterTypePilates;
        case DisplaySubType.Pilates_Oneoff:
          return AppResources.BrowseWorkoutFilterTypePilatesOneOff;
        case DisplaySubType.Pilates_Multiweek:
          return AppResources.BrowseWorkoutFilterTypePilatesMultiWeek;
        case DisplaySubType.Tennis:
          return AppResources.BrowseWorkoutFilterTypeTennis;
        case DisplaySubType.Tennis_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeTennisOneOff;
        case DisplaySubType.Tennis_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeTennisMultiWeek;
        case DisplaySubType.Triathlon:
          return AppResources.BrowseWorkoutFilterTypeTriathlon;
        case DisplaySubType.Triathlon_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeTriathlonOneOff;
        case DisplaySubType.Triathlon_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeTriathlonMultiWeek;
        case DisplaySubType.Soccer:
          return AppResources.BrowseWorkoutFilterTypeSoccer;
        case DisplaySubType.Soccer_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeSoccerOneOff;
        case DisplaySubType.Soccer_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeSoccerMultiWeek;
        case DisplaySubType.Football:
          return AppResources.BrowseWorkoutFilterTypeFootball;
        case DisplaySubType.Football_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeFootballOneOff;
        case DisplaySubType.Football_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeFootballMultiWeek;
        case DisplaySubType.Baseball:
          return AppResources.BrowseWorkoutFilterTypeBaseball;
        case DisplaySubType.Baseball_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeBaseballOneOff;
        case DisplaySubType.Baseball_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeBaseballMultiWeek;
        case DisplaySubType.Basketball:
          return AppResources.BrowseWorkoutFilterTypeBasketball;
        case DisplaySubType.Basketball_Oneoff:
          return AppResources.BrowseWorkoutFilterTypeBasketballOneOff;
        case DisplaySubType.Basketball_Multiweek:
          return AppResources.BrowseWorkoutFilterTypeBasketballMultiWeek;
        default:
          Debugger.Break();
          return type.ToString();
      }
    }
  }
}
