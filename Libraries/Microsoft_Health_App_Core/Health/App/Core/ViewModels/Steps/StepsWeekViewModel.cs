// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Steps.StepsWeekViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Steps
{
  [PageTaxonomy(new string[] {"Fitness", "Steps", "Week Details"})]
  public class StepsWeekViewModel : PanelViewModelBase, IStatWeekViewModel
  {
    private readonly IDateTimeService dateTimeService;
    private readonly IGoalsProvider goalsProvider;
    private readonly IUserDailySummaryProvider userDailySummaryProvider;
    private IList<ChartDateSeriesInfo> chartSeriesInformation = (IList<ChartDateSeriesInfo>) new List<ChartDateSeriesInfo>();
    private DateTimeOffset mostActiveDay;
    private LoadState trackerLoadState;
    private UserDailySummaryGroup userDailySummaryGroup;
    private DateTimeOffset weekStartDay;

    public StepsWeekViewModel(
      IGoalsProvider goalsProvider,
      INetworkService networkService,
      IDateTimeService dateTimeService,
      IUserDailySummaryProvider userDailySummaryProvider)
      : base(networkService)
    {
      if (goalsProvider == null)
        throw new ArgumentNullException(nameof (goalsProvider));
      if (dateTimeService == null)
        throw new ArgumentNullException(nameof (dateTimeService));
      if (userDailySummaryProvider == null)
        throw new ArgumentNullException(nameof (userDailySummaryProvider));
      this.goalsProvider = goalsProvider;
      this.dateTimeService = dateTimeService;
      this.userDailySummaryProvider = userDailySummaryProvider;
    }

    public IList<StatViewModel> Stats { get; } = (IList<StatViewModel>) new ObservableCollection<StatViewModel>();

    public IList<ChartDateSeriesInfo> ChartSeriesInformation => this.chartSeriesInformation;

    public LoadState TrackerLoadState
    {
      get => this.trackerLoadState;
      set => this.SetProperty<LoadState>(ref this.trackerLoadState, value, nameof (TrackerLoadState));
    }

    public bool IsThisWeek
    {
      get
      {
        DateTimeOffset dateTimeOffset1 = new DateTimeOffset(this.weekStartDay.Year, this.weekStartDay.Month, this.weekStartDay.Day, 0, 0, 0, TimeSpan.Zero);
        DateTimeOffset dateTimeOffset2 = new DateTimeOffset(dateTimeOffset1.DateTime.AddDays(1.0));
        return dateTimeOffset1 <= this.dateTimeService.Now && this.dateTimeService.Now < dateTimeOffset2;
      }
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      string input = (string) null;
      StepsWeekViewModel stepsWeekViewModel;
      if (parameters != null && parameters.TryGetValue("StartDate", out input))
      {
        this.weekStartDay = DateTimeOffset.Parse(input);
        stepsWeekViewModel = this;
        UserDailySummaryGroup dailySummaryGroup = stepsWeekViewModel.userDailySummaryGroup;
        UserDailySummaryGroup summaryWeekGroupAsync = await this.userDailySummaryProvider.GetUserDailySummaryWeekGroupAsync(this.weekStartDay);
        stepsWeekViewModel.userDailySummaryGroup = summaryWeekGroupAsync;
        stepsWeekViewModel = (StepsWeekViewModel) null;
      }
      else
      {
        this.weekStartDay = this.dateTimeService.GetToday();
        stepsWeekViewModel = this;
        UserDailySummaryGroup dailySummaryGroup = stepsWeekViewModel.userDailySummaryGroup;
        UserDailySummaryGroup summaryWeekGroupAsync = await this.userDailySummaryProvider.GetUserDailySummaryWeekGroupAsync(this.dateTimeService.GetWeekThroughToday().Low);
        stepsWeekViewModel.userDailySummaryGroup = summaryWeekGroupAsync;
        stepsWeekViewModel = (StepsWeekViewModel) null;
      }
      this.TrackerLoadState = LoadState.Loading;
      this.chartSeriesInformation = (IList<ChartDateSeriesInfo>) new List<ChartDateSeriesInfo>();
      if (this.userDailySummaryGroup == null || this.userDailySummaryGroup.StepsSamples.Count == 0)
      {
        this.chartSeriesInformation.Clear();
      }
      else
      {
        this.mostActiveDay = DateTimeOffset.MinValue;
        double num = 0.0;
        foreach (Sample stepsSample in (IEnumerable<Sample>) this.userDailySummaryGroup.StepsSamples)
        {
          if (stepsSample.Value > num)
          {
            num = stepsSample.Value;
            this.mostActiveDay = stepsSample.DateTimeOffset;
          }
        }
        ChartDateSeriesInfo stepsSeries = new ChartDateSeriesInfo();
        stepsSeries.Name = "Steps";
        stepsSeries.SeriesType = ChartSeriesType.WeeklySteps;
        stepsSeries.Interval = TimeSpan.FromDays(1.0);
        stepsSeries.SeriesData = (IList<DateChartPoint>) this.userDailySummaryGroup.StepsSamples.Where<Sample>((Func<Sample, bool>) (s => s.DateTimeOffset < this.dateTimeService.AddWeeks(this.weekStartDay, 1))).Select<Sample, DateChartPoint>((Func<Sample, DateChartPoint>) (s =>
        {
          return new DateChartPoint()
          {
            Date = this.dateTimeService.ToLocalTime(s.DateTimeOffset).Date,
            Value = s.Value
          };
        })).ToList<DateChartPoint>();
        stepsSeries.Selected = true;
        foreach (DateChartPoint point in (IEnumerable<DateChartPoint>) stepsSeries.SeriesData)
        {
          UsersGoal goalExpandedAsync = await this.goalsProvider.GetGoalExpandedAsync(GoalType.StepGoal);
          if (goalExpandedAsync != null && goalExpandedAsync.GetRoundedPercentCompletedOn((DateTimeOffset) point.Date, (long) point.Value) >= 100L)
            point.Classification = DateChartValueClassification.Highlight;
        }
        this.chartSeriesInformation.Add(stepsSeries);
        stepsSeries = (ChartDateSeriesInfo) null;
      }
      this.LoadStats();
      this.RaisePropertyChanged("ChartSeriesInformation");
      this.TrackerLoadState = LoadState.Loaded;
    }

    private void LoadStats()
    {
      this.Stats.Clear();
      IList<StatViewModel> stats1 = this.Stats;
      StatViewModel statViewModel1 = new StatViewModel();
      statViewModel1.Label = AppResources.TrackerDistance;
      statViewModel1.Glyph = "\uE030";
      UserDailySummaryGroup dailySummaryGroup1 = this.userDailySummaryGroup;
      statViewModel1.Value = (object) (dailySummaryGroup1 != null ? dailySummaryGroup1.TotalDistanceOnFoot : Length.Zero);
      statViewModel1.ValueType = StatValueType.DistanceShort;
      stats1.Add(statViewModel1);
      IList<StatViewModel> stats2 = this.Stats;
      StatViewModel statViewModel2 = new StatViewModel();
      statViewModel2.Label = AppResources.TrackerTotalSteps;
      statViewModel2.Glyph = "\uE008";
      UserDailySummaryGroup dailySummaryGroup2 = this.userDailySummaryGroup;
      statViewModel2.Value = (object) (dailySummaryGroup2 != null ? dailySummaryGroup2.TotalStepsTaken : 0);
      statViewModel2.ValueType = StatValueType.Integer;
      statViewModel2.ShowNotAvailableOnZero = false;
      stats2.Add(statViewModel2);
      string str = this.userDailySummaryGroup == null || this.userDailySummaryGroup.StepsSamples.Count == 0 || this.mostActiveDay == DateTimeOffset.MinValue ? AppResources.NotAvailable : Formatter.FormatTrackerWeekday(this.mostActiveDay);
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.TrackerMostActive,
        Glyph = "\uE068",
        Value = (object) str,
        ValueType = StatValueType.Text
      });
      IList<StatViewModel> stats3 = this.Stats;
      StatViewModel statViewModel3 = new StatViewModel();
      statViewModel3.Label = AppResources.TrackerFlightsClimbed;
      statViewModel3.Glyph = "\uE181";
      UserDailySummaryGroup dailySummaryGroup3 = this.userDailySummaryGroup;
      statViewModel3.Value = (object) (dailySummaryGroup3 != null ? dailySummaryGroup3.TotalFlightsClimbed : 0);
      statViewModel3.ValueType = StatValueType.Integer;
      stats3.Add(statViewModel3);
      IList<StatViewModel> stats4 = this.Stats;
      StatViewModel statViewModel4 = new StatViewModel();
      statViewModel4.Label = AppResources.PanelStatisticLabelUvExposure;
      statViewModel4.Glyph = "\uE091";
      UserDailySummaryGroup dailySummaryGroup4 = this.userDailySummaryGroup;
      statViewModel4.Value = (object) (dailySummaryGroup4 != null ? dailySummaryGroup4.TotalUvExposure : TimeSpan.Zero);
      statViewModel4.ValueType = StatValueType.DurationWithTextWithoutSeconds;
      stats4.Add(statViewModel4);
      IList<StatViewModel> stats5 = this.Stats;
      StatViewModel statViewModel5 = new StatViewModel();
      statViewModel5.Label = AppResources.PanelStatisticLabelCardioMinutes;
      statViewModel5.Glyph = "\uE025";
      UserDailySummaryGroup dailySummaryGroup5 = this.userDailySummaryGroup;
      statViewModel5.Value = (object) (dailySummaryGroup5 != null ? dailySummaryGroup5.UserDailySummaries.Sum<UserDailySummary>((Func<UserDailySummary, int>) (s => s.CardioScore)) / 60 : 0);
      statViewModel5.ValueType = StatValueType.Integer;
      statViewModel5.ShowNotAvailableOnZero = false;
      SubStatViewModel subStatViewModel = new SubStatViewModel();
      subStatViewModel.Label = AppResources.CoachingPlanActivitySubmetricTwoTitle;
      UserDailySummaryGroup dailySummaryGroup6 = this.userDailySummaryGroup;
      subStatViewModel.Value = (object) (dailySummaryGroup6 != null ? new int?(dailySummaryGroup6.UserDailySummaries.Sum<UserDailySummary>((Func<UserDailySummary, int>) (s => s.IntenseCardioSeconds)) / 60) : new int?());
      subStatViewModel.ValueType = SubStatValueType.CardioBonusMinutes;
      statViewModel5.SubStat1 = subStatViewModel;
      stats5.Add(statViewModel5);
    }
  }
}
