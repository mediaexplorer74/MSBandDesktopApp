// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.DynamicConfigurationServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  internal abstract class DynamicConfigurationServiceBase
  {
    private readonly IDateTimeService dateTimeService;
    private readonly IDynamicConfigurationFileStore defaultStore;
    private readonly ICurrentDynamicConfigurationFileStore localStore;
    private readonly ILog logger;
    private readonly IDynamicConfigurationFileStore remoteStore;

    protected DynamicConfigurationServiceBase(
      IDynamicConfigurationFileStore defaultStore,
      ICurrentDynamicConfigurationFileStore localStore,
      IDynamicConfigurationFileStore remoteStore,
      IDateTimeService dateTimeService,
      ILog logger)
    {
      Assert.ParamIsNotNull((object) defaultStore, nameof (defaultStore));
      Assert.ParamIsNotNull((object) localStore, nameof (localStore));
      Assert.ParamIsNotNull((object) remoteStore, nameof (remoteStore));
      Assert.ParamIsNotNull((object) dateTimeService, nameof (dateTimeService));
      Assert.ParamIsNotNull((object) logger, nameof (logger));
      this.defaultStore = defaultStore;
      this.localStore = localStore;
      this.remoteStore = remoteStore;
      this.dateTimeService = dateTimeService;
      this.logger = logger;
    }

    protected IDynamicConfigurationFileStore DefaultStore => this.defaultStore;

    protected ICurrentDynamicConfigurationFileStore LocalStore => this.localStore;

    protected IDynamicConfigurationFileStore RemoteStore => this.remoteStore;

    protected async Task<CurrentDynamicConfigurationFile> GetLatestConfigurationFileAsync(
      RegionInfo region,
      CancellationToken token)
    {
      Task<DynamicConfigurationFile> remoteStoreTask = this.RemoteStore.GetConfigurationFileAsync(region, token);
      Task<DynamicConfigurationFile> defaultStoreTask = this.DefaultStore.GetConfigurationFileAsync(region, token);
      DynamicConfigurationFile[] configurationFileArray = await Task.WhenAll<DynamicConfigurationFile>(remoteStoreTask, defaultStoreTask).ConfigureAwait(false);
      if (remoteStoreTask.Result != null)
        ApplicationTelemetry.LogRemoteDynamicConfigFileDownloaded(region, remoteStoreTask.Result.Version);

            /*
            \u003C\u003Ef__AnonymousType2<DynamicConfigurationFile, ConfigurationFileSource>[] source = new \u003C\u003Ef__AnonymousType2<DynamicConfigurationFile, ConfigurationFileSource>[2]
            {
              new
              {
                File = defaultStoreTask.Result,
                Source = ConfigurationFileSource.Default
              },
              new
              {
                File = remoteStoreTask.Result,
                Source = ConfigurationFileSource.Remote
              }
            };
            */
            /*
            if (source.All(storeFile => storeFile.File == null))
            {
              this.logger.Warn((object) "No configuration files could be retrieved from the Cloud or the packaged application.");
              return (CurrentDynamicConfigurationFile) null;
            }
            var data = source.Where(storeFile => storeFile.File != null).Aggregate((agg, next) => !(next.File.Version > agg.File.Version) ? agg : next);
            return new CurrentDynamicConfigurationFile(this.dateTimeService.Now, data.Source, region.TwoLetterISORegionName, data.File);
            */
            return default; // *ME*
    }

    protected void LogCurrentConfigurationFile(CurrentDynamicConfigurationFile file)
    {
      if (file == null)
        return;
      this.logger.Info((object) "Current dynamic configuration file:");
      this.logger.Info("Region: {0}", (object) file.RegionName);
      this.logger.Info("Source: {0}", (object) file.Source);
      this.logger.Info("Version: {0}", (object) file.File.Version);
    }
  }
}
