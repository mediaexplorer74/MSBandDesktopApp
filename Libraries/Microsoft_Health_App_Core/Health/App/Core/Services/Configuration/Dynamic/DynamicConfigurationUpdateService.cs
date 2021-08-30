// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.DynamicConfigurationUpdateService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client.Services;
using Microsoft.Practices.Unity;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  internal sealed class DynamicConfigurationUpdateService : 
    DynamicConfigurationServiceBase,
    IDynamicConfigurationUpdateService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Configuration\\Dynamic\\DynamicConfigurationUpdateService.cs");
    private static readonly TimeSpan UpdateCheckFrequency = TimeSpan.FromHours(24.0);
    private readonly IDateTimeService dateTimeService;
    private readonly IRegionService regionService;
    private readonly Mutex updateLock;

    public DynamicConfigurationUpdateService(
      [Dependency("DefaultConfigurationFileStore")] IDynamicConfigurationFileStore defaultStore,
      ICurrentDynamicConfigurationFileStore localStore,
      [Dependency("RemoteConfigurationFileStore")] IDynamicConfigurationFileStore remoteStore,
      IDateTimeService dateTimeService,
      IRegionService regionService,
      IMutexService mutexProvider)
      : base(defaultStore, localStore, remoteStore, dateTimeService, DynamicConfigurationUpdateService.Logger)
    {
      Assert.ParamIsNotNull((object) dateTimeService, nameof (dateTimeService));
      Assert.ParamIsNotNull((object) regionService, nameof (regionService));
      Assert.ParamIsNotNull((object) mutexProvider, nameof (mutexProvider));
      this.dateTimeService = dateTimeService;
      this.regionService = regionService;
      this.updateLock = mutexProvider.GetNamedMutex(false, "DynamicConfigurationUpdateService.UpdateLock");
    }

    public Task UpdateConfigurationFileAsync(bool forceUpdate, CancellationToken token) => this.CoreUpdateConfigurationFileAsync(forceUpdate, token);

    public Task UpdateConfigurationFileAsync(CancellationToken token) => this.CoreUpdateConfigurationFileAsync(false, token);

    private Task CoreUpdateConfigurationFileAsync(bool forceUpdate, CancellationToken token) => this.updateLock.RunSynchronizedAsync((Func<Task>) (async () =>
    {
      DynamicConfigurationUpdateService.Logger.Info((object) "Starting dynamic configuration update...");
      CurrentDynamicConfigurationFile currentFile = await this.LocalStore.GetConfigurationFileAsync(token).ConfigureAwait(false);
      RegionInfo currentRegion = this.regionService.CurrentRegion;
      bool flag = false;
      if (currentFile == null || currentFile.File.Version < DynamicConfigurationFile.ExpectedMinimumVersion || !StringComparer.OrdinalIgnoreCase.Equals(currentFile.RegionName, currentRegion.TwoLetterISORegionName))
      {
        DynamicConfigurationUpdateService.Logger.Info((object) "No valid local configuration file found.  Getting latest configuration files.");
        if (currentFile != null)
        {
          DynamicConfigurationUpdateService.Logger.Info((object) string.Format("Current Region:{{{0}}}, File Region:{{{1}}}", new object[2]
          {
            (object) currentRegion.TwoLetterISORegionName,
            (object) currentFile.RegionName
          }));
          DynamicConfigurationUpdateService.Logger.Info((object) string.Format("Expected minimum version:{{{0}}}, File version:{{{1}}}", new object[2]
          {
            (object) DynamicConfigurationFile.ExpectedMinimumVersion,
            (object) currentFile.File.Version
          }));
        }
        currentFile = await this.GetLatestConfigurationFileAsync(currentRegion, token).ConfigureAwait(false);
        flag = currentFile != null;
      }
      else if (forceUpdate || this.dateTimeService.Now - currentFile.Timestamp > DynamicConfigurationUpdateService.UpdateCheckFrequency)
      {
        DynamicConfigurationUpdateService.Logger.Info((object) "Current configuration file is stale.  Fetching latest configuration files.");
        CurrentDynamicConfigurationFile configurationFile = await this.GetLatestConfigurationFileAsync(currentRegion, token).ConfigureAwait(false);
        if (configurationFile != null && configurationFile.File.Version > currentFile.File.Version)
          currentFile = configurationFile;
        else
          currentFile.Timestamp = this.dateTimeService.Now;
        flag = true;
      }
      else
        DynamicConfigurationUpdateService.Logger.Info((object) string.Format("Skipping configuration file fetch because it is not yet stale.  Time since last update: {0} hours.", new object[1]
        {
          (object) (this.dateTimeService.Now - currentFile.Timestamp).Hours
        }));
      if (flag)
      {
        DynamicConfigurationUpdateService.Logger.Info((object) "Updating the current configuration file...");
        this.LogCurrentConfigurationFile(currentFile);
        if (!await this.LocalStore.SetConfigurationFileAsync(currentFile, token).ConfigureAwait(false))
          DynamicConfigurationUpdateService.Logger.Warn((object) "Unable to save the updated configuration file.");
      }
      DynamicConfigurationUpdateService.Logger.Info((object) "Dynamic configuration update complete.");
    }), token);
  }
}
