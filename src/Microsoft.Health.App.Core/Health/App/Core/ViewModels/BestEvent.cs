// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.BestEvent
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class BestEvent
  {
    private readonly IFormattingService formatter;
    private readonly IUserProfileService userProfileService;
    private string label;

    public BestEvent(
      IFormattingService formatter,
      IUserProfileService userProfileService,
      UsersGoal userGoal)
    {
      if (formatter == null)
        throw new ArgumentNullException(nameof (formatter));
      if (userProfileService == null)
        throw new ArgumentNullException(nameof (userProfileService));
      if (userGoal == null)
        throw new ArgumentNullException(nameof (userGoal));
      this.formatter = formatter;
      this.userProfileService = userProfileService;
      this.Id = userGoal.Id;
      this.Label = userGoal.Name;
      this.Type = userGoal.Type;
      this.Category = userGoal.Category;
      if (userGoal.ValueHistory != null)
      {
        GoalValueHistory goalValueHistory = userGoal.ValueHistory.FirstOrDefault<GoalValueHistory>();
        if (goalValueHistory != null && goalValueHistory.HistoryRecords != null && goalValueHistory.HistoryRecords.FirstOrDefault<GoalValueRecord>() != null)
        {
          GoalValueRecord goalValueRecord = goalValueHistory.HistoryRecords.Last<GoalValueRecord>();
          string[] strArray = goalValueRecord.Extension.Split(":".ToCharArray());
          this.StartTime = goalValueRecord.UpdateTime;
          this.BestValue = goalValueRecord.Value;
          this.EventId = ((IEnumerable<string>) strArray).Last<string>().Split("_".ToCharArray())[0];
        }
      }
      else if (userGoal.ValueSummary != null && userGoal.ValueSummary.FirstOrDefault<GoalValueSummary>() != null && userGoal.ValueSummary.FirstOrDefault<GoalValueSummary>().CurrentValue != null)
      {
        GoalValueRecord currentValue;
        if (userGoal.ValueSummary.Count<GoalValueSummary>() == 2 && userGoal.ValueSummary.ElementAt<GoalValueSummary>(1) != null && userGoal.ValueSummary.ElementAt<GoalValueSummary>(1).CurrentValue != null)
        {
          switch (this.userProfileService.DistanceUnitType)
          {
            case DistanceUnitType.Imperial:
              currentValue = userGoal.ValueSummary.ElementAt<GoalValueSummary>(1).CurrentValue;
              break;
            case DistanceUnitType.Metric:
              currentValue = userGoal.ValueSummary.ElementAt<GoalValueSummary>(0).CurrentValue;
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
        else
          currentValue = userGoal.ValueSummary.First<GoalValueSummary>().CurrentValue;
        string str = ((IEnumerable<string>) currentValue.Extension.Split(":".ToCharArray())).Last<string>().Split("_".ToCharArray())[0];
        this.StartTime = currentValue.UpdateTime;
        this.BestValue = currentValue.Value;
        this.EventId = str;
      }
      this.Reason = BestEventReasonUtilities.GetReason(userGoal.Name);
    }

    public string Id { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public string StartTimeStr => this.BestValue == null || (string) this.BestValueSpan == AppResources.NotAvailable ? string.Empty : Formatter.GetMonthDayString(this.StartTime);

    public BestEventReason Reason { get; set; }

    public string Glyph => BestEventReasonUtilities.GetGlyph(this.Reason);

    public string ReasonStr => BestEventReasonUtilities.GetFullText(this.Reason);

    public string ReasonBannerText => BestEventReasonUtilities.GetPhraseText(this.Reason);

    public string Label
    {
      get => !string.IsNullOrEmpty(this.label) ? this.label.ToLower() : this.label;
      set => this.label = value;
    }

    public GoalType Type { get; set; }

    public GoalCategory Category { get; set; }

    public object BestValue { get; set; }

    public StyledSpan BestValueSpan
    {
      get
      {
        if (this.BestValue == null)
          return Formatter.NotAvailableStyledSpan;
        switch (this.Reason)
        {
          case BestEventReason.FastestPaceRun:
            return this.formatter.FormatPace(new Speed?(Speed.FromMillisecondsPerKilometer((double) int.Parse(this.BestValue.ToString()))));
          case BestEventReason.FurthestRun:
          case BestEventReason.FurthestRide:
          case BestEventReason.FurthestHike:
            return this.formatter.FormatDistance(new Length?(Length.FromCentimeters((double) int.Parse(this.BestValue.ToString()))), appendUnit: true);
          case BestEventReason.MostCaloriesRun:
          case BestEventReason.MostCaloriesHike:
            return Formatter.FormatCalories(new int?(int.Parse(this.BestValue.ToString())), true);
          case BestEventReason.FastestSplitRun:
            return this.formatter.FormatPace(new Speed?(Speed.FromMillisecondsPerKilometer((double) int.Parse(this.BestValue.ToString()))));
          case BestEventReason.LargestElevationGainRide:
          case BestEventReason.LargestElevationGainHike:
            Length length = Length.FromCentimeters((double) int.Parse(this.BestValue.ToString()));
            return length <= Length.Zero ? Formatter.NotAvailableStyledSpan : this.formatter.FormatElevation(new Length?(length), appendUnit: true);
          case BestEventReason.MostCaloriesRide:
          case BestEventReason.MostCaloriesWorkout:
            return Formatter.FormatCalories(new int?(int.Parse(this.BestValue.ToString())), true);
          case BestEventReason.FastestSpeedRide:
            return this.formatter.FormatSpeed(new Speed?(Speed.FromCentimetersPerSecond((double) int.Parse(this.BestValue.ToString()))), appendUnit: true);
          case BestEventReason.LongestDurationWorkout:
          case BestEventReason.LongestDurationHike:
            TimeSpan time = TimeSpan.FromSeconds((double) long.Parse(this.BestValue.ToString()));
            bool flag = time.TotalHours >= 1.0;
            return Formatter.FormatTimeSpan(time, Formatter.TimeSpanFormat.NoText, !flag);
          default:
            return new StyledSpan(string.Empty, new object[0]);
        }
      }
    }

    public string EventId { get; set; }

    public bool IsGoodGoal => this.Label != null && this.Type != GoalType.Unknown && this.Type != GoalType.GolfGoal && (uint) this.Category > 0U;
  }
}
