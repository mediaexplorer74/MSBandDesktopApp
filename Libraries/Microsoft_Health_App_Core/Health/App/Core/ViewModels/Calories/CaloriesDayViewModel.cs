// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Calories.CaloriesDayViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
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

namespace Microsoft.Health.App.Core.ViewModels.Calories
{
  [PageTaxonomy(new string[] {"Fitness", "Calories", "Day Details"})]
  public class CaloriesDayViewModel : PanelViewModelBase, IStatDayViewModel
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Calories\\CaloriesDayViewModel.cs");
    private readonly IDateTimeService dateTimeService;
    private readonly IGoalsProvider goalsProvider;
    private readonly IHealthCloudClient healthCloudClient;
    private readonly IMessageSender messageSender;
    private readonly ISmoothNavService smoothNavService;
    private readonly IUserDailySummaryProvider userDailySummaryProvider;
    private readonly IUserProfileService userProfileService;
    private readonly HealthCommand editGoalsCommand;
    private IList<ChartDateSeriesInfo> chartSeriesInformation = (IList<ChartDateSeriesInfo>) new List<ChartDateSeriesInfo>();
    private DateTimeOffset day;
    private bool editButtonVisibility;
    private string goalPercentComplete;
    private long goalValue;
    private UserActivityGroup hourlyUserActivityGroup;
    private LoadState trackerLoadState;
    private UserDailySummaryGroup userDailySummaryGroup;

    public CaloriesDayViewModel(
      IHealthCloudClient healthCloudClient,
      IGoalsProvider goalsProvider,
      ISmoothNavService smoothNavService,
      INetworkService networkService,
      IUserProfileService userProfileService,
      IMessageSender messageSender,
      IDateTimeService dateTimeService,
      IUserDailySummaryProvider userDailySummaryProvider)
      : base(networkService)
    {
      if (healthCloudClient == null)
        throw new ArgumentNullException(nameof (healthCloudClient));
      if (goalsProvider == null)
        throw new ArgumentNullException(nameof (goalsProvider));
      if (smoothNavService == null)
        throw new ArgumentNullException(nameof (smoothNavService));
      if (userProfileService == null)
        throw new ArgumentNullException(nameof (userProfileService));
      if (messageSender == null)
        throw new ArgumentNullException(nameof (messageSender));
      if (dateTimeService == null)
        throw new ArgumentNullException(nameof (dateTimeService));
      if (userDailySummaryProvider == null)
        throw new ArgumentNullException(nameof (userDailySummaryProvider));
      this.healthCloudClient = healthCloudClient;
      this.smoothNavService = smoothNavService;
      this.userProfileService = userProfileService;
      this.messageSender = messageSender;
      this.dateTimeService = dateTimeService;
      this.userDailySummaryProvider = userDailySummaryProvider;
      this.goalsProvider = goalsProvider;
      this.editGoalsCommand = new HealthCommand(new Action(this.EditGoals), (Func<bool>) (() => this.EditButtonVisibility));
    }

    public ICommand EditGoalsCommand => (ICommand) this.editGoalsCommand;

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

    public IList<ChartDateSeriesInfo> ChartSeriesInformation
    {
      get => this.chartSeriesInformation;
      private set => this.SetProperty<IList<ChartDateSeriesInfo>>(ref this.chartSeriesInformation, value, nameof (ChartSeriesInformation));
    }

    public DateTime XAxisMinimum => this.day.LocalDateTime;

    public DateTime XAxisMaximum => this.dateTimeService.AddDays(this.day, 1).LocalDateTime;

    public LoadState TrackerLoadState
    {
      get => this.trackerLoadState;
      set => this.SetProperty<LoadState>(ref this.trackerLoadState, value, nameof (TrackerLoadState));
    }

    private void EditGoals()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary.Add("type", 2.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      this.messageSender.Register<GoalsChangedMessage>((object) this, new Action<GoalsChangedMessage>(this.OnGoalsChanged));
      this.smoothNavService.Navigate(typeof (EditGoalsViewModel), (IDictionary<string, string>) dictionary);
    }

