// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.AddBandService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Sync;
using Microsoft.Health.App.Core.Services.TileSettings;
using Microsoft.Health.App.Core.Themes;
using Microsoft.Health.App.Core.ViewModels.AddBand;
using Microsoft.Health.App.Core.WindowsPhone.ViewModels.AddBand;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class AddBandService : IAddBandService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\AddBandService.cs");
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IHealthCloudClient healthCloudClient;
    private readonly ITileManagementService tileManager;
    private readonly IUserProfileService userProfileService;
    private readonly IWorkoutsProvider workoutsProvider;
    private readonly IBackgroundTaskService backgroundTaskService;
    private readonly ISyncEntryService syncEntryService;
    private readonly IDispatchService dispatchService;
    private readonly IBandHardwareService bandHardwareService;
    private readonly IBandThemeManager bandThemeManager;
    private readonly IBandInfoService bandInfoService;
    private readonly IRegionService regionService;
    private readonly ILocaleSettingsProvider localeSettingsProvider;
    private readonly ICultureService cultureService;
    private readonly IExerciseSyncService exerciseSyncService;
    private readonly IEnvironmentService environmentService;
    private AddBandService.AddBandState state;

    public AddBandService(
      IBandConnectionFactory cargoConnectionFactory,
      IHealthCloudClient healthCloudClient,
      IUserProfileService userProfileService,
      ITileManagementService tileManager,
      IWorkoutsProvider workoutsProvider,
      IBackgroundTaskService backgroundTaskService,
      ISmoothNavService smoothNavService,
      ISyncEntryService syncEntryService,
      IDispatchService dispatchService,
      IBandHardwareService bandHardwareService,
      IBandThemeManager bandThemeManager,
      IBandInfoService bandInfoService,
      IRegionService regionService,
      ILocaleSettingsProvider localeSettingsProvider,
      ICultureService cultureService,
      IExerciseSyncService exerciseSyncService,
      IEnvironmentService environmentService)
    {
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.healthCloudClient = healthCloudClient;
      this.userProfileService = userProfileService;
      this.tileManager = tileManager;
      this.workoutsProvider = workoutsProvider;
      this.backgroundTaskService = backgroundTaskService;
      this.SmoothNavService = smoothNavService;
      this.syncEntryService = syncEntryService;
      this.dispatchService = dispatchService;
      this.bandHardwareService = bandHardwareService;
      this.bandThemeManager = bandThemeManager;
      this.bandInfoService = bandInfoService;
      this.regionService = regionService;
      this.localeSettingsProvider = localeSettingsProvider;
      this.cultureService = cultureService;
      this.exerciseSyncService = exerciseSyncService;
      this.environmentService = environmentService;
    }

    protected ISmoothNavService SmoothNavService { get; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      this.SmoothNavService.DisableNavPanel(typeof (IAddBandService));
      Telemetry.SetBandVersion(string.Empty);
      Telemetry.SetFirmwareVersion(string.Empty);
      await this.bandHardwareService.ClearDeviceTypeAsync(cancellationToken);
      this.state = AddBandService.AddBandState.Connect;
      this.SmoothNavService.Navigate(typeof (AddBandPairingViewModel), action: NavigationStackAction.RemovePrevious);
    }

    public async Task NextAsync(
      CancellationToken cancellationToken,
      IProgress<InitializationProgress> progressListener = null)
    {
      switch (this.state)
      {
        case AddBandService.AddBandState.NotShown:
          throw new InvalidOperationException("Cannot call NextAsync without first calling StartAsync.");
        case AddBandService.AddBandState.Connect:
          this.state = AddBandService.AddBandState.Update;
          this.SmoothNavService.Navigate(typeof (FirmwareUpdateViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "Type",
              "New"
            }
          }, NavigationStackAction.RemovePrevious);
          break;
        case AddBandService.AddBandState.Update:
          this.state = AddBandService.AddBandState.PostUpdate;
          await this.PostUpdateAsync(cancellationToken);
          break;
        case AddBandService.AddBandState.PostUpdate:
          this.state = AddBandService.AddBandState.Done;
          this.SmoothNavService.Navigate(typeof (AddBandDoneViewModel), action: NavigationStackAction.RemovePrevious);
          break;
        case AddBandService.AddBandState.Done:
          await this.CompleteAsync(progressListener, cancellationToken);
          this.state = AddBandService.AddBandState.Shown;
          break;
        case AddBandService.AddBandState.Shown:
          throw new InvalidOperationException("Cannot call NextAsync after add a Band has completed.");
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public Task<bool> SkipAsync(CancellationToken cancellationToken)
    {
      switch (this.state)
      {
        case AddBandService.AddBandState.NotShown:
          throw new InvalidOperationException("Cannot call SkipAsync without first calling StartAsync.");
        case AddBandService.AddBandState.Connect:
        case AddBandService.AddBandState.Update:
        case AddBandService.AddBandState.PostUpdate:
          ++this.state;
          return Task.FromResult<bool>(true);
        case AddBandService.AddBandState.Done:
          throw new InvalidOperationException("Cannot call SkipAsync on Band completion.");
        case AddBandService.AddBandState.Shown:
          throw new InvalidOperationException("Cannot call SkipAsync after add a Band has completed.");
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public async Task ExitAsync(CancellationToken cancellationToken)
    {
      await this.SetBandAsync((IBandInfo) null, cancellationToken);
      await this.dispatchService.RunOnUIThreadAsync((Action) (() =>
      {
        this.SmoothNavService.EnableNavPanel(typeof (IAddBandService));
        this.SmoothNavService.GoHome();
        this.SmoothNavService.ClearBackStack();
      }));
    }

    public Task SetBandAsync(IBandInfo bandInfo, CancellationToken cancellationToken) => this.bandInfoService.SetBandInfoAsync(bandInfo, cancellationToken);

    public async Task SetBandScreenAsync(BandScreen screen, CancellationToken cancellationToken)
    {
      BandClass deviceType = await this.bandHardwareService.GetDeviceTypeAsync(cancellationToken);
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
      {
        switch (screen)
        {
          case BandScreen.CheckUpdate:
            try
            {
              if (deviceType == BandClass.Cargo)
              {
                await cargoConnection.NavigateToScreenAsync(CargoScreen.OobeUpdates, cancellationToken);
                break;
              }
              await cargoConnection.SetOobeStageAsync(OobeStage.CheckingForUpdate, cancellationToken);
              break;
            }
            catch (Exception ex)
            {
              AddBandService.Logger.Error((object) ex);
              break;
            }
          case BandScreen.StartUpdate:
            try
            {
              if (deviceType != BandClass.Cargo)
              {
                await cargoConnection.SetOobeStageAsync(OobeStage.StartingUpdate, cancellationToken);
                break;
              }
              break;
            }
            catch (Exception ex)
            {
              AddBandService.Logger.Error((object) ex);
              break;
            }
          case BandScreen.AlmostDone:
            try
            {
              if (deviceType == BandClass.Cargo)
              {
                await cargoConnection.NavigateToScreenAsync(CargoScreen.OobeAlmostThere, cancellationToken);
                break;
              }
              await cargoConnection.SetOobeStageAsync(OobeStage.WaitingOnPhoneToCompleteOobe, cancellationToken);
              break;
            }
            catch (Exception ex)
            {
              AddBandService.Logger.Error((object) ex);
              break;
            }
          case BandScreen.PressStart:
            if (deviceType == BandClass.Cargo)
            {
              await cargoConnection.NavigateToScreenAsync(CargoScreen.OobePresButtonToStart, cancellationToken);
              break;
            }
            await cargoConnection.FinalizeOobeAsync(cancellationToken);
            break;
        }
      }
    }

    protected virtual async Task PostUpdateAsync(CancellationToken cancellationToken) => await this.NextAsync(cancellationToken, (IProgress<InitializationProgress>) null);

    private async Task CompleteAsync(
      IProgress<InitializationProgress> progressListener,
      CancellationToken cancellationToken)
    {
      bool isAndroid = this.environmentService.IsAndroid();
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
      {
        progressListener?.Report(InitializationProgress.GettingReady);
        await cargoConnection.CheckConnectionWorkingAsync(cancellationToken);
        if (!isAndroid)
          await this.backgroundTaskService.RegisterBackgroundTasksAsync(cancellationToken, new bool?(true));
        await this.ApplyCurrentRegionSettingsAsync(await this.userProfileService.GetCloudUserProfileAsync(cancellationToken), cancellationToken);
        BandClass deviceType = await this.bandHardwareService.GetDeviceTypeAsync(cancellationToken);
        IList<AdminBandTile> defaults = await this.tileManager.PrepareDefaultsAsync(await cargoConnection.GetDefaultTilesAsync(cancellationToken), deviceType, cancellationToken);
        this.bandThemeManager.SetDeviceType(deviceType);
        if (deviceType == BandClass.Cargo)
        {
          this.bandThemeManager.SetCurrentTheme(this.bandThemeManager.DefaultTheme);
          await this.tileManager.SaveTilesWithCurrentThemeToBandAsync(new StartStrip((IEnumerable<AdminBandTile>) defaults), cancellationToken);
        }
        else
          await cargoConnection.SetStartStripAsync(new StartStrip((IEnumerable<AdminBandTile>) defaults), cancellationToken);
        await cargoConnection.UpdateGpsEphemerisDataAsync(cancellationToken, true);
        await cargoConnection.UpdateTimeZoneListAsync(cancellationToken, true);
        await cargoConnection.SetCurrentTimeAndTimeZoneAsync(cancellationToken);
        progressListener?.Report(InitializationProgress.AlmostThere);
        await this.userProfileService.LinkBandToProfileAsync(cancellationToken);
        await this.SaveGoalsToBandAsync(cargoConnection, cancellationToken);
        Guid guidedWorkoutTileId = Guid.Parse("0281c878-afa8-40ff-acfd-bca06c5c4922");
        if (defaults.Any<AdminBandTile>((Func<AdminBandTile, bool>) (tile => tile.TileId == guidedWorkoutTileId)))
          await this.UpdateGuidedWorkoutOnBandAsync(cancellationToken);
        string[] messageResponses = QuickResponsePendingTileSettings.GetDefaultMessageResponses();
        string[] phoneResponses = QuickResponsePendingTileSettings.GetDefaultPhoneResponses();
        await cargoConnection.SetSmsResponsesAsync(messageResponses[0], messageResponses[1], messageResponses[2], messageResponses[3], cancellationToken);
        await cargoConnection.SetPhoneCallResponsesAsync(phoneResponses[0], phoneResponses[1], phoneResponses[2], phoneResponses[3], cancellationToken);
        FirmwareVersions firmwareVersionAsync = await cargoConnection.GetBandFirmwareVersionAsync(cancellationToken);
        if (firmwareVersionAsync != null)
        {
          Telemetry.SetFirmwareVersion(firmwareVersionAsync.ApplicationVersion.ToString(true));
          Telemetry.SetBandVersion(firmwareVersionAsync.PcbId.ToString());
        }
        defaults = (IList<AdminBandTile>) null;
        phoneResponses = (string[]) null;
      }
      await this.SetBandScreenAsync(BandScreen.PressStart, cancellationToken);
      this.exerciseSyncService.ExerciseListSyncNeeded = true;
      await this.SetBandAsync((IBandInfo) null, cancellationToken);
      await this.ExitAsync(cancellationToken);
      try
      {
        AddBandService.Logger.Debug((object) "<START> Trying to refresh the user profile cache.");
        await this.userProfileService.RefreshUserProfileAsync(cancellationToken);
        AddBandService.Logger.Debug((object) "<END> Trying to refresh the user profile cache.");
      }
      catch (Exception ex)
      {
        AddBandService.Logger.Error((object) "<FAILED> Trying to refresh the user profile cache.", ex);
      }
      this.syncEntryService.Sync(SyncType.AddBand, TimeSpan.Zero);
      progressListener?.Report(InitializationProgress.Complete);
      if (!isAndroid)
        return;
      await this.backgroundTaskService.RegisterBackgroundTasksAsync(cancellationToken, new bool?(true));
    }

    private async Task ApplyCurrentRegionSettingsAsync(
      BandUserProfile userProfile,
      CancellationToken token)
    {
      bool updateCloudProfile = false;
      if (string.IsNullOrEmpty(userProfile.PreferredLocale))
      {
        userProfile.PreferredLocale = this.cultureService.CurrentSupportedUICulture.Name;
        updateCloudProfile = true;
      }
      if (string.IsNullOrEmpty(userProfile.PreferredRegion))
      {
        userProfile.PreferredRegion = this.regionService.CurrentRegion.TwoLetterISORegionName;
        updateCloudProfile = true;
      }
      string currentRegion = this.regionService.CurrentRegion.TwoLetterISORegionName;
      ILocaleSettings localeSettings = (await this.localeSettingsProvider.LoadLocaleSettingsAsync(token)).FirstOrDefault<ILocaleSettings>((Func<ILocaleSettings, bool>) (p => p.TwoLetterLocale == currentRegion));
      if (localeSettings != null)
        userProfile.ApplyLocaleSettings(localeSettings);
      if (updateCloudProfile)
        await this.userProfileService.SaveCloudAndBandUserProfileAsync(userProfile, token);
      else
        await this.userProfileService.ImportUserProfileAsync(userProfile, token);
    }

    private async Task SaveGoalsToBandAsync
    (
      IBandConnection cargoConnection,
      CancellationToken cancellationToken)
    {
      IList<UsersGoal> usersGoalsAsync = await this.healthCloudClient.GetUsersGoalsAsync(GoalType.Unknown, cancellationToken);
      Goals goals = new Goals();
      foreach (UsersGoal usersGoal in (IEnumerable<UsersGoal>) usersGoalsAsync)
      {
        switch (usersGoal.Type)
        {
          case GoalType.StepGoal:
            goals.StepsEnabled = true;
            goals.StepsGoal = (uint) usersGoal.Value;
            continue;
          case GoalType.CalorieGoal:
            goals.CaloriesEnabled = true;
            goals.CaloriesGoal = (uint) usersGoal.Value;
            continue;
          default:
            continue;
        }
      }
      if (goals.StepsGoal == 0U)
      {
        goals.StepsEnabled = true;
        goals.StepsGoal = 5000U;
      }
      if (goals.CaloriesGoal == 0U)
      {
        goals.CaloriesEnabled = true;
        goals.CaloriesGoal = 5000U;
      }
      await cargoConnection.SaveGoalsToBandAsync(goals);
    }

    private async Task UpdateGuidedWorkoutOnBandAsync(CancellationToken cancellationToken)
    {
      AddBandService.Logger.Debug((object) "<START> Updating the Guided Workout on the band.");
      cancellationToken.ThrowIfCancellationRequested();
      WorkoutStatus workoutStatus = await this.workoutsProvider.GetLastSyncedWorkoutAsync(cancellationToken).ConfigureAwait(false);
      if (workoutStatus != null && workoutStatus.WorkoutPlanId != null)
      {
        try
        {
          await this.workoutsProvider.UploadWorkoutBandFileAsync(workoutStatus.WorkoutPlanId, workoutStatus.WorkoutIndex, workoutStatus.WeekId, workoutStatus.Day, workoutStatus.WorkoutPlanInstanceId, cancellationToken).ConfigureAwait(false);
          AddBandService.Logger.Debug((object) "<END> Updating the Guided Workout on the band: the previously sync'd Guided Workout was uploaded.");
        }
        catch (Exception ex)
        {
          AddBandService.Logger.Error(ex, "<END> Updating the Guided Workout on the band: the previously sync'd Guided Workout could not be uploaded.");
        }
      }
      else
        AddBandService.Logger.Debug((object) "<END> Updating the Guided Workout on the band: no Guided Workout was previously sync'd.");
    }

    private enum AddBandState
    {
      NotShown,
      Connect,
      Update,
      PostUpdate,
      Done,
      Shown,
    }
  }
}
