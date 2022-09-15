// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.FirmwareUpdateService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class FirmwareUpdateService : IFirmwareUpdateService, IAppUpgradeListener
  {
    private const string FirmwareUpdateInfoFileName = "CachedFirmwareUpdateInfo.json";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\FirmwareUpdateService.cs");
    private static readonly TimeSpan WaitInterval = TimeSpan.FromMilliseconds(30000.0);
    private static readonly TimeSpan ProgressInterval = TimeSpan.FromMinutes(2.0);
    private readonly IEnvironmentService applicationEnvironmentService;
    private readonly IConfig config;
    private readonly IFileObjectStorageService isoObjectStore;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IMutexService mutexService;
    private readonly IServiceLocator serviceLocator;
    private readonly Mutex checkForFirmwareUpdateMutex;
    private UpdateInfo updateInfo;

    public FirmwareUpdateService(
      IBandConnectionFactory cargoConnectionFactory,
      IFileObjectStorageService isoObjectStore,
      IConfig config,
      IEnvironmentService applicationEnvironmentService,
      IMutexService mutexService,
      IServiceLocator serviceLocator)
    {
      Assert.ParamIsNotNull((object) cargoConnectionFactory, nameof (cargoConnectionFactory));
      Assert.ParamIsNotNull((object) isoObjectStore, nameof (isoObjectStore));
      Assert.ParamIsNotNull((object) config, nameof (config));
      Assert.ParamIsNotNull((object) applicationEnvironmentService, nameof (applicationEnvironmentService));
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.isoObjectStore = isoObjectStore;
      this.config = config;
      this.applicationEnvironmentService = applicationEnvironmentService;
      this.mutexService = mutexService;
      this.serviceLocator = serviceLocator;
      this.checkForFirmwareUpdateMutex = this.mutexService.GetNamedMutex(false, "KApp.FirmwareUpdateCheck");
    }

    private static bool IsValid(UpdateInfo updateInfo) => updateInfo != null && updateInfo.LastUpdateTime.Date == DateTime.Today;

    private async Task SetCacheFirmwareUpdateInfoAsync(
      UpdateInfo updateInfoParam,
      CancellationToken token)
    {
      try
      {
        FirmwareUpdateService.Logger.Debug((object) "<START> save firmware update info to isolated storage");
        await this.isoObjectStore.WriteObjectAsync((object) updateInfoParam, "CachedFirmwareUpdateInfo.json", token).ConfigureAwait(false);
        FirmwareUpdateService.Logger.Debug((object) "<END> save firmware update info to isolated storage");
      }
      catch (Exception ex)
      {
        FirmwareUpdateService.Logger.Error(ex, "<FAILED> save firmware update info to isolated storage");
      }
    }

    private async Task<UpdateInfo> GetCachedFirmwareUpdateInfoAsync()
    {
      UpdateInfo updateInfoLocal = (UpdateInfo) null;
      try
      {
        FirmwareUpdateService.Logger.Debug((object) "<START> check cached firmware update info");
        updateInfoLocal = await this.isoObjectStore.ReadObjectAsync<UpdateInfo>("CachedFirmwareUpdateInfo.json", CancellationTokenUtilities.FromTimeout(FirmwareUpdateService.WaitInterval)).ConfigureAwait(false);
        FirmwareUpdateService.Logger.Debug((object) "<END> check cached firmware update info");
      }
      catch (Exception ex)
      {
        FirmwareUpdateService.Logger.Error(ex, "<FAILED> check cached firmware update info");
      }
      return updateInfoLocal;
    }

    private async Task SignalBackgroundToToastNextFailureAsync() => await this.isoObjectStore.WriteObjectAsync((object) false, "IsFirmwareMessageSent", CancellationTokenUtilities.FromTimeout(FirmwareUpdateService.WaitInterval)).ConfigureAwait(false);

    public async Task<bool> CheckForFirmwareUpdateAsync(
      CancellationToken cancellationToken,
      bool forceCloudCheck = false)
    {
      bool isFirmwareUpdateRequired = false;
      if (this.config.IsFirmwareUpdateCheckingEnabled)
        await this.checkForFirmwareUpdateMutex.RunSynchronizedAsync((Func<Task>) (async () =>
        {
          FirmwareUpdateService.Logger.Debug((object) "<START> Checking if there is a firmware update available for the device");
          cancellationToken.ThrowIfCancellationRequested();
          bool singleDevicePolicyEnforced = forceCloudCheck;
          if (forceCloudCheck)
          {
            FirmwareUpdateService.Logger.Debug((object) "<FLAG> clearing firmware update cache, will download from cloud");
            await this.ClearCachedFirmwareUpdateInfoAsync(cancellationToken).ConfigureAwait(false);
          }
          if (this.updateInfo != null && FirmwareUpdateService.IsValid(this.updateInfo))
          {
            FirmwareUpdateService.Logger.Debug("<FLAG> using in-memory firmware update info (result:{0})", (object) this.updateInfo.IsFirmwareUpdateAvailable);
            isFirmwareUpdateRequired = this.updateInfo.IsFirmwareUpdateAvailable;
          }
          else
          {
            FirmwareUpdateService firmwareUpdateService = default;
            UpdateInfo updateInfo5 = firmwareUpdateService.updateInfo;
            ConfiguredTaskAwaitable<UpdateInfo> configuredTaskAwaitable = this.GetCachedFirmwareUpdateInfoAsync().ConfigureAwait(false);
            UpdateInfo updateInfo6 = await configuredTaskAwaitable;
            firmwareUpdateService.updateInfo = updateInfo6;
            firmwareUpdateService = (FirmwareUpdateService) null;
            if (!forceCloudCheck && FirmwareUpdateService.IsValid(this.updateInfo))
            {
              FirmwareUpdateService.Logger.Debug("<FLAG> using cached firmware update info (result:{0})", (object) this.updateInfo.IsFirmwareUpdateAvailable);
              isFirmwareUpdateRequired = this.updateInfo.IsFirmwareUpdateAvailable;
              singleDevicePolicyEnforced = true;
            }
            else
            {
              if (!singleDevicePolicyEnforced)
              {
                cancellationToken.ThrowIfCancellationRequested();
                singleDevicePolicyEnforced = true;
              }
              FirmwareUpdateService.Logger.Debug((object) "<FLAG> fetching new firmware update info from cloud");
              using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
              {
                firmwareUpdateService = this;
                UpdateInfo updateInfo7 = firmwareUpdateService.updateInfo;
                configuredTaskAwaitable = cargoConnection.GetLatestAvailableFirmwareVersionAsync(cancellationToken).ConfigureAwait(false);
                UpdateInfo updateInfo8 = await configuredTaskAwaitable;
                firmwareUpdateService.updateInfo = updateInfo8;
                firmwareUpdateService = (FirmwareUpdateService) null;
                if (!Telemetry.IsFirmwareVersionSet())
                  await FirmwareUpdateService.SetFirmwareVersionForTelemetryAsync(cancellationToken, cargoConnection);
              }
              await this.SetCacheFirmwareUpdateInfoAsync(this.updateInfo, cancellationToken).ConfigureAwait(false);
              FirmwareUpdateService.Logger.Debug("<END> checking if there is a firmware update available for the device (result:{0})", (object) this.updateInfo.IsFirmwareUpdateAvailable);
              isFirmwareUpdateRequired = this.updateInfo.IsFirmwareUpdateAvailable;
            }
          }
          if (isFirmwareUpdateRequired && !singleDevicePolicyEnforced)
            cancellationToken.ThrowIfCancellationRequested();
          if (Telemetry.IsFirmwareVersionSet())
            return;
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
            await FirmwareUpdateService.SetFirmwareVersionForTelemetryAsync(cancellationToken, cargoConnection);
        }), cancellationToken).ConfigureAwait(false);
      return isFirmwareUpdateRequired;
    }

    private static async Task SetFirmwareVersionForTelemetryAsync(
      CancellationToken cancellationToken,
      IBandConnection cargoConnection)
    {
      FirmwareVersions firmwareVersionAsync = await cargoConnection.GetBandFirmwareVersionAsync(cancellationToken);
      if (firmwareVersionAsync == null)
        return;
      Telemetry.SetFirmwareVersion(firmwareVersionAsync.ApplicationVersion.ToString(true));
      Telemetry.SetBandVersion(firmwareVersionAsync.PcbId.ToString());
    }

    public async Task ClearCachedFirmwareUpdateInfoAsync(CancellationToken token)
    {
      FirmwareUpdateService.Logger.Debug((object) "<START> clearing cached firmware update info");
      this.updateInfo = (UpdateInfo) null;
      await this.SetCacheFirmwareUpdateInfoAsync((UpdateInfo) null, token).ConfigureAwait(false);
      FirmwareUpdateService.Logger.Debug((object) "<END> clearing cached firmware update info");
    }

    public async Task UpdateFirmwareAsync(
      bool isOobeUpdate,
      CancellationToken cancellationToken,
      IProgress<FirmwareUpdateProgressReport> firmwareUpdateProgressReport = null)
    {
      FirmwareUpdateService.Logger.Debug((object) "<START> Updating the firmware on the device");
      cancellationToken.ThrowIfCancellationRequested();
      ISmoothNavService smoothNavService = this.serviceLocator.GetInstance<ISmoothNavService>();
      smoothNavService.DisableNavPanel(typeof (IFirmwareUpdateService));
      await this.SignalBackgroundToToastNextFailureAsync().ConfigureAwait(false);
      await this.ClearCachedFirmwareUpdateInfoAsync(cancellationToken).ConfigureAwait(false);
      try
      {
        FirmwareUpdateService.FirmwareUpdateProgressTelemetry progress = null;
        int num = 0;
        if (num != 2 && num != 3)
        {
          this.applicationEnvironmentService.SuspendApplicationWhenIdle = false;
          this.config.IsNavigatedAwayDuringFirmwareUpdatePromptNeeded = true;
          progress = new FirmwareUpdateService.FirmwareUpdateProgressTelemetry(isOobeUpdate, firmwareUpdateProgressReport);
        }
        try
        {
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
            await cargoConnection.UpdateFirmwareAsync(cancellationToken, (IProgress<FirmwareUpdateProgressReport>) progress).ConfigureAwait(false);
        }
        finally
        {
          progress?.Dispose();
        }
        progress = (FirmwareUpdateService.FirmwareUpdateProgressTelemetry) null;
      }
      catch (DeactivatedException ex)
      {
        throw new FirmwareUpdateInterruptedException();
      }
      finally
      {
        this.config.IsNavigatedAwayDuringFirmwareUpdatePromptNeeded = false;
        smoothNavService.DisableNavPanel(typeof (IFirmwareUpdateService));
        this.applicationEnvironmentService.SuspendApplicationWhenIdle = true;
      }
      cancellationToken.ThrowIfCancellationRequested();
      FirmwareUpdateService.Logger.Debug((object) "<END> Updating the firmware on the device");
    }

    async Task IAppUpgradeListener.OnAppUpgradeAsync(
      Version newVersion,
      Version oldVersion,
      CancellationToken token)
    {
      FirmwareUpdateService.Logger.Debug((object) "<START> enforcing clear cache on app upgrade policy");
      await this.ClearCachedFirmwareUpdateInfoAsync(token).ConfigureAwait(false);
      FirmwareUpdateService.Logger.Debug((object) "<END> enforcing clear cache on app upgrade policy");
    }

    private sealed class FirmwareUpdateProgressTelemetry : 
      IProgress<FirmwareUpdateProgressReport>,
      IDisposable
    {
      private readonly IProgress<FirmwareUpdateProgressReport> clientProgress;
      private readonly bool isOobeUpdate;
      private readonly object telemetryLock = new object();
      private FirmwareUpdateState currentState;
      private ITimedTelemetryEvent stateTimedEvent;
      private ITimedTelemetryEvent updateTimedEvent;
      private bool isDisposed;

      public FirmwareUpdateProgressTelemetry(
        bool isOobeUpdate,
        IProgress<FirmwareUpdateProgressReport> clientProgress)
      {
        this.isOobeUpdate = isOobeUpdate;
        this.clientProgress = clientProgress;
      }

      public void Report(FirmwareUpdateProgressReport value)
      {
        bool lockTaken = false;
        try
        {
          FirmwareUpdateService.Logger.Debug((object) "Report - Aquiring firmware telemetry lock.");
          Monitor.TryEnter(this.telemetryLock, FirmwareUpdateService.ProgressInterval, ref lockTaken);
          if (lockTaken)
          {
            FirmwareUpdateService.Logger.Debug((object) "Report - Aquiring firmware telemetry lock succeeded.");
            if (!this.isDisposed)
            {
              if (this.currentState != FirmwareUpdateState.Done && this.currentState != value.FirmwareUpdateState)
              {
                this.currentState = value.FirmwareUpdateState;
                this.HandleStateChange(this.currentState);
              }
              this.clientProgress?.Report(value);
            }
            FirmwareUpdateService.Logger.Debug((object) string.Format("Report - Firmware telemetry disposed = {0}", new object[1]
            {
              (object) this.isDisposed
            }));
          }
          else
          {
            FirmwareUpdateService.Logger.Debug((object) string.Format("Report - Aquiring firmware telemetry lock failed with state = {0}.", new object[1]
            {
              (object) this.currentState
            }));
            Debugger.Break();
          }
        }
        catch (Exception ex)
        {
          FirmwareUpdateService.Logger.Error((object) "Report - Failed to complete firmware telemetry report.", ex);
        }
        finally
        {
          if (lockTaken)
          {
            FirmwareUpdateService.Logger.Debug((object) "Report - Released firmware telemetry lock.");
            Monitor.Exit(this.telemetryLock);
          }
        }
      }

      public void Dispose()
      {
        FirmwareUpdateService.Logger.Debug((object) "Dispose - Aquiring firmware telemetry lock.");
        lock (this.telemetryLock)
        {
          FirmwareUpdateService.Logger.Debug((object) "Dispose - Aquiring firmware telemetry lock succeeded.");
          FirmwareUpdateService.Logger.Debug((object) string.Format("Dispose - Firmware telemetry disposed = {0}", new object[1]
          {
            (object) this.isDisposed
          }));
          if (this.isDisposed)
            return;
          if (this.updateTimedEvent != null)
          {
            FirmwareUpdateService.Logger.Debug((object) "Dispose - Cancelling update timed event.");
            this.updateTimedEvent.Cancel();
            FirmwareUpdateService.Logger.Debug((object) "Dispose - Cancelling update timed event succeeded.");
            FirmwareUpdateService.Logger.Debug((object) "Dispose - Disposing update timed event.");
            this.updateTimedEvent.Dispose();
            FirmwareUpdateService.Logger.Debug((object) "Dispose - Disposing update timed event succeeded.");
            this.updateTimedEvent = (ITimedTelemetryEvent) null;
          }
          if (this.stateTimedEvent != null)
          {
            FirmwareUpdateService.Logger.Debug((object) "Dispose - Cancelling state timed event.");
            this.stateTimedEvent.Cancel();
            FirmwareUpdateService.Logger.Debug((object) "Dispose - Cancelling state timed event succeeded.");
            FirmwareUpdateService.Logger.Debug((object) "Dispose - Disposing state timed event.");
            this.stateTimedEvent.Dispose();
            FirmwareUpdateService.Logger.Debug((object) "Dispose - Disposing state timed event succeeded.");
            this.stateTimedEvent = (ITimedTelemetryEvent) null;
          }
          this.isDisposed = true;
        }
      }

      private void HandleStateChange(FirmwareUpdateState state)
      {
        try
        {
          if (this.updateTimedEvent == null && !this.isOobeUpdate)
            this.updateTimedEvent = CommonTelemetry.TimeFirmwareUpdate();
          if (this.stateTimedEvent != null)
          {
            this.stateTimedEvent.End();
            this.stateTimedEvent.Dispose();
            this.stateTimedEvent = (ITimedTelemetryEvent) null;
          }
          switch (state)
          {
            case FirmwareUpdateState.DownloadingUpdate:
              this.stateTimedEvent = CommonTelemetry.TimeFirmwareUpdateDownload(this.isOobeUpdate);
              break;
            case FirmwareUpdateState.SyncingLog:
              break;
            case FirmwareUpdateState.BootingToUpdateMode:
              break;
            case FirmwareUpdateState.SendingUpdateToDevice:
              this.stateTimedEvent = CommonTelemetry.TimeFirmwareUpdateSendToBand(this.isOobeUpdate);
              break;
            case FirmwareUpdateState.WaitingtoConnectAfterUpdate:
              this.stateTimedEvent = CommonTelemetry.TimeFirmwareUpdateRebootBand(this.isOobeUpdate);
              break;
            case FirmwareUpdateState.Done:
              if (this.updateTimedEvent == null)
                break;
              this.updateTimedEvent.End();
              this.updateTimedEvent.Dispose();
              this.updateTimedEvent = (ITimedTelemetryEvent) null;
              break;
            default:
              DebugUtilities.Fail("Received unrecognized FW update state: {0}", (object) state);
              break;
          }
        }
        catch (Exception ex)
        {
          FirmwareUpdateService.Logger.Error(ex, "Failed to complete telemetry transition for state={0}.", (object) state);
        }
      }
    }

    public static class StateKey
    {
      public const string FirmwareUpdateActive = "FirmwareUpdateActive";
    }
  }
}
