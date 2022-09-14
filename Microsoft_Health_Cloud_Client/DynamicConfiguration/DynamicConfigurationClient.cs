// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.DynamicConfiguration.DynamicConfigurationClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Http;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.DynamicConfiguration
{
  public sealed class DynamicConfigurationClient : HealthCloudClientBase, IDynamicConfigurationClient
  {
    public DynamicConfigurationClient(
      HttpMessageHandler messageHandler,
      Func<CancellationToken, Task<Uri>> baseUriSelector)
      : base(messageHandler, baseUriSelector)
    {
    }

    public async Task<DynamicConfigurationFileMetadata> GetConfigurationFileMetadataAsync(
      RegionInfo region,
      CancellationToken token)
    {
      if (region == null)
        throw new ArgumentNullException(nameof (region));
      HttpRequestMessage request = await this.CreateHttpRequestAsync(HttpMethod.Get, await this.CreateUrlAsync("api/AppsConfig", token).ConfigureAwait(false), token).ConfigureAwait(false);
      request.Headers.Add("Region", region.TwoLetterISORegionName);
      HttpResponseMessage response = await this.SendAsync(request, token).ConfigureAwait(false);
      return response.StatusCode == HttpStatusCode.NoContent ? (DynamicConfigurationFileMetadata) null : await response.ReadJsonAsync<DynamicConfigurationFileMetadata>().ConfigureAwait(false);
    }
  }
}
