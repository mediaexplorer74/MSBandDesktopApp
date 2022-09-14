// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Steps.StepsDayViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.ViewModels.Goals;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Steps
{
  [PageTaxonomy(new string[] {"Fitness", "Steps", "Day Details"})]
  public class StepsDayViewModel : PanelViewModelBase, IStatDayViewModel
  {
    private static readonly ILog Log = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Steps\\StepsDayViewModel.cs");
    private readonly IDateTimeService dateTimeService;
    private readonly IUserDailySummaryProvider userDailySummaryProvider;
    private readonly IGoalsProvider goalsProvider;
    private readonly IHealthCloudClient healthCloudClient;
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService smoothNavService;
    private IList<ChartDateSeriesInfo> chartSeriesInformation = (IList<ChartDateSeriesInfo>) new List<ChartDateSeriesInfo>();
    private DateTimeOffset day;
    private bool editButtonVisibility;
    private string goalPercentComplete;
    private long goalValue;
    private UserActivityGroup hourlyUserActivityGroup;
    private LoadState trackerLoadState;
    private UserDailySummaryGroup userDailySummaryGroup;
    private HealthCommand editGoalsCommand;

    public StepsDayViewModel(
      IHealthCloudClient healthCloudClient,
      IGoalsProvider goalsProvider,
      ISmoothNavService smoothNavService,
      INetworkService networkService,
      IMessageSender messageSender,
      IDateTimeService dateTimeService,
      IUserDailySummaryProvider userDailySummaryProvider)
      : base(networkService)
    {
      if (goalsProvider == null)
        throw new ArgumentNullException(nameof (goalsProvider));
      if (smoothNavService == null)
        throw new ArgumentNullException(nameof (smoothNavService));
      if (messageSender == null)
        throw new ArgumentNullException(nameof (messageSender));
      if (dateTimeService == null)
        throw new ArgumentNullException(nameof (dateTimeService));
      if (userDailySummaryProvider == null)
        throw new ArgumentNullException(nameof (userDailySummaryProvider));
      this.healthCloudClient = healthCloudClient;
      this.goalsProvider = goalsProvider;
      this.smoothNavService = smoothNavService;
      this.messageSender = messageSender;
      this.dateTimeService = dateTimeService;
      this.userDailySummaryProvider = userDailySummaryProvider;
    }

    public string DeviceId { get; set; }

    public IList<StatViewModel> Stats { get; } = (IList<StatViewModel>) new ObservableCollection<StatViewModel>();

    public bool EditButtonVisibility
    {
      get => this.editButtonVisibility;
      set
      {
        this.SetProperty<bool>(ref this.editButtonVisibility, value, nameof (EditButtonVisibility));
        this.editGoalsCommand?.RaiseCanExecuteChanged();
      }
    }

    public long GoalValue
    {
      get => this.goalValue;
      set => this.SetProperty<long>(ref this.goalValue, value, nameof (GoalValue));
    }

    public string GoalPercentComplete
    {
      get => this.goalPercentComplete;
      set => this.SetProperty<string>(ref this.goalPercentComplete, value, nameof (GoalPercentComplete));
    }

    public bool IsToday
    {
      get
      {
        DateTimeOffset dateTimeOffset1 = new DateTimeOffset(this.day.Year, this.day.Month, this.day.Day, 0, 0, 0, TimeSpan.Zero);
        DateTimeOffset dateTimeOffset2 = new DateTimeOffset(dateTimeOffset1.DateTime.AddDays(1.0));
        return dateTimeOffset1 <= this.dateTimeService.Now && this.dateTimeService.Now < dateTimeOffset2;
      }
    }

    public IList<ChartDateSeriesInfo> ChartSeriesInformation => this.chartSeriesInformation;

    public DateTime XAxisMinimum => this.day.LocalDateTime;

    public DateTime XAxisMaximum => this.dateTimeService.AddDays(this.day, 1).LocalDateTime;

    public LoadState TrackerLoadState
    {
      get => this.trackerLoadState;
      set => this.SetProperty<LoadState>(ref this.trackerLoadState, value, nameof (TrackerLoadState));
    }

    public ICommand EditGoalsCommand => (ICommand) this.editGoalsCommand ?? (ICommand) (this.editGoalsCommand = new HealthCommand((Action) (() =>
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary.Add("type", 1.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      this.messageSender.Register<GoalsChangedMessage>((object) this, new Action<GoalsChangedMessage>(this.OnGoalsChanged));
      this.smoothNavService.Navigate(typeof (EditGoalsViewModel), (IDictionary<string, string>) dictionary);
    }), (Func<bool>) (() => this.EditButtonVisibility)));

    private async void OnGoalsChanged(GoalsChangedMessage message)
    {
      this.messageSender.Unregister<GoalsChangedMessage>((object) this, new Action<GoalsChangedMessage>(this.OnGoalsChanged));
      try
      {
        this.SetGoalValues(await this.goalsProvider.GetGoalExpandedAsync(GoalType.StepGoal));
      }
      catch (Exception ex)
      {
        StepsDayViewModel.Log.ErrorAndDebug(ex, "Failed to update goals on GoalsChangedMessage.");
      }
    }

    private void SetGoalValues(UsersGoal goal)
    {
      double val1 = 0.0;
      if (this.userDailySummaryGroup != null && goal != null)
      {
        val1 = (double) goal.GetRoundedPercentCompletedOn((DateTimeOffset) this.day.Date, (long) this.userDailySummaryGroup.TotalStepsTaken);
        this.GoalValue = (long) Math.Min(val1, 100.0);
      }
      else
        this.GoalValue = 0L;
      this.GoalPercentComplete = string.Format("{0:P0}", new object[1]
      {
        (object) (val1 / 100.0)
      });
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.TrackerLoadState = LoadState.Loading;
      Task<UserDailySummaryGroup> getTodaysUserActivityInfoTask;
      Task<IList<UserActivity>> getHourlyUserActivityInfoTask;
      if (parameters != null && parameters.ContainsKey("Date"))
      {
        this.day = DateTimeOffset.Parse(parameters["Date"]);
        getHourlyUserActivityInfoTask = this.healthCloudClient.GetUserActivitiesAsync(this.dateTimeService.GetDateThroughDay(this.day), ActivityPeriod.Hour, this.DeviceId);
        getTodaysUserActivityInfoTask = this.userDailySummaryProvider.GetUserDailySummaryGroupAsync(this.day, this.DeviceId);
      }
      else
      {
        this.day = this.dateTimeService.GetToday();
        getHourlyUserActivityInfoTask = this.healthCloudClient.GetUserActivitiesAsync(this.dateTimeService.GetTodayToTomorrow(), ActivityPeriod.Hour, this.DeviceId);
        getTodaysUserActivityInfoTask = this.userDailySummaryProvider.GetUserDailySummaryGroupAsync(this.dateTimeService.GetToday(), this.DeviceId);
      }
      Task<UsersGoal> getGoalExpandedTask = this.goalsProvider.GetGoalExpandedAsync(GoalType.StepGoal);
      await Task.WhenAll((Task) getTodaysUserActivityInfoTask, (Task) getHourlyUserActivityInfoTask, (Task) getGoalExpandedTask);
      this.userDailySummaryGroup = getTodaysUserActivityInfoTask.Result;
      this.hourlyUserActivityGroup = getHourlyUserActivityInfoTask.Result.ToUserActivityGroup(ActivityPeriod.Hour);
      UsersGoal result = getGoalExpandedTask.Result;
      this.SetGoalValues(result);
      this.EditButtonVisibility = this.day.Date.Equals(DateTime.Now.Date);
      this.chartSeriesInformation = (IList<ChartDateSeriesInfo>) new List<ChartDateSeriesInfo>();
      if (this.hourlyUserActivityGroup == null || this.hourlyUserActivityGroup.StepsSamples == null || this.hourlyUserActivityGroup.StepsSamples.Count == 1)
      {
        this.chartSeriesInformation.Clear();
      }
      else
      {
        ChartDateSeriesInfo chartDateSeriesInfo1 = new ChartDateSeriesInfo();
        chartDateSeriesInfo1.Name = "Steps";
        chartDateSeriesInfo1.SeriesType = ChartSeriesType.Steps;
        chartDateSeriesInfo1.Interval = TimeSpan.FromHours(1.0);
        chartDateSeriesInfo1.GoalValue = result.Value;
        chartDateSeriesInfo1.SeriesData = (IList<DateChartPoint>) this.hourlyUserActivityGroup.StepsSamples.ToHourlyDateChartPoints().EnsurePointExistsForInterval(this.dateTimeService.GetDateThroughDay(this.day), TimeSpan.FromHours(1.0)).ToList<DateChartPoint>();
        chartDateSeriesInfo1.Selected = true;
        foreach (DateChartPoint dateChartPoint in chartDateSeriesInfo1.SeriesData.Where<DateChartPoint>((Func<DateChartPoint, bool>) (d => d.Value > 600.0)))
          dateChartPoint.Classification = DateChartValueClassification.AboveAverage;
        this.chartSeriesInformation.Add(chartDateSeriesInfo1);
        if (this.hourlyUserActivityGroup.HeartRateSamples != null)
        {
          ChartDateSeriesInfo chartDateSeriesInfo2 = new ChartDateSeriesInfo();
          chartDateSeriesInfo2.Name = "HeartRate";
          chartDateSeriesInfo2.SeriesType = ChartSeriesType.HeartRate;
          chartDateSeriesInfo2.Interval = TimeSpan.FromHours(1.0);
          chartDateSeriesInfo2.SeriesData = this.hourlyUserActivityGroup.HeartRateSamples.ToHourlyDateChartPoints();
          chartDateSeriesInfo2.ShowAverageLine = false;
          chartDateSeriesInfo2.ShowAverageMarker = false;
          chartDateSeriesInfo2.ShowHighMarker = true;
          chartDateSeriesInfo2.ShowLowMarker = false;
          chartDateSeriesInfo2.SkipZeroes = true;
          this.chartSeriesInformation.Add(chartDateSeriesInfo2);
        }
      }
      this.LoadStats();
      this.RaisePropertyChanged("ChartSeriesInformation");
      this.RaisePropertyChanged("XAxisMinimum");
      this.RaisePropertyChanged("XAxisMaximum");
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
      statViewModel2.Label = AppResources.TrackerFlightsClimbed;
      statViewModel2.Glyph = "\uE181";
      UserDailySummaryGroup dailySummaryGroup2 = this.userDailySummaryGroup;
      statViewModel2.Value = (object) (dailySummaryGroup2 != null ? dailySummaryGroup2.TotalFlightsClimbed : 0);
      statViewModel2.ValueType = StatValueType.Integer;
      stats2.Add(statViewModel2);
      IList<StatViewModel> stats3 = this.Stats;
      StatViewModel statViewModel3 = new StatViewModel();
      statViewModel3.Label = AppResources.PanelStatisticLabelUvExposure;
      statViewModel3.Glyph = "\uE091";
      UserDailySummaryGroup dailySummaryGroup3 = this.userDailySummaryGroup;
      statViewModel3.Value = (object) (dailySummaryGroup3 != null ? dailySummaryGroup3.TotalUvExposure : TimeSpan.Zero);
      statViewModel3.ValueType = StatValueType.DurationWithTextWithoutSeconds;
      stats3.Add(statViewModel3);
      IList<StatViewModel> stats4 = this.Stats;
      StatViewModel statViewModel4 = new StatViewModel();
      statViewModel4.Label = AppResources.PanelStatisticLabelCardioMinutes;
      statViewModel4.Glyph = "\uE025";
      UserDailySummaryGroup dailySummaryGroup4 = this.userDailySummaryGroup;
      statViewModel4.Value = (object) (dailySummaryGroup4 != null ? new int?(dailySummaryGroup4.TotalCardioScore / 60) : new int?());
      statViewModel4.ValueType = StatValueType.Integer;
      statViewModel4.ShowNotAvailableOnZero = false;
      SubStatViewModel subStatViewModel = new SubStatViewModel();
      subStatViewModel.Label = AppResources.CoachingPlanActivitySubmetricTwoTitle;
      UserDailySummaryGroup dailySummaryGroup5 = this.userDailySummaryGroup;
      subStatViewModel.Value = (object) (dailySummaryGroup5 != null ? new int?(dailySummaryGroup5.TotalCardioBonus / 60) : new int?());
      subStatViewModel.ValueType = SubStatValueType.CardioBonusMinutes;
      statViewModel4.SubStat1 = subStatViewModel;
      stats4.Add(statViewModel4);
    }
  }
}
