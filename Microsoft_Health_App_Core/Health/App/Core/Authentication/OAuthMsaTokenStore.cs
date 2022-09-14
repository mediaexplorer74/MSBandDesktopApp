// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Authentication.OAuthMsaTokenStore
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Services.Storage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Authentication
{
  public class OAuthMsaTokenStore : IOAuthMsaTokenStore
  {
    public const string FileNamePrefix = "OAuthMsaToken";
    private const string FileNameFormat = "{0}_{1}.json";
    private readonly IConfig config;
    private readonly IFileObjectStorageService isoObjectStore;
    private readonly Dictionary<string, StoredMsaToken> tokenCache = new Dictionary<string, StoredMsaToken>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    public OAuthMsaTokenStore(IConfig config, IFileObjectStorageService isoObjectStore)
    {
      this.config = config;
      this.isoObjectStore = isoObjectStore;
    }

    private string IsoFileName => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}_{1}.json", new object[2]
    {
      (object) "OAuthMsaToken",
      (object) new Uri(this.CacheKey).Host
    });

    public async Task<StoredMsaToken> GetAsync()
    {
      string cacheKey = this.CacheKey;
      StoredMsaToken storedMsaToken;
      if (this.tokenCache.TryGetValue(cacheKey, out storedMsaToken))
        return storedMsaToken;
      storedMsaToken = await this.isoObjectStore.ReadObjectAsync<StoredMsaToken>(this.IsoFileName, CancellationToken.None, true).ConfigureAwait(false);
      this.tokenCache[cacheKey] = storedMsaToken;
      return storedMsaToken;
    }

    public async Task SetAsync(StoredMsaToken msaToken)
    {
      await this.isoObjectStore.WriteObjectAsync((object) msaToken, this.IsoFileName, CancellationToken.None, true).ConfigureAwait(false);
      this.tokenCache[this.CacheKey] = msaToken;
    }

    public Task ClearAsync()
    {
      if (this.tokenCache != null)
        this.tokenCache.Clear();
      return this.isoObjectStore.DeleteObjectAsync(this.IsoFileName, CancellationToken.None);
    }

    private string CacheKey => this.config.AuthBaseUrl;
  }
}
