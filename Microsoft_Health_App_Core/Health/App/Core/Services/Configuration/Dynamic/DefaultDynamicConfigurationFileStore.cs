// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.DefaultDynamicConfigurationFileStore
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  public class DefaultDynamicConfigurationFileStore : DynamicConfigurationFileStoreBase
  {
    private const string DefaultDynamicConfigurationName = "defaultconfig";
    public static readonly ConfigurationValue<bool> IsDefaultFileStoreEnabled = ConfigurationValue.CreateBoolean("DynamicConfigurationService", nameof (IsDefaultFileStoreEnabled), true);
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Configuration\\Dynamic\\DefaultDynamicConfigurationFileStore.cs");
    private readonly IPackageResourceService packageResourceService;
    private readonly IConfigurationService configurationService;

    public DefaultDynamicConfigurationFileStore(
      IPackageResourceService packageResourceService,
      IConfigurationService configurationService)
      : base(DefaultDynamicConfigurationFileStore.Logger)
    {
      this.packageResourceService = packageResourceService;
      this.configurationService = configurationService;
    }

    protected override async Task<Stream> GetConfigurationFileStreamAsync(
      RegionInfo region,
      CancellationToken token)
    {
      if (!this.configurationService.GetValue<bool>(DefaultDynamicConfigurationFileStore.IsDefaultFileStoreEnabled))
      {
        DefaultDynamicConfigurationFileStore.Logger.Warn((object) "The default configuration file store has been disabled.");
        return (Stream) null;
      }
      DefaultDynamicConfigurationFileStore.Logger.Info("Getting packaged configuration file for region '{0}'...", (object) region);
      Stream stream = (Stream) null;
      try
      {
        stream = await this.packageResourceService.OpenDynamicConfigAsync(region.TwoLetterISORegionName.ToLowerInvariant(), token).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        DefaultDynamicConfigurationFileStore.Logger.Warn((object) "Unable to get region-specific packaged configuration file due to exception.", ex);
      }
      if (stream == null)
      {
        DefaultDynamicConfigurationFileStore.Logger.Warn((object) "No region-specific packaged configuration file found; getting default packaged configuration file...");
        stream = await this.packageResourceService.OpenDynamicConfigAsync("defaultconfig", token).ConfigureAwait(false);
      }
      if (stream == null)
        DefaultDynamicConfigurationFileStore.Logger.Warn((object) "No packaged configuration file found.");
      return stream;
    }
  }
}
