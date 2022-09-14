// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.BandConnectionBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Band.Admin;
using Microsoft.Band.Personalization;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.Diagnostics;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Bluetooth;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.Utilities.Logging;
using Microsoft.Health.Cloud.Client.Authentication;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Band
{
  public abstract class BandConnectionBase<TClient> : IBandConnection, IDisposable
    where TClient : class
  {
    private const int MaxWeatherLocationLength = 20;
    protected const string CloudFileUploadIdFormat = "yyyyMMddHHmmssfff";
    protected static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Band\\BandConnectionBase.cs");
    protected static readonly IActivityManager ActivityManager = BandConnectionBase<TClient>.Logger.CreateActivityManager();
    private readonly IList<CancellationTokenSource> aggregateCancellationTokenSources;
    private readonly IBandConnectionSynchronizationService synchronizationService;
    private readonly IConnectionInfoProvider connectionInfoProvider;
    private readonly IBandInfoService bandInfoService;
    private readonly IMicrosoftBandUserAgentService microsoftBandUserAgentService;
    private readonly IPolicyEnforcingBandClientFactory<TClient> cargoClientFactory;
    private readonly IBluetoothService bluetoothService;
    private readonly IEnvironmentService environmentService;
    private readonly IPerfLogger perfLogger;
    private readonly IConfig config;
    private readonly ITilePageElementFactory tilePageElementFactory;
    private readonly object disposeLock = new object();
    private bool isDisposed;
    private CancellationTokenSource suspendTriggeredCancellationTokenSource;
    private Task runningUsingCargoClientTask;
    private Mutex clientMutex;
    private Mutex commandMutex;

    protected TClient CargoClient { get; set; }

    protected IUserProfile UserProfile { get; set; }

    protected bool IsPublicRelease => this.environmentService.IsPublicRelease;

    public bool IsBackground { get; set; }

    public bool IsDisposed => this.isDisposed;

    protected IEnumerable<CalendarEvent> LastSentCalendarEvents { get; set; }

    protected BandConnectionBase
    (
      IBandConnectionSynchronizationService synchronizationService,
      IMicrosoftBandUserAgentService microsoftBandUserAgentService,
      IEnvironmentService environmentService,
      IBandInfoService bandInfoService,
      IConnectionInfoProvider connectionInfoProvider,
      IPolicyEnforcingBandClientFactory<TClient> cargoClientFactory,
      IBluetoothService bluetoothService,
      IMutexService mutexProvider,
      IPerfLogger perfLogger,
      IConfig config,
      ITilePageElementFactory tilePageElementFactory
    )
    {
      this.synchronizationService = synchronizationService;
      this.microsoftBandUserAgentService = microsoftBandUserAgentService;
      this.environmentService = environmentService;
      this.bandInfoService = bandInfoService;
      this.connectionInfoProvider = connectionInfoProvider;
      this.cargoClientFactory = cargoClientFactory;
      this.bluetoothService = bluetoothService;
      this.perfLogger = perfLogger;
      this.config = config;
      this.tilePageElementFactory = tilePageElementFactory;
      this.clientMutex = mutexProvider.GetNamedMutex(false, "KApp.cargoClientConnection");
      this.commandMutex = mutexProvider.GetNamedMutex(false, "KApp.cargoConnection.Command");
      this.suspendTriggeredCancellationTokenSource = new CancellationTokenSource();
      this.aggregateCancellationTokenSources = (IList<CancellationTokenSource>) new List<CancellationTokenSource>();
    }

    public Task CheckConnectionWorkingAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Checking if the Band connection is working"), (Func<Task>) (() => (Task) this.RunUsingCargoClientAsync<bool>((Func<TClient, CancellationToken, Task<bool>>) ((cargoClient, cancellationTokenParam) => Task.FromResult<bool>(true)), cancellationToken)));

    public Task<bool> TryCheckConnectionWorkingAsync() => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<bool>(Level.Info, (Func<string>) (() => "Try check if connection to Band is working"), (Func<Task<bool>>) (async () =>
    {
      try
      {
        await this.CheckConnectionWorkingAsync(CancellationToken.None).ConfigureAwait(false);
        return true;
      }
      catch (Exception ex)
      {
        BandConnectionBase<TClient>.Logger.Warn((object) ex);
        return false;
      }
    }), true);

    protected Task<T> RunUsingCargoClientAsync<T>(
      Func<TClient, CancellationToken, Task<T>> func,
      CancellationToken cancellationToken,
      BandClientType clientType = BandClientType.Both,
      bool forceCloudProfile = false,
      bool ignoreCorruptFirmware = false)
    {
      CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, this.suspendTriggeredCancellationTokenSource.Token);
      CancellationToken aggregateCancellationToken = linkedTokenSource.Token;
      this.aggregateCancellationTokenSources.Add(linkedTokenSource);
      T returnVar = default (T);
      return this.RunWithExceptionsAfterCancelledAsDeactivatedExceptionsAsync<T>((Func<Task<T>>) (() =>
      {
        aggregateCancellationToken.ThrowIfCancellationRequested();
        if (!this.IsBackground)
        {
          BandConnectionBase<TClient>.Logger.Debug((object) "signal cancel background agent");
          this.synchronizationService.CancelBackgroundSyncTask();
        }
        BandConnectionBase<TClient>.Logger.Debug((object) "<START> attempting to acquire bluetooth channel mutex");
        return this.clientMutex.RunSynchronizedAsync<T>((Func<Task<T>>) (() =>
        {
          BandConnectionBase<TClient>.Logger.Debug((object) "<END> attempting to acquire bluetooth channel mutex");
          Task<T> task = ((Func<Task<T>>) (async () =>
          {
            BandConnectionBase<TClient> bandConnectionBase = this;
            TClient client = await this.ConnectAsync(aggregateCancellationToken, clientType, forceCloudProfile, ignoreCorruptFirmware).ConfigureAwait(false);
            bandConnectionBase.CargoClient = client;
            bandConnectionBase = (BandConnectionBase<TClient>) null;
            try
            {
              aggregateCancellationToken.ThrowIfCancellationRequested();
              BandConnectionBase<TClient>.Logger.Debug((object) "<START> BandConnectionBase action");

                  //RnD
              // ISSUE: variable of a compiler-generated type
              //BandConnectionBase<TClient>.\u003C\u003Ec__DisplayClass44_0<T> cDisplayClass440 = this;
              // ISSUE: reference to a compiler-generated field
              //T returnVar4 = cDisplayClass440.returnVar;
              T obj = await func(this.CargoClient, aggregateCancellationToken).ConfigureAwait(false);
              // ISSUE: reference to a compiler-generated field
              //cDisplayClass440.returnVar = obj;
              //cDisplayClass440 = (BandConnectionBase<TClient>.\u003C\u003Ec__DisplayClass44_0<T>) null;
              BandConnectionBase<TClient>.Logger.Debug((object) "<END> BandConnectionBase action");
            }
            finally
            {
              this.CloseClientSession();
            }
            return returnVar;
          }))();
          this.runningUsingCargoClientTask = (Task) task;
          return task;
        }), aggregateCancellationToken);
      }), this.suspendTriggeredCancellationTokenSource.Token);
    }

    protected Task<T> RunUsingCargoClientAsync<T>(
      Func<TClient, CancellationToken, Task<T>> func,
      BandClientType clientType = BandClientType.Both)
    {
      return this.RunUsingCargoClientAsync<T>(func, CancellationToken.None, clientType);
    }

    protected Task RunUsingCargoClientAsync(
      Func<TClient, CancellationToken, Task> func,
      CancellationToken cancellationToken,
      BandClientType clientType = BandClientType.Both,
      bool forceCloudProfile = false)
    {
      return (Task) this.RunUsingCargoClientAsync<bool>((Func<TClient, CancellationToken, Task<bool>>) (async (cargoClient, cancellationTokenParam) =>
      {
        await func(cargoClient, cancellationTokenParam).ConfigureAwait(false);
        return true;
      }), cancellationToken, clientType, forceCloudProfile);
    }

    protected Task RunUsingCargoClientAsync(
      Func<TClient, CancellationToken, Task> func,
      BandClientType clientType = BandClientType.Both)
    {
      return this.RunUsingCargoClientAsync(func, CancellationToken.None, clientType);
    }

    protected async Task<T> RunWithExceptionsAfterCancelledAsDeactivatedExceptionsAsync<T>(
      Func<Task<T>> func,
      CancellationToken cancellationToken)
    {
      T obj;
      try
      {
        obj = await func().ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        if (cancellationToken.IsCancellationRequested)
          throw new DeactivatedException("the phone application was deactivated during a cargo client operation", ex);
        throw;
      }
      return obj;
    }

    private async Task<TClient> ConnectAsync(
      CancellationToken cancellationToken,
      BandClientType clientType,
      bool forceCloudProfile = false,
      bool ignoreCorruptFirmware = false)
    {
      BandConnectionBase<TClient>.Logger.Debug((object) "<START> Connecting BandConnectionBase");
      cancellationToken.ThrowIfCancellationRequested();
      if ((object) this.CargoClient != null)
      {
        AlreadyConnectedException connectedException = new AlreadyConnectedException("attempting to connect to CargoClient when it is already in use");
        BandConnectionBase<TClient>.Logger.Error((object) connectedException);
        throw connectedException;
      }
      IBandInfo bandInfoAsync = await this.bandInfoService.GetBandInfoAsync(cancellationToken);
      return await this.cargoClientFactory.CreateClientAsync(clientType, cancellationToken, bandInfoAsync, forceCloudProfile, !this.IsBackground, ignoreCorruptFirmware).ConfigureAwait(false);
    }

    public async Task NotifyOfSuspendAsync(CancellationToken token)
    {
      BandConnectionBase<TClient>.Logger.Debug((object) "<START> cargo connection suspending");
      CancellationTokenSource cancellationTokenSource = (CancellationTokenSource) null;
      Task task1 = (Task) null;
      bool isDisposed;
      lock (this.disposeLock)
      {
        isDisposed = this.isDisposed;
        if (!isDisposed)
        {
          cancellationTokenSource = this.suspendTriggeredCancellationTokenSource;
          task1 = this.runningUsingCargoClientTask;
        }
      }
      if (!isDisposed)
      {
        BandConnectionBase<TClient>.Logger.Debug((object) "<FLAG> sending cancel due to suspend notification");
        cancellationTokenSource.Cancel();
        if (task1 != null)
        {
          try
          {
            BandConnectionBase<TClient>.Logger.Debug((object) "<START> waiting for any running tasks to cancel before disposing");
            Task cancelTask = Task.Delay(-1, token);
            Task task2 = await Task.WhenAny(task1, cancelTask);
            if (task2 == this.runningUsingCargoClientTask)
              BandConnectionBase<TClient>.Logger.Debug("<END> waiting for any running tasks to cancel before disposing (result={0})", (object) "running task completed");
            if (task2 == cancelTask)
              BandConnectionBase<TClient>.Logger.Debug("<END> waiting for any running tasks to cancel before disposing (result={0})", (object) "max wait time reached");
            cancelTask = (Task) null;
          }
          catch (Exception ex)
          {
            BandConnectionBase<TClient>.Logger.Warn(ex, "<END> waiting for any running tasks to cancel before disposing");
          }
        }
        this.CloseClientSession();
      }
      else
        BandConnectionBase<TClient>.Logger.Debug((object) "<FLAG> cargo connection already disposed");
      BandConnectionBase<TClient>.Logger.Debug((object) "<END> cargo connection suspending");
    }

    public void NotifyOfResume()
    {
      BandConnectionBase<TClient>.Logger.Debug((object) "<START> cargo connection activating");
      if (!this.isDisposed)
        this.suspendTriggeredCancellationTokenSource = new CancellationTokenSource();
      else
        BandConnectionBase<TClient>.Logger.Debug((object) "<FLAG> cargo connection already disposed");
      BandConnectionBase<TClient>.Logger.Debug((object) "<END> cargo connection activating");
    }

    public virtual void Dispose()
    {
      lock (this.disposeLock)
      {
        if (this.isDisposed)
          return;
        this.CloseClientSession();
        this.UserProfile = (IUserProfile) null;
        this.runningUsingCargoClientTask = (Task) null;
        this.suspendTriggeredCancellationTokenSource = (CancellationTokenSource) null;
        foreach (CancellationTokenSource cancellationTokenSource in this.aggregateCancellationTokenSources.Where<CancellationTokenSource>((Func<CancellationTokenSource, bool>) (aggregateCancellationTokenSource => aggregateCancellationTokenSource != null)))
          cancellationTokenSource.Dispose();
        this.isDisposed = true;
      }
    }

    public Task<IBandInfo[]> GetPairedBandsAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<IBandInfo[]>(Level.Debug, (Func<string>) (() => "Getting Paired Bands"), (Func<Task<IBandInfo[]>>) (() => this.bluetoothService.GetPairedBandsAsync(cancellationToken)), true);

    public Task<IBandInfo> GetPrimaryPairedBandAsync(
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<IBandInfo>(Level.Debug, (Func<string>) (() => "Getting primary paired Band"), (Func<Task<IBandInfo>>) (async () =>
      {
        IBandInfo bandInfo = (IBandInfo) null;
        try
        {
          IBandInfo[] bandInfoArray = await this.GetPairedBandsAsync(cancellationToken).ConfigureAwait(false);
          if (bandInfoArray != null)
          {
            if (bandInfoArray.Length != 0)
              bandInfo = ((IEnumerable<IBandInfo>) bandInfoArray).First<IBandInfo>();
          }
        }
        catch (Exception ex)
        {
          int hresult = ex.HResult;
          BandConnectionBase<TClient>.Logger.Error(ex, "Exception encountered while getting connected bands");
          throw;
        }
        return bandInfo;
      }), true);
    }

    public Task<BandUserProfile> GetUserProfileAsync(
      Func<IUserProfile, Task<BandUserProfile>> profileConversion,
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<BandUserProfile>(Level.Info, (Func<string>) (() => "Getting user profile from Band"), (Func<Task<BandUserProfile>>) (async () =>
      {
        IUserProfile userProfile = await this.GetUserProfileInternalAsync(cancellationToken).ConfigureAwait(false);
        this.UserProfile = userProfile != null ? userProfile : throw new Exception("cargo client get user profile should not return null");
        return await profileConversion(userProfile).ConfigureAwait(false);
      }), !this.IsPublicRelease);
    }

    public Task<FirmwareVersions> GetBandFirmwareVersionAsync(
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<FirmwareVersions>(Level.Info, (Func<string>) (() => "Getting firmware version from Band"), (Func<Task<FirmwareVersions>>) (async () =>
      {
        return await this.GetBandFirmwareVersionInternalAsync(cancellationToken).ConfigureAwait(false) ?? throw new Exception("cargo client firmware versions property should not be null");
      }), true);
    }

    public Task<string> GetProductSerialNumberAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<string>(Level.Info, (Func<string>) (() => "Getting serial number from Band"), (Func<Task<string>>) (async () =>
    {
      return await this.GetProductSerialNumberInternalAsync(cancellationToken).ConfigureAwait(false) ?? throw new Exception("cargo client get product serial number should not return null");
    }), !this.IsPublicRelease);

    public Task<DiagnosticsBandDevice> GetDiagnosticsInfoAsync(
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<DiagnosticsBandDevice>(Level.Info, (Func<string>) (() => "Getting diagnostics info from Band"), (Func<Task<DiagnosticsBandDevice>>) (() => this.GetDiagnosticsInfoInternalAsync(cancellationToken)), !this.IsPublicRelease);
    }

    public Task<IList<AdminBandTile>> GetDefaultTilesAsync(
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<IList<AdminBandTile>>(Level.Info, (Func<string>) (() => "Getting default tiles from Band"), (Func<Task<IList<AdminBandTile>>>) (() => this.GetDefaultTilesInternalAsync(cancellationToken)), true);
    }

    public Task<StartStrip> GetStartStripAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<StartStrip>(Level.Info, (Func<string>) (() => "Getting start strip from Band"), (Func<Task<StartStrip>>) (() => this.GetStartStripInternalAsync(cancellationToken)), true);

    public Task<StartStrip> GetStartStripWithoutImagesAsync(
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<StartStrip>(Level.Info, (Func<string>) (() => "Getting start strip from Band without images"), (Func<Task<StartStrip>>) (() => this.GetStartStripWithoutImagesInternalAsync(cancellationToken)), true);
    }

    public Task<BandTheme> GetBandThemeAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<BandTheme>(Level.Info, (Func<string>) (() => "Getting device theme from Band"), (Func<Task<BandTheme>>) (() => this.GetBandThemeInternalAsync(cancellationToken)), true);

    public Task SetStartStripAsync(StartStrip startStrip, CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting the start strip on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "StartStrip",
        (object) startStrip
      }
    }), (Func<Task>) (() => this.SetStartStripInternalAsync(startStrip, cancellationToken)));

    public Task PersonalizeBandAsync(
      uint imageId,
      CancellationToken cancellationToken,
      StartStrip startStrip = null,
      BandImage image = null,
      BandTheme color = null,
      IDictionary<Guid, BandTheme> customColors = null)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting user personalization information on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "ImageId",
          (object) imageId
        },
        {
          "StartStrip",
          (object) startStrip
        },
        {
          "BandImage",
          (object) image
        },
        {
          "BandTheme",
          (object) color
        },
        {
          "CustomColors",
          (object) customColors
        }
      }), (Func<Task>) (() => this.PersonalizeBandInternalAsync(imageId, cancellationToken, startStrip, image, color, customColors)));
    }

    public Task NavigateToScreenAsync(CargoScreen screen, CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Navigating Band to screen"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "CargoScreen",
        (object) screen
      }
    }), (Func<Task>) (() => this.NavigateToScreenInternalAsync(screen, cancellationToken)));

    public Task SetOobeStageAsync(OobeStage stage, CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Navigating Band to screen"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "OobeStage",
        (object) stage
      }
    }), (Func<Task>) (() => this.SetOobeStageInternalAsync(stage, cancellationToken)));

    public Task FinalizeOobeAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Finalizing OOBE"), (Func<Task>) (() => this.FinalizeOobeInternalAsync(cancellationToken)));

    public Task SaveCloudUserProfileAsync(Func<IUserProfile, Task> userProfileModifications) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Uploading the user profile to the Health Cloud"), (Func<Task>) (() => this.SaveCloudUserProfileInternalAsync(userProfileModifications)));

    public Task SaveUserProfileAsync(
      Func<IUserProfile, Task> userProfileModifications,
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting user profile on the band and uploading to Health Cloud"), (Func<Task>) (() => this.SaveUserProfileInternalAsync(userProfileModifications, cancellationToken)));
    }

    public Task<string> UploadFileToCloudAsync(
      IFile file,
      LogFileTypes fileType,
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<string>(Level.Info, (Func<string>) (() => "Uploading file to cloud"), (Func<Task<string>>) (() => this.UploadFileToCloudInternalAsync(file, fileType, cancellationToken)), true);
    }

    public Task SendCalendarEventsAsync(
      IEnumerable<CalendarEvent> calendarEvents,
      CancellationToken cancellationToken)
    {
      Dictionary<string, object> activityParameters = new Dictionary<string, object>();
      if (!this.IsPublicRelease)
        activityParameters.Add("CalendarEvents", (object) calendarEvents);
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending calendar updates to the band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) activityParameters), (Func<Task>) (() => this.SendCalendarEventsInternalAsync(calendarEvents, cancellationToken)));
    }

    public Task SetSleepNotificationAsync(
      SleepNotification sleepNotification,
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending sleep notification to the band"), (Func<Task>) (() => this.SetSleepNotificationInternalAsync(sleepNotification, cancellationToken)));
    }

    public Task DisableSleepNotificationAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Disabling sleep notification on the band"), (Func<Task>) (() => this.DisableSleepNotificationInternalAsync(cancellationToken)));

    public Task SetLightExposureNotificationAsync(
      LightExposureNotification lightExposureNotification,
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending light exposure notification to the band"), (Func<Task>) (() => this.SetLightExposureNotificationInternalAsync(lightExposureNotification, cancellationToken)));
    }

    public Task DisableLightExposureNotificationAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Disabling light exposure notification on the band"), (Func<Task>) (() => this.DisableLightExposureNotificationInternalAsync(cancellationToken)));

    public Task SendFinanceUpdateNotificationsAsync(
      IEnumerable<Stock> stocks,
      DateTimeOffset timestamp,
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending finance updates to the band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "Stocks",
          (object) stocks
        },
        {
          "TimeStamp",
          (object) timestamp
        }
      }), (Func<Task>) (() => this.SendFinanceUpdateNotificationsInternalAsync(stocks, timestamp, cancellationToken)));
    }

    public Task SaveGoalsToBandAsync(Microsoft.Band.Admin.Goals goals) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting the user goals on the band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "Goals",
        (object) goals
      }
    }), (Func<Task>) (() => this.SaveGoalsToBandInternalAsync(goals)));

    public Task SendWeatherUpdateNotificationsAsync(
      IList<WeatherDay> dailyConditions,
      string location,
      DateTimeOffset timestamp,
      CancellationToken cancellationToken)
    {
      Dictionary<string, object> activityParameters = new Dictionary<string, object>();
      if (!this.IsPublicRelease)
      {
        activityParameters.Add("DailyConditions", (object) dailyConditions);
        activityParameters.Add("Location", (object) location);
        activityParameters.Add("TimeStamp", (object) timestamp);
      }
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending weather updates to the band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) activityParameters), (Func<Task>) (() => this.SendWeatherUpdateNotificationsInternalAsync(dailyConditions, location, timestamp, cancellationToken)));
    }

    public Task SendStarbucksUpdateNotificationsAsync(
      string cardNumber,
      CancellationToken cancellationToken)
    {
      Dictionary<string, object> activityParameters = new Dictionary<string, object>();
      if (!this.IsPublicRelease)
        activityParameters.Add("CardNumber", (object) cardNumber);
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending Starbucks updates to the band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) activityParameters), (Func<Task>) (() => this.SendStarbucksUpdateNotificationsInternalAsync(cardNumber, cancellationToken)));
    }

    public Task SendWorkoutToBandAsync(
      Stream workoutStream,
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending a guided workout to the Band"), (Func<Task>) (() => this.SendWorkoutToBandInternalAsync(workoutStream, cancellationToken)));
    }

    public Task SetWorkoutActivitiesAsync(
      IList<WorkoutActivity> activities,
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting workout activities on the Band"), (Func<Task>) (() => this.SetWorkoutActivitiesInternalAsync(activities, cancellationToken)));
    }

    public Task SendGolfCourseToBandAsync(
      Stream golfCourseStream,
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending a golf course to the Band"), (Func<Task>) (() => this.SendGolfCourseToBandInternalAsync(golfCourseStream, cancellationToken)));
    }

    public Task<BandUserProfile> GetCloudUserProfileAsync(
      Func<IUserProfile, Task<BandUserProfile>> profileConversion)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<BandUserProfile>(Level.Info, (Func<string>) (() => "Downloading user profile from the Health Cloud"), (Func<Task<BandUserProfile>>) (async () =>
      {
        IUserProfile userProfile = await this.GetCloudUserProfileInternalAsync().ConfigureAwait(false);
        this.UserProfile = userProfile != null ? userProfile : throw new Exception("cargo client get user profile should not return null");
        return await profileConversion(userProfile).ConfigureAwait(false);
      }), !this.IsPublicRelease);
    }

    public Task<bool> GetBandOobeCompletedAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<bool>(Level.Info, (Func<string>) (() => "Getting the OOBE completed status from the Band"), (Func<Task<bool>>) (() => this.GetBandOobeCompletedInternalAsync(cancellationToken)), true);

    public Task LinkBandToProfileAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Linking Band to Health Cloud profile"), (Func<Task>) (() => this.LinkBandToProfileInternalAsync(cancellationToken)));

    public Task UnlinkBandFromProfileAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Unlinking Band from Health Cloud profile"), (Func<Task>) (() => this.commandMutex.RunSynchronizedAsync((Func<Task>) (async () =>
    {
      // ISSUE: explicit non-virtual call
      BandClientType clientType = !await //__nonvirtual 
        (this.TryCheckConnectionWorkingAsync()).ConfigureAwait(false) ? BandClientType.CloudOnly : BandClientType.Both;
      try
      {
        cancellationToken.ThrowIfCancellationRequested();
        await this.UnlinkBandFromProfileInternalAsync(clientType, cancellationToken).ConfigureAwait(false);
      }
      finally
      {
        this.UserProfile = (IUserProfile) null;
      }
    }), cancellationToken)));

    public Task<UpdateInfo> GetLatestAvailableFirmwareVersionAsync(
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<UpdateInfo>(Level.Info, (Func<string>) (() => "Getting firmware update information from the Band and the Health Cloud"), (Func<Task<UpdateInfo>>) (() => Task.Run<UpdateInfo>((Func<Task<UpdateInfo>>) (async () => (await this.GetLatestFirmwareUpdateInfoAsync(cancellationToken).ConfigureAwait(false)).ToUpdateInfo(DateTimeOffset.Now)))), true);
    }

    public Task UpdateFirmwareAsync(
      CancellationToken cancellationToken,
      IProgress<FirmwareUpdateProgressReport> progress = null)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Updating firmware on the Band from the Health Cloud"), (Func<Task>) (async () =>
      {
        IFirmwareUpdateInfo updateInfo = await this.GetLatestFirmwareUpdateInfoAsync(cancellationToken).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        if (!updateInfo.IsFirmwareUpdateAvailable)
          throw new Exception("firmware is already up to date");
        if (!await this.UpdateFirmwareInternalAsync(updateInfo, cancellationToken, progress).ConfigureAwait(false))
        {
          cancellationToken.ThrowIfCancellationRequested();
          throw new Exception("after the firmware update, band version number remained unchanged");
        }
      }));
    }

    public Task<string[]> GetSmsResponsesAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<string[]>(Level.Info, (Func<string>) (() => "Getting SMS responses from the Band"), (Func<Task<string[]>>) (() => this.GetSmsResponsesInternalAsync(cancellationToken)), !this.IsPublicRelease);

    public Task SetSmsResponsesAsync(
      string response1,
      string response2,
      string response3,
      string response4,
      CancellationToken cancellationToken)
    {
      Dictionary<string, object> activityParameters = new Dictionary<string, object>();
      if (!this.IsPublicRelease)
      {
        activityParameters.Add("Response1", (object) response1);
        activityParameters.Add("Response2", (object) response2);
        activityParameters.Add("Response3", (object) response3);
        activityParameters.Add("Response4", (object) response4);
      }
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting SMS responses on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) activityParameters), (Func<Task>) (() => this.SetSmsResponsesInternalAsync(response1, response2, response3, response4, cancellationToken)));
    }

    public Task<string[]> GetPhoneCallResponsesAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<string[]>(Level.Info, (Func<string>) (() => "Getting phone call responses from the Band"), (Func<Task<string[]>>) (() => this.GetPhoneCallResponsesInternalAsync(cancellationToken)), !this.IsPublicRelease);

    public Task SetPhoneCallResponsesAsync(
      string response1,
      string response2,
      string response3,
      string response4,
      CancellationToken cancellationToken)
    {
      Dictionary<string, object> activityParameters = new Dictionary<string, object>();
      if (!this.IsPublicRelease)
      {
        activityParameters.Add("Response1", (object) response1);
        activityParameters.Add("Response2", (object) response2);
        activityParameters.Add("Response3", (object) response3);
        activityParameters.Add("Response4", (object) response4);
      }
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting phone call responses on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) activityParameters), (Func<Task>) (() => this.SetPhoneCallResponsesInternalAsync(response1, response2, response3, response4, cancellationToken)));
    }

    public Task UpdateGpsEphemerisDataAsync(
      CancellationToken cancellationToken,
      bool forceUpdate)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Sending ephemeris data to the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "ForceUpdate",
          (object) forceUpdate
        }
      }), (Func<Task>) (() => this.UpdateGpsEphemerisDataInternalAsync(cancellationToken, forceUpdate)));
    }

    public Task UpdateTimeZoneListAsync(CancellationToken cancellationToken, bool forceUpdate) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Updating the time zone list on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "ForceUpdate",
        (object) forceUpdate
      }
    }), (Func<Task>) (() => this.UpdateTimeZoneListInternalAsync(cancellationToken, forceUpdate)));

    public Task SetCurrentTimeAndTimeZoneAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting the current time and timezone on the Band"), (Func<Task>) (() => this.SetCurrentTimeAndTimeZoneInternalAsync(cancellationToken)));

    public Task<AdminTileSettings> GetTileSettingsAsync(Guid id) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<AdminTileSettings>(Level.Info, (Func<string>) (() => "Getting tile settings from the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "Guid",
        (object) id
      }
    }), (Func<Task<AdminTileSettings>>) (() => this.GetTileSettingsInternalAsync(id)), true);

    public Task SetTileSettingsAsync(Guid id, AdminTileSettings settings) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting tile settings on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "Guid",
        (object) id
      },
      {
        "TileSetting",
        (object) settings
      }
    }), (Func<Task>) (() => this.SetTileSettingsInternalAsync(id, settings)));

    public Task ImportUserProfileAsync(Func<IUserProfile, Task> userProfileModifications) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Importing the user profile from the Health Cloud to the Band"), (Func<Task>) (async () =>
    {
      if (this.UserProfile == null)
      {
        IUserProfile userProfile = await this.GetUserProfileInternalAsync(CancellationToken.None).ConfigureAwait(false);
        BandConnectionBase<TClient> bandConnectionBase = default; /*ME*/
        bandConnectionBase.UserProfile = userProfile;
        bandConnectionBase = (BandConnectionBase<TClient>) null;
      }
      await userProfileModifications(this.UserProfile).ConfigureAwait(false);
      await this.ImportUserProfileInternalAsync(this.UserProfile);
    }));

    public Task<CargoRunDisplayMetrics> GetRunDisplayMetricsAsync() => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<CargoRunDisplayMetrics>(Level.Info, (Func<string>) (() => "Getting run display metrics from the Band"), (Func<Task<CargoRunDisplayMetrics>>) (() => this.GetRunDisplayMetricsInternalAsync()), true);

    public Task SetRunDisplayMetricsAsync(CargoRunDisplayMetrics cargoRunDisplayMetrics) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting run display metrics on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "CargoRunDisplayMetrics",
        (object) cargoRunDisplayMetrics
      }
    }), (Func<Task>) (() => this.SetRunDisplayMetricsInternalAsync(cargoRunDisplayMetrics)));

    public Task<CargoBikeDisplayMetrics> GetBikeDisplayMetricsAsync() => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<CargoBikeDisplayMetrics>(Level.Info, (Func<string>) (() => "Getting bike display metrics from the Band"), (Func<Task<CargoBikeDisplayMetrics>>) (() => this.GetBikeDisplayMetricsInternalAsync()), true);

    public Task SetBikeDisplayMetricsAsync(CargoBikeDisplayMetrics cargoBikeDisplayMetrics) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting bike display metrics on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "CargoBikeDisplayMetrics",
        (object) cargoBikeDisplayMetrics
      }
    }), (Func<Task>) (() => this.SetBikeDisplayMetricsInternalAsync(cargoBikeDisplayMetrics)));

    public Task<int> GetBikeSplitDistanceAsync() => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<int>(Level.Info, (Func<string>) (() => "Getting Bike Split Distance From The Band"), (Func<Task<int>>) (() => this.GetBikeSplitDistanceInternalAsync()), true);

    public Task SetBikeSplitDistanceAsync(int splitGroupSize) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Setting bike split distance on the Band"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
    {
      {
        "SplitDistance",
        (object) splitGroupSize
      }
    }), (Func<Task>) (() => this.SetBikeSplitDistanceInternalAsync(splitGroupSize)));

    public Task<uint> GetMaxTileCountAsync() => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<uint>(Level.Info, (Func<string>) (() => "Getting maximum tiles allowed from the Band"), (Func<Task<uint>>) (() => this.GetMaxTileCountInternalAsync()), true);

    public Task<int> GetBatteryChargeAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<int>(Level.Info, (Func<string>) (() => "Getting battery charge from the Band"), (Func<Task<int>>) (() => this.GetBatteryChargeInternalAsync(cancellationToken)), true);

    public Task<SyncResultWrapper> SyncBandToCloudAsync(
      CancellationToken cancellationToken,
      Action<SyncProgressWrapper> onSyncProgress,
      bool logsOnly = false)
    {
      Action<SyncProgress> action = default;
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<SyncResultWrapper>(Level.Info, (Func<string>) (() => "Syncing collected data from the Band to the Health Cloud"), (Func<Task<SyncResultWrapper>>) (async () =>
      {
        SyncResultWrapper syncResult = 
          new SyncResultWrapper
          (
              await this.SyncBandToCloudInternalAsync(
                  (IProgress<SyncProgress>) new Progress<SyncProgress>
                  (action ?? (action = (Action<SyncProgress>) (syncProgress => onSyncProgress(new SyncProgressWrapper(syncProgress))))), logsOnly, cancellationToken) ?? throw new Exception("cargo client should not return a null sync result"));
        ++this.config.TotalTimesSynced;
        this.LogSyncPerfEvent(syncResult, this.config.TotalTimesSynced.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        return syncResult;
      }), true);
    }

    public Task UpdateLogProcessingAsync(
      IList<LogProcessingStatusWrapper> logProcessingStatusWrappers,
      CancellationToken cancellationToken,
      Action<double> onCloudProcessingLogsUpdate,
      bool singleCallback = true)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Subsribing to the log processing status from the Health Cloud"), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "LogProcessingStatusWrappers",
          (object) logProcessingStatusWrappers
        },
        {
          "SingleCallback",
          (object) singleCallback
        }
      }), (Func<Task>) (() => this.UpdateLogProcessingInternalAsync(logProcessingStatusWrappers, cancellationToken, onCloudProcessingLogsUpdate, singleCallback)));
    }

    public Task<uint> GetMeTileIdAsync(CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<uint>(Level.Info, (Func<string>) (() => "Getting the Me tile ID from the Band"), (Func<Task<uint>>) (() => this.GetMeTileIdInternalAsync(cancellationToken)), true);

    public Task SyncWebTilesAsync(bool forceSync, CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Syncing all WebTiles"), (Func<Task>) (() => this.SyncWebTilesInternalAsync(forceSync, cancellationToken)));

    public Task SyncWebTileAsync(Guid tileId, CancellationToken cancellationToken) => BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Syncing one WebTile"), (Func<Task>) (() => this.SyncWebTileInternalAsync(tileId, cancellationToken)));

    public Task<IDynamicBandConstants> GetAppBandConstantsAsync(
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync<IDynamicBandConstants>(Level.Info, (Func<string>) (() => "Getting the hardware type information from the Band"), (Func<Task<IDynamicBandConstants>>) (() => this.GetAppBandConstantsInternalAsync(cancellationToken)), true);
    }

    public Task SendCallNotificationAsync(
      CallNotification callNotification,
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => string.Format("Sending call update notification: State {0}", new object[1]
      {
        (object) callNotification.Type
      })), (Func<Task>) (() => this.SendCallNotificationInternalAsync(callNotification, cancellationToken)));
    }

    public Task QueueSmsNotificationAsync(
      BandMessage message,
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => "Queueing SMS notification"), (Func<Task>) (() => this.QueueSmsNotificationInternalAsync(message, cancellationToken)));
    }

    public Task SendTileMessageAsync(
      Guid tileId,
      TileMessage message,
      CancellationToken cancellationToken)
    {
      return BandConnectionBase<TClient>.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => string.Format("Sending tile message to tile: {0}", new object[1]
      {
        (object) tileId
      })), (Func<Task>) (() => this.SendTileMessageInternalAsync(tileId, message, cancellationToken)));
    }

    protected virtual Task SendWeatherUpdateNotificationsInternalAsync(
      IList<WeatherDay> dailyConditions,
      string location,
      DateTimeOffset timestamp,
      CancellationToken cancellationToken)
    {
      return this.UpdateTilePagesAsync(this.CreateWeatherTileUpdate(dailyConditions, location, timestamp), cancellationToken);
    }

    protected virtual Task SendFinanceUpdateNotificationsInternalAsync(
      IEnumerable<Stock> stocks,
      DateTimeOffset timestamp,
      CancellationToken cancellationToken)
    {
      return this.UpdateTilePagesAsync(this.CreateFinanceTileUpdate(stocks, timestamp), cancellationToken);
    }

    protected virtual Task SendStarbucksUpdateNotificationsInternalAsync(
      string cardNumber,
      CancellationToken cancellationToken)
    {
      return this.UpdateTilePagesAsync(this.CreateStarbucksTileUpdate(cardNumber), cancellationToken);
    }

    private Task<IFirmwareUpdateInfo> GetLatestFirmwareUpdateInfoAsync(
      CancellationToken cancellationToken)
    {
      return Task.Run<IFirmwareUpdateInfo>((Func<Task<IFirmwareUpdateInfo>>) (async () =>
      {
        IFirmwareUpdateInfo firmwareUpdateInfo = await this.GetLatestFirmwareUpdateInfoInternalAsync(cancellationToken).ConfigureAwait(false);
        if (firmwareUpdateInfo == null)
        {
          cancellationToken.ThrowIfCancellationRequested();
          throw new Exception("checking for firmware update returned a null object");
        }
        return firmwareUpdateInfo;
      }));
    }

    private void CloseClientSession()
    {
      this.CloseClientSession(this.CargoClient);
      this.CargoClient = default (TClient);
    }

    private void LogSyncPerfEvent(SyncResultWrapper syncResult, string syncNumberString)
    {
      this.perfLogger.Mark("Sync", "DownloadedLogSizeBytes", syncNumberString, syncResult.DownloadedSensorLogBytes.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      this.perfLogger.Mark("Sync", "UploadedLogSizeBytes", syncNumberString, syncResult.UploadedSensorLogBytes.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      if (syncResult.DownloadKbitsPerSecond > 0.0)
      {
        int num = (int) ((double) (syncResult.DownloadedSensorLogBytes * 8L) / syncResult.DownloadKbitsPerSecond);
        this.perfLogger.Mark("Sync", "DownloadDurationMs", syncNumberString, num.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
      if (syncResult.UploadKbitsPerSecond > 0.0)
      {
        int num = (int) ((double) (syncResult.UploadedSensorLogBytes * 8L) / syncResult.UploadKbitsPerSecond);
        this.perfLogger.Mark("Sync", "UploadDurationMs", syncNumberString, num.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
      this.perfLogger.Mark("Sync", "Succeeded", syncNumberString, syncResult.RanToCompletion.ToString());
    }

    private TileUpdateInfo CreateWeatherTileUpdate(
      IList<WeatherDay> dailyConditions,
      string location,
      DateTimeOffset timestamp)
    {
      TileUpdateInfo tileUpdateInfo = new TileUpdateInfo(Guid.Parse("69a39b4e-084b-4b53-9a1b-581826df9e36"));
      timestamp = timestamp.ToLocalTime();
      string lastUpdated = AppResources.LastUpdated;
      if (!string.IsNullOrEmpty(lastUpdated) && lastUpdated.Length > 20 && !string.IsNullOrEmpty(location) && location.Length > 20)
        location = location.Substring(0, 20);
      tileUpdateInfo.PageUdpates.Add(new TilePageUpdateInfo()
      {
        PageId = Guid.Parse("76150e97-94cd-4d55-847f-ba7e9fc408e6"),
        PageLayoutIndex = (ushort) 0,
        PageElements = (IList<ITilePageElement>) new List<ITilePageElement>()
        {
          (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 11, lastUpdated),
          (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 21, Formatter.FormatTimeWithSingleCharacterAMOrPM(timestamp, true)),
          (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 31, location)
        }
      });
      for (int index = dailyConditions.Count - 1; index >= 1; --index)
      {
        string textboxValue;
        switch (index)
        {
          case 1:
            textboxValue = AppResources.Today;
            break;
          case 2:
            textboxValue = AppResources.Tomorrow;
            break;
          default:
            textboxValue = dailyConditions[index].Date.ToString("dddd");
            break;
        }
        tileUpdateInfo.PageUdpates.Add(new TilePageUpdateInfo()
        {
          PageId = Guid.Parse(HealthAppConstants.Band.TilePageIds.WeatherPages[index]),
          PageLayoutIndex = (ushort) 1,
          PageElements = (IList<ITilePageElement>) new List<ITilePageElement>()
          {
            (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 11, textboxValue),
            (ITilePageElement) this.tilePageElementFactory.CreateTileIconbox((ushort) 21, (ushort) dailyConditions[index].IconId),
            (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 22, dailyConditions[index].High.ToString() + "°"),
            (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 23, "/" + dailyConditions[index].Low.ToString() + "°")
          }
        });
      }
      tileUpdateInfo.PageUdpates.Add(new TilePageUpdateInfo()
      {
        PageId = Guid.Parse(HealthAppConstants.Band.TilePageIds.WeatherPages[0]),
        PageLayoutIndex = (ushort) 1,
        PageElements = (IList<ITilePageElement>) new List<ITilePageElement>()
        {
          (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 11, AppResources.Now),
          (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 12, "|"),
          (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 13, dailyConditions[0].Caption),
          (ITilePageElement) this.tilePageElementFactory.CreateTileIconbox((ushort) 21, (ushort) dailyConditions[0].IconId),
          (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 22, dailyConditions[0].High.ToString() + "°")
        }
      });
      return tileUpdateInfo;
    }

    private TileUpdateInfo CreateFinanceTileUpdate(
      IEnumerable<Stock> stocks,
      DateTimeOffset timestamp)
    {
      TileUpdateInfo tileUpdateInfo = new TileUpdateInfo(Guid.Parse("5992928a-bd79-4bb5-9678-f08246d03e68"));
      timestamp = timestamp.ToLocalTime();
      tileUpdateInfo.PageUdpates.Add(new TilePageUpdateInfo()
      {
        PageId = Guid.Parse("f375f75e-05b7-42e0-a5ff-9a5c409e9dd9"),
        PageLayoutIndex = (ushort) 0,
        PageElements = (IList<ITilePageElement>) new List<ITilePageElement>()
        {
          (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 11, AppResources.LastUpdated),
          (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 21, Formatter.FormatTimeWithSingleCharacterAMOrPM(timestamp, true))
        }
      });
      List<Stock> list = stocks.ToList<Stock>();
      for (int index = 0; index < list.Count && index < 7; ++index)
      {
        Stock stock = list[list.Count - 1 - index];
        Guid guid = Guid.Parse(HealthAppConstants.Band.TilePageIds.FinancePages[HealthAppConstants.Band.TilePageIds.FinancePages.Count - 1 - index]);
        bool flag = stock.Change >= 0.0;
        string str = flag ? "+" : "-";
        ushort num = !flag ? (ushort) 1 : (ushort) 2;
        tileUpdateInfo.PageUdpates.Add(new TilePageUpdateInfo()
        {
          PageId = guid,
          PageLayoutIndex = num,
          PageElements = (IList<ITilePageElement>) new List<ITilePageElement>()
          {
            (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 11, stock.Symbol),
            (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 12, "|"),
            (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 13, str + Math.Abs(stock.Change).ToString("F2") + "%"),
            (ITilePageElement) this.tilePageElementFactory.CreateTileIconbox((ushort) 21, flag ? (ushort) 1 : (ushort) 2),
            (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 22, stock.Value.ToString("F2"))
          }
        });
      }
      return tileUpdateInfo;
    }

    private TileUpdateInfo CreateStarbucksTileUpdate(string cardNumber)
    {
      TileUpdateInfo tileUpdateInfo = new TileUpdateInfo(Guid.Parse("64a29f65-70bb-4f32-99a2-0f250a05d427"));
      if (!string.IsNullOrEmpty(cardNumber) && cardNumber.Length == 16)
      {
        string textboxValue = string.Format("{0} {1} {2} {3}", (object) cardNumber.Substring(0, 4), (object) cardNumber.Substring(4, 4), (object) cardNumber.Substring(8, 4), (object) cardNumber.Substring(12, 4));
        tileUpdateInfo.PageUdpates.Add(new TilePageUpdateInfo()
        {
          PageId = Guid.Parse("5fe2048d-7336-684f-901d-dda85118c509"),
          PageLayoutIndex = (ushort) 0,
          PageElements = (IList<ITilePageElement>) new List<ITilePageElement>()
          {
            (ITilePageElement) this.tilePageElementFactory.CreateTileBarcode((ushort) 11, BarcodeType.Pdf417, cardNumber),
            (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 21, textboxValue)
          }
        });
      }
      else
        tileUpdateInfo.PageUdpates.Add(new TilePageUpdateInfo()
        {
          PageId = Guid.Parse("5fe2048d-7336-684f-901d-dda85118c509"),
          PageLayoutIndex = (ushort) 1,
          PageElements = (IList<ITilePageElement>) new List<ITilePageElement>()
          {
            (ITilePageElement) this.tilePageElementFactory.CreateTileTextbox((ushort) 11, AppResources.StarbucksNoCardOnDevice)
          }
        });
      return tileUpdateInfo;
    }

    protected Task CheckForTileNotFoundExceptionAsync(
      Func<Task> func,
      TileUpdateInfo tileUpdateInfo)
    {
      return this.CheckForTileNotFoundExceptionAsync(func, string.Format("Tile ID: {0} not found.", new object[1]
      {
        (object) tileUpdateInfo.TileId
      }));
    }

    protected abstract Task<IUserProfile> GetUserProfileInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task<FirmwareVersions> GetBandFirmwareVersionInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task<string> GetProductSerialNumberInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task<DiagnosticsBandDevice> GetDiagnosticsInfoInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task<IList<AdminBandTile>> GetDefaultTilesInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task<StartStrip> GetStartStripInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task<StartStrip> GetStartStripWithoutImagesInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task<BandTheme> GetBandThemeInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task SetStartStripInternalAsync(
      StartStrip s,
      CancellationToken cancellationToken);

    protected abstract Task PersonalizeBandInternalAsync(
      uint imageId,
      CancellationToken cancellationToken,
      StartStrip startStrip = null,
      BandImage image = null,
      BandTheme color = null,
      IDictionary<Guid, BandTheme> customColors = null);

    protected abstract Task NavigateToScreenInternalAsync(
      CargoScreen screen,
      CancellationToken cancellationToken);

    protected abstract Task SetOobeStageInternalAsync(
      OobeStage stage,
      CancellationToken cancellationToken);

    protected abstract Task FinalizeOobeInternalAsync(CancellationToken cancellationToken);

    protected abstract Task SaveCloudUserProfileInternalAsync(
      Func<IUserProfile, Task> userProfileModifications);

    protected abstract Task SaveUserProfileInternalAsync(
      Func<IUserProfile, Task> userProfileModifications,
      CancellationToken cancellationToken);

    protected abstract Task<string> UploadFileToCloudInternalAsync(
      IFile file,
      LogFileTypes fileType,
      CancellationToken cancellationToken);

    protected abstract Task SendCalendarEventsInternalAsync(
      IEnumerable<CalendarEvent> calendarEvents,
      CancellationToken cancellationToken);

    protected abstract Task SetSleepNotificationInternalAsync(
      SleepNotification sleepNotification,
      CancellationToken cancellationToken);

    protected abstract Task DisableSleepNotificationInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task SetLightExposureNotificationInternalAsync(
      LightExposureNotification lightExposureNotification,
      CancellationToken cancellationToken);

    protected abstract Task DisableLightExposureNotificationInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task SaveGoalsToBandInternalAsync(Microsoft.Band.Admin.Goals goals);

    protected abstract Task SendWorkoutToBandInternalAsync(
      Stream workoutStream,
      CancellationToken cancellationToken);

    protected abstract Task SetWorkoutActivitiesInternalAsync(
      IList<WorkoutActivity> activities,
      CancellationToken cancellationToken);

    protected abstract Task SendGolfCourseToBandInternalAsync(
      Stream golfCourseStream,
      CancellationToken cancellationToken);

    protected abstract Task<IUserProfile> GetCloudUserProfileInternalAsync();

    protected abstract Task<bool> GetBandOobeCompletedInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task LinkBandToProfileInternalAsync(CancellationToken cancellationToken);

    protected abstract Task UnlinkBandFromProfileInternalAsync(
      BandClientType clientType,
      CancellationToken cancellationToken);

    protected abstract Task<bool> UpdateFirmwareInternalAsync(
      IFirmwareUpdateInfo updateInfo,
      CancellationToken cancellationToken,
      IProgress<FirmwareUpdateProgressReport> progress = null);

    protected abstract Task<string[]> GetSmsResponsesInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task SetSmsResponsesInternalAsync(
      string response1,
      string response2,
      string response3,
      string response4,
      CancellationToken cancellationToken);

    protected abstract Task<string[]> GetPhoneCallResponsesInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task SetPhoneCallResponsesInternalAsync(
      string response1,
      string response2,
      string response3,
      string response4,
      CancellationToken cancellationToken);

    protected abstract Task UpdateGpsEphemerisDataInternalAsync(
      CancellationToken cancellationToken,
      bool forceUpdate);

    protected abstract Task UpdateTimeZoneListInternalAsync(
      CancellationToken cancellationToken,
      bool forceUpdate);

    protected abstract Task SetCurrentTimeAndTimeZoneInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task<AdminTileSettings> GetTileSettingsInternalAsync(
      Guid id);

    protected abstract Task SetTileSettingsInternalAsync(Guid id, AdminTileSettings settings);

    protected abstract Task ImportUserProfileInternalAsync(IUserProfile userProfile);

    protected abstract Task<CargoRunDisplayMetrics> GetRunDisplayMetricsInternalAsync();

    protected abstract Task SetRunDisplayMetricsInternalAsync(
      CargoRunDisplayMetrics cargoRunDisplayMetrics);

    protected abstract Task<CargoBikeDisplayMetrics> GetBikeDisplayMetricsInternalAsync();

    protected abstract Task SetBikeDisplayMetricsInternalAsync(
      CargoBikeDisplayMetrics cargoBikeDisplayMetrics);

    protected abstract Task<int> GetBikeSplitDistanceInternalAsync();

    protected abstract Task SetBikeSplitDistanceInternalAsync(int splitGroupSize);

    protected abstract Task<uint> GetMaxTileCountInternalAsync();

    protected abstract Task<int> GetBatteryChargeInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task<SyncResult> SyncBandToCloudInternalAsync(
      IProgress<SyncProgress> progress,
      bool logsOnly,
      CancellationToken cancellationToken);

    protected abstract Task UpdateLogProcessingInternalAsync(
      IList<LogProcessingStatusWrapper> logProcessingStatusWrappers,
      CancellationToken cancellationToken,
      Action<double> onCloudProcessingLogsUpdate,
      bool singleCallback = true);

    protected abstract Task<uint> GetMeTileIdInternalAsync(CancellationToken cancellationToken);

    protected abstract Task SyncWebTilesInternalAsync(
      bool forceSync,
      CancellationToken cancellationToken);

    protected abstract Task SyncWebTileInternalAsync(
      Guid tileId,
      CancellationToken cancellationToken);

    protected abstract Task<IDynamicBandConstants> GetAppBandConstantsInternalAsync(
      CancellationToken cancellationToken);

    protected abstract Task<IFirmwareUpdateInfo> GetLatestFirmwareUpdateInfoInternalAsync(
      CancellationToken cancellationToken);

    protected abstract void CloseClientSession(TClient client);

    protected abstract Task UpdateTilePagesAsync(
      TileUpdateInfo tileUpdateInfo,
      CancellationToken cancellationToken);

    protected abstract Task SendCallNotificationInternalAsync(
      CallNotification callNotification,
      CancellationToken cancellationToken);

    protected abstract Task QueueSmsNotificationInternalAsync(
      BandMessage message,
      CancellationToken cancellationToken);

    protected abstract Task SendTileMessageInternalAsync(
      Guid tileId,
      TileMessage message,
      CancellationToken cancellationToken);

    protected abstract Task CheckForTileNotFoundExceptionAsync(Func<Task> func, string message);
       
  }
}
