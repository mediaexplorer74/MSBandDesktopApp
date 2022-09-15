// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Http.Clients.LiveLogin.LiveLoginClient
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client.Http;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Http.Clients.LiveLogin
{
  public class LiveLoginClient : ILiveLoginClient
  {
    private const string LiveLoginUrl = "https://login.live.com/oauth20_token.srf";
    private const string TokenRefreshBodyFormat = "grant_type=refresh_token&client_id={0}&scope=service::{1}::MBI_SSL&refresh_token={2}";
    private readonly HttpClient httpClient;

    public LiveLoginClient(HttpMessageHandler messageHandler) => this.httpClient = new HttpClient(messageHandler);

    public async Task<TokenRefreshResponse> RefreshTokenAsync(
      string realm,
      string refreshToken,
      CancellationToken cancellationToken)
    {
      HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://login.live.com/oauth20_token.srf");
      string content = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "grant_type=refresh_token&client_id={0}&scope=service::{1}::MBI_SSL&refresh_token={2}", new object[3]
      {
        (object) "000000004811DB42",
        (object) realm,
        (object) refreshToken
      });
      request.Content = (HttpContent) new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
      return await (await this.httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false)).ReadJsonAsync<TokenRefreshResponse>();
    }
  }
}
