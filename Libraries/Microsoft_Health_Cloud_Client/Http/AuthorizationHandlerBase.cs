// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.AuthorizationHandlerBase
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Http
{
  public abstract class AuthorizationHandlerBase : DelegatingHandler
  {
    private readonly bool allowUI;

    protected AuthorizationHandlerBase(HttpMessageHandler innerHandler, bool allowUI)
      : base(innerHandler)
    {
      this.allowUI = allowUI;
    }

    protected override sealed async Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      if (request.RequestUri.Scheme == "https")
      {
        string str = await this.GetHeaderValueAsync(this.allowUI, cancellationToken).ConfigureAwait(false);
        request.Headers.TryAddWithoutValidation("Authorization", str);
      }
      return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    protected abstract Task<string> GetHeaderValueAsync(
      bool allowUI,
      CancellationToken cancellationToken);
  }
}
