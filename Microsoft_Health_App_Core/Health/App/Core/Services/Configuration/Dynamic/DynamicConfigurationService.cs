// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.DynamicConfigurationService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Devices;
using Microsoft.Health.App.Core.Services.ForegroundSync;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client.Services;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  internal sealed class DynamicConfigurationService : 
    DynamicConfigurationServiceBase,
    IDynamicConfigurationService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Configuration\\Dynamic\\DynamicConfigurationService.cs");
    private readonly Mutex initializationLock;
    private readonly IRegionService regionService;
    private readonly IDynamicConfigurationUpdateService updateService;
    private readonly IServiceLocator serviceLocator;
    private CurrentDynamicConfigurationFile configurationFile;

    public DynamicConfigurationService(
      [Dependency("DefaultConfigurationFileStore")] IDynamicConfigurationFileStore defaultStore,
      ICurrentDynamicConfigurationFileStore localStore,
      [Dependency("RemoteConfigurationFileStore")] IDynamicConfigurationFileStore remoteStore,
      IDateTimeService dateTimeService,
      IRegionService regionService,
      IDynamicConfigurationUpdateService updateService,
      IMutexService mutexProvider,
      IServiceLocator serviceLocator)
      : base(defaultStore, localStore, remoteStore, dateTimeService, DynamicConfigurationService.Logger)
    {
      this.regionService = regionService;
      this.updateService = updateService;
      this.serviceLocator = serviceLocator;
      this.initializationLock = mutexProvider.GetNamedMutex(false, "DynamicConfigurationService.InitializationLock");
    }

    public IDynamicConfiguration Configuration
    {
      get
      {
        if (this.configurationFile == null)
        {
          DynamicConfigurationService.Logger.Error((object) "An application component attempted to use the dynamic configuration before it was initialized.");
          throw new InvalidOperationException("An application component attempted to use the dynamic configuration before it was initialized.");
        }
        return (IDynamicConfiguration) this.configurationFile.File.Configuration;
      }
    }

    public Task RefreshConfigurationAsync(CancellationToken token) => this.configurationFile != null ? this.initializationLock.RunSynchronizedAsync((Func<Task>) (async () =>
    {
      DynamicConfigurationService.Logger.Debug((object) "<START> Refreshing current configuration from local storage");
      CurrentDynamicConfigurationFile configurationFile = await this.LocalStore.GetConfigurationFileAsync(token).ConfigureAwait(false);
      if (configurationFile == null)
        DynamicConfigurationService.Logger.Warn((object) "Unable to retrieve configuration from local storage for Refresh, keeping in-memory copy.");
      else
        this.configurationFile = configurationFile;
      DynamicConfigurationService.Logger.Debug((object) "<END> Refreshing current configuration from local storage");
    }), token) : (Task) Task.FromResult<bool>(true);

    public Task InitializeAsync(CancellationToken token) => this.configurationFile == null ? this.initializationLock.RunSynchronizedAsync((Func<Task>) (async () =>
    {
      if (this.configurationFile != null)
        return;
      this.serviceLocator.GetInstance<IDeviceManager>().SyncStateChanged += new EventHandler<SyncStateChangedEventArgs>(this.DeviceManagerOnSyncStateChanged);
      DynamicConfigurationService.Logger.Info((object) "Initializing the dynamic configuration service...");
      DynamicConfigurationService configurationService;
      CurrentDynamicConfigurationFile configurationFile1 = configurationService.configurationFile;
      ConfiguredTaskAwaitable<CurrentDynamicConfigurationFile> configuredTaskAwaitable = this.LocalStore.GetConfigurationFileAsync(token).ConfigureAwait(false);
      CurrentDynamicConfigurationFile configurationFile2 = await configuredTaskAwaitable;
      configurationService.configurationFile = configurationFile2;
      configurationService = (DynamicConfigurationService) null;
      if (this.configurationFile != null && this.configurationFile.File.Version >= DynamicConfigurationFile.ExpectedMinimumVersion)
      {
        DynamicConfigurationService.Logger.Info((object) "Found a current configuration file; starting the update process...");
        this.StartUpdateProcess(token);
      }
      else
      {
        DynamicConfigurationService.Logger.Info((object) "No current configuration file; attempting to retrieve one from the Cloud or default to one in the application package.");
        configurationService = this;
        CurrentDynamicConfigurationFile configurationFile3 = configurationService.configurationFile;
        configuredTaskAwaitable = this.GetLatestConfigurationFileAsync(this.regionService.CurrentRegion, token).ConfigureAwait(false);
        CurrentDynamicConfigurationFile configurationFile4 = await configuredTaskAwaitable;
        configurationService.configurationFile = configurationFile4;
        configurationService = (DynamicConfigurationService) null;
        bool flag = this.configurationFile != null;
        if (flag)
          flag = !await this.LocalStore.SetConfigurationFileAsync(this.configurationFile, token).ConfigureAwait(false);
        if (flag)
          DynamicConfigurationService.Logger.Warn((object) "Unable to save the current configuration file.");
      }
      if (this.configurationFile == null)
      {
        DynamicConfigurationService.Logger.Error((object) "Unable to initialize the dynamic configuration service with a valid configuration file.");
        throw new InvalidOperationException("Unable to initialize the dynamic configuration service with a valid configuration file.");
      }
      this.LogCurrentConfigurationFile(this.configurationFile);
      DynamicConfigurationService.Logger.Info((object) "The dynamic configuration service is initialized.");
    }), token) : (Task) Task.FromResult<bool>(true);

    private void DeviceManagerOnSyncStateChanged(object sender, SyncStateChangedEventArgs e)
    {
      if (e.IsSyncing)
        return;
      this.StartUpdateProcess(CancellationToken.None);
    }

    private async void StartUpdateProcess(CancellationToken token)
    {
      try
      {
        await this.updateService.UpdateConfigurationFileAsync(token).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        DynamicConfigurationService.Logger.Warn((object) "Ignoring failure to update the configuration file due to exception.", ex);
      }
    }
  }
}
