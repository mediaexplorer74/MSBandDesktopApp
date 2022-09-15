// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Calories.CaloriesWeekViewModel
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

namespace Microsoft.Health.App.Core.ViewModels.Calories
{
  [PageTaxonomy(new string[] {"Fitness", "Calories", "Week Details"})]
  public class CaloriesWeekViewModel : PanelViewModelBase, IStatWeekViewModel
  {
    private readonly IDateTimeService dateTimeService;
    private readonly IGoalsProvider goalsProvider;
    private readonly IUserDailySummaryProvider userDailySummaryProvider;
    private IList<ChartDateSeriesInfo> chartSeriesInformation = (IList<ChartDateSeriesInfo>) new List<ChartDateSeriesInfo>();
    private LoadState trackerLoadState;
    private UserDailySummaryGroup userActivityGroup;
    private DateTimeOffset weekStartDay;

    public CaloriesWeekViewModel(
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

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      string input = (string) null;
      CaloriesWeekViewModel caloriesWeekViewModel;
      if (parameters != null && parameters.TryGetValue("StartDate", out input))
      {
        this.weekStartDay = DateTimeOffset.Parse(input);
        caloriesWeekViewModel = this;
        UserDailySummaryGroup userActivityGroup = caloriesWeekViewModel.userActivityGroup;
        UserDailySummaryGroup summaryWeekGroupAsync = await this.userDailySummaryProvider.GetUserDailySummaryWeekGroupAsync(this.weekStartDay);
        caloriesWeekViewModel.userActivityGroup = summaryWeekGroupAsync;
        caloriesWeekViewModel = (CaloriesWeekViewModel) null;
      }
      else
      {
        this.weekStartDay = this.dateTimeService.GetToday();
        caloriesWeekViewModel = this;
        UserDailySummaryGroup userActivityGroup = caloriesWeekViewModel.userActivityGroup;
        UserDailySummaryGroup summaryWeekGroupAsync = await this.userDailySummaryProvider.GetUserDailySummaryWeekGroupAsync(this.dateTimeService.GetWeekThroughToday().Low);
        caloriesWeekViewModel.userActivityGroup = summaryWeekGroupAsync;
        caloriesWeekViewModel = (CaloriesWeekViewModel) null;
      }
      this.TrackerLoadState = LoadState.Loading;
      double totalBurnedDouble = 0.0;
      string mostBurnedDay = string.Empty;
      this.chartSeriesInformation = (IList<ChartDateSeriesInfo>) new List<ChartDateSeriesInfo>();
      if (this.userActivityGroup == null || this.userActivityGroup.CaloriesSamples.Count == 0)
      {
        this.chartSeriesInformation.Clear();
      }
      else
      {
        mostBurnedDay = Formatter.FormatTrackerWeekday(this.userActivityGroup.CaloriesSamples[0].DateTimeOffset);
        double num = this.userActivityGroup.CaloriesSamples[0].Value;
        foreach (Sample caloriesSample in (IEnumerable<Sample>) this.userActivityGroup.CaloriesSamples)
        {
          totalBurnedDouble += caloriesSample.Value;
          if (caloriesSample.Value > num)
          {
            num = caloriesSample.Value;
            mostBurnedDay = Formatter.FormatTrackerWeekday(caloriesSample.DateTimeOffset);
          }
        }
        if (num == 0.0)
          mostBurnedDay = AppResources.NotAvailable;
        ChartDateSeriesInfo calSeries = new ChartDateSeriesInfo();
        calSeries.Name = "Calories";
        calSeries.SeriesType = ChartSeriesType.WeeklyCalories;
        calSeries.Interval = TimeSpan.FromDays(1.0);
        calSeries.SeriesData = (IList<DateChartPoint>) this.userActivityGroup.CaloriesSamples.Where<Sample>((Func<Sample, bool>) (s => s.DateTimeOffset < this.dateTimeService.AddWeeks(this.weekStartDay, 1))).Select<Sample, DateChartPoint>((Func<Sample, DateChartPoint>) (s =>
        {
          return new DateChartPoint()
          {
            Date = this.dateTimeService.ToLocalTime(s.DateTimeOffset).Date,
            Value = s.Value
          };
        })).ToList<DateChartPoint>();
        calSeries.Selected = true;
        foreach (DateChartPoint point in (IEnumerable<DateChartPoint>) calSeries.SeriesData)
        {
          UsersGoal goalExpandedAsync = await this.goalsProvider.GetGoalExpandedAsync(GoalType.CalorieGoal);
          if (goalExpandedAsync != null && goalExpandedAsync.GetRoundedPercentCompletedOn((DateTimeOffset) point.Date, (long) point.Value) >= 100L)
            point.Classification = DateChartValueClassification.Highlight;
        }
        this.chartSeriesInformation.Add(calSeries);
        calSeries = (ChartDateSeriesInfo) null;
      }
      this.LoadStats(totalBurnedDouble, mostBurnedDay);
      this.RaisePropertyChanged("ChartSeriesInformation");
      this.TrackerLoadState = LoadState.Loaded;
    }

    private void LoadStats(double totalBurnedDouble, string mostBurnedDay)
    {
      this.Stats.Clear();
      int num = this.userActivityGroup == null || this.userActivityGroup.CaloriesSamples.Count == 0 ? 0 : (int) totalBurnedDouble;
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.TrackerTotalBurned,
        Glyph = "\uE009",
        Value = (object) num,
        ValueType = StatValueType.Integer
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.TrackerMostBurned,
        Glyph = "\uE068",
        Value = (object) mostBurnedDay,
        ValueType = StatValueType.Text
      });
    }
  }
}
