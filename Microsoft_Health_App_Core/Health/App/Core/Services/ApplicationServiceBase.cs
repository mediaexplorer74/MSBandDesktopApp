// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ApplicationServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services.Devices;
using Microsoft.Health.App.Core.Services.Diagnostics;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.App.Core.Services.Sync;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.AddBand;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.App.Core.ViewModels.SendFeedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public abstract class ApplicationServiceBase : IApplicationService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\ApplicationServiceBase.cs");
    private static readonly TimeSpan StartupForegroundSyncThreshold = TimeSpan.FromMinutes(30.0);
    private static readonly TimeSpan NotificationStartupDelay = TimeSpan.FromSeconds(5.0);
    private readonly IShakeFeedbackService shakeFeedbackService;
    private readonly IDiagnosticsService diagnosticsService;
    private readonly IFileSystemService fileSystemService;
    private readonly ISmoothNavService smoothNavService;
    private readonly INetworkService networkService;
    private readonly IMessageBoxService messageBoxService;
    private readonly IFirmwareUpdateService firmwareUpdateService;
    private readonly IBackgroundTaskService backgroundTaskService;
    private readonly ITimeChangeService timeChangeService;
    private readonly ITileManagementService tileManagementService;
    private readonly IUserProfileService userProfileService;
    private readonly IWhatsNewService whatsNewService;
    private readonly ISyncEntryService syncEntryService;
    private readonly INetPromoterService netPromoterService;
    private readonly IDeviceManager deviceManager;
    private readonly IConfig config;

    protected ApplicationServiceBase(
      IShakeFeedbackService shakeFeedbackService,
      IFileSystemService fileSystemService,
      IDiagnosticsService diagnosticsService,
      ISmoothNavService smoothNavService,
      INetworkService networkService,
      IMessageBoxService messageBoxService,
      IFirmwareUpdateService firmwareUpdateService,
      IBackgroundTaskService backgroundTaskService,
      ITimeChangeService timeChangeService,
      IUserProfileService userProfileService,
      ITileManagementService tileManagementService,
      INetPromoterService netPromoterService,
      IDeviceManager deviceManager,
      IConfig config,
      IWhatsNewService whatsNewService,
      ISyncEntryService syncEntryService)
    {
      this.shakeFeedbackService = shakeFeedbackService;
      this.fileSystemService = fileSystemService;
      this.diagnosticsService = diagnosticsService;
      this.smoothNavService = smoothNavService;
      this.networkService = networkService;
      this.messageBoxService = messageBoxService;
      this.firmwareUpdateService = firmwareUpdateService;
      this.backgroundTaskService = backgroundTaskService;
      this.timeChangeService = timeChangeService;
      this.userProfileService = userProfileService;
      this.tileManagementService = tileManagementService;
      this.netPromoterService = netPromoterService;
      this.deviceManager = deviceManager;
      this.config = config;
      this.whatsNewService = whatsNewService;
      this.syncEntryService = syncEntryService;
    }

    public virtual Task OnStartedAsync()
    {
      this.shakeFeedbackService.Initialize();
      this.whatsNewService.UpdateViewStatus();
      this.whatsNewService.IncrementApplicationSessions();
      this.IncrementNpsUseCount();
      return (Task) Task.FromResult<object>((object) null);
    }

    public virtual void OnTerminated()
    {
    }

    public void IncrementNpsUseCount() => this.netPromoterService.IncrementApplicationUseCount();

    public async Task<bool> ShowFirmwareUpdateNotificationIfNeededAsync(
      CancellationToken cancellationToken)
    {
      bool showFirmwareUpdateNotification = false;
      JournalEntry currentJournalEntry = this.smoothNavService.CurrentJournalEntry;
      if (currentJournalEntry != null && (object) currentJournalEntry.ViewModelType != (object) typeof (FirmwareUpdateViewModel))
      {
        if (this.networkService.IsInternetAvailable && this.CheckAdditionalPrerequisitesForFirmwareUpdate())
          showFirmwareUpdateNotification = await this.EnforceFirmwareUpdateStartupPolicyAsync(cancellationToken);
        else
          ApplicationServiceBase.Logger.Warn((object) "Firmware update notification will not be shown because prerequisites check has failed.");
      }
      if (showFirmwareUpdateNotification)
      {
        ApplicationTelemetry.LogFirmwarePrompt();
        await this.ShowFirmwareUpdateNotificationAsync();
      }
      return showFirmwareUpdateNotification;
    }

    public async void StartCoreDelayedLoadingTask(CancellationToken cancellationToken)
    {
      ApplicationServiceBase.Logger.Debug((object) "<START> handling normal startup");
      await this.DeleteOldSessionsAsync();
      if (!await this.ShowFirmwareUpdateNotificationIfNeededAsync(cancellationToken))
      {
        this.PromptForNpsIfNeeded();
        if (this.config.IsForegroundSyncOnAppStartupEnabled && this.networkService.IsInternetAvailable && this.CheckAdditionalPrerequisitesForSync())
        {
          bool flag;
          try
          {
            List<IDevice> list1 = (await this.deviceManager.GetDevicesAsync(cancellationToken)).ToList<IDevice>();
            if (list1.Count > 0)
            {
              List<Task<DateTimeOffset?>> lastSyncTimeTasks = list1.Select<IDevice, Task<DateTimeOffset?>>((Func<IDevice, Task<DateTimeOffset?>>) (d => d.GetLastSyncTimeAsync(cancellationToken))).ToList<Task<DateTimeOffset?>>();
              DateTimeOffset?[] nullableArray = await Task.WhenAll<DateTimeOffset?>((IEnumerable<Task<DateTimeOffset?>>) lastSyncTimeTasks);
              List<DateTimeOffset> list2 = lastSyncTimeTasks.Where<Task<DateTimeOffset?>>((Func<Task<DateTimeOffset?>, bool>) (t => t.Result.HasValue)).Select<Task<DateTimeOffset?>, DateTimeOffset>((Func<Task<DateTimeOffset?>, DateTimeOffset>) (t => t.Result.Value)).ToList<DateTimeOffset>();
              flag = !list2.Any<DateTimeOffset>() || DateTimeOffset.Now - list2.Min<DateTimeOffset>() > ApplicationServiceBase.StartupForegroundSyncThreshold;
              lastSyncTimeTasks = (List<Task<DateTimeOffset?>>) null;
            }
            else
              flag = false;
          }
          catch (Exception ex)
          {
            ApplicationServiceBase.Logger.Warn((object) "Could not get last sync time, running foreground sync.", ex);
            flag = true;
          }
          if (flag)
            this.syncEntryService.Sync(SyncType.Refresh, CancellationTokenUtilities.DefaultCancellationTokenTimespan, true);
        }
        else
          ApplicationServiceBase.Logger.Warn((object) "<WARNING> sync on startup skipped");
      }
      await this.UpdateBandRegistrationStatusAsync(cancellationToken);
      this.timeChangeService.ListenForTimeChanges();
      ApplicationServiceBase.Logger.Debug((object) "<END> handling normal startup");
    }

    public virtual async void StartDelayedLoadingTask(CancellationToken cancellationToken)
    {
      try
      {
        await Task.Yield();
        this.StartCoreDelayedLoadingTask(cancellationToken);
        this.backgroundTaskService.Initialize();
        await this.backgroundTaskService.RegisterBackgroundTasksAsync(cancellationToken).ConfigureAwait(false);
        await Task.Delay(ApplicationServiceBase.NotificationStartupDelay, cancellationToken).ConfigureAwait(false);
        await this.tileManagementService.EnsureTileUpdatesEnabledAsync().ConfigureAwait(false);
      }
      catch (OperationCanceledException ex)
      {
        ApplicationServiceBase.Logger.Error((Exception) ex, "Could not finish delayed loading task. Operation timed out.");
      }
      catch (Exception ex)
      {
        ApplicationServiceBase.Logger.Error(ex, "Could not finish delayed loading task.");
      }
    }

    public virtual Task NavigateToFirstPageAsync() => throw new NotImplementedException();

    public virtual async Task OnInitialHomePageLoadedAsync() => await this.CheckAndReportCrashAsync();

    public virtual async Task<bool> HandleAppStartUpNavigationGatesAsync()
    {
      if (this.config.OobeStatus == OobeStatus.NotShown)
      {
        ApplicationServiceBase.Logger.Debug((object) "OOBE not shown - navigate appropriately");
        this.smoothNavService.Navigate<FreshInstallLoadingViewModel>();
        return true;
      }
      if (this.config.IsNavigatedAwayDuringFirmwareUpdatePromptNeeded)
      {
        this.config.IsNavigatedAwayDuringFirmwareUpdatePromptNeeded = false;
        ApplicationServiceBase.Logger.Debug((object) "navigated away while firmware update");
        int num = (int) await this.messageBoxService.ShowAsync(AppResources.FirmwareUpdateMessageBoxInterruptedErrorMessage, AppResources.FirmwareUpdateMessageBoxGeneralHeader, PortableMessageBoxButton.OK);
        if (this.config.OobeStatus.Equals((object) OobeStatus.Shown))
        {
          this.smoothNavService.Navigate(typeof (FirmwareUpdateViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "Type",
              "Normal"
            }
          });
          this.smoothNavService.ClearBackStack();
          return true;
        }
      }
      return false;
    }

    protected async Task DeleteOldSessionsAsync()
    {
      try
      {
        await this.fileSystemService.DeleteSessionFoldersAsync();
      }
      catch (Exception ex)
      {
        ApplicationServiceBase.Logger.Warn((object) "Unable to delete old session folders.", ex);
      }
    }

    protected virtual bool CheckAdditionalPrerequisitesForFirmwareUpdate() => true;

    protected virtual bool CheckAdditionalPrerequisitesForSync() => true;

    private async Task UpdateBandRegistrationStatusAsync(CancellationToken cancellationToken)
    {
      ApplicationServiceBase.Logger.Debug((object) "<START> Refreshing band registration status.");
      try
      {
        await this.userProfileService.RefreshUserProfileAsync(cancellationToken);
      }
      catch (Exception ex)
      {
        ApplicationServiceBase.Logger.Error((object) "<FAILURE> Refreshing band registration status.", ex);
      }
      ApplicationServiceBase.Logger.Debug((object) "<END> Refreshing band registration status.");
    }

    private async Task CheckAndReportCrashAsync()
    {
      try
      {
        await this.diagnosticsService.CheckAndReportOnLastCrashAsync();
      }
      catch (Exception ex)
      {
        ApplicationServiceBase.Logger.Warn((object) "Unable to check for last crash.", ex);
      }
    }

    private async Task<bool> EnforceFirmwareUpdateStartupPolicyAsync(
      CancellationToken cancellationToken)
    {
      bool isFirmwareUpdateRequired = false;
      try
      {
        isFirmwareUpdateRequired = await this.firmwareUpdateService.CheckForFirmwareUpdateAsync(cancellationToken);
      }
      catch (Exception ex)
      {
        ApplicationServiceBase.Logger.Warn((object) "<FLAG> firmware update check skipped because of failure", ex);
      }
      return isFirmwareUpdateRequired;
    }

    private async Task ShowFirmwareUpdateNotificationAsync()
    {
      if (EnvironmentUtilities.IsDebugSettingEnabled)
      {
        if (await this.messageBoxService.ShowAsync(AppResources.FirmwareUpdateMessage, AppResources.FirmwareUpdateDialogTitle, PortableMessageBoxButton.OKCancel) != PortableMessageBoxResult.OK)
          return;
        this.smoothNavService.Navigate(typeof (FirmwareUpdateViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "Type",
            "Normal"
          }
        });
        this.smoothNavService.ClearBackStack();
      }
      else
      {
        int num = (int) await this.messageBoxService.ShowAsync(string.Format(AppResources.FirmwareUpdateMessageBoxRequiredUpdateMessage), AppResources.FirmwareUpdateMessageBoxRequiredUpdateHeader, PortableMessageBoxButton.OK);
        this.smoothNavService.Navigate(typeof (FirmwareUpdateViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
        {
          {
            "Type",
            "Normal"
          }
        });
        this.smoothNavService.ClearBackStack();
      }
    }

    private void PromptForNpsIfNeeded()
    {
      if (!this.netPromoterService.PromptUserForNpsSurvey)
        return;
      this.smoothNavService.Navigate(typeof (NetPromoterScoreViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "referrerSource",
          "prompt"
        }
      });
      this.netPromoterService.SetUserHasBeenPromptedForNpsFlag(true);
    }
  }
}
