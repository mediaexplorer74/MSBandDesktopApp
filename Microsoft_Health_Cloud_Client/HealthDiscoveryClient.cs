// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.HealthDiscoveryClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Authentication;
using Microsoft.Health.Cloud.Client.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client
{
  public class HealthDiscoveryClient : HealthCloudClientBase, IHealthDiscoveryClient
  {
    public HealthDiscoveryClient(
      HttpMessageHandler messageHandler,
      Uri baseUrl,
      IHttpCacheService cacheService = null)
      : base(messageHandler, (Func<CancellationToken, Task<Uri>>) (_ => Task.FromResult<Uri>(baseUrl)), cacheService)
    {
    }

    public HealthDiscoveryClient(
      HttpMessageHandler messageHandler,
      Func<CancellationToken, Task<Uri>> baseUriSelector,
      IHttpCacheService cacheService = null)
      : base(messageHandler, baseUriSelector, cacheService)
    {
    }

    public async Task<KdsResponse> AuthenticateAsync(
      CancellationToken cancellationToken)
    {
      HttpRequestMessage request = await this.CreateHttpRequestAsync(HttpMethod.Get, await this.CreateUrlAsync("api/v1/user", cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
      this.Trace.HttpRequest(request, true);
      bool userExists = true;
      HttpResponseMessage response = (HttpResponseMessage) null;
      try
      {
        response = await this.SendAsync(request, cancellationToken).ConfigureAwait(false);
        this.Trace.HttpResponse(response, true);
      }
      catch (NotFoundException ex)
      {
        userExists = false;
      }
      if (!userExists)
        response = await this.CreateUserAsync(cancellationToken).ConfigureAwait(false);
      IEnumerable<string> values;
      response.Headers.TryGetValues("Authorization", out values);
      string podSecurityToken = values != null ? AuthUtilities.UnwrapAccessToken(values.First<string>()) : throw new HealthCloudException("Response did not contain expected Authorization header");
      UserInfo userInfo = await response.ReadJsonAsync<UserInfo>().ConfigureAwait(false);
      return new KdsResponse()
      {
        UserInfo = userInfo,
        PodAccessToken = podSecurityToken
      };
    }

    public async Task<UserInfo> GetUserAsync(CancellationToken cancellationToken) => (await this.AuthenticateAsync(cancellationToken).ConfigureAwait(false)).UserInfo;

    public Task<MsaUserProfile> GetMsaUserProfileAsync(
      CancellationToken cancellationToken)
    {
      return this.GetJsonAsync<MsaUserProfile>("api/v1/user/live", cancellationToken);
    }

    public Task<HttpResponseMessage> CreateUserAsync(
      CancellationToken cancellationToken)
    {
      return this.PostJsonAsync<Dictionary<string, string>>("api/v1/user", (NameValueCollection) null, new Dictionary<string, string>(), cancellationToken);
    }
  }
}
