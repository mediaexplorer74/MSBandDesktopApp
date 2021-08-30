// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Authentication.ConnectionInfoStore
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.Cloud.Client;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Authentication
{
  public class ConnectionInfoStore : IConnectionInfoStore
  {
    private const string DataFileFormat = "{0}.dat";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Authentication\\ConnectionInfoStore.cs");
    private readonly Dictionary<string, HealthCloudConnectionInfo> credentialCache = new Dictionary<string, HealthCloudConnectionInfo>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private readonly IConfig config;
    private readonly ICryptographyService cryptographyService;
    private readonly IFileSystemService fileSystemService;

    public ConnectionInfoStore(
      IConfig config,
      ICryptographyService cryptographyService,
      IFileSystemService fileSystemService)
    {
      this.config = config;
      this.cryptographyService = cryptographyService;
      this.fileSystemService = fileSystemService;
    }

    public bool Caching { get; set; }

    private string DataFileName => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}.dat", new object[1]
    {
      (object) new Uri(this.CacheKey, UriKind.Absolute).Host
    });

    public async Task<HealthCloudConnectionInfo> TryGetAsync()
    {
      HealthCloudConnectionInfo connectionInfo = (HealthCloudConnectionInfo) null;
      try
      {
        connectionInfo = await this.TryGetInternalAsync().ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        ConnectionInfoStore.Logger.Error(ex, "Unexpected exception calling TryGetCredentials");
      }
      return connectionInfo;
    }

    public async Task<HealthCloudConnectionInfo> GetAsync() => await this.TryGetAsync().ConfigureAwait(false) ?? throw new MissingCredentialsException("Connection info does not exist.");

    public async Task SetAsync(HealthCloudConnectionInfo connectionInfo)
    {
      try
      {
        byte[] encryptedBytes = await this.cryptographyService.ProtectAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject((object) connectionInfo))).ConfigureAwait(false);
        using (Stream stream = await (await (await this.GetDataFolderAsync().ConfigureAwait(false)).CreateFileAsync(this.DataFileName, CreationCollisionOption.ReplaceExisting).ConfigureAwait(false)).OpenAsync(FileAccess.ReadAndWrite).ConfigureAwait(false))
          stream.WriteAsync(encryptedBytes, 0, encryptedBytes.Length).Wait();
        if (this.Caching)
          this.credentialCache[this.CacheKey] = connectionInfo;
        encryptedBytes = (byte[]) null;
      }
      catch (Exception ex)
      {
        ConnectionInfoStore.Logger.Error(ex, "Unexpected exception calling SetCredentials.");
        throw;
      }
    }

    public async Task ClearAsync()
    {
      try
      {
        if (this.Caching)
          this.credentialCache.Remove(this.CacheKey);
        (await (await this.GetDataFolderAsync().ConfigureAwait(false)).TryGetFileAsync(this.DataFileName).ConfigureAwait(false))?.DeleteAsync().Wait();
      }
      catch (Exception ex)
      {
        ConnectionInfoStore.Logger.Error(ex, "Unexpected exception calling ClearCredentials.");
        throw;
      }
    }

    private async Task<HealthCloudConnectionInfo> TryGetInternalAsync()
    {
      string cacheKey = this.CacheKey;
      HealthCloudConnectionInfo cloudConnectionInfo1;
      if (this.Caching && this.credentialCache.TryGetValue(cacheKey, out cloudConnectionInfo1))
        return cloudConnectionInfo1;
      IFile file = await (await this.GetDataFolderAsync().ConfigureAwait(false)).TryGetFileAsync(this.DataFileName).ConfigureAwait(false);
      if (file == null)
        return (HealthCloudConnectionInfo) null;
      byte[] bytes = await this.cryptographyService.UnprotectAsync(await file.ReadAllBytesAsync().ConfigureAwait(false)).ConfigureAwait(false);
      HealthCloudConnectionInfo cloudConnectionInfo2 = JsonConvert.DeserializeObject<HealthCloudConnectionInfo>(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
      if (this.Caching)
        this.credentialCache[cacheKey] = cloudConnectionInfo2;
      return cloudConnectionInfo2;
    }

    private Task<IFolder> GetDataFolderAsync() => this.fileSystemService.GetConnectionInfoFolderAsync();

    private string CacheKey => this.config.AuthBaseUrl;
  }
}
