// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Devices.DeviceManager
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Services.ForegroundSync;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Pedometer;
using Microsoft.Health.App.Core.Services.Sync;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Devices
{
  public sealed class DeviceManager : IDeviceManager
  {
    public const string PedometerIocDeviceName = "Pedometer";
    public const string BandIocDeviceName = "Band";
    private const string DeviceManagerCategory = "DeviceManager";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Devices\\DeviceManager.cs");
    public static readonly ConfigurationValue<bool> IsBandAvailableValue = ConfigurationValue.CreateBoolean(nameof (DeviceManager), "IsBandAvailable", (Func<bool>) (() => true));
    private readonly IConfigurationService configurationService;
    private readonly IPedometerManager pedometerManager;
    private readonly IServiceLocator serviceLocator;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IUserProfileService userProfileService;
    private readonly ISyncInvalidator syncInvalidator;
    private readonly IRefreshService refreshService;
    private readonly IPlatformSyncService platformSyncService;
    private bool anySucceeded;
    private int deviceCount;
    private CancellationTokenSource syncDevicesTokenSource;

    public DeviceManager(
      IConfigurationService configurationService,
      IPedometerManager pedometerManager,
      IServiceLocator serviceLocator,
      IErrorHandlingService errorHandlingService,
      IUserProfileService userProfileService,
      ISyncInvalidator syncInvalidator,
      IRefreshService refreshService,
      IPlatformSyncService platformSyncService)
    {
      Assert.ParamIsNotNull((object) configurationService, nameof (configurationService));
      Assert.ParamIsNotNull((object) pedometerManager, nameof (pedometerManager));
      Assert.ParamIsNotNull((object) serviceLocator, nameof (serviceLocator));
      Assert.ParamIsNotNull((object) errorHandlingService, nameof (errorHandlingService));
      Assert.ParamIsNotNull((object) userProfileService, nameof (userProfileService));
      Assert.ParamIsNotNull((object) syncInvalidator, nameof (syncInvalidator));
      Assert.ParamIsNotNull((object) refreshService, nameof (refreshService));
      this.configurationService = configurationService;
      this.pedometerManager = pedometerManager;
      this.serviceLocator = serviceLocator;
      this.errorHandlingService = errorHandlingService;
      this.userProfileService = userProfileService;
      this.syncInvalidator = syncInvalidator;
      this.refreshService = refreshService;
      this.platformSyncService = platformSyncService;
      this.pedometerManager.IsEnabledChanged += new EventHandler(this.OnEnableStateChanged);
    }

    public event EventHandler<SyncStateChangedEventArgs> SyncStateChanged;

    public event EventHandler DevicesChanged;

    public bool IsSyncing { get; private set; }

    public SyncStateChangedEventArgs LastReportedSyncState { get; private set; }

    public async Task<IGetDevicesResult> GetDevicesAsync(
      CancellationToken token)
    {
      List<IDevice> devices = new List<IDevice>();
      List<Exception> exceptions = new List<Exception>();
      try
      {
        IDevice bandDevice = this.GetBandDevice();
        if (bandDevice != null)
          devices.Add(bandDevice);
      }
      catch (Exception ex)
      {
        DeviceManager.Logger.Debug((object) "Failed to get Band device.", ex);
        exceptions.Add(ex);
      }
      try
      {
        IDevice device = await this.GetPedometerDeviceAsync(token).ConfigureAwait(false);
        if (device != null)
          devices.Add(device);
      }
      catch (Exception ex)
      {
        DeviceManager.Logger.Debug((object) "Failed to get pedometer device.", ex);
        exceptions.Add(ex);
      }
      return (IGetDevicesResult) new GetDevicesResult((IEnumerable<IDevice>) devices, new AggregateException((IEnumerable<Exception>) exceptions));
    }

    public async Task SyncDevicesAsync(
      SyncType syncType,
      CancellationToken token,
      bool ignoreIfUnable = false,
      IProgress<DeviceSyncProgress> progressReporter = null)
    {
      if (this.IsSyncing)
      {
        DeviceManager.Logger.Warn((object) "Sync aborted, another is already in progress.");
      }
      else
      {
        this.anySucceeded = false;
        this.IsSyncing = true;
        Exception ex = (Exception) null;
        try
        {
          using (this.syncDevicesTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token))
          {
            IDevice pedometerDevice = await this.GetPedometerDeviceAsync(this.syncDevicesTokenSource.Token).ConfigureAwait(false);
            IDevice bandDevice = this.GetBandDevice();
            bool flag1 = pedometerDevice != null;
            if (flag1)
              flag1 = await pedometerDevice.CanSyncAsync(this.syncDevicesTokenSource.Token).ConfigureAwait(false);
            bool isPedometerDeviceAvailable = flag1;
            bool flag2 = bandDevice != null;
            if (flag2)
              flag2 = await bandDevice.CanSyncAsync(this.syncDevicesTokenSource.Token).ConfigureAwait(false);
            bool flag3 = flag2;
            if (!isPedometerDeviceAvailable && !flag3)
              this.deviceCount = 0;
            else if (!flag3 || !isPedometerDeviceAvailable)
            {
              this.deviceCount = 1;
              try
              {
                if (!flag3)
                  await this.SyncPedometerOnlyAsync(syncType, this.syncDevicesTokenSource.Token, ignoreIfUnable, progressReporter, pedometerDevice);
                else
                  await this.SyncBandOnlyAsync(syncType, this.syncDevicesTokenSource.Token, ignoreIfUnable, progressReporter, bandDevice);
                this.anySucceeded = true;
              }
              catch (Exception ex1)
              {
                ex = ex1;
              }
            }
            else
              ex = await this.SyncBandAndPedometerAsync(syncType, this.syncDevicesTokenSource.Token, ignoreIfUnable, progressReporter, pedometerDevice, bandDevice);
            await this.platformSyncService.SyncPlatformItemsAsync(this.syncDevicesTokenSource.Token);
            pedometerDevice = (IDevice) null;
            bandDevice = (IDevice) null;
          }
        }
        catch (Exception ex2)
        {
          ex = ex2;
        }
        this.IsSyncing = false;
        DeviceManager.Logger.DebugFormat("Calling SyncInvalidator. Device count: {0}, Any Succeeded: {1}", (object) this.deviceCount, (object) this.anySucceeded);
        await this.syncInvalidator.InvalidateAsync(this.deviceCount, this.anySucceeded);
        if (this.deviceCount > 0)
          this.ReportProgressChanged(progressReporter, ProgressStage.Analyzing, 100.0);
        await this.refreshService.RefreshAsync(CancellationToken.None);
        if (ex == null)
          return;
        ApplicationTelemetry.LogSyncError(((object) ex).GetType().ToString(), ex.Message);
        if (this.syncDevicesTokenSource != null && this.syncDevicesTokenSource.IsCancellationRequested)
        {
          DeviceManager.Logger.Warn((object) "Exception occured when syncing was cancelled.", ex);
        }
        else
        {
          DeviceManager.Logger.Error((object) "Exception occurred when syncing.", ex);
          try
          {
            await this.errorHandlingService.HandleExceptionAsync(DeviceManager.GetRealException(ex)).ConfigureAwait(false);
          }
          catch (HeadlessException ex3)
          {
          }
        }
      }
    }

    public void CancelDevicesSync()
    {
      if (!this.IsSyncing)
        return;
      if (this.syncDevicesTokenSource == null)
        return;
      try
      {
        this.syncDevicesTokenSource.Cancel();
      }
      catch (ObjectDisposedException ex)
      {
        DeviceManager.Logger.Debug((object) "SyncDevicesTokenSource was already disposed when attempting to cancel syncing.", (Exception) ex);
      }
      catch (Exception ex)
      {
        DeviceManager.Logger.Error((object) "Exception occured when attempting to cancel syncing.", ex);
      }
    }

    private async Task<Exception> SyncBandAndPedometerAsync(
      SyncType syncType,
      CancellationToken token,
      bool ignoreIfUnable,
      IProgress<DeviceSyncProgress> progressReporter,
      IDevice pedometerDevice,
      IDevice bandDevice)
    {
      Exception ex = (Exception) null;
      DeviceManager.Logger.Info((object) "<START> Syncing both band and sensor core.");
      this.deviceCount = 2;
      DeviceSyncProgress pedometerSyncProgress = new DeviceSyncProgress(true, ProgressStage.CheckingYourBand, 0.0);
      DeviceSyncProgress bandSyncProgress = new DeviceSyncProgress(true, ProgressStage.CheckingYourBand, 0.0);
      Task pedometerSyncTask = (Task) null;
      Task bandDeviceSyncTask = (Task) null;
      pedometerSyncTask = pedometerDevice.SyncDeviceAsync(syncType, token, ignoreIfUnable, (IProgress<DeviceSyncProgress>) new SimpleProgress<DeviceSyncProgress>((Action<DeviceSyncProgress>) (e =>
      {
        pedometerSyncProgress = e;
        this.OnMultiSyncProgressChanged(progressReporter, pedometerSyncTask, pedometerSyncProgress, bandDeviceSyncTask, bandSyncProgress);
      })));
      bandDeviceSyncTask = bandDevice.SyncDeviceAsync(syncType, token, ignoreIfUnable, (IProgress<DeviceSyncProgress>) new SimpleProgress<DeviceSyncProgress>((Action<DeviceSyncProgress>) (e =>
      {
        bandSyncProgress = e;
        this.OnMultiSyncProgressChanged(progressReporter, pedometerSyncTask, pedometerSyncProgress, bandDeviceSyncTask, bandSyncProgress);
      })));
      try
      {
        await Task.WhenAll(pedometerSyncTask, bandDeviceSyncTask).ConfigureAwait(false);
      }
      catch (Exception ex1)
      {
        ex = bandDeviceSyncTask.IsFaulted ? (Exception) bandDeviceSyncTask.Exception : (Exception) pedometerSyncTask.Exception;
      }
      if (pedometerSyncTask.Status == TaskStatus.RanToCompletion || bandDeviceSyncTask.Status == TaskStatus.RanToCompletion)
        this.anySucceeded = true;
      DeviceManager.Logger.InfoFormat("<END> Syncing both band and sensor core. Band task status: {0}, Sensor core task status: {1}", (object) bandDeviceSyncTask.Status, (object) pedometerSyncTask.Status);
      return bandDeviceSyncTask.Status == TaskStatus.RanToCompletion ? (Exception) null : ex;
    }

    private async Task SyncBandOnlyAsync(
      SyncType syncType,
      CancellationToken token,
      bool ignoreIfUnable,
      IProgress<DeviceSyncProgress> progressReporter,
      IDevice bandDevice)
    {
      DeviceManager.Logger.Info((object) "<START> Syncing the band only.");
      await bandDevice.SyncDeviceAsync(syncType, token, ignoreIfUnable, this.CreatePassthroughProgressHandler(progressReporter)).ConfigureAwait(false);
      DeviceManager.Logger.Info((object) "<END> Syncing the band only.");
    }

    private async Task SyncPedometerOnlyAsync(
      SyncType syncType,
      CancellationToken token,
      bool ignoreIfUnable,
      IProgress<DeviceSyncProgress> progressReporter,
      IDevice pedometerDevice)
    {
      DeviceManager.Logger.Info((object) "<START> Syncing sensor core only.");
      await pedometerDevice.SyncDeviceAsync(syncType, token, ignoreIfUnable, this.CreatePassthroughProgressHandler(progressReporter)).ConfigureAwait(false);
      DeviceManager.Logger.Info((object) "<END> Syncing sensor core only.");
    }

    private void OnEnableStateChanged(object sender, EventArgs e)
    {
      EventHandler devicesChanged = this.DevicesChanged;
      if (devicesChanged == null)
        return;
      devicesChanged((object) this, EventArgs.Empty);
    }

    private static Exception GetRealException(Exception exception) => !(exception is AggregateException aggregateException) ? exception : aggregateException.InnerException;

    private IProgress<DeviceSyncProgress> CreatePassthroughProgressHandler(
      IProgress<DeviceSyncProgress> progressReporter)
    {
      return (IProgress<DeviceSyncProgress>) new SimpleProgress<DeviceSyncProgress>((Action<DeviceSyncProgress>) (e =>
      {
        if (!e.IsSyncing)
          this.anySucceeded = true;
        this.ReportProgressChanged(progressReporter, e.Stage, e.PercentComplete);
      }));
    }

    private void OnMultiSyncProgressChanged(
      IProgress<DeviceSyncProgress> progressReporter,
      Task pedometerSyncTask,
      DeviceSyncProgress pedometerSyncProgress,
      Task bandSyncTask,
      DeviceSyncProgress bandSyncProgress)
    {
      int num = pedometerSyncTask == null || !pedometerSyncTask.IsCompleted ? (!pedometerSyncProgress.IsSyncing ? 1 : 0) : 1;
      bool flag = bandSyncTask != null && bandSyncTask.IsCompleted || !bandSyncProgress.IsSyncing;
      if (num != 0 && (pedometerSyncTask == null || pedometerSyncTask.Status == TaskStatus.RanToCompletion) || flag && (bandSyncTask == null || bandSyncTask.Status == TaskStatus.RanToCompletion))
        this.anySucceeded = true;
      double percentComplete = (bandSyncProgress.PercentComplete + pedometerSyncProgress.PercentComplete) / 2.0;
      this.ReportProgressChanged(progressReporter, bandSyncProgress.Stage, percentComplete);
    }

    private void ReportProgressChanged(
      IProgress<DeviceSyncProgress> progressReporter,
      ProgressStage stage,
      double percentComplete)
    {
      this.LastReportedSyncState = new SyncStateChangedEventArgs(this.IsSyncing, stage, percentComplete);
      EventHandler<SyncStateChangedEventArgs> syncStateChanged = this.SyncStateChanged;
      if (syncStateChanged != null)
        syncStateChanged((object) this, this.LastReportedSyncState);
      progressReporter?.Report(new DeviceSyncProgress(this.IsSyncing, stage, percentComplete));
    }

    private async Task<IDevice> GetPedometerDeviceAsync(CancellationToken token) => !await this.pedometerManager.IsAvailableAsync(token).ConfigureAwait(false) ? (IDevice) null : this.serviceLocator.GetInstance<IDevice>("Pedometer");

    private IDevice GetBandDevice()
    {
      if (!this.configurationService.GetValue<bool>(DeviceManager.IsBandAvailableValue))
        return (IDevice) null;
      return !this.userProfileService.IsRegisteredBandPaired ? (IDevice) null : this.serviceLocator.GetInstance<IDevice>("Band");
    }
  }
}
