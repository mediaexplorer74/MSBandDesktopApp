// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Steps.StepsTileViewModel
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
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Services.Devices;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Steps
{
  [PageTaxonomy(new string[] {"Steps"})]
  public class StepsTileViewModel : MetricTileViewModel
  {
    private const string StepsTileViewModelCategory = "StepsTileViewModel";
    public static readonly ConfigurationValue<bool> ShowPhonePanelValue = ConfigurationValue.CreateBoolean(nameof (StepsTileViewModel), "ShowPhonePanel", false);
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Steps\\StepsTileViewModel.cs");
    private readonly IConfig config;
    private readonly IDateTimeService dateTimeService;
    private readonly object globalMessageSubscriber = new object();
    private readonly IGoalsProvider goalsProvider;
    private readonly IMessageSender messageSender;
    private readonly IUserDailySummaryProvider userDailySummaryProvider;
    private readonly IUserProfileService userProfileService;
    private UserDailySummaryGroup userDailySummaryGroup;
    private UsersGoal usersGoal;

    public StepsTileViewModel(
      IGoalsProvider goalsProvider,
      IUserProfileService userProfileService,
      IConfig config,
      IConfigurationService configurationService,
      INetworkService networkService,
      IMessageSender messageSender,
      StepsDailyViewModel stepsDailyViewModel,
      StepsWeeklyViewModel stepsWeeklyViewModel,
      ISmoothNavService smoothNavService,
      IDateTimeService dateTimeService,
      IUserDailySummaryProvider userDailySummaryProvider,
      IDeviceManager deviceManager,
      IServiceLocator serviceLocator,
      IEnvironmentService environmentService,
      TileFirstTimeUseViewModel firstTimeUse)
      : base(networkService, smoothNavService, messageSender, firstTimeUse)
    {
      Assert.ParamIsNotNull((object) goalsProvider, nameof (goalsProvider));
      Assert.ParamIsNotNull((object) userProfileService, nameof (userProfileService));
      Assert.ParamIsNotNull((object) config, nameof (config));
      Assert.ParamIsNotNull((object) messageSender, nameof (messageSender));
      Assert.ParamIsNotNull((object) stepsDailyViewModel, nameof (stepsDailyViewModel));
      Assert.ParamIsNotNull((object) smoothNavService, nameof (smoothNavService));
      Assert.ParamIsNotNull((object) dateTimeService, nameof (dateTimeService));
      Assert.ParamIsNotNull((object) userDailySummaryProvider, nameof (userDailySummaryProvider));
      Assert.ParamIsNotNull((object) deviceManager, nameof (deviceManager));
      this.goalsProvider = goalsProvider;
      this.userProfileService = userProfileService;
      this.config = config;
      this.messageSender = messageSender;
      this.dateTimeService = dateTimeService;
      this.userDailySummaryProvider = userDailySummaryProvider;
      this.TileIcon = "\uE008";
      this.ColorLevel = TileColorLevel.Medium;
      this.Pivots.Add(new PivotDefinition(AppResources.PivotDaily, (object) stepsDailyViewModel));
      this.Pivots.Add(new PivotDefinition(AppResources.PivotWeekly, (object) stepsWeeklyViewModel));
      this.AddPhonePivot(configurationService, deviceManager, serviceLocator, environmentService);
      this.messageSender.Register<GoalsChangedMessage>(this.globalMessageSubscriber, (Action<GoalsChangedMessage>) (message =>
      {
        if (message.GoalType != GoalType.StepGoal || this.userDailySummaryGroup == null)
          return;
        this.UpdateSubheader((long) message.Value);
      }));
    }

    private async void AddPhonePivot(
      IConfigurationService configurationService,
      IDeviceManager deviceManager,
      IServiceLocator serviceLocator,
      IEnvironmentService environmentService)
    {
      if (!configurationService.GetValue<bool>(StepsTileViewModel.ShowPhonePanelValue))
        return;
      try
      {
        if ((await deviceManager.GetDevicesAsync(CancellationToken.None)).FirstOrDefault<IDevice>((Func<IDevice, bool>) (device => device.DeviceType == DeviceType.Phone)) == null)
          return;
        string phoneId = environmentService.PhoneId;
        StepsDailyViewModel instance = serviceLocator.GetInstance<StepsDailyViewModel>();
        instance.DeviceId = phoneId;
        this.Pivots.Add(new PivotDefinition("Phone", (object) instance));
      }
      catch (Exception ex)
      {
        StepsTileViewModel.Logger.Error(ex, "Unable to show the Phone panel.");
      }
    }

    protected override async Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      StepsTileViewModel.Logger.Debug((object) "<START> loading the steps tile");
      Task<UserDailySummaryGroup> getUserDailySummariesTask = this.userDailySummaryProvider.GetUserDailySummaryGroupAsync(this.dateTimeService.GetToday());
      Task<UsersGoal> getGoalExpandedTask = this.goalsProvider.GetGoalExpandedAsync(GoalType.StepGoal);
      await Task.WhenAll((Task) getUserDailySummariesTask, (Task) getGoalExpandedTask);
      this.userDailySummaryGroup = getUserDailySummariesTask.Result;
      this.usersGoal = getGoalExpandedTask.Result;
      if ((this.userDailySummaryGroup == null || this.userDailySummaryGroup.TotalStepsTaken == 0) && this.usersGoal == null)
      {
        StepsTileViewModel.Logger.Debug((object) "<END> loading the steps tile (No Data)");
        return false;
      }
      StepsTileViewModel.Logger.Debug((object) "<END> loading the steps tile (Loaded)");
      return true;
    }

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      if (this.userDailySummaryGroup != null)
        this.Header = Formatter.FormatSteps(this.userDailySummaryGroup.TotalStepsTaken, true);
      if (this.usersGoal != null)
        this.UpdateSubheader(this.usersGoal.Value);
      this.CanOpen = true;
    }

    protected override async Task OnTransitionToNoDataStateAsync()
    {
      await base.OnTransitionToNoDataStateAsync();
      this.Subheader = AppResources.StepsTileNoDataMessage;
      this.ColorLevel = TileColorLevel.Medium;
    }

    private void UpdateSubheader(long goalValue)
    {
      if (this.dateTimeService.Now - this.config.LastCompletedFullOobe < TimeSpan.FromDays(1.0))
      {
        this.Subheader = string.Format(AppResources.StepsTileFirstUse, new object[1]
        {
          (object) Formatter.FormatSteps((int) goalValue)
        });
      }
      else
      {
        if (this.userDailySummaryGroup == null)
          return;
        double num = goalValue > 0L ? (double) this.userDailySummaryGroup.TotalStepsTaken / (double) goalValue * 100.0 : 0.0;
        if (num <= 95.0)
          this.Subheader = string.Format(AppResources.GoalsTilePercentComplete, new object[1]
          {
            (object) (int) num
          });
        else if (num < 100.0)
        {
          TimeSpan walkTime = Conversions.StepsToWalkTime((int) (goalValue - (long) this.userDailySummaryGroup.TotalStepsTaken), this.userProfileService.CurrentUserProfile.Gender);
          if (walkTime.TotalSeconds > 60.0)
            this.Subheader = string.Format(AppResources.GoalTileAlmostThereSteps, new object[1]
            {
              (object) Formatter.FormatTimeSpan(walkTime, Formatter.TimeSpanFormat.Full, false)
            });
          else
            this.Subheader = string.Format(AppResources.GoalTileAlmostThereSteps, new object[1]
            {
              (object) Formatter.FormatTimeSpan(TimeSpan.FromMinutes(1.0), Formatter.TimeSpanFormat.Full, false)
            });
        }
        else if (num <= 102.0)
          this.Subheader = AppResources.GoalsReached;
        else
          this.Subheader = string.Format(AppResources.GoalsTileOverAchievementSteps, new object[1]
          {
            (object) ((long) this.userDailySummaryGroup.TotalStepsTaken - goalValue)
          });
      }
    }
  }
}
