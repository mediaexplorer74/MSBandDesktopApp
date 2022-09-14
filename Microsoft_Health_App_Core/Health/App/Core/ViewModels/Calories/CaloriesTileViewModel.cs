// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Calories.CaloriesTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Calories
{
  [PageTaxonomy(new string[] {"Calories"})]
  public class CaloriesTileViewModel : MetricTileViewModel
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Calories\\CaloriesTileViewModel.cs");
    private readonly IConfig config;
    private readonly IDateTimeService dateTimeService;
    private readonly object globalMessageSubscriber = new object();
    private readonly IGoalsProvider goalsProvider;
    private readonly IMessageSender messageSender;
    private readonly IServiceLocator serviceLocator;
    private readonly IUserDailySummaryProvider userDailySummaryProvider;
    private UserDailySummaryGroup userDailySummaryGroup;
    private UsersGoal usersGoal;

    public CaloriesTileViewModel(
      IGoalsProvider goalsProvider,
      IConfig config,
      INetworkService networkService,
      IServiceLocator serviceLocator,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      IDateTimeService dateTimeService,
      IUserDailySummaryProvider userDailySummaryProvider,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(networkService, smoothNavService, messageSender, firstTimeUse)
    {
      if (goalsProvider == null)
        throw new ArgumentNullException(nameof (goalsProvider));
      if (config == null)
        throw new ArgumentNullException(nameof (config));
      if (serviceLocator == null)
        throw new ArgumentNullException(nameof (serviceLocator));
      if (messageSender == null)
        throw new ArgumentNullException(nameof (messageSender));
      if (dateTimeService == null)
        throw new ArgumentNullException(nameof (dateTimeService));
      if (userDailySummaryProvider == null)
        throw new ArgumentNullException(nameof (userDailySummaryProvider));
      this.goalsProvider = goalsProvider;
      this.config = config;
      this.serviceLocator = serviceLocator;
      this.messageSender = messageSender;
      this.dateTimeService = dateTimeService;
      this.userDailySummaryProvider = userDailySummaryProvider;
      this.TileIcon = "\uE009";
      this.ColorLevel = TileColorLevel.Medium;
      this.Pivots.Add(new PivotDefinition(AppResources.PivotDaily, (object) this.serviceLocator.GetInstance<CaloriesDailyViewModel>()));
      this.Pivots.Add(new PivotDefinition(AppResources.PivotWeekly, (object) this.serviceLocator.GetInstance<CaloriesWeeklyViewModel>()));
      this.messageSender.Register<GoalsChangedMessage>(this.globalMessageSubscriber, (Action<GoalsChangedMessage>) (message =>
      {
        if (message.GoalType != GoalType.CalorieGoal || this.usersGoal == null || this.userDailySummaryGroup == null)
          return;
        this.UpdateSubheader((long) message.Value);
      }));
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      CaloriesTileViewModel.Logger.Debug((object) "<START> loading the calories tile");
      Task<UserDailySummaryGroup> getUserDailySummariesTask = this.userDailySummaryProvider.GetUserDailySummaryGroupAsync(this.dateTimeService.GetToday());
      Task<UsersGoal> goalTask = this.goalsProvider.GetGoalExpandedAsync(GoalType.CalorieGoal);
      await Task.WhenAll((Task) getUserDailySummariesTask, (Task) goalTask);
      this.userDailySummaryGroup = getUserDailySummariesTask.Result;
      this.usersGoal = goalTask.Result;
      if ((this.userDailySummaryGroup == null || this.userDailySummaryGroup.TotalCaloriesBurned == 0) && this.usersGoal == null)
      {
        CaloriesTileViewModel.Logger.Debug((object) "<END> loading the calories tile (No Data)");
        return false;
      }
      CaloriesTileViewModel.Logger.Debug((object) "<END> loading the calories tile (Loaded)");
      return true;
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      if (this.userDailySummaryGroup != null)
        this.Header = Formatter.FormatCalories(new int?(this.userDailySummaryGroup.TotalCaloriesBurned), true);
      if (this.usersGoal != null)
        this.UpdateSubheader(this.usersGoal.Value);
      this.CanOpen = true;
    }

    protected override async Task OnTransitionToNoDataStateAsync()
    {
      await base.OnTransitionToNoDataStateAsync();
      this.Subheader = AppResources.CaloriesTileNoDataMessage;
      this.ColorLevel = TileColorLevel.Medium;
    }

    private void UpdateSubheader(long goalValue)
    {
      if (this.dateTimeService.Now - this.config.LastCompletedFullOobe < TimeSpan.FromDays(1.0))
      {
        this.Subheader = string.Format(AppResources.CaloriesTileFirstUse, new object[1]
        {
          (object) Formatter.FormatCalories(new int?((int) goalValue))
        });
      }
      else
      {
        if (this.userDailySummaryGroup == null)
          return;
        double num = goalValue > 0L ? (double) this.userDailySummaryGroup.TotalCaloriesBurned / (double) goalValue * 100.0 : 0.0;
        if (num <= 95.0)
          this.Subheader = string.Format(AppResources.GoalsTilePercentComplete, new object[1]
          {
            (object) (int) num
          });
        else if (num < 100.0)
          this.Subheader = AppResources.GoalTileAlmostThereCalories;
        else if (num <= 102.0)
          this.Subheader = AppResources.GoalsReached;
        else
          this.Subheader = string.Format(AppResources.GoalsTileOverAchievementCalories, new object[1]
          {
            (object) ((long) this.userDailySummaryGroup.TotalCaloriesBurned - goalValue)
          });
      }
    }
  }
}