    private async void OnGoalsChanged(GoalsChangedMessage obj)
    {
      this.messageSender.Unregister<GoalsChangedMessage>((object) this, new Action<GoalsChangedMessage>(this.OnGoalsChanged));
      try
      {
        UsersGoal goalExpandedAsync = await this.goalsProvider.GetGoalExpandedAsync(GoalType.CalorieGoal);
        this.SetGoalValues(goalExpandedAsync);
        this.PrepareDataForCharts(goalExpandedAsync);
      }
      catch (Exception ex)
      {
        CaloriesDayViewModel.Logger.Error(ex, "Failed to update goals on GoalsChangedMessage.");
      }
    }

    private void SetGoalValues(UsersGoal goal)
    {
      double val1 = 0.0;
      if (this.userDailySummaryGroup != null && goal != null)
      {
        val1 = (double) goal.GetRoundedPercentCompletedOn((DateTimeOffset) this.day.Date, (long) this.userDailySummaryGroup.TotalCaloriesBurned);
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
      Task<IList<UserActivity>> getHourlyUserActivityInfoTask;
      Task<UserDailySummaryGroup> getTodaysUserActivityInfoTask;
      if (parameters != null && parameters.ContainsKey("Date"))
      {
        this.day = DateTimeOffset.Parse(parameters["Date"]);
        getHourlyUserActivityInfoTask = this.healthCloudClient.GetUserActivitiesAsync(this.dateTimeService.GetDateThroughDay(this.day), ActivityPeriod.Hour);
        getTodaysUserActivityInfoTask = this.userDailySummaryProvider.GetUserDailySummaryGroupAsync(this.day);
      }
      else
      {
        this.day = this.dateTimeService.GetToday();
        getHourlyUserActivityInfoTask = this.healthCloudClient.GetUserActivitiesAsync(this.dateTimeService.GetTodayToTomorrow(), ActivityPeriod.Hour);
        getTodaysUserActivityInfoTask = this.userDailySummaryProvider.GetUserDailySummaryGroupAsync(this.dateTimeService.GetToday());
      }
      Task<UsersGoal> getGoalExpandedTask = this.goalsProvider.GetGoalExpandedAsync(GoalType.CalorieGoal);
      await Task.WhenAll((Task) getTodaysUserActivityInfoTask, (Task) getHourlyUserActivityInfoTask, (Task) getGoalExpandedTask);
      this.hourlyUserActivityGroup = getHourlyUserActivityInfoTask.Result.ToUserActivityGroup(ActivityPeriod.Hour);
      this.userDailySummaryGroup = getTodaysUserActivityInfoTask.Result;
      UsersGoal result = getGoalExpandedTask.Result;
      this.SetGoalValues(result);
      this.EditButtonVisibility = this.day.Date.Equals(DateTime.Now.Date);
      this.chartSeriesInformation = (IList<ChartDateSeriesInfo>) new List<ChartDateSeriesInfo>();
      DateTimeOffset highestBurnTime;
      DateTimeOffset lowestBurnTime;
      if (this.hourlyUserActivityGroup == null || this.hourlyUserActivityGroup.CaloriesSamples == null || this.hourlyUserActivityGroup.CaloriesSamples.Count == 1)
      {
        highestBurnTime = DateTimeOffset.MinValue;
        lowestBurnTime = DateTimeOffset.MinValue;
        this.chartSeriesInformation.Clear();
      }
      else
      {
        DateTimeOffset dateTimeOffset = DateTimeOffset.Now.RoundDown(TimeSpan.FromHours(1.0));
        double num1 = this.hourlyUserActivityGroup.CaloriesSamples[0].Value;
        double num2 = this.hourlyUserActivityGroup.CaloriesSamples[0].Value;
        lowestBurnTime = this.hourlyUserActivityGroup.CaloriesSamples[0].DateTimeOffset;
        highestBurnTime = this.hourlyUserActivityGroup.CaloriesSamples[0].DateTimeOffset;
        foreach (Sample caloriesSample in (IEnumerable<Sample>) this.hourlyUserActivityGroup.CaloriesSamples)
        {
          if (caloriesSample.DateTimeOffset < dateTimeOffset && caloriesSample.Value < num1)
          {
            num1 = caloriesSample.Value;
            lowestBurnTime = caloriesSample.DateTimeOffset;
          }
          if (caloriesSample.Value > num2)
          {
            num2 = caloriesSample.Value;
            highestBurnTime = caloriesSample.DateTimeOffset;
          }
        }
        this.PrepareDataForCharts(result);
      }
      this.LoadStats(lowestBurnTime, highestBurnTime);
      this.RaisePropertyChanged("XAxisMaximum");
      this.RaisePropertyChanged("XAxisMinimum");
      this.TrackerLoadState = LoadState.Loaded;
    }

    private void LoadStats(DateTimeOffset lowestBurnTime, DateTimeOffset highestBurnTime)
    {
      this.Stats.Clear();
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.TrackerHighestBurn,
        Glyph = "\uE068",
        Value = (object) highestBurnTime.ToLocalTime(),
        ValueType = StatValueType.Time
      });
      this.Stats.Add(new StatViewModel()
      {
        Label = AppResources.TrackerLowestBurn,
        Glyph = "\uE069",
        Value = (object) lowestBurnTime.ToLocalTime(),
        ValueType = StatValueType.Time
      });
    }

    private void PrepareDataForCharts(UsersGoal usersGoal)
    {
      List<ChartDateSeriesInfo> chartDateSeriesInfoList = new List<ChartDateSeriesInfo>();
      ChartDateSeriesInfo chartDateSeriesInfo1 = new ChartDateSeriesInfo();
      chartDateSeriesInfo1.Name = "Calories";
      chartDateSeriesInfo1.SeriesType = ChartSeriesType.Calories;
      chartDateSeriesInfo1.Interval = TimeSpan.FromHours(1.0);
      chartDateSeriesInfo1.SeriesData = (IList<DateChartPoint>) this.hourlyUserActivityGroup.CaloriesSamples.ToHourlyDateChartPoints().EnsurePointExistsForInterval(this.dateTimeService.GetDateThroughDay(this.day), TimeSpan.FromHours(1.0)).ToList<DateChartPoint>();
      chartDateSeriesInfo1.GoalValue = usersGoal.Value;
      chartDateSeriesInfo1.Selected = true;
      ChartDateSeriesInfo chartDateSeriesInfo2 = chartDateSeriesInfo1;
      BandUserProfile currentUserProfile = this.userProfileService.CurrentUserProfile;
      double num = (double) (10U * (currentUserProfile.Weight / 1000U)) + 6.25 * (double) ((int) currentUserProfile.HeightMM / 10) - (double) (5 * currentUserProfile.Birthdate.GetAge()) + (currentUserProfile.Gender == Gender.Male ? 5.0 : -161.0);
      double moderateActivity = num / 24.0 + num * 0.55 / 16.0;
      foreach (DateChartPoint dateChartPoint in chartDateSeriesInfo2.SeriesData.Where<DateChartPoint>((Func<DateChartPoint, bool>) (d => d.Value >= moderateActivity)))
        dateChartPoint.Classification = DateChartValueClassification.AboveAverage;
      chartDateSeriesInfoList.Add(chartDateSeriesInfo2);
      if (this.hourlyUserActivityGroup.HeartRateSamples != null)
      {
        ChartDateSeriesInfo chartDateSeriesInfo3 = new ChartDateSeriesInfo();
        chartDateSeriesInfo3.Name = "HeartRate";
        chartDateSeriesInfo3.SeriesType = ChartSeriesType.HeartRate;
        chartDateSeriesInfo3.Interval = TimeSpan.FromHours(1.0);
        chartDateSeriesInfo3.SeriesData = this.hourlyUserActivityGroup.HeartRateSamples.ToHourlyDateChartPoints();
        chartDateSeriesInfo3.ShowAverageLine = false;
        chartDateSeriesInfo3.ShowAverageMarker = false;
        chartDateSeriesInfo3.ShowHighMarker = true;
        chartDateSeriesInfo3.ShowLowMarker = false;
        chartDateSeriesInfo3.SkipZeroes = true;
        ChartDateSeriesInfo chartDateSeriesInfo4 = chartDateSeriesInfo3;
        chartDateSeriesInfoList.Add(chartDateSeriesInfo4);
      }
      this.ChartSeriesInformation = (IList<ChartDateSeriesInfo>) chartDateSeriesInfoList;
    }
  }
}
