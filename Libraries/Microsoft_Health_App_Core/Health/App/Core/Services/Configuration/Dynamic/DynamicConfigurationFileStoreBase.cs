// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.DynamicConfigurationFileStoreBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  public abstract class DynamicConfigurationFileStoreBase : IDynamicConfigurationFileStore
  {
    private static readonly Uri ExpectedConfigurationSchema = new Uri("http://microsoft.com/health/app/v1/configuration");
    private readonly ILog logger;

    protected DynamicConfigurationFileStoreBase(ILog logger)
    {
      Assert.ParamIsNotNull((object) logger, nameof (logger));
      this.logger = logger;
    }

    public async Task<DynamicConfigurationFile> GetConfigurationFileAsync(
      RegionInfo region,
      CancellationToken token)
    {
      Assert.ParamIsNotNull((object) region, nameof (region));
      try
      {
        using (Stream stream = await this.GetConfigurationFileStreamAsync(region, token).ConfigureAwait(false))
        {
          if (stream != null)
          {
            using (StreamReader streamReader = new StreamReader(stream))
            {
              token.ThrowIfCancellationRequested();
              string str = await streamReader.ReadToEndAsync().ConfigureAwait(false);
              token.ThrowIfCancellationRequested();
              DynamicConfigurationFile configurationFile = JsonConvert.DeserializeObject<DynamicConfigurationFile>(str);
              if (configurationFile.Schema != DynamicConfigurationFileStoreBase.ExpectedConfigurationSchema)
              {
                this.logger.Warn("Ignoring configuration file with schema '{0}'. (Expected '{1}'.)", (object) configurationFile.Schema, (object) DynamicConfigurationFileStoreBase.ExpectedConfigurationSchema);
                return (DynamicConfigurationFile) null;
              }
              if (!(configurationFile.Version == (Version) null) && !(configurationFile.Version < DynamicConfigurationFile.ExpectedMinimumVersion) && configurationFile.Version.Major == DynamicConfigurationFile.ExpectedMinimumVersion.Major)
                return configurationFile;
              this.logger.Warn((object) string.Format("Ignoring configuration file with version '{0}'. (Expected major version {1}, minimum version '{2}'.)", new object[3]
              {
                (object) configurationFile.Version,
                (object) DynamicConfigurationFile.ExpectedMinimumVersion.Major,
                (object) DynamicConfigurationFile.ExpectedMinimumVersion
              }));
              return (DynamicConfigurationFile) null;
            }
          }
        }
      }
      catch (Exception ex)
      {
        this.logger.Warn((object) "Ignoring configuration file due to exception.", ex);
      }
      return (DynamicConfigurationFile) null;
    }

    protected abstract Task<Stream> GetConfigurationFileStreamAsync(
      RegionInfo region,
      CancellationToken token);
  }
}
